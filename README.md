# Fastpix

A developer-friendly, type-safe C# SDK for seamless integration with the FastPix Platform API.

# Introduction

FASTPIX provides a comprehensive set of APIs that enable developers to manage both on-demand media (video/audio) and live streaming experiences, with built-in security features through cryptographic signing keys. These APIs cover the full lifecycle of content creation, management, distribution, playback, and secure access, making them ideal for building scalable video-first applications.

# Key Features

 ## Media API
  - **Upload Media**: Upload media files seamlessly from URLs or devices
  - **Manage Media**: Perform operations such as listing, fetching, updating, and deleting media assets
  - **Playback IDs**: Generate and manage playback IDs for media access
    Use case scenarios - Video-on-Demand Platforms: Manage large content libraries for streaming services. - E-Learning Solutions: Upload and organize lecture videos, metadata, and playback settings. - Multilingual Content Delivery: Add multiple language tracks or subtitles to serve global users.
## Live API
  - **Create & Manage Live Streams**: Create, list, update, and delete live streams effortlessly
  - **Control Stream Access**: Generate playback IDs for live streams to control and manage access
  - **Simulcast to Multiple Platforms**: Stream content to multiple platforms simultaneously
  Use case scenarios - Event Broadcasting: Enable organizers to set up live streams for conferences, concerts, or webinars. - Creator Platforms: Provide streamers with tools for broadcasting gameplay, tutorials, or vlogs with simulcasting support. - Corporate Streaming: Deliver secure internal town halls or meetings with privacy and playback controls.

## Signing Keys
- **Create Signing Keys**: Generate signing keys for secure token-based access.  
- **List & Retrieve Keys**: Fetch all keys or get details for a specific key.  
- **Manage Keys**: Delete or revoke signing keys to maintain secure access control.  
 Use case scenarios - Token-based authentication: Validate user access to premium or subscription-based content. - Key rotation: Regularly rotate keys to reduce risk of compromise. - Protect intellectual property: Prevent unauthorized distribution of valuable media assets. - Control usage: Restrict access to specific users, groups, or contexts. - Prevent tampering: Ensure requested assets have not been modified. - Time-bound access: Enable signed URLs with expiration for controlled viewing windows.


## Video Data API
- **View Analytics**: List video views, get detailed view information, and track top-performing content.  
- **Concurrent Viewer Insights**: Access timeseries data for live and on-demand streams.  
- **Custom Reporting**: Filter viewers by dimensions, list breakdowns, and compare metrics across datasets.  
- **Error Tracking & Diagnostics**: Retrieve logs and analyze errors for proactive monitoring.  
 Use case scenarios

Analytics Dashboards: Monitor performance across content libraries
Quality Monitoring: Diagnose and resolve playback issues
Content Strategy Optimization: Identify high-value content
User Behavior Insights: Understand audience interactions


For detailed usage, refer to the [FastPix API Reference](https://docs.fastpix.io/reference).

<!-- Start Table of Contents [toc] -->
## Table of Contents
<!-- $toc-max-depth=2 -->
* [FastpixSDK](#fastpix)
  * [SDK Installation](#sdk-installation)
  * [ SDK Initialization](#initialization)
  * [SDK Example Usage](#sdk-example-usage)
  * [Available Resources and Operations](#available-resources-and-operations)
  * [Retries](#retries)
  * [Error Handling](#error-handling)
  * [Server Selection](#server-selection)
  * [Detailed Usage](#detailed-usage)
  
  

<!-- End Table of Contents [toc] -->

<!-- Start SDK Installation [installation] -->
## SDK Installation

To add a reference to a local instance of the SDK in a .NET project:
```bash
dotnet add reference src/Fastpix/Fastpix.csproj
```
<!-- End SDK Installation [installation] -->

<!-- Start Initialization [initialization] -->
## Initialization

Create a `FastPix` client by providing HTTP Basic credentials (Username = access token, Password = secret key). You can pass them directly or via a factory function.

### Direct initialization
```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(
    security: new Security
    {
        Username = "your-access-token",
        Password = "secret-key",
    }
);
```

### Using the builder
```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = FastPix.Builder()
    .WithSecurity(new Security
    {
        Username = "your-access-token",
        Password = "secret-key",
    })
    // .WithServerUrl("https://api.fastpix.io/v1/") // optional override
    // .WithRetryConfig(new RetryConfig(...))        // optional retries
    .Build();
```

### From environment variables
```csharp
using Fastpix;
using Fastpix.Models.Components;

var username = Environment.GetEnvironmentVariable("FASTPIX_USERNAME");
var password = Environment.GetEnvironmentVariable("FASTPIX_PASSWORD");

var sdk = new FastPix(
    security: new Security
    {
        Username = username,
        Password = password,
    }
);
```
<!-- End Initialization [initialization] -->

 

<!-- Start SDK Example Usage [usage] -->
## SDK Example Usage

### Example

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

var res = await sdk.Videos.CreateFromUrlAsync(req);

// handle response
```
<!-- End SDK Example Usage [usage] -->



<!-- Start Available Resources and Operations [operations] -->
## Available Resources and Operations

<details open>
<summary>Available methods</summary>

### [Dimensions](docs/sdks/dimensions/README.md)

* [List](docs/sdks/dimensions/README.md#list) - List the dimensions
* [ListFilterValues](docs/sdks/dimensions/README.md#listfiltervalues) - List the filter values for a dimension

### [DrmConfigurations](docs/sdks/drmconfigurations/README.md)

* [List](docs/sdks/drmconfigurations/README.md#list) - Get list of DRM configuration IDs
* [GetById](docs/sdks/drmconfigurations/README.md#getbyid) - Get DRM configuration by ID

### [Errors](docs/sdks/errors/README.md)

* [List](docs/sdks/errors/README.md#list) - List errors


### [InVideoAiFeatures](docs/sdks/invideoaifeatures/README.md)

* [GenerateNamedEntities](docs/sdks/invideoaifeatures/README.md#generatenamedentities) - Generate named entities
* [EnableModeration](docs/sdks/invideoaifeatures/README.md#enablemoderation) - Enable video moderation

### [LivePlayback](docs/sdks/liveplayback/README.md)

* [Create](docs/sdks/liveplayback/README.md#create) - Create a playbackId
* [Delete](docs/sdks/liveplayback/README.md#delete) - Delete a playbackId

### [LivePlaybacks](docs/sdks/liveplaybacks/README.md)

* [Get](docs/sdks/liveplaybacks/README.md#get) - Get playbackId details

### [LiveStreams](docs/sdks/livestreams/README.md)

* [Create](docs/sdks/livestreams/README.md#create) - Create a new stream
* [List](docs/sdks/livestreams/README.md#list) - Get all live streams

### [ManageLiveStream](docs/sdks/managelivestream/README.md)

* [GetViewerCount](docs/sdks/managelivestream/README.md#getviewercount) - Get stream views by ID
* [Disable](docs/sdks/managelivestream/README.md#disable) - Disable a stream
* [Complete](docs/sdks/managelivestream/README.md#complete) - Complete a stream

### [ManageVideos](docs/sdks/managevideos/README.md)

* [ListLiveClips](docs/sdks/managevideos/README.md#listliveclips) - Get all clips of a live stream
* [Delete](docs/sdks/managevideos/README.md#delete) - Delete a media by ID
* [CancelUpload](docs/sdks/managevideos/README.md#cancelupload) - Cancel ongoing upload
* [UpdateTrack](docs/sdks/managevideos/README.md#updatetrack) - Update audio / subtitle track
* [DeleteTrack](docs/sdks/managevideos/README.md#deletetrack) - Delete audio / subtitle track
* [GenerateSubtitles](docs/sdks/managevideos/README.md#generatesubtitles) - Generate track subtitle
* [UpdateSourceAccess](docs/sdks/managevideos/README.md#updatesourceaccess) - Update the source access of a media by ID
* [UpdateMp4Support](docs/sdks/managevideos/README.md#updatemp4support) - Update the mp4Support of a media by ID
* [GetInputInfo](docs/sdks/managevideos/README.md#getinputinfo) - Get info of media inputs
* [ListMediaClips](docs/sdks/managevideos/README.md#listmediaclips) - Get all clips of a media

### [Media](docs/sdks/media/README.md)

* [List](docs/sdks/media/README.md#list) - Get list of all media
* [AddTrack](docs/sdks/media/README.md#addtrack) - Add audio / subtitle track

### [MediaAI](docs/sdks/mediaai/README.md)

* [UpdateSummary](docs/sdks/mediaai/README.md#updatesummary) - Generate video summary
* [UpdateChapters](docs/sdks/mediaai/README.md#updatechapters) - Generate video chapters

### [Metrics](docs/sdks/metrics/README.md)

* [ListBreakdown](docs/sdks/metrics/README.md#listbreakdown) - List breakdown values
* [ListOverall](docs/sdks/metrics/README.md#listoverall) - List overall values
* [GetTimeseries](docs/sdks/metrics/README.md#gettimeseries) - Get timeseries data
* [ListComparisonValues](docs/sdks/metrics/README.md#listcomparisonvalues) - List comparison values

### [Playback](docs/sdks/playback/README.md)

* [Create](docs/sdks/playback/README.md#create) - Create a playback ID
* [Delete](docs/sdks/playback/README.md#delete) - Delete a playback ID
* [GetById](docs/sdks/playback/README.md#getbyid) - Get a playback ID

### [Playlist](docs/sdks/playlist/README.md)

* [GetById](docs/sdks/playlist/README.md#getbyid) - Get a playlist by ID
* [Update](docs/sdks/playlist/README.md#update) - Update a playlist by ID

### [Playlists](docs/sdks/playlists/README.md)

* [Create](docs/sdks/playlists/README.md#create) - Create a new playlist
* [List](docs/sdks/playlists/README.md#list) - Get all playlists
* [Delete](docs/sdks/playlists/README.md#delete) - Delete a playlist by ID
* [AddMedia](docs/sdks/playlists/README.md#addmedia) - Add media to a playlist by ID
* [ChangeMediaOrder](docs/sdks/playlists/README.md#changemediaorder) - Change media order in a playlist by ID
* [DeleteMedia](docs/sdks/playlists/README.md#deletemedia) - Delete media in a playlist by ID

### [SigningKeys](docs/sdks/signingkeys/README.md)

* [Create](docs/sdks/signingkeys/README.md#create) - Create a signing key
* [List](docs/sdks/signingkeys/README.md#list) - Get list of signing key
* [Delete](docs/sdks/signingkeys/README.md#delete) - Delete a signing key
* [GetById](docs/sdks/signingkeys/README.md#getbyid) - Get signing key by ID

### [Simulcasts](docs/sdks/simulcasts/README.md)

* [Delete](docs/sdks/simulcasts/README.md#delete) - Delete a simulcast

### [SimulcastStream](docs/sdks/simulcaststream/README.md)

* [Create](docs/sdks/simulcaststream/README.md#create) - Create a simulcast

### [SimulcastStreams](docs/sdks/simulcaststreams/README.md)

* [GetSpecific](docs/sdks/simulcaststreams/README.md#getspecific) - Get a specific simulcast
* [Update](docs/sdks/simulcaststreams/README.md#update) - Update a simulcast

### [Streams](docs/sdks/streams/README.md)

* [GetById](docs/sdks/streams/README.md#getbyid) - Get stream by ID
* [Delete](docs/sdks/streams/README.md#delete) - Delete a stream
* [Update](docs/sdks/streams/README.md#update) - Update a stream
* [Enable](docs/sdks/streams/README.md#enable) - Enable a stream

### [Uploads](docs/sdks/uploads/README.md)

* [List](docs/sdks/uploads/README.md#list) - Get all unused upload URLs

### [Videos](docs/sdks/videos/README.md)

* [CreateFromUrl](docs/sdks/videos/README.md#createfromurl) - Create media from URL
* [Get](docs/sdks/videos/README.md#get) - Get a media by ID
* [Update](docs/sdks/videos/README.md#update) - Update a media by ID
* [Upload](docs/sdks/videos/README.md#upload) - Upload media from device

### [Views](docs/sdks/views/README.md)

* [List](docs/sdks/views/README.md#list) - List video views
* [GetDetails](docs/sdks/views/README.md#getdetails) - Get details of video view
* [ListTopContent](docs/sdks/views/README.md#listtopcontent) - List by top content
* [GetTimeseries](docs/sdks/views/README.md#gettimeseries) - Get concurrent viewers timeseries
* [GetConcurrentViewersBreakdown](docs/sdks/views/README.md#getconcurrentviewersbreakdown) - Get concurrent viewers breakdown by dimension

</details>
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

If you'd like to override the default retry strategy for all operations that support retries, you can use the `RetryConfig` optional parameter when intitializing the SDK:
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
<!-- End Retries [retries] -->

<!-- Start Error Handling [errors] -->
## Error Handling

[`FastPixException`](./src/Fastpix/Models/Errors/FastPixException.cs) is the base exception class for all HTTP error responses. It has the following properties:

| Property      | Type                  | Description           |
|---------------|-----------------------|-----------------------|
| `Message`     | *string*              | Error message         |
| `Request`     | *HttpRequestMessage*  | HTTP request object   |
| `Response`    | *HttpResponseMessage* | HTTP response object  |

Some exceptions in this SDK include an additional `Payload` field, which will contain deserialized custom error data when present. Possible exceptions are listed in the [Error Classes](#error-classes) section.

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
<!-- End Error Handling [errors] -->

<!-- Start Server Selection [server] -->
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
<!-- End Server Selection [server] -->

<!-- Placeholder for Future Fastpix SDK Sections -->

 # Detailed Usage

For a complete understanding of each API's functionality, including request and response details, parameter descriptions, and additional examples, please refer to the [FastPix API Reference](https://docs.fastpix.io/reference/signingkeys-overview).

The API reference provides comprehensive documentation for all available endpoints and features, ensuring developers can integrate and utilize FastPix APIs efficiently.



