using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Authorization
{
    public class Permissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Authorization";

        public static Permission ChangeRoles = new Permission()
        {
            Name = "Change role definitions",
            Category = Category,
            Description = "Add, change or edit roles and enrolment types and associated permissions.",
            PermissionType = ProviderPermissionType
        };

        public static Permission ChangeSiteLevel = new Permission() 
        {
            Name = "Change Site Enrolment Level",
            Description = "Change Site Enrolment Level",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ChanageUserLevel = new Permission()
        {
            Name = "Change a users's authorization level",
            Description = "Change a users's authorization level",
            Category = Category
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
            yield return ChangeSiteLevel;
            yield return ChanageUserLevel;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new[] {
                new PermissionStereotype(){
                    Permission = ChangeRoles, 
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype(){
                    Permission = ChangeSiteLevel,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                },
                new PermissionStereotype(){
                    Permission = ChanageUserLevel,
                    AllowRoles = new [] { ExpectedRoles.SuperAdministrator, ExpectedRoles.Administrator }
                }
            };

        }
    }
}