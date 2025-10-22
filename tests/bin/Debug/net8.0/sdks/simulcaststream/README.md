# SimulcastStream
(*SimulcastStream*)

## Overview

### Available Operations

* [Create](#create) - Create a simulcast

## Create

Lets you to create a simulcast for a parent live stream. Simulcasting enables you to broadcast the live stream to multiple social platforms simultaneously (e.g., YouTube, Facebook, or Twitch). This feature is useful for expanding your audience reach across different platforms. However, a simulcast can only be created when the parent live stream is in idle state (i.e., not currently live or disabled). Additionally, only one simulcast target can be created per API call. 
#### How it works

1. Upon calling this endpoint, you need to provide the parent `streamId` and the details of the simulcast target (platform and credentials). The system will generate a unique `simulcastId`, which can be used to manage the simulcast later. 

2. To notify your application about the status of simulcast related events check for the <a href="https://docs.fastpix.io/docs/webhooks-collection#simulcast-target-events">webhooks for simulcast</a> target events. 

#### Example
An event manager sets up a live stream for a virtual conference and wants to simulcast the stream on YouTube and Facebook Live. They first create the primary live stream in FastPix, ensuring it's in the idle state. Then, they use the API to create a simulcast target for YouTube. 

Related guide: <a href="https://docs.fastpix.io/docs/simulcast-to-3rd-party-platforms">Simulcast to 3rd party platforms</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="create-simulcast-of-stream" method="post" path="/live/streams/{streamId}/simulcast" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.SimulcastStream.CreateAsync(
    streamId: "8717422d89288ad5958d4a86e9afe2a2",
    simulcastRequest: new SimulcastRequest() {
        Url = "rtmp://hyd01.contribute.live-video.net/app/",
        StreamKey = "live_1012464221_DuM8W004MoZYNxQEZ0czODgfHCFBhk",
        Metadata = new Dictionary<string, string>() {
            { "livestream_name", "Tech-Connect Summit" },
        },
    }
);

Console.WriteLine(JsonConvert.SerializeObject(res.SimulcastResponse, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter                                                                                                                                                                         | Type                                                                                                                                                                              | Required                                                                                                                                                                          | Description                                                                                                                                                                       | Example                                                                                                                                                                           |
| --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `StreamId`                                                                                                                                                                        | *string*                                                                                                                                                                          | :heavy_check_mark:                                                                                                                                                                | Upon creating a new live stream, FastPix assigns a unique identifier to the stream.                                                                                               | 8717422d89288ad5958d4a86e9afe2a2                                                                                                                                                  |
| `SimulcastRequest`                                                                                                                                                                | [SimulcastRequest](../../Models/Components/SimulcastRequest.md)                                                                                                                   | :heavy_check_mark:                                                                                                                                                                | N/A                                                                                                                                                                               | {<br/>"url": "rtmp://hyd01.contribute.live-video.net/app/",<br/>"streamKey": "live_1012464221_DuM8W004MoZYNxQEZ0czODgfHCFBhk",<br/>"metadata": {<br/>"livestream_name": "Tech-Connect Summit"<br/>}<br/>} |

### Response

**[CreateSimulcastOfStreamResponse](../../Models/Requests/CreateSimulcastOfStreamResponse.md)**

### Errors

| Error Type                                          | Status Code                                         | Content Type                                        |
| --------------------------------------------------- | --------------------------------------------------- | --------------------------------------------------- |
| Fastpix.Models.Errors.SimulcastUnavailableException | 400                                                 | application/json                                    |
| Fastpix.Models.Errors.UnauthorizedException         | 401                                                 | application/json                                    |
| Fastpix.Models.Errors.InvalidPermissionException    | 403                                                 | application/json                                    |
| Fastpix.Models.Errors.LiveNotFoundError             | 404                                                 | application/json                                    |
| Fastpix.Models.Errors.ValidationErrorResponse       | 422                                                 | application/json                                    |
| Fastpix.Models.Errors.APIException                  | 4XX, 5XX                                            | \*/\*                                               |