using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Voucher
{
    public class UserPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Voucher";

        public static Permission ViewVoucherCodes = new Permission()
        {
            Name = "View VoucherCodes Feature",
            Description = "View the VoucherCodes section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission CreateOrEditVoucherCodes = new Permission()
        {
            Name = "Create or Edit VoucherCodes Feature",
            Description = "Allow the user to Create or Edit VoucherCodes section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission DeleteVoucherCodes = new Permission()
        {
            Name = "Delete VoucherCodes Feature",
            Description = "Allow the user to Delete VoucherCodes section",
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
            yield return ViewVoucherCodes;
            yield return CreateOrEditVoucherCodes;
            yield return DeleteVoucherCodes;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    AllowRoles = new [] { ExpectedRoles.Administrator, ExpectedRoles.SuperAdministrator },
                    Permission = ViewVoucherCodes,
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { ExpectedRoles.Administrator, ExpectedRoles.SuperAdministrator },
                    Permission = CreateOrEditVoucherCodes
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { ExpectedRoles.Administrator, ExpectedRoles.SuperAdministrator },
                    Permission = DeleteVoucherCodes
                }
            };
        }
    }
}