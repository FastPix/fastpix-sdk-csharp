using System.Text;
using Newtonsoft.Json.Linq;

namespace Fastpix.Tests;

/// <summary>
/// Fixture loading, per-operation default requests, and live-API URL building.
/// Ports defaultSDKRequest / buildSDKRequest / buildUrl from
/// fastpix-php/Tests/validate-get-endpoints.ts.
/// </summary>
internal static class Fixtures
{
    public const string PlaceholderUuid = "00000000-0000-0000-0000-000000000000";

    public static JObject? Read(string? path)
    {
        if (path == null || !File.Exists(path)) return null;
        return JObject.Parse(File.ReadAllText(path));
    }

    /// <summary>Operation-specific defaults that satisfy required params so the call reaches the HTTP layer.</summary>
    public static Dictionary<string, object?>? DefaultSdkRequest(string operationId) => operationId switch
    {
        "get-media" or "get-media-summary" or "retrieveMediaInputInfo" or "list-playback-ids" or "get-media-clips"
            => new() { ["mediaId"] = PlaceholderUuid },
        "get-playback-id"
            => new() { ["mediaId"] = PlaceholderUuid, ["playbackId"] = PlaceholderUuid },
        "list-live-clips"
            => new() { ["livestreamId"] = PlaceholderUuid },
        "get-playlist-by-id"
            => new() { ["playlistId"] = PlaceholderUuid },
        "getDrmConfigurationById"
            => new() { ["drmConfigurationId"] = PlaceholderUuid },
        "get-live-stream-by-id" or "get-live-stream-viewer-count-by-id"
            => new() { ["streamId"] = PlaceholderUuid },
        "get-live-stream-playback-id"
            => new() { ["streamId"] = PlaceholderUuid, ["playbackId"] = PlaceholderUuid },
        "get-specific-simulcast-of-stream"
            => new() { ["streamId"] = PlaceholderUuid, ["simulcastId"] = PlaceholderUuid },
        "get-signing_key_by_id"
            => new() { ["signingKeyId"] = PlaceholderUuid },
        "get_video_view_details"
            => new() { ["viewId"] = PlaceholderUuid },
        "list_filter_values_for_dimension"
            => new() { ["dimensionsId"] = "browser_name" },
        "list_breakdown_values"
            => new() { ["metricId"] = "quality_of_experience_score", ["timespan"] = "24:hours", ["groupBy"] = "browser_name" },
        "list_overall_values"
            => new() { ["metricId"] = "quality_of_experience_score", ["timespan"] = "24:hours" },
        "get_timeseries_data"
            => new() { ["metricId"] = "quality_of_experience_score", ["timespan"] = "24:hours", ["groupBy"] = "hour" },
        "list_comparison_values"
            => new() { ["timespan"] = "24:hours", ["dimension"] = "browser_name", ["value"] = "Chrome" },
        "list_errors"
            => new() { ["timespan"] = "24:hours", ["limit"] = 5 },
        "list_video_views"
            => new() { ["timespan"] = "24:hours", ["limit"] = 5, ["offset"] = 1 },
        "list_by_top_content"
            => new() { ["timespan"] = "24:hours", ["limit"] = 5 },
        "list-media" or "list-uploads" or "get-all-streams"
            => new() { ["limit"] = 5, ["offset"] = 1, ["orderBy"] = "desc" },
        "getDrmConfiguration"
            => new() { ["limit"] = 10, ["offset"] = 1 },
        "get-all-playlists" or "list_signing_keys"
            => new() { ["limit"] = 5, ["offset"] = 1 },
        _ => null,
    };

    public static Dictionary<string, object?> BuildEffectiveRequest(EndpointInfo endpoint, JObject? fixtures)
    {
        var effective = new Dictionary<string, object?>();

        foreach (var (k, v) in DefaultSdkRequest(endpoint.OperationId) ?? new())
            effective[k] = v;

        var opFixture = fixtures?["operations"]?[endpoint.OperationId];
        if (opFixture is JObject of)
        {
            if (of["pathParams"] is JObject pp)
                foreach (var p in pp.Properties()) effective[p.Name] = ToClr(p.Value);
            if (of["query"] is JObject q)
                foreach (var p in q.Properties()) effective[p.Name] = ToClr(p.Value);
        }

        return effective;
    }

    public static (string Url, string? Note) BuildUrl(string baseUrl, EndpointInfo endpoint, JObject? fixtures)
    {
        var effective = BuildEffectiveRequest(endpoint, fixtures);
        var path = endpoint.Path;
        string? note = null;

        var requiredPathParams = endpoint.Parameters
            .Where(p => p["in"]?.ToString() == "path" && p["required"]?.Value<bool>() == true)
            .Select(p => p["name"]!.ToString())
            .ToList();

        foreach (var name in requiredPathParams)
        {
            var hasVal = effective.TryGetValue(name, out var val) && val != null;
            var resolved = hasVal ? val!.ToString()! : PlaceholderUuid;
            if (!hasVal)
                note = note == null ? $"Placeholder used for {name}" : $"{note}; placeholder used for {name}";
            path = path.Replace($"{{{name}}}", Uri.EscapeDataString(resolved));
        }

        var basePart = baseUrl.EndsWith('/') ? baseUrl : baseUrl + "/";
        var builder = new UriBuilder(new Uri(new Uri(basePart), path.TrimStart('/')));

        var query = new List<string>();
        foreach (var p in endpoint.Parameters.Where(p => p["in"]?.ToString() == "query"))
        {
            var name = p["name"]!.ToString();
            var baseName = name.EndsWith("[]") ? name[..^2] : name;
            if (!effective.TryGetValue(name, out var val) || val == null)
                effective.TryGetValue(baseName, out val);
            if (val == null) continue;

            if (val is IEnumerable<object> list && val is not string)
            {
                foreach (var item in list)
                    query.Add($"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(item.ToString() ?? "")}");
            }
            else
            {
                query.Add($"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(val.ToString() ?? "")}");
            }
        }

        builder.Query = string.Join("&", query);
        return (builder.Uri.ToString(), note);
    }

    public static string BasicAuthHeader(string username, string password)
    {
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        return $"Basic {token}";
    }

    private static object? ToClr(JToken token) => token.Type switch
    {
        JTokenType.Integer => token.Value<long>(),
        JTokenType.Float => token.Value<double>(),
        JTokenType.Boolean => token.Value<bool>(),
        JTokenType.Array => token.Select(ToClr).ToList(),
        _ => token.ToString(),
    };
}
