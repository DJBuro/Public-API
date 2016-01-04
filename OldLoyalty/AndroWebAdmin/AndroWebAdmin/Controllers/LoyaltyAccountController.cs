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
    public class LoyaltyAccountController : SiteController
    {
        public ILoyaltyAccountDao LoyaltyAccountDao { get; set; }
        public ICompanyDao CompanyDao { get; set; }
        public IAccountStatusDao AccountStatusDao { get; set; }


        public ActionResult Index()
        {
            var data = new AndroWebAdminViewData.LoyaltyAccountViewData();

            data.LoyaltyAccounts = LoyaltyAccountDao.FindAll();
            data.Companies = CompanyDao.FindAll();
            data.AccountStatuses = AccountStatusDao.FindAll();

            return (View(LoyaltyAccountControllerViews.Index, data));
        }

        public ActionResult Status(int id)
        {
            var data = new AndroWebAdminViewData.LoyaltyAccountViewData();

            var accountsByStatuses = AccountStatusDao.FindById(id).AccountStatuses;

            data.LoyaltyAccounts = new List<LoyaltyAccount>();
            data.AccountStatus = AccountStatusDao.FindById(id);

            foreach (LoyaltyAccountStatus accountStatus in accountsByStatuses)
            {
                //data.AccountStatus = accountStatus.AccountStatus;
                data.LoyaltyAccounts.Add(accountStatus.LoyaltyAccount);
            }
            
            return (View(LoyaltyAccountControllerViews.Status, data));
        }


        public ActionResult Company(int id)
        {
            var data = new AndroWebAdminViewData.LoyaltyAccountViewData();

            data.Company = CompanyDao.FindById(id);
            data.LoyaltyAccounts = LoyaltyAccountDao.FindAll().Where(c => c.Site.Company.Id.Value == id).ToList();

            return (View(LoyaltyAccountControllerViews.Company, data));
        }

        public ActionResult Details(int id)
        {
            var data = new AndroWebAdminViewData.LoyaltyAccountViewData();

            //data.AccountStatusListItems = AccountStatusDao.FindAll().ToSelectList("Id","Name");

            data.LoyaltyAccount = LoyaltyAccountDao.FindById(id);

            return (View(LoyaltyAccountControllerViews.Details, data));
        }

    }
}
