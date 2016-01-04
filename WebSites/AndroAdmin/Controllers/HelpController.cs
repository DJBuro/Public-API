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
    [Security(Permissions = "ViewHelp")]
    public class HelpController : BaseController
    {
        public HelpController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.Help;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult Navigation()
        {
            return View();
        }

        public ActionResult ManageStores()
        {
            return View();
        }

        public ActionResult AddStore()
        {
            return View();
        }

        public ActionResult ViewStoreDetails()
        {
            return View();
        }

        public ActionResult StoreDetails()
        {
            return View();
        }

        public ActionResult StoreAddress()
        {
            return View();
        }

        public ActionResult StoreGPSlocation()
        {
            return View();
        }

        public ActionResult StoreOpeningTimes()
        {
            return View();
        }

        public ActionResult StoreAMSUpload()
        {
            return View();
        }
    }
}
