using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using AndroWebAdmin.Models;
using AndroWebAdmin.Mvc;
using Loyalty.Dao;
using Loyalty.Dao.Domain;

namespace AndroWebAdmin.Controllers
{
    public class LoyaltyCardController : SiteController
    {
        public ILoyaltyAccountDao LoyaltyAccountDao { get; set; }
        public ICompanyDao CompanyDao { get; set; }
        public ILoyaltyCardDao LoyaltyCardDao { get; set; }
        public IStatusDao StatusDao { get; set; }

        public ActionResult Index()
        {
            var data = new AndroWebAdminViewData.LoyaltyCardViewData();

            data.LoyaltyCards = LoyaltyCardDao.FindAll();

            data.Companies = CompanyDao.FindAll();
            
            data.Statuses = StatusDao.FindAll();

            return (View(LoyaltyCardControllerViews.Index, data));
        }

        public ActionResult Status(int id)
        {

            var data = new AndroWebAdminViewData.LoyaltyCardViewData();

            var cardByStatus = StatusDao.FindById(id).LoyaltyCardByStatus;

            data.LoyaltyCards = new List<LoyaltyCard>();

            data.Status = StatusDao.FindById(id);
            
            foreach (LoyaltyCardStatus loyaltyCardStatus in cardByStatus)
            {
                
                data.LoyaltyCards.Add(loyaltyCardStatus.LoyaltyCard);
            }

            return (View(LoyaltyCardControllerViews.Status, data));
        }

        public ActionResult Company(int id)
        {
            var data = new AndroWebAdminViewData.LoyaltyCardViewData();

            data.Company = CompanyDao.FindById(id);

            data.LoyaltyCards = LoyaltyCardDao.FindLoyaltyCardsByCompany(data.Company);

            return (View(LoyaltyCardControllerViews.Company, data));
        }

        public ActionResult Details(int id)
        {
            var data = new AndroWebAdminViewData.LoyaltyCardViewData();

            data.LoyaltyCard = LoyaltyCardDao.FindById(id);
            return (View(LoyaltyCardControllerViews.Details, data));
        }

    }
}
