# PlaybackIdUserAgents

Restrictions based on the user agent (which is typically a string sent by browsers or bots identifying themselves).


## Fields

| Field                                                                   | Type                                                                    | Required                                                                | Description                                                             |
| ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- |
| `DefaultPolicy`                                                         | [PolicyAction](../../Models/Components/PolicyAction.md)                 | :heavy_minus_sign:                                                      | Policy action type                                                      |
| `Allow`                                                                 | List<*string*>                                                          | :heavy_minus_sign:                                                      | A list of specific user agents that are allowed to access the resource. |
| `Deny`                                                                  | List<*string*>                                                          | :heavy_minus_sign:                                                      | A list of specific user agents that are blocked.                        |