using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Models.Services
{
    public class SignPostEntries : List<SignPostEntry> { }
    public class SignPostEntry
    {
        public string Type { get; set; }
        public string Url { get; set; }
        public int Version { get; set; }
        public int Order { get; set; }
    }
}