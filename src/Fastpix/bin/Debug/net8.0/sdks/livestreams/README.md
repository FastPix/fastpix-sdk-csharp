# LiveStreams

## Overview

### Available Operations

* [Create](#create) - Create a new stream
* [GetAll](#getall) - Get all live streams
* [GetViewerCount](#getviewercount) - Get stream views by ID
* [GetById](#getbyid) - Get stream by ID
* [Delete](#delete) - Delete a stream
* [Complete](#complete) - Complete a stream

## Create

Creates a new <a href="https://docs.fastpix.io/docs/get-started-with-live-streaming">RTMPS</a> or <a href="https://docs.fastpix.io/docs/using-srt-to-live-stream">SRT</a> live stream in FastPix. When you create a stream, FastPix generates a unique `streamKey` and `srtSecret` that you can use with broadcasting software such as OBS to connect to FastPix RTMPS or SRT servers. Use SRT for live streaming in unstable network conditions, as it provides error correction and encryption for a more reliable and secure broadcast.

Leverage SRT for live streaming in environments with unstable networks, taking advantage of its error correction and encryption features for a resilient and secure broadcast. 

<h4>How it works</h4> 

1. Send a `POST` request to this endpoint. You can configure the stream settings, including `metadata` (such as stream name and description), `reconnectWindow` (in case of disconnection), and privacy options (`public` or `private`). 

2. FastPix returns the stream details for both RTMPS and SRT configurations. These keys and IDs from the stream details are essential for connecting the broadcasting software to FastPix’s servers and transmitting the live stream to viewers.

3. After the live stream is created, FastPix sends a `POST` request to your specified webhook endpoint with the event <a href="https://docs.fastpix.io/docs/live-events#videolive_streamcreated">video.live_stream.created</a>.


**Example:**


  Imagine a gaming platform that allows users to live stream gameplay directly from their dashboard. The API creates a new stream, provides the necessary stream key, and sets it to "private" so that only specific viewers can access it. 


Related guide: <a href="https://docs.fastpix.io/docs/how-to-livestream">How to live stream</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="create-new-stream" method="post" path="/live/streams" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

CreateLiveStreamRequest req = new CreateLiveStreamRequest() {
    PlaybackSettings = new PlaybackSettings() {},
    InputMediaSettings = new InputMediaSettings() {
        Metadata = new Dictionary<string, string>() {
            { "livestream_name", "fastpix_livestream" },
        },
    },
};

var res = await sdk.LiveStreams.CreateAsync(req);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.LiveStreamResponseDTO,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                     | Type                                                                          | Required                                                                      | Description                                                                   |
| ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- |
| `request`                                                                     | [CreateLiveStreamRequest](../../Models/Components/CreateLiveStreamRequest.md) | :heavy_check_mark:                                                            | The request object to use for the request.                                    |

### Response

**[CreateNewStreamResponse](../../Models/Requests/CreateNewStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## GetAll

Retrieves a list of all live streams associated with the current workspace. It provides an overview of both current and past live streams, including details like `streamId`, `metadata`, `status`, `createdAt` and more.


#### How it works

Use the access token and secret key related to the workspace in the request header. When called, the API provides a paginated response containing all the live streams in that specific workspace. This is helpful for retrieving a large volume of streams and managing content in bulk.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-all-streams" method="get" path="/live/streams" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.LiveStreams.GetAllAsync(
    limit: 20,
    offset: 1,
    orderBy: OrderBy.Desc
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.GetStreamsResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                                                           | Type                                                                                                                                | Required                                                                                                                            | Description                                                                                                                         | Example                                                                                                                             |
| ----------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| `Limit`                                                                                                                             | *long*                                                                                                                              | :heavy_minus_sign:                                                                                                                  | Limit specifies the maximum number of items to display per page.                                                                    | 20                                                                                                                                  |
| `Offset`                                                                                                                            | *long*                                                                                                                              | :heavy_minus_sign:                                                                                                                  | Offset determines the starting point for data retrieval within a paginated list.                                                    | 1                                                                                                                                   |
| `OrderBy`                                                                                                                           | [OrderBy](../../Models/Requests/OrderBy.md)                                                                                         | :heavy_minus_sign:                                                                                                                  | The list of value can be order in two ways DESC (Descending) or ASC (Ascending). In case not specified, by default it will be DESC. | desc                                                                                                                                |

### Response

**[GetAllStreamsResponse](../../Models/Requests/GetAllStreamsResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## GetViewerCount

This endpoint retrieves the current number of viewers watching a specific live stream, identified by its unique `streamId`.

The viewer count is an **approximate value**, optimized for performance. It provides a near-real-time estimate of how many clients are actively watching the stream. This approach ensures high efficiency, especially when the stream is being watched at large scale across multiple devices or platforms.

#### Example

Suppose a content creator is hosting a live concert and wants to display the number of live viewers on their dashboard. This endpoint can be queried to show up-to-date viewer statistics.

Related guide: <a href="https://docs.fastpix.io/docs/manage-streams">Manage streams</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-live-stream-viewer-count-by-id" method="get" path="/live/streams/{streamId}/viewer-count" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.LiveStreams.GetViewerCountAsync(streamId: "<streamId>");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.ViewsCountResponse,
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

**[GetLiveStreamViewerCountByIdResponse](../../Models/Requests/GetLiveStreamViewerCountByIdResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## GetById

This endpoint retrieves details about a specific live stream by its unique `streamId`. It includes data such as the stream’s `status` (idle, preparing, active, disabled), `metadata` (title, description), and more. 
#### Example

  Suppose a news agency is broadcasting a live event and wants to track the configurations set for the live stream while also checking the stream's status.


Related guide: <a href="https://docs.fastpix.io/docs/manage-streams">Manage streams</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-live-stream-by-id" method="get" path="/live/streams/{streamId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.LiveStreams.GetByIdAsync(streamId: "<streamId>");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.LivestreamgetResponse,
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

**[GetLiveStreamByIdResponse](../../Models/Requests/GetLiveStreamByIdResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Delete

Permanently deletes a specified live stream from the workspace. If the stream is active, the encoder is disconnected and ingestion stops immediately. This action is irreversible, and any future playback attempts fail as a result.

  Provide the `streamId` in the request to terminate active connections and remove the stream from the workspace. You can further look for <a href="https://docs.fastpix.io/docs/live-events#videolive_streamdeleted">video.live_stream.deleted</a> webhook to notify your system about the status.

  #### Example

  For an online concert platform, a trial stream was mistakenly made public. The event manager deletes the stream before the concert begins to avoid confusion among viewers. 


  Related guide: <a href="https://docs.fastpix.io/docs/manage-streams">Manage streams</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="delete-live-stream" method="delete" path="/live/streams/{streamId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.LiveStreams.DeleteAsync(streamId: "<streamId>");

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

**[DeleteLiveStreamResponse](../../Models/Requests/DeleteLiveStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Complete

This endpoint marks a livestream as completed by stopping the active stream and transitioning its status to `idle`. It is typically used after a livestream session has ended.

This operation only works when the stream is in the `active` state.

Completing a stream can help finalize the session and trigger post-processing events like VOD generation.

#### Example

A virtual event ends, and the system or host needs to close the livestream to prevent further streaming. This endpoint ensures the livestream status is changed from `active` to `idle`, indicating it's officially completed.

Related guide <a href="https://docs.fastpix.io/docs/manage-streams">Manage streams</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="complete-live-stream" method="put" path="/live/streams/{streamId}/finish" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.LiveStreams.CompleteAsync(streamId: "<streamId>");

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

**[CompleteLiveStreamResponse](../../Models/Requests/CompleteLiveStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |