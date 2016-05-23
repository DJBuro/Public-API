using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Loyalty
{
    public class StoreEnrolementPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Store VoucherCode Feature";

        public static Permission ViewLoyaltyFeature = new Permission()
        {
            Name = "View Loyalty Feature",
            Description = "View the loyalty section",
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
            yield return ViewLoyaltyFeature;
            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] {
                new PermissionStereotype()
                {
                    Permission = ViewLoyaltyFeature,
                    AllowRoles = new [] {
                        ExpectedStoreEnrollments.DefaultStoreEnrollment,
                        ExpectedStoreEnrollments.FullEnrollment,
                        ExpectedStoreEnrollments.GprsStore,
                        ExpectedStoreEnrollments.RamesesStoreEnrollment
                    }
                }
            };
        }
    }
}