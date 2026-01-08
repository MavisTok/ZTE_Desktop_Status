using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using ZTE.Services;
using ZTE.Utils;

namespace ZTE.ViewModels
{
    /// <summary>
    /// ViewModel for the main dashboard
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private readonly HomeDashboardService _dashboardService;
        private readonly DispatcherTimer _refreshTimer;

        #region Properties

        // Network Status
        private string _connectStatus;
        public string ConnectStatus
        {
            get => _connectStatus;
            set => SetProperty(ref _connectStatus, value);
        }

        private string _ipv4Address;
        public string Ipv4Address
        {
            get => _ipv4Address;
            set => SetProperty(ref _ipv4Address, value);
        }

        private string _ipv6Address;
        public string Ipv6Address
        {
            get => _ipv6Address;
            set => SetProperty(ref _ipv6Address, value);
        }

        // Real-time Speed
        private string _uploadSpeed;
        public string UploadSpeed
        {
            get => _uploadSpeed;
            set => SetProperty(ref _uploadSpeed, value);
        }

        private string _downloadSpeed;
        public string DownloadSpeed
        {
            get => _downloadSpeed;
            set => SetProperty(ref _downloadSpeed, value);
        }

        // Traffic Statistics
        private string _dayTraffic;
        public string DayTraffic
        {
            get => _dayTraffic;
            set => SetProperty(ref _dayTraffic, value);
        }

        private string _monthTraffic;
        public string MonthTraffic
        {
            get => _monthTraffic;
            set => SetProperty(ref _monthTraffic, value);
        }

        private string _totalTraffic;
        public string TotalTraffic
        {
            get => _totalTraffic;
            set => SetProperty(ref _totalTraffic, value);
        }

        // SIM Info
        private string _operator;
        public string Operator
        {
            get => _operator;
            set => SetProperty(ref _operator, value);
        }

        private string _simStatus;
        public string SimStatus
        {
            get => _simStatus;
            set => SetProperty(ref _simStatus, value);
        }

        private string _iccid;
        public string Iccid
        {
            get => _iccid;
            set => SetProperty(ref _iccid, value);
        }

        private string _macAddress;
        public string MacAddress
        {
            get => _macAddress;
            set => SetProperty(ref _macAddress, value);
        }

        // Signal Info
        private string _networkType;
        public string NetworkType
        {
            get => _networkType;
            set => SetProperty(ref _networkType, value);
        }

        private string _signalBar;
        public string SignalBar
        {
            get => _signalBar;
            set => SetProperty(ref _signalBar, value);
        }

        private string _activeBand;
        public string ActiveBand
        {
            get => _activeBand;
            set => SetProperty(ref _activeBand, value);
        }

        private string _rsrp;
        public string Rsrp
        {
            get => _rsrp;
            set => SetProperty(ref _rsrp, value);
        }

        private string _rsrq;
        public string Rsrq
        {
            get => _rsrq;
            set => SetProperty(ref _rsrq, value);
        }

        private string _snr;
        public string Snr
        {
            get => _snr;
            set => SetProperty(ref _snr, value);
        }

        // Device Count
        private int _totalDevices;
        public int TotalDevices
        {
            get => _totalDevices;
            set => SetProperty(ref _totalDevices, value);
        }

        private int _wirelessDevices;
        public int WirelessDevices
        {
            get => _wirelessDevices;
            set => SetProperty(ref _wirelessDevices, value);
        }

        private int _wiredDevices;
        public int WiredDevices
        {
            get => _wiredDevices;
            set => SetProperty(ref _wiredDevices, value);
        }

        private int _offlineDevices;
        public int OfflineDevices
        {
            get => _offlineDevices;
            set => SetProperty(ref _offlineDevices, value);
        }

        // Last Update
        private string _lastUpdate;
        public string LastUpdate
        {
            get => _lastUpdate;
            set => SetProperty(ref _lastUpdate, value);
        }

        // Status
        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        #endregion

        public DashboardViewModel(HomeDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

            // Initialize default values
            StatusMessage = "Initializing...";

            // Setup auto-refresh timer (configurable interval)
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(AppSettings.RefreshIntervalSeconds)
            };
            _refreshTimer.Tick += async (s, e) => await RefreshDataAsync();
        }

        /// <summary>
        /// Start auto-refresh
        /// </summary>
        public void StartAutoRefresh()
        {
            _refreshTimer.Start();
            // Immediate first refresh
            Task.Run(async () => await RefreshDataAsync());
        }

        /// <summary>
        /// Stop auto-refresh
        /// </summary>
        public void StopAutoRefresh()
        {
            _refreshTimer.Stop();
        }

        /// <summary>
        /// Refresh all dashboard data
        /// </summary>
        private async Task RefreshDataAsync()
        {
            try
            {
                StatusMessage = "Connecting...";
                var data = await _dashboardService.FetchDashboardDataAsync();

                // Update Network Status
                if (data.WanStatus != null)
                {
                    ConnectStatus = data.WanStatus.connect_status ?? "Unknown";
                    Ipv4Address = data.WanStatus.ipv4_address ?? "N/A";
                    Ipv6Address = data.WanStatus.ipv6_address ?? "N/A";
                }

                // Update Speed
                if (data.SpeedStat != null)
                {
                    UploadSpeed = FormatUtil.FormatSpeed(data.SpeedStat.real_tx_speed);
                    DownloadSpeed = FormatUtil.FormatSpeed(data.SpeedStat.real_rx_speed);

                    // Update Traffic
                    DayTraffic = FormatUtil.FormatBytes(data.SpeedStat.day_tx_bytes + data.SpeedStat.day_rx_bytes);
                    MonthTraffic = FormatUtil.FormatBytes(data.SpeedStat.month_tx_bytes + data.SpeedStat.month_rx_bytes);
                    TotalTraffic = FormatUtil.FormatBytes(data.SpeedStat.total_tx_bytes + data.SpeedStat.total_rx_bytes);
                }

                // Update SIM Info
                if (data.SimInfo != null)
                {
                    Operator = data.SimInfo.Operator ?? "Unknown";
                    SimStatus = data.SimInfo.sim_states ?? "Unknown";
                    Iccid = FormatUtil.MaskString(data.SimInfo.sim_iccid ?? "N/A");
                    MacAddress = data.SimInfo.wlan_mac_address ?? "N/A";
                }

                // Update Signal Info
                if (data.NetInfo != null)
                {
                    NetworkType = data.NetInfo.network_type ?? "Unknown";
                    SignalBar = data.NetInfo.signalbar ?? "0";
                    ActiveBand = data.NetInfo.wan_active_band ?? "N/A";
                    Rsrp = $"{data.NetInfo.nr5g_rsrp} dBm";
                    Rsrq = $"{data.NetInfo.nr5g_rsrq} dB";
                    Snr = $"{data.NetInfo.nr5g_snr} dB";
                }

                // Update Device Count
                if (data.DeviceCount != null)
                {
                    TotalDevices = data.DeviceCount.access_total_num;
                    WirelessDevices = data.DeviceCount.wireless_num;
                    WiredDevices = data.DeviceCount.lan_num;
                    OfflineDevices = data.DeviceCount.offline_num;
                }

                // Update Last Update Time
                LastUpdate = data.LastUpdate.ToString("HH:mm:ss");
                StatusMessage = "Connected";
            }
            catch (System.Net.Http.HttpRequestException httpEx)
            {
                StatusMessage = $"Network Error: {httpEx.Message}";
                if (httpEx.InnerException != null)
                    StatusMessage += $" | Inner: {httpEx.InnerException.Message}";
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                StatusMessage = $"JSON Parse Error: {jsonEx.Message} | Path: {jsonEx.Path}";
            }
            catch (System.ObjectDisposedException dispEx)
            {
                StatusMessage = $"Object Disposed: {dispEx.ObjectName} | {dispEx.Message}";
            }
            catch (Exception ex)
            {
                // Show detailed error information
                var errorMsg = $"Error: {ex.GetType().Name} - {ex.Message}";
                if (ex.InnerException != null)
                    errorMsg += $" | Inner: {ex.InnerException.Message}";

                // Truncate if too long for display (show first 200 chars)
                if (errorMsg.Length > 200)
                    errorMsg = errorMsg.Substring(0, 197) + "...";

                StatusMessage = errorMsg;

                // Log full error to debug output
                System.Diagnostics.Debug.WriteLine("=== Dashboard Refresh Error ===");
                System.Diagnostics.Debug.WriteLine($"Type: {ex.GetType().FullName}");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.GetType().FullName}");
                    System.Diagnostics.Debug.WriteLine($"Inner Message: {ex.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine("===============================");
            }
        }
    }
}
