using System.Collections.Generic;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.Marketing
{
    public class EnrolmentPermissions : IPermissionProvider 
    {
        public const string Category = "Marketing";
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;

        public static Permission MaketingCategory = new Permission()
        {
            Name = "Marketing Feature",
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
            yield return MaketingCategory;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[]
            {
                new PermissionStereotype() { }
            };
        }
    }
}