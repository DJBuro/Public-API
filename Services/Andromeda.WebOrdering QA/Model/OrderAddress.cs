using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class OrderAddress
    {
        public string Org1 { get; set; }
        public string Org2 { get; set; }
        public string Prem1 { get; set; }
        public string Prem2 { get; set; }
        public string RoadNumber { get; set; }
        public string RoadName { get; set; }
        public string Town { get; set; }
        public string UserLocality { get; set; }
        public string ZipCode { get; set; }
        public string Directions { get; set; }
    }
}