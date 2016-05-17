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
            Name = "View voucher codes",
            Description = "View the VoucherCodes section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission CreateVoucherChode = new Permission()
        {
            Name = "Create voucher codes",
            Description = "Allow the user to Create or Edit VoucherCodes section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditVoucherCodes = new Permission()
        {
            Name = "Edit voucher codes",
            Description = "Allow the user to Create or Edit VoucherCodes section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission DeleteVoucherCodes = new Permission()
        {
            Name = "Delete voucher codes Feature",
            Description = "Allow the user to Delete VoucherCodes section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EnableOrDisableVoucherCodes = new Permission()
        {
            Name = "Enable/disable voucher codes",
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
            yield return CreateVoucherChode;
            yield return EditVoucherCodes;
            yield return DeleteVoucherCodes;
            yield return EnableOrDisableVoucherCodes;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    Permission = ViewVoucherCodes,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                },
                new PermissionStereotype() {
                    Permission = EditVoucherCodes,
                    AllowRoles = new [] {                         
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator 
                    }
                },
                new PermissionStereotype()
                {
                    Permission = CreateVoucherChode,
                    AllowRoles = new [] 
                    {
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator,
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                },
                new PermissionStereotype()
                {
                    Permission = DeleteVoucherCodes,
                    AllowRoles = new [] 
                    {
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EnableOrDisableVoucherCodes,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator,
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                }
            };
        }
    }
}