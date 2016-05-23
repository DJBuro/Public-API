using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Postcodes.Models
{
    public class PostcodeStuff 
    {
        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("quality")]
        public int Quality { get; set; }

        [JsonProperty("eastings")]
        public string Eastings { get; set; }

        [JsonProperty("northings")]
        public string Northings { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("County")]
        public string County { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }
    }

    public class PostcodeDataResult
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("result")]
        public PostcodeStuff Result { get; set; }
    }

    public class GeoLocation 
    {
        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }
    }

    public class GeoLocationResult 
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("result")]
        public GeoLocation Result { get; set; }
    }
}
