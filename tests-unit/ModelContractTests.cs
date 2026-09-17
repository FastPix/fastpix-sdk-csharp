using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Fastpix.UnitTests;

// Offline contract tests for the models touched by the OpenAPI sync:
// numeric media duration, enableRecording, live accessRestrictions and the
// live playback restriction update operations. No network, no credentials.
public class ModelContractTests
{
    private static string Serialize(object o) => Utilities.SerializeJSON(o);

    public static IEnumerable<object[]> DurationModels() =>
        new[]
        {
            typeof(GetMediaDetailResponse), typeof(GetAllMediaResponse), typeof(UpdateMedia),
            typeof(SourceAccessMedia), typeof(Media), typeof(LiveMediaClips),
            typeof(MediaClipResponseData), typeof(PlaylistByIdResponseMediaListItem),
        }.Select(t => new object[] { t });

    private static double? DurationOf(object model) =>
        (double?)model.GetType().GetProperty("Duration")!.GetValue(model);

    [Theory]
    [MemberData(nameof(DurationModels))]
    public void Duration_ParsesFractionalSeconds(System.Type model)
    {
        var obj = JsonConvert.DeserializeObject("{\"duration\": 145.821315}", model)!;
        Assert.Equal(145.821315, DurationOf(obj));
    }

    [Theory]
    [MemberData(nameof(DurationModels))]
    public void Duration_ParsesIntegerAsDouble(System.Type model)
    {
        var obj = JsonConvert.DeserializeObject("{\"duration\": 10}", model)!;
        Assert.Equal(10.0, DurationOf(obj));
    }

    [Theory]
    [MemberData(nameof(DurationModels))]
    public void Duration_AbsentIsNull(System.Type model)
    {
        var obj = JsonConvert.DeserializeObject("{}", model)!;
        Assert.Null(DurationOf(obj));
    }

    [Theory]
    [MemberData(nameof(DurationModels))]
    public void Duration_SerializesAsNumber(System.Type model)
    {
        var obj = Activator.CreateInstance(model)!;
        model.GetProperty("Duration")!.SetValue(obj, 145.82);
        Assert.Equal(145.82, (double)JObject.Parse(Serialize(obj))["duration"]!);
    }

    [Theory]
    [MemberData(nameof(DurationModels))]
    public void Duration_RejectsLegacyClockString(System.Type model)
    {
        Assert.Throws<JsonReaderException>(() => JsonConvert.DeserializeObject("{\"duration\": \"00:02:25\"}", model));
    }

    [Fact]
    public void EnableRecording_DefaultsToTrue_AndIsSentOnCreate()
    {
        Assert.True(new InputMediaSettings().EnableRecording);
        var req = new CreateLiveStreamRequest { PlaybackSettings = new PlaybackSettings(), InputMediaSettings = new InputMediaSettings() };
        Assert.True((bool)JObject.Parse(Serialize(req))["inputMediaSettings"]!["enableRecording"]!);
    }

    [Fact]
    public void EnableRecording_FalseRoundTrips()
    {
        var parsed = JsonConvert.DeserializeObject<InputMediaSettings>("{\"enableRecording\": false}")!;
        Assert.False(parsed.EnableRecording);
        Assert.False((bool)JObject.Parse(Serialize(parsed))["enableRecording"]!);
    }

    private const string RestrictionsJson =
        "{\"domains\":{\"defaultPolicy\":\"deny\",\"allow\":[\"example.com\"],\"deny\":[]}," +
        "\"userAgents\":{\"defaultPolicy\":\"allow\",\"allow\":[],\"deny\":[]}}";

    public static IEnumerable<object[]> RestrictionModels() =>
        new[] { typeof(PlaybackIdRequest), typeof(PlaybackSettings), typeof(PlaybackIdSuccessResponseData), typeof(PlaybackIdResponse) }
            .Select(t => new object[] { t });

    private static PlaybackIdAccessRestrictions? RestrictionsOf(object model) =>
        (PlaybackIdAccessRestrictions?)model.GetType().GetProperty("AccessRestrictions")!.GetValue(model);

    [Theory]
    [MemberData(nameof(RestrictionModels))]
    public void AccessRestrictions_ParsesSpecExample(System.Type model)
    {
        var obj = JsonConvert.DeserializeObject("{\"accessRestrictions\":" + RestrictionsJson + "}", model)!;
        var r = RestrictionsOf(obj)!;
        Assert.Equal(PolicyAction.Deny, r.Domains!.DefaultPolicy);
        Assert.Equal(new List<string> { "example.com" }, r.Domains.Allow);
        Assert.Equal(PolicyAction.Allow, r.UserAgents!.DefaultPolicy);
    }

    [Theory]
    [MemberData(nameof(RestrictionModels))]
    public void AccessRestrictions_SerializesWireAliases(System.Type model)
    {
        var obj = JsonConvert.DeserializeObject("{\"accessRestrictions\":" + RestrictionsJson + "}", model)!;
        var json = JObject.Parse(Serialize(obj));
        Assert.Equal("deny", (string)json["accessRestrictions"]!["domains"]!["defaultPolicy"]!);
        Assert.NotNull(json["accessRestrictions"]!["userAgents"]);
    }

    [Theory]
    [MemberData(nameof(RestrictionModels))]
    public void AccessRestrictions_AbsentIsNullAndOmitted(System.Type model)
    {
        var obj = JsonConvert.DeserializeObject("{}", model)!;
        Assert.Null(RestrictionsOf(obj));
        Assert.Null(JObject.Parse(Serialize(obj))["accessRestrictions"]);
    }

    [Fact]
    public void PlaybackIdSuccessResponse_WithAndWithoutRestrictions()
    {
        var with = JsonConvert.DeserializeObject<PlaybackIdSuccessResponse>(
            "{\"success\":true,\"data\":{\"id\":\"p1\",\"accessPolicy\":\"public\",\"accessRestrictions\":" + RestrictionsJson + "}}")!;
        Assert.Equal("example.com", with.Data!.AccessRestrictions!.Domains!.Allow![0]);
        var without = JsonConvert.DeserializeObject<PlaybackIdSuccessResponse>("{\"success\":true,\"data\":{\"id\":\"p1\",\"accessPolicy\":\"public\"}}")!;
        Assert.Null(without.Data!.AccessRestrictions);
    }

    [Fact]
    public void LiveRestrictionBodies_SerializeFlatWithAllowDefault()
    {
        var d = JObject.Parse(Serialize(new UpdateLiveStreamDomainRestrictionsRequestBody { Allow = new() { "a.com" } }));
        Assert.Equal("allow", (string)d["defaultPolicy"]!);
        Assert.Equal("a.com", (string)d["allow"]![0]!);
        Assert.Null(d["deny"]);
        Assert.Null(d["domains"]);

        var u = JObject.Parse(Serialize(new UpdateLiveStreamUserAgentRestrictionsRequestBody { Deny = new() { "PostmanRuntime/7.29.0" } }));
        Assert.Equal("allow", (string)u["defaultPolicy"]!);
        Assert.Null(u["allow"]);
        Assert.Null(u["userAgents"]);
    }

    [Fact]
    public void LiveRestrictionRequests_CarryStreamAndPlaybackIdOnly()
    {
        foreach (var t in new[] { typeof(UpdateLiveStreamDomainRestrictionsRequest), typeof(UpdateLiveStreamUserAgentRestrictionsRequest) })
        {
            Assert.NotNull(t.GetProperty("StreamId"));
            Assert.NotNull(t.GetProperty("PlaybackId"));
            Assert.Null(t.GetProperty("MediaId"));
        }
    }

    [Fact]
    public void LiveRestrictionResponses_ParseEnvelope()
    {
        const string json = "{\"success\":true,\"data\":{\"defaultPolicy\":\"allow\",\"allow\":[\"yourdomain.com\"],\"deny\":[\"malicioussite.io\"]}}";
        var d = JsonConvert.DeserializeObject<UpdateLiveStreamDomainRestrictionsResponseBody>(json)!;
        Assert.True(d.Success);
        Assert.Equal("malicioussite.io", d.Data!.Deny![0]);
        var u = JsonConvert.DeserializeObject<UpdateLiveStreamUserAgentRestrictionsResponseBody>(json)!;
        Assert.Equal("yourdomain.com", u.Data!.Allow![0]);
    }

    [Fact]
    public void Surface_LiveAndOnDemandRestrictionMethodsExist()
    {
        Assert.NotNull(typeof(ILivePlayback).GetMethod("UpdateDomainRestrictionsAsync"));
        Assert.NotNull(typeof(ILivePlayback).GetMethod("UpdateUserAgentRestrictionsAsync"));
        Assert.NotNull(typeof(IPlayback).GetMethod("UpdateDomainRestrictionsAsync"));
        Assert.NotNull(typeof(IPlayback).GetMethod("UpdateUserAgentRestrictionsAsync"));
    }
}
