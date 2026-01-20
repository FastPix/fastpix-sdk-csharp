# ManageLiveStream

## Overview

### Available Operations

* [Enable](#enable) - Enable a stream
* [Disable](#disable) - Disable a stream

## Enable

This endpoint allows you to enable a livestream by transitioning its status from `disabled` to `idle`. After it is enabled, the stream becomes available and ready to accept an incoming broadcast from a streaming tool.

Streams on the trial plan cannot be re-enabled if they are in the `disabled` state.

The `livestreamId` must be provided in the path, and the stream must not already be in an enabled state (`idle`, `preparing`, or `active`).

#### Example

A creator disables a livestream to pause it temporarily. Later, they decide to continue the session. By calling this endpoint with the stream's ID, they can re-enable and restart the same livestream.

Related guide <a href="https://docs.fastpix.io/docs/manage-streams">Manage streams</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="enable-live-stream" method="put" path="/live/streams/{streamId}/live-enable" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.ManageLiveStream.EnableAsync(streamId: "<streamId>");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.LiveStreamDeleteResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                           | Type                                                                                | Required                                                                            | Description                                                                         | Example                                                                             |
| ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| `StreamId`                                                                          | *string*                                                                            | :heavy_check_mark:                                                                  | Upon creating a new live stream, FastPix assigns a unique identifier to the stream. | <streamId>                                                    |

### Response

**[EnableLiveStreamResponse](../../Models/Requests/EnableLiveStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Disable

This endpoint disables a livestream by setting its status to `disabled`. Use this to stop a livestream when it's no longer needed or must be taken offline intentionally.

A disabled stream can later be re-enabled using the enable endpoint — however, if you're on a trial plan, re-enabling is not allowed once the stream is disabled.

#### Example

A speaker finishes their live session and wants to prevent the stream from being mistakenly started again. By calling this endpoint, the stream is transitioned to a `disabled` state, ensuring it's permanently stopped (unless re-enabled on a paid plan).

Related guide <a href="https://docs.fastpix.io/docs/manage-streams">Manage streams</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="disable-live-stream" method="put" path="/live/streams/{streamId}/live-disable" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.ManageLiveStream.DisableAsync(streamId: "<streamId>");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.LiveStreamDeleteResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                            | Type                                                                                 | Required                                                                             | Description                                                                          | Example                                                                              |
| ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------ |
| `StreamId`                                                                           | *string*                                                                             | :heavy_check_mark:                                                                   | After creating a new live stream, FastPix assigns a unique identifier to the stream. | <streamId>                                                     |

### Response

**[DisableLiveStreamResponse](../../Models/Requests/DisableLiveStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |