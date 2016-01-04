using MVCAndromeda.Context;
using MVCAndromeda.Models;
using MVCAndromeda.ViewModels;
using MyAndromeda.Framework.Contexts;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCAndromeda.Controllers
{
    [Authorize]
    public class StoreDataController : Controller
    {
        private readonly ApplicationContext applicationContext;
        private readonly ICurrentUser currentUser;

        private readonly DateTime today;
        private readonly bool isSessionExpired;

        public static readonly string[,] timeGrid = { { "Today", "Yesterday" }, { "Week to Date", "Last Week" }, 
                                                    { "Month To Date", "Last Month" }, { "Year To Date", "Last Year" } };
        public static readonly string[,] shortTimeGrid = { { "TD", "YD" }, { "WTD", "LW" }, 
                                                    { "MTD", "LM" }, { "YTD", "LY" } };

        private static readonly string defaultCountry = "United Kingdom";
        public static readonly string noRecord = "-";
        //------------------------------------------------------------------------------------
        public StoreDataController(ApplicationContext applicatinoContext, ICurrentUser currentUser)
        {
            this.applicationContext = applicatinoContext;
            today = applicatinoContext.Date;
            this.currentUser = currentUser;
        }

        //------------------------Store List with data-----------------------------------
        public ActionResult Index(string chosenCountry, string term = "Day", int unitsAgo = 0,
            string time = "Today", int orderHow = 1, int orderWhat = 0)
        {
            if (isSessionExpired) return RedirectToAction("LogOff", "Account", routeValues: new { i = 0 });

            string cachekey = string.Format("owner{0}{1}{2}", this.applicationContext.FirstName, term, unitsAgo);

            var owner = this.applicationContext.Owner;
            
            
            if (HttpRuntime.Cache[cachekey] == null)
            {
                CubeAdapter.fillStoreMeasures(today, owner, term, unitsAgo);
            }
            owner.orderby(orderHow, orderWhat);
            
            ViewBag.TimeDesc = time + ": " + DateUtilities.getDateDescription(today, term, unitsAgo);
            ViewBag.Time = time;
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.orderHow = orderHow;
            ViewBag.orderWhat = orderWhat;

            string country = (chosenCountry != null) ? chosenCountry :
                (owner.Countries.Contains(defaultCountry) ? defaultCountry : owner.Countries[0]);

            ViewBag.country = country;
            ViewBag.chosenCountry = new SelectList(owner.Countries, country);

            return View(owner);
        }
        //-----------------------One store with data list for timegrid------------------------------------------
        public ActionResult Individual(string storeName, string country, string term = "Day",
            int unitsAgo = 0, string time = "Today", int orderHow = 1, int orderWhat = 0)
        {
            if (isSessionExpired || IsStoreNameHighjacked(storeName)) return RedirectToAction("LogOff", "Account", routeValues: new { i = 0 });

            Store store = FillStore(today, storeName, country, term, unitsAgo, this.applicationContext.FirstName);

            ViewBag.TimeDesc = time + ": " + DateUtilities.getDateDescription(today, term, unitsAgo);
            ViewBag.Time = time;
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.orderHow = orderHow;
            ViewBag.orderWhat = orderWhat;
            ViewBag.country = country;

            var viewModel = new IndividualStoreViewModel 
            { 
                Store = store,
                User = this.currentUser.User
            };

            return View(viewModel);
        }
        //--------------------plotting measure for present and past (storeMeasureIndex) for chosen store----------------
        public ActionResult StorePlot(string storeName, string country, int storeMeasureIndex, string term, int unitsAgo = 0)
        {
            if (isSessionExpired || IsStoreNameHighjacked(storeName)) return RedirectToAction("LogOff", "Account", routeValues: new { i = 0 });

            if (term == null) term = DateUtilities.weekDayAveragingPeriod[0];

            Store storePresent;
            Store storePast;

            storePresent = FillStore(today, storeName, country, term, unitsAgo, this.applicationContext.FirstName);
            storePast = FillStore(today, storeName, country, term, unitsAgo + 1, this.applicationContext.FirstName);

            string term1 = term.Substring(term.LastIndexOf(' ') + 1);// last word
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.TimeDesc = term1 + ": " + DateUtilities.getDateDescription(today, term1, unitsAgo, true) + " vs. "
                + DateUtilities.getDateDescription(today, term1, unitsAgo + 1, true);
            ViewBag.storeMeasureIndex = storeMeasureIndex;
            ViewBag.country = country;

            string averagingPeriod = term.Substring(term.LastIndexOf(' ') + 1); //last word
            var storePlotData = PlotMaker.getStorePlotData(new Store[] { storePresent, storePast },
                                new string[] { "This " + averagingPeriod, "Last " + averagingPeriod }, storeMeasureIndex, term);

            var viewModel = new StorePlotViewModel() 
            { 
                User = this.currentUser.User,
                Data = storePlotData
            };

            return View(viewModel);
        }

        //-----------------------fill store with data and save in cache or just get from cache------------------------------
        private Store FillStore(DateTime today, string storeName, string country, string term, int unitsAgo, string ownerName)
        {
            string storeCacheKey = string.Format("store.{0}.{1}.{2}.{3}.{4}", ownerName, storeName, country, term, unitsAgo);
            string ownerCacheKey = string.Format("owner{0}{1}{2}", ownerName, term, unitsAgo);

            if (HttpRuntime.Cache[storeCacheKey] != null)
            {
                return (Store)HttpRuntime.Cache[storeCacheKey];
            }
            else if (HttpRuntime.Cache[ownerCacheKey] != null)
            {
                return ((Owner)HttpRuntime.Cache[ownerCacheKey]).Stores[storeName];
            }
            else
            {
                Store store = CubeAdapter.fillStoreMeasures(today, storeName, country, term, unitsAgo);

                AddToCache(storeCacheKey, store);
                return store;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private void AddToCache(string cachekey, object obj)
        {
            HttpRuntime.Cache.Add(cachekey, obj, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                new TimeSpan(0, 1, 0), System.Web.Caching.CacheItemPriority.Default, null);
        }
        //---------------------SECURITY-------------------------------------
        private bool IsStoreNameHighjacked(string storeName)
        {
            return !this.applicationContext.Owner.hasStore(storeName);
        }
    }
}
