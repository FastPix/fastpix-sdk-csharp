# Playback

## Overview

Operations for video playback management

### Available Operations

* [Create](#create) - Create a playback ID
* [List](#list) - Get all playback IDs details for a media
* [Delete](#delete) - Delete a playback ID
* [UpdateDomainRestrictions](#updatedomainrestrictions) - Update domain restrictions for a playback ID
* [UpdateUserAgentRestrictions](#updateuseragentrestrictions) - Update user-agent restrictions for a playback ID

## Create

You can create a new playback ID for a specific media asset. If you have already retrieved an existing `playbackId` using the <a href="https://docs.fastpix.io/reference/get-media">Get Media by ID</a> endpoint for a media asset, you can use this endpoint to generate a new playback ID with a specified access policy. 



If you want to create a private playback ID for a media asset that already has a public playback ID, this endpoint also allows you to do so by specifying the desired access policy. 

#### How it works

1. Make a `POST` request to this endpoint, replacing `<mediaId>` with the `uploadId` or `id` of the media asset. 

2. Include the `accessPolicy` in the request body with `private` or `public` as the value. 

3. You receive a response containing the newly created playback ID with the specified access level.


#### Example
A video streaming service generates playback IDs for each media file when users request to view specific content. The video player then uses the playback ID to stream the video.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="create-media-playback-id" method="post" path="/on-demand/{mediaId}/playback-ids" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playback.CreateAsync(
    mediaId: "<mediaId>",
    body: new CreateMediaPlaybackIdRequestBody() {
        AccessPolicy = AccessPolicy.Public,
        DrmConfigurationId = "<drmConfigurationId>",
        Resolution = Fastpix.Models.Requests.Resolution.OneThousandAndEightyp,
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                     | Type                                                                                          | Required                                                                                      | Description                                                                                   | Example                                                                                       |
| --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                     | *string*                                                                                      | :heavy_check_mark:                                                                            | The unique identifier assigned to the media when created. The value must be a valid UUID.     | <mediaId> or <playbackId>                                                          |
| `Body`                                                                                        | [CreateMediaPlaybackIdRequestBody](../../Models/Requests/CreateMediaPlaybackIdRequestBody.md) | :heavy_minus_sign:                                                                            | Request body for creating playback id for an media                                            |                                                                                               |

### Response

**[CreateMediaPlaybackIdResponse](../../Models/Requests/CreateMediaPlaybackIdResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## List

Retrieves all playback IDs associated with a given media asset, including each playback ID’s access policy and detailed access restrictions such as allowed or denied domains and user agents.

**How it works:**
1. Send a `GET` request to this endpoint with the target `mediaId`.
2. The response includes an array of playback ID records with their respective access controls.

**Use case:**
Useful for validating and managing playback permissions programmatically, reviewing restriction settings, or powering an access control dashboard.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="list-playback-ids" method="get" path="/on-demand/{mediaId}/playback-ids" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playback.ListAsync(mediaId: "<mediaId>");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                            | Type                                 | Required                             | Description                          | Example                              |
| ------------------------------------ | ------------------------------------ | ------------------------------------ | ------------------------------------ | ------------------------------------ |
| `MediaId`                            | *string*                             | :heavy_check_mark:                   | N/A                                  | <mediaId> |

### Response

**[ListPlaybackIdsResponse](../../Models/Requests/ListPlaybackIdsResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Delete

This endpoint deletes a specific playback ID associated with a media asset. Deleting a `playback ID` revokes access to the media content linked to that ID.


#### How it works

1. Make a `DELETE` request to this endpoint, replacing `<mediaId>` with the unique ID of the media asset from which you want to delete the playback ID. 

2. Include the `playbackId` you want to delete in the request body.

#### Example

Your platform offers limited-time access to premium content. When the subscription expires, you can revoke access to the content by deleting the associated playback ID, preventing users from streaming the video further.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="delete-media-playback-id" method="delete" path="/on-demand/{mediaId}/playback-ids" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playback.DeleteAsync(
    mediaId: "<mediaId>",
    playbackId: "<playbackId>"
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                             | Type                                                                                                  | Required                                                                                              | Description                                                                                           | Example                                                                                               |
| ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                             | *string*                                                                                              | :heavy_check_mark:                                                                                    | The unique identifier assigned to the media when created. The value must be a valid UUID.             | <mediaId> or <playbackId>                                                                  |
| `PlaybackId`                                                                                          | *string*                                                                                              | :heavy_check_mark:                                                                                    | Return the universal unique identifier for playbacks  which can contain a maximum of 255 characters.  | <mediaId> or <playbackId>                                                                  |

### Response

**[DeleteMediaPlaybackIdResponse](../../Models/Requests/DeleteMediaPlaybackIdResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## UpdateDomainRestrictions

This endpoint updates domain-level restrictions for a specific playback ID associated with a media asset.
It allows you to restrict playback to specific domains or block known unauthorized domains.

**How it works:**
1. Make a `PATCH` request to this endpoint with your desired domain access configuration.
2. Set a default policy (`allow` or `deny`) and specify domain names in the `allow` or `deny` lists.
3. This is commonly used to restrict video playback to your website or approved client domains.

**Example:**
A streaming service can allow playback only from `example.com` and deny all others by setting: `"defaultPolicy": "deny"` and `"allow": ["example.com"]`.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-domain-restrictions" method="patch" path="/on-demand/{mediaId}/playback-ids/{playbackId}/domains" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playback.UpdateDomainRestrictionsAsync(
    mediaId: "<mediaId>",
    playbackId: "<playbackId>",
    body: new UpdateDomainRestrictionsRequestBody() {
        Allow = new List<string>() {
            "yourdomain.com",
            "sampledomain.com",
        },
        Deny = new List<string>() {
            "yourworkdomain.com",
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                           | Type                                                                                                | Required                                                                                            | Description                                                                                         | Example                                                                                             |
| --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                           | *string*                                                                                            | :heavy_check_mark:                                                                                  | N/A                                                                                                 | <mediaId>                                                                |
| `PlaybackId`                                                                                        | *string*                                                                                            | :heavy_check_mark:                                                                                  | N/A                                                                                                 | <playbackId>                                                                |
| `Body`                                                                                              | [UpdateDomainRestrictionsRequestBody](../../Models/Requests/UpdateDomainRestrictionsRequestBody.md) | :heavy_check_mark:                                                                                  | N/A                                                                                                 |                                                                                                     |

### Response

**[UpdateDomainRestrictionsResponse](../../Models/Requests/UpdateDomainRestrictionsResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## UpdateUserAgentRestrictions

This endpoint allows updating user-agent restrictions for a specific playback ID associated with a media asset. 
It can be used to allow or deny specific user-agents during playback request evaluation.

**How it works:**
1. Make a `PATCH` request to this endpoint with your desired user-agent access configuration.
2. Specify a default policy (`allow` or `deny`) and provide specific `allow` or `deny` lists.
3. Use this to restrict access to specific browsers, devices, or bots.

**Example:**
A developer may configure a playback ID to deny access from known scraping user-agents while allowing all others by default.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="update-user-agent-restrictions" method="patch" path="/on-demand/{mediaId}/playback-ids/{playbackId}/user-agents" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Playback.UpdateUserAgentRestrictionsAsync(
    mediaId: "<mediaId>",
    playbackId: "<playbackId>",
    body: new UpdateUserAgentRestrictionsRequestBody() {
        Allow = new List<string>() {
            "Mozilla/55.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36",
        },
        Deny = new List<string>() {
            "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/53745.36 (KHTML, like Gecko) Chrome/138.0.0.0 Mobile Safari/537.36",
        },
    }
);

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);
```

### Parameters

| Parameter                                                                                                 | Type                                                                                                      | Required                                                                                                  | Description                                                                                               | Example                                                                                                   |
| --------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------- |
| `MediaId`                                                                                                 | *string*                                                                                                  | :heavy_check_mark:                                                                                        | N/A                                                                                                       | <mediaId>                                                                      |
| `PlaybackId`                                                                                              | *string*                                                                                                  | :heavy_check_mark:                                                                                        | N/A                                                                                                       | <playbackId>                                                                      |
| `Body`                                                                                                    | [UpdateUserAgentRestrictionsRequestBody](../../Models/Requests/UpdateUserAgentRestrictionsRequestBody.md) | :heavy_check_mark:                                                                                        | N/A                                                                                                       |                                                                                                           |

### Response

**[UpdateUserAgentRestrictionsResponse](../../Models/Requests/UpdateUserAgentRestrictionsResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |