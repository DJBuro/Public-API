using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using System;
using System.Linq;

namespace MyAndromeda.Web.Areas.Authorization.ViewModels
{
    public class NavigationViewModel
    {
        public IWorkContext WorkContext { get; set; }
        public IAuthorizer Authorizer { get; set; }

        public bool AccessToManySites()
        {
            if (!this.WorkContext.CurrentUser.Available)
                return false;

            return this.WorkContext.CurrentUser.AccessibleSites.Length > 1 || this.WorkContext.CurrentChain.SitesBelongingToChain.Length > 1;
                //Model.CurrentUser.AccessibleSites.Length > 1 ||  Model.CurrentChain.SitesBelongingToChain.Length > 1)
        }

        public bool AccessToManyChains() 
        {
            if (!this.WorkContext.CurrentUser.Available)
                return false;

            return this.WorkContext.CurrentUser.FlattenedChains.Length > 1;
        }
    }
}