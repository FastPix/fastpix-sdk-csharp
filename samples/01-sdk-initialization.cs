using System;
using Fastpix;
using Fastpix.Models.Components;

namespace FastPixSamples
{
    /// <summary>
    /// Simple SDK Initialization Examples
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Basic initialization (most common)
            var client = FastPix.Builder()
                .WithSecurity(new Security
                {
                    Username = "your-username-here",
                    Password = "your-password-here"
                })
                .Build();

            // Alternative: Using constructor
            var security = new Security
            {
                Username = "your-username-here",
                Password = "your-password-here"
            };
            var client2 = new FastPix(security);

            // With environment variables
            var client3 = FastPix.Builder()
                .WithSecuritySource(() => new Security
                {
                    Username = Environment.GetEnvironmentVariable("FASTPIX_USERNAME"),
                    Password = Environment.GetEnvironmentVariable("FASTPIX_PASSWORD")
                })
                .Build();

            Console.WriteLine("SDK initialized successfully!");
        }
    }
}
