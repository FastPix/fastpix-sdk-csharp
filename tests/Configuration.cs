using System;

namespace Fastpix.Tests
{
    public static class Configuration
    {
        // Hardcoded values from your .env file
        private const string BASE_URL = "https://v1.fastpix.io";
        private const string LIVE_URL = "https://v1.fastpix.io/live";
        private const string USERNAME = "e6516d00-ff76-41b8-b357-7f10fe39981c";
        private const string PASSWORD = "33b22dff-9363-4461-886a-84e0dfa2ab5a";

        public static string GetBaseUrl() => BASE_URL;
        public static string GetApiUrl() => BASE_URL;
        public static string GetEnvironment() => "production";
        public static string GetUsername() => USERNAME;
        public static string GetPassword() => PASSWORD;
        public static string GetLiveUrl() => LIVE_URL;
    }
} 