/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

/* Test/debug */
settings.alwaysOnline = true;
settings.simulateBadOrder = false;

/* Header */
settings.logoClickGotoParentWebsite = true;
settings.logoClickUrl = 'http://megatest1.androtest.co.uk/';
settings.logoClickReturnHome = 'false';
settings.isHeaderLoginEnabled = true;
settings.supportEmail = ''; // Email address for customers to use for support
settings.companyLogoUrl = 'http://cdn.myandromedaweb.co.uk/test1/websites/3/sitedetails/websitelogo/websitelogo272x130.png';
settings.mobileCompanyLogoUrl = 'http://cdn.myandromedaweb.co.uk/test1/websites/3/sitedetails/mobilelogo/mobilelogo272x130.png';

/* Internationalisation */
settings.customerAddressCountryCode = 'UK'; 
settings.customerAddressCountry = 'United Kingdom'; 
settings.culture = 'en-GB'; 
textStrings = englishTextStrings;

/* Footer */
textStrings.infTermsOfUse = '<pre id="line1"><br /></pre><a href="http://test.myandromeda.co.uk/AndroWebOrdering/Chain/5/CEDD9916-B9D8-4FA1-AF08-9823D9C88F3A/AndroWebOrdering?id=3#LegalNotices-1"><span style="font-size:medium;background-color:#ff0000;">Terms Of Use</span></a>&nbsp;&nbsp; This is to test whether this feature is working fine or not ?';
textStrings.infPrivacyPolicy = '';
textStrings.infCookiePolicy = '';

/* Misc */
settings.logoAlt = 'Andromeda';
settings.welcomeHeader = 'Andromeda';
settings.chainName = 'Andromeda';
settings.mapMarker = 'Content/Images/store.png';
settings.defaultETD = 45; // ETD (in minutes) to be used if it is not provided by the store
settings.minimumDeliveryOrder = 0.0; // Minimum order price for delivery orders in pence
settings.supportEmail = '';

/* Store selector */
settings.storeSelectorMode = false; // Set to true to show a page prompting the customer to select a postcode which is used to select which store to order from (or false)
settings.storeSelectorInHeader = false;

/* Social */
settings.socialMediaEnabled = true;
settings.useAddThis = true; // Use addthis for the facebook/twitter buttons
settings.twitter = 'https://twitter.com/AndromedaPOS'; // Url to the customers twitter page
settings.facebook = 'https://www.facebook.com/Andromedadelivery'; // Url to the customers facebook page
settings.instagram = 'https://instagram.com/AndromedaPOS'; // Url to the customers instagram page - blank disables
settings.pinterest = ''; // Url to the customers pinterest page - blank disables
settings.facebookLikeUrl = 'https://www.facebook.com/Andromedadelivery'; // Facebook like

/* Main menu */
settings.isHomePageEnabled = true;
settings.showDeliveryCheckButton = false;
settings.showSelectStoresButton = false;
settings.showOrderNowButton = true;
settings.showReturnToParentWebsite = false;

/* Toppings popup */
settings.chefCommentsEnabled = false; // Show or hide the chefs comments textbox
settings.personEnabled = false; // Show or hide the for person textbox
    
/* Parent website */
settings.returnToParentWebsiteAfterOrder = true;
settings.parentWebsite = 'http://megatest1.androtest.co.uk/';
settings.requireCustomerDetailsPassthrough = false; 
settings.hidePassthroughQueryString = false; // Set true to remove the passthrough query string from the browser url

/* Checkout */
settings.customerAccountsEnabled = true; // Set to true to enable customer accounts or false to disable them
settings.faceBookLoginEnabled = true;
settings.androLoginEnabled = true;
settings.fullRegistration = true;

// Used for registering/logging in to your customer account using a Facebook account (this need to be setup in the Facebook application in Facebook)
settings.facebookAppId = 234191866729559;
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

/* Tags */
settings.menuItemOverlayTag = '';

/* Google analytics id */
settings.googleAnalyticsId = 'UA-56283640-1';
    
/* Trip advisor */
settings.tripAdvisorIsEnabled = true;

/* Home page */
textStrings.homeStoreDetailsTitle = 'Welcome to Andro Test';
textStrings.homeStoreDetails = '<p><span style="font-family:Verdana, Geneva, sans-serif;font-size:small;">Welcome to Andromeda\'s test site.&nbsp; Located in the heart of Wallington not washington, Andromeda\'s site caters to a wide range of testing scenarios by providing both a test site and associated configurations via&nbsp;My Andromeda.&nbsp;</span></p>';
textStrings.homeMenuTitle = 'Our Menu';
textStrings.homeMenu = '<p>Our menu is a test menu containing the finest generic menu items to allow for a wide range of testing.</p>';
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
        { type: 'Image', url: 'http://cdn.myandromedaweb.co.uk/test1/websites/3/carousels/featured/35d04564-15e5-4e21-0578-9984c88a6d17_1022x300.png?k=53936' }
,{ type: 'Image', url: 'http://cdn.myandromedaweb.co.uk/test1/websites/3/carousels/featured/95a2a92c-7764-4280-77f9-8f841e77b41b_1022x300.png?k=67716' }

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

