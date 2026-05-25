using Newtonsoft.Json.Linq;
using NJsonSchema;
using YamlDotNet.Serialization;

namespace Fastpix.Tests;

internal sealed class EndpointInfo
{
    public required string Path { get; init; }
    public required string OperationId { get; init; }
    public required JObject Responses { get; init; }
    public List<JObject> Parameters { get; init; } = new();
    public string Method { get; init; } = "GET";
}

/// <summary>
/// Loads the OpenAPI spec, extracts GET endpoints, and builds per-status JSON
/// Schema validators using NJsonSchema. Mirrors the spec handling in
/// fastpix-php/Tests/validate-get-endpoints.ts (refs -> #/definitions, plus an
/// OpenAPI `nullable` -> JSON-Schema type-union conversion).
/// </summary>
internal static class OpenApiSpec
{
    public static JObject Load(string specPath)
    {
        var yamlText = File.ReadAllText(specPath);
        var yamlObject = new DeserializerBuilder().Build().Deserialize<object?>(yamlText);
        var json = new SerializerBuilder().JsonCompatible().Build().Serialize(yamlObject);
        return JObject.Parse(json);
    }

    public static List<EndpointInfo> ExtractGetEndpoints(JObject spec)
    {
        var result = new List<EndpointInfo>();
        if (spec["paths"] is not JObject paths) return result;

        foreach (var pathProp in paths.Properties())
        {
            if (pathProp.Value is not JObject methods) continue;
            if (methods["get"] is not JObject get) continue;

            var parameters = new List<JObject>();
            if (get["parameters"] is JArray opParams)
                parameters.AddRange(opParams.OfType<JObject>());
            if (methods["parameters"] is JArray pathParams)
                parameters.AddRange(pathParams.OfType<JObject>());

            result.Add(new EndpointInfo
            {
                Path = pathProp.Name,
                OperationId = get["operationId"]?.ToString() ?? pathProp.Name,
                Responses = get["responses"] as JObject ?? new JObject(),
                Parameters = parameters,
                Method = "GET",
            });
        }

        return result;
    }

    /// <summary>operationId -> endpoint for POST/PUT/PATCH/DELETE operations.</summary>
    public static Dictionary<string, EndpointInfo> ExtractNonGetEndpoints(JObject spec)
    {
        var result = new Dictionary<string, EndpointInfo>();
        if (spec["paths"] is not JObject paths) return result;

        foreach (var pathProp in paths.Properties())
        {
            if (pathProp.Value is not JObject methods) continue;
            foreach (var method in new[] { "post", "put", "patch", "delete" })
            {
                if (methods[method] is not JObject op) continue;
                var opId = op["operationId"]?.ToString();
                if (opId == null) continue;
                result[opId] = new EndpointInfo
                {
                    Path = pathProp.Name,
                    OperationId = opId,
                    Responses = op["responses"] as JObject ?? new JObject(),
                    Method = method.ToUpperInvariant(),
                };
            }
        }

        return result;
    }

    /// <summary>
    /// Build a status -> compiled JSON Schema map for one endpoint's responses.
    /// Returns null when no response defines an application/json schema.
    /// </summary>
    public static async Task<Dictionary<string, JsonSchema>?> BuildValidatorsAsync(JObject spec, EndpointInfo endpoint)
    {
        var definitions = spec["components"]?["schemas"] as JObject ?? new JObject();
        var transformedDefinitions = (JObject)TransformSchemaNode(definitions.DeepClone());

        var validators = new Dictionary<string, JsonSchema>();

        foreach (var statusProp in endpoint.Responses.Properties())
        {
            var schema = statusProp.Value?["content"]?["application/json"]?["schema"];
            if (schema == null) continue;

            var transformed = TransformSchemaNode(schema.DeepClone());
            var composed = transformed is JObject schemaObj ? (JObject)schemaObj.DeepClone() : new JObject { ["allOf"] = new JArray { transformed } };
            composed["definitions"] = transformedDefinitions;

            try
            {
                var compiled = await JsonSchema.FromJsonAsync(composed.ToString());
                validators[statusProp.Name] = compiled;
            }
            catch
            {
                // A schema NJsonSchema can't compile is treated as "no validator"
                // for that status rather than crashing the whole run.
            }
        }

        return validators.Count > 0 ? validators : null;
    }

    public static List<ValidationFinding> Validate(JsonSchema schema, JToken? body)
    {
        var token = body ?? JValue.CreateNull();
        var errors = schema.Validate(token);
        return errors.Select(e => new ValidationFinding
        {
            Path = string.IsNullOrEmpty(e.Path) ? null : e.Path,
            Message = $"{e.Kind}{(string.IsNullOrEmpty(e.Property) ? "" : $" ({e.Property})")}",
        }).ToList();
    }

    /// <summary>
    /// Recursively: rewrite OpenAPI component refs to JSON-Schema definition refs,
    /// and convert `nullable: true` into a JSON-Schema-compatible nullable type.
    /// </summary>
    private static JToken TransformSchemaNode(JToken node)
    {
        switch (node)
        {
            case JArray arr:
            {
                var outArr = new JArray();
                foreach (var item in arr) outArr.Add(TransformSchemaNode(item));
                return outArr;
            }
            case JObject obj:
            {
                var outObj = new JObject();
                bool nullable = obj["nullable"]?.Type == JTokenType.Boolean && obj["nullable"]!.Value<bool>();

                foreach (var prop in obj.Properties())
                {
                    if (prop.Name == "nullable") continue; // not a JSON Schema keyword

                    if (prop.Name == "$ref" && prop.Value.Type == JTokenType.String)
                    {
                        outObj["$ref"] = prop.Value.ToString().Replace("#/components/schemas/", "#/definitions/");
                        continue;
                    }

                    outObj[prop.Name] = TransformSchemaNode(prop.Value);
                }

                if (nullable)
                {
                    ApplyNullable(outObj);
                }

                return outObj;
            }
            default:
                return node;
        }
    }

    private static void ApplyNullable(JObject schema)
    {
        // type: "string" -> type: ["string","null"]
        if (schema["type"]?.Type == JTokenType.String)
        {
            var t = schema["type"]!.ToString();
            schema["type"] = new JArray { t, "null" };
            return;
        }

        // oneOf/anyOf -> add a null branch
        foreach (var key in new[] { "oneOf", "anyOf" })
        {
            if (schema[key] is JArray branches)
            {
                branches.Add(new JObject { ["type"] = "null" });
                return;
            }
        }

        // $ref-only nullable -> wrap so null is also accepted
        if (schema["$ref"] != null)
        {
            var refToken = schema["$ref"]!;
            schema.Remove("$ref");
            schema["oneOf"] = new JArray
            {
                new JObject { ["$ref"] = refToken },
                new JObject { ["type"] = "null" },
            };
        }
    }
}

internal sealed class ValidationFinding
{
    public string? Path { get; init; }
    public string? Message { get; init; }
}
