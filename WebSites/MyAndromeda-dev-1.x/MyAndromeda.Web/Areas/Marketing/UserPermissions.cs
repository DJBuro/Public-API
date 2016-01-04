using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Web.Areas.Marketing
{
    public class UserPermissions : IPermissionProvider
    {
        public const string Category = "Marketing";
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;

        public static Permission ChangeEventMarketing = new Permission()
        {
            Name = "Change event marketing",
            Category = Category,
            Description = "Allow user to change email settings",
            PermissionType = ProviderPermissionType
        };

        public static Permission ChangeEmailSettings = new Permission() 
        { 
            Name = "Change email settings", 
            Category = Category,
            Description = "Allow user to change email settings" ,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewEmailCampaigns = new Permission()
        {
            Name = "View email campaigns",
            Category = Category,
            Description = "Allow user to create or edit email campaigns",
            PermissionType = ProviderPermissionType
        };

        public static Permission CreateCampaignEmails = new Permission()
        {
            Name = "Create email campaigns",
            Category = Category,
            Description = "Allow user to create email campaigns",
            PermissionType = ProviderPermissionType
        };

        public static Permission EditCampaignEmails = new Permission() 
        { 
            Name = "Edit email campaigns", 
            Category = Category,
            Description = "Allow user to edit email campaigns", 
            PermissionType = ProviderPermissionType
        };

        public static Permission DeleteCampaignEmails = new Permission()
        {
            Name = "Delete email campaigns",
            Category = Category,
            Description = "Allow user to delete email campaigns",
            PermissionType = ProviderPermissionType
        };

        public static Permission SendCampaignEmails = new Permission() 
        { 
            Name = "Send email campaigns", 
            Category = Category,
            Description = "Allow user to send email campaigns",
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewCampaignHistory = new Permission()
        {
            Name = "View campaign history",
            Category = Category,
            Description = "View the campaign history",
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
            yield return ViewEmailCampaigns;
            yield return CreateCampaignEmails;
            yield return EditCampaignEmails;
            yield return DeleteCampaignEmails;
            yield return SendCampaignEmails;
            yield return ViewCampaignHistory;

            yield return ChangeEmailSettings;


            yield return ChangeEventMarketing;

            yield break;
        }

        public IEnumerable<Permission> GetPermissions(string category)
        {
            return this.GetPermissions()
                .Where(e => e.Category.Equals(category, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new[]
            {
                new PermissionStereotype()
                {
                    Permission = ViewEmailCampaigns,
                    AllowRoles = new[] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator, ExpectedUserRoles.ChainAdministrator, ExpectedUserRoles.StoreAdministrator }
                },
                
                new PermissionStereotype()
                {
                    Permission = CreateCampaignEmails,
                    AllowRoles = new[] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator, ExpectedUserRoles.ChainAdministrator, ExpectedUserRoles.StoreAdministrator }
                },
                new PermissionStereotype()
                {
                    Permission = EditCampaignEmails,
                    AllowRoles = new[] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator, ExpectedUserRoles.ChainAdministrator, ExpectedUserRoles.StoreAdministrator }
                },
                new PermissionStereotype()
                {
                    Permission = DeleteCampaignEmails,
                    AllowRoles = new[] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator, ExpectedUserRoles.ChainAdministrator, ExpectedUserRoles.StoreAdministrator }
                },
                new PermissionStereotype()
                {
                    Permission = SendCampaignEmails,
                    AllowRoles = new[] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator, ExpectedUserRoles.ChainAdministrator, ExpectedUserRoles.StoreAdministrator }
                },
                new PermissionStereotype()
                {
                    Permission = ViewCampaignHistory,
                    AllowRoles = new[] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator, ExpectedUserRoles.ChainAdministrator, ExpectedUserRoles.StoreAdministrator }
                },
                new PermissionStereotype()
                {
                    Permission = ChangeEmailSettings,
                    AllowRoles = new[] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator, ExpectedUserRoles.ChainAdministrator, ExpectedUserRoles.StoreAdministrator }
                },
                new PermissionStereotype()
                {
                    Permission = ChangeEventMarketing,
                    AllowRoles = new [] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator }
                }
            };
        }
    }
}