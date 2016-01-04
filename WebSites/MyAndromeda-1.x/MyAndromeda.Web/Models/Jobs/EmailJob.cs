using MyAndromeda.Framework.Logging;
using MyAndromeda.Web.Areas.Marketing.Services;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebBackgrounder;

namespace MyAndromeda.Web.Models.Jobs
{
    public class EmailJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailJob" /> class.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="timeout">The timeout.</param>
        public EmailJob(TimeSpan interval, TimeSpan timeout)
            : base("Email Job", interval, timeout)
        {
        }
        
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
        public override Task Execute()
        {
            return new Task(Run);
        }

        public void Run() 
        {
            var emailMarketingService = DependencyResolver.Current.GetService<IMarketEmailTaskService>();
            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();

            var campaignsToSend = emailMarketingService.GetEmailCampaignTasks().ToArray();

            if (campaignsToSend.Length == 0)
                return;

            logger.Debug("{0} email campaign campaigns in the queue to process", campaignsToSend);
            //only take one as a campaign may take a while therefore better for the next ticker to take up the job.
            var runCampaignTasks = campaignsToSend.Take(1).ToArray();
            
            //tell the database that they have been started so its not grabbed by the next thread 
            emailMarketingService.MarkAsStarted(runCampaignTasks);

            foreach (var campaignTask in runCampaignTasks)
            {
                logger.Debug("Start sending emails for {0} campaign", campaignTask.EmailCampaign.Reference);
             
                emailMarketingService.SendCampaignEmail(campaignTask.EmailCampaign);
                campaignTask.Completed = true;
                campaignTask.CompletedAt = DateTime.UtcNow;

                emailMarketingService.Update(campaignTask);

                logger.Debug("Completed sending emails for {0} campaign", campaignTask.EmailCampaign.Reference);
            }
        }

    }
}