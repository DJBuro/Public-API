using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromedaDataAccess.DataAccess;

namespace MyAndromeda.Data.DataAccess.MyAndromeda.Email
{
    public interface IEmailCampaignDataAccess : IDependency, IDataAccessOptions
    { 
        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        Data.Domain.Marketing.EmailCampaign Get(int id);

        /// <summary>
        /// Saves the specified campaign.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        void Save(Data.Domain.Marketing.EmailCampaign campaign);

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Data.Domain.Marketing.EmailCampaign> List();

        /// <summary>
        /// Lists the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<Data.Domain.Marketing.EmailCampaign> List(Expression<Func<Data.Domain.Marketing.EmailCampaign, bool>> query);

        IEnumerable<Data.Domain.Marketing.EmailCampaign> ListByChain(int chainId);

        IEnumerable<Data.Domain.Marketing.EmailCampaign> ListByChainAndSite(int chainId, int siteId);

        IEnumerable<Data.Domain.Marketing.EmailCampaign> ListBySite(int siteId);

        void Destroy(int id);

        /// <summary>
        /// Gets the email settings.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        Data.Domain.Marketing.EmailSettings GetEmailSettings(int id);

        Data.Domain.Marketing.EmailSettings GetEmailSettingsBySiteId(int siteId);

        Data.Domain.Marketing.EmailSettings GetEmailSettingsByChainId(int chainId);

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void SaveSettings(Data.Domain.Marketing.EmailSettings settings);

        /// <summary>
        /// Destroys the email settings.
        /// </summary>
        /// <param name="id">The id.</param>
        void DestroyEmailSettings(int id);

        /// <summary>
        /// Creates the email settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void CreateEmailSettings(Data.Domain.Marketing.EmailSettings settings);
    }
}