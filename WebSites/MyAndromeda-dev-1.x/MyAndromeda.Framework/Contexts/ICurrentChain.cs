using MyAndromeda.Core;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Framework.Contexts
{
    public interface ICurrentChain : IDependency 
    {
        /// <summary>
        /// Check if chain is available the available.
        /// </summary>
        /// <value>The available.</value>
        bool Available { get; }

        /// <summary>
        /// Gets the authorized at chain level.
        /// </summary>
        /// <value>The authorized at chain level.</value>
        bool AuthorizedAtChainLevel { get; }

        /// <summary>
        /// Gets the chain.
        /// </summary>
        /// <value>The chain.</value>
        ChainDomainModel Chain { get; }

        /// <summary>
        /// Gets the sites belonging to chain.
        /// </summary>
        /// <value>The sites belonging to chain.</value>
        SiteDomainModel[] SitesBelongingToChain { get; }


    }
}