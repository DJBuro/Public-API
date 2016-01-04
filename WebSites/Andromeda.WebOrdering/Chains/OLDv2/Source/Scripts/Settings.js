/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

/* Application settings with defaults */
settings =
{
    /* Test/debug */
    // Override the status of the selected store so it's always online
    alwaysOnline: false,
    showMenuIds: false,

    /* Header */
    logoClickUrl: '#',

    // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
    // profile or the stores country cannot be sent in the order JSON.  There is a hard coded country code for that.
    customerAddressCountryCode: 'UK', // The country code that gets sent in the order json.  This is not a locale.
    culture: 'en-GB',  // The language to use - determines which resourcestrings file to use.  Currently en-GB and fr-FR are supported.  NOTE the currency symbol displayed is determined by the country in the store address

    /* Misc */
    mapMarker: '', // The url for an icon to display on the map that represents the store
    defaultETD: 15, // ETD (in minutes) to be used if it is not provided by the store
    minimumDeliveryOrder: 1000, // Minimum order price for delivery orders in pence/cents
    storeSelectorMode: false, // Set to true to show a page prompting the customer to select a postcode which is used to select which store to order from (or false)

    /* Social */
    useAddThis: false, // Use addthis for the facebook/twitter buttons
    twitter: '', // Url to the customers twitter page
    facebook: '', // Url to the customers facebook page

    /* Graphics */
    mobileWidth: 600, // Browser window width less than this is considered a mobile phone.  This should match the media values in the css.
    tabletWidth: 1024, // Browser window width less than this is considered a tablet.  This should match the media values in the css.

    /* Main menu */
    isHomePageDisabled: false, // Set to true to hide the home page and remove the “home” button from the main menu
    showDeliveryCheckButton: true, // Set to false to hide the "check postcode" button on the main menu
    showSelectStoresButton: true, // Set to false to hide the "change store" button on the main menu (this button is only shown when there is more than one store in the chain)
    showOrderNowButton: true, // Set to false to hide the “order now” button on the main menu.  This is the button that shows the menu.  You shouldn’t need to use this.  It’s just for completeness (settings to disable all main menu buttons).
    showReturnToParentWebsite: false,  // Set to true to display a “return to parent website” button.  This uses the parentWebsite setting to redirect the browser to the parent website

    /* Parent website */
    returnToParentWebsiteAfterOrder: false, // Set to true to automatically redirect the browser to the parent website after an order has been placed.  This uses the parentWebsite setting to redirect the browser to the parent website
    parentWebsite: undefined, // The full url of the parent website.  Blank by default
    requireCustomerDetailsPassthrough: false, // Set true to require the customer details to be passed in as a query string. If not provided the browser is redirected back to the parent website
    hidePassthroughQueryString: true, // Set true to remove the passthrough query string from the browser url

    // Used for registering/logging in to your customer account using a Facebook account (this need to be setup in the Facebook application in Facebook)
    facebookAppId: 0,
    facebookAppChannel: '',

    /* Checkout */
    customerAccountsEnabled: true, // Set to true to enable customer accounts or false to disable them
    displayCustomerDetailsPage: true, // Set to false to hide the customer details section from the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    lockCustomerDetailsPage: false, // Set to true to make the customer details section read only on the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    displayCustomerAddressPage: true, // Set to false to hide the customer address section from the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    lockCustomerAddressPage: false, // Set to true to make the customer address section read only on the checkout page.  Only use this when requireCustomerDetailsPassthrough is true as the customer details must be provided – even if it not by the customer
    termsAndConditionsEnabled: true, // Set to false to remove the terms and conditions tick box

    /* Menu */
    invertItems: false, // Switch the menu item name with the items cat1 name before merging menu items

    /* Tags */
    menuItemOverlayTag: undefined
};