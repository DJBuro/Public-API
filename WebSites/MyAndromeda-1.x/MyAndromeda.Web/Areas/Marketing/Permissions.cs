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

        public static Permission ChangeEmailSettings = new Permission() 
        { 
            Name = "Change email settings", 
            Category = Category,
            Description = "Allow user to change email settings" ,
            PermissionType = ProviderPermissionType
        };

        public static Permission CreateEditCampaignEmails = new Permission() 
        { 
            Name = "Create/Edit email campaigns", 
            Category = Category,
            Description = "Allow user to create or edit email campaigns", 
            PermissionType = ProviderPermissionType
        };

        public static Permission SendCampaignEmails = new Permission() 
        { 
            Name = "Send email campaigns", 
            Category = Category,
            Description = "Allow user to send email campaigns",
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
            yield return ChangeEmailSettings;
            yield return CreateEditCampaignEmails;
            yield return SendCampaignEmails;

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
                    Permission = ChangeEmailSettings,
                    AllowRoles = new[] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype()
                {
                    Permission = CreateEditCampaignEmails,
                    AllowRoles = new[] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype()
                {
                    Permission = SendCampaignEmails,
                    AllowRoles = new[] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                }
            };
        }
    }
}