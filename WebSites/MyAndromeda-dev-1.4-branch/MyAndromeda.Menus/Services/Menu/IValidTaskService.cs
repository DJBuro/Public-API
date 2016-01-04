using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;

namespace MyAndromeda.Menus.Services.Menu
{
    public interface IValidTaskService : ITransientDependency
    {
        /// <summary>
        /// Checks that the tasks are valid. That none have cancelled in a fatal way such that it could not be defined to restart
        /// </summary>
        /// <param name="utcNow">The UTC now.</param>
        void CheckAndCorrectTasks(DateTime utcNow);
    }

    public class ValidTaskService : IValidTaskService 
    {
        private readonly IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService;
        private readonly IFtpMaintainenceDataService ftpMaintainenceDataService;
        private readonly IMyAndromedaLogger logger;

        public ValidTaskService(IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService, IMyAndromedaLogger logger, IFtpMaintainenceDataService ftpMaintainenceDataService)
        {
            this.ftpMaintainenceDataService = ftpMaintainenceDataService;
            this.logger = logger;
            this.myAndromedaSiteMenuDataService = myAndromedaSiteMenuDataService;
        }

        public void CheckAndCorrectTasks(DateTime utcNow)
        {
            var downloadTasksToReset = this.ftpMaintainenceDataService.ResetDownloadTasks(utcNow);
            var uploadTasksToReset = this.ftpMaintainenceDataService.ResetUploadTasks(utcNow);

            foreach (var task in downloadTasksToReset) 
            {
                this.logger.Error("Reset task for {0}", task.AndromediaId);
            }

            foreach (var task in uploadTasksToReset) 
            {
                this.logger.Error("Reset task for {0}", task.AndromediaId);
            }
        }
    }
}