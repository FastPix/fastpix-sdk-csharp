using System.Security.Cryptography;
using System.Text;

namespace FastPix.Examples;

// Verify a FastPix webhook signature before trusting the payload — offline, no credentials.
//
// FastPix signs the raw request body with your webhook Signing Secret
// (Dashboard > Webhooks) and sends it as a Base64 HMAC-SHA256 in the
// "FastPix-Signature" header. The Signing Secret is itself Base64-encoded, so
// sign with its DECODED bytes as the key. Verify the exact bytes as received:
// parsing and re-serializing changes them and the signature won't match.
internal static class VerifyWebhook
{
    public static bool IsValidSignature(byte[] rawBody, string signature, string secret)
    {
        if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(signature))
        {
            return false;
        }

        byte[] key;
        try
        {
            key = Convert.FromBase64String(secret); // Signing Secret is Base64.
        }
        catch (FormatException)
        {
            return false;
        }

        using var hmac = new HMACSHA256(key);
        var expected = Convert.ToBase64String(hmac.ComputeHash(rawBody));
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(signature)); // constant-time compare
    }

    // Self-signs a demo payload and checks valid / wrong-secret / tampered-body,
    // so this runs with zero credentials.
    public static Task Run()
    {
        var secret = Environment.GetEnvironmentVariable("FASTPIX_WEBHOOK_SECRET")
            ?? Convert.ToBase64String(Encoding.UTF8.GetBytes("demo-secret"));
        var rawBody = Encoding.UTF8.GetBytes(
            "{\"type\":\"video.media.ready\",\"data\":{\"id\":\"abc-123\"}}");
        var signature = Sign(rawBody, secret);

        Assert(IsValidSignature(rawBody, signature, secret),
            "a correct signature should verify");

        var wrongSignature = Sign(rawBody, Convert.ToBase64String(Encoding.UTF8.GetBytes("other-secret")));
        Assert(!IsValidSignature(rawBody, wrongSignature, secret),
            "a signature from the wrong secret should be rejected");

        var tamperedBody = Encoding.UTF8.GetBytes(
            "{\"type\":\"video.media.ready\",\"data\":{\"id\":\"tampered\"}}");
        Assert(!IsValidSignature(tamperedBody, signature, secret),
            "a tampered body should be rejected");

        Console.WriteLine("verified");
        return Task.CompletedTask;
    }

    private static string Sign(byte[] rawBody, string secret)
    {
        using var hmac = new HMACSHA256(Convert.FromBase64String(secret));
        return Convert.ToBase64String(hmac.ComputeHash(rawBody));
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception($"webhook self-test failed: {message}");
        }
    }
}
