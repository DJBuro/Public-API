using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Authorization
{
    public class SiteEnrollmentUserPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Authorization";

        //public static Permission EditSiteEnrollmentRoleDefinitions = new Permission()
        //{
        //    Name = "Change role definitions",
        //    Category = Category,
        //    Description = "Add, change or edit roles and enrollment types and associated permissions.",
        //    PermissionType = ProviderPermissionType
        //};

        public static Permission ViewSiteEnrollment = new Permission()
        {
            Name = "View site enrollment level",
            Description = "View the site enrollment level",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditSiteEnrollment = new Permission() 
        {
            Name = "Change Site Enrollment Level",
            Description = "Change Site Enrollment Level",
            Category = Category,
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
            //yield return EditSiteEnrollmentRoleDefinitions;

            yield return ViewSiteEnrollment;
            yield return EditSiteEnrollment;
        
        
            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new[] {
                new PermissionStereotype(){
                    Permission = ViewSiteEnrollment,
                    AllowRoles = new [] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator }
                },
                new PermissionStereotype(){
                    Permission = EditSiteEnrollment,
                    AllowRoles = new [] { ExpectedUserRoles.SuperAdministrator, ExpectedUserRoles.Administrator }
                }
            };

        }
    }
}