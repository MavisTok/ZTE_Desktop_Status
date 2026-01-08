using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ZTE.Models;
using ZTE.Services;
using ZTE.Utils;

namespace ZTE.ViewModels
{
    /// <summary>
    /// ViewModel for Extended Information Window
    /// </summary>
    public class ExtendedInfoViewModel : INotifyPropertyChanged
    {
        private readonly ExtendedInfoService _service;
        private readonly DispatcherTimer _autoRefreshTimer;

        #region WiFi Properties

        private string _wifiStatus;
        private string _wifi2gSsid;
        private string _wifi2gAuthMode;
        private string _wifi2gStatus;
        private string _wifi5gSsid;
        private string _wifi5gAuthMode;
        private string _wifi5gStatus;
        private string _lbdStatus;

        public string WifiStatus
        {
            get => _wifiStatus;
            set { _wifiStatus = value; OnPropertyChanged(); }
        }

        public string Wifi2gSsid
        {
            get => _wifi2gSsid;
            set { _wifi2gSsid = value; OnPropertyChanged(); }
        }

        public string Wifi2gAuthMode
        {
            get => _wifi2gAuthMode;
            set { _wifi2gAuthMode = value; OnPropertyChanged(); }
        }

        public string Wifi2gStatus
        {
            get => _wifi2gStatus;
            set { _wifi2gStatus = value; OnPropertyChanged(); }
        }

        public string Wifi5gSsid
        {
            get => _wifi5gSsid;
            set { _wifi5gSsid = value; OnPropertyChanged(); }
        }

        public string Wifi5gAuthMode
        {
            get => _wifi5gAuthMode;
            set { _wifi5gAuthMode = value; OnPropertyChanged(); }
        }

        public string Wifi5gStatus
        {
            get => _wifi5gStatus;
            set { _wifi5gStatus = value; OnPropertyChanged(); }
        }

        public string LbdStatus
        {
            get => _lbdStatus;
            set { _lbdStatus = value; OnPropertyChanged(); }
        }

        #endregion WiFi Properties

        #region Battery Properties

        private string _batteryPercent;
        private string _batteryStatus;
        private string _batteryTemperature;
        private string _chargerStatus;
        private string _timeToEmpty;
        private string _timeToFull;
        private string _batteryMode;

        public string BatteryPercent
        {
            get => _batteryPercent;
            set { _batteryPercent = value; OnPropertyChanged(); }
        }

        public string BatteryStatus
        {
            get => _batteryStatus;
            set { _batteryStatus = value; OnPropertyChanged(); }
        }

        public string BatteryTemperature
        {
            get => _batteryTemperature;
            set { _batteryTemperature = value; OnPropertyChanged(); }
        }

        public string ChargerStatus
        {
            get => _chargerStatus;
            set { _chargerStatus = value; OnPropertyChanged(); }
        }

        public string TimeToEmpty
        {
            get => _timeToEmpty;
            set { _timeToEmpty = value; OnPropertyChanged(); }
        }

        public string TimeToFull
        {
            get => _timeToFull;
            set { _timeToFull = value; OnPropertyChanged(); }
        }

        public string BatteryMode
        {
            get => _batteryMode;
            set { _batteryMode = value; OnPropertyChanged(); }
        }

        #endregion Battery Properties

        #region Power Mode Properties

        private string _powerMode;
        private string _powerModeModule;

        public string PowerMode
        {
            get => _powerMode;
            set { _powerMode = value; OnPropertyChanged(); }
        }

        public string PowerModeModule
        {
            get => _powerModeModule;
            set { _powerModeModule = value; OnPropertyChanged(); }
        }

        #endregion Power Mode Properties

        #region Status Properties

        private string _lastUpdate;
        private string _statusMessage;

        public string LastUpdate
        {
            get => _lastUpdate;
            set { _lastUpdate = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        #endregion Status Properties

        public RelayCommand RefreshCommand { get; }

        public ExtendedInfoViewModel(ExtendedInfoService service)
        {
            _service = service;

            // Commands
            RefreshCommand = new RelayCommand(async _ => await RefreshDataAsync());

            // Setup auto-refresh timer
            _autoRefreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _autoRefreshTimer.Tick += async (s, e) => await RefreshDataAsync();

            // Initialize
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

                var (wifi, battery, powerMode, _, _) = await _service.GetAllExtendedInfoAsync();

                // Update WiFi data
                UpdateWifiData(wifi);

                // Update Battery data
                UpdateBatteryData(battery);

                // Update Power Mode data
                UpdatePowerModeData(powerMode);

                LastUpdate = DateTime.Now.ToString("HH:mm:ss");
                StatusMessage = "Connected";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private void UpdateWifiData(WifiDetailInfo wifi)
        {
            WifiStatus = wifi.WifiOnOff == "1" ? "已开启" : "已关闭";

            Wifi2gSsid = wifi.Main2gSsid ?? "N/A";
            Wifi2gAuthMode = FormatAuthMode(wifi.Main2gAuthMode);
            Wifi2gStatus = wifi.Radio2Disabled == "0" ? "启用" : "禁用";

            Wifi5gSsid = wifi.Main5gSsid ?? "N/A";
            Wifi5gAuthMode = FormatAuthMode(wifi.Main5gAuthMode);
            Wifi5gStatus = wifi.Radio5Disabled == "0" ? "启用" : "禁用";

            LbdStatus = wifi.LbdEnable == "1" ? "已启用" : "未启用";
        }

        private void UpdateBatteryData(BatteryInfo battery)
        {
            BatteryPercent = $"{battery.BatPercent}%";

            BatteryStatus = battery.BatOnline == "1" ? "在线" : "离线";

            BatteryTemperature = $"{battery.BatTemperature}°C";

            ChargerStatus = battery.BatChargerConnect == "1"
                ? (battery.BatChargerStatus == "1" ? "充电中" : "已连接")
                : "未连接";

            // Format time
            if (int.TryParse(battery.BatTimeToEmpty, out int emptyMinutes) && emptyMinutes > 0)
            {
                TimeToEmpty = FormatTime(emptyMinutes);
            }
            else
            {
                TimeToEmpty = "N/A";
            }

            if (int.TryParse(battery.BatTimeToFull, out int fullMinutes) && fullMinutes > 0)
            {
                TimeToFull = FormatTime(fullMinutes);
            }
            else
            {
                TimeToFull = "N/A";
            }

            BatteryMode = battery.BatMode == "1" ? "正常模式" : $"模式 {battery.BatMode}";
        }

        private void UpdatePowerModeData(PowerModeInfo powerMode)
        {
            PowerMode = powerMode.Enable == "1" ? "直接供电模式" : "电池模式";
            PowerModeModule = powerMode.ModuleName ?? "N/A";
        }

        private string FormatAuthMode(string authMode)
        {
            if (string.IsNullOrEmpty(authMode)) return "N/A";

            return authMode switch
            {
                "sae-mixed" => "WPA3/WPA2 混合",
                "sae" => "WPA3",
                "psk2" => "WPA2-PSK",
                "psk" => "WPA-PSK",
                "open" => "开放",
                _ => authMode
            };
        }

        private string FormatTime(int minutes)
        {
            int hours = minutes / 60;
            int mins = minutes % 60;

            if (hours > 0)
                return $"{hours}小时{mins}分钟";
            else
                return $"{mins}分钟";
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged
    }
}