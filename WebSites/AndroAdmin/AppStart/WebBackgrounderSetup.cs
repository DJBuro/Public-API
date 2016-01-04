using System;
using System.Diagnostics;
using System.Linq;
using AndroAdmin.ThreatBoard.Jobs;
using WebBackgrounder;

namespace AndroAdmin.AppStart
{
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
            var jobs = new IJob[]
            {
                //new EmailJob(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(2)),
                new ThreatboardJob()
                //attempt to keep the application pool from shutting down. 
                //need a graceful way of getting the application url
                //new KeepAliveJob(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(30))
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