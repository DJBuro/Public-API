using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting
{
    public class Permissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Reporting";

        public static Permission ViewReports = new Permission()
        {
            Name = "View Report Feature",
            Description = "View the different reports",
            Category = Category,
            PermissionType = PermissionType.StoreEnrolement
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
            yield return ViewReports;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { };
            //return new[] {
            //    new PermissionStereotype() { AllowRoles = new [] { "Administrator" }, Permission = ViewReports } 
            //};
        }
    }
}