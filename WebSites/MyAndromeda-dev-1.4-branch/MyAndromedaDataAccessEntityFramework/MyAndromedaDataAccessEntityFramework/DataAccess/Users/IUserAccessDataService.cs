using System.Collections.Generic;
using MyAndromeda.Core;
using System;
using System.Linq;
using MyAndromedaDataAccess.Domain;
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

    public class UserAccessDataService : IUserAccessDataService
    {

        private readonly IUserSitesDataService userSiteDataService;
        private readonly IUserChainsDataService userChainsDataServoce;
        private readonly IStoreDataService storeDataService;
        
        public UserAccessDataService(IUserSitesDataService userSiteDataService,
            IUserChainsDataService userChainsDataServoce,
            IStoreDataService storeDataService)
        {
            this.storeDataService = storeDataService;
            this.userChainsDataServoce = userChainsDataServoce;
            this.userSiteDataService = userSiteDataService;
        }

        public IList<Chain> ListChainsUserCanAccess(int userId)
        {
            return userChainsDataServoce.GetChainsForUser(userId).ToList();
            //using (var dbContext = new Model.AndroAdmin.AndroAdminDbContext()) 
            //{
            //    var table = dbContext.Chains;
            //    var query = table.Where(e=> e.MyAndromedaUsers.Any(user => user.Id == userId));
            //    var result = query.ToList();

            //    return result;
            //}
        }

        public IList<Site> ListStoresUserCanAccess(int userId)
        {
            return this.userSiteDataService.GetSitesDirectlyLinkedToTheUser(userId).ToList();
            //using (var dbContext = new Model.AndroAdmin.AndroAdminDbContext())
            //{
            //    var table = dbContext.Stores;
            //    var query = table.Where(e => e.MyAndromedaUsers.Any(user => user.Id == userId));
            //    var result = query.ToList();

            //    return result;
            //}
        }

        public bool IsTheUserAssociatedWithStore(int userId, int storeId)
        {
            int chainId;

            var store = this.storeDataService.Get(e => e.Id == storeId);

            chainId = store.ChainId;
            
            //lets go digging 
            return this.IsTheUserAssociatedByChainAndStore(userId, chainId, storeId);
        }

        private IEnumerable<Chain> FlatternChains(IEnumerable<Chain> accessibleChains)
        {
            Func<IEnumerable<Chain>, IEnumerable<Chain>> flatern = null;

            flatern = (nodes) =>
            {
                return nodes.SelectMany(e => flatern(e.Items)).Union(nodes);
            };

            var all = accessibleChains.Union(accessibleChains.SelectMany(e => flatern(e.Items)));

            return all;
        }

        public bool IsTheUserAssociatedWithTheChain(int userId, int chainId)
        {
            //get the entire structure that the user is allowed within 
            var chains = this.userChainsDataServoce.GetChainsForUser(userId);

            var flatListOfAllChains = this.FlatternChains(chains);

            var resultOfAny = flatListOfAllChains.Any(e => e.Id == chainId);

            return resultOfAny;
        }

        public bool IsTheUserAssociatedByChainAndStore(int userId, int chainId, int storeId)
        {
            var allStoresLinked = this.userSiteDataService.GetSitesDirectlyLinkedToTheUser(userId);

            bool hardFactBelongsToStore = allStoresLinked.Any(e=> e.Id == storeId);
            bool hardFactBelongsToChain = this.IsTheUserAssociatedWithTheChain(userId, chainId);

            return hardFactBelongsToChain || hardFactBelongsToStore;
        }
    }
}

