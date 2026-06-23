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

var res = await sdk.Simulcasts.UpdateAsync(
    streamId: "your-stream-id",
    simulcastId: "your-simulcast-id",
    body: new SimulcastUpdateRequest() {
        Metadata = new Dictionary<string, string>() {
            { "simulcast_name", "<simulcast_name>" },
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.SimulcastUpdateResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);