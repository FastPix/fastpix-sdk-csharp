#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Fastpix.Utils.Retries;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "1b92c0d6-5548-4642-b13e-4bb7d77dbaf4",
    Password = "ff32012b-ec02-40ca-b0d4-711d81537e73",
});

// NOTE: user-agent restrictions can only be updated once the playback ID is
// "available". A playback ID is "available" only after its media reaches the
// "Ready" status. Use a playback ID belonging to a Ready media (the values
// below were created from a Ready media). A freshly created playback ID can
// stay "preparing" for up to ~2 minutes, so we retry on that 400 below.
var mediaId = "28da13b6-7e27-4ea7-a971-13c7c6fdc917";
var playbackId = "3c8b5171-4537-4261-bb47-177eedb4fa21";

UpdateUserAgentRestrictionsResponse? res = null;
for (int attempt = 1; attempt <= 24; attempt++) // ~2 min at 5s intervals
{
    try
    {
        res = await sdk.Playback.UpdateUserAgentRestrictionsAsync(
            mediaId: mediaId,
            playbackId: playbackId,
            body: new UpdateUserAgentRestrictionsRequestBody() {
                Allow = new List<string>() {
                    "Mozilla/55.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36",
                },
                Deny = new List<string>() {
                    "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/53745.36 (KHTML, like Gecko) Chrome/138.0.0.0 Mobile Safari/537.36",
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
