# Playlist
(*Playlist*)

## Overview

### Available Operations

* [GetById](#getbyid) - Get a playlist by ID
* [Update](#update) - Update a playlist by ID

## GetById

This endpoint retrieves detailed information about a specific playlist using its unique `playlistId`. It provides comprehensive metadata about the playlist, including its title, creation mode (manual or smart), media items along with the metadata of each media in the playlist.

 
#### Example
An e-learning platform requests details for the playlist "Beginner Python Series" by providing its unique `playlistId`. The response includes the playlist's title, creation mode, and the ordered list of video tutorials contained within, enabling the platform to present the full learning path to users.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get-playlist-by-id" method="get" path="/on-demand/playlists/{playlistId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.Playlist.GetByIdAsync(playlistId: "<id>");

// handle response
```

### Parameters

| Parameter                                           | Type                                                | Required                                            | Description                                         |
| --------------------------------------------------- | --------------------------------------------------- | --------------------------------------------------- | --------------------------------------------------- |
| `PlaylistId`                                        | *string*                                            | :heavy_check_mark:                                  | The unique id of the playlist you want to retrieve. |

### Response

**[GetPlaylistByIdResponse](../../Models/Requests/GetPlaylistByIdResponse.md)**

### Errors

| Error Type                                               | Status Code                                              | Content Type                                             |
| -------------------------------------------------------- | -------------------------------------------------------- | -------------------------------------------------------- |
| Fastpix.Models.Errors.UnauthorizedException              | 401                                                      | application/json                                         |
| Fastpix.Models.Errors.NotFoundError                      | 404                                                      | application/json                                         |
| Fastpix.Models.Errors.InvalidPlaylistIdResponseException | 422                                                      | application/json                                         |
| Fastpix.Models.Errors.APIException                       | 4XX, 5XX                                                 | \*/\*                                                    |

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

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.Playlist.UpdateAsync(
    playlistId: "<id>",
    updatePlaylistRequest: new UpdatePlaylistRequest() {
        Name = "updated name",
        Description = "updated description",
    }
);

// handle response
```

### Parameters

| Parameter                                                                 | Type                                                                      | Required                                                                  | Description                                                               | Example                                                                   |
| ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| `PlaylistId`                                                              | *string*                                                                  | :heavy_check_mark:                                                        | The unique id of the playlist you want to retrieve.                       |                                                                           |
| `UpdatePlaylistRequest`                                                   | [UpdatePlaylistRequest](../../Models/Components/UpdatePlaylistRequest.md) | :heavy_check_mark:                                                        | N/A                                                                       | {<br/>"name": "updated name",<br/>"description": "updated description"<br/>} |

### Response

**[UpdateAPlaylistResponse](../../Models/Requests/UpdateAPlaylistResponse.md)**

### Errors

| Error Type                                               | Status Code                                              | Content Type                                             |
| -------------------------------------------------------- | -------------------------------------------------------- | -------------------------------------------------------- |
| Fastpix.Models.Errors.UnauthorizedException              | 401                                                      | application/json                                         |
| Fastpix.Models.Errors.InvalidPermissionException         | 403                                                      | application/json                                         |
| Fastpix.Models.Errors.InvalidPlaylistIdResponseException | 422                                                      | application/json                                         |
| Fastpix.Models.Errors.APIException                       | 4XX, 5XX                                                 | \*/\*                                                    |