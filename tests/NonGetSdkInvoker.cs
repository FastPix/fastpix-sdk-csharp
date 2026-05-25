using System.Net.Http;
using Fastpix.Models.Errors;
using Fastpix.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Components = Fastpix.Models.Components;
using Requests = Fastpix.Models.Requests;

namespace Fastpix.Tests;

internal sealed class NonGetResult
{
    public bool Ok { get; init; }
    public JToken? Value { get; init; }      // wire-shaped SDK return value
    public int? StatusCode { get; init; }
    public JToken? RawBody { get; init; }     // raw HTTP body off the SDK response
    public string? ErrorName { get; init; }
    public string? ErrorMessage { get; init; }
    public JToken? ErrorBodyJson { get; init; }
}

/// <summary>
/// Calls the mutating C# SDK method for an operationId and returns the parsed
/// value plus the raw wire body (read back from <c>HttpMeta.Response</c>, which
/// the SDK buffers). In-process equivalent of the PHP subprocess in
/// validate-non-get-endpoints.ts.
/// </summary>
internal static class NonGetSdkInvoker
{
    private static readonly HashSet<string> MetadataProps = new()
    {
        "HttpMeta", "StatusCode", "ContentType", "RawResponse", "Headers", "DefaultError",
    };

    public static async Task<NonGetResult> InvokeAsync(FastpixSDK sdk, string operationId, IReadOnlyDictionary<string, string?> ctx)
    {
        string? G(string k) => ctx.TryGetValue(k, out var v) ? v : null;

        try
        {
            object res;
            switch (operationId)
            {
                // ---------------- CREATE ----------------
                case "create-media":
                    res = await sdk.InputVideo.CreateMediaAsync(new Components.CreateMediaRequest
                    {
                        Inputs = new() { Components.Input.CreatePullVideoInput(new Components.PullVideoInput()) },
                        Metadata = new() { ["source"] = "sdk-validate" },
                    });
                    break;
                case "create_signing_key":
                    res = await sdk.SigningKeys.CreateAsync();
                    break;
                case "create-a-playlist":
                    res = await sdk.Playlist.CreateAsync(Components.CreatePlaylistRequest.CreateManual(new Components.CreatePlaylistRequestManual
                    {
                        Name = "sdk-validate-playlist",
                        ReferenceId = "sdkvalidate" + Guid.NewGuid().ToString("N"),
                        Type = Components.CreatePlaylistRequestManualType.Manual,
                    }));
                    break;
                case "create-new-stream":
                    res = await sdk.LiveStreams.CreateAsync(new Components.CreateLiveStreamRequest
                    {
                        PlaybackSettings = new Components.PlaybackSettings(),
                        InputMediaSettings = new Components.InputMediaSettings { Metadata = new() { ["name"] = "sdk-validate" } },
                    });
                    break;
                case "create-media-playback-id":
                    res = await sdk.Playback.CreateAsync(G("mediaId")!, new Requests.CreateMediaPlaybackIdRequestBody { AccessPolicy = Components.AccessPolicy.Public });
                    break;
                case "Add-media-track":
                    res = await sdk.ManageVideos.AddMediaTrackAsync(G("mediaId")!, new Requests.AddMediaTrackRequestBody { Tracks = new Components.AddTrackRequest() });
                    break;
                case "Generate-subtitle-track":
                    res = await sdk.ManageVideos.GenerateSubtitlesAsync(G("mediaId")!, G("trackId")!, new Components.TrackSubtitlesGenerateRequest());
                    break;
                case "create-playbackId-of-stream":
                    res = await sdk.LivePlayback.CreateAsync(G("streamId")!, new Components.PlaybackIdRequest());
                    break;
                case "create-simulcast-of-stream":
                    res = await sdk.Simulcasts.CreateAsync(G("streamId")!, new Components.SimulcastRequest { Url = "rtmp://example.com/live", StreamKey = "sk-" + Guid.NewGuid().ToString("N") });
                    break;
                case "direct-upload-video-media":
                    res = await sdk.InputVideo.UploadAsync(new Requests.DirectUploadVideoMediaRequest
                    {
                        PushMediaSettings = new Requests.PushMediaSettings { Metadata = new() { ["source"] = "sdk-validate" } },
                    });
                    break;

                // ---------------- UPDATE (PUT/PATCH) ----------------
                case "updated-media":
                    res = await sdk.Videos.UpdateAsync(G("mediaId")!, new Requests.UpdatedMediaRequestBody { Metadata = new() { ["updated"] = "true" }, Title = "SDK Validate Title" });
                    break;
                case "updated-source-access":
                    res = await sdk.ManageVideos.UpdateSourceAccessAsync(G("mediaId")!, new Requests.UpdatedSourceAccessRequestBody { SourceAccess = true });
                    break;
                case "updated-mp4Support":
                    res = await sdk.ManageVideos.UpdateMp4SupportAsync(G("mediaId")!, new Requests.UpdatedMp4SupportRequestBody());
                    break;
                case "update-media-summary":
                    res = await sdk.InVideoAIFeatures.UpdateSummaryAsync(G("mediaId")!, new Requests.UpdateMediaSummaryRequestBody { Generate = true });
                    break;
                case "update-media-chapters":
                    res = await sdk.InVideoAI.UpdateMediaChaptersAsync(G("mediaId")!, new Requests.UpdateMediaChaptersRequestBody { Chapters = true });
                    break;
                case "update-media-named-entities":
                    res = await sdk.InVideoAI.UpdateNamedEntitiesAsync(G("mediaId")!, new Requests.UpdateMediaNamedEntitiesRequestBody { NamedEntities = true });
                    break;
                case "update-media-moderation":
                    res = await sdk.Moderations.UpdateAsync(G("mediaId")!, new Requests.UpdateMediaModerationRequestBody { Moderation = new Requests.UpdateMediaModerationModeration() });
                    break;
                case "update-media-track":
                    res = await sdk.Videos.UpdateTrackAsync(G("trackId")!, G("mediaId")!, new Components.UpdateTrackRequest());
                    break;
                case "update-domain-restrictions":
                    res = await sdk.Playback.UpdateDomainRestrictionsAsync(G("mediaId")!, G("playbackId")!, new Requests.UpdateDomainRestrictionsRequestBody { Allow = new() { "example.com" } });
                    break;
                case "update-user-agent-restrictions":
                    res = await sdk.Playback.UpdateUserAgentRestrictionsAsync(G("mediaId")!, G("playbackId")!, new Requests.UpdateUserAgentRestrictionsRequestBody { Allow = new() { "Mozilla" } });
                    break;
                case "update-a-playlist":
                    res = await sdk.Playlists.UpdateAsync(G("playlistId")!, new Components.UpdatePlaylistRequest { Name = "SDK Validate Updated", Description = "updated by validator" });
                    break;
                case "add-media-to-playlist":
                    res = await sdk.Playlists.AddMediaAsync(G("playlistId")!, new Components.MediaIdsRequest { MediaIds = new() { G("mediaId")! } });
                    break;
                case "change-media-order-in-playlist":
                    res = await sdk.Playlists.ReorderMediaAsync(G("playlistId")!, new Components.MediaIdsRequest { MediaIds = new() { G("mediaId")! } });
                    break;
                case "update-live-stream":
                    res = await sdk.Streams.UpdateAsync(G("streamId")!, new Components.PatchLiveStreamRequest { Metadata = new() { ["updated"] = "true" }, ReconnectWindow = 120 });
                    break;
                case "update-specific-simulcast-of-stream":
                    res = await sdk.Simulcasts.UpdateAsync(G("streamId")!, G("simulcastId")!, new Components.SimulcastUpdateRequest { IsEnabled = false });
                    break;
                case "enable-live-stream":
                    res = await sdk.ManageLiveStream.EnableAsync(G("streamId")!);
                    break;
                case "disable-live-stream":
                    res = await sdk.ManageLiveStream.DisableAsync(G("streamId")!);
                    break;
                case "complete-live-stream":
                    res = await sdk.LiveStreams.CompleteAsync(G("streamId")!);
                    break;
                case "cancel-upload":
                    res = await sdk.ManageVideos.CancelUploadAsync(G("uploadId")!);
                    break;

                // ---------------- DELETE ----------------
                case "delete-media-from-playlist":
                    res = await sdk.Playlists.DeleteMediaAsync(G("playlistId")!, new Components.MediaIdsRequest { MediaIds = new() { G("mediaId")! } });
                    break;
                case "delete-a-playlist":
                    res = await sdk.Playlists.DeleteAsync(G("playlistId")!);
                    break;
                case "delete-media-track":
                    res = await sdk.Tracks.DeleteAsync(G("mediaId")!, G("trackId")!);
                    break;
                case "delete-media-playback-id":
                    res = await sdk.Playback.DeleteAsync(G("mediaId")!, G("playbackId")!);
                    break;
                case "delete-simulcast-of-stream":
                    res = await sdk.Simulcasts.DeleteAsync(G("streamId")!, G("simulcastId")!);
                    break;
                case "delete-playbackId-of-stream":
                    res = await sdk.LivePlayback.DeletePlaybackIdAsync(G("streamId")!, G("playbackId")!);
                    break;
                case "delete-live-stream":
                    res = await sdk.LiveStreams.DeleteAsync(G("streamId")!);
                    break;
                case "delete-media":
                    res = await sdk.ManageVideos.DeleteMediaAsync(G("mediaId")!);
                    break;
                case "delete_signing_key":
                    res = await sdk.SigningKeys.DeleteAsync(G("signingKeyId")!);
                    break;

                default:
                    return new NonGetResult { Ok = false, ErrorName = "SDKMappingError", ErrorMessage = $"No C# SDK mapping for {operationId}" };
            }

            var (status, rawBody) = await ReadRawAsync(res);
            var data = ExtractResponseData(res);
            var jsonText = JsonConvert.SerializeObject(data, Utilities.GetDefaultJsonSerializerSettings());
            var value = string.IsNullOrWhiteSpace(jsonText) ? null : JToken.Parse(jsonText);

            return new NonGetResult { Ok = true, Value = value, StatusCode = status, RawBody = rawBody };
        }
        catch (FastpixException ex)
        {
            int? status = null;
            try { status = (int)ex.Response.StatusCode; } catch { /* response may be unset */ }
            JToken? bodyJson = null;
            try { bodyJson = JToken.Parse(ex.Body); } catch { /* not JSON */ }
            return new NonGetResult { Ok = false, ErrorName = ex.GetType().Name, ErrorMessage = ex.Message, StatusCode = status, ErrorBodyJson = bodyJson };
        }
        catch (Exception ex)
        {
            return new NonGetResult { Ok = false, ErrorName = ex.GetType().Name, ErrorMessage = ex.Message };
        }
    }

    private static async Task<(int? status, JToken? rawBody)> ReadRawAsync(object res)
    {
        var httpMeta = res.GetType().GetProperty("HttpMeta")?.GetValue(res);
        var response = httpMeta?.GetType().GetProperty("Response")?.GetValue(httpMeta) as HttpResponseMessage;
        if (response == null) return (null, null);

        int status = (int)response.StatusCode;
        JToken? rawBody = null;
        try
        {
            var text = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(text))
            {
                try { rawBody = JToken.Parse(text); } catch { rawBody = new JValue(text); }
            }
        }
        catch { /* content already disposed; leave null */ }

        return (status, rawBody);
    }

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
