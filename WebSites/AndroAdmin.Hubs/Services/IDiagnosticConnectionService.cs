using System;
using System.Collections.Generic;
using AndroAdmin.ThreatBoard.Models;

namespace AndroAdmin.ThreatBoard.Services
{
    public interface IDiagnosticConnectionService 
    {
        /// <summary>
        /// Gets the get notifications.
        /// </summary>
        /// <value>The get notifications.</value>
        IObservable<string> GetNotifications { get; }

        /// <summary>
        /// Gets the get acs hub event messages.
        /// </summary>
        /// <value>The get acs event messages.</value>
        IObservable<AcsEventMessage> GetAcsHubEventMessages { get; }

        /// <summary>
        /// Gets the get connection status.
        /// </summary>
        /// <value>The get connection status.</value>
        IObservable<AcsConnectionStatusMessage> GetConnectionStatus { get; }

        /// <summary>
        /// Gets the hub connections.
        /// </summary>
        /// <value>The hub connections.</value>
        IDictionary<string, HubGroup> HubConnections { get; }

        /// <summary>
        /// Gets the store changes.
        /// </summary>
        /// <value>The store changes.</value>
        IObservable<Models.Store> StoreChanges { get; }

        /// <summary>
        /// Gets the store status.
        /// </summary>
        /// <value>The store status.</value>
        IDictionary<string, Store> StoreStatus { get; }
    }
}