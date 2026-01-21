# FastPix C# SDK

A robust, type-safe C# SDK designed for seamless integration with the FastPix API platform.

## Introduction

The FastPix C# SDK simplifies integration with the FastPix platform. It provides a clean, strongly-typed interface for secure and efficient communication with the FastPix API, enabling easy management of media uploads, live streaming, on‑demand content, playlists, video analytics, and signing keys for secure access and token management. It is intended for use with .NET 8.0 and above.

## Prerequisites

### Environment and Version Support

| Requirement | Version | Description |
|---|---:|---|
| .NET | `8.0+` | Core runtime environment |
| NuGet | `Latest` | Package manager for dependencies |
| Internet | `Required` | API communication and authentication |

> Pro Tip: We recommend using .NET 8.0+ for optimal performance and the latest language features.

### Getting Started with FastPix

To get started with the FastPix C# SDK, ensure you have the following:

- The FastPix APIs are authenticated using a **Username** and a **Password**. You must generate these credentials to use the SDK.
- Follow the steps in the [Authentication with Basic Auth](https://docs.fastpix.io/docs/basic-authentication) guide to obtain your credentials.

### Environment Variables (Optional)

Configure your FastPix credentials using environment variables for enhanced security and convenience:

```bash
# Set your FastPix credentials
export FASTPIX_USERNAME="your-access-token"
export FASTPIX_PASSWORD="your-secret-key"
```

> Security Note: Never commit your credentials to version control. Use environment variables or secure credential management systems.

## Table of Contents

* [FastPix C# SDK](#fastpix-c-sdk)
  * [Setup](#setup)
  * [Example Usage](#example-usage)
  * [Available Resources and Operations](#available-resources-and-operations)
  * [Retries](#retries)
  * [Error Handling](#error-handling)
  * [Server Selection](#server-selection)
  * [Custom HTTP Client](#custom-http-client)
  * [Development](#development)

## Setup

### Installation

Install the FastPix C# SDK using your preferred package manager:

#### .NET CLI

```bash
dotnet add package Fastpix
```

#### NuGet Package Manager

In Visual Studio, open the Package Manager Console and run:

```bash
Install-Package Fastpix
```

#### Local Reference

To add a reference to a local instance of the SDK in a .NET project:

```bash
dotnet add reference src/Fastpix/Fastpix.csproj
```

### Imports

The SDK uses standard C# namespaces. Import the necessary namespaces at the top of your files:

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using System.Collections.Generic;
```

### Initialization

Initialize the FastPix SDK with your credentials:

```csharp
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});
```

Or using environment variables:

```csharp
using Fastpix;
using Fastpix.Models.Components;
using System;

var sdk = new FastpixSDK(security: new Security() {
    Username = Environment.GetEnvironmentVariable("FASTPIX_USERNAME"), // Your Access Token
    Password = Environment.GetEnvironmentVariable("FASTPIX_PASSWORD"), // Your Secret Key
});
```

## Example Usage

```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreatePullVideoInput(
            new PullVideoInput() {}
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "<key>", "<value>" },
    },
};

var res = await sdk.InputVideo.CreateMediaAsync(req);

// handle response
```

## Available Resources and Operations

Comprehensive C# SDK for FastPix platform integration with full API coverage.

### Media API

Upload, manage, and transform video content with comprehensive media management capabilities.

For detailed documentation, see [FastPix Video on Demand Overview](https://docs.fastpix.io/docs/video-on-demand-overview).

#### Input Video
- [Create from URL](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/inputvideo/README.md#createmedia) - Upload video content from external URL
- [Upload from Device](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/inputvideo/README.md#upload) - Upload video files directly from device

#### Manage Videos
- [List All Media](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#list) - Retrieve complete list of all media files
- [Get Media by ID](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#getbyid) - Get detailed information for specific media
- [Update Media](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/videos/README.md#update) - Modify media metadata and settings
- [Delete Media](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#deletemedia) - Remove media files from library
- [Cancel Upload](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#cancelupload) - Stop ongoing media upload process
- [Get Input Info](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/videos/README.md#getinputinfo) - Retrieve detailed input information
- [List Uploads](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#listuploads) - Get all available upload URLs
- [List Clips](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#listclips) - Get all clips of a media

#### Playback
- [Create Playback ID](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#create) - Generate secure playback identifier
- [List Playback IDs](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#list) - Get all playback IDs for a media
- [Delete Playback ID](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#delete) - Remove playback access
- [Get Playback ID](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playbacks/README.md#get) - Retrieve playback configuration details
- [Update Domain Restrictions](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#updatedomainrestrictions) - Update domain restrictions for a playback ID
- [Update User-Agent Restrictions](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#updateuseragentrestrictions) - Update user-agent restrictions for a playback ID

#### Playlist
- [Create Playlist](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playlist/README.md#create) - Create new video playlist
- [List Playlists](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playlists/README.md#getall) - Get all available playlists
- [Get Playlist](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playlists/README.md#get) - Retrieve specific playlist details
- [Update Playlist](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playlists/README.md#update) - Modify playlist settings and metadata
- [Delete Playlist](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playlists/README.md#delete) - Remove playlist from library
- [Add Media](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playlists/README.md#addmedia) - Add media items to playlist
- [Reorder Media](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playlists/README.md#reordermedia) - Change order of media in playlist
- [Remove Media](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playlists/README.md#deletemedia) - Remove media from playlist

#### Signing Keys
- [Create Key](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/signingkeys/README.md#create) - Generate new signing key pair
- [List Keys](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/signingkeys/README.md#list) - Get all available signing keys
- [Delete Key](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/signingkeys/README.md#delete) - Remove signing key from system
- [Get Key](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/signingkeys/README.md#getbyid) - Retrieve specific signing key details

#### DRM Configurations
- [List DRM Configs](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/drmconfigurations/README.md#list) - Get all DRM configuration options
- [Get DRM Config](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/drmconfigurations/README.md#getbyid) - Retrieve specific DRM configuration

### Live API

Stream, manage, and transform live video content with real-time broadcasting capabilities.

For detailed documentation, see [FastPix Live Stream Overview](https://docs.fastpix.io/docs/live-stream-overview).

#### Start Live Stream
- [Create Stream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/livestreams/README.md#create) - Initialize new live streaming session

#### Manage Live Stream
- [List Streams](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/livestreams/README.md#getall) - Retrieve all active live streams
- [Get Viewer Count](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/livestreams/README.md#getviewercount) - Get real-time viewer statistics
- [Get Stream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/livestreams/README.md#getbyid) - Retrieve detailed stream information
- [Delete Stream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/livestreams/README.md#delete) - Terminate and remove live stream
- [Update Stream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/streams/README.md#update) - Modify stream settings and configuration
- [Enable Stream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managelivestream/README.md#enable) - Activate live streaming
- [Disable Stream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managelivestream/README.md#disable) - Pause live streaming
- [Complete Stream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/livestreams/README.md#complete) - Finalize and archive stream

#### Live Playback
- [Create Playback ID](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/liveplayback/README.md#create) - Generate secure live playback access
- [Delete Playback ID](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/liveplayback/README.md#deleteplaybackid) - Revoke live playback access
- [Get Playback ID](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/liveplayback/README.md#getplaybackdetails) - Retrieve live playback configuration

#### Simulcast Stream
- [Create Simulcast](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcasts/README.md#create) - Set up multi-platform streaming
- [Delete Simulcast](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcasts/README.md#delete) - Remove simulcast configuration
- [Get Simulcast](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcaststream/README.md#getspecific) - Retrieve simulcast settings
- [Update Simulcast](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcasts/README.md#update) - Modify simulcast parameters

### Video Data API

Monitor video performance and quality with comprehensive analytics and real-time metrics.

For detailed documentation, see [FastPix Video Data Overview](https://docs.fastpix.io/docs/video-data-overview).

#### Metrics
- [List Breakdown Values](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/metrics/README.md#listbreakdownvalues) - Get detailed breakdown of metrics by dimension
- [List Overall Values](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/metrics/README.md#listoverallvalues) - Get aggregated metric values across all content
- [Get Timeseries Data](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/metrics/README.md#gettimeseriesdata) - Retrieve time-based metric trends and patterns
- [Compare Values](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/metrics/README.md#compare) - List comparison values

#### Views
- [List Video Views](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/views/README.md#list) - Get comprehensive list of video viewing sessions
- [Get View Details](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/views/README.md#getviewdetails) - Retrieve detailed information about specific video views
- [List Top Content](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/views/README.md#listbytopcontent) - Find your most popular and engaging content

#### Dimensions
- [List Dimensions](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/dimensions/README.md#list) - Get available data dimensions for filtering and analysis
- [List Filter Values](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/dimensions/README.md#listfilters) - Get specific values for a particular dimension

#### Errors
- [List Errors](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/errors/README.md#list) - Get list of playback errors

### Transformations

Transform and enhance your video content with powerful AI and editing capabilities.

#### In-Video AI Features

Enhance video content with AI-powered features including moderation, summarization, and intelligent categorization.

- [Update Summary](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/invideoaifeatures/README.md#updatesummary) - Create AI-generated video summaries
- [Update Chapters](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/invideoai/README.md#updatemediachapters) - Automatically generate video chapter markers
- [Extract Entities](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/invideoai/README.md#updatenamedentities) - Identify and extract named entities from content
- [Enable Moderation](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/moderations/README.md#update) - Activate content moderation and safety checks

#### Media Clips

- [List Live Clips](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/videos/README.md#listliveclips) - Get all clips of a live stream
- [List Media Clips](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#listclips) - Retrieve all clips associated with a source media

#### Subtitles

- [Generate Subtitles](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#generatesubtitles) - Create automatic subtitles for media

#### Media Tracks

- [Add Track](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#addmediatrack) - Add audio or subtitle tracks to media
- [Update Track](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/videos/README.md#updatetrack) - Modify existing audio or subtitle tracks
- [Delete Track](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/tracks/README.md#delete) - Remove audio or subtitle tracks

#### Access Control

- [Update Source Access](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#updatesourceaccess) - Control access permissions for media source

#### Format Support

- [Update MP4 Support](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#updatemp4support) - Configure MP4 download capabilities

<!-- End Available Resources and Operations [operations] -->

<!-- Start Retries [retries] -->
## Retries

Some of the endpoints in this SDK support retries. If you use the SDK without any configuration, it will fall back to the default retry strategy provided by the API. However, the default retry strategy can be overridden on a per-operation basis, or across the entire SDK.

To change the default retry strategy for a single API call, simply pass a `RetryConfig` to the call:

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils.Retries;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreatePullVideoInput(
            new PullVideoInput() {}
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "<key>", "<value>" },
    },
};

var res = await sdk.InputVideo.CreateMediaAsync(
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
using Fastpix.Utils.Retries;
using System.Collections.Generic;

var sdk = new FastpixSDK(
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
        Password = "your-secret-key",
    }
);

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreatePullVideoInput(
            new PullVideoInput() {}
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "<key>", "<value>" },
    },
};

var res = await sdk.InputVideo.CreateMediaAsync(req);

// handle response
```
<!-- End Retries [retries] -->

<!-- Start Error Handling [errors] -->
## Error Handling

[`FastpixException`](./src/Fastpix/Models/Errors/FastpixException.cs) is the base exception class for all HTTP error responses. It has the following properties:

| Property      | Type                  | Description           |
|---------------|-----------------------|-----------------------|
| `Message`     | *string*              | Error message         |
| `Request`     | *HttpRequestMessage*  | HTTP request object   |
| `Response`    | *HttpResponseMessage* | HTTP response object  |
| `Body`        | *string*              | HTTP response body    |

### Example

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Errors;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

try
{
    CreateMediaRequest req = new CreateMediaRequest() {
        Inputs = new List<Fastpix.Models.Components.Input>() {
            Fastpix.Models.Components.Input.CreatePullVideoInput(
                new PullVideoInput() {}
            ),
        },
        Metadata = new Dictionary<string, string>() {
            { "<key>", "<value>" },
        },
    };

    var res = await sdk.InputVideo.CreateMediaAsync(req);

    // handle response
}
catch (FastpixException ex)  // all SDK exceptions inherit from FastpixException
{
    // ex.ToString() provides a detailed error message
    System.Console.WriteLine(ex);

    // Base exception fields
    HttpRequestMessage request = ex.Request;
    HttpResponseMessage response = ex.Response;
    var statusCode = (int)response.StatusCode;
    var responseBody = ex.Body;
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

**Primary exception:**
* [`FastpixException`](./src/Fastpix/Models/Errors/FastpixException.cs): The base class for HTTP error responses.

<details><summary>Less common exceptions (2)</summary>

* [`System.Net.Http.HttpRequestException`](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequestexception): Network connectivity error. For more details about the underlying cause, inspect the `ex.InnerException`.

* Inheriting from [`FastpixException`](./src/Fastpix/Models/Errors/FastpixException.cs):
  * [`ResponseValidationError`](./src/Fastpix/Models/Errors/ResponseValidationError.cs): Thrown when the response data could not be deserialized into the expected type.
</details>
<!-- End Error Handling [errors] -->

<!-- Start Server Selection [server] -->
## Server Selection

### Override Server URL Per-Client

The default server can be overridden globally by passing a URL to the `serverUrl: string` optional parameter when initializing the SDK client instance. For example:

```csharp
using Fastpix;
using Fastpix.Models.Components;
using System.Collections.Generic;

var sdk = new FastpixSDK(
    serverUrl: "<server-url>",
    security: new Security() {
        Username = "your-access-token",
        Password = "your-secret-key",
    }
);

CreateMediaRequest req = new CreateMediaRequest() {
    Inputs = new List<Fastpix.Models.Components.Input>() {
        Fastpix.Models.Components.Input.CreatePullVideoInput(
            new PullVideoInput() {}
        ),
    },
    Metadata = new Dictionary<string, string>() {
        { "<key>", "<value>" },
    },
};

var res = await sdk.InputVideo.CreateMediaAsync(req);

// handle response
```
<!-- End Server Selection [server] -->

<!-- Start Custom HTTP Client [http-client] -->
## Custom HTTP Client

The C# SDK makes API calls using an `IFastpixHttpClient` that wraps the native
[HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient). This
client provides the ability to attach hooks around the request lifecycle that can be used to modify the request or handle
errors and response.

The `IFastpixHttpClient` interface allows you to either use the default `FastpixHttpClient` that comes with the SDK,
or provide your own custom implementation with customized configuration such as custom message handlers, timeouts,
connection pooling, and other HTTP client settings.

The following example shows how to create a custom HTTP client with request modification and error handling:

```csharp
using Fastpix;
using Fastpix.Utils;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

// Create a custom HTTP client
public class CustomHttpClient : IFastpixHttpClient
{
    private readonly IFastpixHttpClient _defaultClient;

    public CustomHttpClient()
    {
        _defaultClient = new FastpixHttpClient();
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken? cancellationToken = null)
    {
        // Add custom header and timeout
        request.Headers.Add("x-custom-header", "custom value");
        request.Headers.Add("x-request-timeout", "30");
        
        try
        {
            var response = await _defaultClient.SendAsync(request, cancellationToken);
            // Log successful response
            Console.WriteLine($"Request successful: {response.StatusCode}");
            return response;
        }
        catch (Exception error)
        {
            // Log error
            Console.WriteLine($"Request failed: {error.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _defaultClient?.Dispose();
    }
}

// Use the custom HTTP client with the SDK
var customHttpClient = new CustomHttpClient();
var sdk = new FastpixSDK(client: customHttpClient);
```

<details>
<summary>You can also provide a completely custom HTTP client with your own configuration:</summary>

```csharp
using Fastpix.Utils;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

// Custom HTTP client with custom configuration
public class AdvancedHttpClient : IFastpixHttpClient
{
    private readonly HttpClient _httpClient;

    public AdvancedHttpClient()
    {
        var handler = new HttpClientHandler()
        {
            MaxConnectionsPerServer = 10,
            // ServerCertificateCustomValidationCallback = customCertValidation, // Custom SSL validation if needed
        };

        _httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken? cancellationToken = null)
    {
        return await _httpClient.SendAsync(request, cancellationToken ?? CancellationToken.None);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

var sdk = new FastpixSDK(client: new AdvancedHttpClient());
```
</details>

<details>
<summary>For simple debugging, you can enable request/response logging by implementing a custom client:</summary>

```csharp
public class LoggingHttpClient : IFastpixHttpClient
{
    private readonly IFastpixHttpClient _innerClient;

    public LoggingHttpClient(IFastpixHttpClient innerClient = null)
    {
        _innerClient = innerClient ?? new FastpixHttpClient();
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken? cancellationToken = null)
    {
        // Log request
        Console.WriteLine($"Sending {request.Method} request to {request.RequestUri}");
        
        var response = await _innerClient.SendAsync(request, cancellationToken);
        
        // Log response
        Console.WriteLine($"Received {response.StatusCode} response");
        
        return response;
    }

    public void Dispose() => _innerClient?.Dispose();
}

var sdk = new FastpixSDK(client: new LoggingHttpClient());
```
</details>

The SDK also provides built-in hook support through the `SDKConfiguration.Hooks` system, which automatically handles
`BeforeRequestAsync`, `AfterSuccessAsync`, and `AfterErrorAsync` hooks for advanced request lifecycle management.
<!-- End Custom HTTP Client [http-client] -->

<!-- Placeholder for Future Fastpix SDK Sections -->

# Development

This C# SDK is programmatically generated from our API specifications. Any manual modifications to internal files will be overwritten during subsequent generation cycles. 

We value community contributions and feedback. Feel free to submit pull requests or open issues with your suggestions, and we'll do our best to include them in future releases.

## Detailed Usage

For comprehensive understanding of each API's functionality, including detailed request and response specifications, parameter descriptions, and additional examples, please refer to the [FastPix API Reference](https://docs.fastpix.io/reference/signingkeys-overview).

The API reference offers complete documentation for all available endpoints and features, enabling developers to integrate and leverage FastPix APIs effectively.
