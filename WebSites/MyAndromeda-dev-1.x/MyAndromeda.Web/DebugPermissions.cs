using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web
{

    public class DebugPermissions : IPermissionProvider
    {
        public const PermissionType UserPermissionType = PermissionType.UserRole;

        public const string Category = "Debug";
        public static Permission ViewTestSection = new Permission()
        {
            Name = "View test forms",
            Description = "View the test forms in the menu",
            Category = Category,
            PermissionType = UserPermissionType
        };

        public static Permission ViewDebugMode = new Permission()
        {
            Name = "View Debug",
            Description = "View the debug pane",
            Category = Category,
            PermissionType = UserPermissionType
        };

        public static Permission ViewProfilingMode = new Permission()
        {
            Name = "View profiling data",
            Description = "View profiling information for requests",
            Category = Category,
            PermissionType = UserPermissionType
        };

        public static Permission TestApiSection = new Permission()
        {
            Name = "View Test Api forms",
            Description = "Can use the api forms for : 1. testing success / failure email; 2. order monitoring service",
            Category = Category,
            PermissionType = UserPermissionType
        };

        public static Permission ViewApiHelpSection = new Permission()
        {
            Name = "View private API help",
            Description = "Describes how the api functions",
            Category = Category,
            PermissionType = UserPermissionType
        };

        public static Permission ExportCustomers = new Permission()
        {
            Name = "Export all customers",
            Description = "",
            Category = Category,
            PermissionType = UserPermissionType
        };

        public static Permission Orders = new Permission()
        {
            Name = "View Orders",
            Description = "",
            Category = Category,
            PermissionType = UserPermissionType
        };

        public PermissionType PermissionType
        {
            get
            {
                return UserPermissionType;
            }
        }

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ViewTestSection;
            yield return ViewDebugMode;
            yield return ViewProfilingMode;
            yield return TestApiSection;
            yield return ViewApiHelpSection;
            yield return Orders;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] 
            {
                new PermissionStereotype()
                {
                    Permission = ViewTestSection,
                    AllowRoles = new []{ 
                        ExpectedUserRoles.SuperAdministrator, 
                        //ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.DebugUser 
                    }
                    
                },
                new PermissionStereotype()
                {
                    Permission = ViewDebugMode,
                    AllowRoles = new []{ 
                        ExpectedUserRoles.SuperAdministrator, 
                        //ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.DebugUser 
                    }
                    
                },
                new PermissionStereotype()
                {
                    Permission = ViewProfilingMode,
                    AllowRoles = new []{ 
                        ExpectedUserRoles.SuperAdministrator, 
                        //ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.DebugUser 
                    }
                },
                new PermissionStereotype()
                {
                    Permission = TestApiSection,
                    AllowRoles = new []{ 
                        ExpectedUserRoles.SuperAdministrator, 
                        //ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.DebugUser 
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewApiHelpSection,
                    AllowRoles = new []{ 
                        ExpectedUserRoles.SuperAdministrator, 
                        //ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.DebugUser 
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ExportCustomers,
                    AllowRoles = new []{
                        ExpectedUserRoles.Experimental
                    }
                },
                new PermissionStereotype()
                {
                    Permission = Orders,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Experimental
                    }
                }
            };
        }
    }
}