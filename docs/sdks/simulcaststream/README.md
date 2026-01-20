# SimulcastStream

## Overview

### Available Operations

* [GetSpecific](#getspecific) - Get a specific simulcast

## GetSpecific

Retrieves the details of a specific simulcast associated with a parent live stream. By providing both the `streamId` of the parent stream and the `simulcastId`, FastPix returns detailed information about the simulcast, such as the stream URL, the status of the simulcast, and metadata. 

#### Example
This endpoint can be used to verify the status of the simulcast on external platforms before the live stream begins. For example, before starting a live gaming event, the organizer wants to ensure that the simulcast to Twitch is set up correctly. They retrieve the simulcast information to confirm that everything is properly configured.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-specific-simulcast-of-stream" method="get" path="/live/streams/{streamId}/simulcast/{simulcastId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.SimulcastStream.GetSpecificAsync(
    streamId: "<streamId>",
    simulcastId: "<simulcastId>"
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.SimulcastResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                                                      | Type                                                                                                                           | Required                                                                                                                       | Description                                                                                                                    | Example                                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ |
| `StreamId`                                                                                                                     | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | After creating a new live stream, FastPix assigns a unique identifier to the stream.                                           | <streamId> or <simulcastId>                                                                                               |
| `SimulcastId`                                                                                                                  | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | When you create the new simulcast, FastPix assign a universal unique identifier which can contain a maximum of 255 characters. | <streamId> or <simulcastId>                                                                                               |

### Response

**[GetSpecificSimulcastOfStreamResponse](../../Models/Requests/GetSpecificSimulcastOfStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |