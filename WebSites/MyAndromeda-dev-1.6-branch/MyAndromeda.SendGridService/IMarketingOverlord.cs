using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.SendGridService.MarketingApi.Models.Template;
using MyAndromeda.SendGridService.Models;
using MyAndromeda.Core;

namespace MyAndromeda.SendGridService
{
    public interface IMarketingOverlord : IDependency
    {
        /// <summary>
        /// Gets the marketing info.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <returns></returns>
        Task<MarketingTemplateModel> GetMarketingInfoAsync(MarketingEventCampaign campaign, string templateName);

        /// <summary>
        /// Adds the people to marketing email.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="campaign">The campaign.</param>
        /// <param name="people">The people.</param>
        /// <returns></returns>
        Task<bool> AddPeopleToRecipientListAsync<TModel>(MarketingEventCampaign campaign, List<TModel> people)
            where TModel : MarketingApi.Models.Recipients.Person;

        /// <summary>
        /// Adds the recipient lists to template.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        Task<bool> AddRecipientListsToTemplateAsync(MarketingEventCampaign campaign, string templateName);

        /// <summary>
        /// Removes the people from marketing email.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <param name="emailAddresses">The email addressed.</param>
        /// <returns></returns>
        Task<bool> RemovePeopleFromMarketingEmailAsync(MarketingEventCampaign campaign, List<string> emailAddresses);

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <param name="categories">The categories.</param>
        /// <returns></returns>
        Task<bool> AddCategoryAsync(string campaignTemplateName, IEnumerable<string> categories);

        /// <summary>
        /// Sends the campaign.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <returns></returns>
        Task<bool> SendCampaignAsync(MarketingEventCampaign campaign, string emailTemplateName);

        /// <summary>
        /// Updates the send grid template.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<GetResponseTemplateModel> UpdateSendGridTemplate(MarketingEventCampaign entity, string html);
    }
}
