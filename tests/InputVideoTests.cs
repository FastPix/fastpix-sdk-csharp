using System;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Xunit;
using System.Collections.Generic;

namespace Fastpix.Tests
{
    public class InputVideoTests
    {
        private readonly FastPix _fastPix;
        private readonly string _mediaId;

        public InputVideoTests()
        {
            var security = new Security
            {
                Username = Configuration.GetUsername(),
                Password = Configuration.GetPassword()
            };
            var serverUrl = Configuration.GetBaseUrl();
            _fastPix = new FastPix(security, serverUrl: serverUrl);
            _mediaId = "test-media-id"; // Replace with actual media ID
        }

        [Fact]
        public async Task CreateMediaAsync_ShouldCreateMedia()
        {
            try
            {
                var request = new CreateMediaRequest
                {
                    Inputs = new List<Models.Components.Input>
                    {
                        Models.Components.Input.CreateVideoInput(
                            new VideoInput
                            {
                                Type = "video",
                                Url = "https://static.fastpix.io/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4",
                                StartTime = 0D,
                                EndTime = 60D
                            }
                        )
                    },
                    Metadata = new CreateMediaRequestMetadata(),
                    AccessPolicy = CreateMediaRequestAccessPolicy.Public,
                    Mp4Support = CreateMediaRequestMp4Support.Capped4k
                };

                var response = await _fastPix.InputVideo.CreateMediaAsync(request);
                Assert.NotNull(response);
                Assert.NotNull(response.Object);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception type: {ex.GetType().FullName}");
                Console.WriteLine($"Exception message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.GetType().FullName}");
                    Console.WriteLine($"Inner exception message: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        [Fact]
        public async Task DirectUploadVideoMediaAsync_ShouldUploadVideo()
        {
            try
            {
                var request = new DirectUploadVideoMediaRequest
                {
                    CorsOrigin = "*",
                    PushMediaSettings = new PushMediaSettings
                    {
                        AccessPolicy = DirectUploadVideoMediaAccessPolicy.Public,
                        StartTime = 0D,
                        EndTime = 60D
                    }
                };

                var response = await _fastPix.InputVideo.DirectUploadVideoMediaAsync(request);
                Assert.NotNull(response);
                Assert.NotNull(response.Object);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception type: {ex.GetType().FullName}");
                Console.WriteLine($"Exception message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.GetType().FullName}");
                    Console.WriteLine($"Inner exception message: {ex.InnerException.Message}");
                }
                throw;
            }
        }
    }
} 