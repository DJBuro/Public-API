using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Authorization.Management
{
    public interface IPermissionManager : IDependency
    {
        /// <summary>
        /// Updates the permissions.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="permissions">The permissions.</param>
        void UpdatePermissions(IUserRole role, IEnumerable<IPermission> permissions);

        /// <summary>
        /// Updates the permissions.
        /// </summary>
        /// <param name="enrolementLevel">The enrolment level.</param>
        /// <param name="permissions">The permissions.</param>
        void UpdatePermissions(IEnrolmentLevel enrolementLevel, IEnumerable<IPermission> permissions);

        /// <summary>
        /// Gets the effective permissions for user-role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectivePermissionsForRole(IUserRole role);

        /// <summary>
        /// Gets the effective permissions for site.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectivePermissionsForSite(ISite site);

        /// <summary>
        /// Gets the effective permissions for enrolment.
        /// </summary>
        /// <param name="enrolmentLevel">The enrolment level.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectivePermissionsForEnrolment(IEnrolmentLevel enrolmentLevel);

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPermission> GetInternalPermissions();

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPermission> GetInternalPermissions(PermissionType permissionType);

        /// <summary>
        /// Gets the stereotypes.
        /// </summary>
        /// <param name="permissionType">Type of the permission.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetStereotypes(PermissionType permissionType, string role);

        /// <summary>
        /// Gets the internal stereotypes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PermissionStereotype> GetInternalStereotypes();
    }
}
