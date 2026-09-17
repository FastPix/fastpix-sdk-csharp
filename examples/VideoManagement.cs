using Fastpix.Models.Components;

namespace FastPix.Examples;

// Create a media from a public URL, read it back, and list the library.
internal static class VideoManagement
{
    public static async Task Run()
    {
        var sdk = Env.CreateSdk();

        // 1. Create a media from a public URL. PullVideoInput defaults to a FastPix
        //    sample video, so this runs without a file of your own.
        var created = await sdk.InputVideo.CreateMediaAsync(new CreateMediaRequest
        {
            Inputs = new List<Input> { Input.CreatePullVideoInput(new PullVideoInput()) },
            AccessPolicy = CreateMediaRequestAccessPolicy.Public,
        });
        var mediaId = created.CreateMediaSuccessResponse?.Data?.Id;
        Console.WriteLine($"created media: {mediaId}");

        // 2. Read it back. A fresh media starts in "Created"/processing — wait for the
        //    video.media.ready webhook before playback operations.
        var media = await sdk.ManageVideos.GetByIdAsync(mediaId!);
        Console.WriteLine($"status: {media.Object?.Data?.Status}");

        // 3. List recent media.
        var list = await sdk.ManageVideos.ListAsync(limit: 5, offset: 1, orderBy: SortOrder.Desc);
        Console.WriteLine($"media on this page: {list.Object?.Data?.Count}");
    }
}
