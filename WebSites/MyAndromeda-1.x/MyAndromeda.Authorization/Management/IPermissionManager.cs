using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using MyAndromeda.Framework.Authorization;
using MyAndromedaDataAccessEntityFramework.DataAccess.Permissions;

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
        /// Gets the internal stereotypes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PermissionStereotype> GetInternalStereotypes();
    }

    public class PermissionManager : IPermissionManager 
    {
        private readonly IPermissionDataAccessService permissionDataAccessService;
        private readonly IPermissionProvider[] permissionProviders;

        public PermissionManager(IPermissionDataAccessService permissionDataAccessService, IPermissionProvider[] permissionProviders)
        { 
            this.permissionDataAccessService = permissionDataAccessService;
            this.permissionProviders = permissionProviders;
        }

        /// <summary>
        /// Updates the permissions.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="permissions">The permissions.</param>
        public void UpdatePermissions(IUserRole role, IEnumerable<IPermission> permissions)
        {
            this.permissionDataAccessService.UpdateRole(role, permissions);
        }

        /// <summary>
        /// Updates the permissions.
        /// </summary>
        /// <param name="enrolementLevel">The enrolment level.</param>
        /// <param name="permissions">The permissions.</param>
        public void UpdatePermissions(IEnrolmentLevel enrolementLevel, IEnumerable<IPermission> permissions)
        {
            this.permissionDataAccessService.UpdateEnrolmentPermissions(enrolementLevel, permissions);
        }

        /// <summary>
        /// Gets the effective permissions for user-role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public IEnumerable<IPermission> GetEffectivePermissionsForRole(IUserRole role)
        {
            return this.permissionDataAccessService.GetEffectivePermissions(role);
        }

        /// <summary>
        /// Gets the effective permissions for site.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        public IEnumerable<IPermission> GetEffectivePermissionsForSite(ISite site)
        {
            return this.permissionDataAccessService.GetEffectivePermissions(site);
        }

        /// <summary>
        /// Gets the effective permissions for enrolment.
        /// </summary>
        /// <param name="enrolmentLevel">The enrolment level.</param>
        /// <returns></returns>
        public IEnumerable<IPermission> GetEffectivePermissionsForEnrolment(IEnrolmentLevel enrolmentLevel)
        {
            return this.permissionDataAccessService.GetEffectivePermissions(enrolmentLevel);
        }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPermission> GetInternalPermissions() 
        {
            var dbPermissions = this.permissionDataAccessService.GetAllPermissions();
            var internalPermissions = this.permissionProviders.SelectMany(e => e.GetPermissions());

            this.EnsurePermissionsAreAdded(internalPermissions, dbPermissions);

            return internalPermissions.Select(e => e as IPermission);
        }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public IEnumerable<IPermission> GetInternalPermissions(PermissionType permissionType)
        {
            var dbPermissions = this.permissionDataAccessService.GetAllPermissions();
            var internalPermissions = this.permissionProviders
                                          //.Where(e => e.PermissionType == permissionType)
                                          .SelectMany(e => e.GetPermissions().Where(permission => permission.PermissionType == permissionType));

            this.EnsurePermissionsAreAdded(internalPermissions, dbPermissions);

            return internalPermissions.Select(e => e as IPermission);
        }

        /// <summary>
        /// Gets the internal stereotypes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PermissionStereotype> GetInternalStereotypes() 
        {
            var items = this.permissionProviders.SelectMany(e => e.GetPermissionStereotypes());

            return items;
        }

        /// <summary>
        /// Ensures the permissions are added to the database.
        /// </summary>
        /// <param name="internalPermissions">The internal permissions.</param>
        /// <param name="dbPermissions">The db permissions.</param>
        private void EnsurePermissionsAreAdded(IEnumerable<IPermission> internalPermissions, IEnumerable<IPermission> dbPermissions) 
        {
            foreach (var internalPermission in internalPermissions) 
            {
                var dbPermission = dbPermissions.FirstOrDefault(e => e.Name == internalPermission.Name && e.Category == internalPermission.Category);
                if (dbPermission == null)
                    continue;

                internalPermission.Id = dbPermission.Id;
            }

            var notInDatabase = internalPermissions
                                                   .Where(e => !dbPermissions.Any(permission => permission.Name == e.Name && permission.Category == e.Category))
                                                   .ToArray();

            this.permissionDataAccessService.AddOrUpdatePermissions(notInDatabase);
        }
    }
}
