
using System.Collections.Generic;
using System;

namespace OrderTracking.Dao.Domain
{
    public class ClientMapData
    {
        public DeliveryCustomer Customer { get; set; }
        public DeliveryOrder DeliveryOrder { get; set; } // No longer used - retained for backwards compatibility
        public DeliveryPremises Premises { get; set; }
        public DeliveryDriver DeliveryDriver { get; set; }
        public string OrderStatus { get; set; } // No longer used - retained for backwards compatibility
        public string OrderStatusTime { get; set; } // No longer used - retained for backwards compatibility
        public string OrderProcessor { get; set; } // No longer used - retained for backwards compatibility
        public List<DeliveryOrder> DeliveryOrders { get; set; }
    }

    public class DeliveryCustomer : IGeoCoordinates
    {
        #region Constructors

        public DeliveryCustomer() { }

        public DeliveryCustomer(string name, float longitude, float latitude, string credentials)
        {
            this.Name = name;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Credentials = credentials;
        }

        #endregion Constructors

        #region Public Properties

        public string Name{get; set;}

        public string Credentials { get; set; }

        #endregion Public Properties

        #region IGeoCoordinates Members

        public float Longitude{get;set;}

        public float Latitude{get;set; }

        #endregion IGeoCoordinates Members
    }

    public class DeliveryOrder
    {
        public bool ProximityDelivered { get; set; }
        public List<string> Items { get; set; }
        public DateTime OrderTakenDateTime { get; set; }
        public DateTime DispatchedDateTime { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusTime { get; set; }
        public string OrderProcessor { get; set; }
        public List<DeliveryStatusUpdate> StatusUpdates { get; set; }

        public DeliveryOrder()
        {
            this.ProximityDelivered = false;
            this.Items = new List<string>();
        }
    }

    public class DeliveryStatusUpdate
    {
        public string Processor { get; set; }
        public DateTime Time { get; set; }
        public long StatusId { get; set; }
    }

    public class DeliveryPremises : IGeoCoordinates
    {
        #region Constructors

        public DeliveryPremises() { }

        public DeliveryPremises(string externalStoreId, string name, float longitude, float latitude)
        {
            this.ExternalStoreId = externalStoreId;
            this.Name = name;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        #endregion Constructors

        #region Public Properties

        public string ExternalStoreId { get; set; }

        public string Name{get;set; }

        #endregion Public Properties

        #region IGeoCoordinates Members

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        #endregion IGeoCoordinates Members
    }

    public class DeliveryDriver : IGeoCoordinates
    {
       public bool HasFix { get; set; }

        public DeliveryDriver()
        {
           this.HasFix = false;
        }

        #region IGeoCoordinates Members

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        #endregion IGeoCoordinates Members
    }

}
