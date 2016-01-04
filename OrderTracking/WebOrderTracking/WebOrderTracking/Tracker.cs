using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.WebOrderTracking
{
    internal class Tracker
    {
        public string Name { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public DateTime LastUpdated { get; set; }

        public Tracker(string name, float? latitude, float? longitude)
        {
            this.Name = name;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.LastUpdated = DateTime.Now;
        }
    }
}
