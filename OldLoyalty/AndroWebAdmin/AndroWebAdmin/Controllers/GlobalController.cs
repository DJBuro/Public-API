using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using AndroWebAdmin.Models;
using AndroWebAdmin.Mvc;
using AndroWebAdmin.Mvc.Extensions;
using Loyalty.Dao;
using Loyalty.Dao.Domain;

namespace AndroWebAdmin.Controllers
{
    public class GlobalController : SiteController
    {
        ICompanyDao CompanyDao { get; set; }
        ICountryDao CountryDao { get; set; }
        ICurrencyDao CurrencyDao { get; set; }
        ILoyaltyLogDao LoyaltyLogDao { get; set; }
        ISiteDao SiteDao { get; set; }

        public ActionResult Index()
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            data.Companies = CompanyDao.FindAll();
            data.Countries = CountryDao.FindAll();
            data.Currencies = CurrencyDao.FindAll();
            data.LoyaltyLogs = LoyaltyLogDao.FindAll().OrderByDescending(c => c.Id.Value).ToList();

            return (View(GlobalControllerViews.Index, data));
        }

        public ActionResult AddCountry()
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            data.CurrencyListItems = CurrencyDao.FindAll().ToSelectList("Id", "Name");

            return (View(GlobalControllerViews.AddCountry, data));
        }

        public ActionResult EditCountry(int id)
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            data.Country = CountryDao.FindById(id);

            foreach (CountryCurrency pp in data.Country.CountryCurrencies)
            {
                data.CurrencyListItems = CurrencyDao.FindAll().ToSelectList("Id", "Name", pp.Currency.Id.ToString() );
            }

            return (View(GlobalControllerViews.EditCountry, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveCountry(Country country)
        {
            var data = new AndroWebAdminViewData.GlobalViewData();
            
            data.Country = country;
            
            if (country.Id.HasValue)
            {
                data.Country = CountryDao.FindById(country.Id.Value);
                data.Country.Name = country.Name;
                data.Country.ISOCode = country.ISOCode;
            }

            var currencyId = Convert.ToInt32(Request.Form.GetValues("Country.Currency.Id")[0]);
            
            var currency = new CountryCurrency();
            currency.Country = country;
            currency.Currency = CurrencyDao.FindById(currencyId);
            country.CountryCurrencies.Add(currency);

            data.Country.CountryCurrencies.Clear();
            data.Country.CountryCurrencies.Add(currency);

            CountryDao.Save(data.Country);

            return (Index());
        }

        public ActionResult AddCurrency()
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            return (View(GlobalControllerViews.AddCurrency, data));
        }

        public ActionResult EditCurrency(int id)
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            data.Currency = CurrencyDao.FindById(id);

            return (View(GlobalControllerViews.EditCurrency, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveCurrency(Currency currency)
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            data.Currency = currency;

            if(currency.Id.HasValue)
            {
                data.Currency = CurrencyDao.FindById(currency.Id.Value);
                data.Currency.Name = currency.Name;
                data.Currency.Symbol = currency.Symbol;
            }

            CurrencyDao.Save(data.Currency);

            return (Index());
        }

        public ActionResult ViewLog(int id)
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            data.LoyaltyLog = LoyaltyLogDao.FindById(id);

            return (View(GlobalControllerViews.ViewLog, data));
        }

        public ActionResult SiteLog(int id)
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            data.Site = SiteDao.FindById(id);

            data.LoyaltyLogs = LoyaltyLogDao.GetLastTwenty(data.Site);

            return (View(GlobalControllerViews.SiteLog, data));
        }

        public ActionResult ClearLog()
        {
            LoyaltyLogDao.DeleteAll(LoyaltyLogDao.FindAll());
            return (Index());
        }

        public ActionResult ClearSiteLog(int id)
        {
            var data = new AndroWebAdminViewData.GlobalViewData();

            data.Site = SiteDao.FindById(id);

            LoyaltyLogDao.DeleteAll(LoyaltyLogDao.GetLastTwenty(data.Site));
            return (Index());

        }

    }
}
