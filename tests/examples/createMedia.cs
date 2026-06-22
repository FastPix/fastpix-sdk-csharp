#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "1b92c0d6-5548-4642-b13e-4bb7d77dbaf4",
    Password = "ff32012b-ec02-40ca-b0d4-711d81537e73",
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