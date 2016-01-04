using System;
namespace OrderTracking.Gps.Dao.Domain
{
    public class Tracker
    {
        public string Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? BatteryLevel { get; set; }
        public bool HasFix { get; set; }
        public DateTime? LastUpdate { get; set; }
        public double? Speed { get; set; } 
    }
}
