using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Logging;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public class MenuPubishDataService : IPubishMenuTaskDataService 
    {
        private readonly IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService;

        private readonly IMyAndromedaLogger logger;

        public MenuPubishDataService(IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService, IMyAndromedaLogger logger)
        {
            this.myAndromedaSiteMenuDataService = myAndromedaSiteMenuDataService;
            this.logger = logger;
        }

        public IEnumerable<SiteMenu> GetPublishTasks(DateTime time) 
        {
            IEnumerable<SiteMenu> results = Enumerable.Empty<SiteMenu>();

            using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var data = dbContext.QueryMenusWithTasks(e =>
                                                             !e.SiteMenuPublishTask.TaskStarted &&
                                                             e.SiteMenuPublishTask.TryTask &&
                                                             (e.SiteMenuPublishTask.PublishOn <= time || e.SiteMenuPublishTask.PublishOn == null));

                results = data;
            }

            return results;
        }

        public void AddHistoryLog(SiteMenu sitemenu, string userName, bool publishAll, bool publishMenu, bool publishThumbnails, DateTime? publishOnUtc)
        {
            using (var dbContex = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var table = dbContex.SiteMenuPublishTaskHistories;

                DateTime now = DateTime.UtcNow;
                var entity = table.Create();
                entity.CreatedOnUtc = now;
                entity.SiteMenuId = sitemenu.Id;
                entity.PublishThumbnails = publishThumbnails;
                entity.PublishOnUtc = publishOnUtc.GetValueOrDefault(now);
                entity.PublishedBy = userName;
                entity.PublishAll = publishAll;

                table.Add(entity);

                dbContex.SaveChanges();
            }
        }

        public void SetAcsUploadMenuDataTaskStatus(SiteMenu menu, TaskStatus status, DateTime? date)
        {
            var publishTask = menu.SiteMenuPublishTask;

            this.logger.Debug("Setting menu publish status for {0}: {1}", menu.AndromediaId, status.ToString());

            switch (status)
            {
                case TaskStatus.Created:
                    {
                        publishTask.LastTryCount = 0;
                        publishTask.TryTask = true;
                        publishTask.TaskStarted = false;
                        publishTask.TaskComplete = false;
                        publishTask.PublishOn = date.GetValueOrDefault(DateTime.UtcNow);

                        break;
                    }

                case TaskStatus.Running:
                    {
                        publishTask.TryTask = false;
                        publishTask.TaskStarted = true;
                        publishTask.TaskComplete = false;

                        publishTask.LastStartedUtc = DateTime.UtcNow;
                        publishTask.LastTriedUtc = DateTime.UtcNow;

                        break;
                    }

                case TaskStatus.RanToCompletion:
                    {
                        publishTask.TryTask = false;
                        publishTask.TaskStarted = false;
                        publishTask.TaskComplete = true;
                        publishTask.LastCompletedUtc = DateTime.UtcNow;

                        break;
                    }

                case TaskStatus.Faulted:
                    {
                        //reset to run again.
                        publishTask.TryTask = true;
                        publishTask.TaskStarted = false;
                        publishTask.TaskComplete = false;
                        publishTask.LastTryCount++;

                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            this.myAndromedaSiteMenuDataService.Update(menu);
        }
    }
}