using Fastpix.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FastPix.Examples;

// Pretty-prints an SDK response the same way the docs snippets do.
internal static class Json
{
    public static void Print(object? value)
    {
        Console.WriteLine(
            JToken.Parse(
                JsonConvert.SerializeObject(value, Utilities.GetDefaultJsonSerializerSettings()))
            .ToString(Formatting.Indented));
    }
}
