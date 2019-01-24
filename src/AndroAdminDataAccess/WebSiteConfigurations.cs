﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AndroAdminDataAccess.Domain.WebOrderingSetup
{
    /// <summary>
    /// TBD: Class to be moved to another layer and method access-specifiers to be modified.
    /// TBD: JSON serialized-object format to+ be freezed.
    /// </summary>
    public class WebSiteConfigurations
    {
        public WebSiteConfigurations()
        {
            this.Home = new AndroAdminDataAccess.Domain.WebOrderingSetup.WebsiteHome();
            this.Home.DefaultWebSiteHome(); // Don't replace with constructor (deserialize with Json.NET will result in duplicates)

            this.LegalNotices = new LegalNotices();

            this.SiteDetails = new SiteDetails();
            this.ThemeSettings = new ThemeSettings();
            this.GeneralSettings = new GeneralSettings();
            this.HomePageSettings = new HomePageSettings();
            this.SocialNetworkSettings = new SocialNetwork();
            this.SocialNetworkSettings.DefaultSocialNetwork();

            this.CustomerAccountSettings = new CustomerAccount();            

            this.MenuPageSettings = new MenuPageSettings();
            this.MenuPageSettings.DefaultMenuPage();

            this.CheckoutSettings = new CheckoutSettings();
            this.CheckoutSettings.DefaultCheckOutPolicy();

   //         this.UpSellingSettings = new UpSellingSettings();

            this.LastUpdatedUtc = DateTime.UtcNow;
            this.AnalyticsSettings = new Analytics();
            
            this.CustomThemeSettings = new CustomThemeSettings();
            this.CustomThemeSettings.DefaultCustomThemeSettings();

            this.CustomEmailTemplate = new CustomEmailTemplate();
            this.CustomEmailTemplate.DefaultCustomThemeSettings();

            this.FacebookCrawlerSettings = new FacebookCrawlerSettings();
            this.FacebookCrawlerSettings.DefaultFacebookCrawlerSettings();

        }

        public int WebSiteId { get; set; }

        public string WebSiteName { get; set; }

        public string ACSApplicationId { get; set; }

        public string MasterStoreId { get; set; }

        public string LiveDomainName { get; set; }

        public string PreviewDomainName { get; set; }

        public string OldLiveDomainName { get; set; }

        public string OldPreviewDomainName { get; set; }

        public bool IsWebSiteEnabled { get; set; }

        public int? ChainId { get; set; }

        public int SubScriptionTypeId { get; set; }

        public string DisabledReason { get; set; }

        public WebsiteHome Home { get; set; }

        public LegalNotices LegalNotices { get; set; }

        public SiteDetails SiteDetails { get; set; }

        public GeneralSettings GeneralSettings { get; set; }

        public ThemeSettings ThemeSettings { get; set; }

        public HomePageSettings HomePageSettings { get; set; }

        public SocialNetwork SocialNetworkSettings { get; set; }

        public CustomerAccount CustomerAccountSettings { get; set; }

        public MenuPageSettings MenuPageSettings { get; set; }

        public CheckoutSettings CheckoutSettings { get; set; }

  //      public UpSellingSettings UpSellingSettings { get; set; }

        public DateTime LastUpdatedUtc { get; set; }

        public Analytics AnalyticsSettings { set; get; }

        public CustomThemeSettings CustomThemeSettings { set; get; }

        public CustomEmailTemplate CustomEmailTemplate { set; get; }

        public FacebookCrawlerSettings FacebookCrawlerSettings { set; get; }

        public TripAdvisorSettings TripAdvisorSettings { set; get; }

        public SEOSettings SEOSettings { set; get; }

        public JivoChatSettings JivoChatSettings { get; set; }

        public CmsPages Pages
        {
            get;
            set;
        }

        public UpSellingModel UpSelling
        {
            get;
            set;
        }

        public static string SerializeJson(Object obj)
        {
            if (obj == null)
                return string.Empty;

            string result = JsonConvert.SerializeObject(obj);
            return result;
        }

        public static WebSiteConfigurations DeserializeJson(string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
                return new WebSiteConfigurations();

            WebSiteConfigurations result = JsonConvert.DeserializeObject<WebSiteConfigurations>(obj);
            return result;
        }
    }

}
