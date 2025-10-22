using System;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPixSamples
{
    /// <summary>
    /// Simple Live Streaming Examples
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

            // 1. Create live stream
            var streamId = await CreateLiveStream(client, "My Live Stream");

            // 2. List live streams
            await ListLiveStreams(client);

            // 3. Generate streaming URLs
            GenerateStreamingUrls(streamId);
        }

        // Create live stream
        static async Task<string> CreateLiveStream(FastPix client, string streamName)
        {
            var request = new CreateLiveStreamRequest
            {
                Metadata = new LiveStreamMetadata
                {
                    Name = streamName,
                    Description = "Live stream created via C# SDK"
                },
                ReconnectWindow = 60,
                Privacy = LiveStreamPrivacy.Public
            };

            var response = await client.LiveStreams.CreateAsync(request);
            var stream = response.LiveStreamResponseDTO;

            Console.WriteLine($"Live stream created:");
            Console.WriteLine($"- Stream ID: {stream.Id}");
            Console.WriteLine($"- Stream Key: {stream.StreamKey}");
            Console.WriteLine($"- RTMPS URL: {stream.RtmpsUrl}");

            return stream.Id;
        }

        // List live streams
        static async Task ListLiveStreams(FastPix client)
        {
            var response = await client.LiveStreams.ListAsync(limit: 10);
            var streams = response.GetStreamsResponse.Data;

            Console.WriteLine($"Found {streams.Count} live streams:");
            foreach (var stream in streams)
            {
                Console.WriteLine($"- {stream.Metadata.Name} (ID: {stream.Id}, Status: {stream.Status})");
            }
        }

        // Generate streaming URLs
        static void GenerateStreamingUrls(string streamId)
        {
            var baseUrl = "https://stream.fastpix.io";
            
            Console.WriteLine("Streaming URLs:");
            Console.WriteLine($"- HLS: {baseUrl}/{streamId}.m3u8");
            Console.WriteLine($"- DASH: {baseUrl}/{streamId}.mpd");
            Console.WriteLine($"- Thumbnail: {baseUrl}/{streamId}/thumbnail.jpg");
        }
    }
}
