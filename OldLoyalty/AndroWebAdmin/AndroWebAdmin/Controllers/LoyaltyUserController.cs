using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using AndroWebAdmin.Models;
using AndroWebAdmin.Mvc;
using Loyalty.Dao;

namespace AndroWebAdmin.Controllers
{
    public class LoyaltyUserController : SiteController
    {
        public ILoyaltyUserDao LoyaltyUserDao { get; set; }

        public ActionResult Index()
        {
            var data = new AndroWebAdminViewData.LoyaltyUserViewData();

            data.LoyaltyUsers = LoyaltyUserDao.FindAll();

            return (View(LoyaltyCardControllerViews.Index, data));
        }

    }
}
