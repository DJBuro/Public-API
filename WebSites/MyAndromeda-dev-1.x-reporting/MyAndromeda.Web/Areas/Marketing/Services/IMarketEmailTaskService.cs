using System;
using System.Collections.Generic;
using MyAndromeda.Core;
using MyAndromeda.Data.DataAccess.MyAndromeda.Email;
using MyAndromeda.Data.Domain.Marketing;
using MyAndromeda.SendGridService.EmailServices;

namespace MyAndromeda.Web.Areas.Marketing.Services
{
    public interface IMarketEmailTaskService : IDependency 
    {
        /// <summary>
        /// Gets the email campaign tasks.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EmailCampaignTask> GetEmailCampaignTasks();

        /// <summary>
        /// Creates the email campaign task.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="runOn">When to attempt to run the task.</param>
        void CreateEmailCampaignTask(int id, DateTime? runOn = null);

        /// <summary>
        /// Sends the campaign email.
        /// </summary>
        /// <param name="emailCampaign">The email campaign.</param>
        void SendCampaignEmail(EmailCampaign emailCampaign);

        /// <summary>
        /// Marks as started.
        /// </summary>
        /// <param name="campaignsToSend">The campaigns to send.</param>
        void MarkAsStarted(IEnumerable<EmailCampaignTask> campaignsToSend);

        /// <summary>
        /// Updates the specified campaign.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        void Update(EmailCampaignTask campaign);

        /// <summary>
        /// Gets the email campaign tasks for site.
        /// </summary>
        /// <param name="siteId">The site id.</param>
        /// <returns></returns>
        IEnumerable<EmailCampaignTask> GetEmailCampaignTasksForSite(int siteId);

        /// <summary>
        /// Gets the running campaign tasks for site.
        /// </summary>
        /// <param name="siteId">The site id.</param>
        /// <returns></returns>
        IEnumerable<EmailCampaignTask> GetRunningCampaignTasksForSite(int siteId);
    }

    public class MarketEmailTaskService : IMarketEmailTaskService 
    {
        //private readonly IDataAccessFactory dataAccessFactory;
        private readonly IMarketEmailService marketEmailService;
        private readonly IEmailSettingsService emailSettingsService;
        private readonly IMailSendingService mailSendingService;
        private readonly IEmailCampaignTasksDataAccess emailCampaignTasksDataAccess;

        public MarketEmailTaskService(IMarketEmailService marketEmailService,
            IMailSendingService mailSendingService,
            IEmailSettingsService emailSettingsService,
            IEmailCampaignTasksDataAccess emailCampaignTasksDataAccess)
        {
            this.emailCampaignTasksDataAccess = emailCampaignTasksDataAccess;
            this.emailSettingsService = emailSettingsService;
            this.mailSendingService = mailSendingService;
            this.marketEmailService = marketEmailService;
            //this.dataAccessFactory = dataAccessFactory;
        }

        /// <summary>
        /// Creates the email campaign task.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="runOn">When to run the campaign.</param>
        public void CreateEmailCampaignTask(int id, DateTime? runOn = null)
        {
            var campaign = this.marketEmailService.GetEmailCampaign(id);
            var settings = this.emailSettingsService.GetBestSettings(campaign.ChainId);

            var campaignTask = new EmailCampaignTask
            {
                EmailCampaign = campaign,
                EmailSettings = settings,
                Created = DateTime.UtcNow,
                RunLaterOnUtc = runOn
            };

            this.emailCampaignTasksDataAccess.Create(campaignTask);
        }

        /// <summary>
        /// Gets the email campaign tasks.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmailCampaignTask> GetEmailCampaignTasks()
        {
            var data = this.emailCampaignTasksDataAccess.GetTasksToRun(DateTime.UtcNow, false, false);

            return data;
        }

        /// <summary>
        /// Gets the running campaign tasks for site.
        /// </summary>
        /// <param name="siteId">The site id.</param>
        /// <returns></returns>
        public IEnumerable<EmailCampaignTask> GetRunningCampaignTasksForSite(int siteId)
        {
            var data = this.emailCampaignTasksDataAccess.GetTasksToRun(DateTime.UtcNow, false, false);

            return data;
        }

        /// <summary>
        /// Updates the specified campaign.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        public void Update(EmailCampaignTask campaign)
        {
            this.emailCampaignTasksDataAccess.Update(campaign);
        }

        /// <summary>
        /// Marks as started.
        /// </summary>
        /// <param name="campaignsToSend">The campaigns to send.</param>
        public void MarkAsStarted(IEnumerable<EmailCampaignTask> campaignsToSend)
        {
            foreach (var item in campaignsToSend) 
            {
                item.Started = true;
                item.RanAt = DateTime.UtcNow;
            }
            this.emailCampaignTasksDataAccess.SetAsRunning(campaignsToSend);
        }

        /// <summary>
        /// Sends the campaign email.
        /// </summary>
        /// <param name="emailCampaign">The email campaign.</param>
        public void SendCampaignEmail(EmailCampaign emailCampaign)
        {
            var settings = emailSettingsService.GetBestSettings(emailCampaign.ChainId);
            
            this.mailSendingService.SendCampaignEmail(emailCampaign, settings);
        }

        /// <summary>
        /// Gets the email campaign tasks for site.
        /// </summary>
        /// <param name="siteId">The site id.</param>
        /// <returns></returns>
        public IEnumerable<EmailCampaignTask> GetEmailCampaignTasksForSite(int siteId)
        {
            return this.emailCampaignTasksDataAccess.GetTasksBySiteId(siteId);
        }
    }
}