using AndroAdmin.ThreatBoard.Services;
using AndroAdmin.ThreatBoard.ViewModels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Linq;

namespace AndroAdmin.ThreatBoard.Hubs
{
    [Authorize]
    [HubName("ThreatDashboardStores")]
    public class ThreatDashboardStoreHub : Hub
    {
        public StoreViewModel[] Read() 
        {
            var stores = ThreatDashboardJobService.Stores.Values.Where(e=> e.Connections.All(conn => !conn.Value.Connected)).ToArray();

            return stores.Select(e => e.ToViewModel()).ToArray();
        }
    }
}
