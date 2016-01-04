using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PrivateSignpost.Models
{
    public class ACSApplicationSync
    {
        public int id { get; set; }
        public string name { get; set; }
        public string environmentId { get; set; }
        public string externalApplicationId { get; set; }
        public List<int> acsApplicationSites { get; set; }
    }
}
