#:project /Users/sumasree/fp-csharp/fastpix-sdk-csharp/src/Fastpix/Fastpix.csproj
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
        ReferenceId = "a3",
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