using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.Model
{
    public class PartnerModel
    {
        public Partner Partner { get; set; }
        public IList<ACSApplication> ACSApplications { get; set; }
    }
}