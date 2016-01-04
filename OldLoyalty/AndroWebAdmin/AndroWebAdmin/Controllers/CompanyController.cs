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
    public class CompanyController : SiteController
    {
        public ICompanyDao CompanyDao { get; set; }
        public ICountryDao CountryDao { get; set; }
        public ISiteDao SiteDao { get; set; }
        public ILoyaltyAccountDao LoyaltyAccountDao { get; set; }
        public ICompanyUserTitleDao CompanyUserTitleDao { get; set; }
        public IUserTitleDao UserTitleDao { get; set; }

        public ActionResult Index()
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.Companies = CompanyDao.FindAll();

            return (View(CompanyControllerViews.Index, data));
        }

        public ActionResult EditCompany(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.Company = CompanyDao.FindById(id);

            var countryListItems = CountryDao.FindAll().ToSelectList("Id", "Name", data.Company.Country.Id.ToString());

            data.CountryListItems = countryListItems;

            return (View(CompanyControllerViews.EditCompany, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveCompany(Company company)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            if(company.Id.HasValue)
            {
                data.Company = CompanyDao.FindById(company.Id.Value);
                data.Company.Country = null;
            }
            else //new company
            {
                data.Company = new Company(); 
            }

            if (UpdateModel(data.Company))
            {
                CompanyDao.SaveOrUpdate(data.Company);
            }

            return (Index());
        }

        //todo: implement
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteCompany(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

           data.Company = CompanyDao.FindById(id);

           if (data.Company.Sites.Count == 0 & data.Company.LoyaltyAccounts.Count == 0)
            {
               //userTitles?
               //accounts
                CompanyDao.Delete(data.Company);
            }
            
            return (Index());
        }

        public ActionResult AddCompany()
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            var countryListItems = CountryDao.FindAll().ToSelectList("Id", "Name");

            data.CountryListItems = countryListItems;

            return (View(CompanyControllerViews.AddCompany, data));
        }


        public ActionResult Sites(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.Sites = SiteDao.FindAll().Where(c => c.Company.Id == id).ToList();
            data.Company = CompanyDao.FindById(id);

            return (View(CompanyControllerViews.Sites, data));
        }

        public ActionResult AddSite(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.Company = CompanyDao.FindById(id);

            var countryListItems = CountryDao.FindAll().ToSelectList("Id", "Name");

            data.CountryListItems = countryListItems;

            return (View(CompanyControllerViews.AddSite, data));
        }

        public ActionResult EditSite(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.Site = SiteDao.FindById(id);
            data.Company = data.Site.Company;

            var countryListItems = CountryDao.FindAll().ToSelectList("Id", "Name", data.Site.Country.Id.Value.ToString());

            data.CountryListItems = countryListItems;

            

            return (View(CompanyControllerViews.EditSite, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveSite(Site site)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            if(site.Id.HasValue)
            {
                data.Site = SiteDao.FindById(site.Id.Value);
            }
            else //adding a new site;
            {
                data.Site = new Site();
                data.Site.Company = site.Company;
                data.Site.Country = site.Country;
            }

            if (UpdateModel(data.Site))
            {
                SiteDao.Save(data.Site);
            }

            return (Index());
        }


        public ActionResult Accounts(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.LoyaltyAccounts = LoyaltyAccountDao.FindAll().Where(c => c.Site.Company.Id == id).ToList();

            return (View(CompanyControllerViews.Sites, data));
        }

        public ActionResult UserTitles(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.Company = CompanyDao.FindById(id);

            data.CompanyUserTitles = CompanyUserTitleDao.FindByCompany(data.Company).ToList();

            return (View(CompanyControllerViews.UserTitles, data));
        }

        public ActionResult EditTitle(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.CompanyUserTitle = CompanyUserTitleDao.FindById(id);

            return (View(CompanyControllerViews.EditUserTitle, data));
        }

        public ActionResult AddTitle(int id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.Company = CompanyDao.FindById(id);

            return (View(CompanyControllerViews.AddTitle, data));
        }


        //todo: implement
        [AcceptVerbs(HttpVerbs.Post)]
        public void DeleteTitle(CompanyUserTitle companyUserTitle)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            if (companyUserTitle.Id.HasValue)
                data.CompanyUserTitle = CompanyUserTitleDao.FindById(companyUserTitle.Id.Value);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveTitle(CompanyUserTitle companyUserTitle)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            if (companyUserTitle.Id.HasValue)
                data.CompanyUserTitle = CompanyUserTitleDao.FindById(companyUserTitle.Id.Value);

            if (UpdateModel(data.CompanyUserTitle))
            {
                if (data.CompanyUserTitle.UserTitle.Title.Length > 0)
                    CompanyUserTitleDao.Save(data.CompanyUserTitle);
            }

            return UserTitles(data.CompanyUserTitle.Company.Id.Value);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveNewTitle(int? id)
        {
            var data = new AndroWebAdminViewData.CompanyViewData();

            data.CompanyUserTitle = new CompanyUserTitle();

            data.CompanyUserTitle.Company = CompanyDao.FindById(id.Value);

            if (UpdateModel(data.CompanyUserTitle))
            {
                if (data.CompanyUserTitle.UserTitle.Title.Length > 0)
                {
                    UserTitleDao.Save(data.CompanyUserTitle.UserTitle);
                    CompanyUserTitleDao.Save(data.CompanyUserTitle);
                }
            }

            return UserTitles(data.CompanyUserTitle.Company.Id.Value);
        }

    }
}
