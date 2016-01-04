using AndroAdmin.ThreatBoard.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Linq;

namespace AndroAdmin.ThreatBoard.Hubs
{
    [Authorize]
    [HubName("ThreatDashboardHubs")]
    public class ThreatDashboardHubs : Hub 
    {
        private readonly DiagnosticConnectionService diagnosticConnectionService;

        public ThreatDashboardHubs() 
        {
            this.diagnosticConnectionService = new DiagnosticConnectionService();
        }

        public IEnumerable<Models.HubState> Read() 
        {
            var values = diagnosticConnectionService.HubConnections.Values.Select(e => e.State);

            return values;
        }
    }
}