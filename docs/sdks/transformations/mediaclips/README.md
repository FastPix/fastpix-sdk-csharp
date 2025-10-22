# Media Clips

Retrieve and manage media clips created from your source content.

## Overview

### Available Operations

* [ListMediaClips](#listmediaclips) - Get all clips of a media
* [ListLiveClips](#listliveclips) - Get all clips of a live stream

## ListMediaClips

Retrieves a list of all media clips generated from a specific media source. Each media entry includes metadata such as the clip media IDs, and other relevant details. A media clip is a segmented portion of an original media file. Clips are often created for various purposes such as previews, highlights, or customized edits.

#### How it works
To use this endpoint, provide the `sourceMediaId` as a parameter. The API then returns a paginated list of clipped media items created from that source media. Pagination ensures optimal performance and usability when dealing with a large number of media files, making it easier to organize and manage content in bulk.

Related guide: <a href="https://docs.fastpix.io/docs/create-clips-from-existing-media">Create clips from existing media</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="list-media-clips" method="get" path="/on-demand/{sourceMediaId}/clips" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.ManageVideos.ListMediaClipsAsync(
    sourceMediaId: "b6f71268143f70c798a7851a0a92dcbf",
    limit: 20,
    offset: 1,
    orderBy: SortOrder.Desc
);

Console.WriteLine(JsonConvert.SerializeObject(res.MediaClipResponse, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `sourceMediaId` | string | Yes | The ID of the source media to get clips for |
| `limit` | long? | No | Maximum number of clips to return (default: 10) |
| `offset` | long? | No | Number of clips to skip (default: 1) |
| `orderBy` | SortOrder? | No | Sort order for results (default: Desc) |

### Response

**[MediaClipResponse](../../../Models/Components/MediaClipResponse.md)**

### Error Handling

This operation may throw the following exceptions:

- `MediaNotFoundException`: If the source media ID is not found
- `UnauthorizedException`: If authentication fails
- `ForbiddenException`: If access is denied

### Example Response

```json
{
  "clips": [
    {
      "id": "clip_123",
      "sourceMediaId": "b6f71268143f70c798a7851a0a92dcbf",
      "startTime": 10.5,
      "endTime": 30.2,
      "createdAt": "2023-01-01T00:00:00Z",
      "status": "ready"
    }
  ],
  "total": 1,
  "limit": 20,
  "offset": 1
}
```

## ListLiveClips

Retrieves a list of all media clips generated from a specific livestream. Each media entry includes metadata such as the clip media IDs, and other relevant details. A media clip is a segmented portion of an original media file (source live stream). Clips are often created for various purposes such as previews, highlights, or customized edits.

#### How it works
To use this endpoint, provide the `livestreamId` as a parameter. The API then returns a paginated list of clipped media items created from that livestream. Pagination ensures optimal performance and usability when dealing with a large number of media files, making it easier to organize and manage content in bulk.

Related guide: <a href="https://docs.fastpix.io/docs/instant-live-clipping">Instant live clipping</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="list-live-clips" method="get" path="/on-demand/{livestreamId}/live-clips" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.ManageVideos.ListLiveClipsAsync(
    livestreamId: "b6f71268143f70c798a7851a0a92dcbf",
    limit: 20,
    offset: 1,
    orderBy: SortOrder.Desc
);

Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter                                                                                 | Type                                                                                      | Required                                                                                  | Description                                                                               | Example                                                                                   |
| ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| `LivestreamId`                                                                            | *string*                                                                                  | :heavy_check_mark:                                                                        | The stream Id is unique identifier assigned to the live stream.                           | b6f71268143f70c798a7851a0a92dcbf                                                          |
| `Limit`                                                                                   | *long*                                                                                    | :heavy_minus_sign:                                                                        | Limit specifies the maximum number of items to display per page.                          | 20                                                                                        |
| `Offset`                                                                                  | *long*                                                                                    | :heavy_minus_sign:                                                                        | Offset determines the starting point for data retrieval within a paginated list.          | 1                                                                                         |
| `OrderBy`                                                                                 | [SortOrder](../../../../Models/Components/SortOrder.md)                                         | :heavy_minus_sign:                                                                        | The values in the list can be arranged in two ways: DESC (Descending) or ASC (Ascending). | desc                                                                                      |

### Response

**[ListLiveClipsResponse](../../../Models/Requests/ListLiveClipsResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | */*                                            |
