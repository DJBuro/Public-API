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
using Domain = MyAndromedaDataAccess.Domain;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;

namespace MyAndromeda.Data.DataAccess.Users
{
    public class UserChainsDataService : IUserChainsDataService
    {
        private readonly IMyAndromedaLogger logger;

        private readonly AndroAdminDbContext androAdminDbContext;
        private readonly MyAndromedaDbContext myAndromedaDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserChainsDataService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="androAdminDbContext">The andro admin db context.</param>
        /// <param name="myAndromedaDbContext">My andromeda db context.</param>
        public UserChainsDataService(IMyAndromedaLogger logger,
            AndroAdminDbContext androAdminDbContext,
            MyAndromedaDbContext myAndromedaDbContext)
        {
            this.logger = logger;
            this.androAdminDbContext = androAdminDbContext;
            this.myAndromedaDbContext = myAndromedaDbContext;
        }

        /// <summary>
        /// Gets the chains for user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public IEnumerable<Domain.Chain> GetChainsForUser(int userId)
        {
            bool failedAtMyAndromeda = false;
            //bool failedAtAndroAdmin = false;

            IEnumerable<Domain.Chain> chains = Enumerable.Empty<Domain.Chain>();

            try
            {
                IEnumerable<int> accessibleChains = Enumerable.Empty<int>();

                //fetch list in myandromeda to compare to androadmin 
                try
                {
                    var userChainsTable = this.myAndromedaDbContext.UserChains;
                    var userChainsQuery = userChainsTable
                                                         .Where(e => e.UserRecordId == userId)
                                                         .Select(e => e.ChainId);

                    accessibleChains = userChainsQuery.ToArray();
                }
                catch (Exception e)
                {
                    var context = new MyAndromedaDbContext();

                    this.logger.Error("Chains Loaded from MyAndromeda failed", e);
                    this.logger.Error("Connection string:" + context.Database.Connection.ConnectionString);

                    failedAtMyAndromeda = true;

                    throw e;
                }

                var chainTable = this.androAdminDbContext.Chains
                                     .Include(e => e.Children)
                                     .Include(e => e.Parents)
                                     .ToArray();

                var chainQuery = chainTable.Where(e => accessibleChains.Contains(e.Id));

                //top node on the chain structure 
                var chainResult = chainQuery.ToArray();

                if (chainResult.Length == 0)
                {
                    return chains;
                }

                chains = this.CreateHierarchyStructure(chainResult).ToArray();
            }
            catch (Exception e)
            {
                if (failedAtMyAndromeda)
                {
                    throw e;
                }

                this.logger.Error("Chains Loaded from AndroAdmin failed", e);
                this.logger.Error("Connection string:" + this.androAdminDbContext.Database.Connection.ConnectionString);

                throw;
            }

            return chains;
        }

        public IEnumerable<AndroWebOrderingWebsite> GetAndroWebOrderingSitesForUser(int userId)
        {
            IEnumerable<Domain.Chain> chainsResult = this.GetChainsForUser(userId);
            IEnumerable<AndroWebOrderingWebsite> androWebOrderingSites = 
                Enumerable.Empty<AndroWebOrderingWebsite>();

            if (chainsResult.ToList().Count > 0)
            {
                var websites = this.androAdminDbContext.AndroWebOrderingWebsites.ToList();
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
        public IEnumerable<Domain.Chain> GetChainsForUser(int userId, Expression<Func<Chain, bool>> query = null)
        {
            if (query == null)
            {
                query = (_) => true;
            }

            IEnumerable<Domain.Chain> chains = Enumerable.Empty<Domain.Chain>();
            //using (var androAdminDbContext = new Model.AndroAdmin.AndroAdminDbContext())
            {
                IEnumerable<int> accessibleChains = Enumerable.Empty<int>();
                //using (var myAndromedaDbContext = new Model.MyAndromeda.MyAndromedaDbContext())
                {
                    var userChainsTable = this.myAndromedaDbContext.UserChains;
                    var userChainsQuery = userChainsTable
                                                         .Where(e => e.UserRecordId == userId)
                                                         .Select(e => e.ChainId);

                    var userChainsResult = userChainsQuery.ToArray();

                    accessibleChains = userChainsResult;
                }

                var chainTable = this.androAdminDbContext.Chains;
                var chainQuery = chainTable
                                           .Where(query)
                                           .Where(e => accessibleChains.Any(chainId => chainId == e.Id));

                //top node on the chain structure 
                var chainResult = chainQuery.ToArray();

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
        public void AddChainLinkToUser(Domain.Chain chain, int userId)
        {
            //using (var myAndromedaDbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var userChainsTable = this.myAndromedaDbContext.UserChains;
                if (userChainsTable.Any(e => e.ChainId == chain.Id && e.UserRecordId == userId))
                {
                    return;
                }

                var link = userChainsTable.Create();
                link.ChainId = chain.Id;
                link.UserRecordId = userId;

                userChainsTable.Add(link);

                this.myAndromedaDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Finds the users belonging to chain.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        public IEnumerable<MyAndromedaUser> FindUsersDirectlyBelongingToChain(int chainId)
        {
            IEnumerable<MyAndromedaUser> myAndromedaUserusers;

            //using (var myAndromedaDbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                IEnumerable<UserRecord> userRecords = this.myAndromedaDbContext.UserChains.Where(e => e.ChainId == chainId).Select(e => e.UserRecord);
                var result = userRecords.ToArray();

                myAndromedaUserusers = result.Select(e => e.ToDomain()).ToArray();
            }

            return myAndromedaUserusers;
        }

        public IEnumerable<Domain.Chain> FindChainsDirectlyBelongingToUser(int userId)
        {
            IEnumerable<Domain.Chain> chains = Enumerable.Empty<Domain.Chain>();

            //using (var myAndromedaDbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var userChainsTable = this.myAndromedaDbContext.UserChains;
                var userChainsquery = userChainsTable.Where(e => e.UserRecordId == userId).Select(e => e.ChainId).ToArray();

                //using (var androAdminDbContext = new Model.AndroAdmin.AndroAdminDbContext())
                {
                    var chainsquery = this.androAdminDbContext.Chains.Where(chain => userChainsquery.Contains(chain.Id));
                    var chainsResult = chainsquery.ToArray().Select(e => new Domain.Chain()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Culture = e.Culture
                    }).ToArray();

                    chains = chainsResult;
                }
            }

            return chains;
        }

        public void RemoveChainLinkToUser(int userId, int chainId)
        {
            //using (var myAndromedaDbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var table = this.myAndromedaDbContext.UserChains;
                var query = table.Where(e => e.ChainId == chainId && e.UserRecordId == userId);
                var results = query.ToArray();

                foreach (var result in results)
                {
                    this.myAndromedaDbContext.UserChains.Remove(result);
                }

                this.myAndromedaDbContext.SaveChanges();
            }
        }

        private IEnumerable<Domain.Chain> CreateHierarchyStructure(Chain[] results)
        {
            var chains = new List<Domain.Chain>(results.Length);

            var linkTable = this.androAdminDbContext.ChainChains;
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

            Action<Domain.Chain> buildTree = null;
            buildTree = (node) =>
            {
                if (!linkResult.ContainsKey(node.Id))
                {
                    return;
                }

                var lookupByParentId = linkResult[node.Id];
                var children = new List<Domain.Chain>(lookupByParentId.Length);

                node.Items = children;

                foreach (var lookup in lookupByParentId)
                {
                    var chain = new Domain.Chain()
                    {
                        Id = lookup.Id,
                        Name = lookup.Name,
                        Culture = lookup.Culture,
                        Items = new List<Domain.Chain>()
                    };

                    children.Add(chain);

                    buildTree(chain);
                }
            };

            foreach (var result in results)
            {
                var chain = new Domain.Chain()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Culture = result.Culture,
                    Items = Enumerable.Empty<Domain.Chain>()
                };

                buildTree(chain);

                chains.Add(chain);
            }

            return chains;
        }
    }
}