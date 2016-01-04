using MyAndromeda.Core.Authorization;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Models;
using MyAndromeda.Menus.Services.Menu;
using MyAndromeda.WebApiClient.Syncing;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebBackgrounder;
using System.Reactive.Linq;

namespace MyAndromeda.Menus.Jobs
{
    public class PublishMenuJob :Job
    {
        public const string JobName = "Menu Publishing Job";

        private const string JobStartedMessage = "Publish menu ftp 'Job' started.";
        private const string JobEndedMessage = "Publish menu ftp 'Job' ended. {0} Completed | {1} Failed.";
        private const string JobTasksStartedMessage = "Publish menu ftp 'job' has {0} tasks starting.";
        private const string TaskStartedMessage = "Publish menu 'task' started. {0}.";
        private const string TaskEndedMessage = "Publish menu 'task' ended {0}.";

        public PublishMenuJob(TimeSpan interval, TimeSpan timeout)
            : base(JobName, interval, timeout)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("new {0}", JobName));
        }

        public override Task Execute()
        {
            return new Task(this.Run);
        }

        public static TimeSpan Interval
        {
            get { return TimeSpan.FromSeconds(15); }
        }

        //public static TimeSpan Timeout
        //{
        //    get { return TimeSpan.FromMinutes(2); }
        //}

        public void Run()
        {
            LogStartOfJob();

            try
            {
                var publishMenuService = DependencyResolver.Current.GetService<IPublishingMenuService>();

                var sitesToFetch = publishMenuService.GetMenusToPublish(DateTime.UtcNow).Take(10)
                    .Select(e => new MenuJobModel() { Menu = e })
                    .ToArray();

                var publish = sitesToFetch.ToObservable();

                publish
                    .Subscribe(
                    onNext: (job) =>
                    {
                        LogStartOfTask(job.Menu);
                        var task = SendSyncCall(job.Menu);
                        task.ConfigureAwait(false);
                        task.Wait();

                        job.RanToCompetion = task.IsCompleted;
                        job.Success = task.Result;
                    },
                    onCompleted: () =>
                    {
                        if (sitesToFetch.Length == 0)
                        {
                            return;
                        }

                        var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
                        logger.Debug("Completed updating menus");

                        var successful = sitesToFetch.Where(e => e.Success).Count();
                        var unSuccessful = sitesToFetch.Where(e => !e.Success).Count();
                        var notFinish = sitesToFetch.Where(e => !e.RanToCompetion).Count();

                        LogTasksCompleted(successful, unSuccessful + notFinish);

                        logger.Debug("Success: {0}; Unsuccessful: {1};", successful, unSuccessful);

                    },
                    onError: (e) =>
                    {
                        var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
                        logger.Error("Failed iterating publish menu job");
                        logger.Error(e);
                    }
                );

                LogEndOfJob();
            }
            catch (System.Exception e)
            {
                var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
                
                logger.Error(e);
                
                throw;
            }
        }

        private static void LogStartOfTasks(SiteMenu[] sitemenus)
        {
            var publishMenuEvents = DependencyResolver.Current.GetServices<IPublishMenuFtpEvent>();
            var message = string.Format(JobTasksStartedMessage, sitemenus.Length);

            foreach (var ev in publishMenuEvents)
            {
                ev.OnTasksStarting(message, ExpectedRoles.Administrator);
            }
        }

        private static void LogStartOfTask(SiteMenu siteMenu) 
        {
            var publishMenuEvents = DependencyResolver.Current.GetServices<IPublishMenuFtpEvent>();
            var message = string.Format(TaskStartedMessage, siteMenu.AndromediaId);
            foreach (var ev in publishMenuEvents)
            {
                ev.OnStartedTask(message, ExpectedRoles.Administrator);
            }
        }

        private static void LogTasksCompleted(int completed, int failed) 
        {
            var publishMenuEvents = DependencyResolver.Current.GetServices<IPublishMenuFtpEvent>();
            var message = string.Format(JobEndedMessage, completed, failed);

            foreach (var ev in publishMenuEvents) 
            {
                ev.OnEndedTask(message, ExpectedRoles.Administrator);
            }
        }

        private static void LogStartOfJob()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Starting: {0}", JobName));

            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            var publishMenuEvents = DependencyResolver.Current.GetServices<IPublishMenuFtpEvent>();
            
            logger.Info(JobStartedMessage);

            foreach (var ev in publishMenuEvents)
            {
                ev.OnStartedTask(JobStartedMessage, ExpectedRoles.Administrator);
            }
        }

        private static void LogEndOfJob() 
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Ending: {0}", JobName));

            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            var publishMenuEvents = DependencyResolver.Current.GetServices<IPublishMenuFtpEvent>();
            var endedMessage = "Publish menu ftp Job ended";

            logger.Info(endedMessage);

            foreach (var ev in publishMenuEvents)
            {
                ev.OnEndedTask(endedMessage, ExpectedRoles.Administrator);
            }
        }

        static async Task<bool> SendSyncCall(SiteMenu sitemenu) 
        {
            LogStartOfTask(sitemenu);

            var controller = DependencyResolver.Current.GetService<ISyncWebCallerController>();

            var result = await controller.RequestMenuSyncAsync(sitemenu.AndromediaId);
            
            return result;
        }
    }
}
