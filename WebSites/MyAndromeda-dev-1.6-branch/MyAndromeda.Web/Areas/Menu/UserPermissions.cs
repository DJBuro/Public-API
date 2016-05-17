using System.Collections.Generic;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Web.Areas.Menu
{
    public class UserPermissions : IPermissionProvider 
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Menu";

        public static Permission ViewStoreMenu = new Permission()
        {
            Name = "View online store menu",
            Description = "View the menu section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditStoreMenu = new Permission()
        {
            Name = "Edit online store Menu",
            Description = "Allow the user to edit the menu",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewToppings = new Permission()
        {
            Name = "View toppings",
            Description = "(Experimental) View the toppings section",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditToppings = new Permission()
        {
            Name = "Edit toppings",
            Description = "(Experimental) Edit the toppings",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission PublishStoreMenu = new Permission()
        {
            Name = "Publish Menu feature",
            Description = "Allow the publishing of the menu",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewFtpToolbar = new Permission()
        {
            Name = "View FTP toolbar",
            Description = "The toolbar that allows ftp fetching and backup.",
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
            yield return ViewStoreMenu;
            yield return EditStoreMenu;
            yield return ViewToppings;
            yield return EditToppings;
            yield return PublishStoreMenu;
            yield return ViewFtpToolbar;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    Permission = ViewStoreMenu,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator,
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditStoreMenu,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator,
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                },
                new PermissionStereotype()
                {
                    Permission = PublishStoreMenu,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator,
                        ExpectedUserRoles.ChainAdministrator,
                        ExpectedUserRoles.StoreAdministrator
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewFtpToolbar,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewToppings,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Experimental, 
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditToppings,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Experimental, 
                    }
                }
            };
        }
    }
}