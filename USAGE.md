<!-- Start SDK Example Usage [usage] -->
```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

CreateLiveStreamRequest req = new CreateLiveStreamRequest() {
    PlaybackSettings = new PlaybackSettings() {},
    InputMediaSettings = new InputMediaSettings() {
        Metadata = new CreateLiveStreamRequestMetadata() {},
    },
};

var res = await sdk.StartLiveStream.CreateNewStreamAsync(req);

// handle response
```
<!-- End SDK Example Usage [usage] -->