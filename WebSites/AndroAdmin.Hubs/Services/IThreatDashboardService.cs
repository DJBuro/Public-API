using AndroAdmin.ThreatBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Reactive.Linq;

namespace AndroAdmin.ThreatBoard.Services
{
    public interface IThreatDashboardJobService
    {

    }

    public class ThreatDashboardJobService : IThreatDashboardJobService
    {
        private static IObservable<AcsEventMessage> acsHubEventMessages;
        private static IObservable<AcsConnectionStatusMessage> connectionStatus;
        private static IObservable<string> notifications;
        
        private readonly DiagnosticConnectionService connectionService;
        private readonly StoreUpdateService storeUpdatService;

        public ThreatDashboardJobService() 
        {
            this.connectionService = new DiagnosticConnectionService();
            this.storeUpdatService = new StoreUpdateService();

            this.Init();
            this.InitStoreConnection();
        }

        public static IObservable<Store> FullyConnectedStores { get; set; }

        public static IObservable<Store> PartiallyConnectedStores { get; set; }

        public static IObservable<Store> DisconnectedStores { get; set; }

        public static ConcurrentDictionary<int, Store> Stores = new ConcurrentDictionary<int, Store>();


        private void InitStoreConnection() 
        {
            if (DisconnectedStores == null) 
            {
                connectionService.StoreChanges.Subscribe(store =>
                {
                    bool allConnected = store.Connections.All(e => e.Value.Connected);
                    bool partiallyConnected = store.Connections.Any(e => e.Value.Connected);
                    bool exists = Stores.ContainsKey(store.AndromedaSiteId);

                    var historicStore = Stores.GetOrAdd(store.AndromedaSiteId, store);
                    historicStore.LastUpdateUtc = store.LastUpdateUtc;
                    historicStore.Connections = store.Connections;
                    
                    //update / destroy
                    if (allConnected) { this.storeUpdatService.Destroy(historicStore); }
                    //update the ui
                    if (exists && partiallyConnected && !allConnected) { this.storeUpdatService.Update(historicStore); }
                    {
                        this.storeUpdatService.Update(historicStore);
                    }
                    if (!exists) 
                    {
                        this.storeUpdatService.Create(historicStore);
                    }

                });

                DisconnectedStores = connectionService.StoreChanges.Where(e => e.Connections.All(conn => !conn.Value.Connected));
            }
            if (FullyConnectedStores == null) 
            {
                FullyConnectedStores = connectionService.StoreChanges.Where(e => e.Connections.All(conn => conn.Value.Connected));
            }
            if (PartiallyConnectedStores == null) 
            {
                PartiallyConnectedStores = connectionService.StoreChanges.Where(e => e.Connections.Any(conn => conn.Value.Connected));
            }
        }

        private void Init() 
        {
            if (acsHubEventMessages == null)
            {
                Trace.WriteLine("AcsHubEventMessages is null");
                acsHubEventMessages = connectionService.GetAcsHubEventMessages;
                acsHubEventMessages.Subscribe((message) =>
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardStoreHub>();
                    hubContext.Clients.All.AcsHubMessage(message);
                });
            }
            if (connectionStatus == null)
            {
                Trace.WriteLine("ConnectionStatus is null");
                connectionStatus = connectionService.GetConnectionStatus;
                connectionStatus.Subscribe((message) =>
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardStoreHub>();
                    hubContext.Clients.All.AcsConnectionMessage(message);
                });
            }

            if (notifications == null)
            {
                Trace.WriteLine("Notifications is null");
                notifications = connectionService.GetNotifications;
                notifications.Subscribe((message) =>
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ThreatDashboardStoreHub>();
                    hubContext.Clients.All.Notification(message);
                });
            }
        }

    }
}
