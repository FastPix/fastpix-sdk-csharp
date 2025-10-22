using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPixSamples
{
    /// <summary>
    /// Simple Video Management Examples
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

            // 1. List all media
            await ListMedia(client);

            // 2. Get specific media details
            await GetMediaDetails(client, "your-media-id-here");

            // 3. Update media metadata
            await UpdateMedia(client, "your-media-id-here", "New Title", "New Description");
        }

        // List all media
        static async Task ListMedia(FastPix client)
        {
            var response = await client.Media.ListAsync(limit: 10);
            var mediaList = response.Object.Data;

            Console.WriteLine($"Found {mediaList.Count} media items:");
            foreach (var media in mediaList)
            {
                Console.WriteLine($"- {media.Title} (ID: {media.Id}, Status: {media.Status})");
            }
        }

        // Get media details
        static async Task GetMediaDetails(FastPix client, string mediaId)
        {
            var response = await client.Videos.GetAsync(mediaId);
            var media = response.Object;

            Console.WriteLine($"Media Details:");
            Console.WriteLine($"- Title: {media.Title}");
            Console.WriteLine($"- Status: {media.Status}");
            Console.WriteLine($"- Duration: {media.Duration} seconds");
            Console.WriteLine($"- Created: {media.CreatedAt}");
        }

        // Update media metadata
        static async Task UpdateMedia(FastPix client, string mediaId, string newTitle, string newDescription)
        {
            var request = new UpdatedMediaRequestBody
            {
                Title = newTitle,
                Description = newDescription
            };

            var response = await client.Videos.UpdateAsync(mediaId, request);
            Console.WriteLine($"Media updated: {response.Object.Title}");
        }
    }
}
