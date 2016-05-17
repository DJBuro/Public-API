using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core.User;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Logging;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Data.DataAccess.Users
{
    public class UserChainsDataService : IUserChainsDataService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly MyAndromedaDbContext myAndromedaDbContext;

        /// <summary>
        /// From AndroAdmin 
        /// </summary>
        public DbSet<Chain> Chains { get; private set; }

        /// <summary>
        /// From AndroAdmin .. what are you doing here?
        /// </summary>
        public DbSet<AndroWebOrderingWebsite> AndroWebOrderingWebsites { get; private set; }

        /// <summary>
        /// From AndroAdmin 
        /// </summary>
        public DbSet<ChainChain> ChainChains { get; private set; }


        /// <summary>
        /// MyAndromeda Link to chains
        /// </summary>
        public DbSet<UserChain> UserChains { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserChainsDataService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="androAdminDbContext">The andro admin db context.</param>
        /// <param name="myAndromedaDbContext">My andromeda db context.</param>
        public UserChainsDataService(
            IMyAndromedaLogger logger,
            AndroAdminDbContext androAdminDbContext,
            MyAndromedaDbContext myAndromedaDbContext)
        {
            this.logger = logger;
            this.Chains = androAdminDbContext.Chains;
            this.AndroWebOrderingWebsites = androAdminDbContext.AndroWebOrderingWebsites;
            this.ChainChains = androAdminDbContext.ChainChains;

            this.UserChains = myAndromedaDbContext.UserChains;
            

            //this.androAdminDbContext = androAdminDbContext;
            this.myAndromedaDbContext = myAndromedaDbContext;
        }

        /// <summary>
        /// Gets the chains for user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public IEnumerable<ChainDomainModel> GetChainsForUser(int userId)
        {
            IEnumerable<ChainDomainModel> chains = Enumerable.Empty<ChainDomainModel>();

            try
            {
                IEnumerable<int> accessibleChains = Enumerable.Empty<int>();

                //fetch list in myandromeda to compare to androadmin 
                try
                {
                    DbSet<UserChain> userChainsTable = this.UserChains;
                    IQueryable<int> userChainsQuery = userChainsTable
                                                         .Where(e => e.UserRecordId == userId)
                                                         .Select(e => e.ChainId);

                    accessibleChains = userChainsQuery.ToArray();
                }
                catch (Exception e)
                {
                    var context = new MyAndromedaDbContext();

                    this.logger.Error("Chains Loaded from MyAndromeda failed", e);
                    this.logger.Error("Connection string:" + context.Database.Connection.ConnectionString);


                    throw e;
                }

                Chain[] chainTable = this.Chains
                                     .Include(e => e.Children)
                                     .Include(e => e.Parents)
                                     .ToArray();

                IEnumerable<Chain> chainQuery = chainTable.Where(e => accessibleChains.Contains(e.Id));

                //top node on the chain structure 
                Chain[] chainResult = chainQuery.ToArray();

                if (chainResult.Length == 0)
                {
                    return chains;
                }

                chains = this.CreateHierarchyStructure(chainResult).ToArray();
            }
            catch (Exception e)
            {
                this.logger.Error("Chains Loaded from AndroAdmin failed", e);
                //this.logger.Error("Connection string:" + this.androAdminDbContext.Database.Connection.ConnectionString);

                throw;
            }

            return chains;
        }

        public IEnumerable<AndroWebOrderingWebsite> GetAndroWebOrderingSitesForUser(int userId)
        {
            IEnumerable<ChainDomainModel> chainsResult = this.GetChainsForUser(userId);
            IEnumerable<AndroWebOrderingWebsite> androWebOrderingSites = 
                Enumerable.Empty<AndroWebOrderingWebsite>();

            if (chainsResult.ToList().Count > 0)
            {
                List<AndroWebOrderingWebsite> websites = this.AndroWebOrderingWebsites.ToList();
                androWebOrderingSites = (from aws in websites
                                            join cr in chainsResult on (aws.ChainId == null ? 0 : aws.ChainId) equals cr.Id
                                            select aws).ToList();
            }
            
            return androWebOrderingSites;
        }

        /// <summary>
        /// Gets the chains for user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IEnumerable<ChainDomainModel> GetChainsForUser(int userId, Expression<Func<Chain, bool>> query = null)
        {
            if (query == null)
            {
                query = (_) => true;
            }

            IEnumerable<ChainDomainModel> chains = Enumerable.Empty<ChainDomainModel>();
            //using (var androAdminDbContext = new Model.AndroAdmin.AndroAdminDbContext())
            {
                IEnumerable<int> accessibleChains = Enumerable.Empty<int>();
                //using (var myAndromedaDbContext = new Model.MyAndromeda.MyAndromedaDbContext())
                {
                    DbSet<UserChain> userChainsTable = this.UserChains;
                    IQueryable<int> userChainsQuery = userChainsTable
                                                         .Where(e => e.UserRecordId == userId)
                                                         .Select(e => e.ChainId);

                    int[] userChainsResult = userChainsQuery.ToArray();

                    accessibleChains = userChainsResult;
                }

                DbSet<Chain> chainTable = this.Chains;
                IQueryable<Chain> chainQuery = chainTable
                                           .Where(query)
                                           .Where(e => accessibleChains.Any(chainId => chainId == e.Id));

                //top node on the chain structure 
                Chain[] chainResult = chainQuery.ToArray();

                if (chainResult.Length == 0)
                {
                    return chains;
                }

                chains = this.CreateHierarchyStructure(chainResult);
            }

            return chains;
        }

        /// <summary>
        /// Adds the chain to user.
        /// </summary>
        /// <param name="chain">The chain.</param>
        /// <param name="userId">The user id.</param>
        public void AddChainLinkToUser(ChainDomainModel chain, int userId)
        {
            DbSet<UserChain> userChainsTable = this.UserChains;
            if (userChainsTable.Any(e => e.ChainId == chain.Id && e.UserRecordId == userId))
            {
                return;
            }

            UserChain link = userChainsTable.Create();
            link.ChainId = chain.Id;
            link.UserRecordId = userId;

            userChainsTable.Add(link);

            this.myAndromedaDbContext.SaveChanges();
        }

        /// <summary>
        /// Finds the users belonging to chain.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        public IEnumerable<MyAndromedaUser> FindUsersDirectlyBelongingToChain(int chainId)
        {
            IEnumerable<MyAndromedaUser> myAndromedaUserusers;
            
            IEnumerable<UserRecord> userRecords = this.UserChains.Where(e => e.ChainId == chainId).Select(e => e.UserRecord);
            UserRecord[] result = userRecords.ToArray();

            myAndromedaUserusers = result.Select(e => e.ToDomainModel()).ToArray();

            return myAndromedaUserusers;
        }

        public IEnumerable<ChainDomainModel> FindChainsDirectlyBelongingToUser(int userId)
        {
            IEnumerable<ChainDomainModel> chains = Enumerable.Empty<ChainDomainModel>();

            DbSet<UserChain> userChainsTable = this.UserChains;
            int[] userChainsquery = userChainsTable.Where(e => e.UserRecordId == userId).Select(e => e.ChainId).ToArray();

            IQueryable<Chain> chainsquery = this.Chains.Where(chain => userChainsquery.Contains(chain.Id));
            ChainDomainModel[] chainsResult = chainsquery
                .ToArray()
                .Select(e => new ChainDomainModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Culture = e.Culture
                })
                .ToArray();

            chains = chainsResult;

            return chains;
        }

        public void RemoveChainLinkToUser(int userId, int chainId)
        {
            DbSet<UserChain> table = this.UserChains;
            IQueryable<UserChain> query = table.Where(e => e.ChainId == chainId && e.UserRecordId == userId);
            UserChain[] results = query.ToArray();

            foreach (var result in results)
            {
                this.UserChains.Remove(result);
            }

            this.myAndromedaDbContext.SaveChanges();
        }

        private IEnumerable<ChainDomainModel> CreateHierarchyStructure(Chain[] results)
        {
            var chains = new List<ChainDomainModel>(results.Length);

            DbSet<ChainChain> linkTable = this.ChainChains;
            var linkQuery = linkTable
                                     .Where(e => e.ParentChainId > 0)
                                     .Select(e => new
                                     {
                                         e.ParentChainId,
                                         e.ChildChain.Id,
                                         e.ChildChain.Name,
                                         e.ChildChain.Culture
                                     })
                                     .ToLookup(e => e.ParentChainId);

            //create a dictionary 
            var linkResult = linkQuery.ToDictionary(e => e.Key, e => e.ToArray());

            Action<ChainDomainModel> buildTree = null;
            buildTree = (node) =>
            {
                if (!linkResult.ContainsKey(node.Id))
                {
                    return;
                }

                var lookupByParentId = linkResult[node.Id];
                var children = new List<ChainDomainModel>(lookupByParentId.Length);

                node.Items = children;

                foreach (var lookup in lookupByParentId)
                {
                    var chain = new ChainDomainModel()
                    {
                        Id = lookup.Id,
                        Name = lookup.Name,
                        Culture = lookup.Culture,
                        Items = new List<ChainDomainModel>()
                    };

                    children.Add(chain);

                    buildTree(chain);
                }
            };

            foreach (var result in results)
            {
                var chain = new ChainDomainModel()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Culture = result.Culture,
                    Items = Enumerable.Empty<ChainDomainModel>()
                };

                buildTree(chain);

                chains.Add(chain);
            }

            return chains;
        }
    }
}