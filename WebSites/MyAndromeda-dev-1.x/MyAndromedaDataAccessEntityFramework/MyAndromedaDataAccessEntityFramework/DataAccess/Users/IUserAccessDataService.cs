using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Users
{
    public interface IUserAccessDataService : IDependency
    {
        /// <summary>
        /// Determines whether the user is associated with store.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        bool IsTheUserAssociatedWithStore(int userId, int storeId);

        /// <summary>
        /// Determines whether [is the user associated with chain] [the specified user id].
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        bool IsTheUserAssociatedWithTheChain(int userId, int chainId);

        /// <summary>
        /// Determines whether the user is associated by chain.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="chainId">The chain id.</param>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        bool IsTheUserAssociatedByChainAndStore(int userId, int chainId, int storeId);

        /// <summary>
        /// Lists the chains user can access.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IList<Chain> ListChainsUserCanAccess(int userId);

        /// <summary>
        /// Lists the stores user can access.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IList<Site> ListStoresUserCanAccess(int userId);
    }
}

