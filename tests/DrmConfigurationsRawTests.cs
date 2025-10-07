using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Fastpix.Tests;

public class DrmConfigurationsRawTests
{
    private static string GetBasicAuth()
    {
        var username = Environment.GetEnvironmentVariable("FASTPIX_USERNAME");
        var password = Environment.GetEnvironmentVariable("FASTPIX_PASSWORD");
        username.Should().NotBeNullOrWhiteSpace("FASTPIX_USERNAME must be set");
        password.Should().NotBeNullOrWhiteSpace("FASTPIX_PASSWORD must be set");
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        return $"Basic {token}";
    }

    private static bool IsUuid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        return Regex.IsMatch(value, "^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$", RegexOptions.IgnoreCase);
    }

    [Fact]
    public async Task List_DrmConfigurations_Returns_Uuid()
    {
        if (!TestConfig.HasCredentials()) return;

        using var http = new HttpClient();
        http.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(GetBasicAuth());
        http.DefaultRequestHeaders.UserAgent.ParseAdd("FastPix-sdk/csharp-tests");

        var url = "https://api.fastpix.io/v1/on-demand/drm-configurations?offset=1&limit=1";
        var resp = await http.GetAsync(url);
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadAsStringAsync();

        var node = JsonNode.Parse(json)!.AsObject();
        node["success"]!.GetValue<bool>().Should().BeTrue();

        var dataNode = node["data"];
        dataNode.Should().NotBeNull();

        string? foundId = null;
        if (dataNode is JsonObject obj)
        {
            foundId = obj["id"]?.GetValue<string>();
        }
        else if (dataNode is JsonArray arr && arr.Count > 0)
        {
            var first = arr[0] as JsonObject;
            foundId = first?["id"]?.GetValue<string>();
        }

        foundId.Should().NotBeNullOrWhiteSpace("response should include a DRM configuration id");
        IsUuid(foundId).Should().BeTrue($"expected a UUID but got '{foundId}'");
    }
}


