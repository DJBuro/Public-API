using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.SignalRHubs
{
    public class PermissionsMenuHub : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;

        public static Permission ViewMenuFtpLogging = new Permission()
        {
            Name = "View Menu ftp logging",
            Description = "Administrator task of reviewing the active transaction logs.",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public const string Category = "Logging";

        public PermissionType PermissionType
        {
            get
            {
                return ProviderPermissionType;
            }
        }

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ViewMenuFtpLogging;
            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[]
            {
                new PermissionStereotype() 
                {
                    Permission = ViewMenuFtpLogging, 
                    AllowRoles = new[] {
                        ExpectedUserRoles.SuperAdministrator,
                        ExpectedUserRoles.Administrator
                    }
                }
            };
        }
    }
}
