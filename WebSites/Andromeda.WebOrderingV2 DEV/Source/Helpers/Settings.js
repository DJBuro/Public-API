/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED.
*/
/* Application settings with defaults */
var settings = (function () {
    function settings() {
    }
    /* Test/debug */
    // Override the status of the selected store so it's always online
    settings.alwaysOnline = false;
    settings.simulateBadOrder = false;
    settings.showMenuIds = false;
    settings.useMenuKnockoutBinding = true;
    settings.diagnosticsMode = false;
    /* Header */
    settings.logoClickGotoParentWebsite = false;
    settings.logoClickReturnToStoreSelector = false;
    settings.logoClickUrl = '#';
    settings.logoClickReturnHome = true;
    settings.isHeaderLoginEnabled = false;
    settings.supportEmail = ""; // Email address for customers to use for support
    settings.socialMediaEnabled = false;
    settings.showStoreName = true;
    // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
    // profile or the stores country cannot be sent in the order JSON.  There is a hard coded country code for that.
    settings.customerAddressCountryCode = 'UK'; // The country code that gets sent in the order json.  This is not a locale.
    settings.customerAddressCountry = 'United Kingdom';
    settings.culture = 'en-GB'; // The language to use - determines which resourcestrings file to use.  Currently en-GB and fr-FR are supported.  NOTE the currency symbol displayed is determined by the country in the store address
    /* Home page */
    settings.mapMarker = ""; // The url for an icon to display on the map that represents the store
    settings.defaultETD = 45; // ETD (in minutes) to be used if it is not provided by the store
    settings.minimumDeliveryOrder = 0; // Minimum order price for delivery orders in pence/cents   
    /* Store selector */
    settings.storeSelectorMode = false; // Set to true to show a page prompting the customer to select a postcode which is used to select which store to order from (or false)
    settings.storeSelectorInHeader = false; // When true; displays the postcode texbox in the header.  When false displays the postcode textbox in the body
    settings.showCollectFromStoreListIfBadPostcode = true; // Show the collect from store list if no stores deliver to the customers postcode
    /* Social media */
    settings.useAddThis = false; // Use addthis for the facebook/twitter buttons
    settings.twitter = ""; // Url to the customers twitter page - blank disables
    settings.facebook = ""; // Url to the customers facebook page - blank disables
    settings.instagram = ""; // Url to the customers instagram page - blank disables
    settings.pinterest = ""; // Url to the customers pinterest page - blank disables
    /* Graphics */
    settings.mobileWidth = 600; // Browser window width less than this is considered a mobile phone.  This should match the media values in the css.
    settings.tabletWidth = 1024; // Browser window width less than this is considered a tablet.  This should match the media values in the css.
    /* Main menu */
    settings.isStoreDetailsPageEnabled = true; // Set to true to show the store details page and add the “store details” button to the main menu
    settings.isHomePageEnabled = true; // Set to true to show the home page and add the “home” button to the main menu
    settings.showDeliveryCheckButton = true; // Set to false to hide the "check postcode" button on the main menu
    settings.showSelectStoresButton = true; // Set to false to hide the "change store" button on the main menu (this button is only shown when there is more than one store in the chain)
    settings.showOrderNowButton = true; // Set to false to hide the “order now” button on the main menu.  This is the button that shows the menu.  You shouldn’t need to use this.  It’s just for completeness (settings to disable all main menu buttons).
    settings.showReturnToParentWebsite = false; // Set to true to display a “return to parent website” button.  This uses the parentWebsite setting to redirect the browser to the parent website
    /* Parent website */
    settings.returnToParentWebsiteAfterOrder = false; // Set to true to automatically redirect the browser to the parent website after an order has been placed.  This uses the parentWebsite setting to redirect the browser to the parent website
    settings.parentWebsite = undefined; // The full url of the parent website.  Blank by default
    settings.requireCustomerDetailsPassthrough = false; // Set true to require the customer details to be passed in as a query string. If not provided the browser is redirected back to the parent website
    settings.hidePassthroughQueryString = true; // Set true to remove the passthrough query string from the browser url
    // Used for registering/logging in to your customer account using a Facebook account (this need to be setup in the Facebook application in Facebook)
    settings.facebookAppId = 0;
    settings.facebookAppChannel = "";
    // Facebook like
    settings.facebookLikeUrl = "";
    /* Toppings popup */
    settings.chefCommentsEnabled = true; // Show or hide the chefs comments textbox
    settings.personEnabled = true; // Show or hide the for person textbox
    /* Checkout */
    settings.customerAccountsEnabled = false; // Set to true to enable customer accounts or false to disable them
    settings.faceBookLoginEnabled = true; // Allow login/register using a facebook account
    settings.androLoginEnabled = true; // Allow login/register using a user/pass
    settings.fullRegistration = false;
    settings.displayCustomerDetailsPage = true; // Set to false to hide the customer details section from the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    settings.lockCustomerDetailsPage = false; // Set to true to make the customer details section read only on the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    settings.displayCustomerAddressPage = true; // Set to false to hide the customer address section from the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    settings.lockCustomerAddressPage = false; // Set to true to make the customer address section read only on the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    settings.termsAndConditionsEnabled = true; // Set to false to remove the terms and conditions tick box
    settings.displayUpsellingPage = false;
    settings.displayOrderSummaryPage = false;
    settings.displayOrderNotesPage = true;
    settings.displayPaymentPage = false;
    settings.checkoutUpdateCustomerAccount = true;
    settings.voucherCodesEnabled = true;
    /* Menu */
    settings.invertItems = false; // Switch the menu item name with the items cat1 name before merging menu items
    settings.alwaysShowToppingsPopup = false;
    settings.maxQuantity = 9;
    /* Tags */
    settings.menuItemOverlayTag = undefined;
    /* Address */
    settings.postcodeRequired = true;
    /* Toppings swap mode */
    settings.allowSwaps = true;
    /* Double toppings */
    settings.showDoubleToppings = true;
    /* Google analytics id */
    settings.googleAnalyticsId = "";
    /* Trip advisor */
    settings.tripAdvisorIsEnabled = false;
    settings.tripAdvisorScript = "";
    /* Company Logo */
    settings.companyLogoUrl = 'Custom/Images/MyLogo.png';
    settings.mobileCompanyLogoUrl = 'Custom/Images/MyLogo.png';
    settings.isFeedbackEnabled = true;
    return settings;
})();
;
//# sourceMappingURL=Settings.js.map