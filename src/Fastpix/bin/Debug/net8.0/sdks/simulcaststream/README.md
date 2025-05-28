# SimulcastStream
(*SimulcastStream*)

## Overview

### Available Operations

* [CreateSimulcastOfStream](#createsimulcastofstream) - Create a simulcast
* [DeleteSimulcastOfStream](#deletesimulcastofstream) - Delete a simulcast
* [GetSpecificSimulcastOfStream](#getspecificsimulcastofstream) - Get a specific simulcast of a stream
* [UpdateSpecificSimulcastOfStream](#updatespecificsimulcastofstream) - Update a specific simulcast of a stream

## CreateSimulcastOfStream

Lets you to create a simulcast for a parent live stream. A simulcast enables you to broadcast the live stream to multiple platforms simultaneously (e.g., YouTube, Facebook, or Twitch). This feature is useful for expanding your audience reach across different platforms. However, a simulcast can only be created when the parent live stream is in an idle state (i.e., not currently live or disabled). Additionally, only one simulcast target can be created per API call. 

  <h4>How it works</h4> 


  Upon calling this endpoint, you need to provide the parent streamId and the details of the simulcast target (platform and credentials). The system will generate a unique simulcastId, which can be used to manage the simulcast later. 



To notify your application about the status of simulcast related events check for the webhooks for simulcast target events. 

**Practical example:** An event manager sets up a live stream for a virtual conference and wants to simulcast the stream on YouTube and Facebook Live. They first create the primary live stream in FastPix, ensuring it's in the idle state. Then, they use the API to create a simulcast target for YouTube. 

### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.SimulcastStream.CreateSimulcastOfStreamAsync(
    streamId: "8717422d89288ad5958d4a86e9afe2a2",
    simulcastRequest: new SimulcastRequest() {
        Url = "rtmp://hyd01.contribute.live-video.net/app/",
        StreamKey = "live_1012464221_DuM8W004MoZYNxQEZ0czODgfHCFBhk",
    }
);

// handle response
```

### Parameters

| Parameter                                                                                                                                                                         | Type                                                                                                                                                                              | Required                                                                                                                                                                          | Description                                                                                                                                                                       | Example                                                                                                                                                                           |
| --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `StreamId`                                                                                                                                                                        | *string*                                                                                                                                                                          | :heavy_check_mark:                                                                                                                                                                | Upon creating a new live stream, FastPix assigns a unique identifier to the stream.                                                                                               | 8717422d89288ad5958d4a86e9afe2a2                                                                                                                                                  |
| `SimulcastRequest`                                                                                                                                                                | [SimulcastRequest](../../Models/Components/SimulcastRequest.md)                                                                                                                   | :heavy_minus_sign:                                                                                                                                                                | N/A                                                                                                                                                                               | {<br/>"url": "rtmp://hyd01.contribute.live-video.net/app/",<br/>"streamKey": "live_1012464221_DuM8W004MoZYNxQEZ0czODgfHCFBhk",<br/>"metadata": {<br/>"livestream_name": "Tech-Connect Summit"<br/>}<br/>} |

### Response

**[CreateSimulcastOfStreamResponse](../../Models/Requests/CreateSimulcastOfStreamResponse.md)**

### Errors

| Error Type                                          | Status Code                                         | Content Type                                        |
| --------------------------------------------------- | --------------------------------------------------- | --------------------------------------------------- |
| Fastpix.Models.Errors.SimulcastUnavailableException | 400                                                 | application/json                                    |
| Fastpix.Models.Errors.UnauthorizedException         | 401                                                 | application/json                                    |
| Fastpix.Models.Errors.InvalidPermissionException    | 403                                                 | application/json                                    |
| Fastpix.Models.Errors.NotFoundError                 | 404                                                 | application/json                                    |
| Fastpix.Models.Errors.ValidationErrorResponse       | 422                                                 | application/json                                    |
| Fastpix.Models.Errors.APIException                  | 4XX, 5XX                                            | \*/\*                                               |

## DeleteSimulcastOfStream

Allows you to delete a simulcast using its unique simulcastId, which was returned during the simulcast creation process. Deleting a simulcast stops the broadcast to the associated platform, but the parent stream will continue to run if it is live. This action is irreversible, and a new simulcast would need to be created if you want to resume streaming to the same platform. 

  **Use case:** A broadcaster needs to stop simulcasting to one platform due to technical difficulties while keeping the stream active on others. For example, a tech company is simulcasting a product launch on multiple platforms. Midway through the event, they decide to stop the simulcast on Facebook due to performance issues, but keep it running on YouTube. They call this API to delete the Facebook simulcast target. 

### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.SimulcastStream.DeleteSimulcastOfStreamAsync(
    streamId: "8717422d89288ad5958d4a86e9afe2a2",
    simulcastId: "9217422d89288ad5958d4a86e9afe2a1"
);

// handle response
```

### Parameters

| Parameter                                                                                                                      | Type                                                                                                                           | Required                                                                                                                       | Description                                                                                                                    | Example                                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ |
| `StreamId`                                                                                                                     | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | Upon creating a new live stream, FastPix assigns a unique identifier to the stream.                                            | 8717422d89288ad5958d4a86e9afe2a2                                                                                               |
| `SimulcastId`                                                                                                                  | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | When you create the new simulcast, FastPix assign a universal unique identifier which can contain a maximum of 255 characters. | 9217422d89288ad5958d4a86e9afe2a1                                                                                               |

### Response

**[DeleteSimulcastOfStreamResponse](../../Models/Requests/DeleteSimulcastOfStreamResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.UnauthorizedException      | 401                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 403                                              | application/json                                 |
| Fastpix.Models.Errors.NotFoundErrorSimulcast     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## GetSpecificSimulcastOfStream

Retrieves the details of a specific simulcast associated with a parent live stream. By providing both the streamId of the parent stream and the simulcastId, FastPix returns detailed information about the simulcast, such as the stream URL, the status of the simulcast (active or idle), and metadata. 

  **Use case:** This endpoint can be used to verify the status of the simulcast on external platforms before the live stream begins. For instance, before starting a live gaming event, the organizer wants to ensure that the simulcast to Twitch is set up correctly. They retrieve the simulcast information to confirm that everything is properly configured. 

### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.SimulcastStream.GetSpecificSimulcastOfStreamAsync(
    streamId: "8717422d89288ad5958d4a86e9afe2a2",
    simulcastId: "8717422d89288ad5958d4a86e9afe2a2"
);

// handle response
```

### Parameters

| Parameter                                                                                                                      | Type                                                                                                                           | Required                                                                                                                       | Description                                                                                                                    | Example                                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ |
| `StreamId`                                                                                                                     | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | Upon creating a new live stream, FastPix assigns a unique identifier to the stream.                                            | 8717422d89288ad5958d4a86e9afe2a2                                                                                               |
| `SimulcastId`                                                                                                                  | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | When you create the new simulcast, FastPix assign a universal unique identifier which can contain a maximum of 255 characters. | 8717422d89288ad5958d4a86e9afe2a2                                                                                               |

### Response

**[GetSpecificSimulcastOfStreamResponse](../../Models/Requests/GetSpecificSimulcastOfStreamResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.UnauthorizedException      | 401                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 403                                              | application/json                                 |
| Fastpix.Models.Errors.NotFoundErrorSimulcast     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## UpdateSpecificSimulcastOfStream

Allows you to enable or disable a specific simulcast associated with a parent live stream. The status of the simulcast can be updated at any point, whether the live stream is active or idle. However, once the live stream is disabled, the simulcast can no longer be modified. 

  **Use case:** When a PATCH request is made to this endpoint, the API updates the status of the simulcast. This can be useful for pausing or resuming a simulcast on a particular platform without stopping the parent live stream. 

### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.SimulcastStream.UpdateSpecificSimulcastOfStreamAsync(
    streamId: "8717422d89288ad5958d4a86e9afe2a2",
    simulcastId: "8717422d89288ad5958d4a86e9afe2a2",
    simulcastUpdateRequest: new SimulcastUpdateRequest() {
        IsEnabled = false,
        Metadata = new SimulcastUpdateRequestMetadata() {},
    }
);

// handle response
```

### Parameters

| Parameter                                                                                                                      | Type                                                                                                                           | Required                                                                                                                       | Description                                                                                                                    | Example                                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ |
| `StreamId`                                                                                                                     | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | Upon creating a new live stream, FastPix assigns a unique identifier to the stream.                                            | 8717422d89288ad5958d4a86e9afe2a2                                                                                               |
| `SimulcastId`                                                                                                                  | *string*                                                                                                                       | :heavy_check_mark:                                                                                                             | When you create the new simulcast, FastPix assign a universal unique identifier which can contain a maximum of 255 characters. | 8717422d89288ad5958d4a86e9afe2a2                                                                                               |
| `SimulcastUpdateRequest`                                                                                                       | [SimulcastUpdateRequest](../../Models/Components/SimulcastUpdateRequest.md)                                                    | :heavy_minus_sign:                                                                                                             | N/A                                                                                                                            | {<br/>"isEnabled": false,<br/>"metadata": {<br/>"simulcast_name": "Tech today"<br/>}<br/>}                                     |

### Response

**[UpdateSpecificSimulcastOfStreamResponse](../../Models/Requests/UpdateSpecificSimulcastOfStreamResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.UnauthorizedException      | 401                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 403                                              | application/json                                 |
| Fastpix.Models.Errors.NotFoundErrorSimulcast     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |