using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroAdmin.ThreatBoard.Models
{
    public class Store
    {
        public Store() { this.Connections = new ConcurrentDictionary<string, ConnectionUpdate>(); }

        public int AndromedaSiteId { get; set; }

        public ConcurrentDictionary<string, ConnectionUpdate> Connections { get; set; }
        public DateTime LastUpdateUtc { get; set; }
    }

    public struct ConnectionUpdate
    {
        public DateTime? LastConnectedUtc { get; set; }
        public DateTime? LastDisconnectedUtc { get; set; }

        public bool Connected { get; set; }
    }
}
