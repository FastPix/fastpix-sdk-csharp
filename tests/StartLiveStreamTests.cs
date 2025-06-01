using Xunit;
using Fastpix;
using Fastpix.Models.Components;
using System.Threading.Tasks;
using System;
using Fastpix.Models.Requests;

namespace Fastpix.Tests
{
    public class StartLiveStreamTests
    {
        private readonly FastPix _fastPix;

        public StartLiveStreamTests()
        {
            var security = new Security
            {
                Username = Configuration.GetUsername(),
                Password = Configuration.GetPassword()
            };
            var serverUrl = Configuration.GetLiveUrl();
            _fastPix = new FastPix(security, serverUrl: serverUrl);
        }

        [Fact]
        public async Task CreateNewStreamAsync_ShouldCreateStream()
        {
            var request = new CreateLiveStreamRequest
            {
                PlaybackSettings = new PlaybackSettings
                {
                    AccessPolicy = PlaybackSettingsAccessPolicy.Public
                },
                InputMediaSettings = new InputMediaSettings
                {
                    MaxResolution = CreateLiveStreamRequestMaxResolution.OneThousandAndEightyp,
                    ReconnectWindow = 60,
                    MediaPolicy = MediaPolicy.Public
                }
            };

            var response = await _fastPix.StartLiveStream.CreateNewStreamAsync(request);
            Assert.NotNull(response);
            Assert.NotNull(response.LiveStreamResponseDTO);
        }
    }
} 