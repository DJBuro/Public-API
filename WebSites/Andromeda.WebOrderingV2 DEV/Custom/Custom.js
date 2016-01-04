/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

/* Test/debug */
settings.alwaysOnline = true;
settings.simulateBadOrder = false;

/* Header */
settings.logoClickGotoParentWebsite = false;
settings.logoClickUrl = '#';
settings.logoClickReturnHome = 'true';
settings.isHeaderLoginEnabled = true;
settings.supportEmail = ''; // Email address for customers to use for support
settings.companyLogoUrl = 'http://cdn.myandromedaweb.co.uk/dev/websites/29/sitedetails/websitelogo/websitelogo272x130.png';
settings.mobileCompanyLogoUrl = 'http://cdn.myandromedaweb.co.uk/dev/websites/29/sitedetails/mobilelogo/mobilelogo272x130.png';

/* Internationalisation */
settings.customerAddressCountryCode = 'UK'; 
settings.customerAddressCountry = 'United Kingdom'; 
settings.culture = 'en-GB'; 
textStrings = englishTextStrings;

/* Footer */
textStrings.infTermsOfUse = 'Some random Terms and Conditions.';
textStrings.infPrivacyPolicy = '';
textStrings.infCookiePolicy = '';

/* Misc */
settings.logoAlt = 'Andromeda';
settings.welcomeHeader = 'Andromeda';
settings.chainName = 'Andromeda';
settings.mapMarker = 'Content/Images/store.png';
settings.defaultETD = 45; // ETD (in minutes) to be used if it is not provided by the store
settings.minimumDeliveryOrder = 500.0; // Minimum order price for delivery orders in pence
settings.supportEmail = '';

/* Store selector */
settings.storeSelectorMode = false; // Set to true to show a page prompting the customer to select a postcode which is used to select which store to order from (or false)
settings.storeSelectorInHeader = false;

/* Social */
settings.socialMediaEnabled = true;
settings.useAddThis = true; // Use addthis for the facebook/twitter buttons
settings.twitter = 'mt'; // Url to the customers twitter page
settings.facebook = 'mf'; // Url to the customers facebook page
settings.instagram = 'mi'; // Url to the customers instagram page - blank disables
settings.pinterest = ''; // Url to the customers pinterest page - blank disables
settings.facebookLikeUrl = 'mf'; // Facebook like

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
settings.returnToParentWebsiteAfterOrder = false;
settings.parentWebsite = 'undefined';
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
settings.googleAnalyticsId = '';
    
/* Trip advisor */
settings.tripAdvisorIsEnabled = false;

/* Home page */
textStrings.homeStoreDetailsTitle = 'blah';
textStrings.homeStoreDetails = '<p>Bacon ipsum dolor amet turducken tongue t-bone rump short loin pig.  Flank andouille jerky porchetta chicken ground round, venison tail beef ribs beef sausage boudin brisket chuck bacon.  Rump t-bone short ribs, prosciutto pork chop chuck cow.  Jowl cow chuck beef ribs shankle shank ribeye chicken sausage alcatra beef.  Tongue cupim prosciutto pork.  Sirloin t-bone bresaola doner kevin.</p><p>Meatloaf picanha pig spare ribs shankle, kielbasa pork chicken pastrami fatback kevin sausage ground round pork loin tenderloin.  Frankfurter ribeye cupim fatback andouille pork loin pork meatloaf filet mignon porchetta corned beef salami venison.  Cupim pork belly corned beef filet mignon, andouille sausage pork chop pancetta pig brisket.  Jerky bacon short loin short ribs spare ribs t-bone, tenderloin rump cow ham hock.  Meatball chuck turducken pancetta ham hock kevin.  Bresaola picanha spare ribs salami doner venison drumstick turducken pastrami tri-tip turkey pig capicola short loin pork belly.  Strip steak turkey t-bone leberkas doner, jowl biltong tail filet mignon meatball short loin pig chuck.</p>';
textStrings.homeMenuTitle = 'blah';
textStrings.homeMenu = '<p>Pastrami corned beef beef prosciutto bresaola landjaeger shank ball tip jerky kevin tenderloin strip steak.  T-bone jerky turkey pastrami landjaeger beef ribs alcatra rump.  Alcatra short loin pastrami beef landjaeger t-bone tail hamburger brisket.  Jerky ball tip tenderloin, alcatra short loin leberkas pig jowl pancetta kielbasa fatback bresaola landjaeger venison.  Ball tip tri-tip fatback, biltong picanha capicola andouille pastrami turducken short loin pancetta venison meatloaf beef ribs.  Swine pastrami boudin, rump picanha leberkas beef shoulder fatback ground round biltong kevin.</p><p>Hamburger pancetta cupim tri-tip.  Fatback ham strip steak sirloin pork loin.  Pancetta pork chop andouille frankfurter corned beef ribeye t-bone swine chuck bresaola meatloaf prosciutto fatback.  Ham pig beef, boudin pork chop capicola rump corned beef sausage andouille chuck doner brisket.  Jowl short ribs strip steak, capicola ham kevin pig spare ribs pastrami meatball beef ribs cow.</p>';
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
        { type: 'Image', url: 'http://cdn.myandromedaweb.co.uk/dev/websites/29/carousels/featured/a8baf8cb-5fe6-4113-45f4-251823e99b71_1022x300.png' }
,{ type: 'Image', url: 'http://cdn.myandromedaweb.co.uk/dev/websites/29/carousels/featured/ee86bcf6-30d1-4e92-0a29-c0068f65f850_1022x300.png' }
,{ type: 'Image', url: 'http://cdn.myandromedaweb.co.uk/dev/websites/29/carousels/featured/2ce170f3-91cd-4215-4ddf-785ed36eed4e_1022x300.png' }
,{ type: 'Image', url: 'http://cdn.myandromedaweb.co.uk/dev/websites/29/carousels/featured/e8fbad32-14d0-455d-a5e0-14984e76125c_1022x300.png?k=75150' }
,{ type: 'Image', url: 'http://cdn.myandromedaweb.co.uk/dev/websites/29/carousels/featured/b5ce80a6-d447-47dc-d520-66e52ff3e72e_1022x300.png?k=53777' }

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

settings.pages = [
    { "Title": "Minions", "Content": "<img alt=\"\" src=\"http://p1.pichost.me/i/59/1835207.jpg\" width=\"800\" />", "Enabled": true },
    { "Title": "CMS Test no imagination", "Content": "<h1>Sample text</h1><p></p><h2><span style=\"text-decoration:underline;\"><em>Test bunnies</em></span></h2><p></p><h3>Holla</h3><p></p><p><em><span style=\"text-decoration:underline;\"><strong>what is this about</strong></span></em></p><p><em><span style=\"text-decoration:underline;\"><strong>hyperlink to a random page</strong></span></em></p><p style=\"text-align:left;\"><em><span style=\"text-decoration:underline;\"><strong>this is very good</strong></span></em></p><p style=\"text-align:left;\"><em><span style=\"text-decoration:underline;\"><strong>thank you Matthew Green</strong></span></em></p><p style=\"text-align:left;\"><em><span style=\"text-decoration:underline;\"><strong>you are a wonderful developer</strong></span></em></p><p style=\"text-align:left;\"><em><span style=\"text-decoration:underline;\"><strong>a kind person</strong></span></em></p><p style=\"text-align:left;\"><em><span style=\"text-decoration:underline;\"><strong>a stimulating conversational partner</strong></span></em></p><p style=\"text-align:left;\"><em><span style=\"text-decoration:underline;\"><strong>a trampolining aficionado </strong></span></em></p><p style=\"text-align:left;\"><em><span style=\"text-decoration:underline;\"><strong>and many other things I don't know about</strong></span></em></p><ol><li style=\"text-align:left;\"><em>ertt</em></li><li><em><span style=\"text-decoration:underline;\"><strong>ertyy</strong></span></em></li><li><em><span style=\"text-decoration:underline;\"><strong>ertyy</strong></span></em></li><li>643235</li></ol><p></p><p><img alt=\"this is my lady bunny\" height=\"300\" src=\"http://ct.iscute.com/ol/ic/sw/i57/2/3/20/ic_31479bd722864c6c931106f9f0baf6e3.jpg\" style=\"display:block;margin-left:auto;margin-right:auto;\" width=\"200\" /></p>", "Enabled": true },
    { "Title": "minion 234523709307u%**#@#*", "Content": "<img alt=\"minion\" height=\"750\" src=\"http://vignette3.wikia.nocookie.net/non-aliencreatures/images/c/c1/Minion.png/revision/latest?cb=20141213101930\" width=\"400\" />", "Enabled": false }
];