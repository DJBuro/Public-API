using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.HumanResources
{
    public class UserPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Human Resources";

        public static Permission ViewHrSection = new Permission()
        {
            Name = "View HR Section",
            Description = "View Employee Records",
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
            yield return ViewHrSection;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Experimental, 
                    },
                    Permission = ViewHrSection
                },
            };
        }
    }

}