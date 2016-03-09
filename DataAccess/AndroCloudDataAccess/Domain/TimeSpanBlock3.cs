using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AndroCloudDataAccess.Domain
{
    public class TimeSpanBlock3
    {
        [JsonIgnore]
        [XmlIgnore]
        public Guid ID { get; set; }

        [JsonProperty(PropertyName = "startTime")]
        public string StartTime { get; set; }

        [JsonProperty(PropertyName = "durationMinutes")]
        public int DurationMinutes { get; set; }
    }
}
