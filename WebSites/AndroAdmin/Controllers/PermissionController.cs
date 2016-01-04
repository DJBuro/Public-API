using AndroAdmin.Helpers;
using AndroAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AndroAdmin.Controllers
{
    [Authorize]
    public class PermissionController : BaseController
    {
        private readonly PermissionControllerService permissionControllerService = new PermissionControllerService(); 

        //
        // GET: /Permission/

        public ActionResult Index()
        {
            this.permissionControllerService.EnsurePermissionsExitForEditingHosts();

            return View();
        }
    }
}
