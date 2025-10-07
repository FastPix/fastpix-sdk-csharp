using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

using Fastpix;

namespace Fastpix.Tests;

public class SmokeTests
{
    [Fact]
    public async Task ListMedia_Should_Work_When_Credentials_Are_Set()
    {
        if (!TestConfig.HasCredentials()) return;
        TestConfig.TryCreateClient(out var sdk, out _).Should().BeTrue();

        var resp = await sdk.Media.ListAsync(limit: 1, offset: 1);
        resp.Should().NotBeNull();
    }
}


