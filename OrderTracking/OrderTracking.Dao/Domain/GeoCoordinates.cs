using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderTracking.Dao.Domain
{
    public class GeoCoordinates : IGeoCoordinates
    {
        public GeoCoordinates()
        {
        }

        public GeoCoordinates(float longitude,float latitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        #region IGeoCoordinates Members

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        #endregion
    }
   
 /* todo: convert to struct
    public struct GeoCoordinates : IGeoCoordinates
    {

        //public GeoCoordinates()
        //{
        //}

        public GeoCoordinates(float longitude, float latitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        #region IGeoCoordinates Members

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        #endregion
    } */
}
