namespace ZTE.Models
{
    /// <summary>
    /// WAN interface status from id=55 (zwrt_data.get_wwaniface)
    /// </summary>
    public class WanStatus
    {
        public string connect_status { get; set; }
        public string ipv4_address { get; set; }
        public string ipv4_dns_prefer { get; set; }
        public string ipv6_address { get; set; }
        public string ipv6_dns_prefer { get; set; }
    }
}
