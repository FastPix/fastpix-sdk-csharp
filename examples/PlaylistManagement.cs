using Fastpix.Models.Components;

namespace FastPix.Examples;

// Create a manual playlist, read it back, then delete it. A playlist's
// referenceId must be alphanumeric and unique per workspace, so this generates
// a fresh one each run.
internal static class PlaylistManagement
{
    public static async Task Run()
    {
        var sdk = Env.CreateSdk();

        var referenceId = "example" + Guid.NewGuid().ToString("N")[..12];

        // 1. Create a manual playlist (you add media to it yourself). "Smart"
        //    playlists instead auto-fill from metadata filters.
        var created = await sdk.Playlist.CreateAsync(CreatePlaylistRequest.CreateManual(
            new CreatePlaylistRequestManual
            {
                Name = "Example playlist",
                ReferenceId = referenceId,
                Type = CreatePlaylistRequestManualType.Manual,
                Description = "Created by the FastPix CSharp examples",
            }));
        var playlistId = created.PlaylistCreatedResponse?.Data?.PlaylistByIdResponseDataManual?.Id;
        Console.WriteLine($"created playlist: {playlistId}");

        // 2. Read it back.
        var fetched = await sdk.Playlists.GetAsync(playlistId!);
        Json.Print(fetched.PlaylistByIdResponse);

        // 3. Clean up so reruns stay tidy.
        await sdk.Playlists.DeleteAsync(playlistId!);
        Console.WriteLine("deleted");
    }
}
