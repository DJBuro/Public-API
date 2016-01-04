using System.Collections.Generic;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.Marketing
{
    public class MarketingFeatureEnrollment : IPermissionProvider 
    {
        public const string Category = "Marketing";
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;

        public static Permission MaketingFeature = new Permission()
        {
            Name = "Email marketing feature",
            Category = Category,
            Description = "Allow the marketing feature",
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
            yield return MaketingFeature;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[]
            {
                new PermissionStereotype() { 
                    Permission = MaketingFeature,
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