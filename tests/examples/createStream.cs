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

CreateLiveStreamRequest req = new CreateLiveStreamRequest() {
    PlaybackSettings = new PlaybackSettings() {},
    InputMediaSettings = new InputMediaSettings() {
        Metadata = new Dictionary<string, string>() {
            { "livestream_name", "fastpix_livestream" },
        },
    },
};

var res = await sdk.LiveStreams.CreateAsync(req);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.LiveStreamResponseDto,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);