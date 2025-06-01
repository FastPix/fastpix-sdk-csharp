using System;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Xunit;
using System.Linq;
using Xunit.Abstractions;
using Newtonsoft.Json;

namespace Fastpix.Tests
{
    public class MediaPlaybackTests
    {
        private readonly FastPix _fastPix;
        private readonly ITestOutputHelper _output;

        public MediaPlaybackTests(ITestOutputHelper output)
        {
            var security = new Security
            {
                Username = Configuration.GetUsername(),
                Password = Configuration.GetPassword()
            };
            var serverUrl = Configuration.GetBaseUrl();
            _fastPix = new FastPix(security, serverUrl: serverUrl);
            _output = output;
        }

        [Fact]
        public async Task CreateMediaPlaybackIdAsync_ShouldCreatePlaybackId()
        {
            try
            {
                // Fetch a real media ID
                var mediaListResponse = await _fastPix.ManageVideos.ListMediaAsync(limit: 1, offset: 1, orderBy: ListMediaOrderBy.Desc);
                var mediaList = mediaListResponse.Object?.Data;
                Assert.NotNull(mediaList);
                Assert.NotEmpty(mediaList!);
                var mediaId = mediaList[0].Id;
                Assert.NotNull(mediaId);

                var requestBody = new CreateMediaPlaybackIdRequestBody
                {
                    AccessPolicy = CreateMediaPlaybackIdAccessPolicy.Public
                };

                var response = await _fastPix.Playback.CreateMediaPlaybackIdAsync(mediaId!, requestBody);
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
        public async Task DeleteMediaPlaybackIdAsync_ShouldDeletePlaybackId()
        {
            try
            {
                // Fetch a real media ID
                var mediaListResponse = await _fastPix.ManageVideos.ListMediaAsync(limit: 1, offset: 1, orderBy: ListMediaOrderBy.Desc);
                var mediaList = mediaListResponse.Object?.Data;
                Assert.NotNull(mediaList);
                Assert.NotEmpty(mediaList!);
                var mediaId = mediaList[0].Id;
                Assert.NotNull(mediaId);

                // Wait a moment to ensure media is fully created
                await Task.Delay(2000);

                // Create a playback ID first
                var requestBody = new CreateMediaPlaybackIdRequestBody
                {
                    AccessPolicy = CreateMediaPlaybackIdAccessPolicy.Public
                };
                var createResponse = await _fastPix.Playback.CreateMediaPlaybackIdAsync(mediaId!, requestBody);
                _output.WriteLine($"CreateMediaPlaybackIdAsync Status Code: {createResponse.HttpMeta.Response.StatusCode}");
                Assert.NotNull(createResponse);
                Assert.NotNull(createResponse.Object);
                await Task.Delay(2000); // Wait for 2 seconds to ensure playback ID is available
                var playbackId = createResponse.Object?.Data?.PlaybackIds?.FirstOrDefault()?.Id;
                Assert.NotNull(playbackId);
                _output.WriteLine($"Created Playback ID: {playbackId}");

                // Now delete the playback ID
                var deleteResponse = await _fastPix.Playback.DeleteMediaPlaybackIdAsync(mediaId!, playbackId!);
                _output.WriteLine($"DeleteMediaPlaybackIdAsync Status Code: {deleteResponse.HttpMeta.Response.StatusCode}");
                Assert.NotNull(deleteResponse);
                Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.HttpMeta.Response.StatusCode);
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