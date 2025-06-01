using System;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Xunit;

namespace Fastpix.Tests
{
    public class ManageVideosTests
    {
        private readonly FastPix _fastPix;

        public ManageVideosTests()
        {
            var security = new Security
            {
                Username = Configuration.GetUsername(),
                Password = Configuration.GetPassword()
            };

            var serverUrl = Configuration.GetBaseUrl();
            _fastPix = new FastPix(security, null, null, serverUrl);
        }

        [Fact]
        public async Task ListMediaAsync_ShouldReturnMediaList()
        {
            try
            {
                var response = await _fastPix.ManageVideos.ListMediaAsync(
                    limit: 20,
                    offset: 1,
                    orderBy: ListMediaOrderBy.Desc
                );
                var mediaList = response.Object?.Data;
                Assert.NotNull(mediaList);
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
        public async Task GetMediaAsync_ShouldReturnMedia()
        {
            try
            {
                var response = await _fastPix.ManageVideos.ListMediaAsync(
                    limit: 1,
                    offset: 1,
                    orderBy: ListMediaOrderBy.Desc
                );
                var mediaList = response.Object?.Data;
                Assert.NotNull(mediaList);
                Assert.NotEmpty(mediaList!);

                var mediaId = mediaList[0].Id;
                Assert.NotNull(mediaId);

                var mediaDetailsResponse = await _fastPix.ManageVideos.GetMediaAsync(mediaId: mediaId!);
                var mediaDetails = mediaDetailsResponse.Object?.Data;

                Assert.NotNull(mediaDetails);
                Assert.Equal(mediaId, mediaDetails?.Id);
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
        public async Task UpdateMediaAsync_ShouldUpdateMedia()
        {
            try
            {
                var response = await _fastPix.ManageVideos.ListMediaAsync(
                    limit: 1,
                    offset: 1,
                    orderBy: ListMediaOrderBy.Desc
                );
                var mediaList = response.Object?.Data;
                Assert.NotNull(mediaList);
                Assert.NotEmpty(mediaList!);

                var mediaId = mediaList[0].Id;
                Assert.NotNull(mediaId);

                var updateResponse = await _fastPix.ManageVideos.UpdatedMediaAsync(
                    mediaId: mediaId!,
                    requestBody: new UpdatedMediaRequestBody()
                    {
                        Metadata = new UpdatedMediaMetadata()
                    }
                );

                Assert.NotNull(updateResponse);
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
        public async Task DeleteMediaAsync_ShouldDeleteMedia()
        {
            try
            {
                var response = await _fastPix.ManageVideos.ListMediaAsync(
                    limit: 1,
                    offset: 1,
                    orderBy: ListMediaOrderBy.Desc
                );
                var mediaList = response.Object?.Data;
                Assert.NotNull(mediaList);
                Assert.NotEmpty(mediaList!);

                var mediaId = mediaList[0].Id;
                Assert.NotNull(mediaId);

                var deleteResponse = await _fastPix.ManageVideos.DeleteMediaAsync(mediaId: mediaId!);

                Assert.NotNull(deleteResponse);
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
        public async Task RetrieveMediaInputInfoAsync_ShouldReturnInputInfo()
        {
            try
            {
                var response = await _fastPix.ManageVideos.ListMediaAsync(
                    limit: 1,
                    offset: 1,
                    orderBy: ListMediaOrderBy.Desc
                );
                var mediaList = response.Object?.Data;
                Assert.NotNull(mediaList);
                Assert.NotEmpty(mediaList!);

                var mediaId = mediaList[0].Id;
                Assert.NotNull(mediaId);

                var inputInfoResponse = await _fastPix.ManageVideos.RetrieveMediaInputInfoAsync(mediaId: mediaId!);

                Assert.NotNull(inputInfoResponse);
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