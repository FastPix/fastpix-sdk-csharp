#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "1b92c0d6-5548-4642-b13e-4bb7d77dbaf4",
    Password = "ff32012b-ec02-40ca-b0d4-711d81537e73",
});

var res = await sdk.Tracks.DeleteAsync(
    mediaId: "e28dbc92-2c94-43b1-a06b-cbe59ce26f49",
    trackId: "81fef37c-880d-45e7-9797-f6c1e4d653d2"
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);