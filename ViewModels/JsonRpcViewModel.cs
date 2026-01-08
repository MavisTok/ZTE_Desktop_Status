using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ZTE.Models;
using ZTE.Utils;

namespace ZTE.ViewModels
{
    /// <summary>
    /// ViewModel for JSON-RPC data viewer window
    /// </summary>
    public class JsonRpcViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<JsonRpcData> _jsonRpcDataList;
        private JsonRpcData _selectedJsonRpcData;
        private string _formattedRequest;
        private string _formattedResponse;

        public JsonRpcViewModel()
        {
            JsonRpcDataList = new ObservableCollection<JsonRpcData>();
        }

        /// <summary>
        /// List of all JSON-RPC data entries
        /// </summary>
        public ObservableCollection<JsonRpcData> JsonRpcDataList
        {
            get => _jsonRpcDataList;
            set
            {
                _jsonRpcDataList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Currently selected JSON-RPC data
        /// </summary>
        public JsonRpcData SelectedJsonRpcData
        {
            get => _selectedJsonRpcData;
            set
            {
                _selectedJsonRpcData = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsJsonRpcSelected));
                OnPropertyChanged(nameof(IsJsonRpcNotSelected));

                // Update formatted request and response
                UpdateFormattedData();
            }
        }

        /// <summary>
        /// Formatted request JSON for display
        /// </summary>
        public string FormattedRequest
        {
            get => _formattedRequest;
            set
            {
                _formattedRequest = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Formatted response JSON for display
        /// </summary>
        public string FormattedResponse
        {
            get => _formattedResponse;
            set
            {
                _formattedResponse = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Whether a JSON-RPC item is selected
        /// </summary>
        public bool IsJsonRpcSelected => SelectedJsonRpcData != null;

        /// <summary>
        /// Whether no JSON-RPC item is selected
        /// </summary>
        public bool IsJsonRpcNotSelected => SelectedJsonRpcData == null;

        /// <summary>
        /// Load HAR file and parse JSON-RPC data
        /// </summary>
        public void LoadHarFile(string harFilePath)
        {
            try
            {
                var dataList = HarParser.ParseJsonRpcData(harFilePath);

                JsonRpcDataList.Clear();
                foreach (var data in dataList)
                {
                    JsonRpcDataList.Add(data);
                }

                // Auto-select first item if available
                if (JsonRpcDataList.Count > 0)
                {
                    SelectedJsonRpcData = JsonRpcDataList[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load HAR file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Update formatted request and response when selection changes
        /// </summary>
        private void UpdateFormattedData()
        {
            if (SelectedJsonRpcData == null)
            {
                FormattedRequest = string.Empty;
                FormattedResponse = string.Empty;
                return;
            }

            // Format request JSON
            FormattedRequest = FormatJson(SelectedJsonRpcData.RequestJson);

            // Format response JSON
            FormattedResponse = FormatJson(SelectedJsonRpcData.ResponseJson);
        }

        /// <summary>
        /// Format JSON string with indentation
        /// </summary>
        private string FormatJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return "No data";
            }

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                }
            }
            catch
            {
                // If JSON parsing fails, return original text
                return json;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
