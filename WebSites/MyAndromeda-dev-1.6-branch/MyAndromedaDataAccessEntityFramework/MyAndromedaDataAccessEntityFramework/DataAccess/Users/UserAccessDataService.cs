using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Data.Domain;
using MyAndromeda.Data.DataAccess.Sites;

namespace MyAndromeda.Data.DataAccess.Users
{
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

        public IList<ChainDomainModel> ListChainsUserCanAccess(int userId)
        {
            return this.userChainsDataServoce.GetChainsForUser(userId).ToList();
            //using (var dbContext = new Model.AndroAdmin.AndroAdminDbContext()) 
            //{
            //    var table = dbContext.Chains;
            //    var query = table.Where(e=> e.MyAndromedaUsers.Any(user => user.Id == userId));
            //    var result = query.ToList();
            //    return result;
            //}
        }

        public IList<SiteDomainModel> ListStoresUserCanAccess(int userId)
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

            Model.AndroAdmin.Store store = this.storeDataService.Get(e => e.Id == storeId);

            chainId = store.ChainId;
            
            //lets go digging 
            return this.IsTheUserAssociatedByChainAndStore(userId, chainId, storeId);
        }

        private IEnumerable<ChainDomainModel> FlatternChains(IEnumerable<ChainDomainModel> accessibleChains)
        {
            Func<IEnumerable<ChainDomainModel>, IEnumerable<ChainDomainModel>> flatern = null;

            flatern = (nodes) =>
            {
                return nodes.SelectMany(e => flatern(e.Items)).Union(nodes);
            };

            IEnumerable<ChainDomainModel> all = accessibleChains.Union(accessibleChains.SelectMany(e => flatern(e.Items)));

            return all;
        }

        public bool IsTheUserAssociatedWithTheChain(int userId, int chainId)
        {
            //get the entire structure that the user is allowed within 
            IEnumerable<ChainDomainModel> chains = this.userChainsDataServoce.GetChainsForUser(userId);

            IEnumerable<ChainDomainModel> flatListOfAllChains = this.FlatternChains(chains);

            bool resultOfAny = flatListOfAllChains.Any(e => e.Id == chainId);

            return resultOfAny;
        }

        public bool IsTheUserAssociatedByChainAndStore(int userId, int chainId, int storeId)
        {
            IEnumerable<SiteDomainModel> allStoresLinked = this.userSiteDataService.GetSitesDirectlyLinkedToTheUser(userId);

            bool hardFactBelongsToStore = allStoresLinked.Any(e => e.Id == storeId);
            bool hardFactBelongsToChain = this.IsTheUserAssociatedWithTheChain(userId, chainId);

            return hardFactBelongsToChain || hardFactBelongsToStore;
        }
    }
}