# InVideoAiFeatures
(*InVideoAiFeatures*)

## Overview

### Available Operations

* [GenerateNamedEntities](#generatenamedentities) - Generate named entities
* [EnableModeration](#enablemoderation) - Enable video moderation

## GenerateNamedEntities

This endpoint allows you to extract named entities from an existing media.
Named Entity Recognition (NER) is a fundamental natural language processing (NLP) technique that identifies and classifies key information (entities) in text into predefined categories. For instance:

  - Organizations (e.g., "Microsoft", "United Nations")
  - Locations (e.g., "Paris", "Mount Everest")
  - Product names (e.g., "iPhone", "Coca-Cola")

#### How it works
1. Make a PATCH request to this endpoint, replacing `<mediaId>` with the ID of the media you want to extract named-entities.
2. Include the `namedEntities` parameter in the request body to enable.
3. Receive a response containing the updated media data, confirming the changes made.

You can use the <a href="https://docs.fastpix.io/docs/ai-events#videomediaainamedentitiesready">video.mediaAI.named-entities.ready</a> webhook event to track and notify about the named entities extraction.

**Use case:** If a user uploads a video and later decides to enable named entity extraction without re-uploading the entire video.

Related guide: <a href="https://docs.fastpix.io/docs/generate-named-entities">Named entities</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-media-named-entities" method="patch" path="/on-demand/{mediaId}/named-entities" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.InVideoAiFeatures.GenerateNamedEntitiesAsync(
    mediaId: "0cec3c88-c69d-4232-9b96-f0976327fa2d",
    requestBody: new UpdateMediaNamedEntitiesRequestBody() {
        NamedEntities = true,
    }
);

// handle response
```

### Parameters

| Parameter                                                                                           | Type                                                                                                | Required                                                                                            | Description                                                                                         | Example                                                                                             |
| --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                           | *string*                                                                                            | :heavy_check_mark:                                                                                  | The unique identifier assigned to the media when created. The value should be a valid UUID.<br/>    | 0cec3c88-c69d-4232-9b96-f0976327fa2d                                                                |
| `RequestBody`                                                                                       | [UpdateMediaNamedEntitiesRequestBody](../../Models/Requests/UpdateMediaNamedEntitiesRequestBody.md) | :heavy_check_mark:                                                                                  | N/A                                                                                                 | {<br/>"namedEntities": true<br/>}                                                                   |

### Response

**[UpdateMediaNamedEntitiesResponse](../../Models/Requests/UpdateMediaNamedEntitiesResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.MediaNotFoundException     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## EnableModeration

This endpoint enables moderation features, such as NSFW and profanity filtering, to detect inappropriate content in existing media.

#### How it works
1. Make a PATCH request to this endpoint, replacing `<mediaId>` with the ID of the media you want to update.
2. Include the `moderation` object and provide the requried `type` parameter in the request body to specify the media type (e.g., video/audio/av).
4. The response will contain the updated media data, confirming the changes made.

You can use the <a href="https://docs.fastpix.io/docs/ai-events#videomediaaimoderationready">video.mediaAI.moderation.ready</a> webhook event to track and notify about the detected moderation results.

**Use case:** This is particularly useful when a user uploads a video and later decides to enable moderation detection without the need to re-upload it.

Related guide: <a href="https://docs.fastpix.io/docs/using-nsfw-and-profanity-filter-for-video-moderation">Moderate NSFW & Profanity</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-media-moderation" method="patch" path="/on-demand/{mediaId}/moderation" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.InVideoAiFeatures.EnableModerationAsync(
    mediaId: "0cec3c88-c69d-4232-9b96-f0976327fa2d",
    requestBody: new UpdateMediaModerationRequestBody() {
        Moderation = new UpdateMediaModerationModeration() {
            Type = MediaType.Video,
        },
    }
);

// handle response
```

### Parameters

| Parameter                                                                                     | Type                                                                                          | Required                                                                                      | Description                                                                                   | Example                                                                                       |
| --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                     | *string*                                                                                      | :heavy_check_mark:                                                                            | The unique identifier assigned to the media when created. The value should be a valid UUID.<br/> | 0cec3c88-c69d-4232-9b96-f0976327fa2d                                                          |
| `RequestBody`                                                                                 | [UpdateMediaModerationRequestBody](../../Models/Requests/UpdateMediaModerationRequestBody.md) | :heavy_check_mark:                                                                            | N/A                                                                                           | {<br/>"moderation": {<br/>"type": "video"<br/>}<br/>}                                         |

### Response

**[UpdateMediaModerationResponse](../../Models/Requests/UpdateMediaModerationResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.MediaNotFoundException     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |