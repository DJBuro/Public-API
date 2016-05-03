using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccessEntityFramework.DataAccess.Permissions;

namespace MyAndromeda.Authorization.Services
{

    public class CurrentPermissionService : ICurrentPermissionService 
    {
        private readonly ICurrentUser currentUser;
        private readonly ICurrentSite currentSite;
        private readonly IPermissionDataAccessService permissionDataAccess;
        private readonly IPermissionProvider[] permissionProviders;

        public CurrentPermissionService(
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            IPermissionDataAccessService permissionDataAccess,
            IPermissionProvider[] permissionProviders)
        {
            this.currentUser = currentUser;
            this.currentSite = currentSite;
            this.permissionDataAccess = permissionDataAccess;
            this.permissionProviders = permissionProviders;
        }

        /// <summary>
        /// Gets the effective permissions for the user and site they are browsing.
        /// </summary>
        /// <returns></returns>
        public PermissionGroup GetEffectivePermissions() 
        {
            if (!this.currentUser.Available)
            {
                return new PermissionGroup();
            }

            IEnumerable<IPermission> sitePermissions;
            IEnumerable<IPermission> userPermissions;

            //what the "store" is allowed to do 
            sitePermissions = this.GetEffectiveSitePermissions(this.currentSite);
            //what the "user" is allowed to do 
            userPermissions = this.GetEffectiveUserPermissions(this.currentUser);

            //this.effectivePermissions = sitePermissions.Union(userPermissions).ToArray();

            return new PermissionGroup()
            {
                StorePermissions = sitePermissions,
                UserPermissions = userPermissions
            };

            //return this.effectivePermissions;
        }

        /// <summary>
        /// Gets the effective permissions.
        /// </summary>
        /// <param name="currentSite">The current site.</param>
        /// <returns></returns>
        public IEnumerable<IPermission> GetEffectiveSitePermissions(ICurrentSite currentSite)
        {
            IEnumerable<IPermission> sitePermissions = Enumerable.Empty<IPermission>();

            if (currentSite.Available)
            {
                sitePermissions = this.permissionDataAccess.GetEffectiveEntolementPermissions(this.currentSite.Site);

                IEnumerable<IPermissionProvider> sitePermissionProviders = this.permissionProviders.Where(e => e.PermissionType == PermissionType.StoreEnrolement);

                IEnumerable<string> storeFeatureEnrolements = currentSite.EnrolmentLevels.Select(e => e.Name);
                
                //check if any provider wants to grant a permission beyond what the database says
                Permission[] validPermissionSterotype =
                    sitePermissionProviders
                                           .SelectMany(e => e.GetPermissionStereotypes())
                    //where any role matches the user role
                                           .Where(e => e.AllowRoles.Any(role => storeFeatureEnrolements.Any(siteRoleName => siteRoleName.Equals(role, StringComparison.InvariantCultureIgnoreCase))))
                                           .Select(e => e.Permission)
                                           .ToArray();

                IPermission[] result = sitePermissions.Union(validPermissionSterotype).ToArray();

                return result;
            }

            return sitePermissions;
        }

        /// <summary>
        /// Gets the effective permissions.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        public IEnumerable<IPermission> GetEffectiveUserPermissions(ICurrentUser currentUser)
        {
            IEnumerable<IPermission> userPermissions = Enumerable.Empty<IPermission>();

            userPermissions = this.permissionDataAccess.GetEffectiveUserPermissions(currentUser.User.Id);

            List<string> roles = currentUser.Roles.Select(e => e.Name).ToList();
            roles.Add(ExpectedUserRoles.User);

            IEnumerable<IPermissionProvider> userPermissionProviders = this.permissionProviders.Where(e => e.PermissionType == PermissionType.UserRole);

            //check if any provider wants to grant a permission further to what is in the database
            IEnumerable<Permission> validPermissionSterotype =
                userPermissionProviders
                                       .SelectMany(e => e.GetPermissionStereotypes())
                //where any role matches the user role
                                       .Where(e => e.AllowRoles.Any(role => roles.Any(userRoleNames => userRoleNames.Equals(role, StringComparison.InvariantCultureIgnoreCase))))
                                       .Select(e => e.Permission);
            
            return userPermissions.Union(validPermissionSterotype);
        }
    }

}