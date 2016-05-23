using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using MyAndromeda.CloudSynchronization.Events;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.SignalRHubs.Handlers
{
    /// <summary>
    /// Synchronization handler
    /// </summary>
    public class SynchronizationHandler : ISynchronizationEvent
    {
        private readonly IHubContext hubContext;

        public SynchronizationHandler()
        {
            this.hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.CloudSynchronizationHub>();
        }

        public void Skipping(SyncronizationContext context)
        {
            
            hubContext.Clients.Group(ExpectedUserRoles.SuperAdministrator).ping(string.Format("SynchronizationHandler skipped ping: {0}", DateTime.UtcNow));
            hubContext.Clients.Group(ExpectedUserRoles.SuperAdministrator).skippedSynchronization(context);
        }

        public void Started(SyncronizationContext context)
        {
            foreach (var id in context.StoreIds) 
            { 
                hubContext.Clients.All.ping(string.Format("SynchronizationHandler started ping: {0}", DateTime.UtcNow));
                hubContext.Clients.All.startedSynchronization(context);
            }
        }

        public void Completed(SyncronizationContext context)
        {
            hubContext.Clients.All.ping(string.Format("SynchronizationHandler Completed ping: {0}", DateTime.UtcNow));
            hubContext.Clients.All.completedSynchronization(context);
        }

        public void Error(SyncronizationContext context, string error)
        {
            hubContext.Clients.All.errorSynchronization(new { context = context, error = error });
        }
    }
}