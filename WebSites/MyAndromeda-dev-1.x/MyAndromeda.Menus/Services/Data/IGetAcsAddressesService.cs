using System.Collections.Generic;
using System.Monads;
using System.Threading.Tasks;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Logging;
using System;
using MyAndromeda.Data.AcsServices.Services;
using Newtonsoft.Json.Linq;

namespace MyAndromeda.Menus.Services.Data
{
    public interface IGetAcsAddressesService : IDependency 
    {
        /// <summary>
        /// Gets the menu endpoints async.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetMenuEndpointsAsync(Store store);

        /// <summary>
        /// Gets the loyalty endpoints async.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="action">The action.</param>
        /// <param name="externalOrderRef">The external order ref.</param>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetLoyaltyEndpointsAsync(string userName, string action, string externalOrderRef, string applicationId);
    }
}