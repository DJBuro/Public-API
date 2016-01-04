using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.Model
{
    public class FTPSiteViewModel
    {
        public FTPSite FtpSite { get; set; }
        public IList<FTPSiteType> FtpSiteTypes { get; set; }
        public string FTPSiteType { get; set; }
    }
}