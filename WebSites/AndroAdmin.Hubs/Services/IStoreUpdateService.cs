using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroAdmin.ThreatBoard.Models;
using AndroAdmin.ThreatBoard.ViewModels;
using Microsoft.AspNet.SignalR;

namespace AndroAdmin.ThreatBoard.Services
{
    public interface IStoreUpdateService
    {
        void Create(Models.Store store);
        void Update(Models.Store store);
        void Destroy(Models.Store store);
    }

    public class StoreUpdateService : IStoreUpdateService 
    {
        public StoreUpdateService() { }
        
        public void Create(Store store)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardStoreHub>();

            hubContext.Clients.All.Create(new [] { store.ToViewModel() });
        }

        public void Update(Store store)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardStoreHub>();

            hubContext.Clients.All.Update(new[] { store.ToViewModel() });
        }

        public void Destroy(Store store)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardStoreHub>();

            hubContext.Clients.All.Destroy(new[] { store.ToViewModel() });
        }
    }

    
}
