using System.Net.Http.Headers;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fastpix.Tests;

internal sealed class EndpointResult
{
    public required string Endpoint { get; set; }
    public required string OperationId { get; set; }
    public bool OpenapiValid { get; set; } = true;
    public List<ValidationFinding> OpenapiErrors { get; set; } = new();
    public bool SdkParseOk { get; set; } = true;
    public string? SdkParseError { get; set; }
    public List<string> MissingInSdk { get; set; } = new();
    public List<string> MissingInApi { get; set; } = new();
    public List<string> EmptyArraysOmittedInSdk { get; set; } = new();
    public List<string> EmptyArraysOmittedInApi { get; set; } = new();
    public string? ApiResponseFile { get; set; }
    public string? SdkResponseFile { get; set; }
    public string ApiResponsePreview { get; set; } = "";
    public string SdkResponsePreview { get; set; } = "";
    public string Status { get; set; } = "FAIL";
    public string? Note { get; set; }
    public List<FixSuggestion> FixSuggestions { get; set; } = new();
}

internal sealed class FixSuggestion
{
    public required string Title { get; init; }
    public required string Why { get; init; }
    public string? Where { get; init; }
    public string? PasteYaml { get; init; }
}

/// <summary>
/// Drives the GET-endpoint validation run and writes the markdown reports.
/// In-process C# port of fastpix-php/Tests/validate-get-endpoints.ts.
/// </summary>
internal static class EndpointValidator
{
    private const string ArtifactsDirName = "artifacts";
    private const int MaxPreviewChars = 4000;
    private const string ReportFile = "GET_ENDPOINTS_OPENAPI_RESPONSE_VALIDATION_REPORT.md";
    private const string FixSuggestionsFile = "GET_ENDPOINTS_OPENAPI_RESPONSE_FIX_SUGGESTIONS.md";

    public static async Task<int> RunAsync()
    {
        var testsDir = ResolveProjectDir();
        var specPath = ResolveSpecPath(testsDir);
        var fixturesPath = Path.Combine(testsDir, "get-endpoints-fixtures.json");

        Console.WriteLine($"Spec:     {specPath}");
        Console.WriteLine($"Fixtures: {(File.Exists(fixturesPath) ? fixturesPath : "(none)")}");

        var spec = OpenApiSpec.Load(specPath);
        var endpoints = OpenApiSpec.ExtractGetEndpoints(spec);
        var fixtures = Fixtures.Read(fixturesPath);

        var baseUrl = Environment.GetEnvironmentVariable("FASTPIX_BASE_URL")
            ?? spec["servers"]?[0]?["url"]?.ToString()
            ?? "https://api.fastpix.com/v1/";

        var username = Environment.GetEnvironmentVariable("FASTPIX_USERNAME") ?? "your-access-token";
        var password = Environment.GetEnvironmentVariable("FASTPIX_PASSWORD") ?? "your-secret-key";

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)
            || username == "your-access-token" || password == "your-secret-key")
        {
            Console.Error.WriteLine("Set FASTPIX_USERNAME and FASTPIX_PASSWORD env vars for BasicAuth (use real credentials for live API validation).");
            return 2;
        }

        var sdk = new FastpixSDK(security: new Security { Username = username, Password = password }, serverUrl: baseUrl);

        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        http.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(Fixtures.BasicAuthHeader(username, password));
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var results = new List<EndpointResult>();
        for (int i = 0; i < endpoints.Count; i++)
        {
            var ep = endpoints[i];
            Console.WriteLine($"[{i + 1}/{endpoints.Count}] Processing: {ep.OperationId} ({ep.Path})");

            var result = new EndpointResult { Endpoint = ep.Path, OperationId = ep.OperationId };
            try
            {
                var (url, note) = Fixtures.BuildUrl(baseUrl, ep, fixtures);
                result.Note = note;

                // 1) Live API: fetch raw JSON.
                int httpStatus = 0;
                JToken? rawBody = null;
                string? requestError = null;
                try
                {
                    var resp = await http.GetAsync(url);
                    httpStatus = (int)resp.StatusCode;
                    var bodyText = await resp.Content.ReadAsStringAsync();
                    rawBody = TryParseJson(bodyText);
                }
                catch (Exception e)
                {
                    requestError = e is TaskCanceledException ? "Request timeout (30s)" : e.Message;
                    Console.Error.WriteLine($"  ⚠️  API request failed: {requestError}");
                }

                // 2) Validate raw response against the OpenAPI response schema.
                var validators = await OpenApiSpec.BuildValidatorsAsync(spec, ep);
                if (requestError != null)
                {
                    result.OpenapiValid = false;
                    result.OpenapiErrors = new() { new ValidationFinding { Message = $"Request failed: {requestError}" } };
                }
                else if (validators != null)
                {
                    var schema = validators.TryGetValue(httpStatus.ToString(), out var s) ? s
                        : validators.TryGetValue("default", out var d) ? d : null;
                    if (schema != null)
                    {
                        var findings = OpenApiSpec.Validate(schema, rawBody);
                        if (findings.Count > 0)
                        {
                            result.OpenapiValid = false;
                            result.OpenapiErrors = findings;
                        }
                    }
                    else
                    {
                        result.Note = Append(result.Note, $"No response schema for status {httpStatus}");
                    }
                }

                // 3) Call the SDK in-process; capture success object or normalized error.
                var sdkReq = Fixtures.BuildEffectiveRequest(ep, fixtures);
                var sdk1 = await SdkInvoker.InvokeAsync(sdk, ep.OperationId, sdkReq);
                JToken? sdkValueForDiff = null;
                JToken sdkPrinted;
                if (sdk1.Ok)
                {
                    sdkValueForDiff = sdk1.Value;
                    sdkPrinted = sdk1.Value ?? JValue.CreateNull();
                }
                else
                {
                    result.SdkParseOk = false;
                    result.SdkParseError = sdk1.ErrorMessage ?? "SDK call failed";
                    sdkPrinted = JObject.FromObject(sdk1.Error ?? new Dictionary<string, object?>());
                    Console.Error.WriteLine($"  ⚠️  SDK call failed: {result.SdkParseError}");
                }

                // 4) Compare JSON paths (normalized, with deliberate event remap).
                var apiNormalized = JsonDiff.NormalizeJsonForComparison(JsonDiff.RemapApiForComparison(ep.OperationId, rawBody));
                var sdkNormalized = sdkValueForDiff is JObject or JArray
                    ? JsonDiff.NormalizeJsonForComparison(sdkValueForDiff)
                    : null;

                var apiPaths = JsonDiff.CollectJsonPaths(apiNormalized, includeEmptyArrays: false);
                var sdkPaths = sdkNormalized != null
                    ? JsonDiff.CollectJsonPaths(sdkNormalized, includeEmptyArrays: false)
                    : new HashSet<string>();

                result.MissingInSdk = sdkPaths.Count > 0
                    ? JsonDiff.SortUnique(apiPaths.Where(p => !sdkPaths.Contains(p)))
                    : new();
                result.MissingInApi = sdkPaths.Count > 0
                    ? JsonDiff.SortUnique(sdkPaths.Where(p => !apiPaths.Contains(p)))
                    : new();

                var apiStrict = JsonDiff.CollectJsonPaths(apiNormalized, includeEmptyArrays: true);
                var sdkStrict = sdkNormalized != null ? JsonDiff.CollectJsonPaths(sdkNormalized, includeEmptyArrays: true) : new HashSet<string>();
                var apiEmpty = JsonDiff.CollectEmptyArrayFieldPaths(apiNormalized);
                var sdkEmpty = sdkNormalized != null ? JsonDiff.CollectEmptyArrayFieldPaths(sdkNormalized) : new HashSet<string>();

                result.EmptyArraysOmittedInSdk = JsonDiff.SortUnique(apiEmpty.Where(p => !sdkStrict.Contains(p)));
                result.EmptyArraysOmittedInApi = JsonDiff.SortUnique(sdkEmpty.Where(p => !apiStrict.Contains(p)));

                var (apiFile, sdkFile, apiPrev, sdkPrev) = WriteArtifacts(testsDir, ep.OperationId, rawBody, sdkPrinted);
                result.ApiResponseFile = apiFile;
                result.SdkResponseFile = sdkFile;
                result.ApiResponsePreview = apiPrev;
                result.SdkResponsePreview = sdkPrev;

                result.Status = result.OpenapiValid && result.SdkParseOk
                    && result.MissingInSdk.Count == 0 && result.MissingInApi.Count == 0
                    ? "PASS" : "FAIL";

                Console.WriteLine($"  ✓ Completed: {ep.OperationId} - {result.Status}");
            }
            catch (Exception ex)
            {
                result.OpenapiValid = false;
                result.OpenapiErrors = new() { new ValidationFinding { Message = $"Unexpected error: {ex.Message}" } };
                result.SdkParseOk = false;
                result.SdkParseError = ex.Message;
                result.Status = "FAIL";
                result.Note = "Unexpected error during processing";
                Console.Error.WriteLine($"  ✗ Unexpected error processing {ep.OperationId}: {ex.Message}");
            }

            results.Add(result);
        }

        foreach (var r in results.Where(r => r.Status == "FAIL"))
            r.FixSuggestions = GenerateFixSuggestions(r);

        WriteReport(testsDir, results);
        return 0;
    }

    // ---------- reporting ----------

    private static void WriteReport(string testsDir, List<EndpointResult> results)
    {
        var total = results.Count;
        var passed = results.Count(r => r.Status == "PASS");
        var failed = results.Count(r => r.Status == "FAIL");
        var generatedAt = DateTime.UtcNow.ToString("o");

        var lines = new List<string>
        {
            "# GET Endpoints — OpenAPI Response Validation Report", "",
            $"Generated: {generatedAt}", "",
            "## Summary", "",
            $"- **Total GET endpoints**: {total}",
            $"- **PASS**: {passed}",
            $"- **FAIL**: {failed}",
            $"- **SKIP**: 0", "",
            "## Consolidated report", "",
            "| Endpoint | OperationId | OpenAPI valid | SDK parse | Missing in SDK (present in API) | Missing in API (present in SDK) | Empty arrays omitted by SDK | Status |",
            "|---|---|---:|---:|---|---|---|---|",
        };
        foreach (var r in results) lines.Add(ConsolidatedRow(r));

        lines.Add("");
        lines.Add("## Per-endpoint details (full missing parameter lists)");
        lines.Add("");
        foreach (var r in results)
        {
            lines.Add($"### {r.OperationId} (`{r.Endpoint}`)");
            lines.Add("");
            lines.Add($"- **Status**: {r.Status}");
            if (r.Note != null) lines.Add($"- **Note**: {r.Note}");
            lines.Add($"- **OpenAPI valid**: {(r.OpenapiValid ? "yes" : "no")}");
            if (!r.OpenapiValid && r.OpenapiErrors.Count > 0)
            {
                lines.Add("- **OpenAPI errors**:");
                foreach (var e in r.OpenapiErrors)
                    lines.Add($"  - {(e.Path != null ? $"`{e.Path}` " : "")}{e.Message}".TrimEnd());
            }
            lines.Add($"- **SDK parse**: {(r.SdkParseOk ? "ok" : "failed")}");
            if (!r.SdkParseOk && r.SdkParseError != null) lines.Add($"- **SDK parse error**: {r.SdkParseError}");
            if (r.ApiResponseFile != null) lines.Add($"- **API response file**: `{r.ApiResponseFile}`");
            if (r.SdkResponseFile != null) lines.Add($"- **SDK response file**: `{r.SdkResponseFile}`");
            lines.Add("");

            AppendPreview(lines, "API response (preview)", r.ApiResponsePreview);
            AppendPreview(lines, "SDK response (preview)", r.SdkResponsePreview);

            AppendList(lines, $"Missing in SDK (present in API) — {r.MissingInSdk.Count}", r.MissingInSdk);
            AppendList(lines, $"Missing in API (present in SDK) — {r.MissingInApi.Count}", r.MissingInApi);
            AppendList(lines, $"Empty arrays omitted by SDK — {r.EmptyArraysOmittedInSdk.Count}", r.EmptyArraysOmittedInSdk);
            AppendList(lines, $"Empty arrays omitted by API — {r.EmptyArraysOmittedInApi.Count}", r.EmptyArraysOmittedInApi);
        }

        File.WriteAllText(Path.Combine(testsDir, ReportFile), string.Join("\n", lines));
        WriteFixSuggestions(testsDir, results);
        UpdateReadme(testsDir, results, generatedAt, total, passed, failed);

        Console.WriteLine($"Report generated: {Path.Combine(testsDir, ReportFile)}");
        Console.WriteLine($"Summary: total={total} pass={passed} fail={failed} skip=0");
    }

    private static string ConsolidatedRow(EndpointResult r)
    {
        string Cells(List<string> xs) => xs.Count > 0 ? string.Join(", ", xs.Select(p => $"`{p}`")) : "None";
        var status = r.Status == "PASS" ? "✅ PASS" : "❌ FAIL";
        return $"| `{r.Endpoint}` | `{r.OperationId}` | {(r.OpenapiValid ? "✅" : "❌")} | {(r.SdkParseOk ? "✅" : "❌")} | {Cells(r.MissingInSdk)} | {Cells(r.MissingInApi)} | {Cells(r.EmptyArraysOmittedInSdk)} | {status} |";
    }

    private static void WriteFixSuggestions(string testsDir, List<EndpointResult> results)
    {
        var failing = results.Where(r => r.Status == "FAIL").ToList();
        var lines = new List<string>
        {
            "# GET Endpoints — OpenAPI Response Fix Suggestions", "",
            $"Generated: {DateTime.UtcNow:o}", "",
            $"Total failing endpoints: {failing.Count}", "",
        };

        foreach (var r in failing)
        {
            lines.Add($"## {r.OperationId} (`{r.Endpoint}`)");
            lines.Add("");
            lines.Add($"- **Status**: {r.Status}");
            lines.Add($"- **OpenAPI valid**: {(r.OpenapiValid ? "yes" : "no")}");
            lines.Add($"- **SDK parse**: {(r.SdkParseOk ? "ok" : "failed")}");
            lines.Add("");

            if (!r.OpenapiValid && r.OpenapiErrors.Count > 0)
            {
                lines.Add("### Observed OpenAPI errors");
                lines.Add("");
                foreach (var e in r.OpenapiErrors)
                    lines.Add($"- {(e.Path != null ? $"`{e.Path}` " : "")}{e.Message}".TrimEnd());
                lines.Add("");
            }

            lines.Add("### Suggested fixes");
            lines.Add("");
            if (r.FixSuggestions.Count == 0)
            {
                lines.Add("- No heuristic suggestions available for this failure yet.");
                lines.Add("");
                continue;
            }
            foreach (var s in r.FixSuggestions)
            {
                lines.Add($"- **{s.Title}**");
                lines.Add($"  - **why**: {s.Why}");
                if (s.Where != null) lines.Add($"  - **where**: {s.Where}");
                if (s.PasteYaml != null)
                {
                    lines.Add("  - **paste**:");
                    lines.Add("");
                    lines.Add("```yaml");
                    lines.Add(s.PasteYaml);
                    lines.Add("```");
                }
                lines.Add("");
            }
        }

        File.WriteAllText(Path.Combine(testsDir, FixSuggestionsFile), string.Join("\n", lines));
    }

    private static void UpdateReadme(string testsDir, List<EndpointResult> results, string generatedAt, int total, int passed, int failed)
    {
        var readmePath = Path.Combine(testsDir, "README.md");
        if (!File.Exists(readmePath)) return;

        const string begin = "<!-- BEGIN GET_ENDPOINTS_CONSOLIDATED -->";
        const string end = "<!-- END GET_ENDPOINTS_CONSOLIDATED -->";
        var readme = File.ReadAllText(readmePath);
        if (!readme.Contains(begin) || !readme.Contains(end)) return;

        var c = new List<string>
        {
            $"Last generated: {generatedAt}", "",
            $"- **Total GET endpoints**: {total}",
            $"- **PASS**: {passed}",
            $"- **FAIL**: {failed}",
            $"- **SKIP**: 0", "",
            "| Endpoint | OperationId | OpenAPI valid | SDK parse | Missing in SDK (present in API) | Missing in API (present in SDK) | Empty arrays omitted by SDK | Status |",
            "|---|---|---:|---:|---|---|---|---|",
        };
        foreach (var r in results) c.Add(ConsolidatedRow(r));
        c.Add("");
        c.Add($"Full details: `tests/{ReportFile}`");

        var block = $"{begin}\n{string.Join("\n", c)}\n{end}";
        var startIdx = readme.IndexOf(begin, StringComparison.Ordinal);
        var endIdx = readme.IndexOf(end, StringComparison.Ordinal) + end.Length;
        var updated = readme[..startIdx] + block + readme[endIdx..];
        File.WriteAllText(readmePath, updated);
    }

    // ---------- fix-suggestion heuristics (ported) ----------

    private static List<FixSuggestion> GenerateFixSuggestions(EndpointResult r)
    {
        var outList = new List<FixSuggestion>();
        bool HasErr(string s) => r.OpenapiErrors.Any(e => (e.Message ?? "").Contains(s));
        var paths = r.OpenapiErrors.Select(e => e.Path).Where(p => !string.IsNullOrEmpty(p)).Select(p => p!).ToList();

        if (HasErr("OneOf") && paths.Any(p => p.Contains("tracks")) || HasErr("must match exactly one schema in oneOf") && paths.Any(p => p.Contains("tracks")))
        {
            outList.Add(new FixSuggestion
            {
                Title = "Fix `tracks[].oneOf` overlap by constraining `type` per track schema",
                Why = "Track schemas overlap (type is a free string and distinguishing fields aren't required), so one track object can match multiple branches. oneOf requires exactly one match.",
                Where = "In OpenAPI spec: components/schemas/{VideoTrack,VideoTrackForGetAll,AudioTrack,SubtitleTrack}.properties.type",
                PasteYaml = "# VideoTrack\ntype:\n  type: string\n  enum: [video]\n# AudioTrack\ntype:\n  type: string\n  enum: [audio]\n# SubtitleTrack\ntype:\n  type: string\n  enum: [subtitle]",
            });
        }
        if ((HasErr("must be equal to one of the allowed values") || HasErr("Enum")) && paths.Any(p => p.Contains("sourceResolution")))
        {
            outList.Add(new FixSuggestion
            {
                Title = "Fix `sourceResolution` enum mismatch (API may return values without `p`)",
                Why = "The API can return values like \"1080\" but the spec constrains the enum to \"1080p\"-style values.",
                Where = "In OpenAPI spec: under the relevant media response schema(s) sourceResolution field definition",
            });
        }
        if ((HasErr("OneOf") || HasErr("oneOf")) && (r.Endpoint == "/data/dimensions" || paths.Any(p => p.Contains("dimensions"))))
        {
            outList.Add(new FixSuggestion
            {
                Title = "Remove redundant `oneOf` on `/data/dimensions` response schema",
                Why = "data is oneOf: [array<string>, $ref: Dimensions] and Dimensions is itself array<string>, so valid responses match multiple branches.",
                Where = "In OpenAPI spec: paths./data/dimensions.get.responses.200.content.application/json.schema.properties.data.oneOf",
            });
        }
        if ((HasErr("OneOf") || HasErr("oneOf")) && paths.Any(p => p.Contains("value")))
        {
            outList.Add(new FixSuggestion
            {
                Title = "Avoid `oneOf: [integer, number]` overlaps (integers are also numbers)",
                Why = "In JSON Schema, integer is a subset of number. A value like 0 matches both, causing oneOf validation errors.",
                Where = "In OpenAPI spec: metrics schemas that use oneOf: [integer, number]",
            });
        }
        if ((HasErr("StringExpected") || HasErr("must be string")) && paths.Any(p => p.Contains("fpApiVersion")))
        {
            outList.Add(new FixSuggestion
            {
                Title = "Make `fpApiVersion` nullable in the spec",
                Why = "The API can return null for fpApiVersion but the schema declares string only.",
                Where = "In OpenAPI spec: components/schemas/Views.properties.fpApiVersion",
            });
        }
        if ((r.Note ?? "").Contains("Placeholder used") && !r.SdkParseOk && System.Text.RegularExpressions.Regex.IsMatch(r.SdkParseError ?? "", "404|not found", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            outList.Add(new FixSuggestion
            {
                Title = "Provide real fixture IDs for this operationId",
                Why = "A placeholder UUID was used for required path params; the API likely returned 404. Add a real ID under tests/get-endpoints-fixtures.json for this operationId.",
            });
        }
        if (r.MissingInApi.Any(p => p.Contains("playOrder")) || r.MissingInSdk.Any(p => p.Contains("playOrder")))
        {
            outList.Add(new FixSuggestion
            {
                Title = "Ensure `playOrder` is correctly modeled for smart playlists only",
                Why = "If playOrder is present/required only for type: smart, the response schemas should reflect that (e.g. discriminator split).",
                Where = "In OpenAPI spec: playlist response schemas for create/update/get-by-id",
            });
        }
        if (r.MissingInSdk.Any(p => p.Contains("simulcastResponses")))
        {
            outList.Add(new FixSuggestion
            {
                Title = "Add `simulcastResponses` to the live stream response schema",
                Why = "The API response includes simulcastResponses but the OpenAPI schema (and generated SDK inbound schema) does not, causing the SDK to drop the field.",
                Where = "In OpenAPI spec: live stream response schema(s) for get/list streams",
            });
        }

        return outList;
    }

    // ---------- helpers ----------

    private static (string apiPath, string sdkPath, string apiPreview, string sdkPreview) WriteArtifacts(
        string testsDir, string operationId, JToken? rawBody, JToken sdkBody)
    {
        var dir = Path.Combine(testsDir, ArtifactsDirName);
        Directory.CreateDirectory(dir);
        var slug = System.Text.RegularExpressions.Regex.Replace(operationId, "[^a-zA-Z0-9._-]+", "_");

        var apiText = (rawBody ?? JValue.CreateNull()).ToString(Formatting.Indented);
        var sdkText = sdkBody.ToString(Formatting.Indented);

        File.WriteAllText(Path.Combine(dir, $"{slug}.api.json"), apiText);
        File.WriteAllText(Path.Combine(dir, $"{slug}.sdk.json"), sdkText);

        return ($"tests/{ArtifactsDirName}/{slug}.api.json", $"tests/{ArtifactsDirName}/{slug}.sdk.json",
            Preview(apiText), Preview(sdkText));
    }

    private static string Preview(string text) =>
        text.Length <= MaxPreviewChars ? text : text[..MaxPreviewChars] + "\n... (truncated)";

    private static void AppendPreview(List<string> lines, string title, string preview)
    {
        if (string.IsNullOrEmpty(preview)) return;
        lines.Add($"**{title}**");
        lines.Add("");
        lines.Add("```json");
        lines.Add(preview);
        lines.Add("```");
        lines.Add("");
    }

    private static void AppendList(List<string> lines, string title, List<string> items)
    {
        lines.Add($"**{title}**");
        lines.Add("");
        if (items.Count == 0) lines.Add("- None");
        else foreach (var p in items) lines.Add($"- `{p}`");
        lines.Add("");
    }

    private static string Append(string? note, string add) => note == null ? add : $"{note}; {add}";

    private static JToken? TryParseJson(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        try { return JToken.Parse(text); } catch { return new JValue(text); }
    }

    internal static string ResolveProjectDir()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Fastpix.Tests.csproj"))) return dir.FullName;
            dir = dir.Parent;
        }
        return Directory.GetCurrentDirectory();
    }

    internal static string ResolveSpecPath(string startDir)
    {
        var env = Environment.GetEnvironmentVariable("FASTPIX_SPEC");
        if (!string.IsNullOrEmpty(env) && File.Exists(env)) return env;

        var names = new[] { "fixed 7.yaml", "fastpix.yaml", "fixed.yaml", "openapi.yaml", "fastpix-openapi.yaml" };
        var dir = new DirectoryInfo(startDir);
        while (dir != null)
        {
            foreach (var n in names)
            {
                var p = Path.Combine(dir.FullName, n);
                if (File.Exists(p)) return p;
            }
            // also match any *.yaml at this level as a last resort within the dir
            dir = dir.Parent;
        }
        throw new FileNotFoundException("OpenAPI spec not found. Set FASTPIX_SPEC or place the spec (e.g. 'fixed 7.yaml') at the repo root.");
    }
}
