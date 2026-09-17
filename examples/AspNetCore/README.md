# FastPix + ASP.NET Core

A minimal web integration with two endpoints:

- `POST /uploads` — mints a signed upload URL with the SDK and returns
  `{ uploadId, url }`. The client uploads the file directly to that URL, so the
  bytes never pass through this server.
- `POST /webhooks` — verifies the `FastPix-Signature` on the raw request body,
  then dispatches on the event type and acks with `200`.

## Run

Set your credentials in the environment (copy `.env.example` to `.env` and load
it), then:

```bash
dotnet run
```

The app listens on `http://localhost:8080`.

| Variable | Where it comes from |
| --- | --- |
| `FASTPIX_USERNAME` | Dashboard → Access Token |
| `FASTPIX_PASSWORD` | Dashboard → Secret Key |
| `FASTPIX_WEBHOOK_SECRET` | Dashboard → Webhooks |

## Try it

Mint an upload URL and send a file straight to it:

```bash
# 1. Ask the app for a signed upload URL
UPLOAD_URL=$(curl -s -X POST localhost:8080/uploads \
  | python3 -c "import sys, json; print(json.load(sys.stdin)['url'])")

# 2. Upload the file straight to it
curl -X PUT --upload-file video.mp4 \
  -H "Content-Type: video/mp4" \
  "$UPLOAD_URL"
```

Or from the browser, straight off a file input:

```js
// 1. Ask your app for a signed upload URL
const res = await fetch("/uploads", { method: "POST" });
const { url } = await res.json();

// 2. Upload the file straight to it
await fetch(url, {
  method: "PUT",
  headers: { "Content-Type": file.type || "application/octet-stream" },
  body: file,
});
```

Point a webhook at `POST /webhooks` from Dashboard → Webhooks (use a tunnel like
ngrok in development). Requests with a valid signature get a `200`; anything else
gets a `401`.

## Notes for production

- The `/uploads` endpoint mints upload URLs — put your own authentication in
  front of it so only your users can call it.
- Uploads are created with `corsOrigin: "*"` so a browser can PUT from anywhere.
  Narrow that to your domains before shipping.
- `/webhooks` is intentionally not CSRF-protected: it's a server-to-server call
  authenticated by the HMAC signature, not a browser cookie.

The upload flow (resumable included) is documented at
https://fastpix.com/docs/upload-videos/upload-videos-from-device
