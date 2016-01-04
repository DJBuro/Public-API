using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AndroAdmin.Helpers;
using AndroAdminDataAccess.Domain;
using AndroAdmin.Model;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewAMSStores")]
    public class AMSStoreController : BaseController
    {
        public AMSStoreController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.AMS;
            ViewBag.SelectedAMSMenu = AMSMenuItemEnum.AMSStores;
        }

        [Security(Permissions = "ViewAMSStores")]
        public ActionResult Index()
        {
            try
            {
                // Get all the stores ams server & ftp sites
                IEnumerable<StoreAMSServerFtpSiteListItem> storeAmsServerFtpSites = this.StoreAMSServerFTPSiteDAO.GetAllListItems();

                return View(storeAmsServerFtpSites);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSStoreController.Index", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [Security(Permissions = "ViewAMSStores")]
        public ActionResult Update(int? id)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Get the store
                    AndroAdminDataAccess.Domain.Store store = this.StoreDAO.GetById(id.Value);

                    if (store == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                    else
                    {
                        // Build a list of AMS servers to display
                        IList<AMSServer> amsServers = this.AMSServerDAO.GetAll();
                        SelectList amsServersSelectList = new SelectList(amsServers, "Id", "DisplayName");

                        // Build a list of FTP sites to display
                        IList<FTPSite> ftpSites = this.FTPSiteDAO.GetAll();
                        SelectList ftpSitesSelectList = new SelectList(ftpSites, "Id", "Name");

                        // Get the stores ams server & ftp sites
                        IEnumerable<StoreAMSServerFtpSite> storeAmsServerFtpSites = this.StoreAMSServerFTPSiteDAO.GetBySiteId(id.Value);

                        // AMS server & ftp sites for this store
                        Dictionary<int, List<StoreAMSServerFtpSite>> storeIndex = new Dictionary<int, List<StoreAMSServerFtpSite>>();

                        foreach (StoreAMSServerFtpSite storeAmsServerFtpSite in storeAmsServerFtpSites)
                        {
                            // Find the ftp sites for this store & AMS server
                            List<StoreAMSServerFtpSite> amsServerFtpSites = null;
                            if (!storeIndex.TryGetValue(storeAmsServerFtpSite.StoreAMSServer.AMSServer.Id, out amsServerFtpSites))
                            {
                                // No ftp sites found for this particular store & AMS server
                                amsServerFtpSites = new List<StoreAMSServerFtpSite>();

                                storeIndex.Add(storeAmsServerFtpSite.StoreAMSServer.AMSServer.Id, amsServerFtpSites);
                            }

                            // Append the ftp server to the store & AMS server
                            amsServerFtpSites.Add(storeAmsServerFtpSite);
                        }

                        // Model for the view only
                        UpdateAMSandFTPSitesModel updateAMSandFtpSites = new UpdateAMSandFTPSitesModel();

                        // AMS servers
                        updateAMSandFtpSites.AMSServers = amsServersSelectList;

                        // FTP sites
                        updateAMSandFtpSites.FTPSites = ftpSitesSelectList;

                        // Store AMS Server Ftp Sites
                        updateAMSandFtpSites.StoreAMSServerFtpSites = storeIndex;

                        updateAMSandFtpSites.Store = store;

                        actionResult = View(updateAMSandFtpSites);
                    }
                }
                else
                {
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSStoreController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [HttpPost]
        [Security(Permissions = "EditAMSStore")]
        public ActionResult Add(int? id, UpdateAMSandFTPSitesModel updateAMSandFtpSites)
        {
            ActionResult actionResult = null;

            try
            {
                // Check if id or updateAMSandFTPSites are null

                // Check for ams server id
                int amsServerId = 0;
                if (!int.TryParse(updateAMSandFtpSites.AMSServerId, out amsServerId))
                {
                    actionResult = RedirectToAction("Index");
                }

                // Check for ftp site id
                int ftpSiteId = 0;
                if (!int.TryParse(updateAMSandFtpSites.FTPSiteId, out ftpSiteId))
                {
                    actionResult = RedirectToAction("Index");
                }

                // Get the store
                Store store = null;
                if (actionResult == null)
                {
                    store = this.StoreDAO.GetById(id.Value);

                    if (store == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }

                // Get the AMS server
                AMSServer amsServer = null;
                if (actionResult == null)
                {
                    amsServer = this.AMSServerDAO.GetById(amsServerId);

                    if (amsServer == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }

                // Get the FTP site
                FTPSite ftpSite = null;
                if (actionResult == null)
                {
                    ftpSite = this.FTPSiteDAO.GetById(ftpSiteId);

                    if (ftpSite == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                }

                // Get StoreAMSServer 
                StoreAMSServer storeAmsServer = null;
                if (actionResult == null)
                {
                    storeAmsServer = this.StoreAMSServerDAO.GetByStoreIdAMServerId(store.Id, amsServer.Id);

                    // If not exist then create it
                    if (storeAmsServer == null)
                    {
                        storeAmsServer = new StoreAMSServer();
                        storeAmsServer.AMSServer = amsServer;
                        storeAmsServer.Store = store;
                        storeAmsServer.Priority = 0;

                        this.StoreAMSServerDAO.Add(storeAmsServer);
                    }
                    else
                    {
                        // Check to see if the StoreAMSServer already has the FTP Site
                        if (StoreAMSServerFTPSiteDAO.GetBySiteIdAMSServerIdFTPSiteId(storeAmsServer.Id, ftpSiteId) != null)
                        {
                            // Already used!
                            TempData["message"] = "Store is already uploading to that AMS Server and FTP Site!!";

                            // Redisplay the view with the error message
                            actionResult = RedirectToAction("Update", new { Id = id });
                        }
                    }
                }

                // Create StoreAMSServerFTPSite
                if (actionResult == null)
                {
                    StoreAMSServerFtpSite storeAmsServerFtpSite = new StoreAMSServerFtpSite();
                    storeAmsServerFtpSite.FTPSite = ftpSite;
                    storeAmsServerFtpSite.StoreAMSServer = storeAmsServer;

                    StoreAMSServerFTPSiteDAO.Add(storeAmsServerFtpSite);

                    // Success
                    TempData["message"] = "AMS Server / FTP site successfully added";
                    actionResult = RedirectToAction("Update", new { Id = id });
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSStoreController.Add (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        public ActionResult Delete(int? siteId, int? id)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Get the StoreAMSServerFTPSite object we're about to delete as we'll need some of it's data afterwards
                    StoreAMSServerFtpSite storeAmsServerFtpSite = this.StoreAMSServerFTPSiteDAO.GetById(id.Value);

                    // Delete the StoreAMSServerFTPSite object
                    this.StoreAMSServerFTPSiteDAO.DeleteById(id.Value);

                    // Get the StoreAMSServerFTPSite object
                    IList<StoreAMSServerFtpSite> storeAmsServerFtpSites = this.StoreAMSServerFTPSiteDAO.GetByStoreAMSServerId(storeAmsServerFtpSite.StoreAMSServer.Id);

                    // Are there any other ftp sites being used to upload to the AMS server for the store
                    if (storeAmsServerFtpSites.Count == 0)
                    {
                        // No other ftp servers.  We need to delete the StoreAMSServer object
                        this.StoreAMSServerDAO.DeleteById(storeAmsServerFtpSite.StoreAMSServer.Id);
                    }

                    actionResult = RedirectToAction("Update", new { id = siteId });

                    TempData["message"] = "Data will no longer be uploaded via the FTP site";
                }
                else
                {
                    actionResult = RedirectToAction("Update", new { id = siteId });
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AMSStoreController.Delete", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }
    }
}
