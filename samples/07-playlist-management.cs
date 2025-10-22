using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPixSamples
{
    /// <summary>
    /// Simple Playlist Management Examples
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

            // 1. Create playlist
            var playlistId = await CreatePlaylist(client, "My Playlist");

            // 2. Add media to playlist
            await AddMediaToPlaylist(client, playlistId, new List<string> { "media-id-1", "media-id-2" });

            // 3. List playlists
            await ListPlaylists(client);
        }

        // Create playlist
        static async Task<string> CreatePlaylist(FastPix client, string playlistName)
        {
            var request = new CreatePlaylistRequest
            {
                Title = playlistName,
                Description = "Created via C# SDK",
                Mode = PlaylistMode.Manual
            };

            var response = await client.Playlists.CreateAsync(request);
            var playlistId = response.PlaylistCreatedResponse.Id;

            Console.WriteLine($"Playlist created: {playlistName} (ID: {playlistId})");
            return playlistId;
        }

        // Add media to playlist
        static async Task AddMediaToPlaylist(FastPix client, string playlistId, List<string> mediaIds)
        {
            var request = new MediaIdsRequest
            {
                MediaIds = mediaIds
            };

            await client.Playlists.AddMediaAsync(playlistId, request);
            Console.WriteLine($"Added {mediaIds.Count} media items to playlist");
        }

        // List playlists
        static async Task ListPlaylists(FastPix client)
        {
            var response = await client.Playlists.ListAsync(limit: 10);
            var playlists = response.GetAllPlaylistsResponseValue.Data;

            Console.WriteLine($"Found {playlists.Count} playlists:");
            foreach (var playlist in playlists)
            {
                Console.WriteLine($"- {playlist.Title} (ID: {playlist.Id}, Media: {playlist.Media?.Count ?? 0})");
            }
        }
    }
}
