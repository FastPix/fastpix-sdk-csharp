using Fastpix.Models.Errors;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Components = Fastpix.Models.Components;

namespace Fastpix.Tests;

internal sealed class SdkResult
{
    public bool Ok { get; init; }
    public JToken? Value { get; init; }
    public Dictionary<string, object?>? Error { get; init; }
    public string? ErrorMessage { get; init; }

    public static SdkResult Success(JToken? value) => new() { Ok = true, Value = value };

    public static SdkResult Err(string name, string message, int? statusCode = null, string? body = null)
    {
        var err = new Dictionary<string, object?> { ["name"] = name, ["message"] = message };
        if (statusCode.HasValue) err["statusCode"] = statusCode.Value;
        if (body != null)
        {
            err["body"] = body;
            try { err["bodyJson"] = JToken.Parse(body); } catch { /* not JSON */ }
        }
        return new SdkResult { Ok = false, Error = err, ErrorMessage = message };
    }
}

/// <summary>
/// Calls the C# SDK method for a given operationId and returns either the parsed
/// response data (wire-shaped JSON) or a normalized error. In-process equivalent
/// of the PHP-subprocess block in validate-get-endpoints.ts.
/// </summary>
internal static class SdkInvoker
{
    private static readonly HashSet<string> MetadataProps = new()
    {
        "HttpMeta", "StatusCode", "ContentType", "RawResponse", "Headers", "DefaultError",
    };

    public static async Task<SdkResult> InvokeAsync(FastpixSDK sdk, string operationId, Dictionary<string, object?> req)
    {
        string? S(string k) => req.TryGetValue(k, out var v) && v != null ? v.ToString() : null;
        long? L(string k) => req.TryGetValue(k, out var v) && v != null ? Convert.ToInt64(v) : null;

        var orderStr = S("orderBy");
        var sortOrder = orderStr == "asc" ? Components.SortOrder.Asc : Components.SortOrder.Desc;
        var liveOrderBy = orderStr == "asc" ? OrderBy.Asc : OrderBy.Desc;

        try
        {
            object res;
            switch (operationId)
            {
                case "list-media":
                    res = await sdk.ManageVideos.ListAsync(limit: L("limit"), offset: L("offset"), orderBy: sortOrder);
                    break;
                case "get-media":
                    res = await sdk.ManageVideos.GetByIdAsync(mediaId: S("mediaId")!);
                    break;
                case "get-media-summary":
                    res = await sdk.Videos.GetSummaryAsync(mediaId: S("mediaId")!);
                    break;
                case "retrieveMediaInputInfo":
                    res = await sdk.Videos.GetInputInfoAsync(mediaId: S("mediaId")!);
                    break;
                case "list-uploads":
                    res = await sdk.ManageVideos.ListUploadsAsync(limit: L("limit"), offset: L("offset"), orderBy: sortOrder);
                    break;
                case "get-media-clips":
                    res = await sdk.ManageVideos.ListClipsAsync(mediaId: S("mediaId")!, offset: L("offset"), limit: L("limit"), orderBy: sortOrder);
                    break;
                case "list-live-clips":
                    res = await sdk.Videos.ListLiveClipsAsync(livestreamId: S("livestreamId")!, limit: L("limit"), offset: L("offset"), orderBy: sortOrder);
                    break;
                case "get-all-playlists":
                    res = await sdk.Playlists.GetAllAsync(limit: L("limit"), offset: L("offset"));
                    break;
                case "get-playlist-by-id":
                    res = await sdk.Playlists.GetAsync(playlistId: S("playlistId")!);
                    break;
                case "list-playback-ids":
                    res = await sdk.Playback.ListAsync(mediaId: S("mediaId")!);
                    break;
                case "get-playback-id":
                    res = await sdk.Playbacks.GetAsync(mediaId: S("mediaId")!, playbackId: S("playbackId")!);
                    break;
                case "getDrmConfiguration":
                    res = await sdk.DrmConfigurations.ListAsync(offset: L("offset"), limit: L("limit"));
                    break;
                case "getDrmConfigurationById":
                    res = await sdk.DrmConfigurations.GetByIdAsync(drmConfigurationId: S("drmConfigurationId")!);
                    break;
                case "get-all-streams":
                    res = await sdk.LiveStreams.GetAllAsync(limit: L("limit"), offset: L("offset"), orderBy: liveOrderBy);
                    break;
                case "get-live-stream-by-id":
                    res = await sdk.LiveStreams.GetByIdAsync(streamId: S("streamId")!);
                    break;
                case "get-live-stream-viewer-count-by-id":
                    res = await sdk.LiveStreams.GetViewerCountAsync(streamId: S("streamId")!);
                    break;
                case "get-live-stream-playback-id":
                    res = await sdk.LivePlayback.GetPlaybackDetailsAsync(streamId: S("streamId")!, playbackId: S("playbackId")!);
                    break;
                case "get-specific-simulcast-of-stream":
                    res = await sdk.SimulcastStream.GetSpecificAsync(streamId: S("streamId")!, simulcastId: S("simulcastId")!);
                    break;
                case "list_signing_keys":
                    res = await sdk.SigningKeys.ListAsync(limit: L("limit"), offset: L("offset"));
                    break;
                case "get-signing_key_by_id":
                    res = await sdk.SigningKeys.GetByIdAsync(signingKeyId: S("signingKeyId")!);
                    break;
                case "list_video_views":
                {
                    var request = new ListVideoViewsRequest
                    {
                        Timespan = S("timespan") is string ts ? ListVideoViewsTimespanExtension.ToEnum(ts) : null,
                        Limit = L("limit"),
                        Offset = L("offset"),
                    };
                    res = await sdk.Views.ListAsync(request: request);
                    break;
                }
                case "get_video_view_details":
                    res = await sdk.Views.GetViewDetailsAsync(viewId: S("viewId")!);
                    break;
                case "list_by_top_content":
                    res = await sdk.Views.ListByTopContentAsync(
                        timespan: S("timespan") is string ts2 ? ListByTopContentTimespanExtension.ToEnum(ts2) : null,
                        limit: L("limit"));
                    break;
                case "list_dimensions":
                    res = await sdk.Dimensions.ListAsync();
                    break;
                case "list_filter_values_for_dimension":
                    res = await sdk.Dimensions.ListFiltersAsync(
                        dimensionsId: DimensionsIdExtension.ToEnum(S("dimensionsId")!),
                        timespan: S("timespan") is string ts3 ? ListFilterValuesForDimensionTimespanExtension.ToEnum(ts3) : null);
                    break;
                case "list_breakdown_values":
                {
                    var request = new ListBreakdownValuesRequest
                    {
                        MetricId = ListBreakdownValuesMetricIdExtension.ToEnum(S("metricId")!),
                        Timespan = S("timespan") is string ts4 ? ListBreakdownValuesTimespanExtension.ToEnum(ts4) : null,
                        GroupBy = S("groupBy"),
                    };
                    res = await sdk.Metrics.ListBreakdownValuesAsync(request: request);
                    break;
                }
                case "list_overall_values":
                    res = await sdk.Metrics.ListOverallValuesAsync(
                        metricId: ListOverallValuesMetricIdExtension.ToEnum(S("metricId")!),
                        timespan: S("timespan") is string ts5 ? ListOverallValuesTimespanExtension.ToEnum(ts5) : null);
                    break;
                case "get_timeseries_data":
                {
                    var request = new GetTimeseriesDataRequest
                    {
                        MetricId = GetTimeseriesDataMetricIdExtension.ToEnum(S("metricId")!),
                        Timespan = S("timespan") is string ts6 ? GetTimeseriesDataTimespanExtension.ToEnum(ts6) : null,
                        GroupBy = S("groupBy") is string gb ? GroupByExtension.ToEnum(gb) : null,
                    };
                    res = await sdk.Metrics.GetTimeseriesDataAsync(request: request);
                    break;
                }
                case "list_comparison_values":
                    res = await sdk.Metrics.CompareAsync(
                        timespan: S("timespan") is string ts7 ? ListComparisonValuesTimespanExtension.ToEnum(ts7) : null,
                        dimension: S("dimension") is string dim ? DimensionExtension.ToEnum(dim) : null,
                        valueP: S("value"));
                    break;
                case "list_errors":
                    res = await sdk.Errors.ListAsync(
                        timespan: S("timespan") is string ts8 ? ListErrorsTimespanExtension.ToEnum(ts8) : null,
                        limit: L("limit"));
                    break;
                default:
                    return SdkResult.Err("SDKMappingError", "No C# SDK method mapping for this operationId");
            }

            var data = ExtractResponseData(res);
            var jsonText = JsonConvert.SerializeObject(data, Utilities.GetDefaultJsonSerializerSettings());
            return SdkResult.Success(string.IsNullOrWhiteSpace(jsonText) ? null : JToken.Parse(jsonText));
        }
        catch (FastpixException ex)
        {
            int? status = null;
            try { status = (int)ex.Response.StatusCode; } catch { /* response may be unset */ }
            return SdkResult.Err(ex.GetType().Name, ex.Message, status, ex.Body);
        }
        catch (Exception ex)
        {
            return SdkResult.Err(ex.GetType().Name, ex.Message);
        }
    }

    /// <summary>
    /// Pull the actual payload out of an SDK response wrapper, skipping transport
    /// metadata. Mirrors the property-scan in the PHP harness.
    /// </summary>
    private static object ExtractResponseData(object res)
    {
        var props = res.GetType().GetProperties();

        var preferred = props.FirstOrDefault(p => p.Name is "Object" or "Array" or "Mixed");
        if (preferred != null)
        {
            var v = preferred.GetValue(res);
            if (v != null) return v;
        }

        foreach (var p in props)
        {
            if (MetadataProps.Contains(p.Name)) continue;
            var v = p.GetValue(res);
            if (v != null) return v;
        }

        return res;
    }
}
