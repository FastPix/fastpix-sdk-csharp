#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreatePullVideoInput(
            new PullVideoInput() {}
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "<key>", "<value>" },
    },
};

var res = await sdk.InputVideo.CreateMediaAsync(req);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.CreateMediaSuccessResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);