# Simulcasts

## Overview

### Available Operations

* [Create](#create) - Create a simulcast
* [Delete](#delete) - Delete a simulcast
* [Update](#update) - Update a simulcast

## Create

Creates a simulcast for a parent live stream. Simulcasting allows you to broadcast a live stream to multiple social platforms simultaneously (for example, YouTube, Facebook, or Twitch). This helps expand your audience reach across platforms. A simulcast can only be created when the parent live stream is in the idle state (not currently live or disabled). Only one simulcast target can be created per API call. 
#### How it works

1. Change to: When you call this endpoint, provide the parent `streamId` along with the simulcast target details (such as platform and credentials). The API returns a unique `simulcastId`, which you can use to manage the simulcast later.  

2. To notify your application about the status of simulcast related events check for the <a href="https://docs.fastpix.io/docs/webhooks-collection#simulcast-target-events">webhooks for simulcast</a> target events. 

#### Example
An event manager sets up a live stream for a virtual conference and wants to simulcast the stream on YouTube and Facebook Live. They first create the primary live stream in FastPix, ensuring it's in the idle state. Then, they use the API to create a simulcast target for YouTube. 

Related guide: <a href="https://docs.fastpix.io/docs/simulcast-to-3rd-party-platforms">Simulcast to 3rd party platforms</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="create-simulcast-of-stream" method="post" path="/live/streams/{streamId}/simulcast" -->
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

var res = await sdk.Simulcasts.CreateAsync(
    streamId: "<streamId>",
    body: new SimulcastRequest() {
        Url = "<rtmp-url>",
        StreamKey = "<streamKey>",
        Metadata = new Dictionary<string, string>() {
            { "livestream_name", "<livestream_name>" },
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.SimulcastResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                                                                                                         | Type                                                                                                                                                                              | Required                                                                                                                                                                          | Description                                                                                                                                                                       | Example                                                                                                                                                                           |
| --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `StreamId`                                                                                                                                                                        | *string*                                                                                                                                                                          | :heavy_check_mark:                                                                                                                                                                | After creating a new live stream, FastPix assigns a unique identifier to the stream.                                                                                              | <streamId>                                                                                                                                                  |
| `Body`                                                                                                                                                                            | [SimulcastRequest](../../Models/Components/SimulcastRequest.md)                                                                                                                   | :heavy_check_mark:                                                                                                                                                                | N/A                                                                                                                                                                               | {<br/>"url": "<rtmp-url>",<br/>"streamKey": "<streamKey>",<br/>"metadata": {<br/>"livestream_name": "<livestream_name>"<br/>}<br/>} |

### Response

**[CreateSimulcastOfStreamResponse](../../Models/Requests/CreateSimulcastOfStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Delete

Deletes a simulcast using its unique simulcastId, which you received during the simulcast creation process. Deleting a simulcast stops the broadcast to the associated platform, while the parent stream continues if it’s live. This action can’t be undone, and you must create a new simulcast to resume streaming to the same platform.

Webhook event: <a href="https://docs.fastpix.io/docs/live-events#videolive_streamsimulcast_targetdeleted">video.live_stream.simulcast_target.deleted</a>


#### Example
A broadcaster may need to stop simulcasting to one platform while keeping the stream active on others. For example, a tech company is simulcasting a product launch across multiple platforms. Midway through the event, they decide to stop the simulcast on Facebook due to performance issues but continue streaming on YouTube. They use this API to delete the Facebook simulcast target. 

### Example Usage

<!-- UsageSnippet language="csharp" operationID="delete-simulcast-of-stream" method="delete" path="/live/streams/{streamId}/simulcast/{simulcastId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Simulcasts.DeleteAsync(
    streamId: "<streamId>",
    simulcastId: "<simulcastId>"
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.SimulcastdeleteResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                                                      | Type                                                                                                                           | Required                                                                                                                       | Description                                                                                                                    | Example                                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ |
| `StreamId`                                                                                                                     | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | After creating a new live stream, FastPix assigns a unique identifier to the stream.                                           | <streamId>                                                                                               |
| `SimulcastId`                                                                                                                  | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | When you create the new simulcast, FastPix assign a universal unique identifier which can contain a maximum of 255 characters. | <simulcastId>                                                                                               |

### Response

**[DeleteSimulcastOfStreamResponse](../../Models/Requests/DeleteSimulcastOfStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Update

Updates the status of a specific simulcast linked to a parent live stream. You can enable or disable the simulcast at any time while the parent stream is active or idle. After the live stream is disabled, the simulcast can no longer be modified.

Webhook event: <a href="https://docs.fastpix.io/docs/live-events#videolive_streamsimulcast_targetupdated">video.live_stream.simulcast_target.updated</a>

#### Example
When a `PATCH` request is made to this endpoint, the API updates the status of the simulcast. This can be useful for pausing or resuming a simulcast on a particular platform without stopping the parent live stream.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-specific-simulcast-of-stream" method="put" path="/live/streams/{streamId}/simulcast/{simulcastId}" -->
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

var res = await sdk.Simulcasts.UpdateAsync(
    streamId: "<streamId>",
    simulcastId: "<simulcastId>",
    body: new SimulcastUpdateRequest() {
        Metadata = new Dictionary<string, string>() {
            { "simulcast_name", "<simulcast_name>" },
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.SimulcastUpdateResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                                                      | Type                                                                                                                           | Required                                                                                                                       | Description                                                                                                                    | Example                                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ |
| `StreamId`                                                                                                                     | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | Upon creating a new live stream, FastPix assigns a unique identifier to the stream.                                            | <streamId>                                                                                               |
| `SimulcastId`                                                                                                                  | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | When you create the new simulcast, FastPix assign a universal unique identifier which can contain a maximum of 255 characters. | <streamId>                                                                                               |
| `Body`                                                                                                                         | [SimulcastUpdateRequest](../../Models/Components/SimulcastUpdateRequest.md)                                                    | :heavy_check_mark:                                                                                                             | N/A                                                                                                                            | {<br/>"isEnabled": true,<br/>"metadata": {<br/>"simulcast_name": "<simulcast_name>"<br/>}<br/>}                                      |

### Response

**[UpdateSpecificSimulcastOfStreamResponse](../../Models/Requests/UpdateSpecificSimulcastOfStreamResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |