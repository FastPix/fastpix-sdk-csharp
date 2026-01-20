# Moderations

## Overview

### Available Operations

* [Update](#update) - Enable video moderation

## Update

This endpoint enables moderation features, such as NSFW and profanity filtering, to detect inappropriate content in existing media.

#### How it works
1. Make a `PATCH` request to this endpoint, replacing `<mediaId>` with the ID of the media you want to update.
2. Include the `moderation` object and provide the requried `type` parameter in the request body to specify the media type (for example, video/audio/av).
4. The response contains the updated media data, confirming the changes made.

You can use the <a href="https://docs.fastpix.io/docs/ai-events#videomediaaimoderationready">video.mediaAI.moderation.ready</a> webhook event to track and notify about the detected moderation results.

**Use case:** This is particularly useful when a user uploads a video and later decides to enable moderation detection without the need to re-upload it.

Related guide: <a href="https://docs.fastpix.io/docs/using-nsfw-and-profanity-filter-for-video-moderation">Moderate NSFW & Profanity</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-media-moderation" method="patch" path="/on-demand/{mediaId}/moderation" -->
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

var res = await sdk.Moderations.UpdateAsync(
    mediaId: "<mediaId>",
    body: new UpdateMediaModerationRequestBody() {
        Moderation = new UpdateMediaModerationModeration() {
            Type = MediaType.Video,
        },
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

| Parameter                                                                                     | Type                                                                                          | Required                                                                                      | Description                                                                                   | Example                                                                                       |
| --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                     | *string*                                                                                      | :heavy_check_mark:                                                                            | The unique identifier assigned to the media when created. The value must be a valid UUID.<br/> | <mediaId>                                                          |
| `Body`                                                                                        | [UpdateMediaModerationRequestBody](../../Models/Requests/UpdateMediaModerationRequestBody.md) | :heavy_check_mark:                                                                            | N/A                                                                                           | {<br/>"moderation": {<br/>"type": "video"<br/>}<br/>}                                         |

### Response

**[UpdateMediaModerationResponse](../../Models/Requests/UpdateMediaModerationResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |