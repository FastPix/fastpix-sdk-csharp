using Fastpix;
using Fastpix.Models.Components;
 using Fastpix.Models.Requests;
using Fastpix.Utils;
using Newtonsoft.Json;

var sdk = new FastpixSDK(security: new Security()
{
    //  Username = "b9730810-c4e0-4d35-bfa3-c87ea493c433",
    //  Password = "d2662dba-1173-4e1c-8cfa-3235b012b633",
        Username = "your-access-token",
        Password = "your-secret-key",
});

var res = await sdk.Views.GetViewDetailsAsync(viewId: "your-view-id");

// handle response
Console.WriteLine(
    JsonConvert.SerializeObject(
        res.Object,
        Formatting.Indented,
        Utilities.GetDefaultJsonSerializerSettings()
    )
);