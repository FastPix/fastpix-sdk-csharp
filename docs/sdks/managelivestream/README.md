# ManageLiveStream
(*ManageLiveStream*)

## Overview

### Available Operations

* [GetAllStreams](#getallstreams) - Get all live streams
* [GetLiveStreamById](#getlivestreambyid) - Get stream by ID
* [DeleteLiveStream](#deletelivestream) - Delete a stream
* [UpdateLiveStream](#updatelivestream) - Update a stream

## GetAllStreams

Retrieves a list of all live streams associated with the user’s account (workspace). It provides an overview of both current and past live streams, including details like streamId, title, status, and creation time. 

### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageLiveStream.GetAllStreamsAsync(
    limit: "20",
    offset: "1",
    orderBy: GetAllStreamsOrderBy.Desc
);

// handle response
```

### Parameters

| Parameter                                                                                                                           | Type                                                                                                                                | Required                                                                                                                            | Description                                                                                                                         | Example                                                                                                                             |
| ----------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| `Limit`                                                                                                                             | *string*                                                                                                                            | :heavy_minus_sign:                                                                                                                  | Limit specifies the maximum number of items to display per page.                                                                    | 20                                                                                                                                  |
| `Offset`                                                                                                                            | *string*                                                                                                                            | :heavy_minus_sign:                                                                                                                  | Offset determines the starting point for data retrieval within a paginated list.                                                    | 1                                                                                                                                   |
| `OrderBy`                                                                                                                           | [GetAllStreamsOrderBy](../../Models/Requests/GetAllStreamsOrderBy.md)                                                               | :heavy_minus_sign:                                                                                                                  | The list of value can be order in two ways DESC (Descending) or ASC (Ascending). In case not specified, by default it will be DESC. | desc                                                                                                                                |

### Response

**[GetAllStreamsResponse](../../Models/Requests/GetAllStreamsResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.UnauthorizedException      | 401                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 403                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## GetLiveStreamById

This endpoint retrieves detailed information about a specific live stream by its unique streamId. It includes data such as the stream’s status (idle, preparing, active, disabled), metadata (title, description), and more. 

  **Practical example:** Suppose a news agency is broadcasting a live event and wants to track the configurations set for the live stream while also checking the stream's status.

### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageLiveStream.GetLiveStreamByIdAsync(streamId: "61a264dcc447b63da6fb79ef925cd76d");

// handle response
```

### Parameters

| Parameter                                                                           | Type                                                                                | Required                                                                            | Description                                                                         | Example                                                                             |
| ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| `StreamId`                                                                          | *string*                                                                            | :heavy_check_mark:                                                                  | Upon creating a new live stream, FastPix assigns a unique identifier to the stream. | 61a264dcc447b63da6fb79ef925cd76d                                                    |

### Response

**[GetLiveStreamByIdResponse](../../Models/Requests/GetLiveStreamByIdResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.UnauthorizedException      | 401                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 403                                              | application/json                                 |
| Fastpix.Models.Errors.NotFoundError              | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## DeleteLiveStream

Permanently removes a specified live stream from the workspace. If the stream is still active, the encoder will be disconnected, and the ingestion will stop. This action cannot be undone, and any future playback attempts will fail. 

  By providing the streamId, the API will terminate any active connections to the stream and remove it from the list of available live streams. You can further look for video.live_stream.deleted webhook to notify your system about the status. 

  **Example:** For an online concert platform, a trial stream was mistakenly made public. The event manager deletes the stream before the concert begins to avoid confusion among viewers. 

### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageLiveStream.DeleteLiveStreamAsync(streamId: "8717422d89288ad5958d4a86e9afe2a2");

// handle response
```

### Parameters

| Parameter                                                                           | Type                                                                                | Required                                                                            | Description                                                                         | Example                                                                             |
| ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| `StreamId`                                                                          | *string*                                                                            | :heavy_check_mark:                                                                  | Upon creating a new live stream, FastPix assigns a unique identifier to the stream. | 8717422d89288ad5958d4a86e9afe2a2                                                    |

### Response

**[DeleteLiveStreamResponse](../../Models/Requests/DeleteLiveStreamResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.UnauthorizedException      | 401                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 403                                              | application/json                                 |
| Fastpix.Models.Errors.NotFoundError              | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## UpdateLiveStream

This endpoint allows users to modify the parameters of an existing live stream, such as its metadata (title, description) or the reconnect window. It’s useful for making changes to a stream that has already been created but not yet ended. Once the live stream is disabled, you cannot update a stream. 


  The updated stream parameters and the streamId needs to be shared in the request, and FastPix will return the updated stream details. Once updated, video.live_stream.updated webhook event notifies your system. 

  **Practical example:** A host realizes they need to extend the reconnect window for their live stream in case they lose connection temporarily during the event. Or suppose during a multi-day online conference, the event organizers need to update the stream title to reflect the next day's session while keeping the same stream ID for continuity. 

### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageLiveStream.UpdateLiveStreamAsync(
    streamId: "91a264dcc447b63da6fb79ef925cd76d",
    patchLiveStreamRequest: new PatchLiveStreamRequest() {
        Metadata = new PatchLiveStreamRequestMetadata() {},
    }
);

// handle response
```

### Parameters

| Parameter                                                                           | Type                                                                                | Required                                                                            | Description                                                                         | Example                                                                             |
| ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| `StreamId`                                                                          | *string*                                                                            | :heavy_check_mark:                                                                  | Upon creating a new live stream, FastPix assigns a unique identifier to the stream. | 91a264dcc447b63da6fb79ef925cd76d                                                    |
| `PatchLiveStreamRequest`                                                            | [PatchLiveStreamRequest](../../Models/Components/PatchLiveStreamRequest.md)         | :heavy_minus_sign:                                                                  | N/A                                                                                 | {<br/>"metadata": {<br/>"livestream_name": "Gaming_stream"<br/>},<br/>"reconnectWindow": 100<br/>} |

### Response

**[UpdateLiveStreamResponse](../../Models/Requests/UpdateLiveStreamResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.UnauthorizedException      | 401                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 403                                              | application/json                                 |
| Fastpix.Models.Errors.NotFoundError              | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |