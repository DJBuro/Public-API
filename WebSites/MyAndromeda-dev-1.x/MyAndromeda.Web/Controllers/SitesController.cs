using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Data.DataAccess.Sites;
using MyAndromeda.Data.Domain;
using System.Collections.Generic;
using MyAndromeda.Core.Linq;

namespace MyAndromeda.Web.Controllers
{
    [MyAndromedaAuthorize]
    public class SitesController : Controller
    {
        private readonly ICurrentChain currentChain;
        private readonly IUserSitesDataService userSitesDataService;
        private readonly ISiteDataService siteDataService;
        private readonly WorkContextWrapper workContextWrapper;

        public SitesController(WorkContextWrapper workContextWrapper,
            ICurrentChain currentChain,
            IUserSitesDataService userSitesDataService,
            ISiteDataService siteDataService,
            IMyAndromedaLogger logger,
            INotifier notifier)
        {
            this.currentChain = currentChain;
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
            ICurrentUser currentUser = this.workContextWrapper.Current.CurrentUser;
            //var currentChain = this.workContextWrapper.Current.CurrentChain;

            if (currentUser.FlattenedChains.Any()) 
            { 
                int[] chainsIds = currentUser.FlattenedChains.Select(e => e.Id).ToArray();
                SiteDomainModel[] stores = this.siteDataService.List(e => chainsIds.Contains(e.ChainId)).ToArray();
                return View(stores);
            }

            SiteDomainModel[] chainsSites = currentUser.AccessibleSites;

            return View(chainsSites);
        }

        public JsonResult ListReadonly(int? chainId, string text)
        {
            ICurrentUser currentUser = this.workContextWrapper.Current.CurrentUser;

            if (!currentUser.Available) { return Json(Enumerable.Empty<object>(), JsonRequestBehavior.AllowGet); }

            ChainDomainModel[] userChains = currentUser.FlattenedChains;

            if (chainId.HasValue)
            {
                if (!currentUser.FlattenedChains.Any(e => e.Id == chainId))
                {
                    throw new Exception(message: "no"); 
                }
            }


            if (!userChains.Any()) 
            {
                //load only the stores that the user associated with. 

                SiteDomainModel[] associatedStores = currentUser.AccessibleSites;

                if (string.IsNullOrWhiteSpace(text)) 
                {
                    return Json(associatedStores, JsonRequestBehavior.AllowGet);
                }
            }

            int[] associatedChainIds;
            if (this.currentChain.Available) {
                IEnumerable<ChainDomainModel> withinChain = currentUser.FlattenedChains.Where(e => e.Id == chainId);
                //change the tree (branch) to a flat level
                IEnumerable<ChainDomainModel> flattened = withinChain.Flatten(e => e.Items);
                associatedChainIds = flattened.Select(e => e.Id).ToArray();
            }
            else {
                associatedChainIds = currentUser.FlattenedChains.Select(e => e.Id).ToArray();

            }

            IEnumerable<SiteDomainModel> siteQuery = this.siteDataService.List(e => associatedChainIds.Contains(e.ChainId));
            //var siteQuery = string.IsNullOrWhiteSpace(text) ? 
            //    Enumerable.Empty<Site>() : 
            //    this.siteDataService.List(e => associatedChainIds.Contains(e.ChainId) && e.ClientSiteName.Contains(text));

            return Json(siteQuery, JsonRequestBehavior.AllowGet);
        }
    }
}
