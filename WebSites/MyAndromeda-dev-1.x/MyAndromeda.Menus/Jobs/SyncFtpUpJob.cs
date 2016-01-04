using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Services.Menu;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;
using WebBackgrounder;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Menus.Jobs
{
    public class SyncThumbnailUpJob : Job 
    {
        public SyncThumbnailUpJob(TimeSpan interval, TimeSpan timeout)
            : base("Ftp Menu Sync job", interval, timeout)
        {
        }

        public override Task Execute()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }

    public class SyncFtpUpJob : Job
    {
        public const string TaskName = "Ftp Menu Sync job";

        public static TimeSpan Timeout 
        {
            get { return TimeSpan.FromMinutes(4); }
        }

        public SyncFtpUpJob(TimeSpan interval, TimeSpan timeout) : base(SyncFtpUpJob.TaskName, interval, timeout)
        {

        }

        public override Task Execute()
        {
            return new Task(this.Run);
        }


        public void Run()
        {
            Trace.WriteLine("Sync Ftp Upload Job.");
            
            var ftpEvents = DependencyResolver.Current.GetServices<IFtpEvents>();

            LogFtpStart(ftpEvents);

            using (var ftpMenuService = DependencyResolver.Current.GetService<IFtpMenuManagerService>())
            {
                SiteMenu[] sitesToFetch = ftpMenuService.GetQueueToUpload().ToArray();

                LogFtpTaskStatus(sitesToFetch, ftpEvents);

                var taskList = sitesToFetch.Select(siteMenu => CreatePublishMenuTask(siteMenu)).ToArray();

                Task.WaitAll(taskList.ToArray());

                var completed = taskList.Where(e => e.Result).Count();
                var failed = taskList.Where(e => !e.Result).Count();

                LogCompletion(ftpEvents, completed, failed);     
            }

        }

        private static Task<bool> CreatePublishMenuTask(SiteMenu siteMenu)
        {
            var task = Task.Factory.StartNew<bool>(() =>
            {
                bool taskResult = false;
                var ftpEvents = DependencyResolver.Current.GetServices<IFtpEvents>();
                var events = DependencyResolver.Current.GetServices<IAccessDatabaseEvent>();

                LogFtpTaskStart(siteMenu, ftpEvents, events);

                //new service for individual threads 
                using (var individualService = DependencyResolver.Current.GetService<IFtpMenuManagerService>())
                {

                    individualService.UpdatePublishTask(siteMenu, TaskStatus.Running);

                    try
                    {
                        //individualService.PublishTask(siteMenu.AndromediaId, siteMenu, true);
                        var result = individualService.UploadMenu(siteMenu.AndromediaId);
                        //individualService.PublishTask(siteMenu.AndromediaId, siteMenu, false);

                        if (result.HasUpdatedMenu.GetValueOrDefault())
                        {
                            individualService.UpdatePublishTask(siteMenu, TaskStatus.RanToCompletion);
                            taskResult = true;
                        }
                        else 
                        {
                            individualService.UpdatePublishTask(siteMenu, TaskStatus.Faulted);
                            taskResult = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(siteMenu, ftpEvents, ex);

                        individualService.UpdatePublishTask(siteMenu, TaskStatus.Faulted);

                        taskResult = false;
                    }
                    finally
                    {
                        LogCompletionOfTask(siteMenu, ftpEvents, events);
                    }
                }

                return taskResult;
            });

            return task;
        }

        private static void LogFtpStart(IEnumerable<IFtpEvents> ftpEvents)
        {
            foreach (var ev in ftpEvents)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), "Upload ftp Job started");
            }
        }

        private static void LogCompletion(IEnumerable<IFtpEvents> ftpEvents, int completed, int failed)
        {
            foreach (var ev in ftpEvents)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(0), string.Format("Upload ftp Job completed; {0} completed; {1} failed", completed, failed));
            }
        }

        private static void LogFtpTaskStatus(SiteMenu[] sitesToFetch, IEnumerable<IFtpEvents> ftpEvents)
        {
            Trace.WriteLine("Ftp Menus to upload:" + sitesToFetch.Length);

            foreach (var ev in ftpEvents)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), string.Format("Ftp upload bits to process: {0}", sitesToFetch.Length));
            }
        }

        private static void LogFtpTaskStart(SiteMenu siteMenu, IEnumerable<IFtpEvents> ftpEvents, IEnumerable<IAccessDatabaseEvent> events)
        {
            var eventContext = new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator);

            foreach (var ev in ftpEvents)
            {
                ev.TransactionLog(eventContext, string.Format("Task Started: {0}", siteMenu.AndromediaId));
                ev.TransactionLog(eventContext, string.Format("{0}: Begin publishing", siteMenu.AndromediaId));
            }

            //foreach (var ev in events)
            //{
            //    ev.StartedCheckingForDatabase(new DatabaseUpdatedEventContext(siteMenu.AndromediaId));
            //}

        }

        private static void LogException(SiteMenu siteMenu, IEnumerable<IFtpEvents> ftpEvents, Exception ex)
        {
            foreach (var ev in ftpEvents)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), string.Format("{0}: exception", siteMenu.AndromediaId));

                while (ex != null)
                {
                    ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), string.Format("Exception for {0}: {1}", siteMenu.AndromediaId, ex.Message));
                    ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), string.Format("Exception source {0}: {1}", siteMenu.AndromediaId, ex.Source));
                    ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), string.Format("Exception source {0}: {1}", siteMenu.AndromediaId, ex.StackTrace));

                    ex = ex.InnerException;
                }
            }
        }

        private static void LogCompletionOfTask(SiteMenu siteMenu, IEnumerable<IFtpEvents> ftpEvents, IEnumerable<IAccessDatabaseEvent> events)
        {
            foreach (var ev in events)
            {
                ev.FinishedCheckingForDatabase(new DatabaseUpdatedEventContext(siteMenu.AndromediaId));
            }

            foreach (var ev in ftpEvents)
            {
                ev.TransactionLog(new DatabaseUpdatedEventContext(ExpectedUserRoles.Administrator), string.Format("Task Completed: {0}", siteMenu.AndromediaId));
            }
        }
    }
}