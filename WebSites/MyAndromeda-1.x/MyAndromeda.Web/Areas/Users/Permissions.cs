using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Web.Areas.Users
{
    public class Permissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;

        public const string Category = "Users";
        public static Permission CreateUsers = new Permission()
        {
            Category = Category,
            Name = "Create/Edit Users",
            Description = "Edit users over their chain",
            PermissionType = ProviderPermissionType
        };

        public static Permission ListUsers = new Permission()
        {
            Category = Category,
            Name = "List Users",
            Description = "View users over the chain",
            PermissionType = ProviderPermissionType
        };

        public static Permission ChangeUserRole = new Permission()
        {
            Category = Category,
            Name = "Edit User Roles",
            Description = "Edit a user role",
            PermissionType = ProviderPermissionType
        };

        public static Permission ResetPassword = new Permission()
        {
            Category = Category,
            Name = "Reset a user password",
            Description = "Reset a user password",
            PermissionType = ProviderPermissionType
        };
        
        public static Permission RemoveUserFromChain = new Permission()
        {
            Category = Category,
            Name = "Remove user",
            Description = "Remove a user from chain",
            PermissionType = ProviderPermissionType
        };

        public static Permission DisableUser = new Permission()
        {
            Category = Category,
            Name = "Disable user",
            Description = "Disables the user from the site.",
            PermissionType = ProviderPermissionType
        };

        public static Permission EditIpLock = new Permission()
        {
            Category = Category,
            Name = "Edit IP lock",
            Description = "Edit the IP location that a user can login with.",
            PermissionType = ProviderPermissionType
        };

        public static Permission DeleteIpLock = new Permission()
        {
            Category = Category,
            Name = "Delete IP lock",
            Description = "Delete the IP location that a user can login with.",
            PermissionType = ProviderPermissionType
        };

        public static Permission AssignAdministratorRole = new Permission()
        {
            Category = Category,
            Name = "Assign administrator role",
            Description = "User can add or remove the administrator role for a user.",
            PermissionType = ProviderPermissionType
        };

        public static Permission AssignSuperAdministratorRole = new Permission()
        {
            Category = Category,
            Name = "Assign super administrator role",
            Description = "User can add or remove the super administrator role for a user.",
            PermissionType = ProviderPermissionType
        };

        public static Permission RemoveUserFromStore = new Permission()
        {
            Category = Category,
            Name = "Remove user",
            Description = "Remove a user from store",
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
            yield return ListUsers;
            yield return CreateUsers;
            yield return RemoveUserFromChain;
            yield return RemoveUserFromStore;
            yield return ChangeUserRole;
            yield return ResetPassword;
            yield return DisableUser;

            yield return EditIpLock;
            yield return DeleteIpLock;

            yield return AssignAdministratorRole;
            yield return AssignSuperAdministratorRole;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] {
                new PermissionStereotype(){
                    Permission = ListUsers,
                    AllowRoles = new []{ ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype(){
                    Permission = CreateUsers,
                    AllowRoles = new[] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype(){
                    Permission = RemoveUserFromChain,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype(){
                    Permission = ChangeUserRole,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator } 
                },
                new PermissionStereotype(){
                    Permission = ResetPassword,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype(){
                    Permission = DisableUser,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype(){
                    Permission = AssignAdministratorRole,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype() {
                    Permission = EditIpLock, 
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype() {
                    Permission = DeleteIpLock,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator }
                },
                new PermissionStereotype() {
                    Permission = AssignSuperAdministratorRole,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator }
                },
                new PermissionStereotype(){
                    Permission = RemoveUserFromStore,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                }
            };
        }
    }
}