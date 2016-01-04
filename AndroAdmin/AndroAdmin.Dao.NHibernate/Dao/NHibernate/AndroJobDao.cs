using System;
using System.Net.NetworkInformation;
using AndroAdmin.Dao.NHibernate.Dao.Factory;
using NHibernate;
//using Quartz;
using Spring.Data.NHibernate.Support;

namespace AndroAdmin.Dao.NHibernate
{
    public class AndroJobDao : HibernateDaoSupport, IAndroJob
    {
        public AndroAdminHibernateDAOFactory AndroAdminHibernateDaoFactory { get; set; }

        #region IJob Members

        
        /*
        public void Execute(JobExecutionContext context)
        {

            var blah = new Ping();
            var reply = blah.Send("www.circuit9.co.uk");

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine(reply.Address + " success");
            }
            else
            {
                Console.WriteLine(reply.Address + " failed");
            }


            /*
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            var sched = schedFact.GetScheduler();
            sched.Start();

            // construct job info
            var jobDetail = new JobDetail("myPingJob", null, typeof(AndroJob));
            // fire every second
            var trigger = TriggerUtils.MakeMinutelyTrigger(19);
            // start on the next even hour
            trigger.StartTimeUtc = TriggerUtils.GetEvenSecondDate(DateTime.UtcNow);
            trigger.Name = "myTrigger";
            sched.ScheduleJob(jobDetail, trigger);
        }*/

        #endregion
    }
}
