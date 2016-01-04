using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Menu
{
    public class StoreEnrolementPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Store Menu Feature";

        public static Permission ViewMenuFeature = new Permission()
        {
            Name = "View Menu Feature",
            Description = "View the menu section",
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
            yield return ViewMenuFeature;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { };
        }
    }
}