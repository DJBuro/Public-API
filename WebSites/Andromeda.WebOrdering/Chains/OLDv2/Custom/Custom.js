/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

/* Test/debug */
settings.alwaysOnline = true;

/* Header */
settings.logoClickUrl = 'http://speedrabbitpizza.com/';

/* Internationalisation */
settings.customerAddressCountryCode = 'FR'; 
settings.culture = 'fr-FR'; 
textStrings = frenchTextStrings;

/* Misc */
settings.logoAlt = 'Speed Rabbit logo';
settings.welcomeHeader = 'Speed Rabbit';
settings.chainName = 'Speed Rabbit';
settings.mapMarker = 'Content/Images/store.png';
settings.defaultETD = 15; // ETD (in minutes) to be used if it is not provided by the store
settings.minimumDeliveryOrder = 1000; // Minimum order price for delivery orders in pence/cents
settings.storeSelectorMode = false; // Set to true to show a page prompting the customer to select a postcode which is used to select which store to order from (or false)

/* Social */
settings.useAddThis = false; // Use addthis for the facebook/twitter buttons
settings.twitter = 'https://twitter.com/pizzalille'; // Url to the customers twitter page
settings.facebook = 'https://www.facebook.com/pages/Speed-Rabbit-Pizza/125043297525347'; // Url to the customers facebook page

/* Main menu */
settings.isHomePageDisabled = true;
settings.showDeliveryCheckButton = false;
settings.showSelectStoresButton = false;
settings.showOrderNowButton = false;
settings.showReturnToParentWebsite = true;

/* Parent website */
settings.returnToParentWebsiteAfterOrder = true;
settings.parentWebsite = 'http://speedrabbitpizza.com';
settings.requireCustomerDetailsPassthrough = true; 
settings.hidePassthroughQueryString = false; // Set true to remove the passthrough query string from the browser url

settings.customerAccountsEnabled = false; // Set to true to enable customer accounts or false to disable them

// Used for registering/logging in to your customer account using a Facebook account (this need to be setup in the Facebook application in Facebook)
settings.facebookAppId = 234191866729559;
settings.facebookAppChannel = 'http://dev.androtest.co.uk/channel.html';

/* Checkout */
settings.displayCustomerDetailsPage = false;
settings.lockCustomerDetailsPage = true;
settings.displayCustomerAddressPage = false;
settings.lockCustomerAddressPage = true;
settings.termsAndConditionsEnabled = false;

/* Tags */
settings.menuItemOverlayTag = 'nopork';
