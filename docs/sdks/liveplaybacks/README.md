# LivePlaybacks
(*LivePlaybacks*)

## Overview

### Available Operations

* [Get](#get) - Get playbackId details

## Get

Retrieves details about a previously created playback ID. If you provide the distinct `playbackId` that was given back to you in the previous stream or <a href="https://docs.fastpix.io/reference/create-playbackid-of-stream">create playbackId</a> request, FastPix will provide the relevant playback details such as the access policy. 

#### Example
A developer needs to confirm the access policy of the playback ID to ensure whether the stream is public or private for viewers.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-live-stream-playback-id" method="get" path="/live/streams/{streamId}/playback-ids/{playbackId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.LivePlaybacks.GetAsync(
    streamId: "61a264dcc447b63da6fb79ef925cd76d",
    playbackId: "61a264dcc447b63da6fb79ef925cd76d"
);

// handle response
```

### Parameters

| Parameter                                                                            | Type                                                                                 | Required                                                                             | Description                                                                          | Example                                                                              |
| ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------ |
| `StreamId`                                                                           | *string*                                                                             | :heavy_check_mark:                                                                   | Upon creating a new live stream, FastPix assigns a unique identifier to the stream.  | 61a264dcc447b63da6fb79ef925cd76d                                                     |
| `PlaybackId`                                                                         | *string*                                                                             | :heavy_check_mark:                                                                   | Upon creating a new playbackId, FastPix assigns a unique identifier to the playback. | 61a264dcc447b63da6fb79ef925cd76d                                                     |

### Response

**[GetLiveStreamPlaybackIdResponse](../../Models/Requests/GetLiveStreamPlaybackIdResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.UnauthorizedException      | 401                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 403                                              | application/json                                 |
| Fastpix.Models.Errors.NotFoundErrorPlaybackId    | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |