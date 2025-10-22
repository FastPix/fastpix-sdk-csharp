# Views
(*Views*)

## Overview

### Available Operations

* [List](#list) - List video views
* [GetDetails](#getdetails) - Get details of video view
* [ListTopContent](#listtopcontent) - List by top content
* [GetTimeseries](#gettimeseries) - Get concurrent viewers timeseries
* [GetConcurrentViewersBreakdown](#getconcurrentviewersbreakdown) - Get concurrent viewers breakdown by dimension

## List

Retrieves a list of video views that fall within the specified filters and have been completed within a defined timespan. It allows you to analyse viewer interactions with your video content effectively. 


#### How it works

  1. Make a `GET` request to this endpoint with the desired query parameters. 

  2. Specify the timespan for which you want to retrieve the video views using the `timespan[]` parameter. 

  3. Filter the views based on dimensions such as browser, device, video title, viewer ID, etc., using the `filterby[]` parameter. Get the dimensions by calling <a href="https://docs.fastpix.io/reference/list_dimensions">list the dimensions</a> endpoint. 

  4. Paginate the results using the `limit` and `offset` parameters. 

  5. Optionally, filter by `viewerId`, `errorCode`, `orderBy` a specific field, and sort in ascending or descending order. 

  6. Receive a response containing the list of video views matching the specified criteria.

Each view in the response will include a unique `viewId`. You can use this `viewId` to call the <a href="https://docs.fastpix.io/reference/get_video_view_details">get details of video view</a> endpoint to retrieve more detailed information about that specific view. 


#### Example

Suppose you're managing a video streaming service and need to analyze how the content performs across different devices and browsers. By calling the List Video Views endpoint with filters like `browser_name` and `device_type`, you can get insights into which platforms are most popular with the audience. This data will help you optimize content for the most-used platforms and troubleshoot any playback issues on less common devices.


  Related guide: <a href="https://docs.fastpix.io/docs/audience-metrics">Audience metrics</a>, <a href="https://docs.fastpix.io/docs/understand-dashboard-ui#1-views-dashboard">Views dashboard</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="list_video_views" method="get" path="/data/viewlist" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

ListVideoViewsRequest req = new ListVideoViewsRequest() {
    Timespan = ListVideoViewsTimespan.Sevendays,
    Filterby = "browser_name:Chrome",
    ViewerId = "09a78f7d-02ee-44f5-aa39-1b268ed2c270",
    ErrorCode = ErrorCode.CreateInteger(
        1002
    ),
};

var res = await sdk.Views.ListAsync(req);
Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter                                                               | Type                                                                    | Required                                                                | Description                                                             |
| ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- |
| `request`                                                               | [ListVideoViewsRequest](../../Models/Requests/ListVideoViewsRequest.md) | :heavy_check_mark:                                                      | The request object to use for the request.                              |

### Response

**[ListVideoViewsResponse](../../Models/Requests/ListVideoViewsResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.BadRequestException        | 400                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## GetDetails

Allows you to retrieve detailed information about a specific video view using its unique `viewId`. This is useful for getting insights into individual viewer interactions with your video content. This detailed information is valuable for enhancing user experience and improving engagement with your video assets. 

Make a `GET` request to this endpoint and you will receive a response containing detailed information about the specified video view, including metrics and attributes related to that view. 


#### Example

Suppose a developer receives a report of a poor viewing experience for a specific user. By using this endpoint with the user's `viewId`, the developer can retrieve metrics like buffering duration, playback errors, and session length. This data allows the developer to pinpoint issues (such as poor connectivity or a browser-specific problem) and take steps to improve the user experience.


Related guide: <a href="https://docs.fastpix.io/page/what-video-data-do-we-capture#/">What Video Data do we capture?</a>

### Example Usage

<!-- UsageSnippet language="csharp" operationID="get_video_view_details" method="get" path="/data/viewlist/{viewId}" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.Views.GetDetailsAsync(viewId: "<id>");
Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter          | Type               | Required           | Description        |
| ------------------ | ------------------ | ------------------ | ------------------ |
| `ViewId`           | *string*           | :heavy_check_mark: | Pass View id       |

### Response

**[GetVideoViewDetailsResponse](../../Models/Requests/GetVideoViewDetailsResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ViewNotFoundException      | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## ListTopContent

Retrieves a list of the top video views that fall within the specified filters and have been completed within a defined timespan. It allows you to identify the most popular content based on viewer interactions. 

#### How it works

  1. Make a `GET` request to this endpoint with the desired query parameters. 

  2. Specify the timespan for which you want to retrieve the top content using the `timespan[]` parameter. 

  3. Filter the views based on dimensions such as browser, device, video title, etc., using the `filterby[]` parameter. 

  4. `Limit` the results to control the number of top views returned. 

  5. Receive a response containing the list of top video views matching the specified criteria.


  Related guide: <a href="https://docs.fastpix.io/page/how-to-get-top-performing-content">Get top-performing content</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="list_by_top_content" method="get" path="/data/viewlist/top-content" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.Views.ListTopContentAsync(
    timespan: ListByTopContentTimespan.Sevendays,
    filterby: "browser_name:Chrome",
    limit: 10
);

Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter                                                                                                                                                                                                                                                                                                                | Type                                                                                                                                                                                                                                                                                                                     | Required                                                                                                                                                                                                                                                                                                                 | Description                                                                                                                                                                                                                                                                                                              | Example                                                                                                                                                                                                                                                                                                                  |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Timespan`                                                                                                                                                                                                                                                                                                               | [ListByTopContentTimespan](../../Models/Requests/ListByTopContentTimespan.md)                                                                                                                                                                                                                                            | :heavy_check_mark:                                                                                                                                                                                                                                                                                                       | This parameter specifies the time span between which the video views list should be retrieved by. You can provide either from and to unix epoch timestamps or time duration. The scope of duration is between 60 minutes to 30 days.<br/>                                                                                | 7:days                                                                                                                                                                                                                                                                                                                   |
| `Filterby`                                                                                                                                                                                                                                                                                                               | *string*                                                                                                                                                                                                                                                                                                                 | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                       | Pass the dimensions and their corresponding values you want to filter the views by. For excluding the values in the filter we can pass '!' before the filter value. The list of filters can be obtained from list of dimensions endpoint.<br/>Example Values : [ browser_name:Chrome , os_name:macOS , device_name:Galaxy ]<br/> | browser_name:Chrome                                                                                                                                                                                                                                                                                                      |
| `Limit`                                                                                                                                                                                                                                                                                                                  | *long*                                                                                                                                                                                                                                                                                                                   | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                       | Pass the limit to display only the rows specified by the value.<br/>                                                                                                                                                                                                                                                     | 10                                                                                                                                                                                                                                                                                                                       |

### Response

**[ListByTopContentResponse](../../Models/Requests/ListByTopContentResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.BadRequestException        | 400                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## GetTimeseries

Retrieves a time series of the number of concurrent viewers, providing a real-time snapshot of audience activity over the last 30 minutes. This endpoint is essential for monitoring live events, gauging audience reaction to new content releases, or understanding immediate engagement trends.

#### How it works

  1. Make a simple `GET` request to this endpoint. No query parameters are needed. 

  2. The API automatically gathers data for the **last 30 minutes**, calculating the number of concurrent viewers at regular intervals within that window.

  3. Receive a response containing a `data` array, where each object represents a specific point in time.

  4. Each object in the array includes the `intervalTime` (the timestamp of the measurement) and `numberOfViews` (the count of concurrent viewers at that instant), allowing you to easily plot viewer activity over time.

#### Example

Imagine you are streaming a major live event, such as a product launch, a sports game, or a webinar. You need to monitor audience engagement in real-time to see if viewership is increasing, decreasing, or holding steady.

By calling this endpoint periodically (e.g., every minute), you can plot a live graph of your viewership. This allows you to identify peak moments of interest, see the immediate impact of social media promotions, or detect potential technical issues if there's a sudden, unexpected drop in viewers


### Example Usage

<!-- UsageSnippet language="csharp" operationID="get_/data/viewlist/current-views/getTimeseriesViews" method="get" path="/data/viewlist/current-views/getTimeseriesViews" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.Views.GetTimeseriesAsync();
Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Response

**[GetDataViewlistCurrentViewsGetTimeseriesViewsResponse](../../Models/Requests/GetDataViewlistCurrentViewsGetTimeseriesViewsResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.BadRequestException        | 400                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## GetConcurrentViewersBreakdown

Retrieves a real-time breakdown of present concurrent viewers, grouped by a chosen dimension. This endpoint allows you to see how your audience is distributed across different categories like geography, content, or technology, based on activity in the last 30 minutes. 

For example, you can see how many viewers are currently watching from the US vs. India, or which video titles are most popular right now.

#### How it works

1. Make a `GET` request to this endpoint.

2. Specify the `dimension` you want to group by in the query parameters (e.g., `dimension=country` or `dimension=video_title`). This is the most important parameter as it defines how the data is categorized.

3. Optionally, use the `limit` parameter to control the number of results returned (e.g., `limit=5` to get the top 5 countries).

4. The API analyzes viewer data from the last 30 minutes and aggregates the viewer counts for each unique value within the chosen dimension.

5. Receive a response containing a `data` array, where each object represents a specific group (e.g., a country or a video title) and its corresponding number of `concurrent_viewers`.

#### Example

Imagine you are running a global streaming platform and have just launched a new original series. You want to see, in real-time, which regions are engaging most with the new content versus your older library content.

By calling this endpoint with `dimension=video_title`, you can immediately see a list of your most-watched videos right now and their respective viewer counts. Then, by calling it again with `dimension=country`, you can get a live breakdown of your audience's geographic distribution. This helps you confirm if your marketing efforts in specific countries are paying off instantly and allows you to make data-driven decisions during live events.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="get_/data/viewlist/current-views/filter" method="get" path="/data/viewlist/current-views/filter" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Newtonsoft.Json;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.Views.GetConcurrentViewersBreakdownAsync(
    dimension: GetDataViewlistCurrentViewsFilterDimension.Country,
    limit: 10
);

Console.WriteLine(JsonConvert.SerializeObject(res.Object, Formatting.Indented) ?? "null");
```

### Parameters

| Parameter                                                                                                                                                                                                                                | Type                                                                                                                                                                                                                                     | Required                                                                                                                                                                                                                                 | Description                                                                                                                                                                                                                              | Example                                                                                                                                                                                                                                  |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Dimension`                                                                                                                                                                                                                              | [GetDataViewlistCurrentViewsFilterDimension](../../Models/Requests/GetDataViewlistCurrentViewsFilterDimension.md)                                                                                                                        | :heavy_minus_sign:                                                                                                                                                                                                                       | The dimension to group and breakdown the concurrent viewers data by.<br/>This determines how the results will be categorized and aggregated.<br/>Choose from geographic, content, technical, or behavioral dimensions.<br/>              | country                                                                                                                                                                                                                                  |
| `Limit`                                                                                                                                                                                                                                  | *long*                                                                                                                                                                                                                                   | :heavy_minus_sign:                                                                                                                                                                                                                       | Maximum number of results to return. Controls the number of dimension values<br/>that will be included in the response. Useful for pagination and performance.<br/>Higher limits provide more detailed breakdowns but may impact response time.<br/> | 10                                                                                                                                                                                                                                       |

### Response

**[GetDataViewlistCurrentViewsFilterResponse](../../Models/Requests/GetDataViewlistCurrentViewsFilterResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.BadRequestException        | 400                                              | application/json                                 |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |