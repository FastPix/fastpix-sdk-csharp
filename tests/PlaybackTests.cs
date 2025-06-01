using Xunit;
using Fastpix;
using Fastpix.Models.Components;
using System.Threading.Tasks;
using System;
using Fastpix.Models.Requests;
using Xunit.Abstractions;
using System.Linq;

namespace Fastpix.Tests
{
    public class PlaybackTests
    {
        private readonly FastPix _fastPix;
        private readonly ITestOutputHelper _output;
        private string? _streamId;

        public PlaybackTests(ITestOutputHelper output)
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

        private async Task<string> GetStreamIdAsync()
        {
            if (_streamId != null) return _streamId;

            try
            {
                var streamsResponse = await _fastPix.ManageLiveStream.GetAllStreamsAsync();
                Assert.NotNull(streamsResponse);
                Assert.NotNull(streamsResponse.GetStreamsResponse?.Data);
                Assert.NotEmpty(streamsResponse.GetStreamsResponse.Data);

                _streamId = streamsResponse.GetStreamsResponse.Data[0].StreamId;
                Assert.NotNull(_streamId);
                _output.WriteLine($"Using stream ID: {_streamId}");
                return _streamId;
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error getting stream ID: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task CreatePlaybackIdAsync_ShouldCreatePlaybackId()
        {
            try
            {
                var streamId = await GetStreamIdAsync();
                var response = await _fastPix.Playback.CreatePlaybackIdOfStreamAsync(
                    streamId: streamId,
                    playbackIdRequest: new PlaybackIdRequest
                    {
                        AccessPolicy = PlaybackIdRequestAccessPolicy.Public
                    }
                );
                Assert.NotNull(response);
                Assert.NotNull(response.PlaybackIdResponse);
                _output.WriteLine($"Created playback ID: {response.PlaybackIdResponse?.Data?.Id}");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error in CreatePlaybackIdAsync_ShouldCreatePlaybackId: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task GetLiveStreamPlaybackIdAsync_ShouldGetPlaybackId()
        {
            try
            {
                var streamId = await GetStreamIdAsync();
                // First create a playback ID
                var createResponse = await _fastPix.Playback.CreatePlaybackIdOfStreamAsync(
                    streamId: streamId,
                    playbackIdRequest: new PlaybackIdRequest
                    {
                        AccessPolicy = PlaybackIdRequestAccessPolicy.Public
                    }
                );
                Assert.NotNull(createResponse);
                Assert.NotNull(createResponse.PlaybackIdResponse);
                var playbackId = createResponse.PlaybackIdResponse?.Data?.Id;
                Assert.NotNull(playbackId);
                _output.WriteLine($"Created playback ID for get test: {playbackId}");

                // Now get the playback ID
                var response = await _fastPix.Playback.GetLiveStreamPlaybackIdAsync(
                    streamId: streamId,
                    playbackId: playbackId!
                );
                Assert.NotNull(response);
                Assert.NotNull(response.PlaybackIdResponse);
                _output.WriteLine($"Retrieved playback ID: {response.PlaybackIdResponse?.Data?.Id}");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error in GetLiveStreamPlaybackIdAsync_ShouldGetPlaybackId: {ex.Message}");
                throw;
            }
        }
    }
} 