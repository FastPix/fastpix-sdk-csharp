# PlaybackIdDomains

Restrictions based on the originating domain of a request (e.g., whether requests from certain websites should be allowed or blocked).


## Fields

| Field                                                                      | Type                                                                       | Required                                                                   | Description                                                                |
| -------------------------------------------------------------------------- | -------------------------------------------------------------------------- | -------------------------------------------------------------------------- | -------------------------------------------------------------------------- |
| `DefaultPolicy`                                                            | [PolicyAction](../../Models/Components/PolicyAction.md)                    | :heavy_minus_sign:                                                         | Policy action type                                                         |
| `Allow`                                                                    | List<*string*>                                                             | :heavy_minus_sign:                                                         | A list of domains that are explicitly allowed access.                      |
| `Deny`                                                                     | List<*string*>                                                             | :heavy_minus_sign:                                                         | A list of domains that are explicitly blocked from accessing the resource. |