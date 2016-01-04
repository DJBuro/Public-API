/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

// Translation strings
var textStrings = undefined;

// TEMP FUDGE
var loyaltyHelper =
    {
        IsEnabled: ko.observable(false)
    };

// This object is data bound to the HTML
function ViewModel()
{
    "use strict";

    var self = this;

    self.getDefaultOrderType = function ()
    {
        if (settings.enableDelivery) return 'delivery';
        else if (settings.enableCollection) return 'collection';
        else if (settings.enableDineIn) return 'dinein';
        else return 'Delivery'; // Something isn't right!
    };

    /* The version of the AndroWeb API that this version requires */
    self.serverUrl = location.protocol + '//' + location.host + '/Services/WebOrdering_2_2_0_0/webordering';

    self.headerViewModel = ko.observable(undefined);

    // Previous view & view model
    self.previousContentViewModel = '';
    self.previousViewName = '';

    // Current view & view model
    self.contentViewModel = ko.observable(undefined);    
    self.viewName = ko.observable(undefined);

    self.isMobileMode = guiHelper.isMobileMode;

    self.queryString = {};

    self.pageManager = undefined;
    self.telemetry = undefined;

    self.mobileMenuViewModel = null;

    self.initialised = ko.observable(false);
    self.initialise = function ()
    {
        ko.setTemplateEngine(new ko.nativeTemplateEngine());

        // Check if the debug culture has been specified and highlight all internationalised text so we can see what has been missed
        this.checkForDebugCulture();

        // Fix for mobile absolute positioning click through bug
        $('#popupMobileMenu').on('click', function (e) { e.stopPropagation(); return false; });

        // We need to know when the window is resized
        $(window).resize(guiHelper.resize);

        // We need to know when the user browses away from the website
        window.onbeforeunload = self.closeSiteWarning;

        // We need to know when the user scrolls
        $(window).scroll(guiHelper.onWindowScroll);

        // Start listening to messages
        window.addEventListener
        (
            'message',
            function (e)
            {
                checkoutHelper.onMessageEvent(e.data);
            },
            false
        );

        // Are we on a mobile device?
        guiHelper.refreshIsMobileMode();

        // Parse the query string
        queryStringHelper.initialise();

        // Should the customer details always be passed in?
        if (settings.requireCustomerDetailsPassthrough && settings.parentWebsite.length > 0)
        {
            // Were the customer details actually passed in?
            if (queryStringHelper.externalSiteId == undefined || queryStringHelper.externalSiteId.length == 0 ||
                queryStringHelper.firstName == undefined || queryStringHelper.firstName.length == 0 ||
                queryStringHelper.lastName == undefined || queryStringHelper.lastName.length == 0 ||
                queryStringHelper.telephoneNumber == undefined || queryStringHelper.telephoneNumber.length == 0 ||
                queryStringHelper.postcode == undefined || queryStringHelper.postcode.length == 0 ||
                queryStringHelper.roadName == undefined || queryStringHelper.roadName.length == 0 ||
                queryStringHelper.town == undefined || queryStringHelper.town.length == 0)
            {
                helper.gotoParentWebsite();
                return;
            }
        }

        // Initialise the page history
        self.pageManager = new AndroWeb.pageManager();
        self.pageManager.initialise();

        // Start the app
        setTimeout
        (
            function ()
            {
                // Initialise view model
                viewModel.headerViewModel(new HeaderViewModel());
                viewModel.contentViewModel(new DefaultViewModel());

                // Initialise loyalty for this store
                loyaltyHelper.initialise(viewModel);

                // Initialise the mobile menu
                self.mobileMenuViewModel = new MobileMenuViewModel()

                // Data bind the HTML to the view model
                ko.applyBindings(viewModel, $('header')[0]);
                ko.applyBindings(self.mobileMenuViewModel, $('#mobileMenu')[0]);
                ko.applyBindings(viewModel, $('#bindToElements')[0]);

                if (queryStringHelper.passwordReset != undefined)
                {
                    guiHelper.isMenuVisible(false);
                    guiHelper.showView('passwordResetView', new PasswordResetViewModel());
                }
                else
                {
                    viewModel.chooseStore();
                }
            },
            0
        );

        self.initialised(true);
    };
    self.checkForDebugCulture = function ()
    {
        if (settings.culture === "debug")
        {
            for (var property in textStrings)
            {
                if (textStrings.hasOwnProperty(property))
                {
                    textStrings[property] = helper.repeat("Z", property.length);
                }
            }
        }
    };
    self.pleaseWaitMessage = ko.observable('');
    self.pleaseWaitProgress = ko.observable(undefined);
    self.sitesMode = ko.observable('pleaseWait'); // The mode that the 'sites' page is currently in
    self.sites = ko.observableArray(); // A list of sites (from the site list web service call)
    self.deliverySites = ko.observableArray();
    self.collectionOnlySites = ko.observableArray();
    self.selectedSite = ko.observable(); // The selected site (from the site list web service call)
    self.siteDetails = ko.observable(); // The selected site (from the siye details web service call)
    self.sitesError = ko.observable(false); // True when there is a problem getting the site details or menu from the server
    self.error =
    {
        message: ko.observable(), // The error message to display
        showReturn: ko.observable(false), // Show/hide the ok button
        returnCallback: undefined // A function to call if the ok button is clicked on the error page
    };
    self.displayAddress = ko.observable(); // The stores address for display purposes
    self.displayAddressMultiLine = ko.observable(); // The stores address for display purposes
    self.chooseStore = function (siteID, storeName)
    {
        // Gets a list of stores from the server. If there is more than one store then prompts the user to select one.
        // Then gets the store details from the server and if required downloads the menu

        try
        {
            // Clear out the existing menu
            viewModel.menu = undefined;
            
            // Empty out the cart
            AndroWeb.Helpers.CartHelper.clearCart();

            // Do we already have a site list?
            if (viewModel.sites() !== undefined && viewModel.sites().length > 0)
            {
                viewModel.gotSiteList();
            }

            // Show the please wait view
            guiHelper.showPleaseWait
            (
                textStrings.gettingStoreDetails,
                undefined,
                function ()
                {
                    // Clear out the store details
                    viewModel.siteDetails(undefined);

                    // Refresh the site list from the server
                    acsapi.getSiteList
                    (
                        function ()
                        {
                            // Mark all the offline stores for display purposes
                            helper.markOfflineStores();

                            viewModel.gotSiteList();
                        },
                        function ()
                        {
                            viewModel.chooseStore();
                        }
                    );
                }
            );
        }
        catch (exception)
        {
            // Got an error
            viewHelper.showError(textStrings.errInternalApplicationError, viewModel.chooseStore, exception);
        }
    };
    self.gotSiteList = function()
    {
        var callback = undefined;
                                
        // Has a store id been passed in?
        var selectedSite = undefined;
        if (queryStringHelper.externalSiteId != undefined || 
            queryStringHelper.storeName != undefined)
        {
            if (viewModel.sites() != undefined)
            {
                for (var siteIndex = 0; siteIndex < viewModel.sites().length; siteIndex++)
                {
                    var checkSite = viewModel.sites()[siteIndex];

                    if (//checkSite.siteId === queryStringHelper.externalSiteId ||
                        checkSite.name.toUpperCase() === decodeURI(queryStringHelper.storeName).toUpperCase())
                    {
                        // Found the selected site
                        selectedSite = checkSite;

                        break;
                    }
                }
            }
        }
                                
        if (queryStringHelper.pageName != undefined)
        {
            callback = viewModel.pageManager.showPage;
        }
                                
        if (selectedSite != undefined)
        {
            // Auto select the only site
            viewModel.selectedSite(selectedSite);

            // Get the store details and menu
            guiHelper.downloadAndShowStoreMenu();
        }
        else if (viewModel.sites() === undefined || viewModel.sites().length === 0)
        {
            // No stores!!
            viewHelper.showError
            (
                'No stores!',
                function ()
                { 
                    viewModel.chooseStore();
                },
                undefined
            );
        }
        else if (viewModel.sites().length == 1)
        {
            // Auto select the only site
            viewModel.selectedSite(viewModel.sites()[0]);

            // Get the store details and menu
            guiHelper.downloadAndShowStoreMenu();
        }
        else if (viewModel.sites().length > 1)
        {
            viewModel.pageManager.showPage("home", true);
        }
    },
    self.storeSelected = function (store)
    {
        viewModel.selectedSite(store);

        guiHelper.downloadAndShowStoreMenu();
    };
    self.isTakingOrders = ko.observable(false);
    self.menu = undefined;
    self.menuViewName = ko.observable('');
    self.sections = ko.observableArray();
    self.deals = ko.observableArray();
    self.visibleSection = ko.observable('');
    self.pickToppings = ko.observable(false);
    self.selectedItem =
    {
        menuItem: ko.observable(undefined),
        menuItems: ko.observableArray(),
        name: ko.observable(''),
        description: ko.observable(''),
        quantity: ko.observable(1),
        price: ko.observable(undefined),
        category1: ko.observable(undefined),
        category2: ko.observable(undefined),
        freeToppings: ko.observable(0),
        freeToppingsRemaining: ko.observable(0),
        toppings: ko.observableArray(),
        instructions: ko.observable(''),
        person: ko.observable(''),
        category1s: ko.observableArray(),
        category2s: ko.observableArray(),
        selectedCategory1: ko.observable(),
        selectedCategory2: ko.observable(),
        thumbnail: ko.observable()
    };
    self.orderType = ko.observable(self.getDefaultOrderType());
    self.isDineInMode = ko.observable(false);
    self.setDineInMode = function (newDineInMode)
    {
        var notDineIn = true;

        var isDineInCurrentlySelected = viewModel.isDineInMode();

        // Is the dine in order type allowed?  
        // Cos if it aint then you can't switch to dine in mode
        if (settings.enableDineIn === true)
        {
            guiHelper.lockToOrderType('dinein');

            viewModel.isDineInMode(newDineInMode);

            if (viewModel.isDineInMode())
            {
                // Switch to dine in
                guiHelper.canChangeOrderType(false);
                viewModel.orderType('dinein');

                notDineIn = false;
            }
        }

        if (notDineIn)
        {
            // Disable dine in mode
            guiHelper.lockToOrderType(undefined);
            guiHelper.canChangeOrderType(true);
            viewModel.orderType(self.getDefaultOrderType());
        }

        if (isDineInCurrentlySelected != viewModel.isDineInMode())
        {
            AndroWeb.Helpers.CartHelper.orderTypeChanged();
        }
    };
    self.maxSectionHeight = ko.observable(0);
    self.changeSection = function ()
    {
        var section = undefined;

        var showMenuSectionId = 0;
        if (this != undefined && this.Index != undefined)
        {
            if (this.display != undefined)
            {
                section = this;
            }
            else
            {
                showMenuSectionId = this.Index;
            }
        }
        else if (viewModel.selectedSection() != undefined)
        {
            section = viewModel.selectedSection();
        }
        
        if (section !== undefined)
        {
            viewModel.pageManager.showPage('Menu/' + section.display.Name, true, undefined, false);
        }
        
        return false;
    };

    self.mobileAddItemToCart = function ()
    {
        if (guiHelper.isMobileMode())
        {
            self.onAddItemToCart(this, true);
        }
    };

    self.addItemToCart = function ()
    {
        self.onAddItemToCart(this, false);
    }
    self.addCustomItemToCart = function()
    {
        self.onAddItemToCart(this, true);
    }
    self.addItemToCart = function (context)
    {
        self.onAddItemToCart(context, false);
    }
    self.addCustomItemToCart = function (context)
    {
        self.onAddItemToCart(context, true);
    }

    self.onAddItemToCart = function (context, customize)
    {
        if (context.isEnabled() === false) return;
        if (viewModel.isTakingOrders() === false) return;
        if (context.isNotAvailableForRestOfDay() === true) return;

        // Get the menu item that the user has selected
        var menuItem = menuHelper.getSelectedMenuItem(context);

        if (menuItem != undefined && menuItem != null)
        {
            var cartItem = new AndroWeb.Models.CartItem
            (
                viewModel,
                context,
                menuItem,
                false
            );
            cartItem.recalculatePrice();

            // Does the customer need to pick any toppings?
            if (customize || guiHelper.isMobileMode() || settings.alwaysShowToppingsPopup)
            {
                // Show the toppings popup
                toppingsPopupHelper.returnToCart = false;

                toppingsPopupHelper.showPopup(cartItem);
            }
            else
            {
                // Do we need to increase the quantity of an existing cart item or add it as a new cart item?
                var isNewCartItem = true;
                var existingCartItem = undefined;
                if (cartItem.toppings().length == 0)
                {
                    if (AndroWeb.Helpers.CartHelper.cart() !== undefined)
                    {
                        //observable 
                        var cart = AndroWeb.Helpers.CartHelper.cart();
                        //observable array
                        var cartItems = cart.cartItems();

                        for (var cartIndex = 0; cartIndex < cartItems.length; cartIndex++)
                        {
                            var existingCartItem = cartItems[cartIndex];

                            //existing cart items' menuItemWrapper changes with the menu item's changes to cat1 & cat2
                            var isTheCurrentItemId = existingCartItem.menuItem.Id === menuItem.Id;

                            if (isTheCurrentItemId)
                            {
                                existingCartItem.quantity(Number(existingCartItem.quantity()) + Number(context.quantity()));
                                existingCartItem.displayName(menuHelper.getCartItemDisplayName(existingCartItem));
                                isNewCartItem = false;
                                break;
                            }
                        }
                    }
                }

                if (isNewCartItem)
                {
                    // Add the the cart
                    toppingsPopupHelper.commitToCart(cartItem);
                }
                else
                {
                    // We've already increased the cart item quantity - refresh the cart
                    AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());
                }

                // If we are in mobile mode show the cart
                if (guiHelper.isMobileMode())
                {
                    viewHelper.showCart();
                }
            }
        }
    };
    self.selectedItemChanged = function ()
    {
        if (!viewModel.ignoreEvents)
        {
            guiHelper.selectedItemChanged(this);
        }
    };
    self.category1Changed = function (itemWrapper)
    {
        if (!viewModel.ignoreEvents && itemWrapper != undefined)
        {
            var previousSelectedCategory2 = itemWrapper.selectedCategory2();

            // Clear the category2 list
            itemWrapper.category2s.removeAll();

            var result = menuHelper.rebuildCategory2List(itemWrapper, itemWrapper.selectedCategory1(), previousSelectedCategory2);

            itemWrapper.category2s(result.category2s);
            itemWrapper.selectedCategory2(result.selectedCategory2);

            // Figure out which menu item the categories relate to
            guiHelper.selectedItemChanged(itemWrapper);
        }
    };
    self.currentSectionIndex = 0;
    self.changeDeliveryTime = function ()
    {
    };
    self.timer = undefined;
    self.addressType = ko.observable();
    self.showCheckoutSection = function (data)
    {
        checkoutHelper.showCheckoutSection(data.index);
    };
    self.selectedSection = ko.observable(undefined); // Used for the mobile section selector
    self.ignoreEvents = false;
    self.etd = function ()
    {
        var etd = settings.defaultETD;
        if (viewModel.selectedSite() !== undefined &&
            viewModel.selectedSite().estDelivTime !== undefined &&
            viewModel.selectedSite().estDelivTime > 0)
        {
            etd = viewModel.selectedSite().estDelivTime;
        }

        return etd;
    };
    self.ect = function ()
    {
        var ect = settings.defaultECT;
        if (viewModel.selectedSite() !== undefined &&
            viewModel.selectedSite().estCollTime !== undefined &&
            viewModel.selectedSite().estCollTime > 0)
        {
            ect = viewModel.selectedSite().estCollTime;
        }

        return ect;
    };
    self.resetOrderType = function ()
    {
        // If we're in dine in mode then the order type can't be changed
        if (viewModel.isDineInMode())
        {
            viewModel.setDineInMode(true); // Just to be sure
            return;
        }

        // Was the order type passed in through the url?
        if (queryStringHelper.orderType == undefined)
        {
            guiHelper.canChangeOrderType(true);

            self.getDefaultOrderType();

            //if (settings.enableDelivery)
            //{
            //    viewModel.orderType('delivery');
            //}
            //else if (settings.enableCollection)
            //{
            //    viewModel.orderType('collection');
            //}
        }
        else
        {
            // Force the order type to be whatever was passed in through the url
            if (queryStringHelper.orderType.toUpperCase() == 'D')
            {
                viewModel.orderType('delivery');
                guiHelper.canChangeOrderType(false);
            }
            else if (queryStringHelper.orderType.toUpperCase() == 'C')
            {
                viewModel.orderType('collection');
                guiHelper.canChangeOrderType(false);
            }
            else
            {
                // Whatever they passed through isn't supported so assume the default
                viewModel.orderType('delivery');
                guiHelper.canChangeOrderType(true);
            }
        }

        AndroWeb.Helpers.CartHelper.ignoreOrderTypeChangeEvents = false;
        menuHelper.updatePrices();
        menuHelper.refreshDealsAvailabilty();
    };
    self.returnToHostWebsite = function ()
    {
        window.onbeforeunload = undefined;
        helper.gotoParentWebsite();
    };
    self.closeSiteWarning = function ()
    {
        var warningMessage = undefined;

        // Are there any items in the cart?
        if (AndroWeb.Helpers.CartHelper.cart().hasItems())
        {
            warningMessage = textStrings.gExitWarning;
        }

        return warningMessage;
    };
};

var viewModel = new ViewModel();




































