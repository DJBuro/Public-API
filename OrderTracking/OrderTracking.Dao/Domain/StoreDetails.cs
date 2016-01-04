using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderTracking.Dao.Domain
{
    public class StoreDetails : IGeoCoordinates, IGeoAddress
    {
        public StoreDetails()
        {
        }

        public StoreDetails(string externalStoreId,string storeName, string accountUserName, string accountPassword, Int16 deliveryRadius, bool gpsEnabled)
        {
            this.ExternalStoreId = externalStoreId;
            this.StoreName = storeName;
            this.AccountUserName = accountUserName;
            this.AccountPassword = accountPassword;
            this.GpsEnabled = gpsEnabled;
            this.DeliveryRadius = deliveryRadius;
        }

        public StoreDetails(string externalStoreId, string storeName, string accountUserName, string accountPassword, Int16 deliveryRadius, bool gpsEnabled, float longitude, float latitude)
        {
            this.ExternalStoreId = externalStoreId;
            this.StoreName = storeName;
            this.AccountUserName = accountUserName;
            this.AccountPassword = accountPassword;
            this.GpsEnabled = gpsEnabled;
            this.DeliveryRadius = deliveryRadius;
            Longitude = longitude;
            Latitude = latitude;
        }

        public StoreDetails(string externalStoreId, string storeName, string accountUserName, string accountPassword, Int16 deliveryRadius, bool gpsEnabled, string houseNumber, string buildingName, string roadName, string townCity, string postCode, string country)
        {
            this.ExternalStoreId = externalStoreId;
            this.StoreName = storeName;
            this.AccountUserName = accountUserName;
            this.AccountPassword = accountPassword;
            this.GpsEnabled = gpsEnabled;
            this.DeliveryRadius = deliveryRadius;

            HouseNumber = houseNumber;
            BuildingName = buildingName;
            RoadName = roadName;
            TownCity = townCity;
            PostCode = postCode;
            Country = country;
        }


        public string ExternalStoreId { get; set; }
        public string StoreName { get; set; }
        public string AccountUserName { get; set; }
        public string AccountPassword { get; set; }
        public bool GpsEnabled { get; set; }
        public Int16 DeliveryRadius { get; set; }


        #region IGeoAddress Members

        public string HouseNumber { get; set; }
        public string BuildingName { get; set; }
        public string RoadName { get; set; }
        public string TownCity { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        #endregion

        #region IGeoCoordinates Members

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        #endregion



    }
}
