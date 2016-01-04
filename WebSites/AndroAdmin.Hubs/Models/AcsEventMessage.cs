using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroAdmin.ThreatBoard.Models
{
    public class AcsEventMessage
    {
        public const string Online = "Online";
        public const string Offline = "Offline";
        public const string Reconnected = "reconnected";


        public DateTime TimeStamp { get; set; }

        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string Message { get; set; }

        public string Payload { get; set; }

        public string SourceHub { get; set; }

        public bool IsOnline()
        {
            return this.Payload.Equals(AcsEventMessage.Online, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsOffline()
        {
            return this.Payload.Equals(AcsEventMessage.Offline, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool HasReconnected() { return this.Message.EndsWith(AcsEventMessage.Reconnected, StringComparison.InvariantCultureIgnoreCase); }
    }
}
