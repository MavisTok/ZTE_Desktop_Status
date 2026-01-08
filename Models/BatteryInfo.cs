namespace ZTE.Models
{
    /// <summary>
    /// 电池详细信息
    /// </summary>
    public class BatteryInfo
    {
        /// <summary>
        /// 电池在线状态：1-在线，0-不在线
        /// </summary>
        public string BatOnline { get; set; }

        /// <summary>
        /// 电池电量百分比
        /// </summary>
        public string BatPercent { get; set; }

        /// <summary>
        /// 充电器连接状态：1-已连接，0-未连接
        /// </summary>
        public string BatChargerConnect { get; set; }

        /// <summary>
        /// 充电状态：0-未充电，1-正在充电
        /// </summary>
        public string BatChargerStatus { get; set; }

        /// <summary>
        /// 电池温度等级
        /// </summary>
        public string BatTemperatureLevel { get; set; }

        /// <summary>
        /// 电池模式
        /// </summary>
        public string BatMode { get; set; }

        /// <summary>
        /// UI 充电器类型
        /// </summary>
        public string BatUiChargerType { get; set; }

        /// <summary>
        /// 外部充电标志
        /// </summary>
        public string ExternalChargingFlag { get; set; }

        /// <summary>
        /// 充满所需时间（分钟）
        /// </summary>
        public string BatTimeToFull { get; set; }

        /// <summary>
        /// 电量耗尽时间（分钟）
        /// </summary>
        public string BatTimeToEmpty { get; set; }

        /// <summary>
        /// 电池温度（摄氏度）
        /// </summary>
        public string BatTemperature { get; set; }

        /// <summary>
        /// 省电模式
        /// </summary>
        public string PowerSaverMode { get; set; }
    }
}
