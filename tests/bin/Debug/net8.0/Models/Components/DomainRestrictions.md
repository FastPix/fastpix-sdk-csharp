# DomainRestrictions

Restrictions based on the originating domain of a request


## Fields

| Field                                                                 | Type                                                                  | Required                                                              | Description                                                           |
| --------------------------------------------------------------------- | --------------------------------------------------------------------- | --------------------------------------------------------------------- | --------------------------------------------------------------------- |
| `DefaultPolicy`                                                       | [PolicyAction](../../Models/Components/PolicyAction.md)               | :heavy_minus_sign:                                                    | Policy action type                                                    |
| `Allow`                                                               | List<*string*>                                                        | :heavy_minus_sign:                                                    | A list of domain names or patterns that are explicitly allowed access |
| `Deny`                                                                | List<*string*>                                                        | :heavy_minus_sign:                                                    | A list of domain names or patterns that are explicitly denied access  |