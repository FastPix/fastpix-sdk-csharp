using System;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPixSamples
{
    /// <summary>
    /// Simple Playback Management Examples
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

            // 1. Create playback ID
            var playbackId = await CreatePlaybackId(client, "your-media-id-here");

            // 2. Generate streaming URLs
            GenerateStreamingUrls(playbackId);

            // 3. Get playback details
            await GetPlaybackDetails(client, "your-media-id-here", playbackId);
        }

        // Create playback ID for media
        static async Task<string> CreatePlaybackId(FastPix client, string mediaId)
        {
            var request = new CreateMediaPlaybackIdRequestBody
            {
                AccessPolicy = AccessPolicy.Public
            };

            var response = await client.Playback.CreateAsync(mediaId, request);
            var playbackId = response.Object.Id;

            Console.WriteLine($"Playback ID created: {playbackId}");
            return playbackId;
        }

        // Generate streaming URLs
        static void GenerateStreamingUrls(string playbackId)
        {
            var baseUrl = "https://stream.fastpix.io";
            
            Console.WriteLine("Streaming URLs:");
            Console.WriteLine($"- HLS: {baseUrl}/{playbackId}.m3u8");
            Console.WriteLine($"- DASH: {baseUrl}/{playbackId}.mpd");
            Console.WriteLine($"- Thumbnail: {baseUrl}/{playbackId}/thumbnail.jpg");
        }

        // Get playback details
        static async Task GetPlaybackDetails(FastPix client, string mediaId, string playbackId)
        {
            var response = await client.Playback.GetByIdAsync(mediaId, playbackId);
            var playback = response.Object;

            Console.WriteLine($"Playback Details:");
            Console.WriteLine($"- ID: {playback.Id}");
            Console.WriteLine($"- Access Policy: {playback.AccessPolicy}");
            Console.WriteLine($"- Created: {playback.CreatedAt}");
        }
    }
}
