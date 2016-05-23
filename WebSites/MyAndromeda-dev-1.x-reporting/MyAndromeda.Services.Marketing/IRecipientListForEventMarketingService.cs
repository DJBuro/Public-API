using System.Collections.Generic;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.SendGridService.MarketingApi.Models.Recipients;

namespace MyAndromeda.Services.Marketing
{
    public interface IRecipientListForEventMarketingService : IDependency
    {
        /// <summary>
        /// Gets the people for no orders async.
        /// </summary>
        /// <param name="acsApplicationId">The acs application id.</param>
        /// <returns></returns>
        Task<List<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>> GetPeopleForNoOrdersAsync(int acsApplicationId, int andromedaSiteId);

        /// <summary>
        /// Gets the current customers async.
        /// </summary>
        /// <param name="acsApplicationId">The acs application id.</param>
        /// <returns></returns>
        Task<List<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>> GetCurrentCustomersAsync(int acsApplicationId, int andromedaSiteId);

        /// <summary>
        /// Gets the lazy customers async.
        /// </summary>
        /// <param name="acsApplicationId">The acs application id.</param>
        /// <returns></returns>
        Task<List<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>> GetLazyCustomersAsync(int acsApplicationId, int andromedaSiteId);

        /// <summary>
        /// Gets the lapsed customers async.
        /// </summary>
        /// <param name="acsApplicationId">The acs application id.</param>
        /// <returns></returns>
        Task<List<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>> GetLapsedCustomersAsync(int acsApplicationId, int andromedaSiteId);

        /// <summary>
        /// Gets the recipients.
        /// </summary>
        /// <param name="eventCampaign">The event campaign.</param>
        /// <returns></returns>
        Task<List<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>> GetRecipients(MarketingEventCampaign eventCampaign);
    }
}