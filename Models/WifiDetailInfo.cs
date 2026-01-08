namespace ZTE.Models
{
    /// <summary>
    /// WiFi 详细信息
    /// </summary>
    public class WifiDetailInfo
    {
        /// <summary>
        /// WiFi 总开关：1-开启，0-关闭
        /// </summary>
        public string WifiOnOff { get; set; }

        /// <summary>
        /// 2.4G SSID 名称
        /// </summary>
        public string Main2gSsid { get; set; }

        /// <summary>
        /// 2.4G 认证模式
        /// </summary>
        public string Main2gAuthMode { get; set; }

        /// <summary>
        /// 5G SSID 名称
        /// </summary>
        public string Main5gSsid { get; set; }

        /// <summary>
        /// 5G 认证模式
        /// </summary>
        public string Main5gAuthMode { get; set; }

        /// <summary>
        /// 2.4G 射频名称
        /// </summary>
        public string Radio2 { get; set; }

        /// <summary>
        /// 5G 射频名称
        /// </summary>
        public string Radio5 { get; set; }

        /// <summary>
        /// 2.4G 禁用状态：0-启用，1-禁用
        /// </summary>
        public string Radio2Disabled { get; set; }

        /// <summary>
        /// 5G 禁用状态：0-启用，1-禁用
        /// </summary>
        public string Radio5Disabled { get; set; }

        /// <summary>
        /// LBD（负载均衡）启用：1-启用，0-禁用
        /// </summary>
        public string LbdEnable { get; set; }

        /// <summary>
        /// WiFi 启动模式
        /// </summary>
        public string WifiStartMode { get; set; }

        /// <summary>
        /// DFS 状态
        /// </summary>
        public string DfsStatus { get; set; }

        /// <summary>
        /// 加载状态
        /// </summary>
        public string LoadStatus { get; set; }
    }
}
