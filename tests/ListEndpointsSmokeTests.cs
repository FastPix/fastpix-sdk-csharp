using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Fastpix.Tests;

public class ListEndpointsSmokeTests
{
    [Fact]
    public async Task Media_List_Works()
    {
        if (!TestConfig.HasCredentials()) return;
        TestConfig.TryCreateClient(out var sdk, out _).Should().BeTrue();
        var res = await sdk.Media.ListAsync(limit: 1, offset: 1);
        res.Should().NotBeNull();
    }

    [Fact]
    public async Task Uploads_List_Works()
    {
        if (!TestConfig.HasCredentials()) return;
        TestConfig.TryCreateClient(out var sdk, out _).Should().BeTrue();
        var res = await sdk.Uploads.ListAsync(limit: 1, offset: 1);
        res.Should().NotBeNull();
    }

    // DRM list tested via raw HTTP in DrmConfigurationsRawTests due to response shape

    [Fact]
    public async Task SigningKeys_List_Works()
    {
        if (!TestConfig.HasCredentials()) return;
        TestConfig.TryCreateClient(out var sdk, out _).Should().BeTrue();
        var res = await sdk.SigningKeys.ListAsync(limit: 1, offset: 1);
        res.Should().NotBeNull();
    }

    [Fact]
    public async Task Playlists_List_Works()
    {
        if (!TestConfig.HasCredentials()) return;
        TestConfig.TryCreateClient(out var sdk, out _).Should().BeTrue();
        var res = await sdk.Playlists.ListAsync(limit: 1, offset: 1);
        res.Should().NotBeNull();
    }
}


