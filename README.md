# FastPix C# SDK

Developer-friendly & type-safe C# SDK specifically designed to leverage the FastPix platform API.

# Introduction

The FastPix C# SDK simplifies integration with the FastPix platform. This SDK is designed for secure and efficient communication with the FastPix API, enabling easy management of media uploads, live streaming, and simulcasting.

# Key Features

- ## Media API
  - **Upload Media**: Upload media files seamlessly from URLs or devices
  - **Manage Media**: Perform operations such as listing, fetching, updating, and deleting media assets
  - **Playback IDs**: Generate and manage playback IDs for media access

- ## Live API
  - **Create & Manage Live Streams**: Create, list, update, and delete live streams effortlessly
  - **Control Stream Access**: Generate playback IDs for live streams to control and manage access
  - **Simulcast to Multiple Platforms**: Stream content to multiple platforms simultaneously

For detailed usage, refer to the [FastPix API Reference](https://docs.fastpix.io/reference).
<!-- Start SDK Installation [installation] -->
## SDK Installation

To add a reference to a local instance of the SDK in a .NET project:
```bash
dotnet add reference src/Fastpix/Fastpix.csproj
```
<!-- End SDK Installation [installation] -->

<!-- Start SDK Example Usage [usage] -->
## SDK Example Usage

### Example

```C#
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(security: new Security() {
    username("your-access-token-id")
    password("your-secret-key")
});

CreateLiveStreamRequest req = new CreateLiveStreamRequest() {
    PlaybackSettings = new PlaybackSettings() {},
    InputMediaSettings = new InputMediaSettings() {
        Metadata = new CreateLiveStreamRequestMetadata() {},
    },
};

var res = await sdk.StartLiveStream.CreateNewStreamAsync(req);

// handle response
```
<!-- End SDK Example Usage [usage] -->

<!-- Start Authentication [security] -->

## Available Resources and Operations

### [InputVideo](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/inputvideo/README.md)

* [CreateMedia](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/inputvideo/README.md#createmedia) - Create media from URL
* [DirectUploadVideoMedia](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/inputvideo/README.md#directuploadvideomedia) - Upload media from device

### [ManageLiveStream](https://github.com/FastPix/fastpix-sdk-csharp/tree/main/docs/sdks/managelivestream)

* [GetAllStreams](https://github.com/FastPix/fastpix-sdk-csharp/tree/main/docs/sdks/managelivestream#getallstreams) - Get all live streams
* [GetLiveStreamById](https://github.com/FastPix/fastpix-sdk-csharp/tree/main/docs/sdks/managelivestream#getlivestreambyid) - Get stream by ID
* [DeleteLiveStream](https://github.com/FastPix/fastpix-sdk-csharp/tree/main/docs/sdks/managelivestream#deletelivestream) - Delete a stream
* [UpdateLiveStream](https://github.com/FastPix/fastpix-sdk-csharp/tree/main/docs/sdks/managelivestream#updatelivestream) - Update a stream

### [ManageVideos](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md)

* [ListMedia](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#listmedia) - Get list of all media
* [GetMedia](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#getmedia) - Get a media by ID
* [UpdatedMedia](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#updatedmedia) - Update a media by ID
* [DeleteMedia](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#deletemedia) - Delete a media by ID
* [RetrieveMediaInputInfo](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/managevideos/README.md#retrievemediainputinfo) - Get info of media inputs

### [Playback](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md)

* [CreatePlaybackIdOfStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#createplaybackidofstream) - Create a playbackId
* [DeletePlaybackIdOfStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#deleteplaybackidofstream) - Delete a playbackId
* [GetLiveStreamPlaybackId](dhttps://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#getlivestreamplaybackid) - Get stream's playbackId
* [CreateMediaPlaybackId](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#createmediaplaybackid) - Create a playback ID
* [DeleteMediaPlaybackId](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/playback/README.md#deletemediaplaybackid) - Delete a playback ID

### [SimulcastStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcaststream/README.md)

* [CreateSimulcastOfStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcaststream/README.md#createsimulcastofstream) - Create a simulcast
* [DeleteSimulcastOfStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcaststream/README.md#deletesimulcastofstream) - Delete a simulcast
* [GetSpecificSimulcastOfStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcaststream/README.md#getspecificsimulcastofstream) - Get a specific simulcast of a stream
* [UpdateSpecificSimulcastOfStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/simulcaststream/README.md#updatespecificsimulcastofstream) - Update a specific simulcast of a stream

### [StartLiveStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/startlivestream/README.md)

* [CreateNewStream](https://github.com/FastPix/fastpix-sdk-csharp/blob/main/docs/sdks/startlivestream/README.md#createnewstream) - Create a new stream

</details>
<!-- End Available Resources and Operations [operations] -->

<!-- Start Error Handling [errors] -->
## Error Handling

Handling errors in this SDK should largely match your expectations. All operations return a response object or throw an exception.

By default, an API error will raise a `Fastpix.Models.Errors.APIException` exception, which has the following properties:

| Property      | Type                  | Description           |
|---------------|-----------------------|-----------------------|
| `Message`     | *string*              | The error message     |
| `Request`     | *HttpRequestMessage*  | The HTTP request      |
| `Response`    | *HttpResponseMessage* | The HTTP response     |

When custom error responses are specified for an operation, the SDK may also throw their associated exceptions. You can refer to respective *Errors* tables in SDK docs for more details on possible exception types for each operation. For example, the `CreateNewStreamAsync` method throws the following exceptions:

| Error Type                                       | Status Code | Content Type     |
| ------------------------------------------------ | ----------- | ---------------- |
| Fastpix.Models.Errors.UnauthorizedException      | 401         | application/json |
| Fastpix.Models.Errors.InvalidPermissionException | 403         | application/json |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422         | application/json |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX    | \*/\*            |

### Example

```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Errors;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token-id",
    Password = "your-secret-key",
});

try
{
    CreateLiveStreamRequest req = new CreateLiveStreamRequest() {
        PlaybackSettings = new PlaybackSettings() {},
        InputMediaSettings = new InputMediaSettings() {
            Metadata = new CreateLiveStreamRequestMetadata() {},
        },
    };

    var res = await sdk.StartLiveStream.CreateNewStreamAsync(req);

    // handle response
}
catch (Exception ex)
{
    if (ex is UnauthorizedException)
    {
        // Handle exception data
        throw;
    }
    else if (ex is InvalidPermissionException)
    {
        // Handle exception data
        throw;
    }
    else if (ex is ValidationErrorResponse)
    {
        // Handle exception data
        throw;
    }
    else if (ex is Fastpix.Models.Errors.APIException)
    {
        // Handle default exception
        throw;
    }
}
```
<!-- End Error Handling [errors] -->

<!-- Start Server Selection [server] -->
## Server Selection

### Override Server URL Per-Client

The default server can be overridden globally by passing a URL to the `serverUrl: string` optional parameter when initializing the SDK client instance. For example:
```C#
using Fastpix;
using Fastpix.Models.Components;

var sdk = new FastPix(
    serverUrl: "https://v1.fastpix.io/live",
    security: new Security() {
        Username = "your-access-token-id",
        Password = "your-secret-key",
    }
);

CreateLiveStreamRequest req = new CreateLiveStreamRequest() {
    PlaybackSettings = new PlaybackSettings() {},
    InputMediaSettings = new InputMediaSettings() {
        Metadata = new CreateLiveStreamRequestMetadata() {},
    },
};

var res = await sdk.StartLiveStream.CreateNewStreamAsync(req);

// handle response
```
<!-- End Server Selection [server] -->

# Development

## Maturity

This SDK is in beta, and there may be breaking changes between versions without a major version update. Therefore, we recommend pinning usage
to a specific package version. This way, you can install the same version each time without breaking changes unless you are intentionally
looking for the latest version.