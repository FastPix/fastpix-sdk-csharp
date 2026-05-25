
# Changelog

All notable changes to this project will be documented in this file.

---

## [1.1.3]

### ⚠️ Important — FastPix is migrating from `.io` to `.com`

All FastPix hosts and documentation links are moving to the `.com` TLD:

| Old (`.io`) | New (`.com`) |
|---|---|
| `api.fastpix.io` | `api.fastpix.com` |
| `stream.fastpix.io` | `stream.fastpix.com` |
| `images.fastpix.io` | `images.fastpix.com` |
| `dashboard.fastpix.io` | `dashboard.fastpix.com` |
| `www.fastpix.io` | `www.fastpix.com` |
| `docs.fastpix.io/...` | `fastpix.com/docs/...` |

The `.io` hosts continue to serve traffic during the transition, but **they are slated for deprecation soon** — please update any hard-coded references in your application as part of your next deploy. **We strongly recommend upgrading to this SDK release (or later) across every language you use** — every official FastPix SDK is being rolled out with the same migration.

What this means for users of the `Fastpix` C# SDK:

- **If you rely on SDK defaults**, no code change is required. The default server URL is `https://api.fastpix.com/v1/`, so updating the `Fastpix` package to `1.1.3` (e.g. `dotnet add package Fastpix --version 1.1.3`) is enough.
- **If you have an explicit server URL override** (e.g. `new FastpixSDK(serverUrl: "https://api.fastpix.io/v1/")` or `FastpixSDK.Builder().WithServerUrl("https://api.fastpix.io/v1/")`), change it to `https://api.fastpix.com/v1/`.
- **If you reference FastPix asset URLs directly** in your app (HLS playback URLs, image CDN, dashboard deep links), update those to the `.com` equivalents before `.io` is decommissioned.

All README and per-SDK doc links in this package have been updated to point at the new `https://fastpix.com/docs/...` URLs.

### Fixed (SDK ↔ API parity)

- `ManageVideos.ListAsync` (`/on-demand`): tracks now include `frameRate`, which was being silently dropped by the previous SDK build (the field was present in the spec but missing from the generated `VideoTrackForGetAll` model).
- `SigningKeys.DeleteAsync`: response shape now includes the optional `data.message` confirmation string the API has been returning.

### Docs

- All README and per-service documentation pages updated from `docs.fastpix.io/...` and `docs.fastpix.com/...` to the new `https://fastpix.com/docs/...` URL structure.

---

## [1.1.2]

### Fixed
- Fixed data event field remapping in hooks.

## [1.1.1]

- Updated documentation redirection links in README.md.
  
## [1.1.0]

- Fixed missing parameters in multiple API methods.
- Improved overall developer experience through more accurate typings.

---

## [1.0.0]

This release introduces a C#-modified version of the SDK.

- It is a generated-code-based SDK, and direct contributions or pull requests are not accepted.
- Instead of modifying the code directly, users are encouraged to open issues for bug reports or feature suggestions.
- Refer to the `CONTRIBUTING.md` for full contribution guidelines, including how to:
  - Report issues clearly with steps to reproduce
  - Share relevant logs, screenshots, or environment details
- The SDK changes in this version aim to improve compatibility with C# environments and follow platform-specific conventions.
- All fixes or improvements will be included in the next code generation cycle.

---

## [0.1.2]

- Redirection links were corrected for all the methods listed under "Available Resources and Operations" in the `README.md`, ensuring users are routed to the appropriate documentation.

---

## [0.1.1]

- Codebase updated to reflect consistent naming conventions, improving overall clarity and maintainability.

---

## [0.1.0]

Initial release of the SDK.

- **Media API**:
  - Upload media assets
  - List, fetch, update, and delete media
  - Generate and manage playback IDs
- **Live API**:
  - Create, list, update, and delete live streams
  - Generate and manage playback IDs
  - Support simulcasting to multiple platforms
- Designed for secure and efficient communication with the FastPix API

---
