using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.OrderManagement
{
    public class StoreEnrolementPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Store OrderManagement Feature";

        public static Permission ViewOrderManagementFeature = new Permission()
        {
            Name = "View OrderManagement Feature",
            Description = "View the OrderManagement section",
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
            yield return ViewOrderManagementFeature;
            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    Permission = ViewOrderManagementFeature,
                    AllowRoles = new [] {
                        ExpectedStoreEnrollments.FullEnrollment,
                        ExpectedStoreEnrollments.DefaultStoreEnrollment,
                        ExpectedStoreEnrollments.RamesesStoreEnrollment,
                        ExpectedStoreEnrollments.GprsStore
                    }
                }
            };
        }
    }
}