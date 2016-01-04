using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using AndroAdmin.ThreatBoard.Models;
using AndroAdminDataAccess.DataAccess;
using AndroAdminDataAccess.EntityFramework.DataAccess;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using System.Reactive.Linq;
using System.Diagnostics;

namespace AndroAdmin.ThreatBoard.Services
{
    internal static class DiagnosticConnectionServiceObserver 
    {
        internal static IDisposable StoreGroupObserver;

        internal static readonly ConcurrentDictionary<string, HubGroup> HubConnections = new ConcurrentDictionary<string, HubGroup>();
        internal static readonly ConcurrentDictionary<string, Models.Store> StoreConnections = new ConcurrentDictionary<string, Store>();

        internal static readonly Subject<AcsEventMessage> AcsMessagesSubject = new Subject<AcsEventMessage>();
        internal static readonly Subject<AcsConnectionStatusMessage> AcsConnectionMessageStatus = new Subject<AcsConnectionStatusMessage>();
        internal static readonly Subject<string> Notifications = new Subject<string>();
        internal static readonly Subject<Models.Store> StoreSubject = new Subject<Store>();
    }
    
    public class DiagnosticConnectionService : IDiagnosticConnectionService
    {
        const string PrivateHub = "WebOrderingSignalR";
        const string AcsHubName = "AndromedaHub"; 

        private readonly IHostV2DataService hostV2DataConnection;

        public DiagnosticConnectionService()
        {
            this.hostV2DataConnection = new HostV2DataConnectionService();
            this.Init();
            this.InitStoreObserver();
        }
 
        private void InitStoreObserver()
        {
            if (DiagnosticConnectionServiceObserver.StoreGroupObserver == null) 
            { 

                DiagnosticConnectionServiceObserver.StoreGroupObserver = this.GetAcsHubEventMessages.GroupBy(e => e.SiteName).Subscribe(siteGroup =>
                {
                    siteGroup.Subscribe((siteEvent) =>
                    {
                        var key = siteEvent.SiteId;

                        var store = DiagnosticConnectionServiceObserver.StoreConnections.GetOrAdd(key, (aKey) => { return new Store { AndromedaSiteId = Convert.ToInt32(aKey), Connections = new ConcurrentDictionary<string, ConnectionUpdate>() }; });

                        //get the record for this hub. 
                        var hubRecord = store.Connections.GetOrAdd(siteEvent.SourceHub, (source) => { return new ConnectionUpdate(); });

                        //record that store is online 
                        if (siteEvent.IsOnline()) 
                        {
                            hubRecord.Connected = true;
                            hubRecord.LastConnectedUtc = DateTime.UtcNow;
                        }
                        //record that the store is offline 
                        if (siteEvent.IsOffline()) 
                        {
                            hubRecord.Connected = false;
                            hubRecord.LastDisconnectedUtc = DateTime.UtcNow;
                        }

                        store.LastUpdateUtc = DateTime.UtcNow;
                        DiagnosticConnectionServiceObserver.StoreSubject.OnNext(store);
                    });
                });
            }
        }

        /// <summary>
        /// Gets the get notifications.
        /// </summary>
        /// <value>The get notifications.</value>
        public IObservable<string> GetNotifications 
        {
            get 
            {
                return DiagnosticConnectionServiceObserver.Notifications;
            } 
        }

        /// <summary>
        /// Gets the get acs hub event messages.
        /// </summary>
        /// <value>The get acs event messages.</value>
        public IObservable<AcsEventMessage> GetAcsHubEventMessages 
        {
            get 
            {
                return DiagnosticConnectionServiceObserver.AcsMessagesSubject;
            }
        }

        /// <summary>
        /// Gets the get connection status.
        /// </summary>
        /// <value>The get connection status.</value>
        public IObservable<AcsConnectionStatusMessage> GetConnectionStatus
        {
            get 
            {
                return DiagnosticConnectionServiceObserver.AcsConnectionMessageStatus;
            }
        }

        public IDictionary<string, HubGroup> HubConnections 
        {
            get 
            {
                return DiagnosticConnectionServiceObserver.HubConnections;
            }
        }

        public IObservable<Store> StoreChanges
        {
            get
            {
                return DiagnosticConnectionServiceObserver.StoreSubject;
            }
        }

        public IDictionary<string, Store> StoreStatus 
        {
            get 
            {
                return DiagnosticConnectionServiceObserver.StoreConnections;
            }
        }

        public void Init() 
        {
            var hosts = hostV2DataConnection.List(e => e.HostType.Name == PrivateHub && e.Version > 1).ToArray();
            //these hosts don't exist anymore for some reason
            var disconnectThese = DiagnosticConnectionServiceObserver.HubConnections.Keys.Where(e => !hosts.Any(host => host.Url == e));
            //get the hosts that do not exist in connections 
            var addHosts = hosts.Where(e => !DiagnosticConnectionServiceObserver.HubConnections.Keys.Any(key => key == e.Url));

            foreach (var url in disconnectThese) 
            {
                this.Disconnect(url);
            }

            foreach (var host in addHosts) 
            {
                this.Connect(host.Url);
            }

        }
 
        public void Disconnect(string url)
        {
            var hubConnection = DiagnosticConnectionServiceObserver.HubConnections[url];

            hubConnection.Dispose();

            HubGroup output;
            DiagnosticConnectionServiceObserver.HubConnections.TryRemove(url, out output);
        }

        public void Connect(string url) 
        {
            var hubConnection = new HubConnection(url);

            hubConnection.TraceLevel = TraceLevels.All;

            var hubProxy = hubConnection.CreateHubProxy(AcsHubName);
            
            var hubGroup = new HubGroup(url)
            {
                Connection = hubConnection,
                Proxy = hubProxy
            };

            DiagnosticConnectionServiceObserver.HubConnections.TryAdd(url, hubGroup);

            var connectionObserver =
                //Observable.FromEventPattern(e => hubConnection.StateChanged += e, e => hubConnection.StateChanged -= e)
                //.Select(e => e.EventArgs as StateChange);
                Observable.FromEvent<StateChange>(
                (stateChanged) => hubConnection.StateChanged += stateChanged, 
                (stateChanged) => hubConnection.StateChanged -= stateChanged);
            var hubConnectionErrorObserver =
                Observable.FromEvent<System.Exception>((ex) => hubConnection.Error += ex, (ex) => hubConnection.Error -= ex);

            var acsMessageObserver = hubProxy.Observe("ACSStateChange")
                .Select(e =>
                {
                    var model = e[0].ToObject<Models.AcsEventMessage>();
                    model.SourceHub = url;

                    return model;
                });

            hubConnectionErrorObserver.Subscribe((onEvent) => {
                Trace.WriteLine(url);
                Trace.WriteLine(onEvent.Message);
            });

            connectionObserver.Subscribe(
                (onEvent) => {
                    hubGroup.State.Online = onEvent.NewState == ConnectionState.Connected; 
                    hubGroup.State.LastUpdatedAtUtc = DateTime.UtcNow;

                    DiagnosticConnectionServiceObserver.AcsConnectionMessageStatus.OnNext(new AcsConnectionStatusMessage()
                    { 
                        OldState = onEvent.OldState.ToString(),
                        NewState = onEvent.NewState.ToString(),
                        Url = url
                    }); 
                },
                (onError) => {
                    DiagnosticConnectionServiceObserver.Notifications.OnNext(onError.Message);
                },
                //on complete
                () => {
                    DiagnosticConnectionServiceObserver.Notifications.OnNext(string.Format("observation of connection complete : {0}", url));
                }
            );

            acsMessageObserver.Subscribe(
                (onMessage) =>
                {
                    DiagnosticConnectionServiceObserver.Notifications.OnNext("ACS State message arrived");
                    DiagnosticConnectionServiceObserver.AcsMessagesSubject.OnNext(onMessage);
                }, 
                (onError) => {
                    DiagnosticConnectionServiceObserver.Notifications.OnNext(onError.Message);
                }, 
                //on complete 
                () => {
                    DiagnosticConnectionServiceObserver.Notifications.OnNext(string.Format("observation of acs events complete: {0}", url));
                }
            );

            hubConnection.Start();
        }
    }
}
