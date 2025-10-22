# FastPix C# SDK

A robust, type-safe C# SDK designed for seamless integration with the FastPix API platform.

<!-- Start Summary [summary] -->
## Introduction

The FastPix C# SDK simplifies integration with the FastPix platform. It provides a clean, type-safe interface for secure and efficient communication with the FastPix API, enabling easy management of media uploads, live streaming, on‑demand content, playlists, video analytics, and signing keys for secure access and token management. It is intended for use with .NET 8 and above.

## Prerequisites

### Environment and Version Support

<table>
<tr>
<th>Requirement</th>
<th>Version</th>
<th>Description</th>
</tr>
<tr>
<td><strong>.NET SDK</strong></td>
<td><code>8.0+</code></td>
<td>Core runtime environment</td> 
</tr>
<tr>
<td><strong>NuGet</strong></td>
<td><code>Latest</code></td>
<td>Package manager for dependencies</td>
</tr>
<tr>
<td><strong>Internet</strong></td>
<td><code>Required</code></td>
<td>API communication and authentication</td>
</tr>
</table>

> **Pro Tip:** We recommend using .NET 9+ for optimal performance and the latest language features.

### Getting Started with FastPix

To get started with the **FastPix C# SDK**, ensure you have the following:

- The FastPix APIs are authenticated using a **Username** and a **Password**. You must generate these credentials to use the SDK.

- Follow the steps in the [Authentication with Basic Auth](https://docs.fastpix.io/docs/basic-authentication) guide to obtain your credentials.

### Environment Variables (Optional)

Configure your FastPix credentials using environment variables for enhanced security and convenience:

```bash
# Set your FastPix credentials
export FASTPIX_USERNAME="your-access-token"
export FASTPIX_PASSWORD="your-secret-key"
```

> **Security Note:** Never commit your credentials to version control. Use environment variables or secure credential management systems.

<!-- Start Table of Contents [toc] -->
## Table of Contents
<!-- $toc-max-depth=2 -->
* [FastPix C# SDK](#fastpix-c-sdk)
  * [Setup](#setup)
  * [Example Usage](#example-usage)
  * [Available Resources and Operations](#available-resources-and-operations)
  * [Retries](#retries)
  * [Error Handling](#error-handling)
  * [Server Selection](#server-selection)
  * [Development](#development)

<!-- End Table of Contents [toc] -->

<!-- Start Setup [setup] -->
## Setup

### Installation

Install the FastPix C# SDK using your preferred package manager:

```bash
dotnet add package Fastpix  
```

### NuGet Package Manager

```bash
Install-Package Fastpix
```

### Package Manager Console

```bash
PM> Install-Package Fastpix
```

### Imports

This SDK supports both **C# 8.0+** and **.NET Framework 4.8+**. Examples in this documentation use modern C# syntax as it's the preferred format, but you can use either approach.

#### Modern C# (Recommended)
```csharp
// Basic imports
using Fastpix;
using Fastpix.Models.Components;
```

#### Legacy .NET Framework
```csharp
// Legacy using statements
using Fastpix;
using Fastpix.Models.Components;
```

> **Why Modern C#?** Modern C# provides better type safety, nullable reference types, and are the current .NET standard. They enable better IntelliSense and development tooling support.

> **Note:** This SDK automatically provides both framework support. If you encounter compatibility issues in your project, you may need to update your target framework to .NET 8.0 or higher.

> **Security Note:** For production applications, it's recommended to make API calls from your backend server rather than directly from client applications to:
> - Keep credentials secure
> - Avoid CORS issues  
> - Implement proper authentication.

### Initialization

Initialize the FastPix SDK with your credentials:

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(
    security: new Security
    {
        Username = "your-access-token",  // ⚠️ Replace with your actual FastPix username
        Password = "secret-key",         // ⚠️ Replace with your actual FastPix password
    }
);
```

Or using environment variables:

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(
    security: new Security
    {
        Username = Environment.GetEnvironmentVariable("FASTPIX_USERNAME"),
        Password = Environment.GetEnvironmentVariable("FASTPIX_PASSWORD"),
    }
);
```
 
<!-- End Setup [setup] -->

<!-- Start Example Usage [example-usage] -->
## Example Usage

### Example

```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;
using Newtonsoft.Json;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
   Password = "your-secret-key",
});

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreateVideoInput(
            new VideoInput() {
                Type = "video",
                Url = "https://static.fastpix.io/sample.mp4",
            }
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "key1", "value1" },
    },
    AccessPolicy = CreateMediaRequestAccessPolicy.Public,
};

var res = await sdk.Videos.CreateFromUrlAsync(req);
Console.WriteLine(JsonConvert.SerializeObject(res.CreateMediaSuccessResponse, Formatting.Indented) ?? "null");
//  handle response
```

> **⚠️ IMPORTANT: Replace Credentials**
> 
> **You MUST replace the placeholder credentials with your actual FastPix API credentials:**
> - Replace `"your-access-token"` with your actual FastPix username/access token
> - Replace `"your-secret-key"` with your actual FastPix password/secret key
> 
> **Using placeholder credentials will result in `UnauthorizedException` errors.**
> 
> Get your credentials from the [FastPix Dashboard](https://dashboard.fastpix.io) or follow the [Authentication Guide](https://docs.fastpix.io/docs/basic-authentication).

<!-- End Example Usage [example-usage] -->

<!-- Start Available Resources and Operations [operations] -->
## Available Resources and Operations

Comprehensive C# SDK for FastPix platform integration with full API coverage.

### Media API

Upload, manage, and transform video content with comprehensive media management capabilities.

For detailed documentation, see [FastPix Video on Demand Overview](https://docs.fastpix.io/docs/video-on-demand-overview).

#### Input Video
- [Create from URL](docs/sdks/videos/README.md#createfromurl) - Upload video content from external URL
- [Upload from Device](docs/sdks/videos/README.md#upload) - Upload video files directly from device

#### Manage Videos
- [List All Media](docs/sdks/managevideos/README.md#list) - Retrieve complete list of all media files
- [Get Media by ID](docs/sdks/managevideos/README.md#get) - Get detailed information for specific media
- [Update Media](docs/sdks/managevideos/README.md#update) - Modify media metadata and settings
- [Delete Media](docs/sdks/managevideos/README.md#delete) - Remove media files from library
- [Cancel Upload](docs/sdks/managevideos/README.md#cancelupload) - Stop ongoing media upload process
- [Get Input Info](docs/sdks/managevideos/README.md#getinputinfo) - Retrieve detailed input information
- [List Uploads](docs/sdks/uploads/README.md#list) - Get all available upload URLs

#### Playback
- [Create Playback ID](docs/sdks/playback/README.md#create) - Generate secure playback identifier
- [Delete Playback ID](docs/sdks/playback/README.md#delete) - Remove playback access
- [Get Playback ID](docs/sdks/playback/README.md#getbyid) - Retrieve playback configuration details

#### Play Specific Segments
- Stream only a portion of a video by appending `start`/`end` parameters to the playback URL. Explore: [Play specific segments](https://docs.fastpix.io/docs/play-your-videos#play-specific-segments). For creating reusable assets instead, see [Create clips from existing media](https://docs.fastpix.io/docs/create-clips-from-existing-media).
- Try it: `https://stream.fastpix.io/{PLAYBACK_ID}.m3u8?start=20&end=60` (replace `{PLAYBACK_ID}` and tweak times).

#### Create Clips from Existing Media
- Create reusable, shareable clip assets from a source video with precise start/end control. Explore: [Create clips from existing media](https://docs.fastpix.io/docs/create-clips-from-existing-media).
- Try it: use the clipping API from the guide to generate a new clip asset, then retrieve it via Media Clips to validate.

#### Playlist
- [Create Playlist](docs/sdks/playlists/README.md#create) - Create new video playlist
- [List Playlists](docs/sdks/playlists/README.md#list) - Get all available playlists
- [Get Playlist](docs/sdks/playlist/README.md#getbyid) - Retrieve specific playlist details
- [Update Playlist](docs/sdks/playlist/README.md#update) - Modify playlist settings and metadata
- [Delete Playlist](docs/sdks/playlists/README.md#delete) - Remove playlist from library
- [Add Media](docs/sdks/playlists/README.md#addmedia) - Add media items to playlist
- [Reorder Media](docs/sdks/playlists/README.md#changemediaorder) - Change order of media in playlist
- [Remove Media](docs/sdks/playlists/README.md#deletemedia) - Remove media from playlist

#### Signing Keys
- [Create Key](docs/sdks/signingkeys/README.md#create) - Generate new signing key pair
- [List Keys](docs/sdks/signingkeys/README.md#list) - Get all available signing keys
- [Delete Key](docs/sdks/signingkeys/README.md#delete) - Remove signing key from system
- [Get Key](docs/sdks/signingkeys/README.md#getbyid) - Retrieve specific signing key details

#### DRM Configurations
- [List DRM Configs](docs/sdks/drmconfigurations/README.md#list) - Get all DRM configuration options
- [Get DRM Config](docs/sdks/drmconfigurations/README.md#getbyid) - Retrieve specific DRM configuration

### Live API 

Stream, manage, and transform live video content with real-time broadcasting capabilities.

For detailed documentation, see [FastPix Live Stream Overview](https://docs.fastpix.io/docs/live-stream-overview).

#### Start Live Stream
- [Create Stream](docs/sdks/livestreams/README.md#create) - Initialize new live streaming session with **DVR mode support**

#### Manage Live Stream
- [List Streams](docs/sdks/livestreams/README.md#list) - Retrieve all active live streams
- [Get Viewer Count](docs/sdks/managelivestream/README.md#getviewercount) - Get real-time viewer statistics
- [Get Stream](docs/sdks/streams/README.md#getbyid) - Retrieve detailed stream information
- [Delete Stream](docs/sdks/streams/README.md#delete) - Terminate and remove live stream
- [Update Stream](docs/sdks/streams/README.md#update) - Modify stream settings and configuration
- [Enable Stream](docs/sdks/streams/README.md#enable) - Activate live streaming
- [Disable Stream](docs/sdks/managelivestream/README.md#disable) - Pause live streaming
- [Complete Stream](docs/sdks/managelivestream/README.md#complete) - Finalize and archive stream

#### Live Playback
- [Create Playback ID](docs/sdks/liveplayback/README.md#create) - Generate secure live playback access
- [Delete Playback ID](docs/sdks/liveplayback/README.md#delete) - Revoke live playback access
- [Get Playback ID](docs/sdks/liveplaybacks/README.md#get) - Retrieve live playback configuration

#### Live Clipping
- Explore instant live clipping during a live stream to capture key moments without creating new assets. See the Live Clipping guide: [Instant Live Clipping](https://docs.fastpix.io/docs/instant-live-clipping).
- Try it: enable a test live stream and capture a highlight clip while broadcasting to see it appear instantly.

#### Simulcast Stream
- [Create Simulcast](docs/sdks/simulcaststream/README.md#create) - Set up multi-platform streaming
- [Delete Simulcast](docs/sdks/simulcasts/README.md#delete) - Remove simulcast configuration
- [Get Simulcast](docs/sdks/simulcaststreams/README.md#getspecific) - Retrieve simulcast settings
- [Update Simulcast](docs/sdks/simulcaststreams/README.md#update) - Modify simulcast parameters

### Video Data API 

Monitor video performance and quality with comprehensive analytics and real-time metrics.

For detailed documentation, see [FastPix Video Data Overview](https://docs.fastpix.io/docs/video-data-overview).

#### Metrics
- [List Breakdown Values](docs/sdks/metrics/README.md#listbreakdown) - Get detailed breakdown of metrics by dimension
- [List Overall Values](docs/sdks/metrics/README.md#listoverall) - Get aggregated metric values across all content
- [Get Timeseries Data](docs/sdks/metrics/README.md#gettimeseries) - Retrieve time-based metric trends and patterns

#### Views
- [List Video Views](docs/sdks/views/README.md#list) - Get comprehensive list of video viewing sessions
- [Get View Details](docs/sdks/views/README.md#getdetails) - Retrieve detailed information about specific video views
- [List Top Content](docs/sdks/views/README.md#listtopcontent) - Find your most popular and engaging content
- [Get Concurrent Viewers](docs/sdks/views/README.md#gettimeseries) - Monitor real-time viewer counts over time
- [Get Viewer Breakdown](docs/sdks/views/README.md#getconcurrentviewersbreakdown) - Analyze viewers by device, location, and other dimensions

#### Dimensions
- [List Dimensions](docs/sdks/dimensions/README.md#list) - Get available data dimensions for filtering and analysis
- [List Filter Values](docs/sdks/dimensions/README.md#listfiltervalues) - Get specific values for a particular dimension

### Transformations

Transform and enhance your video content with powerful AI and editing capabilities.

- [In-Video AI Features](docs/sdks/transformations/README.md#in-video-ai-features) - AI-powered content enhancement
- [Media Clips](docs/sdks/transformations/mediaclips/README.md#listmediaclips) - Manage video clips and segments
- [Live Clips](docs/sdks/transformations/mediaclips/README.md#listliveclips) - Get clips from live streams
- [Subtitles](docs/sdks/transformations/README.md#subtitles) - Generate automatic subtitles
- [Media Tracks](docs/sdks/transformations/mediatracks/README.md#addtrack) - Add audio and subtitle tracks
- [Access Control](docs/sdks/transformations/README.md#access-control) - Control content permissions
- [Format Support](docs/sdks/transformations/README.md#format-support) - Configure download capabilities

### Error Handling

Handle and manage errors with comprehensive error handling capabilities and detailed error information for all API operations.

- [List Errors](docs/sdks/errors/README.md#list) - Retrieve comprehensive error logs and diagnostics

<!-- End Available Resources and Operations [operations] -->

<!-- Start Retries [retries] -->
## Retries

Some of the endpoints in this SDK support retries. If you use the SDK without any configuration, it will fall back to the default retry strategy provided by the API. However, the default retry strategy can be overridden on a per-operation basis, or across the entire SDK.

To change the default retry strategy for a single API call, simply pass a `RetryConfig` to the call:
```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreateVideoInput(
            new VideoInput() {
                Type = "video",
                Url = "https://static.fastpix.io/sample.mp4",
            }
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "key1", "value1" },
    },
    AccessPolicy = CreateMediaRequestAccessPolicy.Public,
};

var res = await sdk.Videos.CreateFromUrlAsync(
    retryConfig: new RetryConfig(
        strategy: RetryConfig.RetryStrategy.BACKOFF,
        backoff: new BackoffStrategy(
            initialIntervalMs: 1L,
            maxIntervalMs: 50L,
            maxElapsedTimeMs: 100L,
            exponent: 1.1
        ),
        retryConnectionErrors: false
    ),
    request: req
);

// handle response
```

If you'd like to override the default retry strategy for all operations that support retries, you can use the `RetryConfig` optional parameter when initializing the SDK:
```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;

var sdk = new FastPix(
    retryConfig: new RetryConfig(
        strategy: RetryConfig.RetryStrategy.BACKOFF,
        backoff: new BackoffStrategy(
            initialIntervalMs: 1L,
            maxIntervalMs: 50L,
            maxElapsedTimeMs: 100L,
            exponent: 1.1
        ),
        retryConnectionErrors: false
    ),
    security: new Security() {
        Username = "your-access-token",
        Password = "secret-key",
    }
);

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreateVideoInput(
            new VideoInput() {
                Type = "video",
                Url = "https://static.fastpix.io/sample.mp4",
            }
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "key1", "value1" },
    },
    AccessPolicy = CreateMediaRequestAccessPolicy.Public,
};

var res = await sdk.Videos.CreateFromUrlAsync(req);

// handle response
```

## Error Handling

[`FastPixException`](./src/Fastpix/Models/Errors/FastPixException.cs) is the base class for all HTTP error responses. It has the following properties:

| Property      | Type                  | Description           |
|---------------|-----------------------|-----------------------|
| `Message`     | *string*              | Error message         |
| `Request`     | *HttpRequestMessage*  | HTTP request object   |
| `Response`    | *HttpResponseMessage* | HTTP response object  |

### Example
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Errors;
using System.Collections.Generic;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

try
{
    CreateMediaRequest req = new CreateMediaRequest() {
        Inputs = new List<Fastpix.Models.Components.Input>() {
            Fastpix.Models.Components.Input.CreateVideoInput(
                new VideoInput() {
                    Type = "video",
                    Url = "https://static.fastpix.io/sample.mp4",
                }
            ),
        },
        Metadata = new Dictionary<string, string>() {
            { "key1", "value1" },
        },
        AccessPolicy = CreateMediaRequestAccessPolicy.Public,
    };

    var res = await sdk.Videos.CreateFromUrlAsync(req);

    // handle response
}
catch (FastPixException ex)  // all SDK exceptions inherit from FastPixException
{
    // ex.ToString() provides a detailed error message
    System.Console.WriteLine(ex);

    // Base exception fields
    HttpRequestMessage request = ex.Request;
    HttpResponseMessage response = ex.Response;
    var statusCode = (int)response.StatusCode;
    var responseBody = ex.Body;

    if (ex is BadRequestException badRequestEx) // different exceptions may be thrown depending on the method
    {
        // Check error data fields
        BadRequestExceptionPayload payload = badRequestEx.Payload;
        bool? Success = payload.Success;
        BadRequestError? Error = payload.Error;
        // ...
    }

    // An underlying cause may be provided
    if (ex.InnerException != null)
    {
        Exception cause = ex.InnerException;
    }
}
catch (OperationCanceledException ex)
{
    // CancellationToken was cancelled
}
catch (System.Net.Http.HttpRequestException ex)
{
    // Check ex.InnerException for Network connectivity errors
}
```

### Error Classes
**Primary exceptions:**
* [`FastPixException`](./src/Fastpix/Models/Errors/FastPixException.cs): The base class for HTTP error responses.
  * [`InvalidPermissionException`](./src/Fastpix/Models/Errors/InvalidPermissionException.cs): *
  * [`ValidationErrorResponse`](./src/Fastpix/Models/Errors/ValidationErrorResponse.cs): Status code `422`. *

<details><summary>Less common exceptions (24)</summary>

* [`System.Net.Http.HttpRequestException`](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequestexception): Network connectivity error. For more details about the underlying cause, inspect the `ex.InnerException`.

* Inheriting from [`FastPixException`](./src/Fastpix/Models/Errors/FastPixException.cs):
  * [`ForbiddenException`](./src/Fastpix/Models/Errors/ForbiddenException.cs): Status code `403`. Applicable to 26 of 66 methods.*
  * [`UnauthorizedException`](./src/Fastpix/Models/Errors/UnauthorizedException.cs): Applicable to 24 of 66 methods.*
  * [`MediaNotFoundException`](./src/Fastpix/Models/Errors/MediaNotFoundException.cs): Status code `404`. Applicable to 17 of 66 methods.*
  * [`BadRequestException`](./src/Fastpix/Models/Errors/BadRequestException.cs): Bad Request. Status code `400`. Applicable to 10 of 66 methods.*
  * [`NotFoundError`](./src/Fastpix/Models/Errors/NotFoundError.cs): Status code `404`. Applicable to 8 of 66 methods.*
  * [`ViewNotFoundException`](./src/Fastpix/Models/Errors/ViewNotFoundException.cs): View Not Found. Status code `404`. Applicable to 7 of 66 methods.*
  * [`LiveNotFoundError`](./src/Fastpix/Models/Errors/LiveNotFoundError.cs): Stream Not Found. Status code `404`. Applicable to 6 of 66 methods.*
  * [`InvalidPlaylistIdResponseException`](./src/Fastpix/Models/Errors/InvalidPlaylistIdResponseException.cs): Payload Validation Failed. Status code `422`. Applicable to 6 of 66 methods.*
  * [`UnAuthorizedResponseException`](./src/Fastpix/Models/Errors/UnAuthorizedResponseException.cs): response for unauthorized request. Status code `401`. Applicable to 4 of 66 methods.*
  * [`ForbiddenResponseException`](./src/Fastpix/Models/Errors/ForbiddenResponseException.cs): response for forbidden request. Status code `403`. Applicable to 4 of 66 methods.*
  * [`TrackDuplicateRequestException`](./src/Fastpix/Models/Errors/TrackDuplicateRequestException.cs): Duplicate language name. Status code `400`. Applicable to 3 of 66 methods.*
  * [`NotFoundErrorSimulcast`](./src/Fastpix/Models/Errors/NotFoundErrorSimulcast.cs): Stream/Simulcast Not Found. Status code `404`. Applicable to 3 of 66 methods.*
  * [`MediaOrPlaybackNotFoundException`](./src/Fastpix/Models/Errors/MediaOrPlaybackNotFoundException.cs): Status code `404`. Applicable to 2 of 66 methods.*
  * [`NotFoundErrorPlaybackId`](./src/Fastpix/Models/Errors/NotFoundErrorPlaybackId.cs): Status code `404`. Applicable to 2 of 66 methods.*
  * [`SigningKeyNotFoundError`](./src/Fastpix/Models/Errors/SigningKeyNotFoundError.cs): Bad Request. Status code `404`. Applicable to 2 of 66 methods.*
  * [`DuplicateMp4SupportException`](./src/Fastpix/Models/Errors/DuplicateMp4SupportException.cs): Mp4Support value already exists. Status code `400`. Applicable to 1 of 66 methods.*
  * [`StreamAlreadyDisabledError`](./src/Fastpix/Models/Errors/StreamAlreadyDisabledError.cs): Stream already disabled. Status code `400`. Applicable to 1 of 66 methods.*
  * [`TrialPlanRestrictionError`](./src/Fastpix/Models/Errors/TrialPlanRestrictionError.cs): Bad Request – Stream is either already enabled or cannot be enabled on trial plan. Status code `400`. Applicable to 1 of 66 methods.*
  * [`StreamAlreadyEnabledError`](./src/Fastpix/Models/Errors/StreamAlreadyEnabledError.cs): Bad Request – Stream is either already enabled or cannot be enabled on trial plan. Status code `400`. Applicable to 1 of 66 methods.*
  * [`SimulcastUnavailableException`](./src/Fastpix/Models/Errors/SimulcastUnavailableException.cs): Simulcast is not available for trial streams. Status code `400`. Applicable to 1 of 66 methods.*
  * [`MediaClipNotFoundException`](./src/Fastpix/Models/Errors/MediaClipNotFoundException.cs): media workspace relation not found. Status code `404`. Applicable to 1 of 66 methods.*
  * [`DuplicateReferenceIdErrorResponse`](./src/Fastpix/Models/Errors/DuplicateReferenceIdErrorResponse.cs): Displays the result of the request. Status code `409`. Applicable to 1 of 66 methods.*
  * [`ResponseValidationError`](./src/Fastpix/Models/Errors/ResponseValidationError.cs): Thrown when the response data could not be deserialized into the expected type.
</details>

\* Refer to the [relevant documentation](#available-resources-and-operations) to determine whether an exception applies to a specific operation.

## Server Selection

### Override Server URL Per-Client

The default server can be overridden globally by passing a URL to the `serverUrl: string` optional parameter when initializing the SDK client instance. For example:
```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;

var sdk = new FastPix(
    serverUrl: "https://api.fastpix.io/v1/",
    security: new Security() {
        Username = "your-access-token",
        Password = "secret-key",
    }
);

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreateVideoInput(
            new VideoInput() {
                Type = "video",
                Url = "https://static.fastpix.io/sample.mp4",
            }
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "key1", "value1" },
    },
    AccessPolicy = CreateMediaRequestAccessPolicy.Public,
};

var res = await sdk.Videos.CreateFromUrlAsync(req);

// handle response
```

# Development

This C# SDK is programmatically generated from our API specifications. Any manual modifications to internal files will be overwritten during subsequent generation cycles. 

We value community contributions and feedback. Feel free to submit pull requests or open issues with your suggestions, and we'll do our best to include them in future releases.

## Detailed Usage

For comprehensive understanding of each API's functionality, including detailed request and response specifications, parameter descriptions, and additional examples, please refer to the [FastPix API Reference](https://docs.fastpix.io/reference/signingkeys-overview).

The API reference offers complete documentation for all available endpoints and features, enabling developers to integrate and leverage FastPix APIs effectively.