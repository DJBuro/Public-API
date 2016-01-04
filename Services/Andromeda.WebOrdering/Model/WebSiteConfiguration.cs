using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class WebSiteConfiguration
    {
        public int? WebSiteId { get; set;}
        public string WebSiteName { get; set;}
        public string ACSApplicationId { get; set; }
        public string MasterStoreId { get; set; }
        public Home Home { get; set; }
        public LegalNotices LegalNotices { get; set; }
        public SiteDetails SiteDetails { get; set; }
        public GeneralSettings GeneralSettings { get; set; }
        public ThemeSettings ThemeSettings { get; set; }
        public HomePageSettings HomePageSettings { get; set; }
        public SocialNetworkSettings SocialNetworkSettings { get; set; }
        public CustomerAccountSettings CustomerAccountSettings { get; set; }
        public MenuPageSettings MenuPageSettings { get; set; }
        public CheckoutSettings CheckoutSettings { get; set; }
        public AnalyticsSettings AnalyticsSettings { get; set; }
        public CustomThemeSettings CustomThemeSettings { get; set; }
        public FacebookCrawlerSettings FacebookCrawlerSettings { get; set; }
        public TripAdvisorSettings TripAdvisorSettings { get; set; }
        public SEOSettings SEOSettings { get; set; }
        public JivoChatSettings JivoChatSettings { get; set; }
        public CmsPages Pages { get; set; }
        public Upselling Upselling { get; set; }
    }

    public class Home
    {
        public HomePageText Welcome { get; set; }
        public HomePageText Menu { get; set; }
        public List<Carousel> Carousels { get; set; }
    }

    public class HomePageText
    {
        public string TitleText { get; set; }
        public string DescriptionText { get; set; }
    }

    public class Carousel
    {
        public List<Slide> Items { get; set; }
    }

    public class Slide
    {
        public string ItemId { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsOverlayText { get; set; }
        public bool? IsOverrideDefaultMenuImage { get; set; }
        public string HTML { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
    public class LegalNotices
    {
        public string TermsOfUse { get; set; }
        public string PrivacyPolicy { get; set; }
        public string CookiePolicy { get; set; }
    }
    public class SiteDetails
    {
        public string DomainName { get; set; }
        public string OldDomainName { get; set; }
        public string ParentWebSiteName { get; set; }
        public bool? RedirectToParentAfterCheckout { get; set; }
        public bool? RedirectToParentOnLogoClick { get; set; }
        public string WebsiteLogoPath { get; set; }
        public string MobileLogoPath { get; set; }
        public string FaviconPath { get; set; }
    }
    public class GeneralSettings
    {
        public decimal? MinimumDeliveryAmount { get; set; }
        public decimal? OptionalFreeDeliveryThreshold { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public decimal? CardCharge { get; set; }
        public bool? EnableStoreLocatorPage { get; set; }
        public bool? IsList { get; set; }
        public bool? IsEnterPostCode { get; set; }
        public bool? EnableDelivery { get; set; }
        public bool? EnableCollection { get; set; }
        public bool? EnableDineIn { get; set; }
    }
    public class ThemeSettings
    {
        public int? Id { get; set; }
        public string ThemeName { get; set; }
        public string Source { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string FileName { get; set; }
    }
    public class HomePageSettings
    {
        public bool? EnableStoreDetails { get; set; }
        public bool? ShowMinimumDelivery { get; set; }
        public bool? ShowDeliveryCharge { get; set; }
        public bool? ShowETD { get; set; }
        public bool? PostcodeLocator { get; set; }
        public string ETD { get; set; }
    }
    public class SocialNetworkSettings
    {
        public FacebookSetting FacebookSettings { get; set; }
        public SocialNetworkSetting TwitterSettings { get; set; }
        public SocialNetworkSetting InstagramSettings { get; set; }
        public SocialNetworkSetting PinterestSettings { get; set; }
    }
    public class FacebookSetting
    {
        public bool IsEnable { get; set; }
        public bool IsShare { get; set; }
        public bool IsFollow { get; set; }
        public string FollowURL { get; set; }
        public string LikeUrl { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
    }
    public class SocialNetworkSetting
    {
        public bool IsEnable { get; set; }
        public bool IsShare { get; set; }
        public bool IsFollow { get; set; }
        public string FollowURL { get; set; }
        public string LikeUrl { get; set; }
    }
    public class CustomerAccountSettings
    {
        public bool? IsEnable { get; set; }
        public bool? EnableAndromedaLogin { get; set; }
        public bool? EnableFacebookLogin { get; set; }
        public string FacebookAccountId { get; set; }
        public bool? IsEnableOrderHistory { get; set; }
        public bool? IsEnableRepeatOrder { get; set; }
    }
    public class MenuPageSettings
    {
        public bool? IsEnableCommentsForChef { get; set; }
        public bool? IsEnableWhosItemIsThisFor { get; set; }
        public bool? IsDisplayAlwaysShowToppingsPopupWhenAddingItemsToTheBasket { get; set; }
        public bool? IsEnableImangesForItems { get; set; }
        public bool? IsDisplayItemQuantityDropDown { get; set; }
        public bool? IsSingleToppingsOnlyEnabled { get; set; }
        public bool? IsDisplayMinimumDeliveryAmountOnMenuPage { get; set; }
        public bool? IsDisplayETDOnMenuPage { get; set; }
        public bool? AreToppingsSwapsEnabled { get; set; }
    }
    public class CheckoutSettings
    {
        public bool? IsEnableCustomerDetailsCheckoutPage { get; set; }
        public bool? IsEnableAddressDetailsCheckoutPage { get; set; }
        public bool? IsEnableFutureTimeOrders { get; set; }
    }

    public class AnalyticsSettings
    {
        public string GoogleAnalyticsId { get; set; }
    }

    public class CustomThemeSettings
    {
        public string DesktopBackgroundImagePath { get; set; }
        public string MobileBackgroundImagePath { get; set; }
        public string ColourRange1 { get; set; }
        public string ColourRange2 { get; set; }
        public string ColourRange3 { get; set; }
        public string ColourRange4 { get; set; }
        public string ColourRange5 { get; set; }
        public string ColourRange6 { get; set; }
        public bool? IsPageHeaderVisible { get; set; }
    }

    public class FacebookCrawlerSettings
    {
        public string Title { get; set; }
        public string SiteName { get; set; }
        public string Description { get; set; }
        public string FacebookProfileLogoPath { get; set; }
    }

    public class TripAdvisorSettings
    {
        public bool? IsEnable { get; set; }
        public string Script { get; set; }
    }

    public class SEOSettings
    {
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public bool IsEnableDescription { get; set; }
    }

    public class JivoChatSettings
    {
        public bool? IsJivoChatEnabled { get; set; }
        public string Script { get; set; }
    }

    public class Upselling
    {
        public bool? Enabled { get; set; }
        public List<DisplayCategory> DisplayCategories { get; set; }
    }

    public class DisplayCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}