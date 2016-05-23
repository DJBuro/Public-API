using System.Collections.Generic;
using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Authorization.Services
{
    public interface ICurrentPermissionService : IDependency
    {
        /// <summary>
        /// Gets the effective permissions for the user and site they are browsing.
        /// </summary>
        /// <returns></returns>
        PermissionGroup GetEffectivePermissions();

        /// <summary>
        /// Gets the effective permissions.
        /// </summary>
        /// <param name="currentSite">The current site.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveSitePermissions(ICurrentSite currentSite);

        /// <summary>
        /// Gets the effective permissions.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveUserPermissions(ICurrentUser currentUser);
    }

}
