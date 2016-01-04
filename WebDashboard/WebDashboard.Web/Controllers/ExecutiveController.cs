using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebDashboard.Dao;
using WebDashboard.Dao.Domain;
using WebDashboard.Dao.Domain.Helpers;
using WebDashboard.Mvc;
using WebDashboard.Mvc.Filters;
using WebDashboard.Mvc.Helpers;
using WebDashboard.Web.Models;
using System.Threading;
using System;

namespace WebDashboard.Web.Controllers
{
    public class ExecutiveController : SiteController
    {
        public IUserDao UserDao { get; set; }
        public IHistoricalDataDao HistoricalDataDao { get; set; }

        [RequiresAuthorisation]
        public ActionResult Index(string forDate)
        {
            // Get the logged in user
            WebDashboardViewData.PageViewData data = new WebDashboardViewData.PageViewData { User = GetUser() };

            DateTime? forDateTime = null;
            if (data.User.IsExecutiveDashboardGroupUser || data.User.IsExecutiveDashboardUser)
            {
                // Was a trading day passed in?
                DateTime tempDateTime;
                if (forDate != null && DateTime.TryParse(forDate, out tempDateTime))
                {
                    forDateTime = tempDateTime;
                }
               
                // Get a list of trading days that data is available for
                this.GetHistoricalDates(data, forDateTime);

                data.ForDateTime = forDateTime;
            }

            // Is this a group executive dashboard user?
            if (data.User.IsExecutiveDashboardGroupUser)
            {
                IList<GroupExchangeRate> groupExchangeRates = GroupExchangeRateDao.FindByGroupId(data.User.HeadOffice.GroupId.Value);
                data.HeadOffices = HeadOfficeDao.FindAll();

                data.ExecutiveGroupDashboard = ExecutiveDashboard.GetGroupData(data.User.HeadOffice, data.HeadOffices, groupExchangeRates, forDateTime, this.HistoricalDataDao);

                data.CurrencySymbol = "$"; //data.User.HeadOffice.CurrencySymbol;

                return View(AdminControllerViews.Index, data);
            }
            // Is this a normal executive dashboard user?
            else if (data.User.IsExecutiveDashboardUser)
            {
                data.Regions = RegionDao.FindHeadOffice(data.User.HeadOffice, true);
                data.UserRegions = UserRegionDao.FindByUserId(data.User.Id.Value);
                data.Sites = new List<Site>();

                data.User.HeadOffice.ExchangeRate = 1;
                
                foreach (Region region in data.Regions)
                {
                    bool allStores = false;
                    
                    // Does this user have permission to view specific regions?
                    if (data.UserRegions != null)
                    {
                        // Does the user have permission to see all the stores in this region?
                        foreach (UserRegion userRegion in data.UserRegions)
                        {
                            if (userRegion.RegionId == region.Id)
                            {
                                // User has permission to see all the stores in this region
                                allStores = true;        
                                break;
                            }
                        }
                    }

                    IDictionary<int, HistoricalData> historicData = null;
                    if (data.ForDateTime.HasValue)
                    {
                        historicData = HistoricalDataDao.FindByHeadOfficeIdAndTradingDay(data.User.HeadOffice.Id.Value, data.ForDateTime.Value);
                    }

                    // Does the user have permission to see all the stores in this region?
                    if (allStores)
                    {
                        // Add all the sites for this region
                        foreach (Site site in region.RegionalSites)
                        {
                            ExecutiveDashboard.AddSite(data.ForDateTime, site, historicData, data.Sites);
                        }
                    }
                    else
                    {
                        // Only add sites that the user has permission to see
                        foreach (Site site in region.RegionalSites)
                        {
                            foreach (Permission permission in data.User.UserPermissions)
                            {
                                // Does the user have permission to view this site?
                                if (permission.Site.Id == site.Id)
                                {
                                    site.ExchangeRate = 1;
                                    ExecutiveDashboard.AddSite(data.ForDateTime, site, historicData, data.Sites);
                                }
                            }
                        }
                    }
                }
                                
                data.ExecutiveCompanyDashboard = ExecutiveDashboard.GetCompanyData(data.User.HeadOffice, data.Sites);
                
                data.CurrencySymbol = data.User.HeadOffice.CurrencySymbol;
                
                return View(AdminControllerViews.Index, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult Region(string forDate)
        {
            // Get the logged in user
            WebDashboardViewData.PageViewData data = new WebDashboardViewData.PageViewData { User = GetUser() };

            // Is this a normal executive dashboard user?
            if (data.User.IsExecutiveDashboardUser)
            {
                // Was a trading day passed in?
                DateTime? forDateTime = null;
                DateTime tempDateTime;
                if (forDate != null && DateTime.TryParse(forDate, out tempDateTime))
                {
                    forDateTime = tempDateTime;
                }

                // Get a list of trading days that data is available for
                this.GetHistoricalDates(data, forDateTime);

                data.ForDateTime = forDateTime;
                data.Regions = RegionDao.FindHeadOffice(data.User.HeadOffice, true);
                data.UserRegions = UserRegionDao.FindByUserId(data.User.Id.Value);

                data.User.HeadOffice.ExchangeRate = 1;
                
                List<Region> removeRegions = new List<Region>();

                foreach (Region region in data.Regions)
                {
                    bool allStores = false;
                    List<Site> regionalSites = new List<Site>();

                    // Does this user have permission to view specific regions?
                    if (data.UserRegions != null)
                    {
                        // Does the user have permission to see all the stores in this region?
                        foreach (UserRegion userRegion in data.UserRegions)
                        {
                            if (userRegion.RegionId == region.Id)
                            {
                                // User has permission to see all the stores in this region
                                allStores = true;
                                break;
                            }
                        }
                    }

                    IDictionary<int, HistoricalData> historicData = null;
                    if (data.ForDateTime.HasValue)
                    {
                        historicData = HistoricalDataDao.FindByHeadOfficeIdAndTradingDay(data.User.HeadOffice.Id.Value, data.ForDateTime.Value);
                    }

                    // Does the user have permission to see all the stores in this region?
                    if (allStores)
                    {
                        foreach (Site site in region.RegionalSites)
                        {
                            if (site.Enabled)
                            {
                                site.ExchangeRate = 1;
                                ExecutiveDashboard.AddSite(data.ForDateTime, site, historicData, regionalSites);
                            }
                        }
                    }
                    else
                    {                        
                        // Only add sites that the user has permission to see
                        foreach (Site site in region.RegionalSites)
                        {
                            if (site.Enabled)
                            {
                                foreach (Permission permission in data.User.UserPermissions)
                                {
                                    // Does the user have permission to view this site?
                                    if (permission.Site.Id == site.Id)
                                    {
                                        ExecutiveDashboard.AddSite(data.ForDateTime, site, historicData, regionalSites);
                                        break;
                                    }

                                }
                            }
                        }
                    }
                    
                    // Are there any sites in the region that the user is allowed to view?
                    if (regionalSites.Count == 0)
                    {
                        // No.  We should remove the region later
                        removeRegions.Add(region);
                    }
                    
                    // Only allow the user to see the sites that he/she is allowed to see
                    region.RegionalSites = regionalSites;
                }

                // Remove empty regions
                foreach (Region removeRegion in removeRegions)
                {
                    data.Regions.Remove(removeRegion);
                }

                data.ExecutiveRegionDashboard = ExecutiveDashboard.GetRegionalData(data.User.HeadOffice, data.Regions);

                data.CurrencySymbol = data.User.HeadOffice.CurrencySymbol;

                return View(AdminControllerViews.Region, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult RegionalStore(int? id, string trans, string orderby, string forDate)
        {
            // Get the logged in user
            WebDashboardViewData.PageViewData data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsExecutiveDashboardUser)
            {
                // Was a trading day passed in?
                DateTime? forDateTime = null;
                DateTime tempDateTime;
                if (forDate != null && DateTime.TryParse(forDate, out tempDateTime))
                {
                    forDateTime = tempDateTime;
                }
                
                // Get a list of trading days that data is available for
                this.GetHistoricalDates(data, forDateTime);

                data.ForDateTime = forDateTime;
                data.Region = RegionDao.FindById(id.Value);
                data.UserRegions = UserRegionDao.FindByUserId(data.User.Id.Value);
                data.Sites = new List<Site>();

                // Get historic data for this head office and trading day
                IDictionary<int, HistoricalData> historicData = null;
                if (forDateTime.HasValue)
                {
                    historicData = this.HistoricalDataDao.FindByHeadOfficeIdAndTradingDay(data.User.HeadOffice.Id.Value, forDateTime.Value);
                }

                bool allStores = false;
                List<Site> sites = new List<Site>();

                if (data.UserRegions != null)
                {
                    // Does the user have permission to see all the stores in this region?
                    foreach (UserRegion userRegion in data.UserRegions)
                    {
                        if (userRegion.RegionId == data.Region.Id)
                        {
                            // User has permission to see all the stores in this region
                            allStores = true;

                            foreach (Site site in data.Region.RegionalSites)
                            {
                                if (site.Enabled)
                                {
                                    ExecutiveDashboard.AddSite(forDateTime, site, historicData, sites);
                                    //sites.Add(site);
                                }
                            }
                            break;
                        }
                    }
                }

                // Does the user have permission to see all the stores in this region?
                if (!allStores)
                {
                    // Only add sites that the user has permission to see
                    foreach (Site site in data.Region.RegionalSites)
                    {
                        if (site.Enabled)
                        {
                            foreach (Permission permission in data.User.UserPermissions)
                            {
                                // Does the user have permission to view this site?
                                if (permission.Site.Id == site.Id)
                                {
                                    ExecutiveDashboard.AddSite(forDateTime, site, historicData, sites);
                                    //sites.Add(site);
                                    break;
                                }
                            }
                        }
                    }
                }

                // Only allow the user to see the sites that he/she is allowed to see
                data.Sites = sites;

                data.ExecutiveStoreDashboard = ExecutiveDashboard.GetStoreData(data.Sites);

                if (trans != null && trans.Length > 0)
                {
                    data.ExecutiveStoreDashboard.StoreList = Calculations.OrderBy(
                        trans, 
                        orderby,
                        data.ExecutiveStoreDashboard.StoreList);
                }
                else
                {
                    data.ExecutiveStoreDashboard.StoreList = Calculations.OrderBy(
                        "Name", 
                        "",
                        data.ExecutiveStoreDashboard.StoreList);
                }

                data.CurrencySymbol = data.User.HeadOffice.CurrencySymbol;
                
                return View(AdminControllerViews.RegionalStore, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult CompanyStore(int? id, string trans, string orderby, string forDate)
        {
            WebDashboardViewData.PageViewData data = new WebDashboardViewData.PageViewData { User = GetUser() };

            // Is this a group executive dashboard user?
            if (data.User.IsExecutiveDashboardGroupUser &&
                id.HasValue)
            {
                IList<GroupExchangeRate> groupExchangeRates = GroupExchangeRateDao.FindByGroupId(data.User.HeadOffice.GroupId.Value);
                HeadOffice company = HeadOfficeDao.FindById(id.Value);

                // Was a trading day passed in?
                DateTime? forDateTime = null;
                DateTime tempDateTime;
                if (forDate != null && DateTime.TryParse(forDate, out tempDateTime))
                {
                    forDateTime = tempDateTime;
                }

                // Get a list of trading days that data is available for
                this.GetHistoricalDates(data, forDateTime);

                data.ForDateTime = forDateTime;

                // Set the exchange rate for the company
                Calculations.SetCompanyExchangeRate(company, groupExchangeRates);

                IDictionary<int, HistoricalData> historicData = null;
                if (data.ForDateTime.HasValue)
                {
                    historicData = HistoricalDataDao.FindByHeadOfficeIdAndTradingDay(id.Value, data.ForDateTime.Value);
                }
                List<Site> sites = new List<Site>();

                foreach (Site site in company.Sites)
                {
                    site.ExchangeRate = company.ExchangeRate;
                    ExecutiveDashboard.AddSite(data.ForDateTime, site, historicData, sites);
                }
                
                data.Sites = new List<Site>();

                ExecutiveGroupDashboard executiveGroupDashboard = new ExecutiveGroupDashboard();

                // Are we displaying live data?
                if (forDateTime == null)
                {
                    // We are displaying live data so we should zero data for stores
                    // that haven't reported today
                    Calculations.CleanupData(sites, true);
                }

                data.ExecutiveStoreDashboard = ExecutiveDashboard.GetStoreData(sites);
                data.ExecutiveStoreDashboard.CompanyId = id;

                // Do we need to sort the stores by one of the columns?
                if (trans != null && 
                    trans.Length > 0)
                {
                    data.ExecutiveStoreDashboard.StoreList = Calculations.OrderBy(
                        trans,
                        orderby,
                        data.ExecutiveStoreDashboard.StoreList);
                }
                else
                {
                    data.ExecutiveStoreDashboard.StoreList = Calculations.OrderBy(
                        "Name",
                        "",
                        data.ExecutiveStoreDashboard.StoreList);
                }

                data.CurrencySymbol = "$"; //data.User.HeadOffice.CurrencySymbol;

                data.IsCompanySites = true;

                return View(AdminControllerViews.Store, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult Store(string id, string trans, string forDate)
        {
            // Get the logged in user
            WebDashboardViewData.PageViewData data = new WebDashboardViewData.PageViewData { User = GetUser() };

            DateTime? forDateTime = null;
            if (data.User.IsExecutiveDashboardGroupUser || data.User.IsExecutiveDashboardUser)
            {
                // Was a trading day passed in?
                DateTime tempDateTime;
                if (forDate != null && DateTime.TryParse(forDate, out tempDateTime))
                {
                    forDateTime = tempDateTime;
                }

                // Get a list of trading days that data is available for
                this.GetHistoricalDates(data, forDateTime);

                data.ForDateTime = forDateTime;
            }

            // Is this a group executive dashboard user?
            if (data.User.IsExecutiveDashboardGroupUser)
            {
                float exchangeRate = 1;
                IList<GroupExchangeRate> groupExchangeRates = GroupExchangeRateDao.FindByGroupId(data.User.HeadOffice.GroupId.Value);
                data.HeadOffices = HeadOfficeDao.FindAll();
                data.Sites = new List<Site>();

                ExecutiveGroupDashboard executiveGroupDashboard = new ExecutiveGroupDashboard();

                // Get data for each company in the group
                foreach (HeadOffice company in data.HeadOffices)
                {
                    // Is the company part of the group?
                    if (company.GroupId.HasValue &&
                        company.GroupId.Value == data.User.HeadOffice.GroupId.Value)
                    {
                        // Does this company have a currency code?
                        if (company.CurrencyCode != null &&
                            company.CurrencyCode.Length > 0)
                        {
                            // Does the company have an exchange rate that needs to be applied to all values
                            foreach (GroupExchangeRate groupExchangeRate in groupExchangeRates)
                            {
                                if (groupExchangeRate.GroupId == company.GroupId &&
                                    groupExchangeRate.CurrencyCode == company.CurrencyCode)
                                {
                                    exchangeRate = groupExchangeRate.ExchangeRate;
                                    break;
                                }
                            }
                        }

                        // Get historic data for this head office and trading day
                        IDictionary<int, HistoricalData> historicData = null;
                        if (forDateTime.HasValue)
                        {
                            historicData = this.HistoricalDataDao.FindByHeadOfficeIdAndTradingDay(company.Id.Value, forDateTime.Value);
                        }

                        // Add all the sites for this region
                        List<Site> sites = new List<Site>();
                        foreach (Site site in company.Sites)
                        {
                            ExecutiveDashboard.AddSite(forDateTime, site, historicData, sites);
                        }

                        // Are we displaying live data?
                        if (forDateTime == null)
                        {
                            // We are displaying live data so we should zero data for stores
                            // that haven't reported today
                            Calculations.CleanupData(sites, true);
                        }

                        foreach (Site site in sites)
                        {
                            site.ExchangeRate = exchangeRate;
                            data.Sites.Add(site);
                        }
                    }
                }

                // Calculate the values to display using all sites in the group i.e. all sites in all companies
                data.ExecutiveStoreDashboard = ExecutiveDashboard.GetStoreData(data.Sites);

                // Do we need to sort the stores by one of the columns?
                if (id.Length > 0)
                {
                    data.ExecutiveStoreDashboard.StoreList = Calculations.OrderBy(
                        id, 
                        trans,
                        data.ExecutiveStoreDashboard.StoreList);
                }
                else
                {
                    data.ExecutiveStoreDashboard.StoreList = Calculations.OrderBy(
                        "Name", 
                        "",
                        data.ExecutiveStoreDashboard.StoreList);
                }

                data.CurrencySymbol = "$"; //data.User.HeadOffice.CurrencySymbol;

                return View(AdminControllerViews.Store, data);
            }
            // Is this a normal executive dashboard user
            else if (data.User.IsExecutiveDashboardUser)
            {
                data.Regions = RegionDao.FindHeadOffice(data.User.HeadOffice, true);
                data.UserRegions = UserRegionDao.FindByUserId(data.User.Id.Value);
                data.Sites = new List<Site>();

                foreach (Region region in data.Regions)
                {
                    bool allStores = false;

                    // Does this user have permission to view specific regions?
                    if (data.UserRegions != null)
                    {
                        // Does the user have permission to see all the stores in this region?
                        foreach (UserRegion userRegion in data.UserRegions)
                        {
                            if (userRegion.RegionId == region.Id)
                            {
                                // User has permission to see all the stores in this region
                                allStores = true;
                                break;
                            }
                        }
                    }

                    IDictionary<int, HistoricalData> historicData = null;
                    if (data.ForDateTime.HasValue)
                    {
                        historicData = HistoricalDataDao.FindByHeadOfficeIdAndTradingDay(data.User.HeadOffice.Id.Value, data.ForDateTime.Value);
                    }

                    // Does the user have permission to see all the stores in this region?
                    if (allStores)
                    {
                        // Add all the sites for this region
                        foreach (Site site in region.RegionalSites)
                        {
                            ExecutiveDashboard.AddSite(data.ForDateTime, site, historicData, data.Sites);
                        }
                    }
                    else
                    {
                        // Only add sites that the user has permission to see
                        foreach (Site site in region.RegionalSites)
                        {
                            foreach (Permission permission in data.User.UserPermissions)
                            {
                                // Does the user have permission to view this site?
                                if (permission.Site.Id == site.Id)
                                {
                                    ExecutiveDashboard.AddSite(data.ForDateTime, site, historicData, data.Sites);
                                }
                            }
                        }
                    }
                }

                data.ExecutiveStoreDashboard = ExecutiveDashboard.GetStoreData(data.Sites);

                // Do we need to sort the stores by one of the columns?
                if (id.Length > 0)
                {
                    data.ExecutiveStoreDashboard.StoreList = Calculations.OrderBy(
                        id, 
                        trans,
                        data.ExecutiveStoreDashboard.StoreList);
                }
                else
                {
                    data.ExecutiveStoreDashboard.StoreList = Calculations.OrderBy(
                        "Name", 
                        "",
                        data.ExecutiveStoreDashboard.StoreList);
                }

                data.CurrencySymbol = data.User.HeadOffice.CurrencySymbol;
                
                return View(AdminControllerViews.Store, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        //todo: expand if there isn't a user
        private User GetUser()
        {
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieSiteId(HttpContext.Request);
            var user = UserDao.FindById(userId);
            return user;
        }

        //private void GetHistoricalDates(WebDashboardViewData.PageViewData data, DateTime? forDateTime)
        //{
        //    // Get a list of trading days that data is available for
        //    IList<HistoricalData> historicData = HistoricalDataDao.FindByHeadOffice(data.User.HeadOffice);
        //    SortedList<string, bool> availableDates = new SortedList<string, bool>();

        //    // Todays trading day
        //    DateTime today = DateTime.Now;
        //    string todayString = today.ToString("yyyy-MM-dd");

        //    // Build a list of available historic trading days
        //    foreach (HistoricalData historicalDataItem in historicData)
        //    {
        //        if (historicalDataItem.TradingDay.HasValue)
        //        {
        //            // The trading day of the historical day (to be displayed on the web page)
        //            string date = historicalDataItem.TradingDay.Value.ToString("yyyy-MM-dd");

        //            // The trading day that the user would like to view data for
        //            string forDate = (forDateTime == null ? "" : forDateTime.Value.ToString("yyyy-MM-dd"));

        //            // Ignore historic data for today
        //            if (date != todayString)
        //            {
        //                // Have we already added the historic trading day to the list?
        //                if (!availableDates.ContainsKey(date))
        //                {
        //                    // Add the historic trading day to the list.
        //                    // Set true if the user is already viewing the historic trading day.
        //                    // This is used to determine which day is initially selected in the drop down combo
        //                    availableDates.Add(date, date == forDate ? true : false);
        //                }
        //            }
        //        }
        //    }

        //    // Sort the historic trading days - most recent first
        //    data.AvailableDates = new SortedList<string, bool>(availableDates, new ReverseComparer());
        //}

        private void GetHistoricalDates(WebDashboardViewData.PageViewData data, DateTime? forDateTime)
        {
            SortedList<string, bool> availableDates = new SortedList<string, bool>();
            IList<string> historicDates = HistoricalDataDao.GetHistoricalDatesByHeadOffice(data.User.HeadOffice);

            // The trading day that the user would like to view data for
            string forDate = (forDateTime == null ? "" : forDateTime.Value.ToString("yyyy-MM-dd"));
            string todayString = DateTime.Now.ToString("yyyy-MM-dd");

            // Remove today from hostorical days.
            historicDates.Remove(todayString);

            historicDates.ToList().ForEach(date => availableDates.Add(date, date == forDate ? true : false));
            data.AvailableDates = new SortedList<string, bool>(availableDates, new ReverseComparer());
        }
    }
}
