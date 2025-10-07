# ViewNotFoundException


## Fields

| Field                                                             | Type                                                              | Required                                                          | Description                                                       | Example                                                           |
| ----------------------------------------------------------------- | ----------------------------------------------------------------- | ----------------------------------------------------------------- | ----------------------------------------------------------------- | ----------------------------------------------------------------- |
| `Success`                                                         | *bool*                                                            | :heavy_minus_sign:                                                | It demonstrates whether the request is successful or not.         | false                                                             |
| `Error`                                                           | [ViewNotFoundError](../../Models/Components/ViewNotFoundError.md) | :heavy_minus_sign:                                                | Returns the problem that has occured                              |                                                                   |
| `HttpMeta`                                                        | [HTTPMetadata](../../Models/Components/HTTPMetadata.md)           | :heavy_check_mark:                                                | N/A                                                               |                                                                   |