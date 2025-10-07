using System;
using Fastpix;
using Fastpix.Models.Components;

namespace Fastpix.Tests;

public static class TestConfig
{
    public static bool TryCreateClient(out FastPix sdk, out string? reason)
    {
        var username = Environment.GetEnvironmentVariable("FASTPIX_USERNAME");
        var password = Environment.GetEnvironmentVariable("FASTPIX_PASSWORD");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            sdk = null!;
            reason = "FASTPIX_USERNAME/FASTPIX_PASSWORD not set";
            return false;
        }

        var security = new Security
        {
            Username = username,
            Password = password
        };

        sdk = new FastPix(security: security);
        reason = null;
        return true;
    }

    // Returns true if credentials exist; tests can early-return when false
    public static bool HasCredentials() =>
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FASTPIX_USERNAME")) &&
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FASTPIX_PASSWORD"));
}


