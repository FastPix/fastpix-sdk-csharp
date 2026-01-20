# InVideoAIFeatures

## Overview

### Available Operations

* [UpdateSummary](#updatesummary) - Generate video summary

## UpdateSummary

This endpoint allows you to generate the summary for an existing media.

#### How it works
1. Send a `PATCH` request to this endpoint, replacing `<mediaId>` with the ID of the media you want to summarize.
2. Include the `generate` parameter in the request body.
3. Include the `summaryLength` parameter, specify the desired length of the summary in words (for example, 120 words), this determines how concise or detailed the summary will be. If no specific summary length is provided, the default length will be 100 words.
4. The response includes the updated media data and confirmation of the changes applied.

You can use the <a href="https://docs.fastpix.io/docs/ai-events#videomediaaisummaryready">video.mediaAI.summary.ready</a> webhook event to track and notify about the summary generation.





**Use case**: This is particularly useful when a user uploads a video and later chooses to generate a summary without needing to re-upload the video.

Related guide: <a href="https://docs.fastpix.io/docs/generate-video-summary">Video summary</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-media-summary" method="patch" path="/on-demand/{mediaId}/summary" -->
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

var res = await sdk.InVideoAIFeatures.UpdateSummaryAsync(
    mediaId: "<mediaId>",
    body: new UpdateMediaSummaryRequestBody() {
        Generate = true,
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

| Parameter                                                                                  | Type                                                                                       | Required                                                                                   | Description                                                                                | Example                                                                                    |
| ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------ |
| `MediaId`                                                                                  | *string*                                                                                   | :heavy_check_mark:                                                                         | The unique identifier assigned to the media when created. The value must be a valid UUID.<br/> | <mediaId>                                                       |
| `Body`                                                                                     | [UpdateMediaSummaryRequestBody](../../Models/Requests/UpdateMediaSummaryRequestBody.md)    | :heavy_check_mark:                                                                         | N/A                                                                                        | {<br/>"generate": true,<br/>"summaryLength": 100<br/>}                                     |

### Response

**[UpdateMediaSummaryResponse](../../Models/Requests/UpdateMediaSummaryResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |