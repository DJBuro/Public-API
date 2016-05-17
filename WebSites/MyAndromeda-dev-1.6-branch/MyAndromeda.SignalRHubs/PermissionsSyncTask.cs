using System.Collections.Generic;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.SignalRHubs
{
    public class PermissionsSyncTask : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;

        public static Permission ViewSyncTaskLogging = new Permission()
        {
            Name = "View cloud synchronization logging",
            Description = "Administrator task of reviewing active synchronization logs",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public const string Category = "Logging";

        public PermissionType PermissionType
        {
            get
            {
                return PermissionType.UserRole;
            }
        }

        public IEnumerable<Permission> GetPermissions() 
        {
            yield return ViewSyncTaskLogging;
            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[]
            {
                new PermissionStereotype() 
                {
                    Permission = ViewSyncTaskLogging, 
                    AllowRoles = new[] { 
                        ExpectedUserRoles.SuperAdministrator,
                        ExpectedUserRoles.Administrator,
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                }
            };
        }
    }
}