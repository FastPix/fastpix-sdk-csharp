# MediaAI
(*MediaAI*)

## Overview

### Available Operations

* [UpdateSummary](#updatesummary) - Generate video summary
* [UpdateChapters](#updatechapters) - Generate video chapters

## UpdateSummary

This endpoint allows you to generate the summary for an existing media.

#### How it works
1. Send a PATCH request to this endpoint, replacing `<mediaId>` with the unique ID of the media for which you wish to generate a summary.
2. Include the `generate` parameter in the request body.
3. Include the `summaryLength` parameter, specify the desired length of the summary in words (e.g., 120 words), this determines how concise or detailed the summary will be. If no specific summary length is provided, the default length will be 100 words. 
4. The response will include the updated media data and confirmation of the changes applied.

You can use the <a href="https://docs.fastpix.io/docs/ai-events#videomediaaisummaryready">video.mediaAI.summary.ready</a> webhook event to track and notify about the summary generation.





**Use case**: This is particularly useful when a user uploads a video and later chooses to generate a summary without needing to re-upload the video.

Related guide: <a href="https://docs.fastpix.io/docs/generate-video-summary">Video summary</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-media-summary" method="patch" path="/on-demand/{mediaId}/summary" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.MediaAI.UpdateSummaryAsync(
    mediaId: "paste-your-media-id-here",
    requestBody: new UpdateMediaSummaryRequestBody() {
        Generate = true,
    }
);

Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter                                                                                    | Type                                                                                         | Required                                                                                     | Description                                                                                  | Example                                                                                      |
| -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                    | *string*                                                                                     | :heavy_check_mark:                                                                           | The unique identifier assigned to the media when created. The value should be a valid UUID.<br/> | 4fa85f64-5717-4562-b3fc-2c963f66afa6                                                         |
| `RequestBody`                                                                                | [UpdateMediaSummaryRequestBody](../../../Models/Requests/UpdateMediaSummaryRequestBody.md)      | :heavy_check_mark:                                                                           | N/A                                                                                          | {<br/>"generate": true,<br/>"summaryLength": 100<br/>}                                       |

### Response

**[UpdateMediaSummaryResponse](../../../Models/Requests/UpdateMediaSummaryResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.MediaNotFoundException     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## UpdateChapters

This endpoint enables you to generate chapters for an existing media file.

#### How it works
1. Make a `PATCH` request to this endpoint, replacing `<mediaId>` with the ID of the media for which you want to generate chapters.
2. Include the `chapters` parameter in the request body to enable.
3. The response will contain the updated media data, confirming the changes made.

You can use the <a href="https://docs.fastpix.io/docs/ai-events#videomediaaichaptersready">video.mediaAI.chapters.ready</a> webhook event to track and notify about the chapters generation.

**Use case:** This is particularly useful when a user uploads a video and later decides to enable chapters without re-uploading the entire video.

Related guide: <a href="https://docs.fastpix.io/reference/update-media-chapters">Video chapters</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-media-chapters" method="patch" path="/on-demand/{mediaId}/chapters" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.MediaAI.UpdateChaptersAsync(
    mediaId: "paste-your-media-id-here",
    requestBody: new UpdateMediaChaptersRequestBody() {
        Chapters = true,
    }
);

Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter                                                                                    | Type                                                                                         | Required                                                                                     | Description                                                                                  | Example                                                                                      |
| -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                    | *string*                                                                                     | :heavy_check_mark:                                                                           | The unique identifier assigned to the media when created. The value should be a valid UUID.<br/> | 4fa85f64-5717-4562-b3fc-2c963f66afa6                                                         |
| `RequestBody`                                                                                | [UpdateMediaChaptersRequestBody](../../../Models/Requests/UpdateMediaChaptersRequestBody.md)    | :heavy_check_mark:                                                                           | N/A                                                                                          | {<br/>"chapters": true<br/>}                                                                 |

### Response

**[UpdateMediaChaptersResponse](../../../Models/Requests/UpdateMediaChaptersResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.MediaNotFoundException     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |