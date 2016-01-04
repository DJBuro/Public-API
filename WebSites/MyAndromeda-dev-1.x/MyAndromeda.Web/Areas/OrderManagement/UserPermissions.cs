using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.OrderManagement
{
    public class UserPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "OrderManagement";

        public static Permission ViewOrderDetail = new Permission()
        {
            Name = "View order detail",
            Description = "View order details for stats",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewOrderManagement = new Permission()
        {
            Name = "View OrderManagement Feature",
            Description = "View the OrderManagement section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission CreateOrEditOrderDetails = new Permission()
        {
            Name = "Create or Edit OrderDetails Feature",
            Description = "Allow the user to Create or Edit OrderDetails",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission DeleteOrderDetails = new Permission()
        {
            Name = "Delete OrderDetails",
            Description = "Allow the user to Delete OrderDetails",
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
            yield return ViewOrderDetail;
            yield return ViewOrderManagement;
            yield return CreateOrEditOrderDetails;
            yield return DeleteOrderDetails;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype() 
                {
                    AllowRoles = new [] { ExpectedUserRoles.User },
                    Permission = ViewOrderDetail
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { ExpectedUserRoles.Administrator, ExpectedUserRoles.SuperAdministrator },
                    Permission = ViewOrderManagement,
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { ExpectedUserRoles.Administrator, ExpectedUserRoles.SuperAdministrator },
                    Permission = CreateOrEditOrderDetails
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { ExpectedUserRoles.Administrator, ExpectedUserRoles.SuperAdministrator },
                    Permission = DeleteOrderDetails
                }
            };
        }
    }
}