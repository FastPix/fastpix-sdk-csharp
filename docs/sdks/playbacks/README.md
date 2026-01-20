# Playbacks

## Overview

### Available Operations

* [Get](#get) - Get a playback ID

## Get

This endpoint retrieves details about a specific playback ID associated with a media asset. Use it to check the access policy for that specific playback ID, such as whether it is public or private.

**How it works:**
1. Make a GET request to the endpoint, replacing `{mediaId}` with the media ID and `{playbackId}` with the playback ID.
2. This request is useful for auditing or validation before granting playback access in your application.

**Example:**
A media platform might use this endpoint to verify if a playback ID is public or private before embedding the video in a frontend player or allowing access to a restricted group.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-playback-id" method="get" path="/on-demand/{mediaId}/playback-ids/{playbackId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playbacks.GetAsync(
    mediaId: "<mediaId>",
    playbackId: "<playbackId>"
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                            | Type                                 | Required                             | Description                          | Example                              |
| ------------------------------------ | ------------------------------------ | ------------------------------------ | ------------------------------------ | ------------------------------------ |
| `MediaId`                            | *string*                             | :heavy_check_mark:                   | N/A                                  | <mediaId> or <playbackId> |
| `PlaybackId`                         | *string*                             | :heavy_check_mark:                   | N/A                                  | <mediaId> or <playbackId> |

### Response

**[GetPlaybackIdResponse](../../Models/Requests/GetPlaybackIdResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |