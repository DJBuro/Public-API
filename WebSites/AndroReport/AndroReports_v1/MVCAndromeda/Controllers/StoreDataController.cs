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
        public static readonly string[,] timeGrid = { { "Today", "Yesterday" }, { "Week to Date", "Last Week" }, 
                                                    { "Month To Date", "Last Month" }, { "Year To Date", "Last Year" } };
        public static readonly string[,] shortTimeGrid = { { "TD", "YD" }, { "WTD", "LW" }, 
                                                    { "MTD", "LM" }, { "YTD", "LY" } };
        
        private static readonly string defaultCountry="United Kingdom";

        //------------------------Store List with data-----------------------------------
        public ActionResult Index(string chosenCountry, string term = "Day", int unitsAgo = 0, string Time = "Today", int orderHow = 1, int orderWhat = 0)
        {
            if (Session["Owner Name"] == null) return RedirectToAction("LogOff", "Account");
            string ownerName = (string)Session["Owner Name"];
            string cachekey = string.Format("owner{0}{1}{2}", ownerName, term, unitsAgo);

            cube = (CubeAdapter)Session["cube"];
            Owner owner;
            if (Session["CountriesAndStores"] == null) Session["CountriesAndStores"] = cube.GetCountriesAndStores(ownerName);
            var countriesAndStores = (Dictionary<string, List<string>>)Session["CountriesAndStores"];
            if (HttpRuntime.Cache[cachekey] == null)
            {
                owner = new Owner(ownerName, countriesAndStores);
                cube.FillStoreData(ref owner, term, unitsAgo);
                addToCache(cachekey, owner);
            }
            else
            {
                owner = (Owner)HttpRuntime.Cache[cachekey];
            }
            owner.orderby(orderHow, orderWhat);
            ViewBag.TimeDesc = Time + ": " + DateUtilities.getDateDescription(term, unitsAgo);
            ViewBag.Time = Time;
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.orderHow = orderHow;
            ViewBag.orderWhat = orderWhat;

            string country=(chosenCountry != null)? chosenCountry: (owner.Countries.Contains(defaultCountry) ? defaultCountry : owner.Countries[0]);
           
                ViewBag.country = country;

            ViewBag.chosenCountry = new SelectList(owner.Countries, country);

            return View(owner);
        }
        //-----------------------One store with data list for timegrid------------------------------------------
        public ActionResult Individual(string storeName, string country, string term = "Day",
            int unitsAgo = 0, string Time = "Today", int orderHow = 1, int orderWhat = 0)
        {
            if (Session["Owner Name"] == null) return RedirectToAction("LogOff", "Account");
            string ownerName = (string)Session["Owner Name"];
            Store store = fillStore(storeName, country, term, unitsAgo, ownerName); ;

            ViewBag.TimeDesc = Time + ": " + DateUtilities.getDateDescription(term, unitsAgo);
            ViewBag.Time = Time;
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.orderHow = orderHow;
            ViewBag.orderWhat = orderWhat;
            ViewBag.country = country;
            return View(store);
        }

        //--------------------plotting measure for present and past (storeDataIndex) for chosen store----------------
        public ActionResult StorePlot(string storeName, string country, int storeDataIndex, string term, int unitsAgo = 0
            )
        {
            if (Session["Owner Name"] == null) return RedirectToAction("LogOff", "Account");
            if (term == null) term = DateUtilities.weekDayAveragingPeriod[0];
            string ownerName = (string)Session["Owner Name"];
            Store storePresent;
            Store storePast;

            storePresent = fillStore(storeName, country, term, unitsAgo, ownerName);
            storePast = fillStore(storeName, country, term, unitsAgo + 1, ownerName);

            string term1 = term.Substring(term.LastIndexOf(' ') + 1);// last word
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.TimeDesc = term1 + ": " + DateUtilities.getDateDescription(term1, unitsAgo, true) + " vs. " + DateUtilities.getDateDescription(term1, unitsAgo + 1, true);
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
        private Store fillStore(string storeName, string country, string term, int unitsAgo, string ownerName)
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
                Store store = cube.FillStoreData(storeName, country, term, unitsAgo);

                addToCache(storeCacheKey, store);
                return store;
            }
        }
        //----------------plotting all data for a store-------------------------------------
        public ActionResult Plots(string storeName, string country, string term, int unitsAgo = 0)
        {
            if (term == null) term = DateUtilities.weekDayAveragingPeriod[0];
            string ownerName = (string)Session["Owner Name"];
            Store storePresent;
            Store storePast;

            storePresent = fillStore(storeName, country, term, unitsAgo, ownerName);
            storePast = fillStore(storeName, country, term, unitsAgo + 1, ownerName);


            string term1 = term.Substring(term.LastIndexOf(' ') + 1);
            ViewBag.unitsAgo = unitsAgo;
            ViewBag.term = term;
            ViewBag.TimeDesc = term1 + ": " + DateUtilities.getDateDescription(term1, unitsAgo, true) + " vs. " + DateUtilities.getDateDescription(term1, unitsAgo + 1, true);

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
            string ownerName = (string)Session["Owner Name"];
            string cachekey = string.Format("chart.{0}.{1}.{2}.{3}.{4}", ownerName, storeName, country, term, _storeDataIndex);
            return File((byte[])HttpRuntime.Cache[cachekey], "image/bytes");
        }
        //-------------------------------------------------------------------------------------------------------
        private void addToCache(string cachekey, object obj)
        {

            HttpRuntime.Cache.Add(cachekey, obj, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                new TimeSpan(0, 1, 0), System.Web.Caching.CacheItemPriority.Default, null);

        }
    }
}
