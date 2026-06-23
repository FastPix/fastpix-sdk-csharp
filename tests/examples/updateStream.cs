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

var res = await sdk.Streams.UpdateAsync(
    streamId: "your-stream-id",
    body: new PatchLiveStreamRequest() {
        Metadata = new Dictionary<string, string>() {
            { "livestream_name", "Gaming_stream" },
        },
        ReconnectWindow = 100,
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PatchResponseDto,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);