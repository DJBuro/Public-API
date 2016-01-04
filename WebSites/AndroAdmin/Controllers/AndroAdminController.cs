using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdmin.DataAccess;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;
using System.Web.Security;
using AndroAdminDataAccess.Domain;
using AndroAdmin.Model;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewAndroAdminLinks")]
    public class AndroAdminController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.SelectedMenu = MenuItemEnum.AndroAdmin;

            return View();
        }
    }
}
