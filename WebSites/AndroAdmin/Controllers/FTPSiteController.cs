using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdmin.DataAccess;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.Model;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewFTPSites")]
    public class FTPSiteController : BaseController
    {
        public FTPSiteController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.AMS;
            ViewBag.SelectedAMSMenu = AMSMenuItemEnum.FTPSites;
        }

        [Security(Permissions = "ViewFTPSites")]
        public ActionResult Index()
        {
            ActionResult actionResult = null;

            try
            {
                // Get all the ftp sites
                IList<FTPSite> ftpSites = this.FTPSiteDAO.GetAll();

                actionResult = View(ftpSites);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("FTPSiteController.Index", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "AddFTPSite")]
        public ActionResult Add()
        {
            // View model
            FTPSiteViewModel viewModel = new FTPSiteViewModel();
            viewModel.FtpSite = new FTPSite();

            // Get all the ftp site types
            viewModel.FtpSiteTypes = this.FTPSiteTypeDAO.GetAll();

            // Default to SFTP
            foreach (FTPSiteType ftpSiteType in viewModel.FtpSiteTypes)
            {
                if (ftpSiteType.Name == "SFTP")
                {
                    viewModel.FTPSiteType = ftpSiteType.Id.ToString();
                    viewModel.FtpSite.Port = 22;
                    break;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [Security(Permissions = "AddFTPSite")]
        public ActionResult Add(FTPSiteViewModel ftpSiteViewModel)
        {
            ActionResult actionResult = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    actionResult = View(ftpSiteViewModel);
                }

                if (actionResult == null)
                {
                    // Get all the ftp site types that can be used with a drop down combo
                    ftpSiteViewModel.FtpSiteTypes = this.FTPSiteTypeDAO.GetAll();
                }

                if (actionResult == null)
                {
                    // Has the store id already been used?
                    if (this.FTPSiteDAO.GetByName(ftpSiteViewModel.FtpSite.Name) != null)
                    {
                        ModelState.AddModelError("FtpSite.Name", "Name already used");
                        actionResult = View(ftpSiteViewModel);
                    }
                }

                if (actionResult == null)
                {
                    // Figure out which ftp site type was selected (can't data bind complex data types to a drop down combo)
                    int ftpSiteTypeId = int.Parse(ftpSiteViewModel.FTPSiteType);

                    foreach (FTPSiteType ftpSiteType in ftpSiteViewModel.FtpSiteTypes)
                    {
                        if (ftpSiteType.Id == ftpSiteTypeId)
                        {
                            ftpSiteViewModel.FtpSite.FTPSiteType = ftpSiteType;
                            break;
                        }
                    }

                    // Add the ftp site
                    this.FTPSiteDAO.Add(ftpSiteViewModel.FtpSite);

                    TempData["message"] = "FTP site successfully added";
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("FTPSiteController.Add (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "EditFTPSite")]
        public ActionResult Update(int? id)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // View model
                    FTPSiteViewModel ftpSiteViewModel = new FTPSiteViewModel();

                    // Get all the ftp sites
                    ftpSiteViewModel.FtpSite = this.FTPSiteDAO.GetById(id.Value);

                    // Get all the ftp site types that can be used with a drop down combo
                    ftpSiteViewModel.FtpSiteTypes = this.FTPSiteTypeDAO.GetAll();

                    ftpSiteViewModel.FTPSiteType = ftpSiteViewModel.FtpSite.FTPSiteType.Id.ToString();

                    if (ftpSiteViewModel.FtpSite == null)
                    {
                        actionResult = RedirectToAction("Index");
                    }
                    else
                    {
                        actionResult = View(ftpSiteViewModel);
                    }
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

        [HttpPost]
        [Security(Permissions = "EditFTPSite")]
        [ValidateInput(false)]
        public ActionResult Update(FTPSiteViewModel ftpSiteViewModel)
        {
            ActionResult actionResult = null;

            try
            {
                // Get all the ftp site types that can be used with a drop down combo
                ftpSiteViewModel.FtpSiteTypes = this.FTPSiteTypeDAO.GetAll();

                if (!ModelState.IsValid)
                {
                    actionResult = View(ftpSiteViewModel);
                }

                if (actionResult == null)
                {
                    // Has the store id already been used?
                    FTPSite ftpSite = this.FTPSiteDAO.GetByName(ftpSiteViewModel.FtpSite.Name);
                    if (ftpSite != null && ftpSite.Id != ftpSiteViewModel.FtpSite.Id)
                    {
                        ModelState.AddModelError("FtpSite.Name", "Name already used");
                        actionResult = View(ftpSiteViewModel);
                    }
                }

                if (actionResult == null)
                {
                    // Figure out which ftp site type was selected (can't data bind complex data types to a drop down combo)
                    int ftpSiteTypeId = int.Parse(ftpSiteViewModel.FTPSiteType);

                    foreach (FTPSiteType ftpSiteType in ftpSiteViewModel.FtpSiteTypes)
                    {
                        if (ftpSiteType.Id == ftpSiteTypeId)
                        {
                            ftpSiteViewModel.FtpSite.FTPSiteType = ftpSiteType;
                            break;
                        }
                    }

                    // Update the ftp site
                    this.FTPSiteDAO.Update(ftpSiteViewModel.FtpSite);

                    TempData["message"] = "FTP site successfully updated";
                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("FTPSiteController.Update (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "DeleteFTPSite")]
        public ActionResult Delete(int? id)
        {
            ActionResult actionResult = null;

            try
            {
                if (id.HasValue)
                {
                    // Delete the StoreAMSServerFTPSite objects that reference the ftp site
                    this.StoreAMSServerFTPSiteDAO.DeleteByFTPSiteId(id.Value);

                    // Delete the ftp sites
                    this.FTPSiteDAO.Delete(id.Value);

                    actionResult = RedirectToAction("Index");

                    TempData["message"] = "FTP site successfully deleted";
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
