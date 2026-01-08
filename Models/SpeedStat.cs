using System;

namespace ZTE.Models
{
    /// <summary>
    /// Speed and traffic statistics from id=46 (zwrt_data.get_wwandst)
    /// </summary>
    public class SpeedStat
    {
        public long real_tx_speed { get; set; }
        public long real_rx_speed { get; set; }
        public long real_max_tx_speed { get; set; }
        public long real_max_rx_speed { get; set; }

        public long real_tx_bytes { get; set; }
        public long real_rx_bytes { get; set; }

        public long day_tx_bytes { get; set; }
        public long day_rx_bytes { get; set; }

        public long month_tx_bytes { get; set; }
        public long month_rx_bytes { get; set; }

        public long total_tx_bytes { get; set; }
        public long total_rx_bytes { get; set; }
    }
}
