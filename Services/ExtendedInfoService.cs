using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ZTE.Api;
using ZTE.Models;

namespace ZTE.Services
{
    /// <summary>
    /// Service for fetching extended device information
    /// </summary>
    public class ExtendedInfoService
    {
        private readonly UbusClient _ubusClient;

        public ExtendedInfoService(UbusClient ubusClient)
        {
            _ubusClient = ubusClient;
        }

        /// <summary>
        /// 获取 WiFi 详细信息
        /// API: zwrt_wlan.report
        /// </summary>
        public async Task<WifiDetailInfo> GetWifiDetailInfoAsync()
        {
            try
            {
                var payload = await _ubusClient.CallAsJsonAsync("zwrt_wlan", "report", new { });

                return new WifiDetailInfo
                {
                    WifiOnOff = GetStringProperty(payload, "wifi_onoff"),
                    Main2gSsid = GetStringProperty(payload, "main2g_ssid"),
                    Main2gAuthMode = GetStringProperty(payload, "main2g_authmode"),
                    Main5gSsid = GetStringProperty(payload, "main5g_ssid"),
                    Main5gAuthMode = GetStringProperty(payload, "main5g_authmode"),
                    Radio2 = GetStringProperty(payload, "radio2"),
                    Radio5 = GetStringProperty(payload, "radio5"),
                    Radio2Disabled = GetStringProperty(payload, "radio2_disabled"),
                    Radio5Disabled = GetStringProperty(payload, "radio5_disabled"),
                    LbdEnable = GetStringProperty(payload, "lbd_enable"),
                    WifiStartMode = GetStringProperty(payload, "wifi_start_mode"),
                    DfsStatus = GetStringProperty(payload, "dfs_status"),
                    LoadStatus = GetStringProperty(payload, "load_status")
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get WiFi detail info: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取电池详细信息
        /// API: zwrt_mc.device.manager.get_device_info
        /// </summary>
        public async Task<BatteryInfo> GetBatteryInfoAsync()
        {
            try
            {
                var args = new
                {
                    deviceInfoList = new[]
                    {
                        "bat_online",
                        "bat_percent",
                        "bat_charger_connect",
                        "bat_charger_status",
                        "bat_temperature_level",
                        "bat_mode",
                        "bat_ui_charger_type",
                        "external_charging_flag",
                        "bat_time_to_full",
                        "bat_time_to_empty",
                        "bat_temperature",
                        "power_saver_mode"
                    }
                };

                var payload = await _ubusClient.CallAsJsonAsync("zwrt_mc.device.manager", "get_device_info", args);

                return new BatteryInfo
                {
                    BatOnline = GetStringProperty(payload, "bat_online"),
                    BatPercent = GetStringProperty(payload, "bat_percent"),
                    BatChargerConnect = GetStringProperty(payload, "bat_charger_connect"),
                    BatChargerStatus = GetStringProperty(payload, "bat_charger_status"),
                    BatTemperatureLevel = GetStringProperty(payload, "bat_temperature_level"),
                    BatMode = GetStringProperty(payload, "bat_mode"),
                    BatUiChargerType = GetStringProperty(payload, "bat_ui_charger_type"),
                    ExternalChargingFlag = GetStringProperty(payload, "external_charging_flag"),
                    BatTimeToFull = GetStringProperty(payload, "bat_time_to_full"),
                    BatTimeToEmpty = GetStringProperty(payload, "bat_time_to_empty"),
                    BatTemperature = GetStringProperty(payload, "bat_temperature"),
                    PowerSaverMode = GetStringProperty(payload, "power_saver_mode")
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get battery info: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取电源模式信息
        /// API: zwrt_deviceui.zwrt_deviceui_direct_power_mode_get
        /// </summary>
        public async Task<PowerModeInfo> GetPowerModeInfoAsync()
        {
            try
            {
                var args = new { moduleName = "web" };
                var payload = await _ubusClient.CallAsJsonAsync("zwrt_deviceui", "zwrt_deviceui_direct_power_mode_get", args);

                return new PowerModeInfo
                {
                    ModuleName = GetStringProperty(payload, "moduleName"),
                    Enable = GetStringProperty(payload, "enable")
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get power mode info: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取5G/LTE小区详细信息
        /// API: zte_nwinfo_api.nwinfo_get_netinfo
        /// </summary>
        public async Task<CellDetailInfo> GetCellDetailInfoAsync()
        {
            try
            {
                var payload = await _ubusClient.CallAsJsonAsync("zte_nwinfo_api", "nwinfo_get_netinfo", new { });

                return new CellDetailInfo
                {
                    network_type = GetStringProperty(payload, "network_type"),
                    network_provider_fullname = GetStringProperty(payload, "network_provider_fullname"),
                    wan_active_band = GetStringProperty(payload, "wan_active_band"),
                    signalbar = GetStringProperty(payload, "signalbar"),

                    // 5G NR 信号
                    nr5g_rsrp = GetStringProperty(payload, "nr5g_rsrp"),
                    nr5g_rsrq = GetStringProperty(payload, "nr5g_rsrq"),
                    nr5g_snr = GetStringProperty(payload, "nr5g_snr"),
                    nr5g_sinr = GetStringProperty(payload, "nr5g_sinr"),

                    // 5G NR 小区
                    nr5g_pci = GetStringProperty(payload, "nr5g_pci"),
                    nr5g_band = GetStringProperty(payload, "nr5g_action_band"),  // 使用nr5g_action_band
                    nr5g_bandwidth = GetStringProperty(payload, "nr5g_bandwidth"),
                    nr5g_arfcn = GetStringProperty(payload, "nr5g_action_channel"),  // 使用nr5g_action_channel
                    nr5g_cell_id = GetStringProperty(payload, "nr5g_cell_id"),

                    // LTE 信号
                    lte_rsrp = GetStringProperty(payload, "lte_rsrp"),
                    lte_rsrq = GetStringProperty(payload, "lte_rsrq"),
                    lte_snr = GetStringProperty(payload, "lte_snr"),
                    lte_sinr = GetStringProperty(payload, "lte_sinr"),
                    lte_rssi = GetStringProperty(payload, "lte_rssi"),

                    // LTE 小区
                    lte_pci = GetStringProperty(payload, "lte_pci"),
                    lte_band = GetStringProperty(payload, "lte_band"),
                    lte_bandwidth = GetStringProperty(payload, "lte_bandwidth"),
                    lte_earfcn = GetStringProperty(payload, "lte_earfcn"),
                    lte_cell_id = GetStringProperty(payload, "lte_cell_id"),

                    // CA 信息
                    ca_status = GetStringProperty(payload, "ca_status"),
                    ca_pcell_band = GetStringProperty(payload, "ca_pcell_band"),
                    ca_scell_band = GetStringProperty(payload, "ca_scell_band"),
                    ca_pcell_bandwidth = GetStringProperty(payload, "ca_pcell_bandwidth"),
                    ca_scell_bandwidth = GetStringProperty(payload, "ca_scell_bandwidth"),

                    // 其他参数
                    network_mode = GetStringProperty(payload, "network_mode"),
                    roaming_status = GetStringProperty(payload, "roaming_status"),
                    service_domain = GetStringProperty(payload, "service_domain"),
                    lac = GetStringProperty(payload, "lac"),
                    tac = GetStringProperty(payload, "tac"),
                    plmn = GetStringProperty(payload, "plmn")
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get cell detail info: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取设备硬件信息
        /// API: 使用router_get_status_no_auth或其他相关API
        /// </summary>
        public async Task<DeviceHardwareInfo> GetDeviceHardwareInfoAsync()
        {
            try
            {
                // 尝试获取设备信息
                var payload = await _ubusClient.CallAsJsonAsync("zwrt_router.api", "router_get_status_no_auth", new { });

                return new DeviceHardwareInfo
                {
                    device_name = GetStringProperty(payload, "device_name"),
                    hardware_version = GetStringProperty(payload, "hardware_version"),
                    software_version = GetStringProperty(payload, "software_version"),
                    firmware_version = GetStringProperty(payload, "firmware_version"),
                    imei = GetStringProperty(payload, "imei"),
                    mac_address = GetStringProperty(payload, "mac_address"),
                    serial_number = GetStringProperty(payload, "serial_number"),

                    cpu_usage = GetStringProperty(payload, "cpu_usage"),
                    cpu_model = GetStringProperty(payload, "cpu_model"),
                    cpu_cores = GetStringProperty(payload, "cpu_cores"),
                    cpu_frequency = GetStringProperty(payload, "cpu_frequency"),

                    memory_total = GetStringProperty(payload, "memory_total"),
                    memory_used = GetStringProperty(payload, "memory_used"),
                    memory_free = GetStringProperty(payload, "memory_free"),
                    memory_usage_percent = GetStringProperty(payload, "memory_usage_percent"),

                    storage_total = GetStringProperty(payload, "storage_total"),
                    storage_used = GetStringProperty(payload, "storage_used"),
                    storage_free = GetStringProperty(payload, "storage_free"),
                    storage_usage_percent = GetStringProperty(payload, "storage_usage_percent"),

                    device_temperature = GetStringProperty(payload, "device_temperature"),
                    cpu_temperature = GetStringProperty(payload, "cpu_temperature"),
                    modem_temperature = GetStringProperty(payload, "modem_temperature"),

                    uptime = GetStringProperty(payload, "uptime"),
                    connection_time = GetStringProperty(payload, "connection_time")
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get device hardware info: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取所有扩展信息（批量调用）
        /// </summary>
        public async Task<(WifiDetailInfo wifi, BatteryInfo battery, PowerModeInfo powerMode, CellDetailInfo cell, DeviceHardwareInfo device)> GetAllExtendedInfoAsync()
        {
            try
            {
                // 构建批量调用（只调用4个已知可用的API）
                var calls = new List<object>
                {
                    _ubusClient.BuildCall("zwrt_wlan", "report", new { }, 1),
                    _ubusClient.BuildCall("zwrt_mc.device.manager", "get_device_info", new
                    {
                        deviceInfoList = new[]
                        {
                            "bat_online", "bat_percent", "bat_charger_connect", "bat_charger_status",
                            "bat_temperature_level", "bat_mode", "bat_ui_charger_type", "external_charging_flag",
                            "bat_time_to_full", "bat_time_to_empty", "bat_temperature", "power_saver_mode"
                        }
                    }, 2),
                    _ubusClient.BuildCall("zwrt_deviceui", "zwrt_deviceui_direct_power_mode_get", new { moduleName = "web" }, 3),
                    _ubusClient.BuildCall("zte_nwinfo_api", "nwinfo_get_netinfo", new { }, 4)
                };

                var results = await _ubusClient.BatchCallAsync(calls);

                var wifi = new WifiDetailInfo();
                var battery = new BatteryInfo();
                var powerMode = new PowerModeInfo();
                var cell = new CellDetailInfo();
                var device = new DeviceHardwareInfo();

                // 解析 WiFi 信息 (id=1)
                if (results.TryGetValue(1, out var wifiPayload))
                {
                    wifi.WifiOnOff = GetStringProperty(wifiPayload, "wifi_onoff");
                    wifi.Main2gSsid = GetStringProperty(wifiPayload, "main2g_ssid");
                    wifi.Main2gAuthMode = GetStringProperty(wifiPayload, "main2g_authmode");
                    wifi.Main5gSsid = GetStringProperty(wifiPayload, "main5g_ssid");
                    wifi.Main5gAuthMode = GetStringProperty(wifiPayload, "main5g_authmode");
                    wifi.Radio2 = GetStringProperty(wifiPayload, "radio2");
                    wifi.Radio5 = GetStringProperty(wifiPayload, "radio5");
                    wifi.Radio2Disabled = GetStringProperty(wifiPayload, "radio2_disabled");
                    wifi.Radio5Disabled = GetStringProperty(wifiPayload, "radio5_disabled");
                    wifi.LbdEnable = GetStringProperty(wifiPayload, "lbd_enable");
                    wifi.WifiStartMode = GetStringProperty(wifiPayload, "wifi_start_mode");
                    wifi.DfsStatus = GetStringProperty(wifiPayload, "dfs_status");
                    wifi.LoadStatus = GetStringProperty(wifiPayload, "load_status");
                }

                // 解析电池信息 (id=2)
                if (results.TryGetValue(2, out var batteryPayload))
                {
                    battery.BatOnline = GetStringProperty(batteryPayload, "bat_online");
                    battery.BatPercent = GetStringProperty(batteryPayload, "bat_percent");
                    battery.BatChargerConnect = GetStringProperty(batteryPayload, "bat_charger_connect");
                    battery.BatChargerStatus = GetStringProperty(batteryPayload, "bat_charger_status");
                    battery.BatTemperatureLevel = GetStringProperty(batteryPayload, "bat_temperature_level");
                    battery.BatMode = GetStringProperty(batteryPayload, "bat_mode");
                    battery.BatUiChargerType = GetStringProperty(batteryPayload, "bat_ui_charger_type");
                    battery.ExternalChargingFlag = GetStringProperty(batteryPayload, "external_charging_flag");
                    battery.BatTimeToFull = GetStringProperty(batteryPayload, "bat_time_to_full");
                    battery.BatTimeToEmpty = GetStringProperty(batteryPayload, "bat_time_to_empty");
                    battery.BatTemperature = GetStringProperty(batteryPayload, "bat_temperature");
                    battery.PowerSaverMode = GetStringProperty(batteryPayload, "power_saver_mode");
                }

                // 解析电源模式信息 (id=3)
                if (results.TryGetValue(3, out var powerModePayload))
                {
                    powerMode.ModuleName = GetStringProperty(powerModePayload, "moduleName");
                    powerMode.Enable = GetStringProperty(powerModePayload, "enable");
                }

                // 解析5G/LTE小区信息 (id=4)
                if (results.TryGetValue(4, out var cellPayload))
                {
                    cell.network_type = GetStringProperty(cellPayload, "network_type");
                    cell.network_provider_fullname = GetStringProperty(cellPayload, "network_provider_fullname");
                    cell.wan_active_band = GetStringProperty(cellPayload, "wan_active_band");
                    cell.signalbar = GetStringProperty(cellPayload, "signalbar");

                    // 5G NR信号
                    cell.nr5g_rsrp = GetStringProperty(cellPayload, "nr5g_rsrp");
                    cell.nr5g_rsrq = GetStringProperty(cellPayload, "nr5g_rsrq");
                    cell.nr5g_snr = GetStringProperty(cellPayload, "nr5g_snr");
                    cell.nr5g_sinr = GetStringProperty(cellPayload, "nr5g_sinr");

                    // 5G NR小区
                    cell.nr5g_pci = GetStringProperty(cellPayload, "nr5g_pci");
                    cell.nr5g_band = GetStringProperty(cellPayload, "nr5g_action_band");  // 使用nr5g_action_band
                    cell.nr5g_bandwidth = GetStringProperty(cellPayload, "nr5g_bandwidth");
                    cell.nr5g_arfcn = GetStringProperty(cellPayload, "nr5g_action_channel");  // 使用nr5g_action_channel
                    cell.nr5g_cell_id = GetStringProperty(cellPayload, "nr5g_cell_id");

                    // LTE信号
                    cell.lte_rsrp = GetStringProperty(cellPayload, "lte_rsrp");
                    cell.lte_rsrq = GetStringProperty(cellPayload, "lte_rsrq");
                    cell.lte_snr = GetStringProperty(cellPayload, "lte_snr");
                    cell.lte_sinr = GetStringProperty(cellPayload, "lte_sinr");
                    cell.lte_rssi = GetStringProperty(cellPayload, "lte_rssi");

                    // LTE小区
                    cell.lte_pci = GetStringProperty(cellPayload, "lte_pci");
                    cell.lte_band = GetStringProperty(cellPayload, "lte_band");
                    cell.lte_bandwidth = GetStringProperty(cellPayload, "lte_bandwidth");
                    cell.lte_earfcn = GetStringProperty(cellPayload, "lte_earfcn");
                    cell.lte_cell_id = GetStringProperty(cellPayload, "lte_cell_id");

                    // CA信息
                    cell.ca_status = GetStringProperty(cellPayload, "ca_status");
                    cell.ca_pcell_band = GetStringProperty(cellPayload, "ca_pcell_band");
                    cell.ca_scell_band = GetStringProperty(cellPayload, "ca_scell_band");
                    cell.ca_pcell_bandwidth = GetStringProperty(cellPayload, "ca_pcell_bandwidth");
                    cell.ca_scell_bandwidth = GetStringProperty(cellPayload, "ca_scell_bandwidth");

                    // 其他参数
                    cell.network_mode = GetStringProperty(cellPayload, "network_mode");
                    cell.roaming_status = GetStringProperty(cellPayload, "roaming_status");
                    cell.service_domain = GetStringProperty(cellPayload, "service_domain");
                    cell.lac = GetStringProperty(cellPayload, "lac");
                    cell.tac = GetStringProperty(cellPayload, "tac");
                    cell.plmn = GetStringProperty(cellPayload, "plmn");
                }

                // 设备硬件信息 - 暂时使用默认值（这些信息需要特定的API或权限）
                device.device_name = "ZTE 5G CPE";
                // MAC地址、IMEI等信息暂时显示为N/A
                // 如果需要这些信息，需要找到正确的API调用方式

                return (wifi, battery, powerMode, cell, device);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get all extended info: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 辅助方法：从 JsonElement 中安全获取字符串属性
        /// 支持多种类型：String, Number, Boolean
        /// </summary>
        private string GetStringProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var prop))
            {
                return prop.ValueKind switch
                {
                    JsonValueKind.String => prop.GetString() ?? "",
                    JsonValueKind.Number => prop.GetRawText(),
                    JsonValueKind.True => "1",
                    JsonValueKind.False => "0",
                    JsonValueKind.Null => "",
                    _ => prop.GetRawText()
                };
            }
            return "";
        }
    }
}
