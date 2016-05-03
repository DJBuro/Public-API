using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Data.DataAccess.Chains;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Web.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic;

namespace MyAndromeda.Web.Controllers
{
    [MyAndromedaAuthorizeAttribute]
    public class ChainsController : Controller
    {
        private readonly ICurrentChain currentChain;
        private readonly INotifier notifier;
        private readonly ITranslator translator; 
        private readonly IUserChainsDataService userChainDataService;
        private readonly ICurrentUser currentUser;

        private readonly IChainDataService chainDataService;

        private readonly ISiteDataService siteDataService;

        public ChainsController(ICurrentChain currentChain,
            ICurrentUser currentUser,
            IUserChainsDataService userChainDataService,
            IChainDataService chainDataService,
            ISiteDataService siteDataService,
            INotifier notifier,
            ITranslator translator) 
        {
            this.translator = translator;
            this.notifier = notifier;
            this.currentChain = currentChain;
            this.chainDataService = chainDataService;
            this.siteDataService = siteDataService;
            this.currentUser = currentUser;
            this.userChainDataService = userChainDataService;
        }

        //
        // GET: /Chains/
        public ActionResult Index()
        {
            var chains = currentUser.FlattenedChains;
            
            //no need to see chains here.
            if (chains.Length == 1) 
            {
                var chain = chains.Single();
                return RedirectToAction(actionName: "Index", controllerName: "Reports", routeValues: new { ChainId = chain.Id, Area = "ChainReporting" });    
            }

            int[] chainIds = currentUser.FlattenedChains.Select(e=> e.Id).ToArray();
            Dictionary<int, Data.Domain.Site[]> chainStoreList = this.siteDataService.List(e => chainIds.Contains(e.ChainId)).ToLookup(e => e.ChainId).ToDictionary(e => e.Key, e => e.ToArray());

            foreach (var storeList in chainStoreList) 
            {
                var chain = currentUser.FlattenedChains.FirstOrDefault(e => e.Id == storeList.Key);
                chain.Stores = storeList.Value;
            }

            var model = new MyAndromeda.Web.ViewModels.ChainListViewModel() 
            {
                FlatternedChains = chains,
                TreeViewChain = currentUser.AccessibleChains
            };

            return View(model);
        }

        /// <summary>
        /// Lists the sites.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <param name="deepSearch">deep search on chain tree.</param>
        /// <returns></returns>
        public JsonResult ListSites(int chainId, bool deepSearch = false) 
        {
            var sites = this.chainDataService.GetSiteList(chainId);

            return Json(sites.Select(e=> new {
                e.ExternalSiteId,
                e.ClientSiteName,
                e.ExternalName
            }));
        }

        [HttpPost]
        public JsonResult ListSiteGrid([DataSourceRequest]DataSourceRequest request, int chainId) 
        {
            var sites = this.chainDataService.GetSiteList(chainId);

            return Json(sites.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCulture(string returnUrl) 
        {
            var model = new ChainUpdateCultureViewModel();
            model.Chain = this.currentChain.Chain;
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("UpdateCulture")]
        public ActionResult UpdateCulturePost() 
        {
            if (!this.ModelState.IsValid) { return View(); }

            var model = new ChainUpdateCultureViewModel();
            model.Chain = this.currentChain.Chain;
            
            this.UpdateModel(model);

            this.chainDataService.Update(model.Chain);

            this.notifier.Notify(translator.T("Your chain details has been updated"));

            if (string.IsNullOrWhiteSpace(model.ReturnUrl)) { return RedirectToAction("Index"); }

            return Redirect(model.ReturnUrl);
        }

        public JsonResult List([DataSourceRequest]DataSourceRequest request, int? userId) 
        {
            if (!userId.HasValue) 
            {
                var models = currentUser.FlattenedChains;

                return Json(models.ToDataSourceResult(request, this.ModelState));
            }

            var userList = userChainDataService.GetChainsForUser(userId.Value);
            var userDefined = CurrentUser.FlatternChains(userList);

            return Json(userDefined.ToDataSourceResult(request, this.ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListReadonly(string text)
        {
            if (currentUser.Available)
            {
                var models = currentUser.FlattenedChains;
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            return Json(Enumerable.Empty<object>(), JsonRequestBehavior.AllowGet);
        }

    }
}
