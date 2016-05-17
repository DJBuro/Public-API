using System.Collections.Generic;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.Store
{
    public class StoreEnrolmentPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Store";

        public static Permission EditStoreDetails = new Permission()
        {
            Name = "Edit Store Details",
            Category = Category,
            Description = "Can edit store details",
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
            yield return EditStoreDetails;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    Permission = EditStoreDetails,
                    AllowRoles = new [] 
                    {
                        ExpectedStoreEnrollments.DefaultStoreEnrollment
                    }
                }
            };
        }
    }
}