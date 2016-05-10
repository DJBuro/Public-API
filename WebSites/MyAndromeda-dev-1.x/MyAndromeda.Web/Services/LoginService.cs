using MyAndromeda.Core;
using MyAndromeda.Core.User;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Logging;
using MyAndromeda.Framework.Mvc;
using MyAndromeda.Framework.Notification;
using System.Linq;
using System.Web.Mvc;

namespace MyAndromeda.Web.Services
{
    public class LoginService : ILoginService 
    {
        private readonly IMyAndromedaLogger logger;
        private readonly INotifier notifier;
        private readonly IUserChainsDataService userChainsDataService;
        private readonly IUserSitesDataService userSitesDataService;
        private readonly IUserDataService userDataService;


        public LoginService(
            IMyAndromedaLogger logger, 
            INotifier notifier, 
            IUserChainsDataService userChainsDataService, 
            IUserSitesDataService userSitesDataService, 
            IUserDataService userDataService) 
        {
            this.userDataService = userDataService;
            this.userSitesDataService = userSitesDataService;
            this.userChainsDataService = userChainsDataService;
            this.notifier = notifier;
            this.logger = logger;
        }

        public ActionResult LoggedIn(IRedirectController controller, string userName, string returnUrl)
        {
            // Get the users details
            this.logger.Debug("Get the user {0}", userName);
            MyAndromedaUser myAndromedaUser = this.userDataService.GetByUserName(userName);
            //this.DataAccessFactory.MyAndromedaUserDataAccess.GetByUsername(model.UserName, out myAndromedaUser);

            if (!string.IsNullOrWhiteSpace(returnUrl) && returnUrl != "/") { 
                return controller.Redirect(returnUrl); 
            }

            //go here first - if applicable
            var chains = this.userChainsDataService.GetChainsForUser(myAndromedaUser.Id);
            if (chains.Any())
                return controller.RedirectToAction("Index", "Chains", new { });

            //go here second if applicable 
            Data.Domain.SiteDomainModel[] accessibleSiteList = this.userSitesDataService.GetSitesDirectlyLinkedToTheUser(myAndromedaUser.Id).ToArray();

            if (accessibleSiteList.Length == 0)
            {
                this.notifier.Error("You do not have access to any sites.");
            }
            else
            {
                
                // Does the user have access to a single site?
                if (accessibleSiteList.Length == 1)
                {
                    Data.Domain.SiteDomainModel site = accessibleSiteList.First();

                    return controller.RedirectToAction("Index", "Store", new { Area = "Reporting", ChainId = site.ChainId, ExternalSiteId = site.ExternalSiteId });
                    //return controller.RedirectToAction("Index", "Site", new { id = site.CustomerSiteId, chainId = site.ChainId });
                }
                else
                {
                    return controller.RedirectToAction("Index", "Sites", new { });
                }
            }

            return null;
        }
    }
}