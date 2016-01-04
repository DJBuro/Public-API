using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignpostDataAccessLayer.Models
{
    public class HostV1
    {
        public string Url { get; set; }
        public string SignalRUrl { get; set; }
        public int Order { get; set; }
    }
}