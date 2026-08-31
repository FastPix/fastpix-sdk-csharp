using Fastpix.Models.Requests;

namespace FastPix.Examples;

// Turn on AI features for a media: a generated summary and chapters. These need
// a media that has finished processing (status "Ready"); set FASTPIX_READY_MEDIA_ID.
internal static class AiFeatures
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

        // 1. Ask FastPix to generate a summary. The result arrives asynchronously;
        //    watch for the video.media.summary.ready webhook.
        var summary = await sdk.InVideoAIFeatures.UpdateSummaryAsync(mediaId,
            new UpdateMediaSummaryRequestBody { Generate = true });
        Json.Print(summary.Object);

        // 2. Ask FastPix to generate chapters.
        var chapters = await sdk.InVideoAI.UpdateMediaChaptersAsync(mediaId,
            new UpdateMediaChaptersRequestBody { Chapters = true });
        Json.Print(chapters.Object);
    }
}
