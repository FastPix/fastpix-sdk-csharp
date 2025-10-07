# Metrics
(*Metrics*)

## Overview

### Available Operations

* [ListBreakdown](#listbreakdown) - List breakdown values
* [ListOverall](#listoverall) - List overall values
* [GetTimeseries](#gettimeseries) - Get timeseries data
* [ListComparisonValues](#listcomparisonvalues) - List comparison values

## ListBreakdown

Retrieves breakdown values for a specified metric and timespan, allowing you to analyze the performance of your content based on various dimensions. It provides insights into how different factors contribute to the overall metrics. 

#### How it works

  1. Before using this endpoint, you can call the <a href="https://docs.fastpix.io/reference/list_dimensions">List Dimensions</a> endpoint to retrieve all available dimensions that can be used in your query. 

  2. Make a `GET` request to this endpoint with the required `metricId` and other query parameters. 

  3. Receive a response containing the breakdown values for the specified metric, grouped and filtered according to your parameters. 

  4. Upon successful retrieval, the response will include the breakdown values based on the specified parameters. Note that the time values ( `totalWatchTime` and `totalPlayingTime` ) are in milliseconds. 


#### Example


A developer wants to analyze how watch time varies across different device types. By calling this endpoint for the `playing_time` metric and filtering by `device_type`, they can understand how engagement differs between mobile, desktop, and tablet users. This data will guide optimization efforts for different platforms. 


Related guide: <a href="https://docs.fastpix.io/docs/metrics-overview">Understand data definitions</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="list_breakdown_values" method="get" path="/data/metrics/{metricId}/breakdown" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

ListBreakdownValuesRequest req = new ListBreakdownValuesRequest() {
    MetricId = ListBreakdownValuesMetricId.QualityOfExperienceScore,
    Timespan = ListBreakdownValuesTimespan.Sevendays,
    Filterby = "browser_name:Chrome",
    GroupBy = "browser_name",
};

var res = await sdk.Metrics.ListBreakdownAsync(req);

// handle response
```

### Parameters

| Parameter                                                                         | Type                                                                              | Required                                                                          | Description                                                                       |
| --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| `request`                                                                         | [ListBreakdownValuesRequest](../../Models/Requests/ListBreakdownValuesRequest.md) | :heavy_check_mark:                                                                | The request object to use for the request.                                        |

### Response

**[ListBreakdownValuesResponse](../../Models/Requests/ListBreakdownValuesResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ViewNotFoundException      | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## ListOverall

Retrieves overall values for a specified metric, providing summary statistics that help you understand the performance of your content. The response includes key metrics such as `totalWatchTime`, `uniqueViews`, `totalPlayTime` and `totalViews`. 

#### How it works

  1. Before using this endpoint, you can call the <a href="https://docs.fastpix.io/reference/list_dimensions">list dimensions</a> endpoint to retrieve all available dimensions that can be used in your query. 

  2. Make a `GET` request to this endpoint with the required `metricId` and other query parameters. 

  3. Receive a response containing the overall values for the specified metric, which may vary based on the applied filters. 






#### Key fields in response


  * **value:** The specific metric value calculated based on the applied filters. 
  * **totalWatchTime:** Total time watched across all views, represented in milliseconds. 
  * **uniqueViews:** The count of unique viewers who interacted with the content. 
  * **totalViews:** The total number of views recorded. 
  * **totalPlayTime:** Total time spent playing the video, represented in milliseconds. 
  * **globalValue:** A global metric value that reflects the overall performance of the specified metric across the entire dataset for the given timespan. This value is not affected by specific filters. 


  Related guide: <a href="https://docs.fastpix.io/docs/metrics-overview">Understand data definitions</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="list_overall_values" method="get" path="/data/metrics/{metricId}/overall" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.Metrics.ListOverallAsync(
    metricId: ListOverallValuesMetricId.QualityOfExperienceScore,
    timespan: ListOverallValuesTimespan.Sevendays,
    measurement: "avg",
    filterby: "browser_name:Chrome"
);

// handle response
```

### Parameters

| Parameter                                                                                                                                                                                                                                                                                                                | Type                                                                                                                                                                                                                                                                                                                     | Required                                                                                                                                                                                                                                                                                                                 | Description                                                                                                                                                                                                                                                                                                              | Example                                                                                                                                                                                                                                                                                                                  |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `MetricId`                                                                                                                                                                                                                                                                                                               | [ListOverallValuesMetricId](../../Models/Requests/ListOverallValuesMetricId.md)                                                                                                                                                                                                                                          | :heavy_check_mark:                                                                                                                                                                                                                                                                                                       | Pass metric Id<br/>                                                                                                                                                                                                                                                                                                      | quality_of_experience_score                                                                                                                                                                                                                                                                                              |
| `Timespan`                                                                                                                                                                                                                                                                                                               | [ListOverallValuesTimespan](../../Models/Requests/ListOverallValuesTimespan.md)                                                                                                                                                                                                                                          | :heavy_check_mark:                                                                                                                                                                                                                                                                                                       | This parameter specifies the time span between which the video views list should be retrieved by. You can provide either from and to unix epoch timestamps or time duration. The scope of duration is between 60 minutes to 30 days.<br/>                                                                                | 7:days                                                                                                                                                                                                                                                                                                                   |
| `Measurement`                                                                                                                                                                                                                                                                                                            | *string*                                                                                                                                                                                                                                                                                                                 | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                       | The measurement for the given metrics.<br/>Possible Values : [95th, median, avg, count or sum]<br/>                                                                                                                                                                                                                      | avg                                                                                                                                                                                                                                                                                                                      |
| `Filterby`                                                                                                                                                                                                                                                                                                               | *string*                                                                                                                                                                                                                                                                                                                 | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                       | Pass the dimensions and their corresponding values you want to filter the views by. For excluding the values in the filter we can pass '!' before the filter value. The list of filters can be obtained from list of dimensions endpoint.<br/>Example Values : [ browser_name:Chrome , os_name:macOS , device_name:Galaxy ]<br/> | browser_name:Chrome                                                                                                                                                                                                                                                                                                      |

### Response

**[ListOverallValuesResponse](../../Models/Requests/ListOverallValuesResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ViewNotFoundException      | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## GetTimeseries

This endpoint retrieves timeseries data for a specified metric, providing insights into how the metric values change over time. The response includes an array of data points, each representing the metric's value at specific intervals. 

Each data point contains the following fields: 

* **intervalTime:** The timestamp for the data point indicating when the metric value was recorded. 
* **metricValue:** The value of the specified metric at the given interval, reflecting the performance or engagement level during that time. 
* **numberOfViews:** The total number of views recorded during that interval, providing context for the metric value. 


### Example Usage

<!-- UsageSnippet language="csharp" operationID="get_timeseries_data" method="get" path="/data/metrics/{metricId}/timeseries" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

GetTimeseriesDataRequest req = new GetTimeseriesDataRequest() {
    MetricId = GetTimeseriesDataMetricId.QualityOfExperienceScore,
    Timespan = GetTimeseriesDataTimespan.Sevendays,
    Filterby = "browser_name:Chrome",
};

var res = await sdk.Metrics.GetTimeseriesAsync(req);

// handle response
```

### Parameters

| Parameter                                                                     | Type                                                                          | Required                                                                      | Description                                                                   |
| ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- |
| `request`                                                                     | [GetTimeseriesDataRequest](../../Models/Requests/GetTimeseriesDataRequest.md) | :heavy_check_mark:                                                            | The request object to use for the request.                                    |

### Response

**[GetTimeseriesDataResponse](../../Models/Requests/GetTimeseriesDataResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ViewNotFoundException      | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |

## ListComparisonValues

This endpoint allows you to compare multiple metrics across specified dimensions. You can specify the metrics you want to compare in the query parameters, and the response will include the relevant metrics for the specified dimensions. 

#### How it works 

  1. Before making a request to this endpoint, call the <a href="https://docs.fastpix.io/reference/list_dimensions">list dimensions</a> endpoint to obtain all available dimensions that can be used for comparison. 

  2. Make a `GET` request to this endpoint with the desired metrics specified in the query parameters. 

  3. Receive a response containing the comparison values for the specified metrics across the selected dimensions. 


  Related guide: <a href="https://docs.fastpix.io/docs/understand-dashboard-ui#compare-metrics">Compare metrics in dashboard</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="list_comparison_values" method="get" path="/data/metrics/comparison" -->
```csharp
using Fastpix;
using Fastpix.Models.Components;
using Fastpix.Models.Requests;

var sdk = new FastPix(security: new Security() {
    Username = "your-access-token",
    Password = "secret-key",
});

var res = await sdk.Metrics.ListComparisonValuesAsync(
    timespan: ListComparisonValuesTimespan.Sevendays,
    filterby: "browser_name:Chrome",
    dimension: ListComparisonValuesDimension.BrowserName,
    valueP: "Chrome"
);

// handle response
```

### Parameters

| Parameter                                                                                                                                                                                                                                                                                                                | Type                                                                                                                                                                                                                                                                                                                     | Required                                                                                                                                                                                                                                                                                                                 | Description                                                                                                                                                                                                                                                                                                              | Example                                                                                                                                                                                                                                                                                                                  |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Timespan`                                                                                                                                                                                                                                                                                                               | [ListComparisonValuesTimespan](../../Models/Requests/ListComparisonValuesTimespan.md)                                                                                                                                                                                                                                    | :heavy_check_mark:                                                                                                                                                                                                                                                                                                       | This parameter specifies the time span between which the video views list should be retrieved by. You can provide either from and to unix epoch timestamps or time duration. The scope of duration is between 60 minutes to 30 days.<br/>                                                                                | 7:days                                                                                                                                                                                                                                                                                                                   |
| `Filterby`                                                                                                                                                                                                                                                                                                               | *string*                                                                                                                                                                                                                                                                                                                 | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                       | Pass the dimensions and their corresponding values you want to filter the views by. For excluding the values in the filter we can pass '!' before the filter value. The list of filters can be obtained from list of dimensions endpoint.<br/>Example Values : [ browser_name:Chrome , os_name:macOS , device_name:Galaxy ]<br/> | browser_name:Chrome                                                                                                                                                                                                                                                                                                      |
| `Dimension`                                                                                                                                                                                                                                                                                                              | [ListComparisonValuesDimension](../../Models/Requests/ListComparisonValuesDimension.md)                                                                                                                                                                                                                                  | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                       | The dimension id in which the views are watched.<br/>                                                                                                                                                                                                                                                                    | browser_name                                                                                                                                                                                                                                                                                                             |
| `Value`                                                                                                                                                                                                                                                                                                                  | *string*                                                                                                                                                                                                                                                                                                                 | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                       | The value for the selected dimension. <br/>For example:<br/> If `dimension` is `browser_name`, the value could be  `Chrome` `,` `Firefox` `etc` .<br/> If `dimension` is `os_name`, the value could be `macOS` `,` `Windows` `etc` .<br/>                                                                                | Chrome                                                                                                                                                                                                                                                                                                                   |

### Response

**[ListComparisonValuesResponse](../../Models/Requests/ListComparisonValuesResponse.md)**

### Errors

| Error Type                                       | Status Code                                      | Content Type                                     |
| ------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------ |
| Fastpix.Models.Errors.InvalidPermissionException | 401                                              | application/json                                 |
| Fastpix.Models.Errors.ViewNotFoundException      | 404                                              | application/json                                 |
| Fastpix.Models.Errors.ValidationErrorResponse    | 422                                              | application/json                                 |
| Fastpix.Models.Errors.APIException               | 4XX, 5XX                                         | \*/\*                                            |