using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;

namespace AndroCloudServices.Domain
{
    public class OrderHeader
    {
        public string Id { get; set; }
        public string ForDateTime { get; set; }
        public int OrderStatus { get; set; }
    }
}
