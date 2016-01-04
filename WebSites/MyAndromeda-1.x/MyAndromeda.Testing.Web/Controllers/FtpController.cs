using MyAndromeda.Configuration;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Menus.Ftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MyAndromeda.Testing.Web.Controllers
{
    public class FtpController : Controller
    {
        private readonly IMenuFtpService ftpMenuService;
        private readonly IMyAndromedaLogger logger;

        //
        // GET: /Ftp/

        

        public FtpController(IMenuFtpService ftpMenuService, IMyAndromedaLogger logger)
        {
            this.ftpMenuService = ftpMenuService;
            this.logger = logger;
        }

        public ActionResult Original(int id) 
        {
            var context = this.ftpMenuService.CopyMenuDown(id, null);

            return View(context);
        }

        public ActionResult Index(int id)
        {
            //FtpService.CheckFolder(id);
            //FtpService.Download(id);
            //FtpService.Download(999999);
            //using (var ftp = FtpService.CreateAndLogin()) 
            //{
            //    if (ftp.GetFileGroup(id, files => ftp.DownloadFile(id))) 
            //    { 
            //    }
            //}
            //using (var ftp = FtpService.CreateAndLogin()) 
            //{
            //    if (ftp.GetFileGroup(99999, files => { })) 
            //    {
            //    }
            //}
            using (var ftp = FtpService.Create()) {
                var directories = ftp.GetDirectories(ftp.RemoteMenuUri());
                
                if (ftp.HasFile(id))
                { 
                    this.logger.Debug("has file {0}", id);
                
                    ftp.GetFiles(id);

                    ftp.DownloadMenu(id);
                    this.logger.Debug("downloaded file");
                }
            }

            using (var ftp = FtpService.Create())
            {
                if (ftp.HasFile(9999))
                {
                    ftp.GetFiles(9999);
                    ftp.DownloadMenu(id);
                }
            }       

            return View();
        }

        public ActionResult Basic(int id) 
        {
            var result = false;
            var downloadTo = string.Format(@"|DataDirectory|\Menus\{0}\", id);
            var fullPath = downloadTo.Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"));

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            fullPath = Path.Combine(fullPath, "menu.7z");


            var request = FtpWebRequest.Create(string.Format("ftp://{0}/Menus/{1}/menu.7z", MenuFtpSettings.Host, id)) as FtpWebRequest;

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(MenuFtpSettings.UserName.Trim(), MenuFtpSettings.Password.Trim());
            request.UsePassive = MenuFtpSettings.TransferMode.ToLower() == "passive";

            try
            {
                var response = request.GetResponse() as FtpWebResponse;
                var responseStream = response.GetResponseStream();

                var file = System.IO.File.Create(fullPath);
                var buffer = new byte[32 * 1024];
                int read;

                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, read);
                }

                file.Close();
                responseStream.Close();
                response.Close();
                result = true;
            }
            catch (WebException e)
            {
                if (e.Message == "The remote server returned an error: (550) File unavailable (e.g., file not found, no access).")
                {
                    result = false;
                }
                else { throw e; }
            }

            return View();
        }

        public ActionResult Basic2(int id) 
        {

            var downloadTo = string.Format(@"|DataDirectory|\Menus\{0}\", id);
            var fullPath = downloadTo.Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"));

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            fullPath = Path.Combine(fullPath, "menu.7z");

            var rempoteDownloadPath = string.Format("ftp://{0}/Menus/{1}/menu.7z", MenuFtpSettings.Host, id);

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(rempoteDownloadPath);
            request.UsePassive = MenuFtpSettings.TransferMode.ToLower() == "passive";
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(MenuFtpSettings.UserName.Trim(), MenuFtpSettings.Password.Trim());
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            FileStream file = System.IO.File.Create(fullPath);
            byte[] buffer = new byte[32 * 1024];
            int read;
            //reader.Read(

            while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                file.Write(buffer, 0, read);
            }

            file.Close();
            responseStream.Close();
            response.Close();

            return View();
        }

        public ActionResult Upload(int id) 
        {
            using (var ftp = FtpService.Create())
            {
                //if (ftp.HasFile(id))
                {
                    ftp.GetFiles(id);
                    ftp.UploadZip(id);
                    ftp.DeleteFile(id);
                    ftp.RenameTempZipToFinal(id);
                }
            }

            return View();
        }

    }
}
