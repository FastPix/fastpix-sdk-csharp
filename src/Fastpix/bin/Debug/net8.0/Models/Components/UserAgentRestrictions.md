# UserAgentRestrictions

Restrictions based on the user agent


## Fields

| Field                                                    | Type                                                     | Required                                                 | Description                                              |
| -------------------------------------------------------- | -------------------------------------------------------- | -------------------------------------------------------- | -------------------------------------------------------- |
| `DefaultPolicy`                                          | [PolicyAction](../../Models/Components/PolicyAction.md)  | :heavy_minus_sign:                                       | Policy action type                                       |
| `Allow`                                                  | List<*string*>                                           | :heavy_minus_sign:                                       | A list of user agents that are explicitly allowed access |
| `Deny`                                                   | List<*string*>                                           | :heavy_minus_sign:                                       | A list of user agents that are explicitly denied access  |