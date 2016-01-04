using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.Model;
using CloudSync;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewCloudServers")]
    public class CloudServerController : BaseController
    {
        public CloudServerController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.OnlineOrdering;
            ViewBag.SelectedWebOrderingMenu = WebOrderingMenuItemEnum.CloudServer;
        }

        [Security(Permissions = "ViewCloudServers")]
        public ActionResult Index()
        {
            try
            {
                // Get all the hosts
                IList<AndroAdminDataAccess.Domain.Host> hosts = this.HostDAO.GetAll();

                return View(hosts);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("CloudServerController.Index", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [Security(Permissions = "ViewCloudServers")]
        public ActionResult Sync()
        {
            try
            {
                // Push the change out to the ACS servers
                string errorMessage = SyncHelper.ServerSync();

                // Success
                if (errorMessage.Length == 0)
                {
                    TempData["message"] = "Cloud servers successfully updated";
                }
                else
                {
                    TempData["errorMessage"] = "Failed to update cloud servers";
                }

                return RedirectToAction("Index", "CloudServer");
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("CloudServerController.Sync", exception);

                return RedirectToAction("Index", "Error");
            }
        }
    }
}
