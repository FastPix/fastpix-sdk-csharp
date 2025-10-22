# FastPix C# SDK Samples

Simple, clear examples for using the FastPix C# SDK.

## Sample Files

- **[01-sdk-initialization.cs](01-sdk-initialization.cs)** - Initialize the SDK
- **[02-video-upload.cs](02-video-upload.cs)** - Upload videos from device and URLs
- **[03-video-management.cs](03-video-management.cs)** - List, get, and update media
- **[04-playback-management.cs](04-playback-management.cs)** - Create playback IDs and streaming URLs
- **[05-live-streaming.cs](05-live-streaming.cs)** - Create and manage live streams
- **[06-ai-features.cs](06-ai-features.cs)** - Generate video summaries and chapters
- **[07-playlist-management.cs](07-playlist-management.cs)** - Create and manage playlists
- **[08-error-handling.cs](08-error-handling.cs)** - Handle errors and retries

## Quick Start

1. **Install**: Add FastPix NuGet package to your project
2. **Get Credentials**: Get your API access token and secret key
3. **Copy Code**: Use the sample code and replace credentials
4. **Run**: Execute the examples

## Prerequisites

- .NET 8.0+
- FastPix API credentials
- Visual Studio or your preferred C# IDE

## Basic Example

```csharp
using Fastpix;

// Initialize SDK
var client = FastPix.Builder()
    .WithSecurity(new Security
    {
        AccessToken = "your-access-token",
        SecretKey = "your-secret-key"
    })
    .Build();

// List media
var response = await client.Media.ListAsync(limit: 10);
Console.WriteLine($"Found {response.Object.Data.Count} media items");
```

## Documentation

- [FastPix API Docs](https://docs.fastpix.io)
- [FastPix Support](https://fastpix.io/support)
