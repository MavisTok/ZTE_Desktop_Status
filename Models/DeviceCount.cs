namespace ZTE.Models
{
    /// <summary>
    /// Connected devices count from id=51 (zwrt_data.get_stanum)
    /// </summary>
    public class DeviceCount
    {
        public int access_total_num { get; set; }
        public int wireless_num { get; set; }
        public int lan_num { get; set; }
        public int offline_num { get; set; }
    }
}
