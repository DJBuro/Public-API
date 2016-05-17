using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Castle.Core.Logging;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Menus.Services.Menu;
using System.Threading.Tasks;
using MyAndromeda.Data.MenuDatabase.Context;
using MyAndromeda.Logging;
using MyAndromeda.Web.Models.Api;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Web.Controllers.Api
{
    public class FtpController : ApiController
    {
        private readonly IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService;
        private readonly IFtpMenuManagerService ftpManagerService;
        private readonly IAccessDbMenuVersionDataService dbVersionService;
        private readonly ICurrentSite site;
        private readonly INotifier notifier;
        private readonly IMyAndromedaLogger logger;
        private readonly IDateServices dateServices;

        public FtpController(ICurrentSite site,
            INotifier notifier,
            IFtpMenuManagerService ftpManagerService,
            IAccessDbMenuVersionDataService dbVersionService,
            IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService,
            IMyAndromedaLogger logger,
            IDateServices dateServices)
        {
            this.dateServices = dateServices;
            this.logger = logger;
            this.myAndromedaSiteMenuDataService = myAndromedaSiteMenuDataService;
            this.dbVersionService = dbVersionService;
            this.site = site;
            this.notifier = notifier;
            this.ftpManagerService = ftpManagerService;
        }

        private SiteMenu GetMenu()
        {
            var menu = this.myAndromedaSiteMenuDataService.GetMenu(this.site.AndromediaSiteId);

            return menu;
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/menu/ftp/download")]
        public async Task<HttpResponseMessage> Download()
        {
            try
            {
                var menu = this.GetMenu();

                this.notifier.Notify("Checking for a newer menu on the backup drive. Please wait.");

                var state = ftpManagerService.DownloadMenu(menu);

                ftpManagerService.UpdateDownloadTaskToCompleted(menu);

                if (!state.HasUpdatedMenu.GetValueOrDefault())
                {
                    this.notifier.Notify("A newer database was not found.");
                }

                if (state.HasUpdatedMenu.GetValueOrDefault())
                {
                    this.notifier.Notify("A newer database was selected. Please refresh if you are using the menu editor.", true);
                }
            }
            catch (Exception e)
            {
                this.logger.Error("Failed to get a menu");
                this.logger.Error(e);
                this.notifier.Error("Failed to get a menu.");

                throw;
            }
            
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/menu/ftp/upload")]
        public async Task<HttpResponseMessage> Upload()
        {
            var completed = false;
            var menu = this.GetMenu();

            this.notifier.Notify("The menu upload task has started.");

            var state = ftpManagerService.UploadMenu(site.AndromediaSiteId);

            if (state.HasUpdatedMenu.GetValueOrDefault())
            {
                ftpManagerService.UpdatePublishTask(menu, TaskStatus.RanToCompletion);
                completed = true;

                if (state.HasUpdatedMenu.GetValueOrDefault())
                {
                    this.notifier.Notify("The database has been backed up.");
                }
            }
            else
            {
                ftpManagerService.UpdatePublishTask(menu, TaskStatus.Faulted);
                this.notifier.Error("The database failed to be backed up. Please try again.", false);

                throw new Exception("The task did not complete");
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
            //return new HttpRequestMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/menu/version")]
        public AccessMenuVersion Version()
        {
            if (!this.dbVersionService.IsAvailable(this.site.AndromediaSiteId))
            {
                return new AccessMenuVersion()
                {
                    Version = "There is no menu. Try downloading a menu",
                    UpdatedOn = ""
                };
                    //"No version can be obtained.";
            }

            var row = dbVersionService.GetMenuVersionRow(this.site.AndromediaSiteId);
            var menu = myAndromedaSiteMenuDataService.GetMenu(this.site.AndromediaSiteId);
            
            //this.notifier.Notify("The current menu version is:" + row.nVersion, true);
            //return "" + row.nVersion;
            return new AccessMenuVersion() 
            {
                Version = row.nVersion.ToString(),
                UpdatedOn = row.tLastUpdated.ToString(),
                LastDownloaded = dateServices.ConvertToLocalString(menu.SiteMenuFtpBackupDownloadTask.LastCompletedUtc.GetValueOrDefault())
                //menu.SiteMenuFtpBackupDownloadTask.LastCompletedUtc.GetValueOrDefault().ToString()
            };
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/menu/delete")]
        public async Task<HttpResponseMessage> Delete() 
        {
            MenuConnectionStringContext context = new MenuConnectionStringContext(this.site.AndromediaSiteId);

            string localPath = context.LocalFullPath;

            if (System.IO.File.Exists(localPath))
            {
                try
                {
                    System.IO.File.Delete(localPath);
                }
                catch (Exception e) 
                {
                    this.logger.Error("Error deleting the mdb file",e);
                    this.notifier.Error("There is a problem deleting the file.");
                }
            }
            this.notifier.Notify("The file has been deleted.");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
