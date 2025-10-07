# ListErrorsData

Displays the result of the request.


## Fields

| Field                                                                                                              | Type                                                                                                               | Required                                                                                                           | Description                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ |
| `Errors`                                                                                                           | List<[ErrorDetails](../../Models/Components/ErrorDetails.md)>                                                      | :heavy_minus_sign:                                                                                                 | Retrieves a list of errors that have occurred in the system.                                                       |
| `TopErrors`                                                                                                        | List<[TopErrorDetails](../../Models/Components/TopErrorDetails.md)>                                                | :heavy_minus_sign:                                                                                                 | Retrieves a list of errors that have occurred most frequently in the system, ranked by their count of occurrences. |