using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting
{
    public class EnrollmentPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.StoreEnrolement;
        public const string Category = "Reporting";

        public static Permission BasicAcsReportsFeature = new Permission()
        {
            Name = "View basic ACS reports",
            Description = "View the different reports based on data out of ACS",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission BasicRamesesReportsFeature = new Permission()
        {
            Name = "View basic Rameses reports",
            Description = "View the different basic Rameses reports",
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
            yield return BasicAcsReportsFeature;
            yield return BasicRamesesReportsFeature;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] 
            {
                new PermissionStereotype(){
                    Permission = BasicAcsReportsFeature,
                    AllowRoles = new [] {
                        ExpectedStoreEnrollments.FullEnrollment,
                        ExpectedStoreEnrollments.GprsStore,
                        ExpectedStoreEnrollments.AcsReports
                        //ExpectedStoreEnrollments.RamesesStoreEnrollment
                    }
                },

                new PermissionStereotype(){
                    Permission = BasicRamesesReportsFeature,
                    AllowRoles = new [] {
                        ExpectedStoreEnrollments.FullEnrollment,
                        ExpectedStoreEnrollments.RamesesAmsReports
                    }
                }
            };
        }
    }
}