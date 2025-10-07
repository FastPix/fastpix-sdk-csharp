<!-- Start SDK Example Usage [usage] -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreateVideoInput(
            new VideoInput() {
                Type = "video",
                Url = "https://static.fastpix.io/sample.mp4",
            }
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "key1", "value1" },
    },
    AccessPolicy = CreateMediaRequestAccessPolicy.Public,
};

var res = await sdk.Videos.CreateFromUrlAsync(req);

// handle response
```
<!-- End SDK Example Usage [usage] -->