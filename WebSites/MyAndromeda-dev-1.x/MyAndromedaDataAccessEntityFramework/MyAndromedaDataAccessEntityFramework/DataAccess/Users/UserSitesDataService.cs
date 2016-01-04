using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Data.Domain;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;

namespace MyAndromeda.Data.DataAccess.Users
{
    public class UserSitesDataService : IUserSitesDataService
    {
        private readonly IUserChainsDataService chainsDataAccessService;

        private readonly AndroAdminDbContext androAdminDbContext;

        private readonly MyAndromedaDbContext myAndromedaDbContext;

        public UserSitesDataService(IUserChainsDataService chainsDataAccessService, AndroAdminDbContext androAdminDbContext, MyAndromedaDbContext myAndromedaDbContext) 
        {
            this.myAndromedaDbContext = myAndromedaDbContext;
            this.androAdminDbContext = androAdminDbContext;
            this.chainsDataAccessService = chainsDataAccessService;
        }

        public IEnumerable<Site> GetSitesDirectlyLinkedToTheUser(int userId, Expression<Func<Store, bool>> query)
        {
            IEnumerable<Site> sites;
            int[] accessibleStores;
            {
                var userStoresTable = this.myAndromedaDbContext.UserStores;
                var userStoresQuery = userStoresTable.Where(e => e.UserRecordId == userId).Select(e => e.StoreId);
                var userStoresResult = userStoresQuery.ToList();

                accessibleStores = userStoresResult.ToArray();
            }

            var storesTable = this.androAdminDbContext.Stores.Include(e => e.Address);
            var storeQuery = storesTable.Where(e => accessibleStores.Contains(e.Id));
            var storeResults = storeQuery.ToArray();

            sites = storeResults.Select(e => e.ToDomainModel()).ToArray();

            return sites;
        }

        public IEnumerable<Site> GetSitesDirectlyLinkedToTheUser(int userId)
        {
            return this.GetSitesDirectlyLinkedToTheUser(userId, e => true);
        }

        public IEnumerable<Site> GetSitesForUserAndChain(int userId, int chainId)
        {
            IEnumerable<Site> sites = Enumerable.Empty<Site>();
            var accessibleChains = this.chainsDataAccessService.GetChainsForUser(userId, e => e.Id == chainId);

            if (!accessibleChains.Any())
            {
                return sites;
            }

            var storesTable = this.androAdminDbContext.Stores.Include(e => e.Address);
            var storesQuery = storesTable.Where(e => e.ChainId == chainId);
            var storesResult = storesQuery.ToArray();

            sites = storesResult.Select(e => e.ToDomainModel()).ToArray();

            return sites;
        }

        public void RemoveStoreLinkToUser(int userId, int storeId)
        {
            var table = this.myAndromedaDbContext.UserStores;
            var query = table.Where(e => e.StoreId == storeId && e.UserRecordId == userId);
            var results = query.ToArray();

            foreach (var result in results)
            {
                this.myAndromedaDbContext.UserStores.Remove(result);
            }

            this.myAndromedaDbContext.SaveChanges();
        }

        public void AddStoreLinkToUser(int storeId, int userId)
        {
            var userStoreTable = this.myAndromedaDbContext.UserStores;
            if (userStoreTable.Any(e => e.StoreId == storeId && e.UserRecordId == userId))
            {
                return;
            }

            var link = userStoreTable.Create();
            link.StoreId = storeId;
            link.UserRecordId = userId;

            userStoreTable.Add(link);
            this.myAndromedaDbContext.SaveChanges();
        }
        //public IEnumerable<Site> GetSitesForUserAndChain(int userId, int chainId, bool deepSearch)
        //{
        //    if (!deepSearch) { return this.GetSitesForUserAndChain(userId, chainId); }
        //    throw new NotImplementedException();
        //}
    }
}