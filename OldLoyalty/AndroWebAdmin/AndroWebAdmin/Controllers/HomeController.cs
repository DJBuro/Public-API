using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroWebAdmin.Models;
using AndroWebAdmin.Mvc;
using Loyalty.Dao;

namespace AndroWebAdmin.Controllers
{
    [HandleError]
    public class HomeController : SiteController
    {
        public ICompanyDao CompanyDao { get; set; }

        public ActionResult Index()
        {
            var data = new AndroWebAdminViewData.IndexViewData();

            data.Company = CompanyDao.FindById(1);

            return (View(HomeControllerViews.Index, data));

        }
    }
}
