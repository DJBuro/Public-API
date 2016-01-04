using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Tokens;
using MyAndromedaDataAccess;
using MyAndromedaDataAccess.Domain.Marketing;
using MyAndromeda.Logging;
using MyAndromedaDataAccess.DataAccess.MyAndromeda.Email;

namespace MyAndromeda.Web.Areas.Marketing.Services
{
    public class MarketEmailService : IMarketEmailService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly ITokenService tokenService;

        private readonly WorkContextWrapper workContextWrapper;

        private readonly IEmailCampaignDataAccess emailCampsignDataAccess;

        public MarketEmailService(WorkContextWrapper workContextWrapper, IEmailCampaignDataAccess emailCampsignDataAccess, IMyAndromedaLogger logger, ITokenService tokenService) 
        {
            this.workContextWrapper = workContextWrapper;
            this.emailCampsignDataAccess = emailCampsignDataAccess;
            this.tokenService = tokenService;
            this.logger = logger;
        }
        
        /// <summary>
        /// Gets the email campaign tasks.
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<EmailCampaignTask> GetEmailCampaignTasks()
        //{
        //    IEnumerable<EmailCampaignTask> tasksToRun = dataAccessFactory.EmailCampaignTasksDataAccess.GetTasksToRun(DateTime.Now, );

        //    return tasksToRun;
        //}

        /// <summary>
        /// Gets the replaceable fields for the editor.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Token> GetReplaceableFields()
        {
            var tokens = this.tokenService.GetAllTokens();

            return tokens;
        }

        /// <summary>
        /// Destroys the specified data model.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        public void Destroy(EmailCampaign dataModel)
        {
            this.emailCampsignDataAccess.Destroy(dataModel.Id);
        }

        /// <summary>
        /// Gets the mail marketing campaigns.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmailCampaign> GetMailMarketingCampaignsForSite()
        {
            if (workContextWrapper.Current.CurrentSite.Site == null) 
            {
                throw new ArgumentNullException("Site");
            }

            var emailCampaigns = this.emailCampsignDataAccess
                
                .ListByChainAndSite(this.workContextWrapper.Current.CurrentChain.Chain.Id, this.workContextWrapper.Current.CurrentSite.Site.Id);

            return emailCampaigns;
        }

        /// <summary>
        /// Gets the mail marketing campaigns for chain.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmailCampaign> GetMailMarketingCampaignsForChain()
        {
            if (!workContextWrapper.Current.CurrentChain.Available) 
            {
                throw new ArgumentException("Chain Required");
            }

            var emailCampaigns = this.emailCampsignDataAccess.ListByChain(this.workContextWrapper.Current.CurrentChain.Chain.Id);

            return emailCampaigns;
        }

        /// <summary>
        /// Gets the mail marketing campaigns for chain.
        /// </summary>
        /// <param name="showOnlyChainCampaigns">The show only chain campaigns.</param>
        /// <returns></returns>
        public IEnumerable<EmailCampaign> GetMailMarketingCampaignsForChain(bool showOnlyChainCampaigns = false)
        {
            IEnumerable<EmailCampaign> returnValues;
            var allEmailCampaigns = this.emailCampsignDataAccess.ListByChain(this.workContextWrapper.Current.CurrentChain.Chain.Id);

            if (showOnlyChainCampaigns)
            {
                returnValues = allEmailCampaigns.Where(e => e.ChainOnly == true);
            }
            else
            {
                returnValues = allEmailCampaigns;
            }

            return returnValues;
        }

        /// <summary>
        /// Saves the specified data model.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        public void Save(EmailCampaign dataModel)
        {
            if (dataModel.ChainId == 0)
                throw new ArgumentException("ChainId is required");
            if (string.IsNullOrWhiteSpace(dataModel.Reference))
                throw new ArgumentException("Reference is required");
            if (string.IsNullOrWhiteSpace(dataModel.Subject))
                throw new ArgumentException("Subject is required");

            this.emailCampsignDataAccess.Save(dataModel);
        }

        /// <summary>
        /// Gets the email campaign.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public EmailCampaign GetEmailCampaign(int id)
        {
            return this.emailCampsignDataAccess.Get(id);
        }

    }
}