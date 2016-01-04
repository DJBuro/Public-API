using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Web.Areas.AndroWebOrdering
{
    public class UserPermissions : IPermissionProvider
    {
        public const PermissionType ProviderPermissionType = PermissionType.UserRole;
        public const string Category = "Websites";

        public Core.Authorization.PermissionType PermissionType
        {
            get
            {
                return ProviderPermissionType;
            }
        }

        public static Permission ViewWebsites = new Permission()
        {
            Name = "View websites",
            Description = "Can view the websites set up for the store.",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission DeleteWebsite = new Permission()
        {
            Name = "Delete websites",
            Description = "Can delete a website from 'My Websites'",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission PublishWebsites = new Permission()
        {
            Name = "Publish websites",
            Description = "User can publish the website.",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewSiteDetails = new Permission()
        {
            Name = "View website details",
            Description = "Can view the site details tab",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewCmsSection = new Permission()
        {
            Name = "View CMS section",
            Description = "View the custom pages for androweb.",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewEmailCmsSection = new Permission()
        {
            Name = "View Email CMS section",
            Description = "View the Email section and edit content.",
            Category = Category,
            PermissionType = ProviderPermissionType
        };


        public static Permission EditSiteDetails = new Permission()
        {
            Name = "View website details",
            Description = "Can view the site details tab",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewHomePage = new Permission()
        {
            Name = "View homepage settings",
            Description = "Can view the homepage tab and carousel",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditHomePage = new Permission()
        {
            Name = "Edit homepage settings",
            Description = "Can edit the homepage tab and carousel",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewGeneralSettings = new Permission()
        {
            Name = "Edit general settings",
            Description = "...",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditGeneralSettings = new Permission()
        {
            Name = "Edit general settings",
            Description = "...",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewSocialNetworks = new Permission()
        {
            Name = "view social network settings",
            Description = "View social network settings and crawler settings",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditSocialNetworks = new Permission()
        {
            Name = "Edit social network settings",
            Description = "Edit social network settings and crawler settings",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewThemes = new Permission()
        {
            Name = "View themes",
            Description = "...",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditThemes = new Permission()
        {
            Name = "Edit themes",
            Description = "...",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewLegalNotices = new Permission()
        {
            Name = "View legal notices",
            Description = "Terms of Use; Privacy Policy; Cookie Policy",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditLegalNotices = new Permission()
        {
            Name = "View legal notices",
            Description = "Terms of Use; Privacy Policy; Cookie Policy",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewAnalytics = new Permission()
        {
            Name = "View Analytics",
            Description = "View the analytics code",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditAnalytics = new Permission()
        {
            Name = "Edit Analytics",
            Description = "Edit the analytics code",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission ViewSeo = new Permission()
        {
            Name = "View SEO",
            Description = "View the Search Engine Optimization fields",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditSeo = new Permission()
        {
            Name = "Edit SEO",
            Description = "Edit the Search Engine Optimization fields",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public static Permission EditUpSelling = new Permission()
        {
            Name = "Edit Up-selling",
            Description = "Choose a menu section to provide up selling for",
            Category = Category,
            PermissionType = ProviderPermissionType
        };

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ViewWebsites;
            yield return DeleteWebsite;

            yield return PublishWebsites;
            yield return ViewHomePage;
            yield return EditHomePage;
            yield return ViewSiteDetails;
            yield return EditSiteDetails;
            yield return ViewHomePage;
            yield return EditHomePage;
            yield return ViewGeneralSettings;
            yield return EditGeneralSettings;
            yield return ViewSocialNetworks;
            yield return EditSocialNetworks;
            yield return ViewThemes;
            yield return EditThemes;
            yield return ViewLegalNotices;
            yield return EditLegalNotices;
            yield return ViewAnalytics;
            yield return EditAnalytics;
            yield return ViewSeo;
            yield return EditSeo;

            yield return ViewCmsSection;
            yield return ViewEmailCmsSection;

            yield return EditUpSelling;
        }

        public IEnumerable<PermissionStereotype> GetPermissionStereotypes()
        {
            return new PermissionStereotype[] { 
                new PermissionStereotype()
                {
                    Permission = ViewWebsites,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = DeleteWebsite,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator 
                    }
                },
                new PermissionStereotype()
                {
                    Permission = PublishWebsites,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },

                new PermissionStereotype()
                {
                    Permission = ViewHomePage,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },

                new PermissionStereotype()
                {
                    Permission = EditHomePage,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },

                new PermissionStereotype()
                {
                    Permission = ViewSiteDetails,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },

                new PermissionStereotype()
                {
                    Permission = EditSiteDetails,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewHomePage,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditHomePage,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewGeneralSettings,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditGeneralSettings,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewSocialNetworks,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditSocialNetworks,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewThemes,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditThemes,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewLegalNotices,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditLegalNotices,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewAnalytics,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditAnalytics,
                    AllowRoles = new [] { 
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewSeo,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = EditSeo,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                }, 
                new PermissionStereotype()
                {
                    Permission = ViewCmsSection,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                },
                new PermissionStereotype()
                {
                    Permission = ViewEmailCmsSection,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Experimental
                    }
                },
                new PermissionStereotype(){
                    Permission = EditUpSelling,
                    AllowRoles = new [] {
                        ExpectedUserRoles.Administrator, 
                        ExpectedUserRoles.SuperAdministrator, 
                        ExpectedUserRoles.StoreAdministrator,
                        ExpectedUserRoles.ChainAdministrator   
                    }
                }
            };
        }
    }
}