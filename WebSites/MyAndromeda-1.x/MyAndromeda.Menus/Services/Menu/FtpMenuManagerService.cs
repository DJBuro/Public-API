using MyAndromeda.Core.Authorization;
using MyAndromeda.Data.MenuDatabase.Context;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Context.Ftp;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Ftp;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyAndromeda.Menus.Services.Menu
{
    public class FtpMenuManagerService : IFtpMenuManagerService
    {
        private readonly Lazy<IMenuFtpService> ftpMenuService;
       
        private readonly IMyAndromedaLogger logger;
        private readonly IAccessDbMenuVersionDataService accessMenuDbVersion;
        private readonly IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService;
        private readonly IFtpDownloadDataService ftpDownloadService;
        private readonly IFtpUploadDataService ftpUploadDataService;
        private readonly IAccessDbMenuVersionDataService accessDbMenuVersionDataService;

        /* events */
        private readonly IAccessDatabaseEvent[] events;
        private readonly IFtpEvents[] ftpEvents;

        public FtpMenuManagerService(IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService,
            IAccessDatabaseEvent[] events,
            IFtpEvents[] ftpEvents,
            IMyAndromedaLogger logger,
            IAccessDbMenuVersionDataService accessMenuDbVersion,
            IFtpDownloadDataService ftpDownloadService,
            IFtpUploadDataService ftpUploadDataService,
            Lazy<IMenuFtpService> ftpMenuService,
            IAccessDbMenuVersionDataService accessDbMenuVersionDataService)
        {
            this.ftpUploadDataService = ftpUploadDataService;
            this.ftpDownloadService = ftpDownloadService;
            this.logger = logger;
            this.accessMenuDbVersion = accessMenuDbVersion;
            this.ftpMenuService = ftpMenuService;
            this.myAndromedaSiteMenuDataService = myAndromedaSiteMenuDataService;
            this.events = events;
            this.ftpEvents = ftpEvents;
            this.accessDbMenuVersionDataService = accessDbMenuVersionDataService;
        }

        public void Dispose()
        {
            if (this.ftpMenuService.IsValueCreated)
            { 
                this.ftpMenuService.Value.Dispose();
            }
        }

        public FtpMenuContext DownloadMenu(SiteMenu siteMenu)
        {
            foreach (var ev in this.events)
            {
                ev.Notify(siteMenu.AndromediaId, "Request to download");
            }

            this.ftpDownloadService.SetDownloadTaskStatus(siteMenu, TaskStatus.Running);
            
            //awaken the ftp service 
            var ftp = this.ftpMenuService.Value;
            var menuPublishDate = ftp.CheckFtpToPublishDate(siteMenu.AndromediaId);

            if (menuPublishDate > siteMenu.SiteMenuPublishTask.LastKnownFtpSitePublish) 
            {
                siteMenu.SiteMenuPublishTask.LastKnownFtpSitePublish = menuPublishDate;
                this.ftpDownloadService.FoundNewPublishDate(siteMenu, menuPublishDate);
            }

            var context = ftp.CopyMenuDown(siteMenu.AndromediaId, siteMenu.SiteMenuFtpBackupDownloadTask.LastDownloadedDateUtc);

            if (!context.HasFoundFolder.GetValueOrDefault() || !context.HasFoundFile.GetValueOrDefault()) 
            {
                return context;
            }

            if (context.HasDownloadedMenu.GetValueOrDefault()) 
            {
                siteMenu.SiteMenuFtpBackupDownloadTask.LastDownloadedDateUtc = DateTime.UtcNow;
            }

            this.UpdateDownloadTaskToCompleted(siteMenu);

            if (context.HasDownloadedMenu.GetValueOrDefault() && 
                context.HasUnzippedMenu.GetValueOrDefault())
            {
                this.logger.Debug("Downloaded, Unzipped, checking which is newer");
                this.ReplaceCurrentDbIfNewer(context, siteMenu.AndromediaId);
                
                this.myAndromedaSiteMenuDataService.SetVersion(siteMenu);
            }
            else
            {
                foreach (var ev in this.events)
                {
                    ev.Notify(siteMenu.AndromediaId, "A Menu was not downloaded. (not needed)");
                }
            }

            var menuVersion = this.accessMenuDbVersion.GetMenuVersionRow(siteMenu.AndromediaId);
            siteMenu.AccessMenuVersion = menuVersion.nVersion;

            //else it didn't download
            return context;
        }

        public FtpMenuContext UploadMenu(int andromedaSiteId)
        {
            var menu = this.myAndromedaSiteMenuDataService.GetMenu(andromedaSiteId);
            var menuVersion = menu.AccessMenuVersion;

            FtpMenuContext context;
            foreach (var ev in this.events)
            {
                ev.Notify(andromedaSiteId, "Request to upload");
            }

            try
            {
                var ftp = this.ftpMenuService.Value;

                var publishDate = menu.SiteMenuPublishTask.PublishOn.GetValueOrDefault(DateTime.UtcNow);
                
                context = ftp.CopyMenuUp(andromedaSiteId,publishDate, menuVersion.ToString());
            }
            catch (Exception e)
            {
                Exception exception = e;
                while (exception != null)
                {
                    foreach (var ev in this.ftpEvents)
                    {
                        ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedRoles.Administrator), string.Format("Exception for {0}: {1}", andromedaSiteId, exception.Message));
                        ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedRoles.Administrator), string.Format("Exception source {0}: {1}", andromedaSiteId, exception.Source));
                        ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedRoles.Administrator), string.Format("Exception source {0}: {1}", andromedaSiteId, exception.StackTrace));

                        exception = exception.InnerException;
                    }
                }
                throw e;
            }

            return context;
        }

        public SiteMenu GetMenu(int andromedaSiteId)
        {
            return this.myAndromedaSiteMenuDataService.GetMenu(andromedaSiteId);
        }

        public bool AddTaskToDownloadTheMenu(SiteMenu siteMenu, TimeSpan throttleTime)
        {
            if (siteMenu.SiteMenuFtpBackupDownloadTask.TryTask) { return true; }

            var lastFtpCheck = siteMenu.SiteMenuFtpBackupDownloadTask.LastTriedUtc;
            
            //it has never run before 
            if (throttleTime == null || !lastFtpCheck.HasValue)
            {
                this.ftpDownloadService.SetDownloadTaskStatus(siteMenu, TaskStatus.Created);

                return true;
            }

            var minimumTimeToStart = lastFtpCheck.Value.Add(throttleTime);
            
            //last checked within the throttle time
            if (DateTime.UtcNow <= minimumTimeToStart)
            {
                if (this.accessMenuDbVersion.IsAvailable(siteMenu.AndromediaId))
                {
                    return false;
                }
            }
            
            this.ftpDownloadService.SetDownloadTaskStatus(siteMenu, TaskStatus.Created);

            return true;
        }
       

        public void UpdateDownloadTaskToCompleted(SiteMenu siteMenu)
        {
            this.ftpDownloadService.SetDownloadTaskStatus(siteMenu, TaskStatus.RanToCompletion);
        }

        public void UpdateDownloadTaskToFailed(SiteMenu siteMenu)
        {
            this.ftpDownloadService.SetDownloadTaskStatus(siteMenu, TaskStatus.Faulted);
        }

        public void CreatePublishFtpTask(SiteMenu siteMenu, DateTime? dateTimeUtc)
        {
            if (!accessDbMenuVersionDataService.IsAvailable(siteMenu.AndromediaId)) 
            {
                return; 
            }

            this.accessDbMenuVersionDataService.IncrementVersion(siteMenu.AndromediaId);
            this.ftpUploadDataService.SetUploadTaskStatus(siteMenu, TaskStatus.Created);
        }


        public void UpdatePublishTask(SiteMenu siteMenu, TaskStatus status)
        {
            this.ftpUploadDataService.SetUploadTaskStatus(siteMenu, status);
        }


        public IEnumerable<SiteMenu> GetQueueToDownload()
        {
            return this.myAndromedaSiteMenuDataService.List(e => 
                e.SiteMenuFtpBackupDownloadTask.TryTask && 
                !e.SiteMenuFtpBackupDownloadTask.TaskStarted
            );
        }

        public IEnumerable<SiteMenu> GetQueueToUpload()
        {
            return this.myAndromedaSiteMenuDataService.List(e => 
                e.SiteMenuFtpBackupUploadTask.TryTask && !e.SiteMenuFtpBackupUploadTask.TaskStarted
            );
        }

        public void SetVersion(SiteMenu siteMenu)
        {
            this.myAndromedaSiteMenuDataService.SetVersion(siteMenu);
        }

        private void ReplaceCurrentDbIfNewer(FtpMenuContext context, int andromedaSiteId) 
        {
            var compareService = new AccessDbMenuVersionDataService(logger);

            var latest = compareService.GetLatest(andromedaSiteId);
            var menu = this.myAndromedaSiteMenuDataService.GetMenu(andromedaSiteId);
           
            menu.SiteMenuFtpBackupDownloadTask.LastDownloadedDateUtc = DateTime.UtcNow;
              
            this.myAndromedaSiteMenuDataService.Update(menu);

            switch (latest) 
            {
                //no need to do anything cool :(
                case LatestMenuVersion.Menu:
                    {
                        context.HasUpdatedMenu = false;

                        this.logger.Debug("A new version of a database was not needed for: {0}", andromedaSiteId);
                        foreach (var ev in this.events)
                        {
                            ev.Notify(andromedaSiteId, "A New version of the menu is NOT needed.");
                            ev.DatabaseNotChanged(new DatabaseUpdatedEventContext(andromedaSiteId));
                        }

                        break; 
                    }

                //overwrite the current version. Temp is newer
                case LatestMenuVersion.Temp:
                    {
                        //to-do : copy temp over the menu file
                        var menuConnectionContexts = new MenuConnectionStringContext(andromedaSiteId);
                        File.Copy(menuConnectionContexts.LocalTempPath, menuConnectionContexts.LocalFullPath, true);

                        context.HasUpdatedMenu = true;

                        this.logger.Debug("A new version of a database is being copied for: {0}", andromedaSiteId);
                        foreach (var ev in this.events)
                        {
                            ev.Notify(andromedaSiteId, "A New version of the menu is being copied.");
                            ev.CopiedDatabase(new DatabaseUpdatedEventContext(andromedaSiteId));
                        }

                        break;
                    }
                //should NEVER get here. 
                default:
                    {
                        throw new Exception("I cant do this");
                    }
            }
        }
    }
}