using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Data.AcsServices;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Menus.Services.Data;
using MyAndromeda.Web.Areas.Menu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Menu.Controllers
{
    public class MenuNavigationController : Controller
    {
        private readonly ICurrentSite currentSite;
        private readonly INotifier notifier;
        private readonly IMyAndromedaLogger logger;
        private readonly IGetAcsAddressesService acsAddresses;
        private readonly IAcsMenuServiceAsync acsMenuServiceService;
        private readonly IAccessDbMenuVersionDataService accessDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuNavigationController" /> class.
        /// </summary>
        /// <param name="workContextWrapper">The work context wrapper.</param>
        /// <param name="menuService">The menu service.</param>
        /// <param name="notifier">The notifier.</param>
        /// <param name="logger">The logger.</param>
        public MenuNavigationController(ICurrentSite currentSite,
            INotifier notifier,
            IMyAndromedaLogger logger,
            IGetAcsAddressesService acsAddresses,
            IAcsMenuServiceAsync acsMenuServiceService,
            IAccessDbMenuVersionDataService accessDatabase) 
        {
            this.accessDatabase = accessDatabase;
            this.acsMenuServiceService = acsMenuServiceService;
            this.currentSite = currentSite;
            this.acsAddresses = acsAddresses;
            this.notifier = notifier;
            this.logger = logger;
        }

        public ActionResult Unavailable() 
        {
            return View();
        }

        public async Task<ActionResult> Hierarchy() 
        {
            var available = accessDatabase.IsAvailable(this.currentSite.Store.AndromedaSiteId);

            if (!available) 
            {
                return View("Unavailable-Sorting");
            }
            
            var endpoints = await this.acsAddresses.GetMenuEndpointsAsync(this.currentSite.Store);
            var viewModel = new MenuIndexViewModel()
            {
                Endpoints = endpoints.ToArray(),
            };

            return View(viewModel); 
        }

        public async Task<ActionResult> Index()
        {
            var endpoints = await this.acsAddresses.GetMenuEndpointsAsync(this.currentSite.Store);

            if (endpoints == null || !endpoints.Any())
            {
                return this.RedirectToAction("Unavailable");
                //run away
            }

            var viewModel = new MenuIndexViewModel() 
            {
                Endpoints = endpoints.ToArray()
            };

            return View(viewModel); 
        }

        [HttpPost]
        public async Task<JsonResult> MenuItems([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "endpoints[]")]string[] endpoints) 
        {
            MyAndromedaMenu siteMenu = null;

            if (endpoints.Length == 0)
            {
                this.logger.Error("There are no endpoints provided with the request");
                this.ModelState.AddModelError("endpoints", new ArgumentException("Need at least one server endpoint", "endpoints"));

                var errorResult = siteMenu.MenuItems.ToDataSourceResult(request, this.ModelState);

                return Json(errorResult);
            }

            int retryCount = 2;
            while (retryCount > 0) 
            { 
                try
                {
                    siteMenu = await this.GetSiteMenu((endpoints));
                    retryCount = 0;
                }
                catch (Exception e) 
                {
                    this.logger.Error(e);
                    this.logger.Debug("Retry? " + (retryCount > 0));
                    retryCount--;
                }
            }

            if (siteMenu == null) 
            {
                this.notifier.Error("There was a problem fetching the menu. Try refreshing the page and if the problem persists ask for help");
            }

            return Json(siteMenu);
        }

        private async Task<MyAndromedaMenu> GetSiteMenu(string[] endpoints) 
        {
            try
            {
                var menu = await this.acsMenuServiceService.GetMenuDataFromEndpointsAsync(this.currentSite.AndromediaSiteId, this.currentSite.ExternalSiteId, endpoints);
             
                return menu;
            }
            catch (Exception e)
            {
                this.logger.Error(e);
            }

            return null;
        }
    }
}