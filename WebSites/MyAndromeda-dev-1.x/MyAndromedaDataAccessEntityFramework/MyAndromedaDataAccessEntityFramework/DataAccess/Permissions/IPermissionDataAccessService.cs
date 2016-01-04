using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Permissions
{
    public interface IPermissionDataAccessService : IDependency
    {
        /// <summary>
        /// Adds the or update permissions.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        void AddOrUpdatePermissions(IEnumerable<IPermission> permissions);

        /// <summary>
        /// Adds the or update permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        void AddOrUpdatePermission(IPermission permission); 
        
        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="permissions">The permissions.</param>
        void UpdateRole(IUserRole role, IEnumerable<IPermission> permissions);
        
        /// <summary>
        /// Updates the enrolment permissions.
        /// </summary>
        /// <param name="enrolementLevel">The enrolment level.</param>
        /// <param name="permissions">The permissions.</param>
        void UpdateEnrolmentPermissions(MyAndromeda.Core.Site.IEnrolmentLevel enrolementLevel, IEnumerable<IPermission> permissions);

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPermission> GetAllPermissions();

        /// <summary>
        /// Gets the effective user permissions.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveUserPermissions(int userId);

        /// <summary>
        /// Gets the effective user permissions.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveUserPermissions(IUserRole role);

        /// <summary>
        /// Gets the effective enrolement permissions.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveEnrolementPermissions(IEnrolmentLevel role);

        /// <summary>
        /// Gets the effective entolement permissions.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveEntolementPermissions(ISite site);

        /// <summary>
        /// Gets the user role permissions.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<IUserRole> GetUserRolePermissions(int userId);
    }
}