using AndroAdmin.Helpers;
using AndroAdminDataAccess.DataAccess;
using AndroAdminDataAccess.Domain;
using AndroAdminDataAccess.EntityFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Kendo.Mvc.UI;
using System.IO;
using System.Web.Configuration;

namespace AndroAdmin.Controllers
{
    public class AndroWebOrderingThemeController : BaseController
    {

        public ActionResult Index()
        {
            IList<ThemeAndroWebOrdering> themes = new List<ThemeAndroWebOrdering>();            
            themes = this.AndroWebOrderingThemeDAO.GetAll();            
            return View(themes);
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            ThemeAndroWebOrdering theme = new ThemeAndroWebOrdering();
            if (Id != null)
            {
                theme = this.AndroWebOrderingThemeDAO.GetAndroWebOrderingThemeById(Convert.ToInt32(Id));                
            }
            return View(theme);
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, ThemeAndroWebOrdering theme)
        //public ActionResult Edit(HttpPostedFileBase file, HttpPostedFileBase zipfile, ThemeAndroWebOrdering theme)
        {
            if (theme != null)
            {
                string url = string.Empty;
                theme.Height = Convert.ToInt32(ConfigurationManager.AppSettings["ThemeHeight"]);
                theme.Width = Convert.ToInt32(ConfigurationManager.AppSettings["ThemeWidth"]);

                //Upload image and get image path
                if (theme.Id == 0 || theme.Id < 0)
                {
                    theme.Source = string.Empty;
                    theme = this.AndroWebOrderingThemeDAO.Add(theme);
                }

                if (file != null)
                {
                  url = UploadImage(file, "themes", GetThemeName(theme.Id, false) + Path.GetExtension(file.FileName), theme.Height, theme.Width);
                }
                //if (zipfile != null)
                //{ 
                //    // un-zip the files to temp folder; upload the files with the folder-structure to Amazon-S3.
                //}

                theme.InternalName =  GetThemeName(theme.Id, true) + ".png";
                theme.Source = url;// "themes/" + theme.InternalName;
                this.AndroWebOrderingThemeDAO.Update(theme);
            }

            return RedirectToAction("Index");
        }

        private bool UploadZIPFile(HttpPostedFileBase zipFile)
        {
            string tmpFolder = HttpContext.Server.MapPath("tmpZipFiles");
            return true;
        }

        private string UploadImage(HttpPostedFileBase file, string folderPath, string fileName, int height, int width)
        {
            MemoryStream origin = new MemoryStream();
            file.InputStream.CopyTo(origin);
            origin.Seek(0, SeekOrigin.Begin);

            ThemeFileResult result = ThemeUploader.ImportMedia(origin, folderPath, fileName, width, height);
            return result.Url;
        }

        private string GetRemoteLocationPath()
        {
            var host = WebConfigurationManager.AppSettings["AndroAdmin.Theme.Host"].ToString();
            AzureStorage azureStorage = new AzureStorage();
            string remoteLocation = azureStorage.RemoteLocation(host);
            return remoteLocation;
        }

        private string GetThemeName(int id, bool includeSize)
        {
            int height = Convert.ToInt32(ConfigurationManager.AppSettings["ThemeHeight"]);
            int width = Convert.ToInt32(ConfigurationManager.AppSettings["ThemeWidth"]);
            return "theme_" + id + "_" +(includeSize ?  (height + "x" + width) : string.Empty);
        }

    }
}
