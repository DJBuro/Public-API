using System.Collections.Generic;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.Authorization
{
    public class UserRolesUserPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Authorization";

        public static Permission ViewUserRoleDefinitions = new Permission()
        {
            Name = "View user role definitions",
            Category = Category,
            Description = "View 'user role definitions' and associated permissions.",
            PermissionType = ProviderPermissionType
        };

        public static Permission EditUserRoleDefinitions = new Permission()
        {
            Name = "Edit user role definitions",
            Category = Category,
            Description = "Edit 'user role definitions' and associated permissions.",
            PermissionType = ProviderPermissionType
        };

        public PermissionType PermissionType
        {
            get
            {
                return ProviderPermissionType;
            }
        }

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ViewUserRoleDefinitions;
            yield return EditUserRoleDefinitions;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new[] 
            {
                new PermissionStereotype()
                {
                    Permission = ViewUserRoleDefinitions,
                    AllowRoles = new[] { ExpectedUserRoles.Administrator, ExpectedUserRoles.SuperAdministrator }
                },
                new PermissionStereotype()
                {
                    Permission = EditUserRoleDefinitions,
                    AllowRoles = new[] { ExpectedUserRoles.Administrator, ExpectedUserRoles.SuperAdministrator }
                }
            };
        }
    }
}