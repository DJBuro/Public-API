using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AndroAdmin.ThreatBoard.Hubs
{
    [Authorize]
    [HubName("ThreatDashboardDiagnosticsHub")]
    public class ThreatDashboardDiagnosticsHub : Hub 
    {
    }
}