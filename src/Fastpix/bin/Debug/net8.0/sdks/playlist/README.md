# Playlist

## Overview

Operations for playlist management

### Available Operations

* [Create](#create) - Create a new playlist

## Create

This endpoint creates a new playlist within a specified workspace. A playlist acts as a container for organizing media items either manually or based on filters and metadata. <br> <br>
### Playlists can be created in two modes
- **Manual:** Creates an empty playlist without any initial media items. Use this mode for manual curation, where you add items later in a user-defined sequence.
- **Smart:** Auto-populates the playlist at creation time based on the filter criteria (for example, a video creation date range) that you provide in the request.

For more details, see <a href="https://docs.fastpix.io/docs/create-and-manage-playlist">Create and manage playlist</a>.

#### How it works 

 - When you send a `POST` request to this endpoint, FastPix creates a playlist and returns a playlist ID, using which items can be added later in a user-defined sequence.
 - You can create a smart playlist that is auto-populated based on the metadata in the request body.


#### Example
An e-learning platform creates a new playlist titled Beginner Python Series through the API. The response returns a unique playlist ID. The platform uses this ID to add a series of video tutorials to the playlist in a defined order. The playlist appears on the frontend as a structured learning path for learners.

### Example Usage

<!-- UsageSnippet language="csharp" operationID="create-a-playlist" method="post" path="/on-demand/playlists" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

CreatePlaylistRequest req = CreatePlaylistRequest.CreateSmart(
    new CreatePlaylistRequestSmart() {
        Name = "playlist name",
        ReferenceId = "a1",
        Type = CreatePlaylistRequestSmartType.Smart,
        Description = "This is a playlist",
        PlayOrder = PlaylistOrder.CreatedDateASC,
        Limit = 20,
        Metadata = new Metadata() {
            CreatedDate = new DateRange() {
                StartDate = "2024-11-11",
                EndDate = "2024-12-12",
            },
            UpdatedDate = new DateRange() {
                StartDate = "2024-11-11",
                EndDate = "2024-12-12",
            },
        },
    }
);

var res = await sdk.Playlist.CreateAsync(req);

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

| Parameter                                                                 | Type                                                                      | Required                                                                  | Description                                                               |
| ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| `request`                                                                 | [CreatePlaylistRequest](../../Models/Components/CreatePlaylistRequest.md) | :heavy_check_mark:                                                        | The request object to use for the request.                                |

### Response

**[CreateAPlaylistResponse](../../Models/Requests/CreateAPlaylistResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |