using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.Domain;
using System.Web.Mvc;

namespace AndroAdmin.Model
{
    public class UpdateAMSandFTPSitesModel
    {
        public Store Store { get; set; }
        public SelectList AMSServers { get; set; }
        public SelectList FTPSites { get; set; }
        public Dictionary<int, List<StoreAMSServerFtpSite>> StoreAMSServerFtpSites { get; set; }
        public string AMSServerId { get; set; }
        public string FTPSiteId { get; set; }
        public string ErrorMessage { get; set; }
    }
}