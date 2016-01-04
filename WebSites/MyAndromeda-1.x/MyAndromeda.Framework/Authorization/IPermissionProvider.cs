using System.Collections.Generic;
using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Framework.Authorization
{
    public interface IPermissionProvider : IDependency
    {
        /// <summary>
        /// Gets the type of the permission. Store / User
        /// </summary>
        /// <value>The type of the permission.</value>
        PermissionType PermissionType { get; }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Permission> GetPermissions();

        /// <summary>
        /// Gets the permission stereotypes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PermissionStereotype> GetPermissionStereotypes();
    }
}