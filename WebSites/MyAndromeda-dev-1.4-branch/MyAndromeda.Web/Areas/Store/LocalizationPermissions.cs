using System.Collections.Generic;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.Store
{
    public class LocalizationPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Localization";

        public static Permission ViewAndEditLocalization = new Permission()
        {
            Name = "Edit store localization",
            Description = "The user can change the time-zone and culture of a store.",
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
            yield return ViewAndEditLocalization;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new[] 
            {
                new PermissionStereotype()
                {
                    Permission = ViewAndEditLocalization,
                    AllowRoles = new[] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator }
                }
            };
        }
    }
}