using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using ZTE.Models;

namespace ZTE.Utils
{
    /// <summary>
    /// Parser for HAR (HTTP Archive) files to extract JSON-RPC data
    /// </summary>
    public class HarParser
    {
        /// <summary>
        /// Parse HAR file and extract all JSON-RPC requests and responses
        /// </summary>
        /// <param name="harFilePath">Path to the HAR file</param>
        /// <returns>List of JSON-RPC data entries</returns>
        public static List<JsonRpcData> ParseJsonRpcData(string harFilePath)
        {
            var jsonRpcDataList = new List<JsonRpcData>();

            try
            {
                string harContent = File.ReadAllText(harFilePath);

                // Parse the HAR file as JSON
                using (JsonDocument document = JsonDocument.Parse(harContent))
                {
                    var root = document.RootElement;

                    // Navigate to entries array: log.entries
                    if (root.TryGetProperty("log", out JsonElement log) &&
                        log.TryGetProperty("entries", out JsonElement entries))
                    {
                        foreach (JsonElement entry in entries.EnumerateArray())
                        {
                            var jsonRpcData = ExtractJsonRpcFromEntry(entry);
                            if (jsonRpcData != null)
                            {
                                jsonRpcDataList.Add(jsonRpcData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse HAR file: {ex.Message}", ex);
            }

            return jsonRpcDataList;
        }

        /// <summary>
        /// Extract JSON-RPC data from a single HAR entry
        /// </summary>
        private static JsonRpcData ExtractJsonRpcFromEntry(JsonElement entry)
        {
            try
            {
                // Check if request contains JSON-RPC data
                if (!entry.TryGetProperty("request", out JsonElement request) ||
                    !request.TryGetProperty("postData", out JsonElement postData) ||
                    !postData.TryGetProperty("text", out JsonElement requestText))
                {
                    return null;
                }

                string requestJsonText = requestText.GetString();

                // Check if it contains "jsonrpc"
                if (!requestJsonText.Contains("jsonrpc"))
                {
                    return null;
                }

                // Extract response
                string responseJsonText = null;
                if (entry.TryGetProperty("response", out JsonElement response) &&
                    response.TryGetProperty("content", out JsonElement content) &&
                    content.TryGetProperty("text", out JsonElement responseText))
                {
                    responseJsonText = responseText.GetString();
                }

                // Parse request JSON to extract details
                var jsonRpcData = ParseJsonRpcDetails(requestJsonText, responseJsonText);

                // Extract timestamp
                if (entry.TryGetProperty("startedDateTime", out JsonElement startedDateTime))
                {
                    if (DateTime.TryParse(startedDateTime.GetString(), out DateTime timestamp))
                    {
                        jsonRpcData.Timestamp = timestamp;
                    }
                }

                // Extract URL
                if (request.TryGetProperty("url", out JsonElement url))
                {
                    jsonRpcData.Url = url.GetString();
                }

                return jsonRpcData;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Parse JSON-RPC text to extract method, id, and parameters
        /// </summary>
        private static JsonRpcData ParseJsonRpcDetails(string requestJson, string responseJson)
        {
            var data = new JsonRpcData
            {
                RequestJson = requestJson,
                ResponseJson = responseJson ?? "No response"
            };

            try
            {
                // The request is wrapped in an array, so we need to parse it
                using (JsonDocument doc = JsonDocument.Parse(requestJson))
                {
                    // Get the first element of the array
                    JsonElement firstElement = doc.RootElement.EnumerateArray().FirstOrDefault();

                    if (firstElement.ValueKind != JsonValueKind.Undefined)
                    {
                        // Extract ID
                        if (firstElement.TryGetProperty("id", out JsonElement idElement))
                        {
                            data.Id = idElement.GetInt32();
                        }

                        // Extract method
                        if (firstElement.TryGetProperty("method", out JsonElement methodElement))
                        {
                            data.Method = methodElement.GetString();
                        }

                        // Extract params array
                        if (firstElement.TryGetProperty("params", out JsonElement paramsElement) &&
                            paramsElement.ValueKind == JsonValueKind.Array)
                        {
                            var paramsArray = paramsElement.EnumerateArray().ToArray();

                            // params[1] is the API namespace
                            if (paramsArray.Length > 1)
                            {
                                data.ApiNamespace = paramsArray[1].GetString();
                            }

                            // params[2] is the API function
                            if (paramsArray.Length > 2)
                            {
                                data.ApiFunction = paramsArray[2].GetString();
                            }
                        }
                    }
                }
            }
            catch
            {
                // If parsing fails, leave the fields empty
            }

            return data;
        }
    }
}
