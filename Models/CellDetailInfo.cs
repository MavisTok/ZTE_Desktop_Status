namespace ZTE.Models
{
    /// <summary>
    /// 5G/LTE 小区详细信息
    /// 从 zte_nwinfo_api.nwinfo_get_netinfo 获取
    /// </summary>
    public class CellDetailInfo
    {
        // 基本网络信息
        public string network_type { get; set; }  // 网络类型：5G NSA/SA, LTE等
        public string network_provider_fullname { get; set; }  // 运营商全名
        public string wan_active_band { get; set; }  // 激活频段
        public string signalbar { get; set; }  // 信号格数

        // 5G NR 信号质量
        public string nr5g_rsrp { get; set; }  // 参考信号接收功率
        public string nr5g_rsrq { get; set; }  // 参考信号接收质量
        public string nr5g_snr { get; set; }   // 信噪比
        public string nr5g_sinr { get; set; }  // 信号与干扰加噪声比

        // 5G NR 小区信息
        public string nr5g_pci { get; set; }   // 物理小区ID
        public string nr5g_band { get; set; }  // NR频段
        public string nr5g_bandwidth { get; set; }  // 带宽
        public string nr5g_arfcn { get; set; } // 绝对射频频道号
        public string nr5g_cell_id { get; set; }  // 小区ID

        // LTE 信号质量（主小区或锚点小区）
        public string lte_rsrp { get; set; }
        public string lte_rsrq { get; set; }
        public string lte_snr { get; set; }
        public string lte_sinr { get; set; }
        public string lte_rssi { get; set; }  // 接收信号强度指示

        // LTE 小区信息
        public string lte_pci { get; set; }
        public string lte_band { get; set; }
        public string lte_bandwidth { get; set; }
        public string lte_earfcn { get; set; }
        public string lte_cell_id { get; set; }

        // CA (载波聚合) 信息
        public string ca_status { get; set; }  // CA状态
        public string ca_pcell_band { get; set; }  // 主载波频段
        public string ca_scell_band { get; set; }  // 辅载波频段
        public string ca_pcell_bandwidth { get; set; }
        public string ca_scell_bandwidth { get; set; }

        // 其他网络参数
        public string network_mode { get; set; }  // 网络模式
        public string roaming_status { get; set; }  // 漫游状态
        public string service_domain { get; set; }  // 服务域
        public string lac { get; set; }  // 位置区码
        public string tac { get; set; }  // 追踪区码
        public string plmn { get; set; }  // 公共陆地移动网络
    }
}
