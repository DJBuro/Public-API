using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Acs
{
    public class UserPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Web Hooks";

        public static Permission ViewAcsPermission = new Permission()
        {
            Name = "View ACS applications",
            Description = "View ACS applications",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditWebhooksPermission = new Permission()
        {
            Name = "Edit Web hooks",
            Description = "View ACS applications",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public Core.Authorization.PermissionType PermissionType
        {
            get
            {
                return ProviderPermissionType;
            }
        }

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ViewAcsPermission;
            yield return EditWebhooksPermission;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    Permission = ViewAcsPermission,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                    }
                },
                new PermissionStereotype() {
                    Permission = EditWebhooksPermission,
                    AllowRoles = new [] {                         
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                    }
                },
            };
        }
    }
}