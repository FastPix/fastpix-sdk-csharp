#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "1b92c0d6-5548-4642-b13e-4bb7d77dbaf4",
    Password = "ff32012b-ec02-40ca-b0d4-711d81537e73",
});

var res = await sdk.Playlists.DeleteAsync(playlistId: "2b2ce02d-54f8-47f6-b21d-9bec5096eb42");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PlaylistDeleteResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);