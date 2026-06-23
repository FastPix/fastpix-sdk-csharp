#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.LiveStreams.DeleteAsync(streamId: "your-stream-id");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.LiveStreamDeleteResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);