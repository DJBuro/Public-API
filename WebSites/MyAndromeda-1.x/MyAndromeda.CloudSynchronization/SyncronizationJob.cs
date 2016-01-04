using CloudSync;
using MyAndromeda.CloudSynchronization.Events;
using MyAndromeda.CloudSynchronization.Services;
using MyAndromeda.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyAndromeda.WebApiClient.Notifications;
using WebBackgrounder;

namespace MyAndromeda.CloudSynchronization
{
    public class SyncronizationJob : Job
    {
        public SyncronizationJob(TimeSpan interval, TimeSpan timeout) : base("Synchronization job", interval, timeout)
        {
        }

        public override Task Execute()
        {
            return new Task(this.Run);
        }

        public void Run() 
        {
            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            
            var dbService = DependencyResolver.Current.GetService<ISynchronizationTaskService>();
            var syncEvents = DependencyResolver.Current.GetServices<ISynchronizationEvent>();
            var notificationSerive = DependencyResolver.Current.GetService<INotifyWebCallerController>();

            var models = dbService.GetTasksToRun(DateTime.UtcNow).ToArray();

            if (models.Length > 0)
            { 
                logger.Debug("Running sync service for {0} synchronization tasks", models.Length);
            }

            var context = new SyncronizationContext()
            {
                Note = "Task",
                Count = models.Length,
                TimeStamp = DateTime.UtcNow,
                StoreIds = models.Select(e => e.StoreId.GetValueOrDefault()).ToArray()
            };

            if (!models.Any())
            {
                foreach (var handler in syncEvents)
                {
                    handler.Skipping(context);
                }

                return;
            }
            
            foreach (var handler in syncEvents)
            {
                handler.Started(context);
            }
            
            string errorMessage = string.Empty;

            var notificationServiceContext = new Framework.Notification.NotificationContext()
            {
                UserName = "MyAndromeda",
                NotifyOthersLoggedIntoStore = true,
                Message = "Synchronization completed without any errors"
            };
            var notificationErrorServiceContext = new Framework.Notification.NotificationContext()
            {
                UserName = "MyAndromeda",
                NotifyOthersLoggedIntoStore = true,
                Message = "Synchronization failed: "
            };


            try
            {
                errorMessage = SyncHelper.ServerSync();

               

                if (string.IsNullOrEmpty(errorMessage)) { }
                
            }
            catch (Exception e) 
            {
                logger.Error(e);
                errorMessage = e.Message;
            }

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                dbService.CompleteTasks(models);
                logger.Debug("Completed syncing {0} tasks", models.Count());

                foreach (var id in context.StoreIds)
                {
                    notificationSerive.Success(id, notificationServiceContext);
                }

                foreach (var handler in syncEvents)
                {
                    handler.Completed(context);
                }

                return;
            }
            else 
            {
                notificationErrorServiceContext.Message += errorMessage;
            }

            foreach (var id in context.StoreIds)
            {
                notificationSerive.Error(id, notificationErrorServiceContext);
            }

            foreach (var handler in syncEvents) 
            {
                
                handler.Error(context, errorMessage);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage)) { 
                logger.Error(errorMessage);
                logger.Error("{0} remain to synchronize", models.Count());
            }

            logger.Debug("Synchronization Task Job Completed");
        }
    }
}
