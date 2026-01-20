# Fastpix SDK Test Example

This directory contains a sample test application that demonstrates how to use the Fastpix SDK.

## Setup

1. **Set up your credentials** (choose one method):

   **Option A: Environment Variables (Recommended)**
   ```bash
   export FASTPIX_USERNAME="your-username"
   export FASTPIX_PASSWORD="your-password"
   ```

   **Option B: Edit Program.cs**
   - Open `Program.cs` and replace `"your-username"` and `"your-password"` with your actual credentials

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run the test**:
   ```bash
   dotnet run
   ```

## What the Test Does

The test program demonstrates:

1. **SDK Initialization**: Shows how to initialize the Fastpix SDK with security credentials
2. **Create Media API**: Calls `InputVideo.CreateMediaAsync()` to create a new media item from a URL
3. **Request Building**: Shows how to build a `CreateMediaRequest` with inputs and metadata
4. **Response Handling**: Displays the created media information in a formatted way
5. **Error Handling**: Includes try-catch blocks to handle any errors gracefully
6. **JSON Output**: Prints the full response as JSON for debugging purposes

## Example Output

```
=== Fastpix SDK Test Example ===

1. Testing Create Media API...
   Calling InputVideo.CreateMediaAsync()...

   Request Details:
   - Inputs Count: 1
   - Metadata Entries: 1
     * key1: value1

   Response received!

   ✅ Success Response:
   - Success: True
   - Media ID: abc123-def456-ghi789
   - Status: Preparing
   - Created At: 2024-01-15 10:30:00
   - Updated At: 2024-01-15 10:30:00
   - Trial: True
   - Playback IDs:
     * ID: pb-id-123
       Access Policy: Public

2. Full Response (JSON):
   {
     "CreateMediaSuccessResponse": {
       "success": true,
       "data": {...}
     }
   }

=== Test Completed Successfully ===
```

## Customization

### Setting a Custom Video URL

To use your own video URL, modify the `PullVideoInput` in `Program.cs`:

```csharp
Input.CreatePullVideoInput(
    new PullVideoInput()
    {
        Url = "https://example.com/your-video.mp4"
    }
)
```

### Testing Other SDK Methods

You can modify `Program.cs` to test other SDK methods:

- `sdk.ManageVideos.ListAsync()` - List all media
- `sdk.ManageVideos.GetByIdAsync()` - Get a specific media by ID
- `sdk.Videos.ListLiveClipsAsync()` - List live stream clips
- `sdk.LiveStreams.ListAsync()` - List live streams
- `sdk.Views.GetAsync()` - Get view metrics
- And many more...

Check the SDK documentation for all available methods.

