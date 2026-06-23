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

var res = await sdk.Playlists.AddMediaAsync(
    playlistId: "your-playlist-id",
    body: new MediaIdsRequest() {
        MediaIds = new List<string>() {
            "your-media-id-1",
            "your-media-id-2",
            "your-media-id-3",
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PlaylistByIdResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);