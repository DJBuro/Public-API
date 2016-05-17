using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.User;
using Domain = MyAndromedaDataAccess.Domain;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Data.DataAccess.Users
{
    public interface IUserChainsDataService : IDependency
    {
        /// <summary>
        /// Gets the chains for user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<ChainDomainModel> GetChainsForUser(int userId);

        /// <summary>
        /// Gets the chains for user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<ChainDomainModel> GetChainsForUser(int userId, Expression<Func<Chain, bool>> query);

        /// <summary>
        /// Adds the chain to user.
        /// </summary>
        /// <param name="chain">The chain.</param>
        /// <param name="userId">The user id.</param>
        void AddChainLinkToUser(ChainDomainModel chain, int userId);

        /// <summary>
        /// Finds the users belonging to chain.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        IEnumerable<MyAndromedaUser> FindUsersDirectlyBelongingToChain(int chainId);

        /// <summary>
        /// Finds the chains directly belonging to user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<ChainDomainModel> FindChainsDirectlyBelongingToUser(int userId);

        /// <summary>
        /// Removes the chain link to user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="chainId">The chain id.</param>
        void RemoveChainLinkToUser(int userId, int chainId);

        /// <summary>
        /// Gets AndroWebOrderingWebsites for user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        IEnumerable<AndroWebOrderingWebsite> GetAndroWebOrderingSitesForUser(int userId);
    }
}
