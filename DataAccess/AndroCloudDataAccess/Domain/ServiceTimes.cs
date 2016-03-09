using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AndroCloudDataAccess.Domain
{
    public class ServiceTimes
    {
        [JsonProperty(PropertyName = "occasion")]
        public string Occasion { get; set; }

        [JsonProperty(PropertyName = "monday")]
        public DayServiceTimes Monday { get; set; }

        [JsonProperty(PropertyName = "tuesday")]
        public DayServiceTimes Tuesday { get; set; }

        [JsonProperty(PropertyName = "wednesday")]
        public DayServiceTimes Wednesday { get; set; }

        [JsonProperty(PropertyName = "thursday")]
        public DayServiceTimes Thursday { get; set; }

        [JsonProperty(PropertyName = "friday")]
        public DayServiceTimes Friday { get; set; }

        [JsonProperty(PropertyName = "saturday")]
        public DayServiceTimes Saturday { get; set; }

        [JsonProperty(PropertyName = "sunday")]
        public DayServiceTimes Sunday { get; set; }

        public ServiceTimes()
        {
            this.Monday = new DayServiceTimes();
            this.Tuesday = new DayServiceTimes();
            this.Wednesday = new DayServiceTimes();
            this.Thursday = new DayServiceTimes();
            this.Friday = new DayServiceTimes();
            this.Saturday = new DayServiceTimes();
            this.Sunday = new DayServiceTimes();
        }
    }
}
