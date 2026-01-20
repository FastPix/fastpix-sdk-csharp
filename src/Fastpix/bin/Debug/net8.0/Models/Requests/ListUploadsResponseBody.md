# ListUploadsResponseBody

List of video media


## Fields

| Field                                                                          | Type                                                                           | Required                                                                       | Description                                                                    | Example                                                                        |
| ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ |
| `Success`                                                                      | *bool*                                                                         | :heavy_minus_sign:                                                             | Shows the request status. Returns true for success and false for failure.      | true                                                                           |
| `Data`                                                                         | List<[UnusedDirectUpload](../../Models/Components/UnusedDirectUpload.md)>      | :heavy_minus_sign:                                                             | Displays the result of the request.                                            |                                                                                |
| `Pagination`                                                                   | [Pagination](../../Models/Components/Pagination.md)                            | :heavy_minus_sign:                                                             | Pagination organizes content into pages for better readability and navigation. |                                                                                |