using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.OrderManagement
{
    public class StoreEnrolementPermissions
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Store OrderManagement Feature";

        public static Permission ViewOrderManagementArea = new Permission()
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
            yield return ViewOrderManagementArea;
            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { };
        }
    }
}