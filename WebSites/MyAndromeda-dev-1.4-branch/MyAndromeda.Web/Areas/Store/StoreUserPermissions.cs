using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Store
{

    public class StoreUserPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Store";

        public static Permission ViewStoreDetails = new Permission()
        {
            Name = "View Store Details",
            Description = "The user can visit the store details page",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditStoreDetails = new Permission()
        {
            Name = "Edit Store Details",
            Description = "The user can edit the store details page",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewStoreAddress = new Permission()
        {
            Name = "View Store address",
            Description = "The user can view the store address on the page",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditStoreAddress = new Permission()
        {
            Name = "Edit Store address",
            Description = "The user can edit the store address on the page",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewOpeningTimes = new Permission()
        {
            Name = "View opening times",
            Description = "Can the user create/edit and times that the store is open",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditOpeningTimes = new Permission() 
        {
            Name = "Edit opening times", 
            Description = "Can the user create/edit and times that the store is open",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewEdtTime = new Permission()
        {
            Name = "View ETD",
            Description = "View the estimated delivery time",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditEdtTime = new Permission()
        {
            Name = "Edit ETD",
            Description = "Edit the estimated delivery time",
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
            yield return ViewStoreDetails;
            yield return EditStoreDetails;
            yield return ViewStoreAddress;
            yield return EditStoreDetails;
            yield return ViewOpeningTimes;
            yield return EditOpeningTimes;

            yield break;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype {
                    Permission = ViewStoreDetails,
                    AllowRoles = new [] 
                    {
                        ExpectedUserRoles.User
                    }
                },
                new PermissionStereotype {
                    Permission = EditStoreDetails,
                    AllowRoles = new [] 
                    {
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                },

                new PermissionStereotype {
                    Permission = ViewStoreAddress, 
                    AllowRoles = new [] 
                    {
                        ExpectedUserRoles.User
                    }
                },

                new PermissionStereotype {
                    Permission = EditStoreAddress, 
                    AllowRoles = new [] 
                    {
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                },

                new PermissionStereotype {
                    Permission = ViewOpeningTimes,
                    AllowRoles = new [] 
                    {
                        ExpectedUserRoles.User
                    }
                },

                new PermissionStereotype {
                    Permission = EditOpeningTimes,
                    AllowRoles = new [] 
                    {
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                },
                
                new PermissionStereotype {
                    Permission = ViewEdtTime,
                    AllowRoles = new [] {
                        ExpectedUserRoles.User
                    }
                },

                new PermissionStereotype {
                    Permission = EditEdtTime,
                    AllowRoles = new [] {
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                }
            };
        }
    }
}