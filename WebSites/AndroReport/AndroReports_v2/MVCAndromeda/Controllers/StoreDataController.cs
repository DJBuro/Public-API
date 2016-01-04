using MVCAndromeda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace MVCAndromeda.Controllers
{
    [Authorize]
    public class StoreDataController : Controller
    {
        private CubeAdapter cube;
        private readonly string ownerName;
        private readonly DateTime today;
        private readonly bool isSessionExpired;

        public static readonly string[,] timeGrid = { { "Today", "Yesterday" }, { "Week to Date", "Last Week" }, 
                                                    { "Month To Date", "Last Month" }, { "Year To Date", "Last Year" } };
        public static readonly string[,] shortTimeGrid = { { "TD", "YD" }, { "WTD", "LW" }, 
                                                    { "MTD", "LM" }, { "YTD", "LY" } };

        private static readonly string defaultCountry = "United Kingdom";
        //------------------------------------------------------------------------------------
        public StoreDataController()
            : base()
        {
            var Session = System.Web.HttpContext.Current.Session;
            ownerName = (Session["Owner Name"] == null )? null : (string)Session["Owner Name"];
            today = (Session["Today"] == null )? new DateTime() : (DateTime)Session["Today"];
            isSessionExpired = (Session["Owner Name"] == null) ? true : false;
        }
        //------------------------Store List with data-----------------------------------
        public ActionResult Index(string chosenCountry, string term = "Day", int unitsAgo = 0,
            string time = "Today", int orderHow = 1, int orderWhat = 0)
        {
            if (isSessionExpired) return RedirectToAction("LogOff", "Account", routeValues: new { i = 0 });

            string cachekey = string.Format("owner{0}{1}{2}", ownerName, term, unitsAgo);

            cube = (CubeAdapter)Session["cube"];
            Owner owner;
            if (Session["CountriesAndStores"] == null) Session["CountriesAndStores"] =
                cube.GetCountriesAndStores((List<string>)Session["Chains"]);
            var countriesAndStores = (Dictionary<string, List<string>>)Session["CountriesAndStores"];
            if (HttpRuntime.Cache[cachekey] == null)
            {
                owner = new Owner(ownerName, countriesAndStores);
                if (Session["Owner"] == null) Session["Owner"] = owner;// used for getting store names for security only                
                cube.FillStoreData(today, ref owner, term, unitsAgo);
                addToCache(cachekey, owner);
            }
            else
            {
                owner = (Owner)HttpRuntime.Cache[cachekey];
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
            int unitsAgo = 0, string Time = "Today", int orderHow = 1, int orderWhat = 0)
        {
            if (isSessionExpired || isStoreNameHighjacked(storeName)) return RedirectToAction("LogOff", "Account", routeValues: new { i = 0 });

            Store store = fillStore(today, storeName, country, term, unitsAgo, ownerName);

            ViewBag.TimeDesc = Time + ": " + DateUtilities.getDateDescription(today, term, unitsAgo);
            ViewBag.Time = Time;
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.orderHow = orderHow;
            ViewBag.orderWhat = orderWhat;
            ViewBag.country = country;

            return View(store);
        }
        //--------------------plotting measure for present and past (storeDataIndex) for chosen store----------------
        public ActionResult StorePlot(string storeName, string country, int storeDataIndex, string term, int unitsAgo = 0)
        {
            if (isSessionExpired || isStoreNameHighjacked(storeName)) return RedirectToAction("LogOff", "Account", routeValues: new { i = 0 });

            if (term == null) term = DateUtilities.weekDayAveragingPeriod[0];

            Store storePresent;
            Store storePast;

            storePresent = fillStore(today, storeName, country, term, unitsAgo, ownerName);
            storePast = fillStore(today, storeName, country, term, unitsAgo + 1, ownerName);

            string term1 = term.Substring(term.LastIndexOf(' ') + 1);// last word
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.TimeDesc = term1 + ": " + DateUtilities.getDateDescription(today, term1, unitsAgo, true) + " vs. "
                + DateUtilities.getDateDescription(today, term1, unitsAgo + 1, true);
            ViewBag.storeDataIndex = storeDataIndex;
            ViewBag.country = country;

            savePlotToCache(storeName, country, storeDataIndex, term, ownerName, storePresent, storePast);

            return View(storePresent);
        }
        //----------------Make and Save plot to cache if it does not exist in cache------------------------------------------------------------
        private void savePlotToCache(string storeName, string country, int storeDataIndex, string term, string ownerName, Store storePresent, Store storePast)
        {
            byte[] chart;
            string cachekey = string.Format("chart.{0}.{1}.{2}.{3}.{4}", ownerName, storeName, country, term, storeDataIndex);
            if (HttpRuntime.Cache[cachekey] == null)
            {
                // for plot legend
                string averagingPeriod = term.Substring(term.LastIndexOf(' ') + 1); //last word

                chart = PlotMaker.CreateChart(new Store[] { storePresent, storePast },
                    new string[] { "This " + averagingPeriod, "Last " + averagingPeriod }, storeDataIndex, term);
                addToCache(cachekey, chart);
            }
        }
        //-----------------------fill store with data and save in cache or just get from cache------------------------------
        private Store fillStore(DateTime today, string storeName, string country, string term, int unitsAgo, string ownerName)
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
                cube = (CubeAdapter)Session["cube"];
                Store store = cube.FillStoreData(today, storeName, country, term, unitsAgo);

                addToCache(storeCacheKey, store);
                return store;
            }
        }
        //----------------plotting all data for a store-------------------------------------
        public ActionResult Plots(DateTime today, string storeName, string country, string term, int unitsAgo = 0)
        {
            if (term == null) term = DateUtilities.weekDayAveragingPeriod[0];
            Store storePresent;
            Store storePast;

            storePresent = fillStore(today, storeName, country, term, unitsAgo, ownerName);
            storePast = fillStore(today, storeName, country, term, unitsAgo + 1, ownerName);

            string term1 = term.Substring(term.LastIndexOf(' ') + 1);
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.TimeDesc = term1 + ": " + DateUtilities.getDateDescription(today, term1, unitsAgo, true) + " vs. "
                + DateUtilities.getDateDescription(today, term1, unitsAgo + 1, true);

            for (var storeDataIndex = 0; storeDataIndex < storePresent.StoreData.Length; storeDataIndex++)
            {
                savePlotToCache(storeName, country, storeDataIndex, term, ownerName, storePresent, storePast);
            }
            return View(storePresent);
        }
        //-------------------------------------------------------------------------------------------
        public ActionResult DrawChart(string storeName, string country, int _storeDataIndex, int unitsAgo = 0,
            string term = "Weekday Year", string Time = "Week Day for Year")
        {
            string cachekey = string.Format("chart.{0}.{1}.{2}.{3}.{4}", ownerName, storeName, country, term, _storeDataIndex);
            return File((byte[])HttpRuntime.Cache[cachekey], "image/bytes");
        }
        //-------------------------------------------------------------------------------------------------------
        private void addToCache(string cachekey, object obj)
        {
            HttpRuntime.Cache.Add(cachekey, obj, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                new TimeSpan(0, 1, 0), System.Web.Caching.CacheItemPriority.Default, null);
        }
        //---------------------SECURITY-------------------------------------
        private bool isStoreNameHighjacked(string storeName)
        {
            Owner owner = (Owner)Session["Owner"];
            return !owner.hasStore(storeName);
        }
    }
}
