using System.Collections.Generic;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Web.Areas.Menu
{
    public class UserPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Menu";

        public static Permission ViewMenuFeature = new Permission()
        {
            Name = "View Menu Feature",
            Description = "View the menu section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditMenuFeature = new Permission()
        {
            Name = "Edit Menu Feature",
            Description = "Allow the user to edit the menu",
            Category = Category,
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
            yield return ViewMenuFeature;
            yield return EditMenuFeature;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    AllowRoles = new [] { ExpectedRoles.Administrator, ExpectedRoles.SuperAdministrator },
                    Permission = ViewMenuFeature,
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { ExpectedRoles.Administrator, ExpectedRoles.SuperAdministrator },
                    Permission = EditMenuFeature
                }
            };
        }
    }
}