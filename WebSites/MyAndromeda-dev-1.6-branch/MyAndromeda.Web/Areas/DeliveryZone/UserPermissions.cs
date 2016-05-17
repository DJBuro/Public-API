using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.DeliveryZone
{
    public class UserPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Delivery Zone";

        public static Permission ViewDeliveryZone = new Permission()
        {
            Name = "View delivery zone",
            Description = "View the delivery zones for a store.",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditDeliveryZone = new Permission()
        {
            Name = "Edit delivery zone",
            Description = "edit the delivery zones for a store.",
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
            yield return ViewDeliveryZone;
            yield return EditDeliveryZone;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    },
                    Permission = ViewDeliveryZone,
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator
                    },
                    Permission = EditDeliveryZone
                },
            };
        }
    }
}