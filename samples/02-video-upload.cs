using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

namespace FastPixSamples
{
    /// <summary>
    /// Simple Video Upload Examples
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("FastPix SDK Sample - Video Upload");
                Console.WriteLine("================================");

                // Initialize SDK
                var client = FastPix.Builder()
                    .WithSecurity(new Security
                    {
                        Username = "5030b0db-40fb-4222-b5f3-960497046aac",
                        Password = "b93ab5d7-6ee2-48ae-b120-44da31e6cb17"
                    })
                    .Build();

                Console.WriteLine("✓ SDK initialized successfully!");

                // 1. Upload video from device (will fail due to missing file)
                Console.WriteLine("\n1. Testing video upload from device...");
                try
                {
                    await UploadVideoFromDevice(client, "path/to/video.mp4");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Upload failed: {ex.Message}");
                }

                // 2. Create media from URL (will fail due to invalid URL)
                Console.WriteLine("\n2. Testing media creation from URL...");
                try
                {
                    await CreateMediaFromUrl(client, "https://example.com/video.mp4");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Create media failed: {ex.Message}");
                }

                Console.WriteLine("\nSample completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        // Upload video file directly
        static async Task<string> UploadVideoFromDevice(FastPix client, string filePath)
        {
            Console.WriteLine($"  Attempting to upload video from: {filePath}");
            
            // Step 1: Get upload URL
            var uploadRequest = new DirectUploadVideoMediaRequest
            {
                CorsOrigin = "*", // Allow all origins
                PushMediaSettings = new PushMediaSettings
                {
                    // Add any required settings here
                }
            };

            Console.WriteLine("  Requesting upload URL...");
            var response = await client.Videos.UploadAsync(uploadRequest);
            
            Console.WriteLine($"  Response received. Success: {response.Object.Success}");
            Console.WriteLine($"  Response data: {response.Object.Data}");
            
            var uploadId = response.Object.Data?.UploadId;
            var uploadUrl = response.Object.Data?.Url;

            Console.WriteLine($"  Upload ID: {uploadId}");
            Console.WriteLine($"  Upload URL: {uploadUrl}");

            // Step 2: Upload file
            Console.WriteLine("  Attempting to upload file...");
            using (var fileStream = File.OpenRead(filePath))
            using (var httpClient = new HttpClient())
            {
                var content = new StreamContent(fileStream);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("video/mp4");
                
                await httpClient.PutAsync(uploadUrl, content);
            }

            Console.WriteLine($"  ✅ Video uploaded! ID: {uploadId}");
            return uploadId ?? "unknown";
        }

        // Create media from public URL
        static async Task<string> CreateMediaFromUrl(FastPix client, string videoUrl)
        {
            Console.WriteLine($"  Attempting to create media from URL: {videoUrl}");
            
            // Create video input for the media
            var videoInput = new VideoInput
            {
                Type = "video",
                Url = videoUrl
            };

            // Create input using the correct Input class from Components
            var input = Fastpix.Models.Components.Input.CreateVideoInput(videoInput);

            var request = new CreateMediaRequest
            {
                Inputs = new List<Fastpix.Models.Components.Input> { input },
                AccessPolicy = CreateMediaRequestAccessPolicy.Public
            };

            Console.WriteLine("  Sending create media request...");
            var response = await client.Videos.CreateFromUrlAsync(request);
            
            Console.WriteLine($"  Response received. Success: {response.CreateMediaSuccessResponse?.Success}");
            Console.WriteLine($"  Response data: {response.CreateMediaSuccessResponse?.Data}");
            
            var mediaId = response.CreateMediaSuccessResponse?.Data?.Id;

            Console.WriteLine($"  ✅ Media created from URL! ID: {mediaId}");
            return mediaId ?? "unknown";
        }
    }
}