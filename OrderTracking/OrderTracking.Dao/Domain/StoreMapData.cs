using System.Collections.Generic;
using System;

namespace OrderTracking.Dao.Domain
{
    public class StoreMapData
    {
        public List<PremisesDriver> PremisesDrivers { get; set; }
        public List<Delivery> OutStandingDeliveries { get; set; }

    }

    public class PremisesDriver: IGeoCoordinates
    {
        public string Id { get; set; }
        public string ExternalId { get; set; }
        public bool HasFix { get; set; }
        public int BatteryLevel { get; set; }
        public DateTime? LastUpdate { get; set; }
        public double? Speed { get; set; }
        public List<Delivery> Deliveries { get; set; }

        #region IGeoCoordinates Members

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        #endregion
        
        public PremisesDriver()
        {
        }

        public PremisesDriver(
            string externalId, 
            bool hasFix, 
            int batteryLevel, 
            DateTime? lastUpdate,
            float? speed,
            float longitude, 
            float latitude, 
            List<Delivery> deliveries)
        {
            ExternalId = externalId;
            HasFix = hasFix;
            BatteryLevel = batteryLevel;
            LastUpdate = lastUpdate;
            Speed = speed;
            Longitude = longitude;
            Latitude = latitude;
            Deliveries = deliveries;
        }       
    }

    public class Delivery
    {
        public bool ProximityDelivered { get; set; }
        public string ExternalId { get; set; }
        public long StatusId { get; set; }
        public long OrderDispatchedTicks { get; set; }
        //public List<string> Items { get; set; }

        public Delivery()
        {
        }

        public Delivery(string externalId, bool proximityDelivered, long statusId, long orderDispatchedTicks)
        {
            this.ExternalId = externalId;
            this.ProximityDelivered = proximityDelivered;
            this.StatusId = statusId;
            this.OrderDispatchedTicks = orderDispatchedTicks;
        }
    }
}
