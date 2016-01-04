using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Voucher
{
    public class StoreEnrolementPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Store VoucherCode Feature";

        public static Permission ViewVoucherCodeArea = new Permission()
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
            yield return ViewVoucherCodeArea;
            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { };
        }
    }
}