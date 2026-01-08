using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Media;
using ZTE.Models;
using ZTE.Services;
using ZTE.Utils;

namespace ZTE.ViewModels
{
    /// <summary>
    /// Extended ViewModel with Cell and Device info
    /// </summary>
    public class ExtendedInfoViewModel2 : INotifyPropertyChanged
    {
        private readonly ExtendedInfoService _service;
        private readonly DispatcherTimer _autoRefreshTimer;

        #region WiFi Properties (省略,与原来相同)
        private string _wifiStatus;
        private string _wifi2gSsid;
        private string _wifi2gAuthMode;
        private string _wifi2gStatus;
        private string _wifi5gSsid;
        private string _wifi5gAuthMode;
        private string _wifi5gStatus;
        private string _lbdStatus;

        public string WifiStatus { get => _wifiStatus; set { _wifiStatus = value; OnPropertyChanged(); } }
        public string Wifi2gSsid { get => _wifi2gSsid; set { _wifi2gSsid = value; OnPropertyChanged(); } }
        public string Wifi2gAuthMode { get => _wifi2gAuthMode; set { _wifi2gAuthMode = value; OnPropertyChanged(); } }
        public string Wifi2gStatus { get => _wifi2gStatus; set { _wifi2gStatus = value; OnPropertyChanged(); } }
        public string Wifi5gSsid { get => _wifi5gSsid; set { _wifi5gSsid = value; OnPropertyChanged(); } }
        public string Wifi5gAuthMode { get => _wifi5gAuthMode; set { _wifi5gAuthMode = value; OnPropertyChanged(); } }
        public string Wifi5gStatus { get => _wifi5gStatus; set { _wifi5gStatus = value; OnPropertyChanged(); } }
        public string LbdStatus { get => _lbdStatus; set { _lbdStatus = value; OnPropertyChanged(); } }
        #endregion

        #region Battery Properties (省略,与原来相同)
        private string _batteryPercent;
        private string _batteryStatus;
        private string _batteryTemperature;
        private string _chargerStatus;
        private string _timeToEmpty;
        private string _timeToFull;
        private string _batteryMode;

        public string BatteryPercent { get => _batteryPercent; set { _batteryPercent = value; OnPropertyChanged(); } }
        public string BatteryStatus { get => _batteryStatus; set { _batteryStatus = value; OnPropertyChanged(); } }
        public string BatteryTemperature { get => _batteryTemperature; set { _batteryTemperature = value; OnPropertyChanged(); } }
        public string ChargerStatus { get => _chargerStatus; set { _chargerStatus = value; OnPropertyChanged(); } }
        public string TimeToEmpty { get => _timeToEmpty; set { _timeToEmpty = value; OnPropertyChanged(); } }
        public string TimeToFull { get => _timeToFull; set { _timeToFull = value; OnPropertyChanged(); } }
        public string BatteryMode { get => _batteryMode; set { _batteryMode = value; OnPropertyChanged(); } }
        #endregion

        #region Power Mode Properties (省略,与原来相同)
        private string _powerMode;
        private string _powerModeModule;

        public string PowerMode { get => _powerMode; set { _powerMode = value; OnPropertyChanged(); } }
        public string PowerModeModule { get => _powerModeModule; set { _powerModeModule = value; OnPropertyChanged(); } }
        #endregion

        #region 5G Cell Properties
        private string _networkType;
        private string _activeBand;
        private string _nr5gRsrp;
        private string _nr5gRsrq;
        private string _nr5gSnr;
        private string _nr5gPci;
        private string _nr5gBand;
        private string _nr5gBandwidth;
        private string _nr5gCellId;
        private string _lteRsrp;
        private string _lteRsrq;
        private string _ltePci;
        private string _lteBand;
        private string _caStatus;

        public string NetworkType { get => _networkType; set { _networkType = value; OnPropertyChanged(); } }
        public string ActiveBand { get => _activeBand; set { _activeBand = value; OnPropertyChanged(); } }
        public string Nr5gRsrp { get => _nr5gRsrp; set { _nr5gRsrp = value; OnPropertyChanged(); } }
        public string Nr5gRsrq { get => _nr5gRsrq; set { _nr5gRsrq = value; OnPropertyChanged(); } }
        public string Nr5gSnr { get => _nr5gSnr; set { _nr5gSnr = value; OnPropertyChanged(); } }
        public string Nr5gPci { get => _nr5gPci; set { _nr5gPci = value; OnPropertyChanged(); } }
        public string Nr5gBand { get => _nr5gBand; set { _nr5gBand = value; OnPropertyChanged(); } }
        public string Nr5gBandwidth { get => _nr5gBandwidth; set { _nr5gBandwidth = value; OnPropertyChanged(); } }
        public string Nr5gCellId { get => _nr5gCellId; set { _nr5gCellId = value; OnPropertyChanged(); } }
        public string LteRsrp { get => _lteRsrp; set { _lteRsrp = value; OnPropertyChanged(); } }
        public string LteRsrq { get => _lteRsrq; set { _lteRsrq = value; OnPropertyChanged(); } }
        public string LtePci { get => _ltePci; set { _ltePci = value; OnPropertyChanged(); } }
        public string LteBand { get => _lteBand; set { _lteBand = value; OnPropertyChanged(); } }
        public string CaStatus { get => _caStatus; set { _caStatus = value; OnPropertyChanged(); } }
        #endregion

        #region Signal Level Properties
        private int _nr5gRsrpLevel;
        private int _nr5gRsrqLevel;
        private int _nr5gSnrLevel;
        private int _lteRsrpLevel;
        private int _lteRsrqLevel;

        private Brush _nr5gRsrpBrush = Brushes.Gray;
        private Brush _nr5gRsrqBrush = Brushes.Gray;
        private Brush _nr5gSnrBrush = Brushes.Gray;
        private Brush _lteRsrpBrush = Brushes.Gray;
        private Brush _lteRsrqBrush = Brushes.Gray;

        public int Nr5gRsrpLevel { get => _nr5gRsrpLevel; set { _nr5gRsrpLevel = value; OnPropertyChanged(); } }
        public int Nr5gRsrqLevel { get => _nr5gRsrqLevel; set { _nr5gRsrqLevel = value; OnPropertyChanged(); } }
        public int Nr5gSnrLevel { get => _nr5gSnrLevel; set { _nr5gSnrLevel = value; OnPropertyChanged(); } }
        public int LteRsrpLevel { get => _lteRsrpLevel; set { _lteRsrpLevel = value; OnPropertyChanged(); } }
        public int LteRsrqLevel { get => _lteRsrqLevel; set { _lteRsrqLevel = value; OnPropertyChanged(); } }

        public Brush Nr5gRsrpBrush { get => _nr5gRsrpBrush; set { _nr5gRsrpBrush = value; OnPropertyChanged(); } }
        public Brush Nr5gRsrqBrush { get => _nr5gRsrqBrush; set { _nr5gRsrqBrush = value; OnPropertyChanged(); } }
        public Brush Nr5gSnrBrush { get => _nr5gSnrBrush; set { _nr5gSnrBrush = value; OnPropertyChanged(); } }
        public Brush LteRsrpBrush { get => _lteRsrpBrush; set { _lteRsrpBrush = value; OnPropertyChanged(); } }
        public Brush LteRsrqBrush { get => _lteRsrqBrush; set { _lteRsrqBrush = value; OnPropertyChanged(); } }
        #endregion

        #region Device Hardware Properties
        private string _deviceName;
        private string _hardwareVersion;
        private string _softwareVersion;
        private string _imei;
        private string _macAddress;
        private string _cpuUsage;
        private string _memoryUsage;
        private string _storageUsage;
        private string _deviceTemp;
        private string _uptime;

        public string DeviceName { get => _deviceName; set { _deviceName = value; OnPropertyChanged(); } }
        public string HardwareVersion { get => _hardwareVersion; set { _hardwareVersion = value; OnPropertyChanged(); } }
        public string SoftwareVersion { get => _softwareVersion; set { _softwareVersion = value; OnPropertyChanged(); } }
        public string Imei { get => _imei; set { _imei = value; OnPropertyChanged(); } }
        public string MacAddress { get => _macAddress; set { _macAddress = value; OnPropertyChanged(); } }
        public string CpuUsage { get => _cpuUsage; set { _cpuUsage = value; OnPropertyChanged(); } }
        public string MemoryUsage { get => _memoryUsage; set { _memoryUsage = value; OnPropertyChanged(); } }
        public string StorageUsage { get => _storageUsage; set { _storageUsage = value; OnPropertyChanged(); } }
        public string DeviceTemp { get => _deviceTemp; set { _deviceTemp = value; OnPropertyChanged(); } }
        public string Uptime { get => _uptime; set { _uptime = value; OnPropertyChanged(); } }
        #endregion

        #region Status Properties
        private string _lastUpdate;
        private string _statusMessage;

        public string LastUpdate { get => _lastUpdate; set { _lastUpdate = value; OnPropertyChanged(); } }
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }
        #endregion

        public RelayCommand RefreshCommand { get; }

        public ExtendedInfoViewModel2(ExtendedInfoService service)
        {
            _service = service;
            RefreshCommand = new RelayCommand(async _ => await RefreshDataAsync());

            _autoRefreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _autoRefreshTimer.Tick += async (s, e) => await RefreshDataAsync();

            StatusMessage = "Initializing...";
        }

        public void StartAutoRefresh()
        {
            _autoRefreshTimer.Start();
            Task.Run(async () => await RefreshDataAsync());
        }

        public void StopAutoRefresh()
        {
            _autoRefreshTimer.Stop();
        }

        private async Task RefreshDataAsync()
        {
            try
            {
                StatusMessage = "Loading...";

                var (wifi, battery, powerMode, cell, device) = await _service.GetAllExtendedInfoAsync();

                // Update WiFi
                WifiStatus = wifi.WifiOnOff == "1" ? "已开启" : "已关闭";
                Wifi2gSsid = wifi.Main2gSsid ?? "N/A";
                Wifi2gAuthMode = FormatAuthMode(wifi.Main2gAuthMode);
                Wifi2gStatus = wifi.Radio2Disabled == "0" ? "启用" : "禁用";
                Wifi5gSsid = wifi.Main5gSsid ?? "N/A";
                Wifi5gAuthMode = FormatAuthMode(wifi.Main5gAuthMode);
                Wifi5gStatus = wifi.Radio5Disabled == "0" ? "启用" : "禁用";
                LbdStatus = wifi.LbdEnable == "1" ? "已启用" : "未启用";

                // Update Battery
                BatteryPercent = $"{battery.BatPercent}%";
                BatteryStatus = battery.BatOnline == "1" ? "在线" : "离线";
                BatteryTemperature = $"{battery.BatTemperature}°C";
                ChargerStatus = battery.BatChargerConnect == "1" ? (battery.BatChargerStatus == "1" ? "充电中" : "已连接") : "未连接";
                TimeToEmpty = FormatMinutes(battery.BatTimeToEmpty);
                TimeToFull = FormatMinutes(battery.BatTimeToFull);
                BatteryMode = battery.BatMode == "1" ? "正常模式" : $"模式 {battery.BatMode}";

                // Update Power Mode
                PowerMode = powerMode.Enable == "1" ? "直接供电模式" : "电池模式";
                PowerModeModule = powerMode.ModuleName ?? "N/A";

                // Update Cell Info
                NetworkType = cell.network_type ?? "N/A";
                ActiveBand = cell.wan_active_band ?? "N/A";
                var nr5gRsrpValue = ParseSignalValue(cell.nr5g_rsrp);
                var nr5gRsrqValue = ParseSignalValue(cell.nr5g_rsrq);
                var nr5gSnrValue = ParseSignalValue(cell.nr5g_snr);

                Nr5gRsrp = double.IsNaN(nr5gRsrpValue) ? "N/A" : $"{cell.nr5g_rsrp} dBm";
                Nr5gRsrq = double.IsNaN(nr5gRsrqValue) ? "N/A" : $"{cell.nr5g_rsrq} dB";
                Nr5gSnr = double.IsNaN(nr5gSnrValue) ? "N/A" : $"{cell.nr5g_snr} dB";
                Nr5gPci = cell.nr5g_pci ?? "N/A";
                Nr5gBand = cell.nr5g_band ?? "N/A";
                Nr5gBandwidth = string.IsNullOrEmpty(cell.nr5g_bandwidth) ? "N/A" : $"{cell.nr5g_bandwidth} MHz";
                Nr5gCellId = cell.nr5g_cell_id ?? "N/A";
                var lteRsrpValue = ParseSignalValue(cell.lte_rsrp);
                var lteRsrqValue = ParseSignalValue(cell.lte_rsrq);

                LteRsrp = double.IsNaN(lteRsrpValue) ? "N/A" : $"{cell.lte_rsrp} dBm";
                LteRsrq = double.IsNaN(lteRsrqValue) ? "N/A" : $"{cell.lte_rsrq} dB";
                LtePci = cell.lte_pci ?? "N/A";
                // LTE频段：如果太长则只显示前面部分
                LteBand = FormatLteBand(cell.lte_band);
                CaStatus = cell.ca_status ?? "N/A";

                Nr5gRsrpLevel = MapToPercent(nr5gRsrpValue, -120, -80);
                Nr5gRsrqLevel = MapToPercent(nr5gRsrqValue, -20, -3);
                Nr5gSnrLevel = MapToPercent(nr5gSnrValue, -10, 30);
                LteRsrpLevel = MapToPercent(lteRsrpValue, -120, -80);
                LteRsrqLevel = MapToPercent(lteRsrqValue, -20, -3);

                Nr5gRsrpBrush = GetRsrpBrush(nr5gRsrpValue);
                Nr5gRsrqBrush = GetRsrqBrush(nr5gRsrqValue);
                Nr5gSnrBrush = GetSnrBrush(nr5gSnrValue);
                LteRsrpBrush = GetRsrpBrush(lteRsrpValue);
                LteRsrqBrush = GetRsrqBrush(lteRsrqValue);

                // Update Device Info
                DeviceName = string.IsNullOrWhiteSpace(device.device_name) ? "ZTE 5G CPE" : device.device_name;
                HardwareVersion = string.IsNullOrWhiteSpace(device.hardware_version) ? "N/A" : device.hardware_version;
                SoftwareVersion = string.IsNullOrWhiteSpace(device.software_version) ? "N/A" : device.software_version;
                Imei = string.IsNullOrWhiteSpace(device.imei) ? "N/A" : device.imei;
                MacAddress = string.IsNullOrWhiteSpace(device.mac_address) ? "N/A" : device.mac_address;
                CpuUsage = string.IsNullOrEmpty(device.cpu_usage) ? "N/A" : $"{device.cpu_usage}%";
                MemoryUsage = string.IsNullOrEmpty(device.memory_usage_percent) ? "N/A" : $"{device.memory_usage_percent}%";
                StorageUsage = string.IsNullOrEmpty(device.storage_usage_percent) ? "N/A" : $"{device.storage_usage_percent}%";
                DeviceTemp = string.IsNullOrEmpty(device.device_temperature) ? "N/A" : $"{device.device_temperature}°C";
                Uptime = device.uptime ?? "N/A";

                LastUpdate = DateTime.Now.ToString("HH:mm:ss");
                StatusMessage = "Connected";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private string FormatAuthMode(string authMode)
        {
            if (string.IsNullOrEmpty(authMode)) return "N/A";
            return authMode switch
            {
                "sae-mixed" => "WPA3/WPA2",
                "sae" => "WPA3",
                "psk2" => "WPA2",
                "psk" => "WPA",
                "open" => "开放",
                _ => authMode
            };
        }

        private string FormatMinutes(string minutes)
        {
            if (!int.TryParse(minutes, out int mins) || mins <= 0) return "N/A";
            int hours = mins / 60;
            int m = mins % 60;
            return hours > 0 ? $"{hours}h{m}m" : $"{m}m";
        }

        private string FormatLteBand(string lteBand)
        {
            if (string.IsNullOrEmpty(lteBand)) return "N/A";
            // 将逗号替换为逗号+空格，便于换行
            return lteBand.Replace(",", ", ");
        }

        private static double ParseSignalValue(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw) || raw == "0") return double.NaN;
            return double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                ? value
                : double.NaN;
        }

        private static int MapToPercent(double value, double min, double max)
        {
            if (double.IsNaN(value)) return 0;
            if (min >= max) return 0;
            var clamped = Math.Max(min, Math.Min(max, value));
            return (int)Math.Round((clamped - min) / (max - min) * 100);
        }

        private static Brush GetRsrpBrush(double value)
        {
            if (double.IsNaN(value)) return Brushes.Gray;
            if (value >= -80) return Brushes.Green;
            if (value >= -90) return Brushes.YellowGreen;
            if (value >= -100) return Brushes.Gold;
            if (value >= -110) return Brushes.Orange;
            return Brushes.Red;
        }

        private static Brush GetRsrqBrush(double value)
        {
            if (double.IsNaN(value)) return Brushes.Gray;
            if (value >= -10) return Brushes.Green;
            if (value >= -15) return Brushes.YellowGreen;
            if (value >= -20) return Brushes.Orange;
            return Brushes.Red;
        }

        private static Brush GetSnrBrush(double value)
        {
            if (double.IsNaN(value)) return Brushes.Gray;
            if (value >= 20) return Brushes.Green;
            if (value >= 13) return Brushes.YellowGreen;
            if (value >= 0) return Brushes.Gold;
            return Brushes.Red;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
