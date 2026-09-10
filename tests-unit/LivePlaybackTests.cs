using System.Net;
using System.Text;
using Fastpix.Models.Components;
using Fastpix.Models.Errors;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Fastpix.Utils.Retries;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Fastpix.UnitTests;

// Drives the live playback restriction updates through a fake transport and
// asserts the exact wire shape plus the error path. No network.
public class LivePlaybackTests
{
    private sealed class FakeClient : FastpixHttpClient
    {
        private readonly HttpStatusCode _status;
        private readonly string _body;
        public HttpRequestMessage? Sent;
        public string? SentBody;
        public FakeClient(HttpStatusCode status, string body) { _status = status; _body = body; }

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken? cancellationToken = null)
        {
            Sent = request;
            SentBody = request.Content == null ? null : await request.Content.ReadAsStringAsync();
            return new HttpResponseMessage(_status) { Content = new StringContent(_body, Encoding.UTF8, "application/json"), RequestMessage = request };
        }
    }

    private static (FastpixSDK sdk, FakeClient client) Sdk(HttpStatusCode status, string body)
    {
        var client = new FakeClient(status, body);
        var sdk = new FastpixSDK(security: new Security { Username = "u", Password = "p" }, client: client,
            retryConfig: new RetryConfig(RetryConfig.RetryStrategy.NONE, null, false));
        return (sdk, client);
    }

    private const string Ok = "{\"success\":true,\"data\":{\"defaultPolicy\":\"allow\",\"allow\":[\"yourdomain.com\"],\"deny\":[]}}";

    [Fact]
    public async Task UpdateDomainRestrictions_SendsFlatPatch()
    {
        var (sdk, client) = Sdk(HttpStatusCode.OK, Ok);
        var res = await sdk.LivePlayback.UpdateDomainRestrictionsAsync("s1", "p1",
            new UpdateLiveStreamDomainRestrictionsRequestBody { Allow = new() { "yourdomain.com" }, Deny = new() { "malicioussite.io" } });

        Assert.Equal(HttpMethod.Patch, client.Sent!.Method);
        Assert.EndsWith("/live/streams/s1/playback-ids/p1/domains", client.Sent.RequestUri!.AbsoluteUri);
        Assert.Equal("application/json", client.Sent.Content!.Headers.ContentType!.MediaType);
        var body = JObject.Parse(client.SentBody!);
        Assert.Equal("allow", (string)body["defaultPolicy"]!);
        Assert.Equal("malicioussite.io", (string)body["deny"]![0]!);
        Assert.Null(body["domains"]);
        Assert.True(res.Object!.Success);
        Assert.Equal("yourdomain.com", res.Object.Data!.Allow![0]);
    }

    [Fact]
    public async Task UpdateUserAgentRestrictions_SendsFlatPatch()
    {
        var (sdk, client) = Sdk(HttpStatusCode.OK, Ok);
        var res = await sdk.LivePlayback.UpdateUserAgentRestrictionsAsync("s1", "p1",
            new UpdateLiveStreamUserAgentRestrictionsRequestBody { Deny = new() { "PostmanRuntime/7.29.0" } });

        Assert.Equal(HttpMethod.Patch, client.Sent!.Method);
        Assert.EndsWith("/live/streams/s1/playback-ids/p1/user-agents", client.Sent.RequestUri!.AbsoluteUri);
        var body = JObject.Parse(client.SentBody!);
        Assert.Equal("PostmanRuntime/7.29.0", (string)body["deny"]![0]!);
        Assert.Null(body["allow"]);
        Assert.Null(body["userAgents"]);
        Assert.True(res.Object!.Success);
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task HttpFailures_ThrowApiException(HttpStatusCode status)
    {
        var (sdk, _) = Sdk(status, "{\"success\":false}");
        var ex = await Assert.ThrowsAsync<ApiException>(() =>
            sdk.LivePlayback.UpdateDomainRestrictionsAsync("s1", "p1", new UpdateLiveStreamDomainRestrictionsRequestBody()));
        Assert.Equal(status, ex.Response.StatusCode);

        var ex2 = await Assert.ThrowsAsync<ApiException>(() =>
            sdk.LivePlayback.UpdateUserAgentRestrictionsAsync("s1", "p1", new UpdateLiveStreamUserAgentRestrictionsRequestBody()));
        Assert.Equal(status, ex2.Response.StatusCode);
    }
}
