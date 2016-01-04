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
using CloudSync;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewAMSServers")]
    public class AMSServerController : BaseController
    {
        public AMSServerController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.AMS;
            ViewBag.SelectedAMSMenu = AMSMenuItemEnum.AMSServers;
        }

        [Security(Permissions = "ViewAMSServers")]
        public ActionResult Index()
        {
            ActionResult actionResult = null;

            try
            {
                //// Does the user have permission to access this page?
                //actionResult = SecurityHelper.CheckAccess("ViewAMSServer");

                if (actionResult == null)
                {
                    // Get all the ams servers
                    IEnumerable<AndroAdminDataAccess.Domain.AMSServer> amsServers = this.AMSServerDAO.GetAll();

                    actionResult = View(amsServers);
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSServerController.Index", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "AddAMSServer")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Security(Permissions = "AddAMSServer")]
        public ActionResult Add(AMSServer amsServer)
        {
            ActionResult actionResult = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    actionResult = View(amsServer);
                }

                if (actionResult == null)
                {
                    // Has the name already been used?
                    AMSServer existingAMSServer = this.AMSServerDAO.GetByName(amsServer.Name);
                    if (existingAMSServer != null)
                    {
                        ModelState.AddModelError("Name", "Name already used");
                        actionResult = View(amsServer);
                    }
                }

                if (actionResult == null)
                {
                    // Add the ams server
                    this.AMSServerDAO.Add(amsServer);

                    // Success
                    TempData["message"] = "AMS server successfully added";
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSServerController.Add (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "EditAMSServer")]
        public ActionResult Update(int? id)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Get the ams server to update
                    AndroAdminDataAccess.Domain.AMSServer amsServer = this.AMSServerDAO.GetById(id.Value);

                    if (amsServer == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                    else
                    {
                        actionResult = View(amsServer);
                    }
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSServerController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [HttpPost]
        [Security(Permissions = "EditAMSServer")]
        public ActionResult Update(AndroAdminDataAccess.Domain.AMSServer amsServer)
        {
            ActionResult actionResult = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    actionResult = View(amsServer);
                }

                if (actionResult == null)
                {
                    // Has the ams server name already been used?
                    AMSServer existingAMSServer = this.AMSServerDAO.GetByName(amsServer.Name);
                    if (existingAMSServer != null && existingAMSServer.Id != amsServer.Id)
                    {
                        ModelState.AddModelError("Name", "Name already used");
                        actionResult = View(amsServer);
                    }
                }

                if (actionResult == null)
                {
                    // Update the ams server
                    this.AMSServerDAO.Update(amsServer);

                    // Success
                    TempData["message"] = "AMS server successfully updated";
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSServerController.Update (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "ViewAMSServers")]
        public ActionResult Diagnostics(int? id)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Get the ams server to update
                    AndroAdminDataAccess.Domain.AMSServer amsServer = this.AMSServerDAO.GetById(id.Value);

                    if (amsServer == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                    else
                    {
                        actionResult = View(amsServer);
                    }

                    string xml = "";
                    bool success = HttpHelper.RestGet("https://www.androtechnology.co.uk/services/androadmin/AndroAdminServices.svc/GetSitesForAMS?password=429C19EE237245358F8E1189ABDB1388&amsId=" + amsServer.Name, out xml);

                    if (success)
                    {
                        ViewBag.SiteList = XMLFormatter.FormatXml(xml);
                    }
                    else
                    {
                        ViewBag.SiteList = "Web service call failed";
                    }

               //     "https://www.androtechnology.co.uk/services/androadmin/AndroAdminServices.svc/GetAMSFTPInfoForSite?password=429C19EE237245358F8E1189ABDB1388&siteId="
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSServerController.Diagnostics", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "DeleteAMSServer")]
        public ActionResult Delete(int? id)
        {
            ViewBag.SelectedAMSMenu = AMSMenuItemEnum.AMSServers;

            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Delete the AMS Server
                    this.AMSServerDAO.Delete(id.Value);

                    actionResult = RedirectToAction("Index");

                    TempData["message"] = "AMS server successfully deleted";
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("FTPSiteController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }
    }
}
