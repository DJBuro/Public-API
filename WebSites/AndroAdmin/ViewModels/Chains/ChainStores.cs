using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AndroAdminDataAccess.Domain;

namespace AndroAdmin.ViewModels.Chains
{
    public class ChainStoresViewModel
    {
        public string ChainName { set; get; }
        public IList<Store> Stores{set; get;}
        public IList<Store> MasterMenuList { set; get; }
        public int MasterMenuId { set; get; }
    }

    public class ChainStoresExtension
    {
        public ChainStoresViewModel ToViewModel(IList<Store> stores) 
        {
            return new ChainStoresViewModel { 

            };
        }

    }
}