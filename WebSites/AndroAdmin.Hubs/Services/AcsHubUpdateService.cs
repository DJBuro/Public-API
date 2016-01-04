using Microsoft.AspNet.SignalR;
using System;


namespace AndroAdmin.ThreatBoard.Services
{
    public interface IAcsHubUpdateService
    {
        void Create(Models.HubState hubState);
        void Update(Models.HubState hubState);
        void Destroy(Models.HubState hubState);
    }

    public class AcsHubUpdateService : IAcsHubUpdateService 
    {
        public void Create(Models.HubState hubState)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardHubs>();

            hubContext.Clients.All.Create(hubState);
        }

        public void Update(Models.HubState hubState)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardHubs>();

            hubContext.Clients.All.Update(hubState);
        }

        public void Destroy(Models.HubState hubState)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardHubs>();

            hubContext.Clients.All.Destroy(hubState);
        }
    }
}