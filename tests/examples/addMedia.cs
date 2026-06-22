#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "1b92c0d6-5548-4642-b13e-4bb7d77dbaf4",
    Password = "ff32012b-ec02-40ca-b0d4-711d81537e73",
});

var res = await sdk.Playlists.AddMediaAsync(
    playlistId: "942a0e12-a857-4037-9433-634f6dcabb9c",
    body: new MediaIdsRequest() {
        MediaIds = new List<string>() {
            "5f2f46f8-ca41-44cd-8d92-8aab95ecbd0e",
            "f46cb48b-0b84-40a8-926d-d861ebf1ce00",
            "e70fc528-15e9-4239-b55d-81f620c70df3",
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