using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Notification;
using MyAndromedaDataAccess.Domain;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Web.Controllers
{
    [MyAndromedaAuthorizeAttribute]
    public class SitesController : Controller
    {
        private readonly IUserSitesDataService userSitesDataService;
        private readonly ISiteDataService siteDataService;
        private readonly WorkContextWrapper workContextWrapper;

        public SitesController(WorkContextWrapper workContextWrapper,
            IUserSitesDataService userSitesDataService,
            ISiteDataService siteDataService,
            IMyAndromedaLogger logger,
            INotifier notifier)
        {
            this.siteDataService = siteDataService;
            this.workContextWrapper = workContextWrapper;
            this.userSitesDataService = userSitesDataService;
            this.Notifier = notifier;
            this.Logger = logger;
        }
        
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IMyAndromedaLogger Logger { get; private set; }

        /// <summary>
        /// Gets or sets the notifier.
        /// </summary>
        /// <value>The notifier.</value>
        public INotifier Notifier { get; private set; }

        //
        // GET: /Sites/
        public ActionResult Index()
        {
            // Get the users details
            var currentUser = this.workContextWrapper.Current.CurrentUser;
            var currentChain = this.workContextWrapper.Current.CurrentChain;

            if (currentChain.Available)
            {
                var chainsSites = currentChain.SitesBelongingToChain;
                return View(chainsSites);
            }

            if (currentUser.FlattenedChains.Any()) 
            { 
                var chainsIds = currentUser.FlattenedChains.Select(e => e.Id).ToArray();
                var stores = this.siteDataService.List(e => chainsIds.Contains(e.ChainId)).ToArray();
                return View(stores);
            }

            var userStors = currentUser.AccessibleSites;

            return View(userStors);
            
            //return RedirectToAction("Index", "Chains");
        }

        public JsonResult ListReadonly(string text)
        {
            var currentUser = this.workContextWrapper.Current.CurrentUser;

            if (!currentUser.Available) { return Json(Enumerable.Empty<object>(), JsonRequestBehavior.AllowGet); }

            var userChains = currentUser.FlattenedChains;

            if (!userChains.Any()) 
            {
                //load only the stores that the user associated with. 

                var associatedStores = currentUser.AccessibleSites;

                if (string.IsNullOrWhiteSpace(text)) 
                {
                    return Json(associatedStores, JsonRequestBehavior.AllowGet);
                }
            }

            var associatedChainIds = currentUser.FlattenedChains.Select(e=> e.Id).ToArray();

            var siteQuery = this.siteDataService.List(e => associatedChainIds.Contains(e.ChainId));
            //var siteQuery = string.IsNullOrWhiteSpace(text) ? 
            //    Enumerable.Empty<Site>() : 
            //    this.siteDataService.List(e => associatedChainIds.Contains(e.ChainId) && e.ClientSiteName.Contains(text));

            return Json(siteQuery, JsonRequestBehavior.AllowGet);
        }
    }
}
