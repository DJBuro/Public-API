using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.Model
{
    public class ACSApplicationModel
    {
        public Partner Partner { get; set; }
        public ACSApplication ACSApplication { get; set; }
        public List<StoreModel> Stores { get; set; }

        public IList<AndroAdminDataAccess.Domain.Environment> EnvironmentsList { set; get; }

        public ACSApplicationModel()
        {
            this.Stores = new List<StoreModel>();
        }
    }
}