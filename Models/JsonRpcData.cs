using System;

namespace ZTE.Models
{
    /// <summary>
    /// Represents a JSON-RPC request and response pair from HAR file
    /// </summary>
    public class JsonRpcData
    {
        /// <summary>
        /// Request ID from JSON-RPC
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// JSON-RPC method being called
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// API namespace (first parameter in params array)
        /// </summary>
        public string ApiNamespace { get; set; }

        /// <summary>
        /// API function name (second parameter in params array)
        /// </summary>
        public string ApiFunction { get; set; }

        /// <summary>
        /// Full request JSON text
        /// </summary>
        public string RequestJson { get; set; }

        /// <summary>
        /// Full response JSON text
        /// </summary>
        public string ResponseJson { get; set; }

        /// <summary>
        /// Timestamp of the request
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// URL of the request
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Display summary for UI binding
        /// </summary>
        public string DisplaySummary => $"[{Id}] {ApiNamespace}.{ApiFunction}";
    }
}
