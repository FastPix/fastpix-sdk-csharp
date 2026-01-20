# InVideoAI

## Overview

### Available Operations

* [UpdateMediaChapters](#updatemediachapters) - Generate video chapters
* [UpdateNamedEntities](#updatenamedentities) - Generate named entities

## UpdateMediaChapters

This endpoint enables you to generate chapters for an existing media file.

#### How it works
1. Make a `PATCH` request to this endpoint, replacing `<mediaId>` with the ID of the media for which you want to generate chapters.
2. Include the `chapters` parameter in the request body to enable.
3. The response contains the updated media data, confirming the changes made.

You can use the <a href="https://docs.fastpix.io/docs/ai-events#videomediaaichaptersready">video.mediaAI.chapters.ready</a> webhook event to track and notify about the chapters generation.

**Use case:** This is particularly useful when a user uploads a video and later decides to enable chapters without re-uploading the entire video.

Related guide: <a href="https://docs.fastpix.io/docs/generate-video-chapters">Video chapters</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-media-chapters" method="patch" path="/on-demand/{mediaId}/chapters" -->
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

var res = await sdk.InVideoAI.UpdateMediaChaptersAsync(
    mediaId: "<mediaId>",
    body: new UpdateMediaChaptersRequestBody() {}
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

| Parameter                                                                                  | Type                                                                                       | Required                                                                                   | Description                                                                                | Example                                                                                    |
| ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------ |
| `MediaId`                                                                                  | *string*                                                                                   | :heavy_check_mark:                                                                         | The unique identifier assigned to the media when created. The value must be a valid UUID.<br/> | <mediaId>                                                       |
| `Body`                                                                                     | [UpdateMediaChaptersRequestBody](../../Models/Requests/UpdateMediaChaptersRequestBody.md)  | :heavy_check_mark:                                                                         | N/A                                                                                        | {<br/>"chapters": true<br/>}                                                               |

### Response

**[UpdateMediaChaptersResponse](../../Models/Requests/UpdateMediaChaptersResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## UpdateNamedEntities

This endpoint allows you to extract named entities from an existing media.
Named Entity Recognition (NER) is a fundamental natural language processing (NLP) technique that identifies and classifies key information (entities) in text into predefined categories. For instance:

  - Organizations (for example, "Microsoft", "United Nations")
  - Locations (for example, "Paris", "Mount Everest")
  - Product names (for example, "iPhone", "Coca-Cola")

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
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.InVideoAI.UpdateNamedEntitiesAsync(
    mediaId: "<mediaId>",
    body: new UpdateMediaNamedEntitiesRequestBody() {
        NamedEntities = true,
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

| Parameter                                                                                           | Type                                                                                                | Required                                                                                            | Description                                                                                         | Example                                                                                             |
| --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                           | *string*                                                                                            | :heavy_check_mark:                                                                                  | The unique identifier assigned to the media when created. The value must be a valid UUID.<br/>      | <mediaId>                                                                |
| `Body`                                                                                              | [UpdateMediaNamedEntitiesRequestBody](../../Models/Requests/UpdateMediaNamedEntitiesRequestBody.md) | :heavy_check_mark:                                                                                  | N/A                                                                                                 | {<br/>"namedEntities": true<br/>}                                                                   |

### Response

**[UpdateMediaNamedEntitiesResponse](../../Models/Requests/UpdateMediaNamedEntitiesResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |