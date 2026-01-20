# Videos

## Overview

### Available Operations

* [ListLiveClips](#listliveclips) - Get all clips of a live stream
* [Update](#update) - Update a media by ID
* [UpdateTrack](#updatetrack) - Update audio / subtitle track
* [GetSummary](#getsummary) - Get the summary of a video
* [GetInputInfo](#getinputinfo) - Get info of media inputs

## ListLiveClips

Retrieves a list of all media clips generated from a specific livestream. Each media entry includes metadata such as the clip media IDs, and other relevant details. A media clip is a segmented portion of an original media file (source live stream). Clips are often created for various purposes such as previews, highlights, or customized edits.
#### How it works
1. Provide the livestreamId as a parameter when calling this endpoint.

2. The API returns a paginated list of media clips created from the specified livestream.

3. Pagination helps maintain performance and usability when handling large sets of media files, making it easier to organize and manage content in bulk.

#### Use case
Suppose you’re hosting a live gaming event and want to showcase key moments from the stream — such as top plays or final match highlights. You can use this endpoint to fetch all clips generated from that livestream, display them in your dashboard, or use them for post-event editing and sharing.


Related guide: <a href="https://docs.fastpix.io/docs/instant-live-clipping">Instant live clipping</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="list-live-clips" method="get" path="/on-demand/{livestreamId}/live-clips" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Videos.ListLiveClipsAsync(
    livestreamId: "<livestreamId>",
    limit: 20,
    offset: 1,
    orderBy: SortOrder.Desc
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

| Parameter                                                                                 | Type                                                                                      | Required                                                                                  | Description                                                                               | Example                                                                                   |
| ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| `LivestreamId`                                                                            | *string*                                                                                  | :heavy_check_mark:                                                                        | The stream Id is unique identifier assigned to the live stream.                           | <livestreamId>                                                          |
| `Limit`                                                                                   | *long*                                                                                    | :heavy_minus_sign:                                                                        | Limit specifies the maximum number of items to display per page.                          | 20                                                                                        |
| `Offset`                                                                                  | *long*                                                                                    | :heavy_minus_sign:                                                                        | Offset determines the starting point for data retrieval within a paginated list.          | 1                                                                                         |
| `OrderBy`                                                                                 | [SortOrder](../../Models/Components/SortOrder.md)                                         | :heavy_minus_sign:                                                                        | The values in the list can be arranged in two ways: DESC (Descending) or ASC (Ascending). | desc                                                                                      |

### Response

**[ListLiveClipsResponse](../../Models/Requests/ListLiveClipsResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Update

This endpoint allows you to update specific parameters of an existing media file. You can modify the key-value pairs of the metadata that were provided in the payload during the creation of media from a URL or when uploading the media directly from device. 


#### How it works

1. Make a PATCH request to this endpoint. Replace `<mediaId>` with the unique ID (`uploadId` or `id`) of the media you received after uploading to FastPix

2. Include the updated parameters in the request body.

3. The response returns the updated media data, confirming the changes. 

4. Monitor the <a href="https://docs.fastpix.io/docs/media-events#videomediaupdated">video.media.updated</a> webhook event to track the update status in your system.

#### Example
If a user uploads a video and later needs to change the title, add a new description, or update tags, you can use this endpoint to update the media metadata without re-uploading the entire video.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="updated-media" method="patch" path="/on-demand/{mediaId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Videos.UpdateAsync(
    mediaId: "<mediaId>",
    body: new UpdatedMediaRequestBody() {
        Metadata = new Dictionary<string, string>() {
            { "user", "fastpix_admin" },
        },
        Title = "test title",
    }
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

| Parameter                                                                                 | Type                                                                                      | Required                                                                                  | Description                                                                               | Example                                                                                   |
| ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| `MediaId`                                                                                 | *string*                                                                                  | :heavy_check_mark:                                                                        | The unique identifier assigned to the media when created. The value must be a valid UUID. | <mediaId>                                                      |
| `Body`                                                                                    | [UpdatedMediaRequestBody](../../Models/Requests/UpdatedMediaRequestBody.md)               | :heavy_check_mark:                                                                        | N/A                                                                                       |                                                                                           |

### Response

**[UpdatedMediaResponse](../../Models/Requests/UpdatedMediaResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## UpdateTrack

This endpoint allows you to update an existing audio or subtitle track associated with a media file. When updating a track, you must provide the new track `url`, `languageName`, and `languageCode`, ensuring all three parameters are included in the request.


#### How it works

1. Send a PATCH request to this endpoint, replacing `{mediaId}` with the media ID, and `{trackId}` with the ID of the track you want to update.

2. Provide the necessary details in the request body.

3. Receive a response confirming the track update.

#### Webhook Events

After updating a track, your system must receive webhook notifications:

1. After successfully updating a track, your system must receive the webhook event <a href="https://docs.fastpix.io/docs/transform-media-events#videomediatrackupdated">video.media.track.updated</a>.

2. Once the new track is processed and ready, you must receive the webhook event <a href="https://docs.fastpix.io/docs/transform-media-events#videomediatrackready">video.media.track.ready</a>.

3. Once the media file is updated with the new track details, a <a href="https://docs.fastpix.io/docs/media-events#videomediaupdated">video.media.updated</a> event must be triggered.


#### Example
Suppose you previously added a French subtitle track to a video but now need to update it with a different file. By calling this API, you can replace the existing subtitle file (.vtt) with a new one while keeping the same track ID. This is useful when:

  - The original track file has errors and needs correction.
  - You want to improve subtitle translations or replace an audio track with a better-quality version.

Related guides: <a href="https://docs.fastpix.io/docs/manage-subtitle-tracks">Add own subtitle tracks</a>, <a href="https://docs.fastpix.io/docs/manage-audio-tracks">Add own audio tracks</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-media-track" method="patch" path="/on-demand/{mediaId}/tracks/{trackId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Videos.UpdateTrackAsync(
    trackId: "<trackId>",
    mediaId: "<mediaId>",
    body: new UpdateTrackRequest() {
        LanguageName = "french",
    }
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

| Parameter                                                                                 | Type                                                                                      | Required                                                                                  | Description                                                                               | Example                                                                                   |
| ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| `TrackId`                                                                                 | *string*                                                                                  | :heavy_check_mark:                                                                        | The unique identifier assigned to the media when created. The value must be a valid UUID. | <mediaId>                                                      |
| `MediaId`                                                                                 | *string*                                                                                  | :heavy_check_mark:                                                                        | The unique identifier assigned to the media when created. The value must be a valid UUID. | <mediaId>                                                      |
| `Body`                                                                                    | [UpdateTrackRequest](../../Models/Components/UpdateTrackRequest.md)                       | :heavy_check_mark:                                                                        | N/A                                                                                       |                                                                                           |

### Response

**[UpdateMediaTrackResponse](../../Models/Requests/UpdateMediaTrackResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## GetSummary

This endpoint returns the generated summary of a video.  

The summary is created using the **InVideo Summary** feature, which processes the video content and produces a textual summary.  

To use this endpoint, you must first generate the video summary using the Generate Video Summary endpoint. This endpoint can return the summary only after that process is complete. 

Typical use cases include:  
- Providing viewers with a quick preview of the video's main content.  
- Enabling search or recommendation systems to surface summarized insights.  
- Supporting accessibility and content discovery without requiring users to watch the full video.  

If the summary has not been generated or the feature is disabled for the requested media, the endpoint returns an error indicating that the summary is unavailable. 


### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-media-summary" method="get" path="/on-demand/{mediaId}/summary" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Videos.GetSummaryAsync(mediaId: "<mediaId>");

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

| Parameter                                                                                 | Type                                                                                      | Required                                                                                  | Description                                                                               | Example                                                                                   |
| ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| `MediaId`                                                                                 | *string*                                                                                  | :heavy_check_mark:                                                                        | The unique identifier assigned to the media when created. The value must be a valid UUID. | <mediaId>                                                      |

### Response

**[GetMediaSummaryResponse](../../Models/Requests/GetMediaSummaryResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## GetInputInfo

This endpoint lets you retrieve detailed information about the media inputs associated with a specific media item. You can use it to verify the media file’s input URL, track its creation status, and check its container format. You must provide the mediaId (either the uploadId or the id) to fetch this information.


#### How it works

Upon making a `GET` request with the mediaId, FastPix returns a response with: 

* The public storage input `url` of the uploaded media file. 

* Information about the media’s video and audio tracks, including whether they were successfully created.

* The container format of the uploaded media file (for example, MP4, MKV).

This endpoint is particularly useful for ensuring that all necessary tracks (video and audio) have been correctly associated with the media during the upload or media creation process.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="retrieveMediaInputInfo" method="get" path="/on-demand/{mediaId}/input-info" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Videos.GetInputInfoAsync(mediaId: "<mediaId>");

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

| Parameter                                                                                 | Type                                                                                      | Required                                                                                  | Description                                                                               | Example                                                                                   |
| ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| `MediaId`                                                                                 | *string*                                                                                  | :heavy_check_mark:                                                                        | Pass the list of the input objects used to create the media, along with applied settings. | <mediaId>                                                      |

### Response

**[RetrieveMediaInputInfoResponse](../../Models/Requests/RetrieveMediaInputInfoResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |