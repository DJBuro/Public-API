using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdmin.Helpers;
using AndroAdmin.Model;

namespace AndroAdmin.Controllers
{
    [Security(Permissions = "ACSMenuSync")]
    public class MenuSyncController : BaseController
    {
        public MenuSyncController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.OnlineOrdering;
            ViewBag.SelectedWebOrderingMenu = WebOrderingMenuItemEnum.MenuSync;
        }

        //
        // GET: /MenuSync/

        [Security(Permissions = "ACSMenuSync")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
