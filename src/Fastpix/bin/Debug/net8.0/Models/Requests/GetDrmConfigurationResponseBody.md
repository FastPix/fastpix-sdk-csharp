# GetDrmConfigurationResponseBody

DRM configuration(s) retrieved successfully


## Fields

| Field                                                                          | Type                                                                           | Required                                                                       | Description                                                                    | Example                                                                        |
| ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ |
| `Success`                                                                      | *bool*                                                                         | :heavy_minus_sign:                                                             | Shows the request status. Returns true for success and false for failure.      | true                                                                           |
| `Data`                                                                         | List<[DrmIdResponse](../../Models/Components/DrmIdResponse.md)>                | :heavy_minus_sign:                                                             | N/A                                                                            |                                                                                |
| `Pagination`                                                                   | [Pagination](../../Models/Components/Pagination.md)                            | :heavy_minus_sign:                                                             | Pagination organizes content into pages for better readability and navigation. |                                                                                |