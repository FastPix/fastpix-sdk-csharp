using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

// A minimal FastPix integration with exactly two endpoints:
//   POST /uploads   -> mint a signed upload URL (client uploads directly to it)
//   POST /webhooks  -> verify the signature, then handle the event
// Credentials come from the environment; see README.md.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string Require(string name) =>
    Environment.GetEnvironmentVariable(name)
    ?? throw new InvalidOperationException($"Missing {name}. See README.md.");

var sdk = new FastpixSDK(security: new Security
{
    Username = Require("FASTPIX_USERNAME"),
    Password = Require("FASTPIX_PASSWORD"),
});

// Mint a signed direct-upload URL. The file is PUT straight to this URL by the
// client, so the bytes never pass through this server.
// Production note: this endpoint mints upload URLs — put your own auth in front of it.
app.MapPost("/uploads", async () =>
{
    var res = await sdk.InputVideo.UploadAsync(new DirectUploadVideoMediaRequest
    {
        CorsOrigin = "*", // lets a browser PUT from anywhere — tighten for production
        PushMediaSettings = new PushMediaSettings(),
    });
    var upload = res.Object?.Data;
    return Results.Ok(new { uploadId = upload?.UploadId, url = upload?.Url });
});

// Handle FastPix webhooks. No CSRF token is involved: this is a server-to-server
// call authenticated by the HMAC signature, not a browser cookie.
app.MapPost("/webhooks", async (HttpRequest request) =>
{
    // Read the RAW body before any parsing — the signature covers these exact bytes.
    using var ms = new MemoryStream();
    await request.Body.CopyToAsync(ms);
    var rawBody = ms.ToArray();

    var signature = request.Headers["FastPix-Signature"].ToString();
    var secret = Environment.GetEnvironmentVariable("FASTPIX_WEBHOOK_SECRET") ?? "";

    if (!IsValidSignature(rawBody, signature, secret))
    {
        return Results.Unauthorized();
    }

    // Signature is good — now it's safe to parse and dispatch on the event type.
    var eventType = "unknown";
    try
    {
        using var doc = JsonDocument.Parse(rawBody);
        if (doc.RootElement.TryGetProperty("type", out var t))
        {
            eventType = t.GetString() ?? "unknown";
        }
    }
    catch (JsonException) { /* keep "unknown"; still ack so FastPix stops retrying */ }

    switch (eventType)
    {
        case "video.media.ready":
            Console.WriteLine("media is ready for playback");
            break;
        case "video.media.failed":
            Console.WriteLine("media processing failed");
            break;
        default:
            Console.WriteLine($"received event: {eventType}");
            break;
    }

    // Ack fast with 2xx — FastPix retries on any non-2xx response.
    return Results.Ok();
});

app.Run();

// Verify a FastPix webhook signature. The Signing Secret (Dashboard > Webhooks)
// is Base64, so sign with its DECODED bytes; the header is Base64 HMAC-SHA256 of
// the raw body. Compare in constant time.
static bool IsValidSignature(byte[] rawBody, string signature, string secret)
{
    if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(signature))
    {
        return false;
    }

    byte[] key;
    try
    {
        key = Convert.FromBase64String(secret);
    }
    catch (FormatException)
    {
        return false;
    }

    using var hmac = new HMACSHA256(key);
    var expected = Convert.ToBase64String(hmac.ComputeHash(rawBody));
    return CryptographicOperations.FixedTimeEquals(
        Encoding.UTF8.GetBytes(expected),
        Encoding.UTF8.GetBytes(signature));
}
