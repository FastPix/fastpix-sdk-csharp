#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Fastpix.Utils.Retries;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

// NOTE: domain restrictions can only be updated once the playback ID is
// "available". A playback ID is "available" only after its media reaches the
// "Ready" status. Use a playback ID belonging to a Ready media (the values
// below were created from a Ready media). A freshly created playback ID can
// stay "preparing" for up to ~2 minutes, so we retry on that 400 below.
var mediaId = "your-media-id";
var playbackId = "your-playback-id";

UpdateDomainRestrictionsResponse? res = null;
for (int attempt = 1; attempt <= 24; attempt++) // ~2 min at 5s intervals
{
    try
    {
        res = await sdk.Playback.UpdateDomainRestrictionsAsync(
            mediaId: mediaId,
            playbackId: playbackId,
            body: new UpdateDomainRestrictionsRequestBody() {
                Allow = new List<string>() {
                    "yourdomain.com",
                    "sampledomain.com",
                },
                Deny = new List<string>() {
                    "yourworkdomain.com",
                },
            },
            // SDK-level retries for transient (connection / 5xx) errors.
            retryConfig: new RetryConfig(
                strategy: RetryConfig.RetryStrategy.BACKOFF,
                backoff: new BackoffStrategy(
                    initialIntervalMs: 1000L,
                    maxIntervalMs: 10000L,
                    maxElapsedTimeMs: 30000L,
                    exponent: 1.5
                ),
                retryConnectionErrors: false
            )
        );
        break;
    }
    // A 400 "not ready for updates" is NOT retried by the SDK, so handle it
    // here while the playback ID transitions from "preparing" to "available".
    catch (Exception ex) when (ex.Message.Contains("not ready for updates"))
    {
        Console.WriteLine($"attempt {attempt}: playback ID still preparing, retrying in 5s...");
        await Task.Delay(5000);
    }
}

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res?.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
