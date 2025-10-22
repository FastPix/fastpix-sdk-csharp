using System;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPixSamples
{
    /// <summary>
    /// Simple Error Handling Examples
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

            // Example 1: Basic error handling
            await BasicErrorHandling(client);

            // Example 2: Retry on failure
            await RetryOnFailure(client);

            // Example 3: Handle specific errors
            await HandleSpecificErrors(client);
        }

        // Basic error handling
        static async Task BasicErrorHandling(FastPix client)
        {
            try
            {
                var response = await client.Videos.GetAsync("invalid-media-id");
                Console.WriteLine("Media retrieved successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        // Retry on failure
        static async Task RetryOnFailure(FastPix client)
        {
            int maxRetries = 3;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    var response = await client.Media.ListAsync(limit: 10);
                    Console.WriteLine("Media list retrieved successfully");
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Console.WriteLine($"Attempt {retryCount} failed: {ex.Message}");
                    
                    if (retryCount >= maxRetries)
                    {
                        Console.WriteLine("Max retries reached. Operation failed.");
                        throw;
                    }
                    
                    // Wait before retry
                    await Task.Delay(1000 * retryCount);
                }
            }
        }

        // Handle specific errors
        static async Task HandleSpecificErrors(FastPix client)
        {
            try
            {
                var response = await client.Videos.GetAsync("invalid-media-id");
            }
            catch (Exception ex)
            {
                // Check error type and handle accordingly
                if (ex.Message.Contains("404") || ex.Message.Contains("Not Found"))
                {
                    Console.WriteLine("Media not found - this is expected for invalid ID");
                }
                else if (ex.Message.Contains("401") || ex.Message.Contains("Unauthorized"))
                {
                    Console.WriteLine("Authentication failed - check your credentials");
                }
                else if (ex.Message.Contains("403") || ex.Message.Contains("Forbidden"))
                {
                    Console.WriteLine("Access denied - check your permissions");
                }
                else
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }
    }
}
