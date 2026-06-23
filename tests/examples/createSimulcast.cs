#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Simulcasts.CreateAsync(
    streamId: "your-stream-id",
    body: new SimulcastRequest() {
        Url = "rtmp://a.rtmp.youtube.com/live2",
        StreamKey = "abcd-efgh-ijkl-mnop-qrst",
        Metadata = new Dictionary<string, string>() {
            { "livestream_name", "<livestream_name>" },
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.SimulcastResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);