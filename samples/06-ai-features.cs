using System;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPixSamples
{
    /// <summary>
    /// Simple AI Features Examples
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize SDK
            var client = FastPix.Builder()
                .WithSecurity(new Security
                {
                    Username = "your-username-here",
                    Password = "your-password-here"
                })
                .Build();

            // 1. Generate video summary
            await GenerateVideoSummary(client, "your-media-id-here");

            // 2. Generate video chapters
            await GenerateVideoChapters(client, "your-media-id-here");

            // 3. Check AI features status
            await CheckAIFeaturesStatus(client, "your-media-id-here");
        }

        // Generate video summary
        static async Task GenerateVideoSummary(FastPix client, string mediaId)
        {
            var request = new UpdateMediaSummaryRequestBody
            {
                Generate = true,
                SummaryLength = 150
            };

            await client.MediaAI.UpdateSummaryAsync(mediaId, request);
            Console.WriteLine("Video summary generation started!");
        }

        // Generate video chapters
        static async Task GenerateVideoChapters(FastPix client, string mediaId)
        {
            var request = new UpdateMediaChaptersRequestBody
            {
                Chapters = true
            };

            await client.MediaAI.UpdateChaptersAsync(mediaId, request);
            Console.WriteLine("Video chapters generation started!");
        }

        // Check AI features status
        static async Task CheckAIFeaturesStatus(FastPix client, string mediaId)
        {
            var response = await client.Videos.GetAsync(mediaId);
            var media = response.Object;

            Console.WriteLine($"AI Features Status:");
            Console.WriteLine($"- Summary: {(media.Summary != null ? "Available" : "Processing")}");
            Console.WriteLine($"- Chapters: {(media.Chapters?.Count > 0 ? $"Available ({media.Chapters.Count} chapters)" : "Processing")}");
        }
    }
}