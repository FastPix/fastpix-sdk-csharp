#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "1b92c0d6-5548-4642-b13e-4bb7d77dbaf4",
    Password = "ff32012b-ec02-40ca-b0d4-711d81537e73",
});

var res = await sdk.SigningKeys.DeleteAsync(signingKeyId: "28f3c0f3-2a8f-4dc5-aa2e-c6a7a9dad454");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.DeleteSigningKeyResponseValue,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);