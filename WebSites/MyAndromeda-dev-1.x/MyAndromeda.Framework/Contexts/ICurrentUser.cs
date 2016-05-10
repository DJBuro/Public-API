using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.User;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Framework.Contexts
{
    public interface ICurrentUser : IDependency 
    {
        bool Available { get; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>The user.</value>
        MyAndromedaUser User { get; }

        /// <summary>
        /// Gets the accessible chains to the user.
        /// </summary>
        /// <value>The accessible chains.</value>
        ChainDomainModel[] AccessibleChains { get; }

        /// <summary>
        /// flattened chain list based on the tree(s) the user has access to.
        /// </summary>
        /// <value>The flattened chains.</value>
        ChainDomainModel[] FlattenedChains { get; }

        /// <summary>
        /// Gets the accessible sites.
        /// </summary>
        /// <value>The accessible sites.</value>
        SiteDomainModel[] AccessibleSites { get; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        IUserRole[] Roles { get; }

        //bool HasAndroWebOrderingSites { get; }
    }

}