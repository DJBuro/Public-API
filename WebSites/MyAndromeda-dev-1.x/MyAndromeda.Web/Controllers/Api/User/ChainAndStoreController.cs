using MyAndromeda.Framework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Logging;

namespace MyAndromeda.Web.Controllers.Api.User
{
    public class ChainAndStoreController : ApiController
    {
        readonly ICurrentUser currentUser;
        readonly AndroAdminDbContext androAdminDataContext;
        readonly MyAndromedaDbContext myAndromedaDataContext;
        readonly IMyAndromedaLogger logger;
        readonly IAuthorizer authorizer;

        public ChainAndStoreController(
            IAuthorizer authorizer,
            ICurrentUser currentUser,
            AndroAdminDbContext androAdminDataContext,
            MyAndromedaDbContext myAndromedaDataContext)
        {
            this.authorizer = authorizer;
            this.logger = logger;
            this.myAndromedaDataContext = myAndromedaDataContext;
            this.androAdminDataContext = androAdminDataContext;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Route("user/chains")]
        public async Task<IEnumerable<ChainModel>> List()
        {
            int[] usersChains = this.currentUser.FlattenedChains.Select(e => e.Id).ToArray();

            IOrderedQueryable<Chain> chainsQuery = this.androAdminDataContext.Chains
                .Where(e => usersChains.Contains(e.Id))
                .OrderBy(e => e.Name);

            var projectionQuery = chainsQuery
                .Select(e => new
                {
                    Id = e.Id,
                    e.Name,
                    StoreCount = e.Stores.Count,
                    ParentId = e.Parents
                                        //.Where(parent => parent.Id != e.Id)
                                        .Select(chainChain => chainChain.ParentChainId)
                                        .FirstOrDefault()
                });

            var projectionResult = await projectionQuery.ToArrayAsync();

            List<ChainModel> result = projectionResult.Select(e => new ChainModel
            {
                Id = e.Id,
                Name = e.Name,
                StoreCount = e.StoreCount,
                ChildChainCount = projectionResult.Count(chain => chain.ParentId == e.Id),
                ChildStoreCount = CountRecursivelyChildStores(
                        projectionResult,
                        e,
                        d => d.Id,
                        id => projectionResult.Where(d => d.ParentId == id),
                        d => d.StoreCount),

                ParentId = e.ParentId == 0
                            ? new int?()
                            : e.ParentId
            }).ToList();

            result.ForEach((item) =>
            {
                if (item.ParentId.HasValue)
                {
                    bool parentExists = result.Any(e => e.Id == item.ParentId.GetValueOrDefault());

                    if (!parentExists)
                    {
                        item.ParentId = null;
                    }
                }
            });

            return result;

        }

        private int CountRecursivelyChildStores<A>(
            IEnumerable<A> allModels,
            A model,
            Func<A, int> idFunc,                    //current node in the chain 
            Func<int, IEnumerable<A>> childrenFunc, //results from all models
            Func<A, int> sum)
        {
            int total = 0;
            IEnumerable<A> children = childrenFunc(idFunc(model));
            foreach (var item in children)
            {
                total += sum(item);
                total += CountRecursivelyChildStores(allModels, item, idFunc, childrenFunc, sum);
            }
            return total;
        }

        [HttpGet]
        [Route("user/chains/{chainId}")]
        public async Task<IEnumerable<StoreModel>> ListStores([FromUri]int chainId)
        {
            MyAndromeda.Data.Domain.Chain[] usersChains = this.currentUser.FlattenedChains.ToArray();

            if (!usersChains.Any(e => e.Id == chainId))
            {
                throw new Exception(message: "Chain is inaccessible");
            }

            IOrderedQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> storesQuery = this.androAdminDataContext.Stores
                .Where(e => e.ChainId == chainId)
                .OrderBy(e => e.Name);

            IQueryable<StoreModel> projection = storesQuery.CreateStoreModelProjection();
            //    storesQuery.Select(e => new StoreModel() { 
            //    Id = e.Id,
            //    AndromedaSiteId = e.AndromedaSiteId,
            //    Name = e.Name,
            //    ChainId = e.ChainId,
            //    ChainName = e.Chain.Name,
            //    ExternalSiteId = e.ExternalId,
            //    HasRameses = e.StoreDevices.Count == 0 || e.StoreDevices.Any(storeDevice => storeDevice.Device.Name == "Rameses")
            //});

            List<StoreModel> results = await projection.ToListAsync();

            int[] storeIds = results.Select(e => e.Id).ToArray();
            List<StoreEnrollmentQueryModel> enrollments = await this.myAndromedaDataContext.StoreEnrolments
                .QueryByStoreIds(storeIds)
                .CreateStoreEnrollmentProjection()
                .ToListAsync();

            results.FillInStoreWithEnrollments(enrollments);

            return results;
        }

        [HttpGet]
        [Route("user/stores/findById/{andromedaSiteId}")]
        public async Task<IEnumerable<StoreModel>> FindStoresByAndromedaSiteId([FromUri] int andromedaSiteId)
        {
            MyAndromeda.Data.Domain.Chain[] usersChains = this.currentUser.FlattenedChains.ToArray();
            int[] chainIds = usersChains.Select(e => e.Id).ToArray();

            IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> storesQuery =
                this.androAdminDataContext.Stores
                    .Where(e => e.AndromedaSiteId == andromedaSiteId)
                    .QueryStoreWithChainIds(chainIds);

            IQueryable<StoreModel> projection = storesQuery.CreateStoreModelProjection();
            
            List<StoreModel> results = await projection.ToListAsync();

            int[] storeIds = results.Select(e => e.Id).ToArray();

            List<StoreEnrollmentQueryModel> enrollments = await this.myAndromedaDataContext.StoreEnrolments
                .QueryByStoreIds(storeIds)
                .CreateStoreEnrollmentProjection()
                .ToListAsync();

            results.FillInStoreWithEnrollments(enrollments);

            return results;
        }

        [HttpGet]
        [Route("user/stores/find/{name}")]
        public async Task<IEnumerable<StoreModel>> FindStores([FromUri]string name)
        {
            MyAndromeda.Data.Domain.Chain[] usersChains = this.currentUser.FlattenedChains.ToArray();
            int[] chainIds = usersChains.Select(e => e.Id).ToArray();

            IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> storesQuery =
                this.androAdminDataContext.Stores
                .QueryStoreWithChainIds(chainIds)
                .QueryByStoreName(name);

            IQueryable<StoreModel> projection = storesQuery.CreateStoreModelProjection();

            List<StoreModel> results = await projection.ToListAsync();

            int[] storeIds = results.Select(e => e.Id).ToArray();

            List<StoreEnrollmentQueryModel> enrollments = await this.myAndromedaDataContext.StoreEnrolments
                .QueryByStoreIds(storeIds)
                .CreateStoreEnrollmentProjection()
                .ToListAsync();

            results.FillInStoreWithEnrollments(enrollments);

            return results;
        }

    }

    public static class Extensions
    {
        public static IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> QueryStoreWithChainId(this IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> query, int chainId)
        {
            return query
                .Where(e => e.ChainId == chainId)
                .OrderBy(e => e.Name);
        }



        public static IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> QueryStoreWithChainIds(this IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> query, int[] chainIds)
        {
            return query
                .Where(e => chainIds.Contains(e.ChainId))
                .OrderBy(e => e.Name);
        }

        public static IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> QueryByStoreName(this IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> query, string name)
        {
            return query.Where(e => e.Name.Contains(name) || e.ClientSiteName.Contains(name) || e.ExternalSiteName.Contains(name));
        }

        public static IQueryable<StoreModel> CreateStoreModelProjection(this IQueryable<MyAndromeda.Data.Model.AndroAdmin.Store> query)
        {
            return query.Select(e => new StoreModel()
            {
                Id = e.Id,
                AndromedaSiteId = e.AndromedaSiteId,
                Name = e.Name,
                ExternalSiteId = e.ExternalId,
                ChainId = e.ChainId,
                ChainName = e.Chain.Name,
                HasRameses = e.StoreDevices.Count == 0 || e.StoreDevices.Any(storeDevice => storeDevice.Device.Name == "Rameses")
            });
        }

        public static IQueryable<StoreEnrolment> QueryByStoreIds(this IQueryable<StoreEnrolment> query, int[] storeIds)
        {
            return query
                .Where(e => e.Active && storeIds.Any(k => k == e.StoreId));
        }
        public static IQueryable<StoreEnrollmentQueryModel> CreateStoreEnrollmentProjection(this IQueryable<StoreEnrolment> query)
        {
            return query
                //.Where(e=> e.Active)
                .Select(e => new StoreEnrollmentQueryModel()
                {
                    Name = e.EnrolmentLevel.Name,
                    StoreId = e.StoreId
                });
        }

        public static void FillInStoreWithEnrollments(this IEnumerable<StoreModel> stores, List<StoreEnrollmentQueryModel> enrollements)
        {

            foreach (var store in stores)
            {
                List<StoreEnrollmentQueryModel> storeEnrollments = enrollements.Where(e => e.StoreId == store.Id).ToList();

                store.StoreEnrollments = storeEnrollments.Select(e => new StoreEnrollment()
                {
                    Name = e.Name
                }).ToList();

            }

        }

    }

}