/// <reference path="MyAndromeda.WebOrdering.App.ts" />
declare module MyAndromeda.WebOrdering.Models {
    export interface IObservable {
        get?: (key: string) => any;
        set?: (key: string, value: any) => void;
        bind? (eventName: string, handler: Function): IObservable;
    }

    export interface IStore {
        AndromedaSiteId: number;
        Name: string;
    }

    export interface IPage {
        Title: string;
        Content: string;
        Enabled: boolean;
    }
    export interface ICmsPages
    {
        Pages : IPage[]
    }

    export interface IJivoChatSettings {
        IsJivoChatEnabled: boolean;
        Script: string;
    }

    export interface IWebSiteRow {
        LiveDomainName: string;
        PreviewDomainName: string;
    }

    export interface IWebOrderingSettings extends IObservable, ICmsPages {
        WebSiteId: number;
        WebSiteName: string;
        ACSApplicationId: number;
        /* Site details tab */
        SiteDetails: ISiteDetails;
        /* home page tab */
        Home: IHomePage;

        /* Pick a theme tab */
        ThemeSettings: IWebOrderingTheme;
        GeneralSettings: IGeneralSettings;

        MenuPageSettings: IMenuPageSettings;
        LegalNotices: ILegalNotices;
        SocialNetworkSettings: ISocialNetworkSettings;
        TripAdvisorSettings: ITripAdvisorSettings;
        FacebookCrawlerSettings: IFacebookCrawlerSettings;

        LastUpdatedUtc: Date

        JivoChatSettings: IJivoChatSettings;
        CustomerAccountSettings: ICustomerAccountSettings;
        AnalyticsSettings: IAnalyticSettings;
        CustomThemeSettings: ICustomThemeSettings;
        CustomEmailTemplate: ICustomEmailTemplate;
        SEOSettings: ISEOSettings;

        //custom cms pages
        Pages: IPage[];

        UpSelling: IUpSellSettings;
    }

    export interface IUpSellSettings {
        Enabled: boolean;
        DisplayCategories: IDisplayCategoy[];
    }

    export interface IDisplayCategoy {
        Name : string 
    }

    export interface IMenuPageSettings {
        //hides the double modifier 
        IsSingleToppingsOnlyEnabled: boolean;
        //shows the thumbnails for items
        IsThumbnailsEnabled?: boolean;
        
        IsQuantityDropdownEnabled?: boolean;
        
        AreToppingsSwapsEnabled?: boolean;

        //charge the topping price difference between 
        ChargeToppingPriceDifference?: boolean;
    }

    export interface IWebOrderingTheme {
        Id: number;
        ThemeName: string;
        FileName: string;
        Source: string;
    }

    export interface IGeneralSettings extends IObservable {
        /* "GeneralSettings":{"MinimumDeliveryAmount":25.0,"DeliveryCharge":2.0,"ApplyDeliveryCharges":false,"EnableStoreLocatorPage":false,"IsList":false,"IsEnterPostCode":true},"ThemeSettings":{"Id":3,"ThemeName":"theme_3_600x600.png","Source":null,"Width":null,"Height":null,"FileName":null},"HomePageSettings":{"EnableHomePage":true,"ShowMinimumDelivery":false,"ShowDeliveryCharge":false,"ShowETD":false} */
        
        /* Delivery  */
        MinimumDeliveryAmount?: number;
        DeliveryCharge?: number;
        OptionalFreeDeliveryThreshold?: number;
        ApplyDeliveryCharges?: boolean;
        CardCharge?: number;

        /* Store picker - i think */
        EnableStoreLocatorPage?: boolean;
        IsList?: boolean;
        IsEnterPostCode?: boolean;

        /* Page settings */
        EnableHomePage: boolean;
        
        /* collection or dine in  */
        EnableDelivery?: boolean;
        EnableCollection?: boolean;
        EnableDineIn?: boolean;
        /* customer account settings */
        //EnableAndromedaLogin: boolean;
        //FacebookApplicationId?: string;
        //EnableFacebookLogin?: boolean;

        DineInServiceCharge?: number;
        LegalDineInServiceChargeLimit?: number;
        CompulsoryDineInServiceCharge?: boolean;
    }

    export interface IHomePage {
        Welcome: IWelcomeSettings;
        Menu: IMenuSettings;
        Carousels: ICarousel[];
    }

    export interface ISiteDetails extends IObservable {
        DomainName: string;
        ParentWebSiteName: string;
        RedirectToParentAfterCheckout: boolean;
        RedirectToParentOnLogoClick: boolean;

        WebsiteLogoPath: string;    //"http://cdn.myandromedaweb.co.uk/dev/websites/1/websitelogo450x150.png",
        MobileLogoPath: string;     //"http://cdn.myandromedaweb.co.uk/dev/websites/1/mobilelogo130x130.png"},
        FaviconPath: string;

    }

    export interface ILegalNotices {
        TermsOfUse: string;
        PrivacyPolicy: string;
        CookiePolicy: string;
    }

    export interface IWelcomeSettings {
        TitleText: string;
        DescriptionText: string;
    }

    export interface IMenuSettings {
        TitleText: string;
        DescriptionText: string;
    }

    export interface ISocialNetworkSiteSettings extends IObservable {
        IsEnable: boolean;
        IsShare: boolean;
        IsFollow: boolean;
        FollowURL: string;        
    }

    export interface IFacebookSocialNetworkSiteSettings extends ISocialNetworkSiteSettings {
        Caption: string;
        Description: string;
    }

    export interface ITripAdvisorSettings extends IObservable {
        Script: string;
        IsEnable: boolean;
    }

    export interface ISocialNetworkSettings extends IObservable {
        FacebookSettings: IFacebookSocialNetworkSiteSettings;
        TwitterSettings: ISocialNetworkSiteSettings;
        InstagramSettings: ISocialNetworkSiteSettings;
        PinterestSettings: ISocialNetworkSiteSettings;
        TripAdvisorSettings: ISocialNetworkSiteSettings;
    }

    export interface ICarousel {
        Name: string;
        CarouselContainer: any;
        CarouselNavigator: any;
        Items: ICarouselItem[]
    }

    export interface ICarouselEditorModel {
        Carousel: ICarousel;
        DataSource: kendo.data.DataSource;
        GridOptions: kendo.ui.GridOptions;
    }

    export interface ICarouselItem {
        Id: string;

        IsOverlayText?: boolean;

        // applicable only for Menu type
        IsOverrideDefaultMenuImage?: boolean;

        // applicable only for HTML type
        HTML?: string;

        ImageUrl: string;
        Description: string;

        Type: string;
    }


    export interface ICustomerAccountSettings extends IObservable {
        IsEnable: boolean;
        EnableAndromedaLogin: boolean;
        EnableFacebookLogin?: boolean;

        FacebookAccountId?: string;
        EnableOrderHistory?: boolean;
        EnableRepeatOrder?: boolean;
    }

    export interface IAnalyticSettings extends IObservable {
        AnalyticsScript: string;
        GoogleAnalyticsId: string;
    }

    export interface ICustomThemeSettings extends IObservable {
        DesktopBackgroundImagePath: string;
        MobileBackgroundImagePath: string;

        ColourRange1: string;
        ColourRange2: string;
        ColourRange3: string;
        ColourRange4: string;
        ColourRange5: string;
        ColourRange6: string;
       
        LiveColourRange1: string;
        LiveColourRange2: string;
        LiveColourRange3: string;
        LiveColourRange4: string;
        LiveColourRange5: string;
        LiveColourRange6: string;
        
        AddOrCustomizeColour?: string; // optional hex colour 

        IsPageHeaderVisible: boolean;
    }

    export interface ICustomEmailTemplate extends IObservable {
        HeaderColour: string;
        FooterColour: string;

        LiveHeaderColour: string;
        LiveFooterColour: string;

        CustomTemplates: IPageLookup;
    }
    
    interface IPageLookup {[andromedaId :string] : IPage[]}

    export interface IFacebookCrawlerSettings extends IObservable {
        Title: string;
        SiteName: string;
        Description: string;
        FacebookProfileLogoPath: string;        
    }

    export interface ISEOSettings extends IObservable {
        Title: string;
        Keywords: string;
        Description: string;        
        IsEnableDescription: boolean;
    }
}