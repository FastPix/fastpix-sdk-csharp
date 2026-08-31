using Fastpix.Models.Components;

namespace FastPix.Examples;

// Create a live stream, toggle it, then delete it. New streams start enabled,
// so the demo disables before it enables.
internal static class LiveStreaming
{
    public static async Task Run()
    {
        var sdk = Env.CreateSdk();

        // 1. Create a stream. Keep streamKey secret — it's what your encoder
        //    (OBS, ffmpeg, ...) pushes RTMP to.
        var created = await sdk.LiveStreams.CreateAsync(new CreateLiveStreamRequest
        {
            PlaybackSettings = new PlaybackSettings(),
            InputMediaSettings = new InputMediaSettings
            {
                Metadata = new Dictionary<string, string> { { "livestream_name", "fastpix_example" } },
            },
        });
        var stream = created.LiveStreamResponseDto?.Data;
        var streamId = stream?.StreamId;
        Console.WriteLine($"streamId:  {streamId}");
        Console.WriteLine($"streamKey: {stream?.StreamKey}");

        // 2. Toggle it. Fresh streams are already enabled, so disable first.
        await sdk.ManageLiveStream.DisableAsync(streamId!);
        Console.WriteLine("disabled");
        await sdk.ManageLiveStream.EnableAsync(streamId!);
        Console.WriteLine("enabled");

        // 3. Clean up so reruns stay tidy.
        await sdk.LiveStreams.DeleteAsync(streamId!);
        Console.WriteLine("deleted");
    }
}
