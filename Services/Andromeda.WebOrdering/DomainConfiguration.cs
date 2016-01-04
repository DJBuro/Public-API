using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering
{
    public class DomainConfiguration
    {
        public string ApplicationId { get; set; }
        public List<string> SignpostServers { get; set; }
        public string HostHeader { get; set; }
        public Dictionary<string, Email> TemplateEmails { get; set; }
        public string DataCashPageSetId { get; set; }
        public bool Is3DSecureEnabled { get; set; }
    }
}