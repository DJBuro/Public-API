using AndroAdmin.ThreatBoard.Models;
using AndroAdmin.ThreatBoard.Services;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Diagnostics;
namespace AndroAdmin.ThreatBoard.Jobs
{
    public class ThreatboardJob : WebBackgrounder.Job
    {
        public const string Name = "DiagnosticHubJob"; 

        public ThreatboardJob()
            : base(ThreatboardJob.Name, TimeSpan.FromSeconds(10)) 
        {
            
        }

        public void Run() 
        {
            IThreatDashboardJobService service = new ThreatDashboardJobService();

            //var h = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardStoreHub>();
            //h.Clients.All.Create(new [] { new ViewModels.StoreViewModel() { Id = Guid.NewGuid(), Name = "bob", LastUpdatedAtUtc = DateTime.UtcNow } });
        }

        public override Task Execute()
        {
            return new Task(this.Run);   
        }
    }
}
