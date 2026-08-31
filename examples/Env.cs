using Fastpix;
using Fastpix.Models.Components;

namespace FastPix.Examples;

// Small helper so every example reads credentials the same way and never
// hardcodes them. Setup (env vars, .env) is documented once in README.md.
internal static class Env
{
    // Builds an SDK client from FASTPIX_USERNAME (Access Token) and
    // FASTPIX_PASSWORD (Secret Key).
    public static FastpixSDK CreateSdk()
    {
        return new FastpixSDK(security: new Security
        {
            Username = Require("FASTPIX_USERNAME"),
            Password = Require("FASTPIX_PASSWORD"),
        });
    }

    public static string Require(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException(
                $"Missing {name}. See examples/README.md for setup.");
        }
        return value;
    }
}
