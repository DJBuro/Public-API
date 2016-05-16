using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AndroAdminDataAccess.Domain.WebOrderingSetup
{

    public class Website 
    {
        public int Id { get; set; }
        public string LiveDomainName { get; set; }
        public string PreviewDomainName { get; set; }
    }

}