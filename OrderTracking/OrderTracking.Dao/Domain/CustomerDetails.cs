using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderTracking.Dao.Domain
{
    /// <summary>
    /// Only used to Add orders
    /// And to geo-code the address
    /// </summary>
    public class CustomerDetails: IGeoCoordinates, IGeoAddress
    {
        protected string _externalId;
        protected string _name;
        protected string _credentials;
        		
        public CustomerDetails() { }
  
        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="externalId"></param>
        /// <param name="name"></param>
        /// <param name="credentials"></param>
        /// <param name="houseNumber"></param>
        /// <param name="buildingName"></param>
        /// <param name="roadName"></param>
        /// <param name="townCity"></param>
        /// <param name="postCode"></param>
        /// <param name="country"></param>
        public CustomerDetails(string externalId, string name, string credentials, string houseNumber, string buildingName, string roadName, string townCity, string postCode, string country)
		{
            this._externalId = externalId;
			this._name = name;
			this._credentials = credentials;

            HouseNumber = houseNumber;
            BuildingName = buildingName;
            RoadName = roadName;
            TownCity = townCity;
            PostCode = postCode;
            Country = country;
		}

        /// <summary>
        /// Optional constructor if they are sending in GPS coords
        /// </summary>
        /// <param name="externalId"></param>
        /// <param name="name"></param>
        /// <param name="credentials"></param>
        /// <param name="houseNumber"></param>
        /// <param name="buildingName"></param>
        /// <param name="roadName"></param>
        /// <param name="townCity"></param>
        /// <param name="postCode"></param>
        /// <param name="country"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
       public CustomerDetails(string externalId, string name, string credentials, string houseNumber, string buildingName, string roadName, string townCity, string postCode, string country, float longitude, float latitude)
        {
            this._externalId = externalId;
            this._name = name;
            this._credentials = credentials;

            HouseNumber = houseNumber;
            BuildingName = buildingName;
            RoadName = roadName;
            TownCity = townCity;
            PostCode = postCode;
            Country = country;

            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        public string ExternalId
        {
            get { return _externalId; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ExternalId", value, value.ToString());
                _externalId = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
                _name = value;
            }
        }

        public string Credentials
        {
            get { return _credentials; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Credentials", value, value.ToString());
                _credentials = value;
            }
        }

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



    public class OrderItem
    {
        public OrderItem()
        {
        }

        public OrderItem(int quantity,string name)
        {
            this.Quantity = quantity;
            this.Name = name;
        }

        public int Quantity { get; set; }
        public string Name { get; set; }

    }
}
