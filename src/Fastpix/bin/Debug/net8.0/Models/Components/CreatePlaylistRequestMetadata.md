# CreatePlaylistRequestMetadata

Required when playlist type is smart - media created between startDate and endDate of createdDate will be add, similarily updatedDate (Optional)


## Fields

| Field                                             | Type                                              | Required                                          | Description                                       |
| ------------------------------------------------- | ------------------------------------------------- | ------------------------------------------------- | ------------------------------------------------- |
| `CreatedDate`                                     | [DateRange](../../Models/Components/DateRange.md) | :heavy_minus_sign:                                | Date range with start and end dates.              |
| `UpdatedDate`                                     | [DateRange](../../Models/Components/DateRange.md) | :heavy_minus_sign:                                | Date range with start and end dates.              |