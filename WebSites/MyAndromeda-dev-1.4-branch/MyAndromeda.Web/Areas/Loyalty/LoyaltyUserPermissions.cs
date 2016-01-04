using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Loyalty
{
    public class LoyaltyUserPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Loyalty";

        public static Permission ViewLoyalty = new Permission()
        {
            Name = "View Loyalty Feature",
            Description = "View the VoucherCodes section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditLoyalty = new Permission()
        {
            Name = "Create or Edit Loyalty Feature",
            Description = "Allow the user to Create or Edit VoucherCodes section",
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
            yield return ViewLoyalty;
            yield return EditLoyalty;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    Permission = ViewLoyalty,
                    AllowRoles = new [] { ExpectedUserRoles.Administrator, ExpectedUserRoles.SuperAdministrator }
                },
                new PermissionStereotype()
                {
                    Permission = EditLoyalty,
                    AllowRoles = new [] { ExpectedUserRoles.Administrator, ExpectedUserRoles.SuperAdministrator }
                }
            };
        }
    }
}