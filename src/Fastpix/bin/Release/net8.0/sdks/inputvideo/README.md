# InputVideo
(*InputVideo*)

## Overview

### Available Operations

* [CreateMedia](#createmedia) - Create media from URL
* [DirectUploadVideoMedia](#directuploadvideomedia) - Upload media from device

## CreateMedia

This endpoint allows developers or users to create a new video or audio media in FastPix using a publicly accessible URL. FastPix will fetch the media from the provided URL, process it, and store it on the platform for use. 



#### Public URL requirement:


  The provided URL must be publicly accessible and should point to a video stored in one of the following supported formats: .m4v, .ogv, .mpeg, .mov, .3gp, .f4v, .rm, .ts, .wtv, .avi, .mp4, .wmv, .webm, .mts, .vob, .mxf, asf, m2ts 



#### Supported storage types:

The URL can originate from various cloud storage services or content delivery networks (CDNs) such as: 


* **Amazon S3:** URLs from Amazon's Simple Storage Service. 

* **Google Cloud Storage:** URLs from Google Cloud's storage solution. 

* **Azure Blob Storage:** URLs from Microsoft's Azure storage. 

* **Public CDNs:** URLs from public content delivery networks that host video files. 

Upon successful creation, the API returns an id that should be retained for future operations related to this media. 

#### How it works


1. Send a POST request to the /on-demand endpoint with the media URL (typically a video or audio file) and optional media settings. 

2. FastPix uploads the video from the provided URL to its storage. 

3. Receive a response containing the unique id for the newly created media item. 

4. Use the id in subsequent API calls, such as checking the status of the media with the **Get Media by ID** endpoint to determine when the media is ready for playback. 

FastPix uses webhooks to tell your application about things that happen in the background, outside of the API regular request flow. For instance, once the media file is created (but not yet processed or encoded), we’ll shoot a POST message to the address you give us with the webhook event video.media.created. 


Once processing is done you can look for the events video.media.ready and video.media.failed to see the status of your new media file.

#### Use case scenario


* **Use case:** A developer wants to integrate a user-generated content platform where users can upload links to their videos hosted on third-party platforms like AWS or Google Cloud Storage. This endpoint is used to create media directly from those URLs. 


* **Detailed example:** 
Say you’re building an online learning platform where instructors upload video URLs hosted on their private cloud servers. By providing the video URL to this endpoint, the platform processes and adds it to your media library, ready for playback. 


### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Models.Components.Input>() {
        Input.CreateVideoInput(
            new VideoInput() {
                Type = "video",
                Url = "https://static.fastpix.io/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4",
                StartTime = 0D,
                EndTime = 60D,
            }
        ),
        Input.CreateWatermarkInput(
            new WatermarkInput() {
                Type = WatermarkInputType.Watermark,
                Url = "https://static.fastpix.io/watermark-4k.png",
                Placement = new Placement() {
                    XAlign = XAlign.Left,
                    XMargin = "10%",
                    YAlign = YAlign.Top,
                    YMargin = "10%",
                },
            }
        ),
    },
    Metadata = new CreateMediaRequestMetadata() {},
    AccessPolicy = CreateMediaRequestAccessPolicy.Public,
    Mp4Support = CreateMediaRequestMp4Support.Capped4k,
};

var res = await sdk.InputVideo.CreateMediaAsync(req);

// handle response
```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `request`                                                           | [CreateMediaRequest](../../Models/Components/CreateMediaRequest.md) | :heavy_check_mark:                                                  | The request object to use for the request.                          |

### Response

**[Models.Requests.CreateMediaResponse](../../Models/Requests/CreateMediaResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.BadRequestException        | 400                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## DirectUploadVideoMedia

This endpoint enables you to upload a video file directly from your local device to FastPix for processing, storage. When you invoke this API with your preferred media settings, the response returns an uploadId and a pre-signed URL, providing a streamlined experience for uploading.



#### How it works

1. Send a POST request to the /on-demand/uploads endpoint with optional media settings.  

2. The response includes an **uploadId** and a pre-signed URL for direct video file upload.

3. Upload your video file to the provided **URL** by making **PUT** request. The API accepts the media file from the device and uploads it to the FastPix platform. 

4. Once uploaded, the media undergoes processing and is assigned a unique ID for tracking. Retain this **uploadId** for any future operations related to this upload. 




After uploading, you can use the **Get Media by ID** endpoint to check the status of the uploaded media asset and see if it has transitioned to a "ready" status for playback. 

To notify your application about the status of this API request check for the webhooks for Upload related events.  


#### Use case scenario 

**Use case:** A social media platform allows users to upload video content directly from their phones or computers. This endpoint facilitates the upload process. For example, if you are developing a video-sharing app where users can upload short clips from their mobile devices, this endpoint enables them to select a video, upload it to the platform.


### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

DirectUploadVideoMediaRequest req = new DirectUploadVideoMediaRequest() {
    CorsOrigin = "*",
    PushMediaSettings = new PushMediaSettings() {
        AccessPolicy = DirectUploadVideoMediaAccessPolicy.Public,
        Metadata = new DirectUploadVideoMediaMetadata() {},
    },
};

var res = await sdk.InputVideo.DirectUploadVideoMediaAsync(req);

// handle response
```

### Parameters

| Parameter                                                                               | Type                                                                                    | Required                                                                                | Description                                                                             |
| --------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- |
| `request`                                                                               | [DirectUploadVideoMediaRequest](../../Models/Requests/DirectUploadVideoMediaRequest.md) | :heavy_check_mark:                                                                      | The request object to use for the request.                                              |

### Response

**[DirectUploadVideoMediaResponse](../../Models/Requests/DirectUploadVideoMediaResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.BadRequestException        | 400                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |