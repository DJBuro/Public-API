/// <reference path="MyAndromeda.WebOrdering.App.ts" />
declare module MyAndromeda.WebOrdering.Scopes {
    export interface IWebOrderingScope extends ng.IScope {
        IsBusy: boolean;
        SaveChanges: () => void;
    }

    export interface IStatusControllerScope extends IWebOrderingScope {
        Modal: kendo.mobile.ui.ModalView;
        PreviewWindow: kendo.ui.Window;

        PublishChanges: () => void;
        PreviewChanges: () => void;

        PublishPreviewBusy: boolean;
        PublishLiveBusy: boolean;
        Saving: boolean;
    }

    export interface ISiteDetailsScope extends IWebOrderingScope {
        SiteDetails: Models.ISiteDetails;
        WebSiteSettings: Models.IWebOrderingSettings;

        MainImageUpload: kendo.ui.Upload;
        MobileImageUpload: kendo.ui.Upload;
        FaviconImageUpload: kendo.ui.Upload;

        SiteDetailsValidator: kendo.ui.Validator;
        HasWebsiteLogo: boolean;
        TempWebsiteLogoPath: string;

        HasMobileLogo: boolean;
        TempMobileLogoPath: string;

        HasFaviconLogo: boolean;
        TempFaviconLogoPath: string;
        ValidateFavicon: (e: Object) => void;      
    }

    export interface IThemeScope extends IWebOrderingScope {
        SearchText: string;
        SearchTemplates: () => void;


        HasPreviewTheme: boolean;
        HasCurrentTheme: boolean;
        IsThemesBusy: boolean;
        IsDataBusy: boolean;
        ListViewTemplate: string;
        DataSource: kendo.data.DataSource;

        SelectedTheme: Models.IWebOrderingTheme;
        CurrentTheme: Models.IWebOrderingTheme;

        //actions 
        SelectTemplate: (id: number) => void;
        SelectPreviewTheme: (settings: Models.IWebOrderingTheme) => void;
    }

    export interface ILegalNoticesScope extends IWebOrderingScope {

    }

    export interface IHomePageScope extends IWebOrderingScope {

    }

    export interface IGeneralSettingsScope extends IWebOrderingScope {
        GeneralSettingsValidator: kendo.ui.Validator;
        
        MinimumDeliveryAmount: number; 
        ShowFacebookAppId: boolean;
        HasLoginOptions: boolean;

        ShowHideFacebookAppId: () => void;
        SetRequiredAttribute: () => void;
        
        ResetToDefault: () => void;

        GeneralSettings: Models.IGeneralSettings;
        JivoChatSettings: Models.IJivoChatSettings;

        DineInServiceCharge: kendo.ui.NumericTextBox;
        DineInServiceChargeOptions: kendo.ui.NumericTextBoxOptions;
        DineInServiceChargeLimit: kendo.ui.NumericTextBox;
        DineInServiceChargeLimitOptions: kendo.ui.NumericTextBoxOptions;
    }

    export interface ISocialNetworkSettingsScope extends IWebOrderingScope {
        SocialNetworkSettingsValidator: kendo.ui.Validator;

        ShowHideFacebookSettings: () => void;
        ShowFacebookSettings: boolean;
        ShowTwitterSettings: boolean;
        ShowInstagramSettings: boolean;
        //ShowTripAdvisorSettings: boolean;
        
        SocialNetworkSettings: Models.ISocialNetworkSettings;
        GeneralSettings: Models.IGeneralSettings;
        CustomerAccountSettings: Models.ICustomerAccountSettings;
    }

    export interface ITripAdvisorSettingsScope extends IWebOrderingScope {
        TripAdvisorSettingsValidator: kendo.ui.Validator;
        ShowTripAdvisorSettings: boolean; 
    }

    export interface ICarouselControllerScope extends IWebOrderingScope {
        CarouselBlocks: Models.ICarouselEditorModel[];

        EditCarouselItem: (carouselIndex: string, carouselItemId: string) => void;
        RemoveCarouselItem: (carouselIndex: string, carouselItemId: string) => void;
        CreateImageCarouselItem: (carouselName: string) => void;
        CreateHtmlCarouselItem: (carouselName: string) => void;

        ShowCarousel: boolean;
        ShowImageEditor: boolean;
        HasImageSlideUrl: boolean;
        HtmlBeforeEdit: string;

        CarouselImageUpload: kendo.ui.Upload;

        ShowHtmlEditor: boolean;

        CancelItem: (Id: string) => void;
        SaveItem: () => void;
        BindCarouselsList: () => void;
    }

    export interface ICustomerAccountSettingsScope extends IWebOrderingScope {

    }

    export interface IAnalyticsScope extends IWebOrderingScope {
        AnalyticsScript: string;
        EncodeScript: (analyticsScript:string) => void;
    }

    export interface ICustomThemeSettingsScope extends IWebOrderingScope {
        CustomThemeSettings: Models.ICustomThemeSettings

        HasDesktopBackgroundImage: boolean;
        TempDesktopBackgroundImagePath: string;
        HasMobileBackgroundImage: boolean;
        TempMobileBackgroundImagePath: string;

        CreateStyle: (colour: string) => any;
        ResetColors: () => void;
        SetLiveColors: () => void;
        DeleteCustomBackgroundImage: (imageType: string) => void;

        
    }

    export interface ICustomEmailTemplateScope extends IWebOrderingScope {
        SetLiveColors: () => void;
        ResetColors: () => void;

        CustomEmailTemplate: Models.ICustomEmailTemplate;
    }

    export interface IFacebookCrawlerSettingsScope extends IWebOrderingScope {
        FacebookProfileLogoUpload: kendo.ui.Upload;
        FacebookCrawlerSettingsValidator: kendo.ui.Validator;

        HasFacebookProfileLogo: boolean;
        TempFaceboolProfileLogoPath: string;
    }

    export interface ISEOSettingsScope extends IWebOrderingScope {
        FaviconUpload: kendo.ui.Upload;
        SEOSettingsValidator: kendo.ui.Validator;
        ShowSEODescription: boolean;       
    }
} 