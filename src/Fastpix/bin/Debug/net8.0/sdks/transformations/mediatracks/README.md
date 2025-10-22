# Media Tracks

Add, update, and manage audio and subtitle tracks for your media content with multi-language support.

## Overview

### Available Operations

* [AddTrack](#addtrack) - Add audio / subtitle track

## AddTrack

This endpoint allows you to add audio or subtitle tracks to existing media content. You can add multiple language tracks to make your content accessible to a global audience. The endpoint supports both audio tracks (for multilingual audio) and subtitle tracks (for closed captions and accessibility).

### Supported Formats

**Audio Tracks:**
- MP3, AAC, WAV, OGG formats
- Must be publicly accessible URLs

**Subtitle Tracks:**
- VTT (WebVTT) and SRT formats
- Must be publicly accessible URLs

### Language Code Support

FastPix strictly adheres to **BCP 47 language codes** when managing tracks. When adding or updating tracks, ensure that the `languageCode` is specified in this format. BCP 47 language tags provide a consistent and standardized way to represent language and locale information.

For detailed information, see:
- [Manage Audio Tracks](https://docs.fastpix.io/docs/manage-audio-tracks)
- [Manage Subtitle Tracks](https://docs.fastpix.io/docs/manage-subtitle-tracks)

#### How it works
To use this endpoint, provide the `mediaId` and track details in the request body. The API will process the track and make it available for playback. This is useful for adding multilingual support to your media content.

For detailed documentation, see [Add tracks](https://docs.fastpix.io/docs/add-tracks).

### Example Usage

#### Adding Audio Track
<!-- UsageSnippet language="csharp" operationID="add-media-track" method="post" path="/on-demand/{mediaId}/tracks" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

// Add audio track
var res = await sdk.Media.AddTrackAsync(
    mediaId: "your-media-id",
    requestBody: new AddMediaTrackRequestBody() {
        Tracks = new AddTrackRequest() {
            Type = AddTrackRequestType.Audio,
            Url = "https://static.fastpix.io/music-1.mp3",
            LanguageCode = "fr",  // BCP 47 language code
            LanguageName = "French"
        }
    }
);

Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

#### Adding Subtitle Track
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

// Add subtitle track
var res = await sdk.Media.AddTrackAsync(
    mediaId: "your-media-id",
    requestBody: new AddMediaTrackRequestBody() {
        Tracks = new AddTrackRequest() {
            Type = AddTrackRequestType.Subtitle,
            Url = "https://static.fastpix.io/subtitles-en.vtt",
            LanguageCode = "en-US",  // BCP 47 language code
            LanguageName = "English"
        }
    }
);

Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `mediaId` | string | Yes | The ID of the media to add tracks to |
| `requestBody` | AddMediaTrackRequestBody | Yes | Track details including type, URL, and language information |

### Response

**[AddMediaTrackResponse](../../../Models/Requests/AddMediaTrackResponse.md)**

### Errors

| Error Type | Status Code | Content Type |
|------------|-------------|--------------|
| Fastpix.Models.Errors.InvalidPermissionException | 401 | application/json |
| Fastpix.Models.Errors.ForbiddenException | 403 | application/json |
| Fastpix.Models.Errors.MediaNotFoundException | 404 | application/json |
| Fastpix.Models.Errors.ValidationErrorResponse | 422 | application/json |
| Fastpix.Models.Errors.APIException | 4XX, 5XX | */* |

### Example Response

#### Audio Track Response
```json
{
  "success": true,
  "data": {
    "id": "22d6607d-b7bc-4756-817c-e959d4dbbb81",
    "type": "audio",
    "url": "https://static.fastpix.io/music-1.mp3",
    "languageCode": "fr",
    "languageName": "French"
  }
}
```

#### Subtitle Track Response
```json
{
  "success": true,
  "data": {
    "id": "subtitle-123-456-789",
    "type": "subtitles",
    "url": "https://static.fastpix.io/subtitles-en.vtt",
    "languageCode": "en-US",
    "languageName": "English"
  }
}
```
