using Fastpix;
using Fastpix.Models.Components;

namespace FastPix.Examples;

// Three ways to construct the SDK client. This example doesn't call the API.
internal static class SdkInitialization
{
    public static Task Run()
    {
        // 1. Static credentials.
        var sdk = new FastpixSDK(security: new Security
        {
            Username = "your-access-token",
            Password = "your-secret-key",
        });

        // 2. From environment variables, resolved on every request.
        var fromEnv = new FastpixSDK(securitySource: () => new Security
        {
            Username = Environment.GetEnvironmentVariable("FASTPIX_USERNAME") ?? "",
            Password = Environment.GetEnvironmentVariable("FASTPIX_PASSWORD") ?? "",
        });

        // 3. Point at a specific base URL (handy for a local mock).
        var customUrl = new FastpixSDK(
            security: new Security { Username = "your-access-token", Password = "your-secret-key" },
            serverUrl: "https://api.fastpix.com/v1/");

        Console.WriteLine("SDK client constructed three ways.");
        return Task.CompletedTask;
    }
}
