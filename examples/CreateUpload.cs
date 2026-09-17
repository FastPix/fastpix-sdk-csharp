using Fastpix.Models.Requests;

namespace FastPix.Examples;

// Mint a signed direct-upload URL, then show how to send the file to it.
internal static class CreateUpload
{
    public static async Task Run()
    {
        var sdk = Env.CreateSdk();

        var res = await sdk.InputVideo.UploadAsync(new DirectUploadVideoMediaRequest
        {
            // "*" lets a browser PUT from any origin — tighten this for production.
            CorsOrigin = "*",
            // Required by the API; AccessPolicy defaults to Public here.
            PushMediaSettings = new PushMediaSettings(),
        });

        var upload = res.Object?.Data;
        Console.WriteLine($"uploadId: {upload?.UploadId}");
        Console.WriteLine($"url:      {upload?.Url}");
        Console.WriteLine();
        Console.WriteLine("Send your file straight to that URL — the bytes never touch your server.");
        Console.WriteLine("One PUT is fine for small files:");
        Console.WriteLine("  curl -X PUT --upload-file video.mp4 \\");
        Console.WriteLine("    -H \"Content-Type: video/mp4\" \"<url>\"");
        Console.WriteLine();
        Console.WriteLine("For larger files you'll usually want a resumable upload (chunked,");
        Console.WriteLine("with retries and progress); the same signed URL supports that too.");
    }
}
