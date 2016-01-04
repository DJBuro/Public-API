using System;
using System.Threading.Tasks;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Menu
{
    public class FtpUploadDataService : IFtpUploadDataService 
    {
        private readonly IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService; 

        public FtpUploadDataService(IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService)
        {
            this.myAndromedaSiteMenuDataService = myAndromedaSiteMenuDataService;
        }

        public void SetUploadTaskStatus(SiteMenu siteMenu, TaskStatus status)
        {
            var uploadTask = siteMenu.SiteMenuFtpBackupUploadTask;
            switch (status)
            {
                case TaskStatus.Created:
                    {
                        uploadTask.TryTask = true;
                        uploadTask.TaskComplete = false;
                        uploadTask.LastTryCount = 0;

                        break;
                    }
                case TaskStatus.Running:
                    {
                        uploadTask.LastTryCount++;
                        uploadTask.TaskStarted = true;
                        uploadTask.LastStartedUtc = DateTime.UtcNow;
                        uploadTask.LastTriedUtc = DateTime.UtcNow;

                        break;
                    }
                case TaskStatus.RanToCompletion:
                    {
                        uploadTask.TryTask = false;
                        uploadTask.TaskStarted = false;
                        uploadTask.TaskComplete = true;
                        uploadTask.LastCompletedUtc = DateTime.UtcNow;

                        break;
                    }
                case TaskStatus.Faulted:
                    {
                        //reset to run again.
                        uploadTask.TryTask = true;
                        uploadTask.TaskStarted = false;

                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            this.myAndromedaSiteMenuDataService.Update(siteMenu);
        }
    }
}