<!-- Start SDK Example Usage [usage] -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
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
```
<!-- End SDK Example Usage [usage] -->