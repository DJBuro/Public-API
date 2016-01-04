using MyAndromeda.Core.Authorization;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Services.Menu;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;
using WebBackgrounder;
using System.Threading;

namespace MyAndromeda.Menus.Jobs
{
    public class SyncFtpDownJob : Job
    {
        public static TimeSpan Timeout
        {
            get { return TimeSpan.FromMinutes(4); }
        }

        public const string JobName = "Ftp Menu Sync job";

        public SyncFtpDownJob(TimeSpan interval, TimeSpan timeout) : base(JobName, interval, timeout)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("new {0}", JobName));
        }

        public override Task Execute()
        {
            return new Task(this.Run);
        }

        public void Run() 
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Starting: {0}", JobName));
            
            var taskValidator = DependencyResolver.Current.GetService<IValidTaskService>();

            LogStartOfJob();

            taskValidator.CheckAndCorrectTasks(DateTime.UtcNow.Subtract(Timeout));

            using (var ftpMenuService = DependencyResolver.Current.GetService<IFtpMenuManagerService>())
            {
                var sitesToFetch = ftpMenuService
                    .GetQueueToDownload()
                    .ToArray();

                LogStartOfTasks(sitesToFetch);

                var taskList = sitesToFetch.Select(e => StartNewTask(e)).ToArray();
                
                Task.WaitAll(taskList);

                var failedCount = taskList.Where(e => !e.Result).Count();
                var completeCount = taskList.Where(e => e.Result).Count();

                LogCompletion(completeCount, failedCount);
            }
        }

        private static Task<bool> StartNewTask(SiteMenu siteMenu)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task.Factory.StartNew(() =>
            {
                var result = false;

                var service = DependencyResolver.Current.GetService<IFtpMenuManagerService>();

                try
                {
                    LogStartOfTask(siteMenu);

                    service.DownloadMenu(siteMenu);

                    service.UpdateDownloadTaskToCompleted(siteMenu);

                    LogEndOfTask(siteMenu);

                    result = true;
                }
                catch (Exception ex)
                {
                    LogFailureOfTask(siteMenu, ex);

                    service.UpdateDownloadTaskToFailed(siteMenu);
                }

                return result;
            });

            return task;
        }

        #region Log Messages
        
        private static void LogStartOfJob()
        {
            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            var ftpEvents = DependencyResolver.Current.GetServices<IFtpEvents>();
            var startedMessage = "Download ftp Job started";
            
            logger.Info(startedMessage);

            foreach (var ev in ftpEvents) 
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), startedMessage);
            }
        }

        private static void LogStartOfTasks(SiteMenu[] sitesToFetch)
        {
            var ftpEvents = DependencyResolver.Current.GetServices<IFtpEvents>();
            var message = string.Format("Ftp download tasks to process: {0}", sitesToFetch.Length);
                
            Trace.WriteLine("Ftp Menus to fetch:" + sitesToFetch.Length);

            foreach (var ev in ftpEvents)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), message);
            }
        }

        private static void LogStartOfTask(SiteMenu siteMenu) 
        {
            var events = DependencyResolver.Current.GetServices<IAccessDatabaseEvent>();
            foreach (var ev in events)
            {
                ev.StartedCheckingForDatabase(new DatabaseUpdatedEventContext(siteMenu.AndromediaId));
            }
        }

        private static void LogEndOfTask(SiteMenu siteMenu) 
        {
            var events = DependencyResolver.Current.GetServices<IAccessDatabaseEvent>();

            foreach (var ev in events)
            {
                ev.FinishedCheckingForDatabase(new DatabaseUpdatedEventContext(siteMenu.AndromediaId));
            }
        }

        private static void LogCompletion(int completeCount, int failedCount) 
        {
            var ftpEvents = DependencyResolver.Current.GetServices<IFtpEvents>();

            foreach (var ev in ftpEvents)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(0), string.Format("Download ftp Job completed; {0} completed; {1} failed", completeCount, failedCount));
            }
        }

        private static void LogFailureOfTask(SiteMenu siteMenu, Exception ex)
        {
            var events = DependencyResolver.Current.GetServices<IAccessDatabaseEvent>();
            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();

            logger.Error(ex);

            foreach (var ev in events)
            {
                ev.FailedDownloadingDatabase(new DatabaseUpdatedEventContext(siteMenu.AndromediaId));
            }
        }

        #endregion

        
    }
}
