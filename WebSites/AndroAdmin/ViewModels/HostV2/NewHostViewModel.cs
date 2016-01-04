using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.ViewModels.HostV2
{
    public class NewHostViewModel
    {
        public System.Guid HostTypeId { get; set; }
        public string Url { get; set; }
        public int Version { get; set; }
        public int Order { get; set; }
        public bool Public { get; set; }
        public bool OptInOnly { get; set; }
        public bool Enabled { get; set; }
    }

    public class EditHostViewModel 
    {
        public System.Guid Id { get; set; }
        public System.Guid HostTypeId { get; set; }
        public string Url { get; set; }
        public int Version { get; set; }
        public int Order { get; set; }
        public bool Public { get; set; }
        public bool OptInOnly { get; set; }
        public bool Enabled { get; set; }

        //private DateTime lastUpdatedUtc;

        //public DateTime LastUpdatedUtc
        //{
        //    get { return lastUpdatedUtc; }
        //    set { lastUpdatedUtc = new DateTime(value.Ticks, DateTimeKind.Utc); ; }
        //}

    }
}