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
  from the project for `fixed 7.yaml`, `fastpix.yaml`, `fixed.yaml`, `openapi.yaml`,
  or `fastpix-openapi.yaml`.

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
Last generated: 2026-06-10T07:05:48.4116620Z

- **Total GET endpoints**: 30
- **PASS**: 25
- **FAIL**: 5
- **SKIP**: 0

| Endpoint | OperationId | OpenAPI valid | SDK parse | Missing in SDK (present in API) | Missing in API (present in SDK) | Empty arrays omitted by SDK | Status |
|---|---|---:|---:|---|---|---|---|
| `/on-demand` | `list-media` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/{livestreamId}/live-clips` | `list-live-clips` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/{mediaId}` | `get-media` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/{mediaId}/summary` | `get-media-summary` | ✅ | ❌ | None | None | None | ❌ FAIL |
| `/on-demand/{mediaId}/input-info` | `retrieveMediaInputInfo` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/{mediaId}/playback-ids` | `list-playback-ids` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/uploads` | `list-uploads` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/{mediaId}/media-clips` | `get-media-clips` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/playlists` | `get-all-playlists` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/playlists/{playlistId}` | `get-playlist-by-id` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/{mediaId}/playback-ids/{playbackId}` | `get-playback-id` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/on-demand/drm-configurations` | `getDrmConfiguration` | ✅ | ❌ | None | None | None | ❌ FAIL |
| `/on-demand/drm-configurations/{drmConfigurationId}` | `getDrmConfigurationById` | ✅ | ❌ | None | None | None | ❌ FAIL |
| `/live/streams` | `get-all-streams` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/live/streams/{streamId}/viewer-count` | `get-live-stream-viewer-count-by-id` | ✅ | ❌ | None | None | None | ❌ FAIL |
| `/live/streams/{streamId}` | `get-live-stream-by-id` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/live/streams/{streamId}/playback-ids/{playbackId}` | `get-live-stream-playback-id` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/live/streams/{streamId}/simulcast/{simulcastId}` | `get-specific-simulcast-of-stream` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/iam/signing-keys` | `list_signing_keys` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/iam/signing-keys/{signingKeyId}` | `get-signing_key_by_id` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/viewlist` | `list_video_views` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/viewlist/{viewId}` | `get_video_view_details` | ✅ | ✅ | `data.custom`, `data.custom.Device`, `data.custom.Device[]`, `data.custom.Device[].dimensionName`, `data.custom.Device[].displayName` | None | None | ❌ FAIL |
| `/data/viewlist/top-content` | `list_by_top_content` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/dimensions` | `list_dimensions` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/dimensions/{dimensionsId}` | `list_filter_values_for_dimension` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/metrics/{metricId}/breakdown` | `list_breakdown_values` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/metrics/{metricId}/overall` | `list_overall_values` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/metrics/{metricId}/timeseries` | `get_timeseries_data` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/metrics/comparison` | `list_comparison_values` | ✅ | ✅ | None | None | None | ✅ PASS |
| `/data/errors` | `list_errors` | ✅ | ✅ | None | None | None | ✅ PASS |

Full details: `tests/GET_ENDPOINTS_OPENAPI_RESPONSE_VALIDATION_REPORT.md`
<!-- END GET_ENDPOINTS_CONSOLIDATED -->
