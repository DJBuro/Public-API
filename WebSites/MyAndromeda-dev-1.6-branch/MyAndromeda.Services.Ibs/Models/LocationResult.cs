using System.Collections.Generic;

namespace MyAndromeda.Services.Ibs.Models
{
    public class Locations : List<LocationResult>
    {
        
    }

    public class LocationResult 
    {
        /// <summary>
        /// Gets or sets the location code.
        /// </summary>
        /// <value>The location code.</value>
        public string LocationCode { get; set; }
        public string Description { get; set; }
        public string SiteReference { get; set; }
        public string Contact { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool DeliveryOffline { get; set; }
        public bool CollectionOffline { get; set; }

        //there is more ... got bored. 
    }
}