using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPix.Examples;

// Create a live stream, add a restricted playback ID, toggle the stream, then
// delete it. New streams start enabled, so the demo disables before it enables.
internal static class LiveStreaming
{
    public static async Task Run()
    {
        var sdk = Env.CreateSdk();

        // 1. Create a stream. Keep streamKey secret — it's what your encoder
        //    (OBS, ffmpeg, ...) pushes RTMP to.
        //    EnableRecording=false skips the Live-to-VOD recording; the
        //    playback restrictions limit which sites may embed the stream.
        var created = await sdk.LiveStreams.CreateAsync(new CreateLiveStreamRequest
        {
            PlaybackSettings = new PlaybackSettings
            {
                AccessRestrictions = new PlaybackIdAccessRestrictions
                {
                    Domains = new PlaybackIdDomains { DefaultPolicy = PolicyAction.Deny, Allow = new() { "example.com" } },
                },
            },
            InputMediaSettings = new InputMediaSettings
            {
                Metadata = new Dictionary<string, string> { { "livestream_name", "fastpix_example" } },
                EnableRecording = false,
            },
        });
        var stream = created.LiveStreamResponseDto?.Data;
        var streamId = stream?.StreamId;
        Console.WriteLine($"streamId:  {streamId}");
        Console.WriteLine($"streamKey: {stream?.StreamKey}");
        Console.WriteLine($"recording: {stream?.EnableRecording}");

        // 2. Add a second playback ID with its own restrictions, then tighten
        //    the domain policy in place.
        var playback = await sdk.LivePlayback.CreateAsync(streamId!, new PlaybackIdRequest
        {
            AccessRestrictions = new PlaybackIdAccessRestrictions
            {
                UserAgents = new PlaybackIdUserAgents { DefaultPolicy = PolicyAction.Allow, Deny = new() { "PostmanRuntime" } },
            },
        });
        var playbackId = playback.PlaybackIdSuccessResponse?.Data?.Id;
        Console.WriteLine($"playbackId: {playbackId}");

        var domains = await sdk.LivePlayback.UpdateDomainRestrictionsAsync(streamId!, playbackId!, new UpdateLiveStreamDomainRestrictionsRequestBody
        {
            DefaultPolicy = UpdateLiveStreamDomainRestrictionsDefaultPolicy.Deny,
            Allow = new() { "example.com", "*.example.com" },
        });
        Json.Print(domains.Object);

        // 3. Toggle it. Fresh streams are already enabled, so disable first.
        await sdk.ManageLiveStream.DisableAsync(streamId!);
        Console.WriteLine("disabled");
        await sdk.ManageLiveStream.EnableAsync(streamId!);
        Console.WriteLine("enabled");

        // 4. Clean up so reruns stay tidy.
        await sdk.LiveStreams.DeleteAsync(streamId!);
        Console.WriteLine("deleted");
    }
}
