using System;
using System.Threading.Tasks;
using Fastpix;
using Fastpix.Models;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Xunit;

namespace Fastpix.Tests
{
    public class ManageLiveStreamTests
    {
        private readonly FastPix _fastPix;

        public ManageLiveStreamTests()
        {
            var security = new Security
            {
                Username = Configuration.GetUsername(),
                Password = Configuration.GetPassword()
            };

            var serverUrl = Configuration.GetLiveUrl();
            _fastPix = new FastPix(security, null, null, serverUrl);
        }

        [Fact]
        public async Task GetAllStreamsAsync_ShouldReturnStreamList()
        {
            try
            {
                var response = await _fastPix.ManageLiveStream.GetAllStreamsAsync(
                    limit: "20",
                    offset: "1",
                    orderBy: GetAllStreamsOrderBy.Desc
                );
                var streamList = response.GetStreamsResponse?.Data;
                Assert.NotNull(streamList);
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
        public async Task GetLiveStreamByIdAsync_ShouldReturnStream()
        {
            try
            {
                var response = await _fastPix.ManageLiveStream.GetAllStreamsAsync(
                    limit: "1",
                    offset: "1",
                    orderBy: GetAllStreamsOrderBy.Desc
                );
                var streamList = response.GetStreamsResponse?.Data;
                Assert.NotNull(streamList);
                Assert.NotEmpty(streamList!);

                var streamId = streamList[0].StreamId;
                Assert.NotNull(streamId);

                var streamDetailsResponse = await _fastPix.ManageLiveStream.GetLiveStreamByIdAsync(streamId: streamId!);
                var streamDetails = streamDetailsResponse.LivestreamgetResponse?.Data;

                Assert.NotNull(streamDetails);
                Assert.Equal(streamId, streamDetails?.StreamId);
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
        public async Task UpdateLiveStreamAsync_ShouldUpdateStream()
        {
            try
            {
                var response = await _fastPix.ManageLiveStream.GetAllStreamsAsync(
                    limit: "1",
                    offset: "1",
                    orderBy: GetAllStreamsOrderBy.Desc
                );
                var streamList = response.GetStreamsResponse?.Data;
                Assert.NotNull(streamList);
                Assert.NotEmpty(streamList!);

                var streamId = streamList[0].StreamId;
                Assert.NotNull(streamId);

                var updateResponse = await _fastPix.ManageLiveStream.UpdateLiveStreamAsync(
                    streamId: streamId!,
                    patchLiveStreamRequest: new PatchLiveStreamRequest()
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
        public async Task DeleteLiveStreamAsync_ShouldDeleteStream()
        {
            try
            {
                var response = await _fastPix.ManageLiveStream.GetAllStreamsAsync(
                    limit: "1",
                    offset: "1",
                    orderBy: GetAllStreamsOrderBy.Desc
                );
                var streamList = response.GetStreamsResponse?.Data;
                Assert.NotNull(streamList);
                Assert.NotEmpty(streamList!);

                var streamId = streamList[0].StreamId;
                Assert.NotNull(streamId);

                var deleteResponse = await _fastPix.ManageLiveStream.DeleteLiveStreamAsync(streamId: streamId!);

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
    }
} 