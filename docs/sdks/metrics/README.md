# Metrics

## Overview

Operations involving metrics

### Available Operations

* [ListBreakdownValues](#listbreakdownvalues) - List breakdown values
* [ListOverallValues](#listoverallvalues) - List overall values
* [GetTimeseriesData](#gettimeseriesdata) - Get timeseries data
* [Compare](#compare) - List comparison values

## ListBreakdownValues

Retrieves breakdown values for a specified metric and timespan, allowing you to analyze the performance of your content based on various dimensions. It provides insights into how different factors contribute to the overall metrics. 

#### How it works

  1. Before using this endpoint, you can call the <a href="https://docs.fastpix.io/reference/list_dimensions">List Dimensions</a> endpoint to retrieve all available dimensions that can be used in your query. 

  2. Send a `GET` request to this endpoint with the required `metricId` and other query parameters. 

  3. You receive a response containing the breakdown values for the specified metric, grouped and filtered according to your parameters. 

  4. Upon successful retrieval, the response includes the breakdown values based on the specified parameters. Note that the time values ( `totalWatchTime` and `totalPlayingTime` ) are in milliseconds 


#### Example


A developer wants to analyze how watch time varies across different device types. By calling this endpoint for the `playing_time` metric and filtering by `device_type`, they can understand how engagement differs between mobile, desktop, and tablet users. This data guides optimization efforts for different platforms.

#### Key fields in response


  * **views:** The count of views based based on the applied filters.

  * **value:** The specific metric value calculated based on the applied filters. 
  * **totalWatchTime:** Total time watched across all views, represented in milliseconds. 

  * **totalPlayTime:** Total time spent playing the video, represented in milliseconds. 
  * **field:** The grouping field value based on the groupBy parameter. 


Related guide: <a href="https://docs.fastpix.io/docs/metrics-overview">Understand data definitions</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="list_breakdown_values" method="get" path="/data/metrics/{metricId}/breakdown" -->
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

ListBreakdownValuesRequest req = new ListBreakdownValuesRequest() {
    MetricId = ListBreakdownValuesMetricId.QualityOfExperienceScore,
    Timespan = ListBreakdownValuesTimespan.TwentyFourhours,
    Filterby = "browser_name:Chrome",
    GroupBy = "browser_name",
};

var res = await sdk.Metrics.ListBreakdownValuesAsync(req);

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

| Parameter                                                                         | Type                                                                              | Required                                                                          | Description                                                                       |
| --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| `request`                                                                         | [ListBreakdownValuesRequest](../../Models/Requests/ListBreakdownValuesRequest.md) | :heavy_check_mark:                                                                | The request object to use for the request.                                        |

### Response

**[ListBreakdownValuesResponse](../../Models/Requests/ListBreakdownValuesResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## ListOverallValues

Retrieves overall values for a specified metric, providing summary statistics that help you understand the performance of your content. The response includes key metrics such as `totalWatchTime`, `uniqueViews`, `totalPlayTime` and `totalViews`. 

#### How it works

  1. Before using this endpoint, you can call the <a href="https://docs.fastpix.io/reference/list_dimensions">list dimensions</a> endpoint to retrieve all available dimensions that can be used in your query. 

  2. Send a `GET` request to this endpoint with the required `metricId` and other query parameters. 

  3. You receive a response containing the overall values for the specified metric, which may vary based on the applied filters. 






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
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security() {
    Username = "your-access-token",
    Password = "your-secret-key",
});

var res = await sdk.Metrics.ListOverallValuesAsync(
    metricId: ListOverallValuesMetricId.QualityOfExperienceScore,
    measurement: "avg",
    timespan: ListOverallValuesTimespan.TwentyFourhours,
    filterby: "browser_name:Chrome"
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

| Parameter                                                                                                                                                                                                                                                                                                                                                                                                                        | Type                                                                                                                                                                                                                                                                                                                                                                                                                             | Required                                                                                                                                                                                                                                                                                                                                                                                                                         | Description                                                                                                                                                                                                                                                                                                                                                                                                                      | Example                                                                                                                                                                                                                                                                                                                                                                                                                          |
| -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `MetricId`                                                                                                                                                                                                                                                                                                                                                                                                                       | [ListOverallValuesMetricId](../../Models/Requests/ListOverallValuesMetricId.md)                                                                                                                                                                                                                                                                                                                                                  | :heavy_check_mark:                                                                                                                                                                                                                                                                                                                                                                                                               | Pass metric Id<br/>                                                                                                                                                                                                                                                                                                                                                                                                              | quality_of_experience_score                                                                                                                                                                                                                                                                                                                                                                                                      |
| `Measurement`                                                                                                                                                                                                                                                                                                                                                                                                                    | *string*                                                                                                                                                                                                                                                                                                                                                                                                                         | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                                                                                                                               | The measurement for the given metrics.<br/>Possible Values : [95th, median, avg, count or sum]<br/>                                                                                                                                                                                                                                                                                                                              | avg                                                                                                                                                                                                                                                                                                                                                                                                                              |
| `Timespan`                                                                                                                                                                                                                                                                                                                                                                                                                       | [ListOverallValuesTimespan](../../Models/Requests/ListOverallValuesTimespan.md)                                                                                                                                                                                                                                                                                                                                                  | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                                                                                                                               | This parameter specifies the time span between which the video views list must be retrieved by. You can provide either from and to unix epoch timestamps or time duration. The scope of duration is between 60 minutes to 30 days.<br/><br/>**Accepted formats are:**<br/><br/>array of epoch timestamps for example <br/>`timespan[]=1498867200&timespan[]=1498953600`<br/><br/>duration string for example  <br/>`timespan[]=24:hours` or `timespan[]=7:days`<br/> | 24:hours                                                                                                                                                                                                                                                                                                                                                                                                                         |
| `Filterby`                                                                                                                                                                                                                                                                                                                                                                                                                       | *string*                                                                                                                                                                                                                                                                                                                                                                                                                         | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                                                                                                                               | Pass the dimensions and their corresponding values you want to filter the views by. For excluding the values in the filter we can pass "!" before the filter value. The list of filters can be obtained from list of dimensions endpoint.<br/>Example Values : [ browser_name:Chrome , os_name:macOS , !device_name:Galaxy ]<br/>                                                                                                | browser_name:Chrome                                                                                                                                                                                                                                                                                                                                                                                                              |

### Response

**[ListOverallValuesResponse](../../Models/Requests/ListOverallValuesResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## GetTimeseriesData

This endpoint retrieves timeseries data for a specified metric, providing insights into how the metric values change over time. The response includes an array of data points, each representing the metrics value at specific intervals. 

#### Key fields in response

* **intervalTime:** The timestamp for the data point indicating when the metric value was recorded. 
* **metricValue:** The value of the specified metric at the given interval, reflecting the performance or engagement level during that time. 
* **numberOfViews:** The total number of views recorded during that interval, providing context for the metric value.


### Example Usage

<!-- UsageSnippet language="csharp" operationID="get_timeseries_data" method="get" path="/data/metrics/{metricId}/timeseries" -->
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

GetTimeseriesDataRequest req = new GetTimeseriesDataRequest() {
    MetricId = GetTimeseriesDataMetricId.QualityOfExperienceScore,
    Timespan = GetTimeseriesDataTimespan.TwentyFourhours,
    Filterby = "browser_name:Chrome",
};

var res = await sdk.Metrics.GetTimeseriesDataAsync(req);

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

| Parameter                                                                     | Type                                                                          | Required                                                                      | Description                                                                   |
| ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- |
| `request`                                                                     | [GetTimeseriesDataRequest](../../Models/Requests/GetTimeseriesDataRequest.md) | :heavy_check_mark:                                                            | The request object to use for the request.                                    |

### Response

**[GetTimeseriesDataResponse](../../Models/Requests/GetTimeseriesDataResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |

## Compare

This endpoint lets you to compare multiple metrics across specified dimensions. You can specify the metrics you want to compare in the query parameters, and the response includes the relevant metrics for the specified dimensions.

#### Key fields in response 

* **value:** The specific metric value calculated based on the applied filters.
* **type:** The data unit or format type (for example, "number", "milliseconds", "percentage").
* **name:** The display name of the metric (for example, "Views", "Overall Score").
* **metric:** The metric field represents the name of the Key Performance Indicator (KPI) being tracked or analyzed. It identifies a specific measurable aspect of the video playback experience, such as buffering time, video start failure rate, or playback quality.
* **items:** Nested breakdown of related metrics for more detailed analysis.
* **measurement:** Defines the aggregation type (for example, "avg", "sum", "median", "95th").

#### How it works 

  1. Before making a request to this endpoint, call the <a href="https://docs.fastpix.io/reference/list_dimensions">list dimensions</a> endpoint to obtain all available dimensions that can be used for comparison. 

  2. Send a `GET` request to this endpoint with the desired metrics specified in the query parameters. 

  3. You Receive a response containing the comparison values for the specified metrics across the selected dimensions. 


  Related guide: <a href="https://docs.fastpix.io/docs/understand-dashboard-ui#compare-metrics">Compare metrics in dashboard</a>


### Example Usage

<!-- UsageSnippet language="csharp" operationID="list_comparison_values" method="get" path="/data/metrics/comparison" -->
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

var res = await sdk.Metrics.CompareAsync(
    timespan: ListComparisonValuesTimespan.TwentyFourhours,
    filterby: "browser_name:Chrome",
    dimension: Dimension.BrowserName,
    valueP: "Chrome"
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

| Parameter                                                                                                                                                                                                                                                                                                                                                                                                                       | Type                                                                                                                                                                                                                                                                                                                                                                                                                            | Required                                                                                                                                                                                                                                                                                                                                                                                                                        | Description                                                                                                                                                                                                                                                                                                                                                                                                                     | Example                                                                                                                                                                                                                                                                                                                                                                                                                         |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Timespan`                                                                                                                                                                                                                                                                                                                                                                                                                      | [ListComparisonValuesTimespan](../../Models/Requests/ListComparisonValuesTimespan.md)                                                                                                                                                                                                                                                                                                                                           | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                                                                                                                              | This parameter specifies the time span between which the video views list must be retrieved by. You can provide either from and to unix epoch timestamps or time duration. The scope of duration is between 60 minutes to 30 days.<br/><br/>**Accepted formats are:**<br/><br/>array of epoch timestamps for example <br/>`timespan[]=1498867200&timespan[]=1498953600`<br/><br/>duration string for example <br/>`timespan[]=24:hours` or `timespan[]=7:days`<br/> | 24:hours                                                                                                                                                                                                                                                                                                                                                                                                                        |
| `Filterby`                                                                                                                                                                                                                                                                                                                                                                                                                      | *string*                                                                                                                                                                                                                                                                                                                                                                                                                        | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                                                                                                                              | Pass the dimensions and their corresponding values you want to filter the views by. For excluding the values in the filter we can pass "!" before the filter value. The list of filters can be obtained from list of dimensions endpoint.<br/>Example Values : [ browser_name:Chrome , os_name:macOS , !device_name:Galaxy ]<br/>                                                                                               | browser_name:Chrome                                                                                                                                                                                                                                                                                                                                                                                                             |
| `Dimension`                                                                                                                                                                                                                                                                                                                                                                                                                     | [Dimension](../../Models/Requests/Dimension.md)                                                                                                                                                                                                                                                                                                                                                                                 | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                                                                                                                              | The dimension id in which the views are watched.<br/>                                                                                                                                                                                                                                                                                                                                                                           | browser_name                                                                                                                                                                                                                                                                                                                                                                                                                    |
| `Value`                                                                                                                                                                                                                                                                                                                                                                                                                         | *string*                                                                                                                                                                                                                                                                                                                                                                                                                        | :heavy_minus_sign:                                                                                                                                                                                                                                                                                                                                                                                                              | The value for the selected dimension. <br/>For example:<br/> If `dimension` is `browser_name`, the value could be  `Chrome` `,` `Firefox` `etc` .<br/> If `dimension` is `os_name`, the value could be `macOS` `,` `Windows` `etc` .<br/>                                                                                                                                                                                       | Chrome                                                                                                                                                                                                                                                                                                                                                                                                                          |

### Response

**[ListComparisonValuesResponse](../../Models/Requests/ListComparisonValuesResponse.md)**

### Errors

| Error Type                         | Status Code                        | Content Type                       |
| ---------------------------------- | ---------------------------------- | ---------------------------------- |
| Fastpix.Models.Errors.APIException | 4XX, 5XX                           | \*/\*                              |