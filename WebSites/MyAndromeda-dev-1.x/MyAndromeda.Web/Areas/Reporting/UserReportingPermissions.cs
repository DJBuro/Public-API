using System;
using System.Collections.Generic;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.Reporting
{
    public class UserReportingPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Reporting";

        public PermissionType PermissionType
        {
            get
            {
                return ProviderPermissionType;
            }
        }

        //public static Permission ViewReports = new Permission()
        //{
        //    Name = "View any of the reports",
        //    Description = "View the different reports",
        //    Category = Category,
        //    PermissionType = PermissionType.UserRole
        //};

        public static Permission ViewChainReports = new Permission()
        {
            Name = "View Chain reports",
            Description = "View the chain reports",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewRamesesReports = new Permission()
        {
            Name = "View Rameses reports",
            Description = "View the Rameses reports",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewGprsReports = new Permission()
        {
            Name = "View GPRS reports",
            Description = "View the Rameses reports",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };


        public static Permission ViewAllOrdersReport = new Permission()
        {
            Name = "View All Orders - Rameses",
            Description = "View Rameses style report from",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewAllAcsOrdersReport = new Permission()
        {
            Name = "View on-line orders - ACS",
            Description = "View ACS orders",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewPerformanceReport = new Permission()
        {
            Name = "View performance report - Rameses",
            Description = "View performance metrics for a store",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewDeliveryPerformanceMetrics = new Permission()
        {
            Name = "View delivery performance report - Rameses",
            Description = "View the delivery metrics",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewProductReport = new Permission()
        {
            Name = "View product report - ACS",
            Description = "View which products are selling.",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewCustomerReports = new Permission()
        {
            Name = "View customer data/report - ACS",
            Description = "Address, Marketing details, Order History...",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewCustomerLoyaltyReport = new Permission()
        {
            Name = "View customer loyalty report",
            Description = "Address, Marketing details, Order History...",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewVoucherReport = new Permission()
        {
            Name = "View voucher usage report - ACS",
            Description = "Address, Marketing details, Order History...",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewMap = new Permission()
        {
            Name = "View map",
            Description = "GPRS location of orders between 2 dates",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public static Permission ViewBetaReports = new Permission()
        {
            Name = "View beta reports",
            Description = "Current beta reports",
            Category = Category,
            PermissionType = PermissionType.UserRole
        };

        public IEnumerable<Permission> GetPermissions()
        {
            //yield return ViewReports;
            yield return ViewChainReports;
            yield return ViewRamesesReports;
            yield return ViewGprsReports;

            yield return ViewAllOrdersReport;
            yield return ViewAllAcsOrdersReport;
            yield return ViewCustomerReports;
            yield return ViewDeliveryPerformanceMetrics;
            yield return ViewPerformanceReport;
            yield return ViewProductReport;
            yield return ViewVoucherReport;
            yield return ViewCustomerLoyaltyReport;
            yield return ViewBetaReports;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] 
            {
                new PermissionStereotype() 
                {
                    Permission = ViewChainReports,
                    AllowRoles = new [] {
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.Administrator,
                        ExpectedUserRoles.SuperAdministrator,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewRamesesReports,
                    AllowRoles = new [] {              
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewGprsReports,
                    AllowRoles = new [] {
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewAllAcsOrdersReport,
                    AllowRoles = new []{
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewAllOrdersReport,
                    AllowRoles = new []{
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewCustomerReports,
                    AllowRoles = new []{
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewDeliveryPerformanceMetrics,
                    AllowRoles = new []{
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewMap,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Experimental
                    }
                },
                new PermissionStereotype() {
                    Permission = ViewBetaReports,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Experimental
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewPerformanceReport,
                    AllowRoles = new [] {
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewProductReport,
                    AllowRoles = new [] {
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewProductReport,
                    AllowRoles = new [] {
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewVoucherReport,
                    AllowRoles = new []{
                        ExpectedUserRoles.User,
                        ExpectedUserRoles.ReportingUser
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewCustomerLoyaltyReport,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Experimental
                    }
                }
            };
        }
    }
}