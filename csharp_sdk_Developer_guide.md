

#  Fastpix - C#

The FastPix C# SDK provides a powerful, developer-friendly way to build video capabilities into your .NET applications. You get support for media uploads, live streaming, simulcasting, playback management, DRM configurations, signing keys, and more, so you can build sophisticated video-based applications for your use case.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Step 1: Installation](#step-1-installation)
- [Step 2: Import the SDK](#step-2-import-the-sdk)
- [Step 3: Initialization](#step-3-initialization)
- [Perform Media Operations](#perform-media-operations)
  - [Basic Media Operations](#11-create-media-from-url)
  - [AI-Powered Media Features](#19-ai-powered-media-features)
  - [Media Tracks Management](#110-media-tracks-management)
  - [Upload Management](#111-upload-management)
  - [Media Clips](#112-media-clips)
  - [Playlist Management](#113-playlist-management)
- [Live Stream Operations](#live-stream-operations)
  - [Basic Live Stream Operations](#21-create-live-stream)
  - [Live Stream Control Operations](#213-live-stream-control-operations)
  - [Simulcast Management](#214-simulcast-management)
- [DRM Configuration Operations](#drm-configuration-operations)
- [Signing Keys Operations](#signing-keys-operations)
- [Video Data Operations](#video-data-operations)
  - [Views Analytics](#51-views-analytics)
  - [Dimensions Analytics](#52-dimensions-analytics)
  - [Metrics Analytics](#53-metrics-analytics)
  - [Error Analytics](#54-error-analytics)
- [Error Handling](#error-handling)
- [Support & Resources](#support--resources)

## Prerequisites

Before you start using the SDK, make sure you have the following:

* **.NET 8.0 or later**: This SDK is compatible with .NET 8.0 or higher
* **Visual Studio 2022 or VS Code**: Recommended IDE for C# development
* **NuGet Package Manager**: Required for package management
* **FastPix API credentials**: You'll need an **Access Token** and a **Secret Key**. You can generate these credentials by following the steps in the Authentication guide
* **Basic understanding of C# and REST APIs**: Familiarity with C# development and API integration concepts

## Step 1: Installation

The SDK is available through NuGet and can be installed using your preferred method.

### Using Package Manager Console

In Visual Studio, open the Package Manager Console and run:

```powershell
Install-Package Fastpix
```

### Using .NET CLI

```bash
dotnet add package Fastpix
```

### Using PackageReference

Add this to your `.csproj` file:

```xml
<PackageReference Include="Fastpix" Version="1.0.0" />
```

### Alternative Installation Methods

#### From Source
```bash
git clone https://github.com/FastPix/fastpix-csharp.git
cd fastpix-csharp
dotnet add reference src/Fastpix/Fastpix.csproj
```

#### Development Installation
For development or testing purposes, you can install the SDK in development mode:

```bash
dotnet add package Fastpix --version 1.0.0
```

## Step 2: Import the SDK

Add the necessary using statements to your C# files:

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Models.Errors;
using Fastpix.Utils.Retries;
```

## Step 3: Initialization

You can set the security parameters through the builder when initializing the SDK client instance. The SDK uses different server URLs for different operations:

### For Media Operations

Use the base FastPix server URL:

```csharp
var sdk = FastPix.Builder()
    .WithSecurity(new Security
    {
        Username = "your-access-token",
        Password = "your-secret-key",
    })
    .WithServerUrl("https://api.fastpix.io/v1/")
    .Build();
```

### For Live Stream Operations

Use the live streaming specific server URL:

```csharp
var sdk = FastPix.Builder()
    .WithSecurity(new Security
    {
        Username = "your-access-token",
        Password = "your-secret-key",
    })
    .WithServerUrl("https://api.fastpix.io/v1/")
    .Build();
```

### Environment Variables (Recommended)

For production applications, consider using environment variables for credentials:

```csharp
var sdk = FastPix.Builder()
    .WithSecurity(new Security
    {
        Username = Environment.GetEnvironmentVariable("FASTPIX_ACCESS_TOKEN"),
        Password = Environment.GetEnvironmentVariable("FASTPIX_SECRET_KEY"),
    })
    .Build();
```

## Perform Media Operations

### 1.1 Create Media from URL

Create media by providing a URL to an existing video file. This is useful when you have videos hosted on external platforms like AWS S3, Google Cloud Storage, or any publicly accessible URL.

```csharp
var request = new CreateMediaRequest
{
    Inputs = new List<Input>
    {
        Input.CreateVideoInput(new VideoInput
        {
            Type = "video",
            Url = "https://static.fastpix.io/sample.mp4",
        })
    },
    Metadata = new Dictionary<string, string>
    {
        { "title", "My Video" },
        { "description", "Video created from URL" }
    },
    AccessPolicy = CreateMediaRequestAccessPolicy.Public
};

var response = await sdk.Media.CreateMediaAsync(request);

if (response.CreateMediaSuccessResponse != null)
{
    Console.WriteLine("Media created successfully!");
    Console.WriteLine($"Media ID: {response.CreateMediaSuccessResponse.Id}");
}
```

### 1.2 Direct Upload Media from Device

Upload media files directly from your local device or server using the direct upload endpoint:

```csharp
var request = new DirectUploadVideoMediaRequest
{
    CorsOrigin = "*",
    PushMediaSettings = new PushMediaSettings
    {
        AccessPolicy = BasicAccessPolicy.Public,
        Metadata = new Dictionary<string, string>
        {
            { "title", "Uploaded Video" },
            { "category", "user-generated" }
        }
    }
};

var response = await sdk.Videos.UploadAsync(request);

if (response.DirectUploadVideoMediaSuccessResponse != null)
{
    Console.WriteLine("Direct upload initiated successfully!");
    Console.WriteLine($"Upload ID: {response.DirectUploadVideoMediaSuccessResponse.Data.UploadId}");
    Console.WriteLine($"Upload URL: {response.DirectUploadVideoMediaSuccessResponse.Data.Url}");
}
```

### 1.3 List All Media

Retrieve a list of all media in your workspace with pagination support:

```csharp
var response = await sdk.ManageVideos.ListVideosAsync(
    offset: 1,
    limit: 10
);

if (response.ListVideosSuccessResponse != null)
{
    var videos = response.ListVideosSuccessResponse.Data;
    foreach (var video in videos)
    {
        Console.WriteLine($"Video ID: {video.Id}");
        Console.WriteLine($"Title: {video.Metadata?.GetValueOrDefault("title", "No title")}");
    }
}
```

### 1.4 Get Media by ID

Retrieve detailed information about a specific media item:

```csharp
var mediaId = "your-media-id";

var response = await sdk.ManageVideos.GetVideoByIdAsync(mediaId);

if (response.GetVideoByIdSuccessResponse != null)
{
    var video = response.GetVideoByIdSuccessResponse.Data;
    Console.WriteLine($"Video ID: {video.Id}");
    Console.WriteLine($"Status: {video.Status}");
    Console.WriteLine($"Created At: {video.CreatedAt:yyyy-MM-dd HH:mm:ss}");
}
```

### 1.5 Update Media

Update metadata and settings for an existing media item:

```csharp
var mediaId = "your-media-id";

var request = new UpdatedMediaRequestBody
{
    Metadata = new Dictionary<string, string>
    {
        { "title", "Updated Video Title" },
        { "description", "Updated description" },
        { "category", "updated-category" }
    }
};

var response = await sdk.Videos.UpdateAsync(mediaId, request);

if (response.UpdatedMediaSuccessResponse != null)
{
    Console.WriteLine("Media updated successfully!");
}
```

### 1.6 Media Configuration Operations

#### 1.6.1 Update Source Access

Update the source access policy for a media item:

```csharp
var mediaId = "your-media-id";

var request = new UpdateSourceAccessRequestBody
{
    AccessPolicy = UpdateSourceAccessRequestBodyAccessPolicy.Private
};

var response = await sdk.ManageVideos.UpdateSourceAccessAsync(mediaId, request);

if (response.UpdateSourceAccessSuccessResponse != null)
{
    Console.WriteLine("Source access updated successfully!");
}
```

#### 1.6.2 Update MP4 Support

Enable or disable MP4 support for a media item:

```csharp
var mediaId = "your-media-id";

var request = new UpdateMp4SupportRequestBody
{
    Mp4Support = true
};

var response = await sdk.ManageVideos.UpdateMp4SupportAsync(mediaId, request);

if (response.UpdateMp4SupportSuccessResponse != null)
{
    Console.WriteLine("MP4 support updated successfully!");
}
```

#### 1.6.3 Get Media Input Info

Retrieve input information for a media item:

```csharp
var mediaId = "your-media-id";

var response = await sdk.ManageVideos.RetrieveMediaInputInfoAsync(mediaId);

if (response.RetrieveMediaInputInfoSuccessResponse != null)
{
    var inputInfo = response.RetrieveMediaInputInfoSuccessResponse.Data;
    Console.WriteLine($"Input URL: {inputInfo.InputUrl}");
    Console.WriteLine($"Input Type: {inputInfo.InputType}");
    Console.WriteLine($"File Size: {inputInfo.FileSize}");
}
```

### 1.7 Delete Media

Remove a media item from your workspace:

```csharp
var mediaId = "your-media-id";

var response = await sdk.ManageVideos.DeleteVideoAsync(mediaId);

if (response.StatusCode == 204)
{
    Console.WriteLine("Media deleted successfully!");
}
```

### 1.8 Playback Operations

#### 1.8.1 Create Media Playback ID

Generate a playback ID for secure video access:

```csharp
var mediaId = "your-media-id";

var request = new CreateMediaPlaybackIdRequestBody
{
    AccessPolicy = AccessPolicy.Public
};

var response = await sdk.Playback.CreateAsync(mediaId, request);

if (response.CreateMediaPlaybackIdSuccessResponse != null)
{
    var playbackId = response.CreateMediaPlaybackIdSuccessResponse.Data.Id;
    Console.WriteLine($"Playback ID created: {playbackId}");
}
```

#### 1.8.2 Get Playback ID

Retrieve details of a specific playback ID:

```csharp
var mediaId = "your-media-id";
var playbackId = "your-playback-id";

var response = await sdk.Playback.GetByIdAsync(mediaId, playbackId);

if (response.GetPlaybackIdSuccessResponse != null)
{
    var playback = response.GetPlaybackIdSuccessResponse.Data;
    Console.WriteLine($"Playback ID: {playback.Id}");
    Console.WriteLine($"Access Policy: {playback.AccessPolicy}");
}
```

#### 1.8.3 Delete Media Playback ID

Remove a playback ID:

```csharp
var mediaId = "your-media-id";
var playbackId = "your-playback-id";

var response = await sdk.Playback.DeleteAsync(mediaId, playbackId);

if (response.StatusCode == 204)
{
    Console.WriteLine("Playback ID deleted successfully!");
}
```

### 1.9 AI-Powered Media Features

The FastPix SDK includes powerful AI features for media enhancement and analysis:

#### 1.9.1 Generate Video Summary

Generate an AI-powered summary of your video content:

```csharp
var mediaId = "your-media-id";

var requestBody = new UpdateMediaSummaryRequestBody
{
    Generate = true,
    SummaryLength = 120 // Optional: specify word count
};

var response = await sdk.InVideoAiFeatures.UpdateMediaSummaryAsync(mediaId, requestBody);

if (response.UpdateMediaSummarySuccessResponse != null)
{
    Console.WriteLine("Video summary generation initiated successfully!");
}
```

#### 1.9.2 Generate Video Chapters

Automatically generate chapters for your video:

```csharp
var mediaId = "your-media-id";

var requestBody = new UpdateMediaChaptersRequestBody
{
    Chapters = true
};

var response = await sdk.InVideoAiFeatures.UpdateMediaChaptersAsync(mediaId, requestBody);

if (response.UpdateMediaChaptersSuccessResponse != null)
{
    Console.WriteLine("Video chapters generation initiated successfully!");
}
```

#### 1.9.3 Extract Named Entities

Extract named entities (people, places, organizations) from your video:

```csharp
var mediaId = "your-media-id";

var requestBody = new UpdateMediaNamedEntitiesRequestBody
{
    NamedEntities = true
};

var response = await sdk.InVideoAiFeatures.UpdateMediaNamedEntitiesAsync(mediaId, requestBody);

if (response.UpdateMediaNamedEntitiesSuccessResponse != null)
{
    Console.WriteLine("Named entities extraction initiated successfully!");
}
```

#### 1.9.4 Content Moderation

Enable AI-powered content moderation for inappropriate content:

```csharp
var mediaId = "your-media-id";

var requestBody = new UpdateMediaModerationRequestBody
{
    Moderation = new UpdateMediaModerationModeration
    {
        Type = MediaType.Video
    }
};

var response = await sdk.InVideoAiFeatures.UpdateMediaModerationAsync(mediaId, requestBody);

if (response.UpdateMediaModerationSuccessResponse != null)
{
    Console.WriteLine("Content moderation enabled successfully!");
}
```

### 1.10 Media Tracks Management

Manage audio and subtitle tracks for your media:

#### 1.10.1 Add Media Track

Add an audio or subtitle track to your media:

```csharp
var mediaId = "your-media-id";

var request = new AddTrackRequestBody
{
    Type = AddTrackRequestBodyType.Audio,
    Language = "en",
    Url = "https://example.com/audio-track.mp3"
};

var response = await sdk.ManageVideos.AddMediaTrackAsync(mediaId, request);

if (response.AddMediaTrackSuccessResponse != null)
{
    Console.WriteLine("Media track added successfully!");
}
```

#### 1.10.2 Update Media Track

Update an existing media track:

```csharp
var mediaId = "your-media-id";
var trackId = "your-track-id";

var request = new UpdateTrackRequestBody
{
    Language = "es",
    Url = "https://example.com/updated-track.mp3"
};

var response = await sdk.ManageVideos.UpdateMediaTrackAsync(mediaId, trackId, request);

if (response.UpdateMediaTrackSuccessResponse != null)
{
    Console.WriteLine("Media track updated successfully!");
}
```

#### 1.10.3 Delete Media Track

Remove a media track:

```csharp
var mediaId = "your-media-id";
var trackId = "your-track-id";

var response = await sdk.ManageVideos.DeleteMediaTrackAsync(mediaId, trackId);

if (response.StatusCode == 204)
{
    Console.WriteLine("Media track deleted successfully!");
}
```

#### 1.10.4 Generate Subtitle Track

Generate subtitles for your media:

```csharp
var mediaId = "your-media-id";
var trackId = "your-track-id";

var response = await sdk.ManageVideos.GenerateSubtitleTrackAsync(mediaId, trackId);

if (response.GenerateSubtitleTrackSuccessResponse != null)
{
    Console.WriteLine("Subtitle generation initiated successfully!");
}
```

### 1.11 Upload Management

Manage uploads and track their status:

#### 1.11.1 List Uploads

Get all unused upload URLs:

```csharp
var response = await sdk.Uploads.ListAsync(
    offset: 1,
    limit: 10
);

if (response.ListUploadsSuccessResponse != null)
{
    var uploads = response.ListUploadsSuccessResponse.Data;
    foreach (var upload in uploads)
    {
        Console.WriteLine($"Upload ID: {upload.Id}");
        Console.WriteLine($"Status: {upload.Status}");
    }
}
```

#### 1.11.2 Cancel Upload

Cancel an ongoing upload:

```csharp
var uploadId = "your-upload-id";

var response = await sdk.Uploads.CancelAsync(uploadId);

if (response.StatusCode == 200)
{
    Console.WriteLine("Upload cancelled successfully!");
}
```

### 1.12 Media Clips

Work with media clips and live stream clips:

#### 1.12.1 Get Media Clips

Retrieve all clips of a media file:

```csharp
var sourceMediaId = "your-source-media-id";

var response = await sdk.ManageVideos.GetMediaClipsAsync(sourceMediaId);

if (response.GetMediaClipsSuccessResponse != null)
{
    var clips = response.GetMediaClipsSuccessResponse.Data;
    foreach (var clip in clips)
    {
        Console.WriteLine($"Clip ID: {clip.Id}");
        Console.WriteLine($"Start Time: {clip.StartTime}");
        Console.WriteLine($"End Time: {clip.EndTime}");
    }
}
```

#### 1.12.2 Get Live Clips

Retrieve all clips of a live stream:

```csharp
var livestreamId = "your-livestream-id";

var response = await sdk.ManageVideos.ListLiveClipsAsync(livestreamId);

if (response.ListLiveClipsSuccessResponse != null)
{
    var clips = response.ListLiveClipsSuccessResponse.Data;
    foreach (var clip in clips)
    {
        Console.WriteLine($"Clip ID: {clip.Id}");
        Console.WriteLine($"Duration: {clip.Duration}");
    }
}
```

### 1.13 Playlist Management

Create and manage playlists for organizing your media content:

#### 1.13.1 Create Playlist

Create a new playlist:

```csharp
var request = new CreatePlaylistRequest
{
    Name = "My Video Playlist",
    Description = "A collection of my favorite videos",
    Type = CreatePlaylistRequestType.Manual,
    PlayOrder = PlaylistOrder.CreatedDateASC,
    Metadata = new Dictionary<string, string>
    {
        { "category", "entertainment" },
        { "tags", "videos,playlist" }
    }
};

var response = await sdk.Playlists.CreateAsync(request);

if (response.CreateAPlaylistSuccessResponse != null)
{
    Console.WriteLine("Playlist created successfully!");
    Console.WriteLine($"Playlist ID: {response.CreateAPlaylistSuccessResponse.Data.Id}");
}
```

#### 1.13.2 List All Playlists

Retrieve all playlists in your workspace:

```csharp
var response = await sdk.Playlists.ListAsync(
    offset: 1,
    limit: 10
);

if (response.GetAllPlaylistsSuccessResponse != null)
{
    var playlists = response.GetAllPlaylistsSuccessResponse.Data;
    foreach (var playlist in playlists)
    {
        Console.WriteLine($"Playlist ID: {playlist.Id}");
        Console.WriteLine($"Name: {playlist.Name}");
        Console.WriteLine($"Media Count: {playlist.Media?.Count ?? 0}");
    }
}
```

#### 1.13.3 Get Playlist by ID

Retrieve details of a specific playlist:

```csharp
var playlistId = "your-playlist-id";

var response = await sdk.Playlists.GetByIdAsync(playlistId);

if (response.GetPlaylistByIdSuccessResponse != null)
{
    var playlist = response.GetPlaylistByIdSuccessResponse.Data;
    Console.WriteLine($"Playlist Name: {playlist.Name}");
    Console.WriteLine($"Description: {playlist.Description}");
    Console.WriteLine($"Media Count: {playlist.Media?.Count ?? 0}");
}
```

#### 1.13.4 Update Playlist

Update playlist details:

```csharp
var playlistId = "your-playlist-id";

var request = new UpdatePlaylistRequestBody
{
    Name = "Updated Playlist Name",
    Description = "Updated description"
};

var response = await sdk.Playlists.UpdateAsync(playlistId, request);

if (response.UpdatePlaylistSuccessResponse != null)
{
    Console.WriteLine("Playlist updated successfully!");
}
```

#### 1.13.5 Add Media to Playlist

Add media items to a playlist:

```csharp
var playlistId = "your-playlist-id";

var request = new MediaIdsRequest
{
    MediaIds = new List<string> { "media-id-1", "media-id-2" }
};

var response = await sdk.Playlists.AddMediaAsync(playlistId, request);

if (response.AddMediaToPlaylistSuccessResponse != null)
{
    Console.WriteLine("Media added to playlist successfully!");
}
```

#### 1.13.6 Replace Playlist Media

Replace all media in a playlist:

```csharp
var playlistId = "your-playlist-id";

var request = new MediaIdsRequest
{
    MediaIds = new List<string> { "new-media-id-1", "new-media-id-2" }
};

var response = await sdk.Playlists.ChangeMediaOrderAsync(playlistId, request);

if (response.ChangeMediaOrderInPlaylistSuccessResponse != null)
{
    Console.WriteLine("Playlist media replaced successfully!");
}
```

#### 1.13.7 Delete Media from Playlist

Remove specific media from a playlist:

```csharp
var playlistId = "your-playlist-id";

var request = new MediaIdsRequest
{
    MediaIds = new List<string> { "media-id-to-remove" }
};

var response = await sdk.Playlists.DeleteMediaAsync(playlistId, request);

if (response.DeleteMediaFromPlaylistSuccessResponse != null)
{
    Console.WriteLine("Media removed from playlist successfully!");
}
```

#### 1.13.8 Delete Playlist

Remove an entire playlist:

```csharp
var playlistId = "your-playlist-id";

var response = await sdk.Playlists.DeleteAsync(playlistId);

if (response.StatusCode == 204)
{
    Console.WriteLine("Playlist deleted successfully!");
}
```

## Live Stream Operations

### 2.1 Create Live Stream

Create a new live stream for broadcasting:

```csharp
var request = new CreateLiveStreamRequest
{
    PlaybackSettings = new PlaybackSettings
    {
        AccessPolicy = BasicAccessPolicy.Public
    },
    InputMediaSettings = new InputMediaSettings
    {
        MediaPolicy = BasicAccessPolicy.Public,
        Metadata = new Dictionary<string, string>
        {
            { "title", "My Live Stream" }
        },
        EnableDvrMode = true
    }
};

var response = await sdk.LiveStreams.CreateAsync(request);

if (response.CreateNewStreamSuccessResponse != null)
{
    var streamId = response.CreateNewStreamSuccessResponse.Data.StreamId;
    Console.WriteLine("Live stream created successfully!");
    Console.WriteLine($"Stream ID: {streamId}");
}
```

### 2.2 List All Live Streams

Retrieve all live streams in your workspace:

```csharp
var response = await sdk.LiveStreams.ListAsync(
    limit: 10,
    offset: 1,
    orderBy: OrderBy.Desc
);

if (response.GetAllStreamsSuccessResponse != null)
{
    var streams = response.GetAllStreamsSuccessResponse.Data;
    foreach (var stream in streams)
    {
        Console.WriteLine($"Stream ID: {stream.StreamId}");
        Console.WriteLine($"Name: {stream.Name}");
        Console.WriteLine($"Status: {stream.Status}");
    }
}
```

### 2.3 Get Live Stream by ID

Retrieve details of a specific live stream:

```csharp
var streamId = "your-stream-id";

var response = await sdk.Streams.GetByIdAsync(streamId);

if (response.GetLiveStreamByIdSuccessResponse != null)
{
    var stream = response.GetLiveStreamByIdSuccessResponse.Data;
    Console.WriteLine($"Stream ID: {stream.StreamId}");
    Console.WriteLine($"Name: {stream.Name}");
    Console.WriteLine($"Status: {stream.Status}");
    Console.WriteLine($"Stream URL: {stream.StreamUrl}");
}
```

### 2.4 Update Live Stream

Update live stream settings and metadata:

```csharp
var streamId = "your-stream-id";

var request = new PatchLiveStreamRequest
{
    Metadata = new Dictionary<string, string>
    {
        { "title", "Updated Live Event" },
        { "description", "Updated description" }
    },
    ReconnectWindow = 30
};

var response = await sdk.Streams.UpdateAsync(streamId, request);

if (response.UpdateLiveStreamSuccessResponse != null)
{
    Console.WriteLine("Live stream updated successfully!");
}
```

### 2.5 Delete Live Stream

Remove a live stream:

```csharp
var streamId = "your-stream-id";

var response = await sdk.Streams.DeleteAsync(streamId);

if (response.StatusCode == 204)
{
    Console.WriteLine("Live stream deleted successfully!");
}
```

### 2.6 Create Live Stream Playback ID

Generate a playback ID for live stream access:

```csharp
var streamId = "your-stream-id";

var request = new CreatePlaybackIdOfStreamRequestBody
{
    AccessPolicy = CreatePlaybackIdOfStreamRequestAccessPolicy.Public
};

var response = await sdk.LivePlayback.CreatePlaybackIdOfStreamAsync(streamId, request);

if (response.CreatePlaybackIdOfStreamSuccessResponse != null)
{
    var playbackId = response.CreatePlaybackIdOfStreamSuccessResponse.Data.Id;
    Console.WriteLine($"Live stream playback ID created: {playbackId}");
}
```

### 2.7 Delete Live Stream Playback ID

Remove a live stream playback ID:

```csharp
var streamId = "your-stream-id";
var playbackId = "your-playback-id";

var response = await sdk.LivePlayback.DeletePlaybackIdOfStreamAsync(streamId, playbackId);

if (response.StatusCode == 204)
{
    Console.WriteLine("Live stream playback ID deleted successfully!");
}
```

### 2.8 Get Live Stream Playback ID

Retrieve playback details for a live stream:

```csharp
var streamId = "your-stream-id";
var playbackId = "your-playback-id";

var response = await sdk.LivePlayback.GetPlaybackIdOfStreamAsync(streamId, playbackId);

if (response.GetPlaybackIdOfStreamSuccessResponse != null)
{
    var playback = response.GetPlaybackIdOfStreamSuccessResponse.Data;
    Console.WriteLine($"Playback Policy: {playback.AccessPolicy}");
}
```

### 2.9 Create Simulcast

Create a simulcast for broadcasting to multiple platforms simultaneously:

```csharp
var streamId = "your-stream-id";

var request = new CreateSimulcastOfStreamRequestBody
{
    Platform = CreateSimulcastOfStreamRequestPlatform.YOUTUBE,
    Credentials = new CreateSimulcastOfStreamRequestCredentials
    {
        StreamKey = "your-youtube-stream-key"
    }
};

var response = await sdk.SimulcastStream.CreateSimulcastOfStreamAsync(streamId, request);

if (response.SimulcastResponse != null)
{
    Console.WriteLine("Simulcast created successfully!");
    Console.WriteLine($"Simulcast ID: {response.SimulcastResponse.Data.Id}");
}
```

### 2.10 Get Simulcast Details

Retrieve details of a specific simulcast:

```csharp
var streamId = "your-stream-id";
var simulcastId = "your-simulcast-id";

var response = await sdk.SimulcastStream.GetSpecificSimulcastOfStreamAsync(streamId, simulcastId);

if (response.SimulcastResponse != null)
{
    var simulcast = response.SimulcastResponse.Data;
    Console.WriteLine($"Simulcast ID: {simulcast.Id}");
    Console.WriteLine($"Platform: {simulcast.Platform}");
    Console.WriteLine($"Status: {simulcast.Status}");
}
```

### 2.11 Update Simulcast

Update simulcast configuration:

```csharp
var streamId = "your-stream-id";
var simulcastId = "your-simulcast-id";

var request = new UpdateSimulcastOfStreamRequestBody
{
    Status = UpdateSimulcastOfStreamRequestStatus.IDLE
};

var response = await sdk.SimulcastStream.UpdateSpecificSimulcastOfStreamAsync(streamId, simulcastId, request);

if (response.SimulcastResponse != null)
{
    Console.WriteLine("Simulcast updated successfully!");
}
```

### 2.12 Delete Simulcast

Remove a simulcast:

```csharp
var streamId = "your-stream-id";
var simulcastId = "your-simulcast-id";

var response = await sdk.SimulcastStream.DeleteSimulcastOfStreamAsync(streamId, simulcastId);

if (response.StatusCode == 204)
{
    Console.WriteLine("Simulcast deleted successfully!");
}
```

### 2.13 Live Stream Control Operations

#### 2.13.1 Enable Live Stream

Enable a live stream for broadcasting:

```csharp
var streamId = "your-stream-id";

var response = await sdk.Streams.EnableAsync(streamId);

if (response.StatusCode == 200)
{
    Console.WriteLine("Live stream enabled successfully!");
}
```

#### 2.13.2 Disable Live Stream

Disable a live stream:

```csharp
var streamId = "your-stream-id";

var response = await sdk.ManageLiveStream.DisableLiveStreamAsync(streamId);

if (response.StatusCode == 200)
{
    Console.WriteLine("Live stream disabled successfully!");
}
```

#### 2.13.3 Complete Live Stream

Mark a live stream as completed:

```csharp
var streamId = "your-stream-id";

var response = await sdk.ManageLiveStream.CompleteLiveStreamAsync(streamId);

if (response.StatusCode == 200)
{
    Console.WriteLine("Live stream completed successfully!");
}
```

#### 2.13.4 Get Live Stream Viewer Count

Get the current viewer count for a live stream:

```csharp
var streamId = "your-stream-id";

var response = await sdk.ManageLiveStream.GetLiveStreamViewerCountByIdAsync(streamId);

if (response.GetLiveStreamViewerCountByIdSuccessResponse != null)
{
    var viewerCount = response.GetLiveStreamViewerCountByIdSuccessResponse.Data;
    Console.WriteLine($"Current Viewers: {viewerCount.ViewerCount}");
    Console.WriteLine($"Peak Viewers: {viewerCount.PeakViewers}");
}
```

### 2.14 Simulcast Management

#### 2.14.1 List Simulcast Targets

Get all simulcast targets for a stream:

```csharp
var streamId = "your-stream-id";

var response = await sdk.SimulcastStreams.ListSimulcastsOfStreamAsync(streamId);

if (response.ListSimulcastsOfStreamSuccessResponse != null)
{
    var simulcasts = response.ListSimulcastsOfStreamSuccessResponse.Data;
    foreach (var simulcast in simulcasts)
    {
        Console.WriteLine($"Simulcast ID: {simulcast.Id}");
        Console.WriteLine($"Platform: {simulcast.Platform}");
        Console.WriteLine($"Status: {simulcast.Status}");
    }
}
```

## DRM Configuration Operations

### 3.1 Get DRM Configuration List

Retrieve all DRM configurations in your workspace:

```csharp
var response = await sdk.DrmConfigurations.ListAsync(
    offset: 1,
    limit: 10
);

if (response.GetDrmConfigurationSuccessResponse != null)
{
    var configurations = response.GetDrmConfigurationSuccessResponse.Data;
    foreach (var config in configurations)
    {
        Console.WriteLine($"DRM Configuration ID: {config.Id}");
        Console.WriteLine($"Name: {config.Name}");
    }
}
```

### 3.2 Get DRM Configuration by ID

Retrieve details of a specific DRM configuration:

```csharp
var drmConfigurationId = "4fa85f64-5717-4562-b3fc-2c963f66afa6";

var response = await sdk.DrmConfigurations.GetByIdAsync(drmConfigurationId);

if (response.GetDrmConfigurationByIdSuccessResponse != null)
{
    var config = response.GetDrmConfigurationByIdSuccessResponse.Data;
    Console.WriteLine($"DRM Configuration ID: {config.Id}");
    Console.WriteLine($"Name: {config.Name}");
    Console.WriteLine($"Description: {config.Description}");
}
```

## Signing Keys Operations

### 4.1 Create Signing Key

Generate a new signing key pair for JWT authentication:

```csharp
var response = await sdk.SigningKeys.CreateAsync();

if (response.CreateSigningKeySuccessResponse != null)
{
    var keyData = response.CreateSigningKeySuccessResponse.Data;
    Console.WriteLine("Signing key created successfully!");
    Console.WriteLine($"Key ID: {keyData.Id}");
    Console.WriteLine($"Private Key: {keyData.PrivateKey}");
    Console.WriteLine($"Created At: {keyData.CreatedAt:yyyy-MM-dd HH:mm:ss}");
}
```

### 4.2 List Signing Keys

Retrieve all signing keys in your workspace:

```csharp
var response = await sdk.SigningKeys.ListAsync(
    offset: 1,
    limit: 10
);

if (response.ListSigningKeysSuccessResponse != null)
{
    var keys = response.ListSigningKeysSuccessResponse.Data;
    foreach (var key in keys)
    {
        Console.WriteLine($"Key ID: {key.Id}");
        Console.WriteLine($"Created At: {key.CreatedAt:yyyy-MM-dd HH:mm:ss}");
    }
}
```

### 4.3 Get Signing Key by ID

Retrieve details of a specific signing key:

```csharp
var keyId = "your-key-id";

var response = await sdk.SigningKeys.GetByIdAsync(keyId);

if (response.GetSigningKeyByIdSuccessResponse != null)
{
    var key = response.GetSigningKeyByIdSuccessResponse.Data;
    Console.WriteLine($"Key ID: {key.Id}");
    Console.WriteLine($"Created At: {key.CreatedAt:yyyy-MM-dd HH:mm:ss}");
}
```

### 4.4 Delete Signing Key

Remove a signing key:

```csharp
var keyId = "your-key-id";

var response = await sdk.SigningKeys.DeleteAsync(keyId);

if (response.StatusCode == 204)
{
    Console.WriteLine("Signing key deleted successfully!");
}
```

## Video Data Operations

The FastPix SDK provides comprehensive analytics and data operations to help you understand your content performance and user engagement.

### 5.1 Views Analytics

#### 5.1.1 Get Views Data

Retrieve detailed view analytics for your videos:

```csharp
var request = new ListVideoViewsRequest
{
    Timespan = ListVideoViewsTimespan.Sevendays,
    Filterby = "browser_name:Chrome",
    Limit = 100
};

var response = await sdk.Views.ListAsync(request);

if (response.ListVideoViewsSuccessResponse != null)
{
    var views = response.ListVideoViewsSuccessResponse.Data;
    foreach (var view in views)
    {
        Console.WriteLine($"View ID: {view.ViewId}");
        Console.WriteLine($"Video ID: {view.VideoId}");
        Console.WriteLine($"Viewer ID: {view.ViewerId}");
        Console.WriteLine($"Watch Time: {view.WatchTime}");
    }
}
```

#### 5.1.2 Get Views by Dimension

Get view data filtered by specific dimensions:

```csharp
var viewId = "your-view-id";

var response = await sdk.Views.GetDetailsAsync(viewId);

if (response.GetVideoViewDetailsSuccessResponse != null)
{
    var viewData = response.GetVideoViewDetailsSuccessResponse.Data;
    Console.WriteLine($"View ID: {viewData.ViewId}");
    Console.WriteLine($"Video ID: {viewData.VideoId}");
    Console.WriteLine($"Watch Time: {viewData.WatchTime}");
}
```

#### 5.1.3 Get Top Content Views

Retrieve views data for your top-performing content:

```csharp
var response = await sdk.Views.ListTopContentAsync(
    timespan: ListByTopContentTimespan.Sevendays,
    limit: 10
);

if (response.ListByTopContentSuccessResponse != null)
{
    var topContent = response.ListByTopContentSuccessResponse.Data;
    foreach (var content in topContent)
    {
        Console.WriteLine($"Video ID: {content.VideoId}");
        Console.WriteLine($"Total Views: {content.TotalViews}");
        Console.WriteLine($"Rank: {content.Rank}");
    }
}
```

#### 5.1.4 Get Timeseries Views

Get concurrent viewers data over time:

```csharp
var response = await sdk.Views.GetTimeseriesAsync();

if (response.GetDataViewlistCurrentViewsGetTimeseriesViewsSuccessResponse != null)
{
    var timeseries = response.GetDataViewlistCurrentViewsGetTimeseriesViewsSuccessResponse.Data;
    foreach (var dataPoint in timeseries)
    {
        Console.WriteLine($"Timestamp: {dataPoint.IntervalTime}");
        Console.WriteLine($"Concurrent Viewers: {dataPoint.NumberOfViews}");
    }
}
```

#### 5.1.5 Get Views Count

Get filtered views count by dimension:

```csharp
var response = await sdk.Views.GetConcurrentViewersBreakdownAsync(
    dimension: GetDataViewlistCurrentViewsFilterDimension.Country,
    limit: 10
);

if (response.GetDataViewlistCurrentViewsFilterSuccessResponse != null)
{
    var breakdown = response.GetDataViewlistCurrentViewsFilterSuccessResponse.Data;
    foreach (var item in breakdown)
    {
        Console.WriteLine($"Dimension: {item.Dimension}");
        Console.WriteLine($"Value: {item.Value}");
        Console.WriteLine($"Count: {item.ConcurrentViewers}");
    }
}
```

### 5.2 Dimensions Analytics

#### 5.2.1 List Dimensions

Get all available dimensions for analytics:

```csharp
var response = await sdk.Dimensions.ListAsync();

if (response.ListDimensionsSuccessResponse != null)
{
    var dimensions = response.ListDimensionsSuccessResponse.Data;
    foreach (var dimension in dimensions)
    {
        Console.WriteLine($"Dimension: {dimension.Name}");
        Console.WriteLine($"Type: {dimension.Type}");
        Console.WriteLine($"Description: {dimension.Description}");
    }
}
```

#### 5.2.2 Get Dimensions Data

Retrieve dimensional analytics data:

```csharp
var dimensionsId = "your-dimensions-id";

var response = await sdk.Dimensions.GetByIdAsync(dimensionsId);

if (response.GetDimensionsDataSuccessResponse != null)
{
    var dimensionData = response.GetDimensionsDataSuccessResponse.Data;
    Console.WriteLine($"Dimension: {dimensionData.Name}");
    Console.WriteLine($"Values: {string.Join(", ", dimensionData.Values)}");
}
```

### 5.3 Metrics Analytics

#### 5.3.1 Get Metrics Breakdown

Get metrics data broken down by specific criteria:

```csharp
var request = new ListBreakdownValuesRequest
{
    MetricId = ListBreakdownValuesMetricId.QualityOfExperienceScore,
    Timespan = ListBreakdownValuesTimespan.Sevendays,
    Filterby = "browser_name:Chrome",
    GroupBy = "browser_name"
};

var response = await sdk.Metrics.ListBreakdownAsync(request);

if (response.ListBreakdownValuesSuccessResponse != null)
{
    var breakdown = response.ListBreakdownValuesSuccessResponse.Data;
    foreach (var item in breakdown)
    {
        Console.WriteLine($"Category: {item.Category}");
        Console.WriteLine($"Value: {item.Value}");
        Console.WriteLine($"Count: {item.Count}");
    }
}
```

#### 5.3.2 Get Overall Video Metrics

Retrieve overall metrics for your videos:

```csharp
var response = await sdk.Metrics.ListOverallAsync(
    metricId: ListOverallValuesMetricId.QualityOfExperienceScore,
    timespan: ListOverallValuesTimespan.Sevendays,
    measurement: "avg",
    filterby: "browser_name:Chrome"
);

if (response.ListOverallValuesSuccessResponse != null)
{
    var metrics = response.ListOverallValuesSuccessResponse.Data;
    Console.WriteLine($"Total Views: {metrics.TotalViews}");
    Console.WriteLine($"Unique Viewers: {metrics.UniqueViews}");
    Console.WriteLine($"Watch Time: {metrics.TotalWatchTime}");
    Console.WriteLine($"Engagement Rate: {metrics.Value}");
}
```

#### 5.3.3 Get Metrics Timeseries

Get metrics data over time:

```csharp
var request = new GetTimeseriesDataRequest
{
    MetricId = GetTimeseriesDataMetricId.QualityOfExperienceScore,
    Timespan = GetTimeseriesDataTimespan.Sevendays,
    Filterby = "browser_name:Chrome"
};

var response = await sdk.Metrics.GetTimeseriesAsync(request);

if (response.GetTimeseriesDataSuccessResponse != null)
{
    var timeseries = response.GetTimeseriesDataSuccessResponse.Data;
    foreach (var dataPoint in timeseries)
    {
        Console.WriteLine($"Date: {dataPoint.IntervalTime}");
        Console.WriteLine($"Value: {dataPoint.MetricValue}");
    }
}
```

#### 5.3.4 Get Metrics Comparison

Compare metrics across different periods or content:

```csharp
var response = await sdk.Metrics.ListComparisonValuesAsync(
    timespan: ListComparisonValuesTimespan.Sevendays,
    filterby: "browser_name:Chrome",
    dimension: ListComparisonValuesDimension.BrowserName,
    valueP: "Chrome"
);

if (response.ListComparisonValuesSuccessResponse != null)
{
    var comparison = response.ListComparisonValuesSuccessResponse.Data;
    foreach (var item in comparison)
    {
        Console.WriteLine($"Dimension: {item.Dimension}");
        Console.WriteLine($"Value: {item.Value}");
        Console.WriteLine($"Count: {item.Count}");
    }
}
```

### 5.4 Error Analytics

#### 5.4.1 Get Error Data

Retrieve error analytics and debugging information:

```csharp
var response = await sdk.Errors.ListAsync(
    offset: 1,
    limit: 10
);

if (response.ListErrorsSuccessResponse != null)
{
    var errors = response.ListErrorsSuccessResponse.Data;
    foreach (var error in errors)
    {
        Console.WriteLine($"Error ID: {error.Id}");
        Console.WriteLine($"Error Type: {error.Type}");
        Console.WriteLine($"Message: {error.Message}");
        Console.WriteLine($"Timestamp: {error.Timestamp}");
    }
}
```

## Error Handling

Handling errors in this SDK should largely match your expectations. All operations return a response object or throw an exception.

By default an API error will throw a `FastPixException` exception, which has the following properties:

| Property         | Type         | Description           |
| ---------------- | ------------ | --------------------- |
| Message          | string       | The error message     |
| StatusCode       | int          | The HTTP status code  |
| RawResponse      | string       | The raw HTTP response |
| Body             | string       | The response content  |

### Example Error Handling

```csharp
try
{
    var response = await sdk.LiveStreams.CreateAsync(request);
    
    if (response.CreateNewStreamSuccessResponse != null)
    {
        // Handle success
        Console.WriteLine($"Stream created: {response.CreateNewStreamSuccessResponse.Data.StreamId}");
    }
}
catch (UnauthorizedException ex)
{
    // Handle authentication errors
    Console.WriteLine($"Authentication failed: {ex.Message}");
}
catch (ValidationErrorResponse ex)
{
    // Handle validation errors
    Console.WriteLine($"Validation error: {ex.Message}");
}
catch (BadRequestException ex)
{
    // Handle general API errors
    Console.WriteLine($"API error: {ex.Message}");
    Console.WriteLine($"Status code: {ex.StatusCode}");
}
catch (FastPixException ex)
{
    // Handle general SDK errors
    Console.WriteLine($"SDK error: {ex.Message}");
    Console.WriteLine($"Status code: {ex.StatusCode}");
}
```

### Retry Configuration

Configure automatic retries for failed requests:

```csharp
var retryConfig = new RetryConfig
{
    InitialInterval = 1,
    MaxInterval = 50,
    Exponent = 1.1,
    MaxElapsedTime = 100,
    RetryConnectionErrors = false
};

var sdk = FastPix.Builder()
    .WithSecurity(new Security
    {
        Username = "your-access-token",
        Password = "your-secret-key",
    })
    .WithRetryConfig(retryConfig)
    .Build();
```

## Support & Resources

* **Documentation**: Visit the FastPix [API Reference](https://docs.fastpix.io/reference/error-codes) for detailed API documentation
* **GitHub Issues**: Report bugs or request features on the [GitHub repository](https://github.com/FastPix/fastpix-sdk-csharp)
* **FastPix Platform**: [dashboard.fastpix.io](https://dashboard.fastpix.io)
* **Contact us**: [Technical support ](https://www.fastpix.io/technical-support)

> **SDK Maturity**
> 
> This SDK is currently in beta. While it's production-ready, there may be breaking changes between versions without a major version update. We recommend:
> 
> * **Pinning to specific versions** for production applications
> * **Testing thoroughly** before deploying updates
> * **Monitoring the changelog** for any breaking changes

---