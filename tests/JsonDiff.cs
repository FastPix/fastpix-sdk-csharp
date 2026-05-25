using Newtonsoft.Json.Linq;

namespace Fastpix.Tests;

/// <summary>
/// JSON path collection, key canonicalization, and the deliberate event-field
/// remap — ported 1:1 from fastpix-php/Tests/validate-get-endpoints.ts so the
/// API-vs-SDK comparison behaves identically across language SDKs.
/// </summary>
internal static class JsonDiff
{
    // Mirrors src/Fastpix/Hooks/EventsFieldRemapHook.cs: get_video_view_details
    // returns abbreviated wire keys on data.events[] that the SDK intentionally
    // remaps to spec-shaped long names. Apply the same remap to the raw API body
    // so the comparison reflects what the SDK is contracted to emit.
    private static readonly Dictionary<string, string> EventOuterRemap = new()
    {
        ["pt"] = "player_playhead_time",
        ["e"] = "event_name",
        ["d"] = "event_details",
        ["vt"] = "viewer_time",
        ["et"] = "event_time",
    };

    private static readonly Dictionary<string, string> EventInnerRemap = new()
    {
        ["br"] = "bitrate",
        ["h"] = "height",
        ["w"] = "width",
        ["cd"] = "codec",
        ["host"] = "hostName",
        ["txt"] = "text",
        ["c"] = "code",
        ["err"] = "error",
        ["t"] = "type",
        ["u"] = "url",
    };

    public static JToken? RemapApiForComparison(string operationId, JToken? body)
    {
        if (operationId != "get_video_view_details" || body is not JObject root)
        {
            return body;
        }

        if (root["data"] is not JObject data || data["events"] is not JArray events)
        {
            return body;
        }

        var rebuiltEvents = new JArray();
        foreach (var ev in events)
        {
            if (ev is not JObject evObj)
            {
                rebuiltEvents.Add(ev);
                continue;
            }

            var rebuilt = new JObject();
            foreach (var prop in evObj.Properties())
            {
                var newKey = EventOuterRemap.TryGetValue(prop.Name, out var mapped) ? mapped : prop.Name;
                if (newKey == "event_details" && prop.Value is JObject inner)
                {
                    var rebuiltInner = new JObject();
                    foreach (var ip in inner.Properties())
                    {
                        var ik = EventInnerRemap.TryGetValue(ip.Name, out var im) ? im : ip.Name;
                        rebuiltInner[ik] = ip.Value;
                    }
                    rebuilt[newKey] = rebuiltInner;
                }
                else
                {
                    rebuilt[newKey] = prop.Value;
                }
            }
            rebuiltEvents.Add(rebuilt);
        }

        var clone = (JObject)root.DeepClone();
        ((JObject)clone["data"]!)["events"] = rebuiltEvents;
        return clone;
    }

    public static string CanonicalizeKey(string key)
    {
        // 1) snake_case -> camelCase
        string camel = key;
        if (key.Contains('_'))
        {
            var lowered = key.ToLowerInvariant();
            var sb = new System.Text.StringBuilder(lowered.Length);
            for (int i = 0; i < lowered.Length; i++)
            {
                char c = lowered[i];
                if (c == '_' && i + 1 < lowered.Length)
                {
                    char next = lowered[i + 1];
                    if (char.IsLetterOrDigit(next))
                    {
                        sb.Append(char.ToUpperInvariant(next));
                        i++;
                        continue;
                    }
                }
                sb.Append(c);
            }
            camel = sb.ToString();
        }

        // 2) normalize acronym casing
        return camel.Replace("SDK", "Sdk").Replace("API", "Api");
    }

    public static JToken? NormalizeJsonForComparison(JToken? value)
    {
        switch (value)
        {
            case null:
                return null;
            case JArray arr:
            {
                var outArr = new JArray();
                foreach (var item in arr) outArr.Add(NormalizeJsonForComparison(item)!);
                return outArr;
            }
            case JObject obj:
            {
                var outObj = new JObject();
                foreach (var prop in obj.Properties())
                {
                    outObj[CanonicalizeKey(prop.Name)] = NormalizeJsonForComparison(prop.Value);
                }
                return outObj;
            }
            default:
                return value;
        }
    }

    /// <summary>
    /// Collect the set of JSON paths present in <paramref name="value"/>.
    /// When <paramref name="includeEmptyArrays"/> is false, empty arrays, empty
    /// objects, and null leaves are treated as "missing" (symmetric API/SDK rule).
    /// </summary>
    public static HashSet<string> CollectJsonPaths(JToken? value, string prefix = "", bool includeEmptyArrays = true)
    {
        var outSet = new HashSet<string>();
        if (value == null || value.Type == JTokenType.Null) return outSet;

        if (value is JArray arr)
        {
            if (!includeEmptyArrays && arr.Count == 0) return outSet;
            var arrayPrefix = prefix.Length > 0 ? prefix + "[]" : "[]";
            outSet.Add(arrayPrefix);
            foreach (var item in arr)
            {
                foreach (var p in CollectJsonPaths(item, arrayPrefix, includeEmptyArrays)) outSet.Add(p);
            }
            return outSet;
        }

        if (value is JObject obj)
        {
            foreach (var prop in obj.Properties())
            {
                var v = prop.Value;
                if (!includeEmptyArrays && v is JArray a && a.Count == 0) continue;
                if (!includeEmptyArrays && (v == null || v.Type == JTokenType.Null)) continue;
                if (!includeEmptyArrays && v is JObject o && o.Count == 0) continue;

                var p = prefix.Length > 0 ? $"{prefix}.{prop.Name}" : prop.Name;
                outSet.Add(p);
                foreach (var child in CollectJsonPaths(v, p, includeEmptyArrays)) outSet.Add(child);
            }
            return outSet;
        }

        // scalar leaf
        if (prefix.Length > 0) outSet.Add(prefix);
        return outSet;
    }

    public static HashSet<string> CollectEmptyArrayFieldPaths(JToken? value, string prefix = "")
    {
        var outSet = new HashSet<string>();
        if (value is not JObject and not JArray) return outSet;

        if (value is JArray arr)
        {
            var arrayPrefix = prefix.Length > 0 ? prefix + "[]" : "[]";
            foreach (var item in arr)
            {
                foreach (var p in CollectEmptyArrayFieldPaths(item, arrayPrefix)) outSet.Add(p);
            }
            return outSet;
        }

        var obj = (JObject)value;
        foreach (var prop in obj.Properties())
        {
            var p = prefix.Length > 0 ? $"{prefix}.{prop.Name}" : prop.Name;
            if (prop.Value is JArray a && a.Count == 0) outSet.Add(p);
            foreach (var child in CollectEmptyArrayFieldPaths(prop.Value, p)) outSet.Add(child);
        }
        return outSet;
    }

    public static List<string> SortUnique(IEnumerable<string> items)
    {
        return items.Distinct().OrderBy(s => s, StringComparer.Ordinal).ToList();
    }
}
