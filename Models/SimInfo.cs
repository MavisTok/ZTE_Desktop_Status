namespace ZTE.Models
{
    /// <summary>
    /// SIM card information from id=47 (zwrt_data.getbasicinfo)
    /// </summary>
    public class SimInfo
    {
        public string sim_states { get; set; }
        public string Operator { get; set; }
        public string sim_iccid { get; set; }
        public string sim_imsi { get; set; }
        public string modem_main_state { get; set; }
        public string wlan_mac_address { get; set; }
        public string mdm_mcc { get; set; }
        public string mdm_mnc { get; set; }
    }
}
