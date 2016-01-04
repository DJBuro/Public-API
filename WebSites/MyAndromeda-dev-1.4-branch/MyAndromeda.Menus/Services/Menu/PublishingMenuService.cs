using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyAndromeda.CloudSynchronization.Services;
using MyAndromeda.Menus.Services.Export;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromeda.Menus.Services.Menu
{
    /// <summary>
    /// Todo: destroy this. 
    /// </summary>
    public class PublishingMenuService : IPublishingMenuService 
    {
        private readonly IMenuThumbnailSyncService menuSyncService;
        private readonly IPubishMenuTaskDataService publishDataService;
        private readonly ISynchronizationTaskService synchronizationTaskService;

        public PublishingMenuService(IPubishMenuTaskDataService publishDataService,
            IMenuThumbnailSyncService menuSyncService,
            ISynchronizationTaskService synchronizationTaskService) 
        { 
            this.synchronizationTaskService = synchronizationTaskService;
            this.menuSyncService = menuSyncService;
            this.publishDataService = publishDataService;
        }

        public void CreateSynchronizationTask(SiteMenu siteMenu)
        {
            this.synchronizationTaskService.CreateTask(new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.CloudSynchronizationTask()
            {
                Completed = false,
                Description = "Site menu thumbnails have been updated",
                Name = "Site Update",
                Timestamp = DateTime.UtcNow,
                StoreId = siteMenu.AndromediaId,
                InvokedByUserId = null,
                InvokedByUserName = "Publish menu services"
            });
        }

        public void PublishThumbnailJsonAndXml(SiteMenu siteMenu)
        {
            this.menuSyncService.CreateDataStructureForJsonAndXml(siteMenu.AndromediaId);
        }

        public void PublishMenuLater(SiteMenu sitemenu, DateTime? dateUtc)
        {
            this.publishDataService.SetAcsUploadMenuDataTaskStatus(sitemenu, TaskStatus.Created, dateUtc);
        }

        public void PublishNow(SiteMenu sitemenu)
        {
            this.publishDataService.SetAcsUploadMenuDataTaskStatus(sitemenu, TaskStatus.Created, DateTime.UtcNow);
        }

        public IEnumerable<SiteMenu> GetMenusToPublish(DateTime dateUtc)
        {
            return publishDataService.GetPublishTasks(dateUtc);
        }

        public void AddHistoryLog(SiteMenu sitemenu, string userName, bool publishAll, bool publishMenu, bool publishThumbnails, DateTime? publishOn)
        {
            this.publishDataService.AddHistoryLog(sitemenu, userName, publishAll, publishMenu, publishThumbnails, publishOn);
        }

        public void StartedTask(SiteMenu sitemenu)
        {
            this.publishDataService.SetAcsUploadMenuDataTaskStatus(sitemenu, TaskStatus.Running);
        }

        public void CompletedTask(SiteMenu sitemenu)
        {
            this.publishDataService.SetAcsUploadMenuDataTaskStatus(sitemenu, TaskStatus.RanToCompletion);
        }

        public void FailedTask(SiteMenu siteMenu)
        {
            this.publishDataService.SetAcsUploadMenuDataTaskStatus(siteMenu, TaskStatus.Faulted);
        }
    }
}