using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace AndroAdmin.ThreatBoard.Models
{
    public class HubGroup : IDisposable
    {
        public HubGroup(string host) 
        {
            this.State = new HubState() { Host = host, Online = false };
        }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public HubConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        /// <value>The proxy.</value>
        public IHubProxy Proxy { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public HubState State { get; set; }

        public void Dispose()
        {
            Connection.Dispose();
            Proxy = null;
        }
    }

    public class HubState 
    {
        public HubState() { LastUpdatedAtUtc = DateTime.UtcNow; }

        public string Host { get; set; }
        public bool Online { get; set; }
        public DateTime LastUpdatedAtUtc { get; set; }

    }
}
