using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace SignpostDataAccessLayer.Models
{
    public class ACSApplication
    {
        public int id { get; set; }
        public string name { get; set; }
        public Guid environmentId { get; set; }
        public string externalApplicationId { get; set; }
        public List<int> acsApplicationSites { get; set; }
    }
}
