using System.Collections.Generic;
using Newtonsoft.Json;

namespace AndroCloudDataAccess.Domain
{
    public class DayServiceTimes
    {
        [JsonProperty(PropertyName = "isOpenAllDay")]
        public bool IsOpenAllDay { get; set; }

        [JsonProperty(PropertyName = "isClosedAllDay")]
        public bool IsClosedAllDay { get; set; }

        [JsonProperty(PropertyName = "times", NullValueHandling = NullValueHandling.Ignore)]
        public List<TimeSpanBlock3> Times { get; set; }

        public DayServiceTimes()
        {
            this.IsOpenAllDay = false;
            this.IsClosedAllDay = false;
            this.Times = new List<TimeSpanBlock3>();
        }
    }
}
