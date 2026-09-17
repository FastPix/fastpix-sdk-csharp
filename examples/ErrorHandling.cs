using Fastpix.Models.Errors;

namespace FastPix.Examples;

// How the SDK surfaces failures: a 4xx/5xx response becomes an ApiException you catch.
internal static class ErrorHandling
{
    public static async Task Run()
    {
        var sdk = Env.CreateSdk();

        // Ask for a media id that doesn't exist. The API returns an error status,
        // which the SDK raises as an ApiException carrying the response and body.
        try
        {
            await sdk.ManageVideos.GetByIdAsync("does-not-exist");
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"status: {(int)ex.Response.StatusCode}");
            Console.WriteLine($"body:   {ex.Body}");
        }
        catch (FastpixException ex)
        {
            // Base type for anything the SDK throws (network, validation, ...).
            Console.WriteLine($"request failed: {ex.Message}");
        }
    }
}
