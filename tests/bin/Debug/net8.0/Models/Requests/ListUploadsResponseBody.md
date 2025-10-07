# ListUploadsResponseBody

List of video media


## Fields

| Field                                                                          | Type                                                                           | Required                                                                       | Description                                                                    | Example                                                                        |
| ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ | ------------------------------------------------------------------------------ |
| `Success`                                                                      | *bool*                                                                         | :heavy_minus_sign:                                                             | Demonstrates whether the request is successful or not.                         | true                                                                           |
| `Data`                                                                         | List<[DirectUpload](../../Models/Components/DirectUpload.md)>                  | :heavy_minus_sign:                                                             | Displays the result of the request.                                            |                                                                                |
| `Pagination`                                                                   | [Pagination](../../Models/Components/Pagination.md)                            | :heavy_minus_sign:                                                             | Pagination organizes content into pages for better readability and navigation. |                                                                                |