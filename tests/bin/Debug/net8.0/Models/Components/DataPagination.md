# DataPagination

Pagination organizes content into pages for better readability and navigation.


## Fields

| Field                                                        | Type                                                         | Required                                                     | Description                                                  | Example                                                      |
| ------------------------------------------------------------ | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------------------------------------------------------ |
| `TotalRecords`                                               | *long*                                                       | :heavy_minus_sign:                                           | The total number of records retrieved within the timeframe.<br/> | 2                                                            |
| `CurrentOffset`                                              | *long*                                                       | :heavy_minus_sign:                                           | The current offset value. <br/><br/>Default: 1<br/>          | 1                                                            |
| `OffsetCount`                                                | *long*                                                       | :heavy_minus_sign:                                           | The total number of offsets based on limit.<br/>             | 1                                                            |