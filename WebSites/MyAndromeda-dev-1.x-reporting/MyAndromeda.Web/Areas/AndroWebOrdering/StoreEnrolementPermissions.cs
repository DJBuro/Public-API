using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.AndroWebOrdering
{
    public class StoreEnrolementPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Websites";

        public static Permission ViewDeliveryZoneFeature = new Permission()
        {
            Name = "Website feature ",
            Description = "Has access the website features.",
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
            yield return ViewDeliveryZoneFeature;

        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    Permission = ViewDeliveryZoneFeature,
                    AllowRoles = new [] 
                    {
                        ExpectedStoreEnrollments.DefaultStoreEnrollment,
                        ExpectedStoreEnrollments.FullEnrollment,
                        ExpectedStoreEnrollments.RamesesStoreEnrollment,
                        ExpectedStoreEnrollments.GprsStore
                    }
                }
            };
        }
    }
}