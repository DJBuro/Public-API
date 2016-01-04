using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.WebOrderTracking
{
    public class Location
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        
        public Location()
        {
            this.Latitude = 0;
            this.Longitude = 0;
        }
    }
}
