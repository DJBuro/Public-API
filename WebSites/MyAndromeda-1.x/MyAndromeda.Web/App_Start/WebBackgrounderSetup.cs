using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.CloudSynchronization;
using MyAndromeda.Logging;
using WebBackgrounder;
using MyAndromeda.Menus.Jobs;

namespace MyAndromeda.Web.AppStart
{
    [assembly: WebActivator.PostApplicationStartMethod(typeof(MyAndromeda.Web.AppStart.WebBackgrounderSetup), "Start")]
    [assembly: WebActivator.ApplicationShutdownMethod(typeof(MyAndromeda.Web.AppStart.WebBackgrounderSetup), "Shutdown")]
    public static class WebBackgrounderSetup
    {
        static readonly JobManager jobManager = CreateJobWorkersManager();

        public static void Start()
        {
            jobManager.Start();
        }

        public static void Shutdown()
        {   
            //notifying any task would be useful here. 
            //ie abandon what they are doing and run away. 
            jobManager.Dispose();
        }

        private static JobManager CreateJobWorkersManager()
        {
            var downloadTimeout = SyncFtpDownJob.Timeout;
            var uploadTimeout = SyncFtpUpJob.Timeout;

            var jobs = new IJob[]
            {
                //new EmailJob(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(2)),
                new SyncronizationJob(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30)),
                //new SyncFtpDownJob(TimeSpan.FromMinutes(1), downloadTimeout),
                //new SyncFtpUpJob(TimeSpan.FromSeconds(15), uploadTimeout),
                new PublishMenuJob(TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(5))
            };

            var jobHost = new JobHost(); 
            var manager = new JobManager(jobs, jobHost);

            manager.RestartSchedulerOnFailure = true;
            manager.Fail((ex) => LogFail(ex));
            
            return manager;
        }

        private static void LogFail(Exception ex) 
        {
            try
            {
                Trace.WriteLine("Web backgrounder task failed:");
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.Source);

                var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();

                logger.Error("Web backgrounder task failed", ex);
            }
            catch (Exception e) 
            {
                Trace.WriteLine("Something else that is unexpected died:");
                Trace.WriteLine(e.Message);
                Trace.WriteLine(e.Source);
            }
        }
    }
}