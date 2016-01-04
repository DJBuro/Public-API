using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Core;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Tokens;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Web.Areas.Marketing.Services
{
    public interface IMarketEmailService : IDependency
    {
        void Destroy(EmailCampaign dataModel);

        /// <summary>
        /// Gets the email campaign.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        EmailCampaign GetEmailCampaign(int id);
        
        /// <summary>
        /// Saves the specified data model.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        void Save(EmailCampaign dataModel);

        /// <summary>
        /// Gets the replaceable fields for the editor.
        /// </summary>
        /// <returns></returns>
        //IEnumerable<string> GetReplaceableFields();

        IEnumerable<Token> GetReplaceableFields();

        /// <summary>
        /// Gets the mail marketing campaigns.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EmailCampaign> GetMailMarketingCampaignsForSite();

        /// <summary>
        /// Gets the mail marketing campaigns for chain.
        /// </summary>
        /// <param name="showOnlyChainCampaigns">The show only chain campaigns.</param>
        /// <returns></returns>
        IEnumerable<EmailCampaign> GetMailMarketingCampaignsForChain(bool showOnlyChainCampaigns = false);

    }
}