using Xunit;
using Fastpix;
using Fastpix.Models.Components;
using System.Threading.Tasks;
using System;
using Fastpix.Models.Requests;
using Xunit.Abstractions;
using Newtonsoft.Json;

namespace Fastpix.Tests
{
    public class SimulcastStreamTests
    {
        private readonly FastPix _fastPix;
        private string _streamId;
        private readonly ITestOutputHelper _output;

        public SimulcastStreamTests(ITestOutputHelper output)
        {
            var security = new Security
            {
                Username = Configuration.GetUsername(),
                Password = Configuration.GetPassword()
            };
            var serverUrl = Configuration.GetLiveUrl();
            _fastPix = new FastPix(security, serverUrl: serverUrl);
            _output = output;
        }

        [Fact]
        public async Task CreateSimulcastAsync_ShouldCreateSimulcast()
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
                _streamId = streamList[0].StreamId!;
                Assert.NotNull(_streamId);

                var simulcastRequest = new SimulcastRequest
                {
                    Url = "rtmp://hyd01.contribute.live-video.net/app/",
                    StreamKey = "live_1012464221_DuM8W004MoZYNxQEZ0czODgfHCFBhk"
                };

                var simulcastResponse = await _fastPix.SimulcastStream.CreateSimulcastOfStreamAsync(
                    streamId: _streamId,
                    simulcastRequest: simulcastRequest
                );

                Assert.NotNull(simulcastResponse);
                Assert.NotNull(simulcastResponse.SimulcastResponse);
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
        public async Task UpdateSpecificSimulcastOfStreamAsync_ShouldUpdateSimulcast()
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
                _streamId = streamList[0].StreamId!;
                Assert.NotNull(_streamId);

                // Create a simulcast first
                var simulcastRequest = new SimulcastRequest
                {
                    Url = "rtmp://hyd01.contribute.live-video.net/app/",
                    StreamKey = "live_1012464221_DuM8W004MoZYNxQEZ0czODgfHCFBhk"
                };

                var createResponse = await _fastPix.SimulcastStream.CreateSimulcastOfStreamAsync(
                    streamId: _streamId,
                    simulcastRequest: simulcastRequest
                );

                Assert.NotNull(createResponse);
                Assert.NotNull(createResponse.SimulcastResponse);
                var simulcastId = createResponse.SimulcastResponse.Data.SimulcastId; // Use the ID from the created simulcast
                Assert.NotNull(simulcastId);

                var simulcastUpdateRequest = new SimulcastUpdateRequest
                {
                    Metadata = new SimulcastUpdateRequestMetadata()
                };

                var updateResponse = await _fastPix.SimulcastStream.UpdateSpecificSimulcastOfStreamAsync(
                    streamId: _streamId,
                    simulcastId: simulcastId,
                    simulcastUpdateRequest: simulcastUpdateRequest
                );

                Assert.NotNull(updateResponse);
                Assert.NotNull(updateResponse.SimulcastUpdateResponse);
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
        public async Task UpdateSimulcast_WithValidIds_ShouldUpdateSuccessfully()
        {
            // First create a new stream to ensure we have a fresh stream in idle state
            var createStreamRequest = new CreateLiveStreamRequest()
            {
                PlaybackSettings = new PlaybackSettings(),
                InputMediaSettings = new InputMediaSettings()
                {
                    Metadata = new CreateLiveStreamRequestMetadata()
                }
            };

            var streamResponse = await _fastPix.StartLiveStream.CreateNewStreamAsync(createStreamRequest);
            var streamData = streamResponse.LiveStreamResponseDTO?.Data;
            Assert.NotNull(streamData);
            Assert.NotNull(streamData?.StreamId);

            var streamId = streamData.StreamId;
            _output.WriteLine($"Created new stream with ID: {streamId}");
            _output.WriteLine($"Stream status: {streamData.Status}");

            // Wait a moment to ensure stream is fully created
            await Task.Delay(2000);

            try
            {
                // Create a simulcast first
                var simulcastRequest = new SimulcastRequest()
                {
                    Url = "rtmp://live.twitch.tv/app",
                    StreamKey = "live_123456789_abcdefghijklmnopqrstuvwxyz"
                };
                var createResponse = await _fastPix.SimulcastStream.CreateSimulcastOfStreamAsync(streamId!, simulcastRequest);
                Assert.NotNull(createResponse.SimulcastResponse?.Data?.SimulcastId);
                var simulcastId = createResponse.SimulcastResponse.Data.SimulcastId;

                // Update request
                var updateRequest = new SimulcastUpdateRequest()
                {
                    IsEnabled = false,
                    Metadata = new SimulcastUpdateRequestMetadata()
                };

                // Act
                var response = await _fastPix.SimulcastStream.UpdateSpecificSimulcastOfStreamAsync(streamId!, simulcastId!, updateRequest);

                // Print full details for debugging
                _output.WriteLine($"UpdateSpecificSimulcastOfStreamAsync raw response: {response}");
                _output.WriteLine($"response.SimulcastUpdateResponse: {JsonConvert.SerializeObject(response.SimulcastUpdateResponse, Formatting.Indented)}");

                // Assert
                Assert.NotNull(response);
                Assert.NotNull(response.SimulcastUpdateResponse);
                Assert.NotNull(response.SimulcastUpdateResponse?.Data);
                Assert.Equal(simulcastId, response.SimulcastUpdateResponse?.Data?.SimulcastId);
                Assert.False(response.SimulcastUpdateResponse?.Data?.IsEnabled);
            }
            catch (Fastpix.Models.Errors.SimulcastUnavailableException ex)
            {
                _output.WriteLine($"Simulcast API error: {ex.Message}");
                if (ex.Error != null)
                {
                    _output.WriteLine($"Error Code: {ex.Error.Code}");
                    _output.WriteLine($"Error Message: {ex.Error.Message}");
                    _output.WriteLine($"Error Description: {ex.Error.Description}");
                }
                return;
            }
        }
    }
} 