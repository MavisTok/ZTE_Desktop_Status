namespace ZTE.Models
{
    /// <summary>
    /// WiFi configuration from id=53 (zwrt_wifi.get_reportwifi)
    /// </summary>
    public class WifiReport
    {
        public string wifi_onoff { get; set; }
        public string main2g_ssid { get; set; }
        public string main5g_ssid { get; set; }
        public string main2g_authmode { get; set; }
        public string main5g_authmode { get; set; }
    }
}
