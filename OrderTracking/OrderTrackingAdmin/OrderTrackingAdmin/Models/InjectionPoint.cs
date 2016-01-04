using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderTrackingAdmin.Models
{
    public class InjectionPoint
    {
        public string Id { get; set; }
        public string Prompt { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }

        public InjectionPoint()
        {
            this.Id = "";
            this.Prompt = "";
            this.Type = "";
            this.DefaultValue = "";
        }
    }
}
