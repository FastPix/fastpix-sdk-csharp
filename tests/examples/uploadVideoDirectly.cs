#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

DirectUploadVideoMediaRequest req = new DirectUploadVideoMediaRequest() {
    PushMediaSettings = new PushMediaSettings() {
        Metadata = new Dictionary<string, string>() {
            { "<key>", "<value>" },
        },
    },
};

var res = await sdk.InputVideo.UploadAsync(req);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);