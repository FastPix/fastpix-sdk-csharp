using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPix.Examples;

// Create and list playback IDs for a media. Playback needs a media that has
// finished processing (status "Ready"), so point this at an existing one via
// FASTPIX_READY_MEDIA_ID.
internal static class PlaybackManagement
{
    public static async Task Run()
    {
        var mediaId = Environment.GetEnvironmentVariable("FASTPIX_READY_MEDIA_ID");
        if (string.IsNullOrEmpty(mediaId))
        {
            Console.WriteLine("Set FASTPIX_READY_MEDIA_ID to a Ready media id, then rerun.");
            return;
        }

        var sdk = Env.CreateSdk();

        // 1. Create a public playback ID. Stream it at
        //    https://stream.fastpix.com/<playbackId>.m3u8
        var created = await sdk.Playback.CreateAsync(mediaId, new CreateMediaPlaybackIdRequestBody
        {
            AccessPolicy = AccessPolicy.Public,
        });
        Json.Print(created.Object);

        // 2. List every playback ID on the media.
        var ids = await sdk.Playback.ListAsync(mediaId);
        Json.Print(ids.Object);
    }
}
