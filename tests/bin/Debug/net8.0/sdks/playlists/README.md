# Playlists

## Overview

### Available Operations

* [GetAll](#getall) - Get all playlists
* [Get](#get) - Get a playlist by ID
* [Update](#update) - Update a playlist by ID
* [Delete](#delete) - Delete a playlist by ID
* [AddMedia](#addmedia) - Add media to a playlist by ID
* [ReorderMedia](#reordermedia) - Change media order in a playlist by ID
* [DeleteMedia](#deletemedia) - Delete media in a playlist by ID

## GetAll

This endpoint retrieves all playlists in a specified workspace. It allows you to view the collection of manual and smart playlists along with their associated metadata.
#### How it works

 - When a user sends a GET request to this endpoint, FastPix returns a list of all playlists in the workspace, including details such as playlist IDs, titles, creation mode (manual or smart), and other relevant metadata.

#### Example

  An e-learning platform requests all playlists within a workspace to display an overview of available learning paths. The response includes multiple playlists like "Beginner Python Series" and "Advanced Java Tutorials," enabling the platform to show users a catalog of curated content collections.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-all-playlists" method="get" path="/on-demand/playlists" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playlists.GetAllAsync(
    limit: 1,
    offset: 1
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.GetAllPlaylistsResponseValue,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                          | Type                                                                                               | Required                                                                                           | Description                                                                                        | Example                                                                                            |
| -------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- |
| `Limit`                                                                                            | *long*                                                                                             | :heavy_minus_sign:                                                                                 | The number of playlists to return (default is 10, max is 50).                                      | 1                                                                                                  |
| `Offset`                                                                                           | *long*                                                                                             | :heavy_minus_sign:                                                                                 | The page number to retrieve, starting from 1. Use this parameter to paginate the playlist results. | 1                                                                                                  |

### Response

**[Models.Requests.GetAllPlaylistsResponse](../../Models/Requests/GetAllPlaylistsResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Get

This endpoint retrieves detailed information about a specific playlist using its unique `playlistId`. It provides comprehensive metadata about the playlist, including its title, creation mode (manual or smart), media items along with the metadata of each media in the playlist.


#### Example
An e-learning platform requests details for the playlist "Beginner Python Series" by providing its unique `playlistId`. The response includes the playlist"s title, creation mode, and the ordered list of video tutorials contained within, enabling the platform to present the full learning path to users.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-playlist-by-id" method="get" path="/on-demand/playlists/{playlistId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playlists.GetAsync(playlistId: "<id>");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PlaylistByIdResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                           | Type                                                | Required                                            | Description                                         |
| --------------------------------------------------- | --------------------------------------------------- | --------------------------------------------------- | --------------------------------------------------- |
| `PlaylistId`                                        | *string*                                            | :heavy_check_mark:                                  | The unique id of the playlist you want to retrieve. |

### Response

**[GetPlaylistByIdResponse](../../Models/Requests/GetPlaylistByIdResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Update

This endpoint allows you to update the name and description of an existing playlist. It enables modifications to the playlist's metadata without altering the media items or playlist structure.
#### How it works

 - When a user sends a PUT request to this endpoint with the `playlistId` and updated name and description in the request body, FastPix updates the playlist metadata accordingly and returns the updated playlist details.

#### Example
An e-learning platform updates the playlist titled "Beginner Python Series" to rename it as "Python Basics" and add a more detailed description. The updated metadata is reflected when retrieving the playlist, helping users better understand the playlist content.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-a-playlist" method="put" path="/on-demand/playlists/{playlistId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playlists.UpdateAsync(
    playlistId: "<id>",
    body: new UpdatePlaylistRequest() {
        Name = "updated name",
        Description = "updated description",
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PlaylistCreatedResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                 | Type                                                                      | Required                                                                  | Description                                                               | Example                                                                   |
| ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| `PlaylistId`                                                              | *string*                                                                  | :heavy_check_mark:                                                        | The unique id of the playlist you want to retrieve.                       |                                                                           |
| `Body`                                                                    | [UpdatePlaylistRequest](../../Models/Components/UpdatePlaylistRequest.md) | :heavy_check_mark:                                                        | N/A                                                                       | {<br/>"name": "updated name",<br/>"description": "updated description"<br/>} |

### Response

**[UpdateAPlaylistResponse](../../Models/Requests/UpdateAPlaylistResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Delete

This endpoint allows you to delete an existing playlist from the workspace. After deleted, the playlist and its metadata are permanently removed and cannot be recovered.
#### How it works
 - When a user sends a DELETE request to this endpoint with the `playlistId`, FastPix removes the specified playlist from the workspace and returns a confirmation of successful deletion.

#### Example
An e-learning platform deletes an outdated playlist titled "Old Python Tutorials" by providing its unique playlist ID. The platform receives confirmation that the playlist has been removed, ensuring learners no longer see the obsolete content.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="delete-a-playlist" method="delete" path="/on-demand/playlists/{playlistId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playlists.DeleteAsync(playlistId: "<id>");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PlaylistDeleteResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                         | Type                                              | Required                                          | Description                                       |
| ------------------------------------------------- | ------------------------------------------------- | ------------------------------------------------- | ------------------------------------------------- |
| `PlaylistId`                                      | *string*                                          | :heavy_check_mark:                                | The unique id of the playlist you want to delete. |

### Response

**[DeleteAPlaylistResponse](../../Models/Requests/DeleteAPlaylistResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## AddMedia

This endpoint allows you to add one or more media items to an existing playlist. By passing the media ID(s) in the request, the specified media items are appended to the playlist in the order provided.
#### How it works

 - When a user sends a PATCH request to this endpoint with the `playlistId` as path parameter and a list of media ID(s) in the request body, FastPix adds the specified media items to the playlist and returns the updated playlist details.

#### Example
An e-learning platform adds new video tutorials to the "Beginner Python Series" playlist by sending their media IDs in the request. The playlist is updated with the new content, ensuring learners have access to the latest tutorials in sequence.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="add-media-to-playlist" method="patch" path="/on-demand/playlists/{playlistId}/media" -->
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

var res = await sdk.Playlists.AddMediaAsync(
    playlistId: "<playlistId>",
    body: new MediaIdsRequest() {
        MediaIds = new List<string>() {
            "<mediaId1>",
            "<mediaId2>",
            "<mediaId3>",
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PlaylistByIdResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `PlaylistId`                                                        | *string*                                                            | :heavy_check_mark:                                                  | The unique id of the playlist you want to perform the operation on. |
| `Body`                                                              | [MediaIdsRequest](../../Models/Components/MediaIdsRequest.md)       | :heavy_check_mark:                                                  | N/A                                                                 |

### Response

**[AddMediaToPlaylistResponse](../../Models/Requests/AddMediaToPlaylistResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## ReorderMedia

This endpoint allows you to change the order of media items within a playlist. By passing the complete list of media IDs in the desired sequence, the playlist's play order is updated accordingly.
#### How it works

 - When a user sends a PUT request to this endpoint with the `playlistId` as path parameter and the reordered list of all media IDs in the request body, FastPix updates the playlist to reflect the new media sequence and returns the updated playlist details.

#### Example
An e-learning platform rearranges the "Beginner Python Series" playlist by submitting a reordered list of media IDs. The playlist now follows the new sequence, providing learners with a better structured learning path.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="change-media-order-in-playlist" method="put" path="/on-demand/playlists/{playlistId}/media" -->
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

var res = await sdk.Playlists.ReorderMediaAsync(
    playlistId: "<id>",
    body: new MediaIdsRequest() {
        MediaIds = new List<string>() {
            "<mediaId1>",
            "<mediaId2>",
            "<mediaId3>",
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PlaylistByIdResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `PlaylistId`                                                        | *string*                                                            | :heavy_check_mark:                                                  | The unique id of the playlist you want to perform the operation on. |
| `Body`                                                              | [MediaIdsRequest](../../Models/Components/MediaIdsRequest.md)       | :heavy_check_mark:                                                  | N/A                                                                 |

### Response

**[ChangeMediaOrderInPlaylistResponse](../../Models/Requests/ChangeMediaOrderInPlaylistResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## DeleteMedia

This endpoint allows you to delete one or more media items from an existing playlist. By passing the media ID(s) in the request, the specified media items are removed from the playlist.
#### How it works

 - When a user sends a DELETE request to this endpoint with the playlist ID as the path parameter and the media ID(s) to be removed in the request body, FastPix deletes the specified media items from the playlist and returns the updated playlist details.

#### Example
An e-learning platform removes outdated video tutorials from the "Beginner Python Series" playlist by specifying their media IDs in the request. The playlist is updated to exclude these items, ensuring learners only access relevant content.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="delete-media-from-playlist" method="delete" path="/on-demand/playlists/{playlistId}/media" -->
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

var res = await sdk.Playlists.DeleteMediaAsync(
    playlistId: "<id>",
    body: new MediaIdsRequest() {
        MediaIds = new List<string>() {
            "<mediaId1>",
            "<mediaId2>",
            "<mediaId3>",
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.PlaylistByIdResponse,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `PlaylistId`                                                        | *string*                                                            | :heavy_check_mark:                                                  | The unique id of the playlist you want to perform the operation on. |
| `Body`                                                              | [MediaIdsRequest](../../Models/Components/MediaIdsRequest.md)       | :heavy_minus_sign:                                                  | N/A                                                                 |

### Response

**[DeleteMediaFromPlaylistResponse](../../Models/Requests/DeleteMediaFromPlaylistResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |