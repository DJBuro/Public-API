/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

/* Test/debug */
settings.alwaysOnline = true;
settings.simulateBadOrder = false;
settings.generateTestPerson = true;
settings.generateTestEventName = true;
settings.sendDineInOrdersAsDelivery = true;

/* Header */
settings.isPageHeaderVisible = <<ISPAGEHEADERVISIBLE>>;

settings.logoClickGotoParentWebsite = <<LOGOCLICKGOTOPARENTWEBSITE>>;
settings.logoClickUrl = '<<LOGOCLICKURL>>';
settings.logoClickReturnHome = '<<LOGOCLICKRETURNHOME>>';
settings.isHeaderLoginEnabled = true;
settings.supportEmail = ''; // Email address for customers to use for support
settings.companyLogoUrl = '<<COMPANYLOGOURL>>';
settings.mobileCompanyLogoUrl = '<<MOBILECOMPANYLOGOURL>>';

/* Internationalisation */
settings.customerAddressCountryCode = 'UK'; 
settings.customerAddressCountry = 'United Kingdom'; 
settings.culture = 'en-GB'; 
textStrings = englishTextStrings;

/* Footer */
textStrings.infTermsOfUse = '<<TERMSOFUSE>>';
textStrings.infPrivacyPolicy = '<<PRIVACYPOLICY>>';
textStrings.infCookiePolicy = '<<COOKIEPOLICY>>';

/* Misc */
settings.logoAlt = 'Andromeda';
settings.welcomeHeader = 'Andromeda';
settings.chainName = 'Andromeda';

/* Home page */
settings.mapMarker = 'Content/Images/store.png';
settings.defaultETD = <<ETD>>; // ETD (in minutes) to be used if it is not provided by the store
settings.minimumDeliveryOrder = <<MINUMUMDELIVERYORDER>>; // Minimum order price for delivery orders in pence
settings.deliveryCharge = <<DELIVERYCHARGE>>; // The decimal delivery charge (if a delviery charge needs to be applied)
settings.optionalFreeDeliveryThreshold = <<OPTIONALFREEDELIVERYTHRESHOLD>>; // If the total order value is less this value then apply the delivery charge
settings.cardCharge = <<CARDCHARGE>>; // The decimal card charge (if a card charge needs to be applied)

/* Store selector */
settings.storeSelectorMode = false; // Set to true to show a page prompting the customer to select a postcode which is used to select which store to order from (or false)
settings.storeSelectorInHeader = false;

/* Social */
settings.socialMediaEnabled = true;
settings.useAddThis = true; // Use addthis for the facebook/twitter buttons
settings.twitter = '<<TWITTER>>'; // Url to the customers twitter page
settings.facebook = '<<FACEBOOK>>'; // Url to the customers facebook page
settings.instagram = '<<INSTAGRAM>>'; // Url to the customers instagram page - blank disables
settings.pinterest = '<<PINTEREST>>'; // Url to the customers pinterest page - blank disables
settings.facebookLikeUrl = '<<FACEBOOKLIKEURL>>'; // Facebook like

/* Main menu */
settings.isHomePageEnabled = true;
settings.showDeliveryCheckButton = false;
settings.showSelectStoresButton = false;
settings.showOrderNowButton = true;
settings.showReturnToParentWebsite = false;

/* Occasions */
settings.enableDelivery = <<ENABLEDELIVERY>>; // False when delivery is not allowed
settings.enableCollection = <<ENABLECOLLECTION>>; // False when collection is not allowed
settings.enableDineIn = <<ENABLEDINEIN>>; // False when dinein is not allowed

/* Toppings popup */
settings.chefCommentsEnabled = false; // Show or hide the chefs comments textbox
settings.personEnabled = false; // Show or hide the for person textbox
    
/* Double toppings */
settings.showDoubleToppings = <<SHOWDOUBLETOPPINGS>>;

/* Parent website */
settings.returnToParentWebsiteAfterOrder = <<RETURNTOPARENTWEBSITEAFTERORDER>>;
settings.parentWebsite = <<PARENTWEBSITE>>;
settings.requireCustomerDetailsPassthrough = false; 
settings.hidePassthroughQueryString = false; // Set true to remove the passthrough query string from the browser url

/* Checkout */
settings.customerAccountsEnabled = <<CUSTOMERACCOUNTSENABLED>>; // Set to true to enable customer accounts or false to disable them
settings.faceBookLoginEnabled = <<FACEBOOKLOGINENABLED>>;
settings.androLoginEnabled = <<ANDROLOGINENABLED>>;
settings.fullRegistration = true;

// Used for registering/logging in to your customer account using a Facebook account (this need to be setup in the Facebook application in Facebook)
settings.facebookAppId = <<FACEBOOKAPPID>>;
settings.facebookAppChannel = 'http://' + window.location.host + '/channel.html';

settings.displayCustomerDetailsPage = false;
settings.lockCustomerDetailsPage = false;
settings.displayCustomerAddressPage = true;
settings.lockCustomerAddressPage = false;
settings.termsAndConditionsEnabled = false;
settings.displayUpsellingPage = false;
settings.displayOrderSummaryPage = true;
settings.displayOrderNotesPage = false;
settings.displayPaymentPage = true;

/* Menu */
settings.alwaysShowToppingsPopup = false;
settings.maxQuantity = 99;

/* Toppings */
settings.areToppingSwapsEnabled = <<ARETOPPINGSWAPSENABLED>>;

/* Tags */
settings.menuItemOverlayTag = '';

/* Google analytics id */
settings.googleAnalyticsId = '<<GOOGLEANALYTICSID>>';
    
/* Trip advisor */
settings.tripAdvisorIsEnabled = <<TRIPADVISORISENABLED>>;

/* Home page */
textStrings.homeStoreDetailsTitle = '<<HOMESTOREDETAILSTITLE>>';
textStrings.homeStoreDetails = '<<HOMESTOREDETAILS>>';
textStrings.homeMenuTitle = '<<HOMEMENUTITLE>>';
textStrings.homeMenu = '<<HOMEMENUDETAILS>>';
settings.HomePageCarousels =
[ 
    {
        elementId: 'homeCarousel1Container',
        options:
        {
            $AutoPlay: true,
            $ArrowNavigatorOptions:
            {
                $Class: $JssorArrowNavigator$,
                $ChanceToShow: 2
            }
        },
        items:
        [
        <<CAROUSELITEMS>>
        ]
    },
    {
        elementId: 'homeCarousel2Container',
        options:
        {
            $AutoPlay: false,
            $DisplayPieces: 4,
            $AutoPlaySteps: 4,
            $SlideWidth: 225,
            $SlideDuration: 300,
            $ArrowNavigatorOptions:
            {
                $Class: $JssorArrowNavigator$,
                $ChanceToShow: 2,
                $AutoCenter: 2,
                $Steps: 4
            }
        },
        items:
        [
        ]
    }
];
settings.pages = <<CMSPAGES>>;

/* Upsell */
settings.isUpsellEnabled = <<UPSELLINGENABLED>>;
settings.upsellingCategories = <<UPSELLINGCATEGORIES>>;

/* Facebook upsell */
settings.isFacebookShareEnabled = <<FACEBOOKSHAREENABLED>>;
settings.facebookShareCaption = '<<FACEBOOKSHARECAPTION>>';
settings.facebookShareDescription = '<<FACEBOOKSHARDESCRIPTION>>';
