using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AndroCloudServices.Domain
{
    [XmlRoot("Order")]
    public class OrderStatusUpdate
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "driver")]
        public string Driver { get; set; }

        [JsonProperty(PropertyName = "driverId")]
        public int? DriverId { get; set; }

        [JsonProperty(PropertyName = "driverMobileNumber")]
        public string DriverMobileNumber { get; set; }
                      
        [JsonProperty(PropertyName = "ticketNumber")]
        public int? TicketNumber { get; set; }

        [JsonProperty(PropertyName = "bags")]
        public int? Bags { get; set; }
    }
}
