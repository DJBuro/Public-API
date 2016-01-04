using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Andromeda.WebOrdering.Services;
using Newtonsoft.Json;
//using Andromeda.WebOrdering.SPG;

namespace Andromeda.WebOrdering.Tests
{
    public class TestStaticPageGenerator : IStaticPageGenerator
    {
        public Model.Site[] GetSiteList(string domainName)
        {
            return new Model.Site[]
            {
                new Model.Site() { name="Test" }
            };
        }

        public Model.StoreMenu GetSiteMenu(string domainName, WebSiteServicesData webSiteServicesData, string siteId)
        {
            return new Model.StoreMenu() 
            {
                MenuData = new Model.RawMenu() 
                {
                    Display = new System.Collections.Generic.List<Model.RawCategory>()
                    {
                        new Model.RawCategory() { Name="Pizzas" },
                        new Model.RawCategory() { Name="Puddings" }
                    }
                } 
            };
        }
    }

    [TestClass]
    public class StaticPageGenerationTests
    {
        [TestMethod]
        public void StaticPageGeneration()
        {
            // Website configuration (copied from the AndroAdmin database)
            string websiteConfigurationData = "{\"WebSiteId\":38,\"WebSiteName\":\"Robs Pizza\",\"ACSApplicationId\":\"c7ed7837-5ee5-4774-8ffe-d39f48bfdf39\",\"MasterStoreId\":\"CEDD9916-B9D8-4FA1-AF08-9823D9C88F3A\",\"LiveDomainName\":\"rob1.androtest.co.uk\",\"PreviewDomainName\":\"preview.rob1.androtest.co.uk\",\"OldLiveDomainName\":null,\"OldPreviewDomainName\":null,\"IsWebSiteEnabled\":true,\"ChainId\":5,\"SubScriptionTypeId\":0,\"DisabledReason\":null,\"Home\":{\"Welcome\":{\"TitleText\":\"Rob QA\",\"DescriptionText\":\"<p><span style=\\\"font-size:medium;font-family:Georgia, serif;\\\">Welcome to Mexican Fiesta,&nbsp;Mexican&nbsp;cuisine takeaway and delivery in Morden.</span></p><p><span style=\\\"font-family:Georgia, serif;font-size:medium;\\\">We also deliver to the areas; Raynes Park, Mitcham, Rosehill, Wimbledon.</span></p><p><span style=\\\"font-family:Georgia, serif;font-size:medium;\\\">At Mexican Fiesta we provide our customers with great tasting authentic Mexican food. So you can have a food fiesta before your siesta!</span></p><p><span style=\\\"font-family:Georgia, serif;font-size:medium;\\\">Don't forget to follow us on&nbsp;<a href=\\\"https://www.facebook.com/MexicanFiestaSM\\\" target=\\\"_blank\\\">Facebook</a>&nbsp;and&nbsp;<a href=\\\"https://twitter.com/MexicanFiestaSM\\\" target=\\\"_blank\\\">Twitter</a>&nbsp;for all the deals and offers!</span></p>\"},\"Menu\":{\"TitleText\":\"Rob QA\",\"DescriptionText\":\"<span style=\\\"font-family:Georgia, serif;font-size:medium;\\\">Take a look at our menu, we have all the Mexican food&nbsp;favourites,&nbsp;including; Burritos, Quesadillas, Tacos, Nachos, Enchilados, Tosada Salads, Chicken Taquitos, Mexican Sides, Italian Pizza and Desserts!</span>\"},\"Carousels\":[{\"Name\":\"Featured\",\"CarouselContainer\":{\"AutoPlay\":false,\"DisplayPieces\":0,\"AutoPlaySteps\":0,\"SlideWidth\":0,\"SlideDuration\":0},\"CarouselNavigator\":{\"AutoCenter\":0,\"ChanceToShow\":0,\"Steps\":0,\"Class\":null},\"Items\":[{\"Id\":\"25f997b6-fea6-4f91-aaee-cac947643cc2\",\"IsOverlayText\":false,\"IsOverrideDefaultMenuImage\":false,\"HTML\":\"\",\"ImageUrl\":\"http://cdn.myandromedaweb.co.uk/test1/websites/38/carousels/featured/25f997b6-fea6-4f91-aaee-cac947643cc2_1022x300.png?k=71176\",\"Description\":\"\",\"Type\":\"image\"},{\"Id\":\"1590e8c3-5bb8-4f91-f7f9-e7851d5e9216\",\"IsOverlayText\":false,\"IsOverrideDefaultMenuImage\":false,\"HTML\":\"\",\"ImageUrl\":\"http://cdn.myandromedaweb.co.uk/test1/websites/38/carousels/featured/1590e8c3-5bb8-4f91-f7f9-e7851d5e9216_1022x300.png?k=79623\",\"Description\":\"\",\"Type\":\"image\"},{\"Id\":\"4520835a-1fb0-4e1c-5c81-438adabf4aa6\",\"IsOverlayText\":false,\"IsOverrideDefaultMenuImage\":false,\"HTML\":\"\",\"ImageUrl\":\"http://cdn.myandromedaweb.co.uk/test1/websites/38/carousels/featured/4520835a-1fb0-4e1c-5c81-438adabf4aa6_1022x300.png?k=65738\",\"Description\":\"\",\"Type\":\"image\"}]}]},\"LegalNotices\":{\"TermsOfUse\":null,\"PrivacyPolicy\":null,\"CookiePolicy\":null},\"SiteDetails\":{\"DomainName\":\"rob1.androtest.co.uk\",\"OldDomainName\":\"\",\"ParentWebSiteName\":null,\"RedirectToParentAfterCheckout\":false,\"RedirectToParentOnLogoClick\":false,\"WebSiteLogo\":null,\"MobileLogo\":null,\"WebsiteLogoPath\":\"http://cdn.myandromedaweb.co.uk/test1/websites/38/sitedetails/websitelogo/websitelogo272x130.png\",\"MobileLogoPath\":null,\"FaviconPath\":\"http://cdn.myandromedaweb.co.uk/test1/websites/38/sitedetails/favicon/favicon.ico\"},\"GeneralSettings\":{\"MinimumDeliveryAmount\":0.0,\"DeliveryCharge\":300.0,\"OptionalFreeDeliveryThreshold\":null,\"CardCharge\":null,\"ApplyDeliveryCharges\":false,\"IsList\":false,\"IsEnterPostCode\":false,\"EnableStoreLocatorPage\":false,\"EnableHomePage\":false,\"EnableDelivery\":true,\"EnableCollection\":true,\"EnableDineIn\":true},\"ThemeSettings\":{\"Id\":14,\"ThemePath\":\"http://cdn.myandromedaweb.co.uk/dev/themes/theme_2_600x600.png\",\"Height\":600,\"Width\":600,\"ThemeName\":\"Olive Green 2.11\",\"InternalName\":null,\"Description\":\"Olive Green 2.11\",\"FileName\":null},\"HomePageSettings\":{\"EnableStoreDetails\":false,\"ShowMinimumDelivery\":false,\"ShowDeliveryCharge\":false,\"ShowETD\":false,\"PostcodeLocator\":false},\"SocialNetworkSettings\":{\"FacebookSettings\":{\"IsEnable\":true,\"IsShare\":false,\"IsFollow\":true,\"FollowURL\":\"https://www.facebook.com/OlivePizzaKT6\",\"LikeURL\":null,\"EnableFacebookLike\":true,\"EnableFacebookActivityFeeds\":false},\"TwitterSettings\":{\"IsEnable\":false,\"IsShare\":false,\"IsFollow\":false,\"FollowURL\":null,\"LikeURL\":null,\"EnableFacebookLike\":false,\"EnableFacebookActivityFeeds\":false},\"InstagramSettings\":{\"IsEnable\":false,\"IsShare\":false,\"IsFollow\":false,\"FollowURL\":null,\"LikeURL\":null,\"EnableFacebookLike\":false,\"EnableFacebookActivityFeeds\":false},\"PinterestSettings\":{\"IsEnable\":false,\"IsShare\":false,\"IsFollow\":false,\"FollowURL\":null,\"LikeURL\":null,\"EnableFacebookLike\":false,\"EnableFacebookActivityFeeds\":false}},\"CustomerAccountSettings\":{\"IsEnable\":true,\"EnableAndromedaLogin\":true,\"EnableFacebookLogin\":false,\"FacebookAccountId\":null},\"MenuPageSettings\":{\"IsThumbnailsEnabled\":true,\"IsQuantityDropdownEnabled\":true,\"IsSingleToppingsOnlyEnabled\":false,\"AreToppingsSwapsEnabled\":true},\"CheckoutSettings\":{\"IsEnableCustomerDetailsCheckoutPage\":false,\"IsEnableAddressDetailsCheckoutPage\":true,\"IsEnableFutureTimeOrders\":true},\"UpSellingSettings\":{\"IsEnable\":false,\"Soup\":null,\"Sushi_Boxes\":null,\"Sharing_Boxes\":null,\"Side_Dishes\":null,\"Desserts\":null,\"Snacks\":null,\"Drinks\":null},\"LastUpdatedUtc\":\"2015-08-20T14:20:40.6674905Z\",\"AnalyticsSettings\":{\"AnalyticsScript\":null,\"GoogleAnalyticsId\":null},\"CustomThemeSettings\":{\"DesktopBackgroundImagePath\":\"http://cdn.myandromedaweb.co.uk/test1/websites/38/customthemesettings/desktopbackgroundimage/desktopbackgroundimage.png\",\"MobileBackgroundImagePath\":\"\",\"ColourRange1\":\"#efe33c\",\"ColourRange2\":\"#000000\",\"ColourRange3\":\"#f4a13d\",\"ColourRange4\":\"#000000\",\"ColourRange5\":\"#efe33c\",\"ColourRange6\":\"#f4a02a\",\"IsPageHeaderVisible\":true},\"CustomEmailTemplate\":{\"HeaderColour\":\"#EEEEEE\",\"FooterColour\":\"#EEEEEE\",\"CustomTemplates\":{\"1423\":[{\"Title\":\"Custom Content\",\"Content\":\"\",\"Enabled\":false}],\"1457\":[{\"Title\":\"Custom Content\",\"Content\":\"\",\"Enabled\":false}],\"1459\":[{\"Title\":\"Custom Content\",\"Content\":\"\",\"Enabled\":false}],\"1470\":[{\"Title\":\"Custom Content\",\"Content\":\"\",\"Enabled\":false}],\"1471\":[{\"Title\":\"Custom Content\",\"Content\":\"\",\"Enabled\":false}],\"1472\":[{\"Title\":\"Custom Content\",\"Content\":\"\",\"Enabled\":false}],\"1474\":[{\"Title\":\"Custom Content\",\"Content\":\"\",\"Enabled\":false}],\"1490\":[{\"Title\":\"Custom Content\",\"Content\":\"\",\"Enabled\":false}]}},\"FacebookCrawlerSettings\":{\"Title\":\"\",\"SiteName\":\"\",\"Description\":\"\",\"FacebookProfileLogoPath\":\"\"},\"TripAdvisorSettings\":{\"IsEnable\":false,\"Script\":null},\"SEOSettings\":{\"Title\":\"\",\"Keywords\":\"\",\"Description\":\"\",\"IsEnableDescription\":false},\"JivoChatSettings\":{\"IsJivoChatEnabled\":false,\"Script\":\"\"},\"Pages\":[]}";
            Model.WebSiteConfiguration webSiteConfiguration = JsonConvert.DeserializeObject<Model.WebSiteConfiguration>(websiteConfigurationData);

            // Load host mappings
            Global.GetMappings();

            // The website confuguration to use
            WebSiteServicesData webSiteServicesData = new WebSiteServicesData();
            webSiteServicesData.WebSiteConfiguration = webSiteConfiguration;
            webSiteServicesData.WebSiteConfiguration.SiteDetails = new Model.SiteDetails();
            webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName = "rob1.androtest.co.uk";

        //    StaticPageGenerator.GenerateStoreDirectory(webSiteServicesData, new TestStaticPageGenerator());
            StaticPageGenerator.GenerateStoreSnapshot(webSiteServicesData, (IStaticPageGenerator)(new StaticPageGenerator()));
        }
    }
}
