using Microsoft.AspNet.SignalR.Client;

namespace AndroAdmin.ThreatBoard.Models
{
    public class AcsConnectionStatusMessage 
    {
        public const string Online = "Online";
        public const string OffLine = "Offline";

        public string Url { get; set; }
        
        public string OldState { get; set; }
        public string NewState { get; set; }
    }

    public class Notification 
    {
        public string Message { get; set; }
    }
}