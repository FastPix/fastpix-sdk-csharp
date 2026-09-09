# Fastpix C# SDK — Endpoint Validation

This directory contains a validation harness that checks the Fastpix C# SDK
against the live API and the OpenAPI spec. It is the C# counterpart of
`fastpix-php/Tests/validate-get-endpoints.ts` and `validate-non-get-endpoints.ts`,
but it calls the SDK **in-process** instead of shelling out to a separate runtime.

## Modes

```bash
dotnet run --project tests              # GET endpoints (default, read-only)
dotnet run --project tests -- get       # GET endpoints
dotnet run --project tests -- non-get   # POST/PUT/PATCH/DELETE lifecycle — MUTATES live data
dotnet run --project tests -- all       # both
```

- **GET** (`EndpointValidator`) — read-only; validates every `GET` operation.
- **non-GET** (`NonGetValidator`) — runs a **CREATE → UPDATE → DELETE** lifecycle: creates real
  resources (media, streams, playlists, signing keys, simulcasts, uploads), exercises updates
  against them, then deletes them last. It polls for async readiness (media/track) and retries
  while a playback id is still provisioning. **This mutates the workspace — use a test account.**

## GET mode

For every `GET` operation in the OpenAPI spec the harness:

1. **Calls the live API** directly and captures the raw JSON response.
2. **Validates** that raw response against the OpenAPI response schema
   (via [NJsonSchema](https://github.com/RicoSuter/NJsonSchema)).
3. **Calls the matching C# SDK method** (in-process) and captures either the
   parsed response object or the thrown error (normalized).
4. **Diffs JSON paths** between the raw API body and the SDK-parsed body, using
   the same normalization rules as the PHP harness (snake→camel key
   canonicalization, empty-array/null symmetry, and the deliberate
   `get_video_view_details` event-field remap from `EventsFieldRemapHook`).
5. Writes per-endpoint artifacts and markdown reports.

## Files

| File | Purpose |
|---|---|
| `Program.cs` | Entry point — dispatches by mode (`get` / `non-get` / `all`). |
| `EndpointValidator.cs` | GET orchestration, comparison, and report generation. |
| `NonGetValidator.cs` | Non-GET CREATE→UPDATE→DELETE lifecycle + report. |
| `OpenApiSpec.cs` | Spec loading, GET/non-GET extraction, NJsonSchema validators. |
| `SdkInvoker.cs` | Maps each GET `operationId` to its C# SDK method; normalizes errors. |
| `NonGetSdkInvoker.cs` | Maps each mutating `operationId` to its C# SDK method; reads the raw wire body. |
| `JsonDiff.cs` | JSON path collection, key canonicalization, event remap. |
| `Fixtures.cs` | Fixture loading, per-operation defaults, live-API URL building (GET). |
| `get-endpoints-fixtures.json` | Real path-param / query values per `operationId`. |

The non-GET lifecycle needs **no fixtures** — it creates the resources it operates on and
writes artifacts to `artifacts-non-get/` plus `NON_GET_ENDPOINTS_VALIDATION_REPORT.md`.

## Setup

Provide real BasicAuth credentials (the harness refuses to run with placeholders):

```bash
export FASTPIX_USERNAME="your-access-token"
export FASTPIX_PASSWORD="your-secret-key"
```

Optional overrides:

- `FASTPIX_BASE_URL` — defaults to the spec's `servers[0].url` (`https://api.fastpix.com/v1/`).
- `FASTPIX_SPEC` — path to the OpenAPI spec. By default the runner searches upward
  from the project for `openapi.yaml` (kept untracked at the repo root).

## Run

```bash
# from the repo root
dotnet run --project tests
```

## Fixtures

`get-endpoints-fixtures.json` maps each `operationId` to the `pathParams` and
`query` values used for both the live API call and the SDK call. Replace the
sample IDs with resources that exist in your workspace to avoid `404`s on
detail endpoints (`get-media`, `get-live-stream-by-id`, etc.).

## Outputs

After a run the following are written under `tests/`:

- `GET_ENDPOINTS_OPENAPI_RESPONSE_VALIDATION_REPORT.md` — full per-endpoint report.
- `GET_ENDPOINTS_OPENAPI_RESPONSE_FIX_SUGGESTIONS.md` — heuristic spec-fix suggestions for failures.
- `artifacts/<operationId>.api.json` / `.sdk.json` — captured API and SDK payloads.
- The consolidated table below is refreshed in place.

A row **PASSes** only when the OpenAPI response validates, the SDK call parses,
and there are no JSON-path discrepancies in either direction.

## Latest consolidated report

<!-- BEGIN GET_ENDPOINTS_CONSOLIDATED -->
<!-- END GET_ENDPOINTS_CONSOLIDATED -->
