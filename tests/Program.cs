using Fastpix.Tests;

// Endpoint validation harness for the Fastpix C# SDK.
//
//   dotnet run --project tests              # GET endpoints (default, read-only)
//   dotnet run --project tests -- get       # GET endpoints
//   dotnet run --project tests -- non-get   # POST/PUT/PATCH/DELETE lifecycle (MUTATES data)
//   dotnet run --project tests -- all       # both
//
// Each mode:
//   1. calls the live API + validates the raw response against the OpenAPI schema,
//   2. calls the matching C# SDK method in-process,
//   3. diffs JSON paths between the raw API body and the SDK-parsed body,
//   4. writes artifacts + a markdown report under tests/.
//
// Requires real BasicAuth credentials:
//   export FASTPIX_USERNAME=... FASTPIX_PASSWORD=...
// Optional: FASTPIX_BASE_URL, FASTPIX_SPEC.

var mode = (args.FirstOrDefault() ?? "get").ToLowerInvariant();

return mode switch
{
    "get" => await EndpointValidator.RunAsync(),
    "non-get" or "nonget" or "mutate" => await NonGetValidator.RunAsync(),
    "all" => Math.Max(await EndpointValidator.RunAsync(), await NonGetValidator.RunAsync()),
    _ => Fail($"Unknown mode '{mode}'. Use: get | non-get | all"),
};

static int Fail(string msg)
{
    Console.Error.WriteLine(msg);
    return 2;
}
