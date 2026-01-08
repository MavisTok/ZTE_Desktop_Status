namespace ZTE.Models
{
    /// <summary>
    /// 设备硬件信息
    /// </summary>
    public class DeviceHardwareInfo
    {
        // 设备基本信息
        public string device_name { get; set; }  // 设备名称
        public string hardware_version { get; set; }  // 硬件版本
        public string software_version { get; set; }  // 软件版本
        public string firmware_version { get; set; }  // 固件版本
        public string imei { get; set; }  // IMEI号
        public string mac_address { get; set; }  // MAC地址
        public string serial_number { get; set; }  // 序列号

        // CPU 信息
        public string cpu_usage { get; set; }  // CPU使用率
        public string cpu_model { get; set; }  // CPU型号
        public string cpu_cores { get; set; }  // CPU核心数
        public string cpu_frequency { get; set; }  // CPU频率

        // 内存信息
        public string memory_total { get; set; }  // 总内存
        public string memory_used { get; set; }  // 已用内存
        public string memory_free { get; set; }  // 空闲内存
        public string memory_usage_percent { get; set; }  // 内存使用率

        // 存储信息
        public string storage_total { get; set; }  // 总存储空间
        public string storage_used { get; set; }  // 已用存储
        public string storage_free { get; set; }  // 空闲存储
        public string storage_usage_percent { get; set; }  // 存储使用率

        // 温度信息
        public string device_temperature { get; set; }  // 设备温度
        public string cpu_temperature { get; set; }  // CPU温度
        public string modem_temperature { get; set; }  // 调制解调器温度

        // 运行状态
        public string uptime { get; set; }  // 运行时间
        public string connection_time { get; set; }  // 连接时长
    }
}
