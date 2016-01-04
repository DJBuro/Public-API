using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.ViewModels.HostV2
{
    public class HostListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class HostApplicationListViewModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExternalApplicationId { get; set; }
    }

    public class HostTypeDetailViewModel 
    {
        public Guid Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool OptInOnly { get; set; }
        public bool Public { get; set; }
        public int Version { get; set; }
        public Guid HostTypeId { get; set; }
    }

    
}