using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.Model;
using CloudSync;
using System.Configuration;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions="ACSDiagnosticTests")]
    public class ACSDiagnosticTestsController : BaseController
    {
        public ACSDiagnosticTestsController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.OnlineOrdering;
            ViewBag.SelectedWebOrderingMenu = WebOrderingMenuItemEnum.ACSDiagnosticTests;
        }

        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("ACSDiagnosticTests.Index", exception);

                return RedirectToAction("Index", "Error");
            }
        }
    }
}
