# ManageVideos
(*ManageVideos*)

## Overview

### Available Operations

* [ListMedia](#listmedia) - Get list of all media
* [GetMedia](#getmedia) - Get a media by ID
* [UpdatedMedia](#updatedmedia) - Update a media by ID
* [DeleteMedia](#deletemedia) - Delete a media by ID
* [RetrieveMediaInputInfo](#retrievemediainputinfo) - Get info of media inputs

## ListMedia

This endpoint returns a list of all media files created from a URL or uploaded as file objects within your organization. Each media entry contains metadata such as the media ID, creation date, status, and type. It allows you to retrieve a comprehensive overview of your media assets, making it easier to manage and review them. 


#### How it works

When called, the API provides a paginated response containing the media items in the organization's library. This is helpful for retrieving a large volume of media and managing content in bulk. 



#### Use case scenario 

* **Use case:** A content manager at a video-on-demand platform wants to see all uploaded media to assess the quality and status of videos. 



* **Detailed example:** 
You're managing a video platform and need to check all the uploaded media in your library to ensure no outdated or low-quality content is being served. Using this endpoint, you can retrieve a complete list of media, allowing you to filter, sort, or update items as needed. 


### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageVideos.ListMediaAsync(
    limit: 20,
    offset: 1,
    orderBy: ListMediaOrderBy.Desc
);

// handle response
```

### Parameters

| Parameter                                                                                 | Type                                                                                      | Required                                                                                  | Description                                                                               | Example                                                                                   |
| ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| `Limit`                                                                                   | *long*                                                                                    | :heavy_minus_sign:                                                                        | Limit specifies the maximum number of items to display per page.                          | 20                                                                                        |
| `Offset`                                                                                  | *long*                                                                                    | :heavy_minus_sign:                                                                        | Offset determines the starting point for data retrieval within a paginated list.          | 1                                                                                         |
| `OrderBy`                                                                                 | [ListMediaOrderBy](../../Models/Requests/ListMediaOrderBy.md)                             | :heavy_minus_sign:                                                                        | The values in the list can be arranged in two ways: DESC (Descending) or ASC (Ascending). | desc                                                                                      |

### Response

**[ListMediaResponse](../../Models/Requests/ListMediaResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## GetMedia

By calling this endpoint, you can retrieve detailed information about a specific media item, including its current status and a playbackId. This is particularly useful for retrieving specific media details when managing large content libraries. 



#### How it works 



1. Make a GET request to the **/on-demand/`<mediaId>`**  endpoint, replacing `<mediaId>` with the **uploadId** received during the upload process or the id obtained when creating media from a URL. 


2. Receive a response that includes details about the media, including: 

* **status:** Indicates whether the media is still “preparing” or has transitioned to "ready."  

* **playbackId:** The playbackId is a unique identifier that allows you to stream the media once it is ready. You can construct the stream URL dynamically using the playbackId in the following format: `https://stream.fastpix.io/<playbackId>.m3u8`



**Please note:** Polling this API will let you know the status that whether the upload media has been moved to ready status, so that you can get started with streaming your media. 

#### Use case scenario

**Use case:** Suppose your platform provides users with an interface where they can manage their uploaded content. A user requests detailed information about a specific video to see if it has been fully processed and is available for playback. Using the media ID, you can fetch the information from FastPix and display it in the user’s dashboard. 


### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageVideos.GetMediaAsync(mediaId: "4fa85f64-5717-4562-b3fc-2c963f66afa6");

// handle response
```

### Parameters

| Parameter                                                                                              | Type                                                                                                   | Required                                                                                               | Description                                                                                            | Example                                                                                                |
| ------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ |
| `MediaId`                                                                                              | *string*                                                                                               | :heavy_check_mark:                                                                                     | The Media Id is assigned a universal unique identifier, which can contain a maximum of 255 characters. | 4fa85f64-5717-4562-b3fc-2c963f66afa6                                                                   |

### Response

**[GetMediaResponse](../../Models/Requests/GetMediaResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.MediaNotFoundException     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## UpdatedMedia

This endpoint allows you to update specific parameters of an existing media file. You can modify the key-value pairs of the metadata that were provided in the payload during the creation of media from a URL or when uploading the media as a file object. 


#### How it works

1. Make a PATCH request to the **/on-demand/`<mediaId>`**  endpoint, replacing `<mediaId>` with the uploadId or the id of the media you want to update. 

2. Include the updated parameters in the request body. 

3. Receive a response containing the updated media data, confirming the changes made. 

Once you have made the update request, you can also look for the webhook event **video.media.updated** to notify your system about update status. 


**Use case:** Imagine a scenario where a user uploads a video and later realizes they need to change the title, add a new description or tags. You can use this endpoint to update the media metadata without having to re-upload the entire video.


### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageVideos.UpdatedMediaAsync(
    mediaId: "4fa85f64-5717-4562-b3fc-2c963f66afa6",
    requestBody: new UpdatedMediaRequestBody() {
        Metadata = new UpdatedMediaMetadata() {},
    }
);

// handle response
```

### Parameters

| Parameter                                                                                                         | Type                                                                                                              | Required                                                                                                          | Description                                                                                                       | Example                                                                                                           |
| ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                                         | *string*                                                                                                          | :heavy_check_mark:                                                                                                | When creating the media, FastPix assigns a universally unique identifier with a maximum length of 255 characters. | 4fa85f64-5717-4562-b3fc-2c963f66afa6                                                                              |
| `RequestBody`                                                                                                     | [UpdatedMediaRequestBody](../../Models/Requests/UpdatedMediaRequestBody.md)                                       | :heavy_minus_sign:                                                                                                | N/A                                                                                                               | {<br/>"metadata": {<br/>"user": "fastpix_admin"<br/>}<br/>}                                                       |

### Response

**[UpdatedMediaResponse](../../Models/Requests/UpdatedMediaResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.MediaNotFoundException     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## DeleteMedia

This endpoint allows you to permanently delete a a specific video or audio media file along with all associated data. If you wish to remove a media file from FastPix storage, use this endpoint with the **mediaId** (either **uploadId** or **id**) received during the media's creation or upload. 


#### How it works


1. Make a DELETE request to the **/on-demand/`<mediaId>`**  endpoint, replacing `<mediaId>` with the uploadId or the id of the media you want to delete. 

2. Confirm the deletion: Since this action is irreversible, ensure that you no longer need the media before proceeding. Once deleted, the media cannot be retrieved or played back. 

3. Webhook event to look for: **video.media.deleted** 

**Use case:** A user on a video-sharing platform decides to remove an old video from their profile, or suppose you're running a content moderation system, and one of the videos uploaded by a user violates your platform’s policies. Using this endpoint, the media is permanently deleted from your library, ensuring it’s no longer accessible or viewable by other users. 


### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageVideos.DeleteMediaAsync(mediaId: "4fa85f64-5717-4562-b3fc-2c963f66afa6");

// handle response
```

### Parameters

| Parameter                                                                                                         | Type                                                                                                              | Required                                                                                                          | Description                                                                                                       | Example                                                                                                           |
| ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                                         | *string*                                                                                                          | :heavy_check_mark:                                                                                                | When creating the media, FastPix assigns a universally unique identifier with a maximum length of 255 characters. | 4fa85f64-5717-4562-b3fc-2c963f66afa6                                                                              |

### Response

**[DeleteMediaResponse](../../Models/Requests/DeleteMediaResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.MediaNotFoundException     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## RetrieveMediaInputInfo

Allows you to retrieve detailed information about the media inputs associated with a specific media item. You can use this endpoint to verify the media file’s input URL, track creation status, and container format. The mediaId (either uploadId or id) must be provided to fetch the information. 


#### How it works



Upon making a GET request with the mediaId, FastPix returns a response that includes: 

* **Input-url:** The URL of the uploaded media file. 

* **tracks:** Information about the tracks associated with the media, including both video and audio tracks, indicating whether they have been successfully created. 

* **containerFormat:** The format of the uploaded media file container (e.g., MP4, MKV). 



This endpoint is particularly useful for ensuring that all necessary tracks (video and audio) have been correctly associated with the media during the upload or media creation process. 


### Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "",
    Password = "",
});

var res = await sdk.ManageVideos.RetrieveMediaInputInfoAsync(mediaId: "4fa85f64-5717-4562-b3fc-2c963f66afa6");

// handle response
```

### Parameters

| Parameter                                                                                 | Type                                                                                      | Required                                                                                  | Description                                                                               | Example                                                                                   |
| ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| `MediaId`                                                                                 | *string*                                                                                  | :heavy_check_mark:                                                                        | Pass the list of the input objects used to create the media, along with applied settings. | 4fa85f64-5717-4562-b3fc-2c963f66afa6                                                      |

### Response

**[RetrieveMediaInputInfoResponse](../../Models/Requests/RetrieveMediaInputInfoResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ForbiddenException         | 403                                              | application/json                                 |
| Fastpix.Models.Errors.MediaNotFoundException     | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |