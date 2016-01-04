using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.Model;

namespace AndroAdmin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.Home;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
