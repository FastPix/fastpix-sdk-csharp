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