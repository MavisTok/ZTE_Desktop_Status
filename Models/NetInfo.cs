namespace ZTE.Models
{
    /// <summary>
    /// Network and signal information from id=48 (zwrt_data.get_nrinfo)
    /// </summary>
    public class NetInfo
    {
        public string network_type { get; set; }
        public string network_provider_fullname { get; set; }
        public string wan_active_band { get; set; }
        public string signalbar { get; set; }

        // 5G NR signal metrics
        public int nr5g_rsrp { get; set; }
        public int nr5g_rsrq { get; set; }
        public string nr5g_snr { get; set; }

        // LTE signal metrics (fallback)
        public int lte_rsrp { get; set; }
        public int lte_rsrq { get; set; }
        public string lte_snr { get; set; }
    }
}
