using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.SendGridService.MarketingApi.Models.Recipients;

namespace MyAndromeda.SendGridService.MarketingApi
{
    public interface IMarketingEmailRecipientsService : IDependency
    {
        /// <summary>
        /// Adds the async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> AddAsync(AddOrRemoveRequestModel model);

        /// <summary>
        /// Lists the async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<ListRecipientListNames> ListAsync(GetRequestModel model);

        /// <summary>
        /// Removes the list from marketing email async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> RemoveListFromMarketingEmailAsync(AddOrRemoveRequestModel model);
    }
}
