using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignpostDataAccessLayer.Models
{
    public class HostV2
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string TypeId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Version { get; set; }
        public int Order { get; set; }
    }
}