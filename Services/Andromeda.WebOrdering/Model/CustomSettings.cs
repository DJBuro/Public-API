using Andromeda.WebOrdering.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class CustomSettings
    {
        // Headers
        public string COMPANYLOGOURL  { get; set;}
        public string MOBILECOMPANYLOGOURL { get; set; }
        public string REDIRECTTOPARENTONLOGOCLICK { get; set; }

        // Legal stuff
        public string TERMSOFUSE { get; set; }
        public string PRIVACYPOLICY { get; set; }
        public string COOKIEPOLICY { get; set; }

        // Ordering
        public string ETD { get; set;}
        public string MINUMUMDELIVERYORDER { get; set; }
        public string OPTIONALFREEDELIVERYTHRESHOLD { get; set; }
        public string DELIVERYCHARGE { get; set; }
        public string CARDCHARGE { get; set; }

        // Social media
        public string TWITTER { get; set; }
        public string FACEBOOK { get; set; }
        public string INSTAGRAM { get; set; }
        public string PINTEREST { get; set; }
        public string FACEBOOKLIKEURL { get; set; }

        // Home page
        public string HOMESTOREDETAILSTITLE { get; set; }
        public string HOMESTOREDETAILS { get; set; }
        public string HOMEMENUTITLE { get; set; }
        public string HOMEMENUDETAILS { get; set; }
        public string CAROUSELITEMS { get; set; }

        // Customer accounts
        public string CUSTOMERACCOUNTSENABLED { get; set; }
        public string ANDROLOGINENABLED { get; set; }
        public string FACEBOOKLOGINENABLED { get; set; }
        public string FACEBOOKAPPID { get; set; }

        // Menu page
        public string SHOWDOUBLETOPPINGS { get; set; }

        // Occasions
        public string ENABLEDELIVERY { get; set; }
        public string ENABLECOLLECTION { get; set; }
        public string ENABLEDINEIN { get; set; }

        // Toppings
        public string ARETOPPINGSWAPSENABLED { get; set; }

        // Analytics
        public string GOOGLEANALYTICSID { get; set; }

        public string GenerateCustomJS(WebSiteServicesData webSiteServicesData, string customJSTemplate)
        {
            this.ExtractFromJSON(webSiteServicesData);

            return this.ToJavascript(customJSTemplate);
        }

        // Parent website
        public string RETURNTOPARENTWEBSITEAFTERORDER { get; set; }
        public string PARENTWEBSITE { get; set; }

        // Trip advisor
        public bool TRIPADVISORISENABLED { get; set; }

        // Enable/disable header
        public string ISPAGEHEADERVISIBLE { get; set; }

        // Upselling
        public bool UPSELLINGENABLED { get; set; }
        public string UPSELLINGCATEGORIES { get; set; }

        public bool FACEBOOKSHAREENABLED { get; set; }
        public string FACEBOOKSHARECAPTION { get; set; }
        public string FACEBOOKSHARDESCRIPTION { get; set; }

        /// <summary>
        /// Cms Pages 
        /// </summary>
        public string CMSPAGES { get; set; } 

        public void ExtractFromJSON(WebSiteServicesData webSiteServicesData)
        {
            // Header
            this.COMPANYLOGOURL = webSiteServicesData.WebSiteConfiguration.SiteDetails.WebsiteLogoPath == null ? "" : webSiteServicesData.WebSiteConfiguration.SiteDetails.WebsiteLogoPath;
            this.MOBILECOMPANYLOGOURL = webSiteServicesData.WebSiteConfiguration.SiteDetails.MobileLogoPath == null ?
                                        this.COMPANYLOGOURL :
                                        webSiteServicesData.WebSiteConfiguration.SiteDetails.MobileLogoPath;
            this.REDIRECTTOPARENTONLOGOCLICK = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.SiteDetails.RedirectToParentOnLogoClick, false);

            this.TERMSOFUSE = CustomSettings.StripUnsupportedCharacters(webSiteServicesData.WebSiteConfiguration.LegalNotices.TermsOfUse);
            this.PRIVACYPOLICY = CustomSettings.StripUnsupportedCharacters(webSiteServicesData.WebSiteConfiguration.LegalNotices.PrivacyPolicy);
            this.COOKIEPOLICY = CustomSettings.StripUnsupportedCharacters(webSiteServicesData.WebSiteConfiguration.LegalNotices.CookiePolicy);
            
            this.ETD = webSiteServicesData.WebSiteConfiguration.HomePageSettings.ETD == null ? ConfigurationManager.AppSettings["DefaultETD"] : webSiteServicesData.WebSiteConfiguration.HomePageSettings.ETD;
            this.MINUMUMDELIVERYORDER = webSiteServicesData.WebSiteConfiguration.GeneralSettings.MinimumDeliveryAmount.ToString();

            this.OPTIONALFREEDELIVERYTHRESHOLD = webSiteServicesData.WebSiteConfiguration.GeneralSettings.OptionalFreeDeliveryThreshold.HasValue ? webSiteServicesData.WebSiteConfiguration.GeneralSettings.OptionalFreeDeliveryThreshold.ToString() : "undefined";
            this.DELIVERYCHARGE = webSiteServicesData.WebSiteConfiguration.GeneralSettings.DeliveryCharge.HasValue ? ((int)webSiteServicesData.WebSiteConfiguration.GeneralSettings.DeliveryCharge).ToString() : "undefined";
            this.CARDCHARGE = webSiteServicesData.WebSiteConfiguration.GeneralSettings.CardCharge.HasValue ? ((int)webSiteServicesData.WebSiteConfiguration.GeneralSettings.CardCharge).ToString() : "undefined";

            this.TWITTER = CustomSettings.ProcessFollowShareSocialNetworkSetting(webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.TwitterSettings);
            this.FACEBOOK = CustomSettings.ProcessFollowShareSocialNetworkSetting(webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.FacebookSettings);
            this.INSTAGRAM = CustomSettings.ProcessFollowShareSocialNetworkSetting(webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.InstagramSettings);
            this.PINTEREST = CustomSettings.ProcessFollowShareSocialNetworkSetting(webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.PinterestSettings);
            this.FACEBOOKLIKEURL = CustomSettings.ProcessFollowShareSocialNetworkSetting(webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.FacebookSettings);

            this.HOMESTOREDETAILSTITLE = CustomSettings.StripUnsupportedCharacters(webSiteServicesData.WebSiteConfiguration.Home.Welcome.TitleText);
            this.HOMESTOREDETAILS = CustomSettings.StripUnsupportedCharacters(webSiteServicesData.WebSiteConfiguration.Home.Welcome.DescriptionText);
            this.HOMEMENUTITLE = CustomSettings.StripUnsupportedCharacters(webSiteServicesData.WebSiteConfiguration.Home.Menu.TitleText);
            this.HOMEMENUDETAILS = CustomSettings.StripUnsupportedCharacters(webSiteServicesData.WebSiteConfiguration.Home.Menu.DescriptionText);

            // Customer accounts
            this.CUSTOMERACCOUNTSENABLED = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.CustomerAccountSettings.IsEnable, true);
            this.ANDROLOGINENABLED = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.CustomerAccountSettings.EnableAndromedaLogin, true);
            this.FACEBOOKLOGINENABLED = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.CustomerAccountSettings.EnableFacebookLogin, true);
            this.FACEBOOKAPPID = StripUnsupportedCharacters(webSiteServicesData.WebSiteConfiguration.CustomerAccountSettings.FacebookAccountId);
            this.FACEBOOKAPPID = this.FACEBOOKAPPID.Length == 0 ? "0" : this.FACEBOOKAPPID;

            // Occasions
            this.ENABLEDELIVERY = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.GeneralSettings.EnableDelivery, true);
            this.ENABLECOLLECTION = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.GeneralSettings.EnableCollection, true);
            this.ENABLEDINEIN = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.GeneralSettings.EnableDineIn, false);

            this.SHOWDOUBLETOPPINGS = CustomSettings.BoolToString
                (webSiteServicesData.WebSiteConfiguration.MenuPageSettings.IsSingleToppingsOnlyEnabled != null ? 
                !webSiteServicesData.WebSiteConfiguration.MenuPageSettings.IsSingleToppingsOnlyEnabled :
                true, true);

            // Toppings
            this.ARETOPPINGSWAPSENABLED = CustomSettings.BoolToString
                (webSiteServicesData.WebSiteConfiguration.MenuPageSettings.AreToppingsSwapsEnabled, true);

            // Google analytics
            if (webSiteServicesData.WebSiteConfiguration.AnalyticsSettings != null)
            {
                this.GOOGLEANALYTICSID = webSiteServicesData.WebSiteConfiguration.AnalyticsSettings.GoogleAnalyticsId;
            }
            else
            {
                this.GOOGLEANALYTICSID = "";
            }

            // Carousel items
            this.CAROUSELITEMS = "";
            if (webSiteServicesData.WebSiteConfiguration.Home.Carousels != null)
            {
                foreach (Carousel carousel in webSiteServicesData.WebSiteConfiguration.Home.Carousels)
                {
                    foreach (Slide slide in carousel.Items)
                    {
                        if (!String.IsNullOrEmpty(slide.ImageUrl))
                        {
                            this.CAROUSELITEMS += (this.CAROUSELITEMS.Length > 0 ? "," : String.Empty);
                            this.CAROUSELITEMS += "{ type: 'Image', url: '" + slide.ImageUrl + "' }\r\n";
                        }
                    }
                }
            }

            // Parent website
            this.RETURNTOPARENTWEBSITEAFTERORDER = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.SiteDetails.RedirectToParentAfterCheckout, false);
            this.PARENTWEBSITE = String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.SiteDetails.ParentWebSiteName) ? "undefined" : webSiteServicesData.WebSiteConfiguration.SiteDetails.ParentWebSiteName;

            // Trip advisor
            this.TRIPADVISORISENABLED = false;
            if (webSiteServicesData.WebSiteConfiguration.TripAdvisorSettings != null)
            {
                if (webSiteServicesData.WebSiteConfiguration.TripAdvisorSettings.IsEnable.HasValue)
                {
                    if (webSiteServicesData.WebSiteConfiguration.TripAdvisorSettings.IsEnable == true && 
                        !String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.TripAdvisorSettings.Script))
                    {
                        this.TRIPADVISORISENABLED = true;
                    }
                }
            }

            // Cms Pages 
            this.CMSPAGES = JsonConvert.SerializeObject(webSiteServicesData.WebSiteConfiguration.Pages.Where(e=> e.Enabled) ?? new CmsPages());

            // Up selling
            this.UPSELLINGENABLED = false;
            this.UPSELLINGCATEGORIES = "[]";

            if (webSiteServicesData.WebSiteConfiguration.Upselling != null)
            {
                if (webSiteServicesData.WebSiteConfiguration.Upselling.Enabled.HasValue)
                {
                    if (webSiteServicesData.WebSiteConfiguration.Upselling.Enabled == true &&
                        webSiteServicesData.WebSiteConfiguration.Upselling.DisplayCategories != null &&
                        webSiteServicesData.WebSiteConfiguration.Upselling.DisplayCategories.Count > 0)
                    {
                        this.UPSELLINGENABLED = true;

                        this.UPSELLINGCATEGORIES = "[";
                        foreach(DisplayCategory displayCategory in webSiteServicesData.WebSiteConfiguration.Upselling.DisplayCategories)
                        {
                            this.UPSELLINGCATEGORIES += this.UPSELLINGCATEGORIES.Length > 1 ? "," : "";
                            this.UPSELLINGCATEGORIES += displayCategory.Id.ToString(); 
                        }
                        this.UPSELLINGCATEGORIES += "]";
                    }
                }
            }

            // Facebook share
            this.FACEBOOKSHAREENABLED = false;
            this.FACEBOOKSHARECAPTION = "";
            this.FACEBOOKSHARDESCRIPTION = "";

            if (webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings != null &&
                webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.FacebookSettings != null)
            {
                if (webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.FacebookSettings.IsShare)
                {
                    this.FACEBOOKSHAREENABLED = true;
                    this.FACEBOOKSHARECAPTION = webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.FacebookSettings.Caption;
                    this.FACEBOOKSHARDESCRIPTION = webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings.FacebookSettings.Description;
                }
            }

            // Hide page header
            this.ISPAGEHEADERVISIBLE = CustomSettings.BoolToString(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.IsPageHeaderVisible, true);
        }

        public static string ProcessFollowShareSocialNetworkSetting(SocialNetworkSetting socialNetworkSetting)
        {
            string url = "";
            if (socialNetworkSetting != null && socialNetworkSetting.IsEnable)
            {
                url = socialNetworkSetting.FollowURL;
            }

            return url;
        }

        public static string ProcessFollowShareSocialNetworkSetting(FacebookSetting socialNetworkSetting)
        {
            string url = "";
            if (socialNetworkSetting != null && socialNetworkSetting.IsEnable)
            {
                url = socialNetworkSetting.FollowURL;
            }

            return url;
        }

        public static string ProcessLikeSocialNetworkSetting(SocialNetworkSetting socialNetworkSetting)
        {
            string url = "";
            if (socialNetworkSetting != null && socialNetworkSetting.IsEnable)
            {
                if (!String.IsNullOrEmpty(socialNetworkSetting.LikeUrl))
                {
                    url = socialNetworkSetting.LikeUrl;
                }
            }

            return url;
        }

        public static string NullToText(string text, string defaultText)
        {
            return String.IsNullOrEmpty(text) ? defaultText : text;
        }

        public static string NullToBlank(string text)
        {
            return text == null ? String.Empty : text;
        }

        public static string BoolToString(bool? theValue, bool theDefault)
        {
            if (theValue.HasValue)
            {
                return theValue.ToString().ToLower();
            }
            else
            {
                return theDefault.ToString().ToLower();
            }
        }

        public static string StripUnsupportedCharacters(string source)
        {
            return NullToBlank(source).Replace("\'", "\\\'").Replace("\r", "").Replace("\n", "");
        }

        public string ToJavascript(string customJSTemplate)
        {
            // Build the custom.js file

            customJSTemplate = customJSTemplate.Replace("<<COMPANYLOGOURL>>", this.COMPANYLOGOURL);
            customJSTemplate = customJSTemplate.Replace("<<MOBILECOMPANYLOGOURL>>", this.MOBILECOMPANYLOGOURL);

            customJSTemplate = customJSTemplate.Replace("<<LOGOCLICKGOTOPARENTWEBSITE>>", this.REDIRECTTOPARENTONLOGOCLICK);
            customJSTemplate = customJSTemplate.Replace("<<LOGOCLICKRETURNHOME>>", (this.REDIRECTTOPARENTONLOGOCLICK == "false" ? "true" : "false"));
            customJSTemplate = customJSTemplate.Replace("<<LOGOCLICKURL>>", this.REDIRECTTOPARENTONLOGOCLICK == "true" ? this.PARENTWEBSITE : "#");

            customJSTemplate = customJSTemplate.Replace("<<TERMSOFUSE>>", this.TERMSOFUSE);
            customJSTemplate = customJSTemplate.Replace("<<PRIVACYPOLICY>>", this.PRIVACYPOLICY);
            customJSTemplate = customJSTemplate.Replace("<<COOKIEPOLICY>>", this.COOKIEPOLICY);

            customJSTemplate = customJSTemplate.Replace("<<ETD>>", this.ETD);
            customJSTemplate = customJSTemplate.Replace("<<MINUMUMDELIVERYORDER>>", this.MINUMUMDELIVERYORDER);
            customJSTemplate = customJSTemplate.Replace("<<OPTIONALFREEDELIVERYTHRESHOLD>>", this.OPTIONALFREEDELIVERYTHRESHOLD);
            customJSTemplate = customJSTemplate.Replace("<<DELIVERYCHARGE>>", this.DELIVERYCHARGE);
            customJSTemplate = customJSTemplate.Replace("<<CARDCHARGE>>", this.CARDCHARGE);

            customJSTemplate = customJSTemplate.Replace("<<TWITTER>>", this.TWITTER);
            customJSTemplate = customJSTemplate.Replace("<<FACEBOOK>>", this.FACEBOOK);
            customJSTemplate = customJSTemplate.Replace("<<INSTAGRAM>>", this.INSTAGRAM);
            customJSTemplate = customJSTemplate.Replace("<<PINTEREST>>", this.PINTEREST);

            customJSTemplate = customJSTemplate.Replace("<<HOMESTOREDETAILSTITLE>>", this.HOMESTOREDETAILSTITLE);
            customJSTemplate = customJSTemplate.Replace("<<HOMESTOREDETAILS>>", this.HOMESTOREDETAILS);
            customJSTemplate = customJSTemplate.Replace("<<HOMEMENUTITLE>>", this.HOMEMENUTITLE);
            customJSTemplate = customJSTemplate.Replace("<<HOMEMENUDETAILS>>", this.HOMEMENUDETAILS);

            customJSTemplate = customJSTemplate.Replace("<<CUSTOMERACCOUNTSENABLED>>", this.CUSTOMERACCOUNTSENABLED);
            customJSTemplate = customJSTemplate.Replace("<<ANDROLOGINENABLED>>", this.ANDROLOGINENABLED);
            customJSTemplate = customJSTemplate.Replace("<<FACEBOOKLOGINENABLED>>", this.FACEBOOKLOGINENABLED);
            customJSTemplate = customJSTemplate.Replace("<<FACEBOOKAPPID>>", this.FACEBOOKAPPID);

            customJSTemplate = customJSTemplate.Replace("<<FACEBOOKLIKEURL>>", this.FACEBOOKLIKEURL);

            customJSTemplate = customJSTemplate.Replace("<<GOOGLEANALYTICSID>>", this.GOOGLEANALYTICSID);

            customJSTemplate = customJSTemplate.Replace("<<CAROUSELITEMS>>", this.CAROUSELITEMS);

            customJSTemplate = customJSTemplate.Replace("<<RETURNTOPARENTWEBSITEAFTERORDER>>", this.RETURNTOPARENTWEBSITEAFTERORDER);
            customJSTemplate = customJSTemplate.Replace("<<PARENTWEBSITE>>", "'" + this.PARENTWEBSITE + "'");

            customJSTemplate = customJSTemplate.Replace("<<TRIPADVISORISENABLED>>", this.TRIPADVISORISENABLED.ToString().ToLower());

            customJSTemplate = customJSTemplate.Replace("<<SHOWDOUBLETOPPINGS>>", this.SHOWDOUBLETOPPINGS);

            customJSTemplate = customJSTemplate.Replace("<<ARETOPPINGSWAPSENABLED>>", this.ARETOPPINGSWAPSENABLED);

            customJSTemplate = customJSTemplate.Replace("<<CMSPAGES>>", this.CMSPAGES);

            customJSTemplate = customJSTemplate.Replace("<<ISPAGEHEADERVISIBLE>>", this.ISPAGEHEADERVISIBLE);

            customJSTemplate = customJSTemplate.Replace("<<ENABLECOLLECTION>>", this.ENABLECOLLECTION);
            customJSTemplate = customJSTemplate.Replace("<<ENABLEDELIVERY>>", this.ENABLEDELIVERY);
            customJSTemplate = customJSTemplate.Replace("<<ENABLEDINEIN>>", this.ENABLEDINEIN);

            customJSTemplate = customJSTemplate.Replace("<<UPSELLINGENABLED>>", this.UPSELLINGENABLED.ToString().ToLower());
            customJSTemplate = customJSTemplate.Replace("<<UPSELLINGCATEGORIES>>", this.UPSELLINGCATEGORIES);

            customJSTemplate = customJSTemplate.Replace("<<FACEBOOKSHAREENABLED>>", this.FACEBOOKSHAREENABLED.ToString().ToLower());
            customJSTemplate = customJSTemplate.Replace("<<FACEBOOKSHARECAPTION>>", this.FACEBOOKSHARECAPTION);
            customJSTemplate = customJSTemplate.Replace("<<FACEBOOKSHARDESCRIPTION>>", this.FACEBOOKSHARDESCRIPTION);

            return customJSTemplate;
        }
    }
}