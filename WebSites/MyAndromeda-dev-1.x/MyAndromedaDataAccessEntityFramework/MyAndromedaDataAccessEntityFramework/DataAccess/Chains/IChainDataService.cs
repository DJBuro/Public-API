using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Data.DataAccess.Chains
{
    public interface IChainDataService : IDependency
    {
        /// <summary>
        /// Gets the specified chain id.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        ChainDomainModel Get(int chainId);

        /// <summary>
        /// Lists the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<ChainDomainModel> List(Expression<Func<Chain, bool>> query);

        /// <summary>
        /// Gets the site list of a chain.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        IEnumerable<SiteDomainModel> GetSiteList(int chainId);

        /// <summary>
        /// Updates the specified chain.
        /// </summary>
        /// <param name="chain">The chain.</param>
        void Update(ChainDomainModel model); 
    }
}
