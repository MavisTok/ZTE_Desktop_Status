using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ZTE.Api;
using ZTE.Models;

namespace ZTE.Services
{
    /// <summary>
    /// Service for fetching all home dashboard data in batch
    /// </summary>
    public class HomeDashboardService
    {
        private readonly UbusClient _client;

        public HomeDashboardService(UbusClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Fetch all dashboard data in a single batch request (no-auth mode)
        /// Uses unauthenticated APIs that work with session "00000000000000000000000000000000"
        /// </summary>
        public async Task<DashboardData> FetchDashboardDataAsync()
        {
            // Build batch request with all required calls (no-auth mode)
            var calls = new List<object>
            {
                // id=1: router status (no auth required)
                _client.BuildCall("zwrt_router.api", "router_get_status_no_auth", new {}, 1),

                // id=2: SIM info (before login)
                _client.BuildCall("zwrt_zte_mdm.api", "get_sim_info_before", new {}, 2),

                // id=3: network type and signal info
                _client.BuildCall("zte_nwinfo_api", "nwinfo_get_netinfo", new {}, 3),

                // id=4: speed and traffic statistics
                _client.BuildCall("zwrt_data", "get_wwandst", new { source_module = "web", cid = 1, type = 4 }, 4),

                // id=5: WAN interface status
                _client.BuildCall("zwrt_data", "get_wwaniface", new { source_module = "web", cid = 1, connect_status = "" }, 5)
            };

            // Execute batch request
            var results = await _client.BatchCallAsync(calls);

            // Parse results into strongly-typed models
            var data = new DashboardData();

            // id=2: SIM info
            if (results.ContainsKey(2))
                data.SimInfo = JsonSerializer.Deserialize<SimInfo>(results[2].GetRawText());

            // id=3: network and signal info
            if (results.ContainsKey(3))
                data.NetInfo = JsonSerializer.Deserialize<NetInfo>(results[3].GetRawText());

            // id=4: speed and traffic statistics
            if (results.ContainsKey(4))
                data.SpeedStat = JsonSerializer.Deserialize<SpeedStat>(results[4].GetRawText());

            // id=5: WAN interface status
            if (results.ContainsKey(5))
                data.WanStatus = JsonSerializer.Deserialize<WanStatus>(results[5].GetRawText());

            data.LastUpdate = DateTime.Now;

            return data;
        }
    }

    /// <summary>
    /// Container for all dashboard data
    /// </summary>
    public class DashboardData
    {
        public SpeedStat SpeedStat { get; set; }
        public SimInfo SimInfo { get; set; }
        public NetInfo NetInfo { get; set; }
        public DeviceCount DeviceCount { get; set; }
        public WifiReport WifiReport { get; set; }
        public WanStatus WanStatus { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
