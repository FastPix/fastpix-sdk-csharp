using Fastpix.Utils;
using Xunit;

namespace Fastpix.UnitTests;

// Deterministic, offline unit tests for the SDK's URL construction. No network,
// no credentials — safe to run as a CI gate on every push/PR.
public class UrlBuilderTests
{
    [Fact]
    public void Build_TrimsTrailingSlashOnBaseUrl_AndAppendsPath()
    {
        var url = UrlBuilder.Build("https://api.fastpix.com/v1/", "/on-demand", request: null);
        Assert.Equal("https://api.fastpix.com/v1/on-demand", url);
    }

    [Fact]
    public void Build_PreservesFragment()
    {
        var url = UrlBuilder.Build("https://api.fastpix.com/v1", "/on-demand#section", request: null);
        Assert.Equal("https://api.fastpix.com/v1/on-demand#section", url);
    }

    [Fact]
    public void Build_ThrowsOnMalformedUrlWithMultipleFragments()
    {
        Assert.Throws<ArgumentException>(
            () => UrlBuilder.Build("https://api.fastpix.com/v1", "/a#b#c", request: null));
    }

    [Fact]
    public void ReplaceParameters_SubstitutesAndUrlEscapesPathParams()
    {
        var url = UrlBuilder.ReplaceParameters(
            "https://api.fastpix.com/v1/on-demand/{mediaId}",
            new Dictionary<string, string> { ["mediaId"] = "a b" });

        Assert.Equal("https://api.fastpix.com/v1/on-demand/a%20b", url);
    }

    [Fact]
    public void ReplaceParameters_HandlesMultipleParams()
    {
        var url = UrlBuilder.ReplaceParameters(
            "/on-demand/{mediaId}/tracks/{trackId}",
            new Dictionary<string, string> { ["mediaId"] = "m1", ["trackId"] = "t1" });

        Assert.Equal("/on-demand/m1/tracks/t1", url);
    }

    [Fact]
    public void SerializeQueryParams_JoinsPairsWithAmpersand()
    {
        var result = UrlBuilder.SerializeQueryParams(new Dictionary<string, List<string>>
        {
            ["limit"] = new() { "10" },
            ["orderBy"] = new() { "desc" },
        });

        Assert.Contains("limit=10", result);
        Assert.Contains("orderBy=desc", result);
        Assert.Contains("&", result);
    }

    [Fact]
    public void SerializeQueryParams_EmitsRepeatedKeyForMultipleValues()
    {
        var result = UrlBuilder.SerializeQueryParams(new Dictionary<string, List<string>>
        {
            ["id"] = new() { "1", "2" },
        });

        Assert.Equal("id=1&id=2", result);
    }

    [Fact]
    public void SerializeQueryParams_ReturnsEmptyStringWhenNoParams()
    {
        var result = UrlBuilder.SerializeQueryParams(new Dictionary<string, List<string>>());
        Assert.Equal("", result);
    }
}
