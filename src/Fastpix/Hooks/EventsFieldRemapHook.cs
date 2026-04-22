#nullable enable
namespace Fastpix.Hooks
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Rewrites abbreviated wire keys in the get_video_view_details response body
    /// so the generated deserializer sees the spec-shaped payload.
    /// </summary>
    /// <remarks>
    /// The live API emits pt/e/d/vt/et on each event (and br/h/w/cd/host/txt/c/err/t/u
    /// inside event_details) while the OpenAPI spec documents the fully spelled-out
    /// names. Without this remap, data.events[] entries deserialize as empty objects.
    /// Unknown keys pass through unchanged so new server fields aren't silently dropped.
    /// </remarks>
    public class EventsFieldRemapHook : IAfterSuccessHook
    {
        private const string TargetOperationId = "get_video_view_details";

        private static readonly Dictionary<string, string> OuterMap = new Dictionary<string, string>
        {
            ["pt"] = "player_playhead_time",
            ["e"]  = "event_name",
            ["d"]  = "event_details",
            ["vt"] = "viewer_time",
            ["et"] = "event_time",
        };

        private static readonly Dictionary<string, string> InnerMap = new Dictionary<string, string>
        {
            ["br"]   = "bitrate",
            ["h"]    = "height",
            ["w"]    = "width",
            ["cd"]   = "codec",
            ["host"] = "hostName",
            ["txt"]  = "text",
            ["c"]    = "code",
            ["err"]  = "error",
            ["t"]    = "type",
            ["u"]    = "url",
        };

        public async Task<HttpResponseMessage> AfterSuccessAsync(AfterSuccessContext hookCtx, HttpResponseMessage response)
        {
            if (hookCtx.OperationID != TargetOperationId)
            {
                return response;
            }

            int statusCode = (int)response.StatusCode;
            if (statusCode < 200 || statusCode >= 300)
            {
                return response;
            }

            if (response.Content == null)
            {
                return response;
            }

            var mediaType = response.Content.Headers.ContentType?.MediaType;
            if (!string.Equals(mediaType, "application/json", System.StringComparison.OrdinalIgnoreCase))
            {
                return response;
            }

            var body = await response.Content.ReadAsStringAsync();

            JObject root;
            try
            {
                root = JObject.Parse(body);
            }
            catch (JsonReaderException)
            {
                return response;
            }

            if (!(root["data"] is JObject data))
            {
                return response;
            }

            if (!(data["events"] is JArray events))
            {
                return response;
            }

            for (int i = 0; i < events.Count; i++)
            {
                if (events[i] is JObject evt)
                {
                    events[i] = RemapEvent(evt);
                }
            }

            var newBody = root.ToString(Formatting.None);
            response.Content = new StringContent(newBody, Encoding.UTF8, "application/json");
            return response;
        }

        private static JObject RemapEvent(JObject evt)
        {
            var rewritten = new JObject();
            foreach (var prop in evt.Properties())
            {
                var newKey = OuterMap.TryGetValue(prop.Name, out var mapped) ? mapped : prop.Name;
                if (newKey == "event_details" && prop.Value is JObject detailsObj)
                {
                    rewritten[newKey] = RemapDetails(detailsObj);
                }
                else
                {
                    rewritten[newKey] = prop.Value;
                }
            }
            return rewritten;
        }

        private static JObject RemapDetails(JObject details)
        {
            var rewritten = new JObject();
            foreach (var prop in details.Properties())
            {
                var newKey = InnerMap.TryGetValue(prop.Name, out var mapped) ? mapped : prop.Name;
                rewritten[newKey] = prop.Value;
            }
            return rewritten;
        }
    }
}
