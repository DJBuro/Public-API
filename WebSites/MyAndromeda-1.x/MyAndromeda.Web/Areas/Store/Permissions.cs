using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Store
{
    public class EnrolmentPermissions : IPermissionProvider 
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
            return new PermissionStereotype[] { };
        }
    }

    public class Permissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Store";

        public static Permission EditOpeningTimes = new Permission() 
        {
            Name = "Edit opening times", 
            Description = "Can the user create/edit and and times that the store is open",
            Category = "Store",
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
            yield return EditOpeningTimes;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype {
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator },
                    Permission = EditOpeningTimes
                }
            };
        }
    }
}