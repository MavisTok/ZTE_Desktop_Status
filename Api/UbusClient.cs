using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Text;

using System.Text.Json;

namespace ZTE.Api
{
    /// <summary>
    /// Ubus JSON-RPC client for OpenWrt-like /ubus/ endpoint.
    /// This device uses array-style request/response:
    ///   Request:  [ { jsonrpc, id, method:"call", params:[session, obj, func, args] }, ... ]
    ///   Response: [ { id, result:[0, payload] }, ... ]
    /// </summary>
    public class UbusClient
    {
        private readonly HttpClient _http;
        private int _id = 1;

        /// <summary>
        /// Ubus session token. You can replace it once you get a new one.
        /// </summary>
        public string Session { get; set; }

        public UbusClient(string baseUrl, string session, int timeoutSeconds = 3)
        {
            Session = session;
            _http = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }

        /// <summary>
        /// Generate a new JSON-RPC ID.
        /// </summary>
        public int NextId() => _id++;

        /// <summary>
        /// Create one JSON-RPC call object with current session.
        /// </summary>
        public object BuildCall(string objectPath, string function, object args = null, int? id = null)
        {
            return new
            {
                jsonrpc = "2.0",
                id = id ?? NextId(),
                method = "call",
                @params = new object[]
                {
                    Session,
                    objectPath,
                    function,
                    args ?? new { }
                }
            };
        }

        /// <summary>
        /// POST to /ubus/?t=timestamp with array-style JSON-RPC request.
        /// Returns raw response as string array (to avoid JsonDocument disposal issues).
        /// </summary>
        public async Task<string[]> PostRawAsync(IEnumerable<object> callObjects)
        {
            var json = JsonSerializer.Serialize(callObjects);
            var t = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var url = $"/ubus/?t={t}";

            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var resp = await _http.PostAsync(url, content).ConfigureAwait(false);

            resp.EnsureSuccessStatusCode();
            var respText = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            using var doc = JsonDocument.Parse(respText);

            if (doc.RootElement.ValueKind != JsonValueKind.Array)
                throw new Exception("Unexpected ubus response format (expected array).");

            // Convert each element to string to avoid JsonDocument disposal issues
            return doc.RootElement.EnumerateArray()
                .Select(e => e.GetRawText())
                .ToArray();
        }

        /// <summary>
        /// Batch call ubus and return a map: id -> payload(JsonElement).
        /// payload is result[1] when result[0]==0.
        /// </summary>
        public async Task<Dictionary<int, JsonElement>> BatchCallAsync(IEnumerable<object> callObjects)
        {
            var raw = await PostRawAsync(callObjects).ConfigureAwait(false);

            var map = new Dictionary<int, JsonElement>();
            foreach (var itemJson in raw)
            {
                using var doc = JsonDocument.Parse(itemJson);
                var item = doc.RootElement;

                var id = item.GetProperty("id").GetInt32();
                var payload = ExtractPayload(item);

                // Clone the payload to a new JsonDocument to avoid disposal issues
                var clonedPayload = JsonDocument.Parse(payload.GetRawText()).RootElement;
                map[id] = clonedPayload;
            }
            return map;
        }

        /// <summary>
        /// Single ubus call: returns payload as JsonElement.
        /// </summary>
        public async Task<JsonElement> CallAsJsonAsync(string objectPath, string function, object args = null)
        {
            var call = BuildCall(objectPath, function, args);
            var raw = await PostRawAsync(new[] { call }).ConfigureAwait(false);

            using var doc = JsonDocument.Parse(raw[0]);
            var payload = ExtractPayload(doc.RootElement);

            // Clone the payload to avoid disposal issues
            return JsonDocument.Parse(payload.GetRawText()).RootElement;
        }

        /// <summary>
        /// Single ubus call: deserialize payload into T.
        /// </summary>
        public async Task<T> CallAsync<T>(string objectPath, string function, object args = null)
        {
            var payload = await CallAsJsonAsync(objectPath, function, args).ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(payload.GetRawText());
        }

        /// <summary>
        /// Extract payload from one ubus response item:
        ///   { id:xx, result:[code, payload] }
        /// code==0 means success.
        /// </summary>
        public static JsonElement ExtractPayload(JsonElement rpcResponseItem)
        {
            if (!rpcResponseItem.TryGetProperty("result", out var result))
                throw new Exception("Invalid ubus response: missing result field.");

            // result: [code, payload]
            if (result.ValueKind == JsonValueKind.Object)
            {
                return result;
            }

            if (result.ValueKind != JsonValueKind.Array)
                throw new Exception("Invalid ubus response: result is not [code,payload].");

            if (result.GetArrayLength() < 1)
                throw new Exception("Invalid ubus response: result is not [code,payload].");

            var code = result[0].GetInt32();
            if (code != 0)
            {
                // Some devices will return a human-readable error in result[1]
                var detail = result.GetArrayLength() > 1 &&
                             (result[1].ValueKind == JsonValueKind.Object || result[1].ValueKind == JsonValueKind.String)
                    ? result[1].ToString()
                    : "";

                throw new Exception($"Ubus call failed. code={code}, detail={detail}");
            }

            if (result.GetArrayLength() < 2)
            {
                // Some calls return [0] without payload when no data exists.
                return JsonDocument.Parse("{}").RootElement;
            }

            return result[1];
        }
    }
}
