using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web
{
    public class Permissions : IPermissionProvider
    {
        public const PermissionType UserPermissionType = PermissionType.UserRole;

        public const string Category = "Debug";
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

        public PermissionType PermissionType
        {
            get
            {
                return UserPermissionType;
            }
        }

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ViewDebugMode;
            yield return ViewProfilingMode;
            yield return TestApiSection;
            yield return ViewApiHelpSection;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] 
            {
                new PermissionStereotype()
                {
                    AllowRoles = new []{ "Debug", ExpectedRoles.SuperAdministrator },
                    Permission = ViewDebugMode
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { "Debug", ExpectedRoles.SuperAdministrator },
                    Permission = ViewProfilingMode
                },
                new PermissionStereotype()
                {
                    AllowRoles = new [] { "Debug", ExpectedRoles.SuperAdministrator },
                    Permission = TestApiSection
                },
                new PermissionStereotype()
                {
                    AllowRoles= new [] { "Debug", ExpectedRoles.SuperAdministrator },
                    Permission = ViewApiHelpSection
                }
            };
        }
    }
}