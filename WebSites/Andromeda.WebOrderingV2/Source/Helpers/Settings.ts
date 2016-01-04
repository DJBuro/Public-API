/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

/* Application settings with defaults */
class settings
{
    /* Test/debug */
    // Override the status of the selected store so it's always online
    static alwaysOnline: boolean = false;
    static simulateBadOrder: boolean = false;
    static showMenuIds: boolean = false;
    static useMenuKnockoutBinding: boolean = true;
    static diagnosticsMode: boolean = false;

    /* Header */
    static logoClickGotoParentWebsite: boolean = false;
    static logoClickReturnToStoreSelector: boolean = false;
    static logoClickUrl: string = '#';
    static logoClickReturnHome: boolean = true;
    static isHeaderLoginEnabled: boolean = false;
    static supportEmail: string = ""; // Email address for customers to use for support
    static socialMediaEnabled: boolean = false;
    static showStoreName: boolean = true;

    // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
    // profile or the stores country cannot be sent in the order JSON.  There is a hard coded country code for that.
    static customerAddressCountryCode: string = 'UK'; // The country code that gets sent in the order json.  This is not a locale.
    static customerAddressCountry: string = 'United Kingdom';
    static culture: string = 'en-GB';  // The language to use - determines which resourcestrings file to use.  Currently en-GB and fr-FR are supported.  NOTE the currency symbol displayed is determined by the country in the store address

    /* Home page */
    static mapMarker: string = ""; // The url for an icon to display on the map that represents the store
    static defaultETD: number = 45; // ETD (in minutes) to be used if it is not provided by the store
    static minimumDeliveryOrder: number = 0; // Minimum order price for delivery orders in pence/cents   

    /* Store selector */
    static storeSelectorMode: boolean = false; // Set to true to show a page prompting the customer to select a postcode which is used to select which store to order from (or false)
    static storeSelectorInHeader: boolean = false; // When true; displays the postcode texbox in the header.  When false displays the postcode textbox in the body
    static showCollectFromStoreListIfBadPostcode: boolean = true; // Show the collect from store list if no stores deliver to the customers postcode

    /* Social media */
    static useAddThis: boolean = false; // Use addthis for the facebook/twitter buttons
    static twitter: string = ""; // Url to the customers twitter page - blank disables
    static facebook: string = ""; // Url to the customers facebook page - blank disables
    static instagram: string = ""; // Url to the customers instagram page - blank disables
    static pinterest: string = ""; // Url to the customers pinterest page - blank disables

    /* Graphics */
    static mobileWidth: number = 600; // Browser window width less than this is considered a mobile phone.  This should match the media values in the css.
    static tabletWidth: number = 1024; // Browser window width less than this is considered a tablet.  This should match the media values in the css.

    /* Main menu */
    static isStoreDetailsPageEnabled: boolean = true; // Set to true to show the store details page and add the “store details” button to the main menu
    static isHomePageEnabled: boolean = true; // Set to true to show the home page and add the “home” button to the main menu
    static showDeliveryCheckButton: boolean = true; // Set to false to hide the "check postcode" button on the main menu
    static showSelectStoresButton: boolean = true; // Set to false to hide the "change store" button on the main menu (this button is only shown when there is more than one store in the chain)
    static showOrderNowButton: boolean = true; // Set to false to hide the “order now” button on the main menu.  This is the button that shows the menu.  You shouldn’t need to use this.  It’s just for completeness (settings to disable all main menu buttons).
    static showReturnToParentWebsite: boolean = false;  // Set to true to display a “return to parent website” button.  This uses the parentWebsite setting to redirect the browser to the parent website

    /* Parent website */
    static returnToParentWebsiteAfterOrder: boolean = false; // Set to true to automatically redirect the browser to the parent website after an order has been placed.  This uses the parentWebsite setting to redirect the browser to the parent website
    static parentWebsite: string = undefined; // The full url of the parent website.  Blank by default
    static requireCustomerDetailsPassthrough: boolean = false; // Set true to require the customer details to be passed in as a query string. If not provided the browser is redirected back to the parent website
    static hidePassthroughQueryString: boolean = true; // Set true to remove the passthrough query string from the browser url

    // Used for registering/logging in to your customer account using a Facebook account (this need to be setup in the Facebook application in Facebook)
    static facebookAppId: number = 0;
    static facebookAppChannel: string = "";

    // Facebook like
    static facebookLikeUrl: string = "";

    /* Toppings popup */
    static chefCommentsEnabled: boolean = true; // Show or hide the chefs comments textbox
    static personEnabled: boolean = true; // Show or hide the for person textbox

    /* Checkout */
    static customerAccountsEnabled: boolean = false; // Set to true to enable customer accounts or false to disable them
    static faceBookLoginEnabled: boolean = true; // Allow login/register using a facebook account
    static androLoginEnabled: boolean = true; // Allow login/register using a user/pass
    static fullRegistration: boolean = false;
    static displayCustomerDetailsPage: boolean = true; // Set to false to hide the customer details section from the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    static lockCustomerDetailsPage: boolean = false; // Set to true to make the customer details section read only on the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    static displayCustomerAddressPage: boolean = true; // Set to false to hide the customer address section from the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    static lockCustomerAddressPage: boolean = false; // Set to true to make the customer address section read only on the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    static termsAndConditionsEnabled: boolean = true; // Set to false to remove the terms and conditions tick box
    static displayUpsellingPage: boolean = false;
    static displayOrderSummaryPage: boolean = false;
    static displayOrderNotesPage: boolean = true;
    static displayPaymentPage: boolean = false;
    static checkoutUpdateCustomerAccount: boolean = true;
    static voucherCodesEnabled: boolean = true;

    /* Menu */
    static invertItems: boolean = false; // Switch the menu item name with the items cat1 name before merging menu items
    static alwaysShowToppingsPopup: boolean = false;
    static maxQuantity: number = 9;

    /* Tags */
    static menuItemOverlayTag: string = undefined;

    /* Address */
    static postcodeRequired: boolean = true;

    /* Toppings swap mode */
    static allowSwaps: boolean = true;

    /* Double toppings */
    static showDoubleToppings: boolean = true;

    /* Google analytics id */
    static googleAnalyticsId: string = "";

    /* Trip advisor */
    static tripAdvisorIsEnabled: boolean = false;
    static tripAdvisorScript: string = "";

    /* Company Logo */
    static companyLogoUrl: string = 'Custom/Images/MyLogo.png';
    static mobileCompanyLogoUrl: string = 'Custom/Images/MyLogo.png';

    static isFeedbackEnabled: boolean = true;
};