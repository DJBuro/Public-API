using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Voucher
{
    public class VoucherCodesFeature : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Vouchers";

        public static Permission HasVoucherCodesFeature = new Permission()
        {
            Name = "View VoucherCode Feature",
            Description = "View the VoucherCode section",
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
            yield return HasVoucherCodesFeature;
            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] 
            { 
                new PermissionStereotype()
                {
                    Permission = HasVoucherCodesFeature,
                    AllowRoles = new []{
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