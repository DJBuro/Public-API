/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

//(function ($)
//{
    
//}
//)(jQuery);

// This object is data bound to the HTML
var viewModel =
{
    queryString: {},
    initialise: function ()
    {
        ko.setTemplateEngine(new ko.nativeTemplateEngine());

        // Fix for mobile absolute positioning click through bug
        $('#popupMobileMenu').on('click', function (e) { e.stopPropagation(); return false; });

        $(window).resize(guiHelper.resize);

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
                // Are there any cached customer details?
  //              cacheHelper.cacheObject(accountHelper.customerDetails);

                window.location.href = settings.parentWebsite;
                return;
            }
            //else
            //{
            //    // Save for later
            //    accountHelper.customerDetails =
            //    {
            //        firstname: queryStringHelper.firstName == undefined ? '' : queryStringHelper.firstName,
            //        surname: queryStringHelper.lastName == undefined ? '' : queryStringHelper.lastName,
            //        emailAddress: queryStringHelper.email == undefined ? '' : queryStringHelper.email,
            //        telephoneNumber: queryStringHelper.telephoneNumber == undefined ? '' : queryStringHelper.telephoneNumber,
            //        marketing: queryStringHelper.marketing == undefined ? '' : queryStringHelper.marketing,
            //        address: 
            //        {
            //            roadName: queryStringHelper.roadName == undefined ? '' : queryStringHelper.roadName,
            //            roadNumber: queryStringHelper.houseNumber == undefined ? '' : queryStringHelper.houseNumber,
            //            town: queryStringHelper.town == undefined ? '' : queryStringHelper.town,
            //            city: queryStringHelper.town == undefined ? '' : queryStringHelper.town,
            //            zipCode: queryStringHelper.postcode == undefined ? '' : queryStringHelper.postcode
            //        }
            //    };

            //    var contacts = [];
            //    contacts.push({ type: 'Mobile', value: queryStringHelper.telephoneNumber });

            //    if (queryStringHelper.email != undefined && queryStringHelper.email.length > 0)
            //    {
            //        contacts.push({ type: 'Email', value: queryStringHelper.email });
            //    }

            //    // Cache the customer details
            //    cacheHelper.cacheObject(accountHelper.customerDetails);
            //}
        }

        // Load the correct text strings
 //       viewModel.loadTextStrings();

        setTimeout
        (
            function ()
            {
                // Initialise error messages
                viewModel.defaultWebErrorMessage = textStrings.defaultWebErrorMessage;
                viewModel.defaultInternalErrorMessage = textStrings.defaultInternalErrorMessage;
                viewModel.defaultPaymentErrorMessage = textStrings.defaultPaymentErrorMessage;

                // Data bind the HTML to the view model
                ko.applyBindings(viewModel);

                // Get a list of sites
                viewModel.chooseStore();
            },
            0
        );
    },
    //loadTextStrings: function()
    //{
    //    // Load the correct text strings
    //    $('#resourceStrings').html('<script type="text/javascript" src="ResourceStrings/' + settings.culture + '.js"></script>');
    //},
    serverUrl: 'http://' + location.host + '/Services/WebOrdering/webordering',
    viewName: ko.observable(undefined),
    pleaseWaitMessage: ko.observable(''),
    pleaseWaitProgress: ko.observable(undefined),
    sitesMode: ko.observable('pleaseWait'), // The mode that the 'sites' page is currently in
    sites: ko.observableArray(), // A list of sites (from the site list web service call)
    deliverySites: ko.observableArray(),
    collectionOnlySites: ko.observableArray(),
    selectedSite: ko.observable(), // The selected site (from the site list web service call)
    siteDetails: ko.observable(), // The selected site (from the siye details web service call)
    sitesError: ko.observable(false), // True when there is a problem getting the site details or menu from the server
    error:
    {
        message: ko.observable(), // The error message to display
        showReturn: ko.observable(false), // Show/hide the ok button
        returnCallback: undefined // A function to call if the ok button is clicked on the error page
    },
    displayAddress: ko.observable(), // The stores address for display purposes
    displayAddressMultiLine: ko.observable(), // The stores address for display purposes
    chooseStore: function ()
    {
        // Gets a list of stores from the server. If there is more than one store then prompts the user to select one.
        // Then gets the store details from the server and if required downloads the menu

        try
        {
            // Clear out the existing menu
            viewModel.menu = undefined;

            // Empty out the cart
            cartHelper.clearCart();

            // No mobile menu on this page
            guiHelper.isMobileMenuVisible(false);

            if (settings.storeSelectorMode)
            {
                // Show the postcode textbox
                siteSelectorHelper.isPostcodeTextboxVisible(true);

                // Show the store selector page
                guiHelper.showView('siteSelectorView');
            }
            else
            {
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

                                // Has a store id been passed in?
                                var selectedSite = undefined;
                                if (queryStringHelper.externalSiteId != undefined)
                                {
                                    if (viewModel.sites() != undefined)
                                    {
                                        for (var siteIndex = 0; siteIndex < viewModel.sites().length; siteIndex++)
                                        {
                                            var checkSite = viewModel.sites()[siteIndex];
                                            if (checkSite.siteId == queryStringHelper.externalSiteId)
                                            {
                                                // Found the selected site
                                                selectedSite = checkSite;

                                                break;
                                            }
                                        }
                                    }
                                }

                                if (selectedSite != undefined)
                                {
                                    // Auto select the only site
                                    viewModel.selectedSite(selectedSite);

                                    // Get the store details and menu
                                    guiHelper.showStoreMenu();
                                }
                                else if (viewModel.sites() === undefined || viewModel.sites().length === 0)
                                {
                                    // No stores!!
                                    guiHelper.showError
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
                                    guiHelper.showStoreMenu();
                                }
                                else
                                {
                                    // Show the site list view
                                    guiHelper.showView('sitesView');
                                }
                            }
                        );
                    }
                );
            }
        }
        catch (exception)
        {
            // Got an error
            guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.chooseStore, exception);
        }
    },
    storeSelected: function (store)
    {
        siteSelectorHelper.isPostcodeTextboxVisible(false);

        viewModel.selectedSite(store);

        guiHelper.showStoreMenu();
    },
    isTakingOrders: ko.observable(false),
    menu: undefined,
    menuViewName: ko.observable(''),
    sections: ko.observableArray(),
    deals: ko.observableArray(),
    visibleSection: ko.observable(''),
    pickToppings: ko.observable(false),
    selectedItem:
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
        price: ko.observable(undefined)
    },
    orderType: ko.observable('delivery'),
    maxSectionHeight: ko.observable(0),
    changeSection: function ()
    {
        if (this != undefined && this.Index != undefined)
        {
            guiHelper.showSection(this.Index);
        }
        else if (viewModel.selectedSection() != undefined)
        {
            guiHelper.showSection(viewModel.selectedSection().Index);
        }

        $('#mobileMenuSectionsSelect').blur();

        return true;
    },
    addItemToCart: function ()
    {
        // Get the menu item that the user has selected
        var menuItem = menuHelper.getSelectedMenuItem(this);

        if (menuItem != null)
        {
            // Get the categories
            var category1 = helper.findById(menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1, viewModel.menu.Category1);
            var category2 = helper.findById(menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2, viewModel.menu.Category2);

            // The item to show on the toppings popup
            viewModel.selectedItem.name(this.name);

            // Copy over the menu items
            viewModel.selectedItem.menuItems.removeAll();
            for (var index = 0; index < this.menuItems().length; index++)
            {
                viewModel.selectedItem.menuItems.push(this.menuItems()[index]);
            }

            viewModel.selectedItem.description(this.description);
            viewModel.selectedItem.quantity(this.quantity);
            viewModel.selectedItem.menuItem(menuItem);
            viewModel.selectedItem.freeToppings(menuItem.FreeTops);
            viewModel.selectedItem.freeToppingsRemaining(menuItem.FreeTops);
            viewModel.selectedItem.instructions('');
            viewModel.selectedItem.person('');
            viewModel.selectedItem.price(helper.formatPrice(this.price()));

            // Do these last becuase the UI is data bound to them
            viewModel.ignoreEvents = true;
            viewModel.selectedItem.category1s.removeAll();
            for (var index = 0; index < this.category1s().length; index++)
            {
                viewModel.selectedItem.category1s.push(this.category1s()[index]);
            }
            viewModel.selectedItem.category2s.removeAll();
            for (var index = 0; index < this.category2s().length; index++)
            {
                viewModel.selectedItem.category2s.push(this.category2s()[index]);
            }
            viewModel.ignoreEvents = false;

            viewModel.selectedItem.category1(category1);
            viewModel.selectedItem.category2(category2);
            viewModel.selectedItem.selectedCategory1(this.selectedCategory1());
            viewModel.selectedItem.selectedCategory2(this.selectedCategory2());

            // Get the toppings
            viewModel.selectedItem.toppings(menuHelper.getItemToppings(viewModel.selectedItem.menuItem()));

            // Calculate the price
            var price = menuHelper.calculateItemPrice(menuItem, this.quantity, viewModel.selectedItem.toppings());

            // Set the price
            viewModel.selectedItem.price(helper.formatPrice(price));

            // Does the customer need to pick any toppings?
            if (viewModel.selectedItem.toppings().length == 0)
            {
                // Add the the cart
                popupHelper.commitToCart();

                // If we are in mobile mode show the cart
                if (guiHelper.isMobileMode())
                {
                    guiHelper.showCart();
                }
            }
            else
            {
                // Show the toppings popup
                popupHelper.returnToCart = false;
                popupHelper.showPopup('addItem');
            }
        }
    },
    selectedItemChanged: function ()
    {
        if (!viewModel.ignoreEvents)
        {
            guiHelper.selectedItemChanged(this);
        }
    },
    category1Changed: function (item)
    {
        if (!viewModel.ignoreEvents && item != undefined)
        {
            // Clear the category2 list
            item.category2s.removeAll();

            // Rebuild the category2 list based on the selected category1
            for (var index = 0; index < item.menuItems().length ; index++)
            {
                var checkItem = item.menuItems()[index];

                // Does the menu item have the selected category1?
                if (item.selectedCategory1().Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1))
                {
                    // Is this items category 2 already in the list?
                    var category = helper.findById(checkItem.Cat2 == undefined ? checkItem.Category2 : checkItem.Cat2, viewModel.menu.Category2);
                    if (category != undefined && !helper.findCategory(category, item.category2s))
                    {
                        item.category2s.push(category);
                    }
                }
            }

            // Make sure the currently selected cat2 is in the new cat2 list
            if (item.category2s().length > 0)
            {
                var ok = false;
                for (var index = 0; index < item.category2s().length; index++)
                {
                    var cat2 = item.category2s()[index];
                    if (cat2.Id == item.selectedCategory2().Id)
                    {
                        ok = true;
                        break;
                    }
                }

                if (!ok)
                {
                    // The currently selected cat2 is no longer valid so we need to pick a valid one
                    item.selectedCategory2(item.category2s()[0]);
                }
            }
            else
            {
                item.selectedCategory2(undefined);
            }

            // Figure out which menu item the categories relate to
            guiHelper.selectedItemChanged(item);
        }
    },
    currentSectionIndex: 0,
    changeDeliveryTime: function ()
    {
    },
    timer: undefined,
    addressType: ko.observable(),
    showCheckoutSection: function (data)
    {
        checkoutHelper.showCheckoutSection(data.index);
    },
    selectedSection: ko.observable(undefined), // Used for the mobile section selector
    ignoreEvents: false,
    etd: function ()
    {
        var etd = settings.defaultETD;
        if (viewModel.selectedSite().estDelivTime != undefined && viewModel.selectedSite().estDelivTime > 0)
        {
            etd = viewModel.selectedSite().estDelivTime;
        }

        return etd;
    },
    resetOrderType: function ()
    {
        // Was the order type passed in through the url?
        if (queryStringHelper.orderType == undefined)
        {
            guiHelper.canChangeOrderType(true);
            viewModel.orderType('delivery');
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
    },
    returnToHostWebsite: function ()
    {
        window.location.href = settings.parentWebsite;
    }
};

var guiHelper =
{
    isMainMenuVisible: ko.observable(false),
    isCartLocked: ko.observable(false),
    isMobileMenuVisible: ko.observable(true),
    canChangeOrderType: ko.observable(false),
    cartActionsCheckout: 'Checkout',
    cartActionsPayment: 'Payment',
    cartActionsMenu: 'Menu',
    cartActions: ko.observable('Menu'),
    isMobileMode: ko.observable(false),
    refreshIsMobileMode: function ()
    {
        var newPageWidth = $(window).width();

        if (Number(newPageWidth) < settings.mobileWidth)
        {
            guiHelper.isMobileMode(true);

            $('body').on("touchstart", function (event)
            {
                event.stopPropagation();
                $(this).click();
            });
        }
        else
        {
            guiHelper.isMobileMode(false);

            // Sliding menu not supported in mobile mode
            settings.useSlideMenu = false;
        }
    },
    isMenuBuilt: false,
    isViewVisible: ko.observable(true),
    isMenuVisible: ko.observable(false),
    isInnerMenuVisible: ko.observable(false),
    showHome: function ()
    {
        mobileMenuHelper.title(textStrings.mmHome);
        mobileMenuHelper.titleClass('popupMobileMenuHomeButton');

        guiHelper.isMobileMenuVisible(true);
        guiHelper.showView('homeView');
        viewModel.resetOrderType();

        // Let knockout do its thing
        setTimeout
        (
            function ()
            {
                // Initialise the map
                mapHelper.initialiseMap();
            },
            0
        );
    },
    showCart: function ()
    {
        $(window).scrollTop(0);
        guiHelper.showView('cartView');
    },
    showPostcodeCheck: function ()
    {
        guiHelper.showView('postcodeCheckView');

        guiHelper.isMobileMenuVisible(true);
    },
    showSection: function (sectionIndex, callback)
    {
        //try
        //{
        if (sectionIndex != undefined)
        {
            sectionIndex = typeof (sectionIndex) == 'number' ? sectionIndex : 0;

            var section = viewModel.sections()[sectionIndex];

            viewModel.selectedSection(section);

            viewModel.currentSectionIndex = sectionIndex;
            viewModel.visibleSection(section.display.Name);


            for (var index = 0; index < viewModel.sections().length; index++)
            {
                if (index == sectionIndex)
                {
                    // This section should be visible
                    $('#section' + index).css('display', 'block');
                }
                else
                {
                    // Hide this section
                    $('#section' + index).css('display', 'none');
                }
            }

            $('#sections').scrollTop(0);
        }
        else
        {
            if (callback != undefined) callback();
        }

        //}
        //catch (exception)
        //{
        //    alert(exception);
        //}
    },
    showView: function (viewName)
    {
        viewModel.viewName(viewName);

        // Do we need to show the menu?  This is a bit of a fudge - the menu used to be a view
        // but using a template to hide and show it triggers a shed load of knockout events which
        // causes a several second delay on the stock Android (and possibly other) browser.
        // Instead, we do the slow menu bindings once and and hide/show it
        if (viewName == 'menuView')
        {
            // Hide the view
            guiHelper.isViewVisible(false);

            // Show the menu
            guiHelper.isMenuVisible(true);
        }
        else
        {
            // Hide the menu
            guiHelper.isMenuVisible(false);
            guiHelper.isInnerMenuVisible(false);

            // Show the view
            guiHelper.isViewVisible(true);
        }

        $(window).scrollTop(0);
    },
    showMenuView: function (viewName)
    {
        // Hide the view
        guiHelper.isViewVisible(false);

        // Show the menu
        guiHelper.isMenuVisible(true);

        if (viewName == 'menuSectionsView')
        {
            guiHelper.isInnerMenuVisible(true);
        }
        else
        {
            guiHelper.isInnerMenuVisible(false);

            // Show the menu sub view
            viewModel.menuViewName(viewName);
        }
    },
    defaultWebErrorMessage: '',
    defaultInternalErrorMessage: '',
    defaultPaymentErrorMessage: '',
    showError: function (message, returnCallback, exception)
    {
        // Set the error details
        viewModel.error.message(message);

        // Return button
        viewModel.error.showReturn(returnCallback != undefined);
        viewModel.error.returnCallback = returnCallback;

        // Show the error view
        guiHelper.showView('errorView');

        //if (exception != undefined && exception.message != undefined)
        //{
        //    console.error(exception.message);
        //}
    },
    errorReturn: function ()
    {
        // Is there a callback function?
        if (typeof (viewModel.error.returnCallback) === 'function')
        {
            // Callback
            viewModel.error.returnCallback();
        }
    },
    showCheckoutAfterError: function ()
    {
        cartHelper.checkout();
        //if (guiHelper.isMobileMode())
        //{
        //    cartHelper.checkout();
        //}
        //else
        //{
        //    //guiHelper.showMenuView('checkoutView');
        //}
    },
    resize: function ()
    {
        guiHelper.refreshIsMobileMode();

        var offset = 0;
        var newPageWidth = $(window).width();
        var sectionWidth = $('#' + css_menu).width();

        $('.' + css_sections).css('width', sectionWidth + 'px');
        $('.' + css_section).css('width', sectionWidth + 'px');
    },
    showStoreMenu: function ()
    {
        // Gets the site details (address etc...) and then downloads the menu
        try
        {
            // Test mode
            if (settings.alwaysOnline)
            {
                viewModel.isTakingOrders(true);
            }
            else
            {
                viewModel.isTakingOrders(viewModel.selectedSite().isOpen);
            }

            // Is the menu already cached locally?  If it is then load it
            viewModel.menu = undefined;
            var menu = cacheHelper.loadCachedMenuForSiteId(viewModel.selectedSite().siteId)
            viewModel.menu = menu;

            // Get the site details from the server
            guiHelper.showPleaseWait
            (
                textStrings.gettingStoreDetails,
                '0%',
                function ()
                {
                    // If we managed to get the menu from the cache then there's no need to get it from the server
                    var gotMenuVersion = (viewModel.menu == undefined ? -1 : viewModel.selectedSite().menuVersion);

                    // Get the site menu from the server
                    acsapi.getSite
                    (
                        viewModel.selectedSite().siteId,
                        gotMenuVersion,
                        function (data)
                        {
                            try
                            {
                                // Rendering the menu can take a couple of seconds on a mobile
                                viewModel.pleaseWaitMessage(textStrings.savingMenuToDevice);
                                viewModel.pleaseWaitProgress('');

                                // Allow JavaScript to process events (kind of like doEvents)
                                setTimeout
                                (
                                    function ()
                                    {
                                        try
                                        {
                                            // Did that work?
                                            if (data == undefined || data.Menu == undefined)
                                            {
                                                guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
                                            }
                                            else
                                            {
                                                // Do we have a menu?
                                                if (data.Menu.MenuData != undefined)
                                                {
                                                    var menu = JSON.parse(data.Menu.MenuData);

                                                    // Cache the menu locally.  If this fails just carry on anyway
                                                    cacheHelper.cacheMenu(menu);

                                                    // Keep hold of the menu that we just got from the server
                                                    viewModel.menu = menu;
                                                }

                                                if (data.Menu.MenuDataThumbnails != undefined)
                                                {
                                                    // Keep hold of the menu images we just got from the server
                                                    try
                                                    {
                                                        menuHelper.menuDataThumbnails = JSON.parse(data.Menu.MenuDataThumbnails);
                                                    }
                                                    catch (error) { }
                                                }

                                                if (data.Menu.MenuDataExtended != undefined)
                                                {
                                                    // Keep hold of the menu extensions we just got from the server
                                                    try
                                                    {
                                                        menuHelper.menuDataExtended = JSON.parse(data.Menu.MenuDataExtended);
                                                    }
                                                    catch (error) { }
                                                }

                                                // Keep hold of the delivery zones we just got from the server
                                                try
                                                {
                                                    deliveryZoneHelper.deliveryZones(JSON.parse(data.DeliveryZones));
                                                }
                                                catch (error) { }

                                                // Keep hold of the site details we just got from the server
                                                viewModel.siteDetails(data.Details);

                                                // Rendering the menu can take a couple of seconds on a mobile
                                                viewModel.pleaseWaitMessage(textStrings.loadingMenu);
                                                viewModel.pleaseWaitProgress('');

                                                setTimeout
                                                (
                                                    function ()
                                                    {
                                                        try
                                                        {
                                                            // Render the menu 
                                                            guiHelper.renderMenu
                                                            (
                                                                viewModel.menu,
                                                                function ()
                                                                {
                                                                    if (settings.isHomePageDisabled)
                                                                    {
                                                                        // Show the menu page
                                                                        guiHelper.showMenu();
                                                                    }
                                                                    else
                                                                    {
                                                                        // Show the home page
                                                                        guiHelper.showHome();
                                                                    }
                                                                }
                                                            );
                                                        }
                                                        catch (exception)
                                                        {
                                                            // Got an error
                                                            guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                                                        }
                                                    },
                                                    0
                                                );
                                            }
                                        }
                                        catch (exception)
                                        {
                                            // Got an error
                                            guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                                        }
                                    },
                                    0
                                );
                            }
                            catch (exception)
                            {
                                // Got an error
                                guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                            }
                        }
                    );
                }
            );
        }
        catch (exception)
        {
            // Got an error
            guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
        }
    },
    showPleaseWait: function (message, progress, callback)
    {
        viewModel.pleaseWaitMessage(message);
        viewModel.pleaseWaitProgress(progress);
        guiHelper.showView('pleaseWaitView');

        if (typeof (callback) == 'function')
        {
            // Let knockout do its thing
            setTimeout
            (
                function ()
                {
                    callback();
                },
                0
            );
        }
    },
    renderMenu: function (menu, callback)
    {
        guiHelper.showPleaseWait
        (
            textStrings.renderingMenu,
            undefined,
            function ()
            {
                // This is the menu we will be working with
                viewModel.menu = menu;

                // Figure out what address format to use
                if (viewModel.siteDetails().address.country == "United Kingdom")
                {
                    viewModel.addressType('ukAddress-template');
                }
                else if (viewModel.siteDetails().address.country == "United States")
                {
                    viewModel.addressType('usAddress-template');
                }
                else if (viewModel.siteDetails().address.country == "France")
                {
                    viewModel.addressType('frenchAddress-template');
                }
                else
                {
                    viewModel.addressType('genericAddress-template');
                }

                // Build a store address we can display on the page
                var displayAddress = "";
                displayAddress += viewModel.siteDetails().address.roadNum != null && viewModel.siteDetails().address.roadNum.length > 0 ? viewModel.siteDetails().address.roadNum + ' ' : '';
                displayAddress += viewModel.siteDetails().address.roadName != null && viewModel.siteDetails().address.roadName.length > 0 ? viewModel.siteDetails().address.roadName + ', ' : '';
                displayAddress += viewModel.siteDetails().address.org1 != null && viewModel.siteDetails().address.org1.length > 0 ? viewModel.siteDetails().address.org1 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.org2 != null && viewModel.siteDetails().address.org2.length > 0 ? viewModel.siteDetails().address.org2 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.org3 != null && viewModel.siteDetails().address.org3.length > 0 ? viewModel.siteDetails().address.org3 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.prem1 != null && viewModel.siteDetails().address.prem1.length > 0 ? viewModel.siteDetails().address.prem1 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.prem2 != null && viewModel.siteDetails().address.prem2.length > 0 ? viewModel.siteDetails().address.prem2 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.prem3 != null && viewModel.siteDetails().address.prem3.length > 0 ? viewModel.siteDetails().address.prem3 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.prem4 != null && viewModel.siteDetails().address.prem4.length > 0 ? viewModel.siteDetails().address.prem4 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.prem5 != null && viewModel.siteDetails().address.prem5.length > 0 ? viewModel.siteDetails().address.prem5 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.prem6 != null && viewModel.siteDetails().address.prem6.length > 0 ? viewModel.siteDetails().address.prem6 + ', ' : '';
                displayAddress += viewModel.siteDetails().address.locality != null && viewModel.siteDetails().address.locality.length > 0 ? viewModel.siteDetails().address.locality + ', ' : '';
                displayAddress += viewModel.siteDetails().address.town != null && viewModel.siteDetails().address.town.length > 0 ? viewModel.siteDetails().address.town + ', ' : '';
                displayAddress += viewModel.siteDetails().address.county != null && viewModel.siteDetails().address.county.length > 0 ? viewModel.siteDetails().address.county + ', ' : '';
                displayAddress += viewModel.siteDetails().address.state != null && viewModel.siteDetails().address.state.length > 0 ? viewModel.siteDetails().address.state + ', ' : '';
                displayAddress += viewModel.siteDetails().address.postcode != null && viewModel.siteDetails().address.postcode.length > 0 ? viewModel.siteDetails().address.postcode + ', ' : '';

                if (displayAddress[displayAddress.length - 2] == ',')
                {
                    displayAddress = displayAddress.substr(0, displayAddress.length - 2);
                }

                var displayAddressMultiLine = "";
                displayAddressMultiLine += viewModel.siteDetails().address.roadNum != null && viewModel.siteDetails().address.roadNum.length > 0 ? viewModel.siteDetails().address.roadNum + ' ' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.roadName != null && viewModel.siteDetails().address.roadName.length > 0 ? viewModel.siteDetails().address.roadName + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.org1 != null && viewModel.siteDetails().address.org1.length > 0 ? viewModel.siteDetails().address.org1 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.org2 != null && viewModel.siteDetails().address.org2.length > 0 ? viewModel.siteDetails().address.org2 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.org3 != null && viewModel.siteDetails().address.org3.length > 0 ? viewModel.siteDetails().address.org3 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.prem1 != null && viewModel.siteDetails().address.prem1.length > 0 ? viewModel.siteDetails().address.prem1 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.prem2 != null && viewModel.siteDetails().address.prem2.length > 0 ? viewModel.siteDetails().address.prem2 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.prem3 != null && viewModel.siteDetails().address.prem3.length > 0 ? viewModel.siteDetails().address.prem3 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.prem4 != null && viewModel.siteDetails().address.prem4.length > 0 ? viewModel.siteDetails().address.prem4 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.prem5 != null && viewModel.siteDetails().address.prem5.length > 0 ? viewModel.siteDetails().address.prem5 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.prem6 != null && viewModel.siteDetails().address.prem6.length > 0 ? viewModel.siteDetails().address.prem6 + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.locality != null && viewModel.siteDetails().address.locality.length > 0 ? viewModel.siteDetails().address.locality + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.town != null && viewModel.siteDetails().address.town.length > 0 ? viewModel.siteDetails().address.town + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.county != null && viewModel.siteDetails().address.county.length > 0 ? viewModel.siteDetails().address.county + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.state != null && viewModel.siteDetails().address.state.length > 0 ? viewModel.siteDetails().address.state + '<br />' : '';
                displayAddressMultiLine += viewModel.siteDetails().address.postcode != null && viewModel.siteDetails().address.postcode.length > 0 ? viewModel.siteDetails().address.postcode + '<br />' : '';

                // Currency symbol
                switch (viewModel.siteDetails().address.country)
                {
                    case 'Austria':
                    case 'Belgium':
                    case 'Cyprus':
                    case 'Germany':
                    case 'Estonia':
                    case 'Spain':
                    case 'Finland':
                    case 'France':
                    case 'Greece':
                    case 'Ireland':
                    case 'Italy':
                    case 'Luxembourg':
                    case 'Malta':
                    case 'Netherlands':
                    case 'Monaco':
                    case 'Portugal':
                    case 'Slovakia':
                    case 'Slovenia':
                    case 'Holy See (Vatican City State)':
                    case 'San Marino':
                        helper.useCommaDecimalPoint = true;
                        helper.curencyAfter = true;
                        helper.use24hourClock = true;
                        helper.currencySymbol = '&euro;';
                        break;
                    case 'Canada':
                    case 'Australia':
                    case 'New Zealand':
                    case 'United States':
                        helper.useCommaDecimalPoint = false;
                        helper.currencySymbol = '$';
                        break;
                    case 'Russian Federation':
                        helper.useCommaDecimalPoint = false;
                        helper.currencySymbol = 'р';
                        break;
                    default:
                        helper.useCommaDecimalPoint = false;
                        helper.currencySymbol = '&pound;';
                        break;
                }

                // Opening times
                openingTimesHelper.getToday();
                openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.monday, 'Monday');
                openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.tuesday, 'Tuesday');
                openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.wednesday, 'Wednesday');
                openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.thursday, 'Thursday');
                openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.friday, 'Friday');
                openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.saturday, 'Saturday');
                openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.sunday, 'Sunday');

                // Is the store open today?
                var todaysOpeningTimes = openingTimesHelper.getTodaysOpeningTimes();
                if (todaysOpeningTimes.length == 0 && !settings.alwaysOnline)
                {
                    // Store is closed so can't take any orders
                    viewModel.isTakingOrders(false);
                }

                // Set the display address
                viewModel.displayAddress(displayAddress);
                viewModel.displayAddressMultiLine(displayAddressMultiLine);

                // Clear the menu sections
                viewModel.sections.removeAll();

                // Section display order - need to display them in the same order they appear in the JSON
                for (var displayIndex = 0; displayIndex < viewModel.menu.Display.length; displayIndex++)
                {
                    var display = viewModel.menu.Display[displayIndex];
                    display.displayOrder = displayIndex + 1;
                }

                // Sort out special characters and trim names
                menuHelper.fixNames();

                // Build an index of items
                menuHelper.buildItemLookup();

                // Build an index of deals
                menuHelper.buildDealLookup();

                // See if there are any images for the items
                menuHelper.lookupItemImages();

                // See if there are any extensions to the items
                menuHelper.lookupItemExtensions();

                // See if there are any extensions to the deals
                menuHelper.lookupDealExtensions();

                // Build the deals section
                menuHelper.buildDealsSection();

                // Build the sections that contain menu items
                menuHelper.buildItemsSections
                (
                    function ()
                    {
                        // Make the first section visible
                        viewModel.visibleSection(viewModel.sections()[0].display.Name);

                        callback();
                    }
                );
            }
        );
    },
    showMenu: function (index)
    {
        var timer = new Date();

        mobileMenuHelper.title(textStrings.mmOrderNow);
        mobileMenuHelper.titleClass('popupMobileMenuOrderNowButton');

        // Make the main menu visible
        guiHelper.isMainMenuVisible(true);

        // Show the menu view
        guiHelper.showMenuView('menuSectionsView');

        guiHelper.isMobileMenuVisible(true);
        viewModel.resetOrderType();
        guiHelper.cartActions(guiHelper.cartActionsMenu);
        guiHelper.isCartLocked(false);

        // Let knockout update the bindings
        setTimeout
        (
            function ()
            {
                var timeNow = new Date();
                var sv = Math.abs(timeNow - timer);

                // Make sure the sections are layed out correctly
                guiHelper.resize();

                guiHelper.showSection
                (
                    typeof (index) == 'number' ? index : undefined,
                    function ()
                    {
                        // When the customer started placing the order
                        viewModel.timer = new Date();

                        timeNow = new Date();
                        var tot = Math.abs(timeNow - timer);
                        //                   alert('showview:' + sv + ' Total:' + tot);
                    }
                );
            },
            0
        );

        // DON'T ADD CODE HERE - ADD IT EITHER BEFORE setTimeout OR IN THE setTimeout CALLBACK FUNCTION
    },
    selectedItemChanged: function (item, mode)
    {
        if (item != undefined)
        {
            // Get the menu item that the user has selected
            var menuItem = menuHelper.getSelectedMenuItem(item);

            if (menuItem != undefined)
            {
                var quantity = (typeof (item.quantity) == 'function' ? item.quantity() : item.quantity);
                var toppings = (typeof (item.toppings) == 'function' ? item.toppings() : undefined);

                // Update the price
                item.price(helper.formatPrice(menuHelper.calculateItemPrice(menuItem, quantity, toppings)));
            }
        }
    },
    showMenuSection: function ()
    {
        if (guiHelper.isMobileMode())
        {
            mobileMenuHelper.showMenu(this.Index);
        }
        else
        {
            viewModel.selectedSection(this);
            guiHelper.showMenu(this.Index);
        }
    },
    enableUI: function (enable)
    {
        var viewContainer = $('#viewContainer');
        guiHelper.enableDisableElement(viewContainer, enable);

        var menuContainer = $('#menuSection');
        guiHelper.enableDisableElement(menuContainer, enable);
    },
    enableDisableElement: function (element, enable)
    {
        // Enable or disable the element
        if (enable)
        {
            element.find('*').removeClass("disabled");
        }
        else
        {
            element.find('*').addClass("disabled");
        }
    }
}

// Menu helper functions
var menuHelper =
{
    menuDataThumbnails: undefined,
    menuDataExtended: undefined,
    fixName: function (name)
    {
        if (name == undefined)
        {
            return '';
        }

        return name.replace("&#146;", "'");
    },
    refreshItemPrices: function ()
    {
        // Change each section
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];

            // Does this section have any items?
            if (section.items != undefined)
            {
                // Change the items in the section
                for (var itemIndex = 0; itemIndex < section.items().length; itemIndex++)
                {
                    var item = section.items()[itemIndex];

                    // Set the price depending on the current order type
                    item.price(helper.formatPrice(menuHelper.getItemPrice(item.menuItem)));

                    // Is the item available for the current order type
                    item.isEnabled(menuHelper.isItemEnabledForMenu(item.menuItem));

                    // Does the item have any toppings?
                    if (item.toppings != undefined)
                    {
                        // Change the toppings in the item
                        for (var toppingIndex = 0; toppingIndex < item.toppings().length; toppingIndex++)
                        {
                            var topping = item.toppings()[itemIndex];

                            var price = menuHelper.getToppingPrice(topping);

                            topping.price(helper.formatPrice(price));
                            topping.doublePrice(helper.formatPrice(price * 2));
                        }
                    }
                }
            }
        }
    },
    refreshDealsAvailabilty: function ()
    {
        // Change each deal
        for (var dealIndex = 0; dealIndex < viewModel.deals().length; dealIndex++)
        {
            var dealItem = viewModel.deals()[dealIndex];
            dealItem.isEnabled(menuHelper.isDealItemEnabledForMenu(dealItem.deal));
        }
    },
    calculateItemPrice: function (menuItem, quantity, toppings, addToppingPrices, recalcFreeQuantities)
    {
        var price = menuHelper.getItemPrice(menuItem);

        if (price == undefined) return;

        // Does the item have any toppings?
        if (toppings != undefined)
        {
            var toppingPrices = [];

            // Get a list of topping prices to be applied
            for (var index = 0; index < toppings.length; index++)
            {
                var topping = toppings[index];
                if (recalcFreeQuantities) topping.freeQuantity = 0;

                menuHelper.calculateToppingPrice(topping, toppingPrices);
            }

            if (menuItem.FreeTops > 0)
            {
                // Are there more toppings than free toppings (i.e. there are some free and some non-free toppings)
                if (toppingPrices.length > menuItem.FreeTops)
                {
                    // Sort the prices low to high
                    toppingPrices.sort(function (a, b) { return a.price - b.price });

                    // Free toppings are free!
                    for (var index = 0; index < menuItem.FreeTops; index++)
                    {
                        // This topping is free
                        toppingPrices[index].toppingWrapper.freeQuantity++;
                    }

                    // Remove the free toppings (the cheapest ones!) (we're minusing 1 because array indexes are zero based)
                    for (var index = menuItem.FreeTops; index < toppingPrices.length; index++)
                    {
                        var toppingPrice = toppingPrices[index];
                        if (addToppingPrices) price += toppingPrice.price;
                    }
                }
                else
                {
                    // All toppings are free
                    for (var index = 0; index < toppings.length; index++)
                    {
                        var topping = toppings[index];
                        topping.freeQuantity = topping.quantity;
                    }
                }
            }
            else
            {
                // Adjust the price based on the toppings
                for (var index = 0; index < toppingPrices.length ; index++)
                {
                    var toppingPrice = toppingPrices[index];
                    if (addToppingPrices) price += toppingPrice.price;
                }
            }
        }

        price = price * quantity;

        return price;
    },
    calculateToppingPrice: function (toppingWrapper, prices)
    {
        var price = 0;

        if (toppingWrapper.type == 'removable')
        {
            // The item already comes with this topping (for free) but does the customer want to double up the quantity?
            if (toppingWrapper.selectedDouble() && toppingWrapper.freeQuantity == 0)
            {
                // This topping is already on the item so doubling up just adds a single topping
                price = (menuHelper.getToppingPrice(toppingWrapper.topping));

                if (prices != undefined && price > 0)
                {
                    prices.push
                    (
                        {
                            price: menuHelper.getToppingPrice(toppingWrapper.topping),
                            toppingWrapper: toppingWrapper
                        }
                    );
                }
            }
        }
        else
        {
            // Customer wants to add a topping to the item

            // Add a single topping?
            if (toppingWrapper.selectedSingle())
            {
                if (toppingWrapper.freeQuantity == 0)
                {
                    price = menuHelper.getToppingPrice(toppingWrapper.topping);

                    if (prices != undefined && price > 0)
                    {
                        prices.push
                        (
                            {
                                price: menuHelper.getToppingPrice(toppingWrapper.topping),
                                toppingWrapper: toppingWrapper
                            }
                        );
                    }
                }
            }
            else if (toppingWrapper.selectedDouble())
            {
                price = (menuHelper.getToppingPrice(toppingWrapper.topping) * (2 - toppingWrapper.freeQuantity));

                if (prices != undefined && price > 0)
                {
                    // It's a double - seperate them out as only one might be eligable for a free topping
                    if (toppingWrapper.freeQuantity <= 1)
                    {
                        prices.push
                        (
                            {
                                price: menuHelper.getToppingPrice(toppingWrapper.topping),
                                toppingWrapper: toppingWrapper
                            }
                        );
                    }

                    if (toppingWrapper.freeQuantity == 0)
                    {
                        prices.push
                        (
                            {
                                price: menuHelper.getToppingPrice(toppingWrapper.topping),
                                toppingWrapper: toppingWrapper
                            }
                        );
                    }
                }
            }
        }

        return price;
    },
    calculateDealItemPrice: function (menuItem, toppings, recalcFreeQuantities)
    {
        var price = 0

        // Get the categories
        var category1 = helper.findById(menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1, viewModel.menu.Category1);
        var category2 = helper.findById(menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2, viewModel.menu.Category2);

        // Add category deal premiums
        price += ((category1 == undefined ? 0 : category1.DealPremium) + (category2 == undefined ? 0 : category2.DealPremium));

        // Add item deal premium
        price += menuItem.DealPricePremiumOverride == 0 ? menuItem.DealPricePremium : menuItem.DealPricePremiumOverride;

        // Does the item have any toppings?
        if (toppings != undefined)
        {
            var toppingPrices = [];

            // Adjust the price based on the toppings
            for (var index = 0; index < toppings.length; index++)
            {
                var topping = toppings[index];
                if (recalcFreeQuantities) topping.freeQuantity = 0;

                menuHelper.calculateToppingPrice(topping, toppingPrices);
            }

            if (menuItem.FreeTops > 0)
            {
                // Are there more toppings than free toppings (i.e. there are some free and some non-free toppings)
                if (toppingPrices.length > menuItem.FreeTops)
                {
                    // Sort the prices low to high
                    toppingPrices.sort(function (a, b) { return a.price - b.price });

                    // Free toppings are free!
                    for (var index = 0; index < menuItem.FreeTops; index++)
                    {
                        // This topping is free
                        toppingPrices[index].toppingWrapper.freeQuantity++;
                    }

                    // Remove the free toppings (the cheapest ones!) (we're minusing 1 because array indexes are zero based)
                    for (var index = menuItem.FreeTops; index < toppingPrices.length; index++)
                    {
                        var toppingPrice = toppingPrices[index];
                        price += toppingPrice.price;
                    }
                }
                else
                {
                    // All toppings are free
                    for (var index = 0; index < toppings.length; index++)
                    {
                        var topping = toppings[index];
                        topping.freeQuantity = topping.quantity;
                    }
                }
            }
            else
            {
                // Adjust the price based on the toppings
                for (var index = 0; index < toppingPrices.length ; index++)
                {
                    var toppingPrice = toppingPrices[index];
                    price += toppingPrice.price;
                }
            }
        }

        return price;
    },
    calculateDealLinePrice: function (bindableDealLine, excludeDealCalculation)
    {
        var price = 0;

        if (bindableDealLine.selectedMenuItem != undefined)
        {
            // Get the price of the selected item
            var basePrice = menuHelper.getItemPrice(bindableDealLine.selectedMenuItem);

            if (excludeDealCalculation != undefined && excludeDealCalculation == true)
            {
                price = basePrice;
            }
            else
            {
                switch (bindableDealLine.dealLine.Type)
                {
                    case 'Fixed':
                        // Get the fixed price
                        price = menuHelper.getDealLinePrice(bindableDealLine);
                        break;
                    case 'Discount':
                        // Get the discount amount
                        var discountAmount = menuHelper.getDealLinePrice(bindableDealLine);

                        // Deduct the discount from the item price
                        price = basePrice - discountAmount;

                        // No refunds!
                        if (price < 0) price = 0;

                        break;
                    case 'Percentage':
                        // Get the percentage
                        var percentage = menuHelper.getDealLinePrice(bindableDealLine) / 100;

                        // Deduct the percentage from the item price
                        price = basePrice * percentage;

                        break;
                }
            }
        }

        return price;
    },
    getItemPrice: function (menuItem)
    {
        if (viewModel.orderType() == 'delivery')
        {
            return menuItem.DelPrice == undefined ? menuItem.DeliveryPrice : menuItem.DelPrice == undefined ? menuItem.DeliveryPrice : menuItem.DelPrice;
        }
        else
        {
            return menuItem.ColPrice == undefined ? menuItem.CollectionPrice : menuItem.ColPrice == undefined ? menuItem.CollectionPrice : menuItem.ColPrice;
        }
    },
    getDealLinePrice: function (dealLine)
    {
        if (viewModel.orderType() == 'delivery')
        {
            return dealLine.dealLine.DelAmount == undefined ? dealLine.dealLine.DeliveryAmount : dealLine.dealLine.DelAmount;
        }
        else
        {
            return dealLine.dealLine.ColAmount == undefined ? dealLine.dealLine.CollectionAmount : dealLine.dealLine.ColAmount;
        }
    },
    getToppingPrice: function (topping)
    {
        if (viewModel.orderType() == 'delivery')
        {
            return topping.DelPrice == undefined ? topping.DeliveryPrice : topping.DelPrice;
        }
        else
        {
            return topping.ColPrice == undefined ? topping.CollectionPrice : topping.ColPrice;
        }
    },
    isItemAvailable: function (menuItem)
    {
        // Is the item available for collection or delivery?
        if ((viewModel.orderType() == 'delivery' && menuItem.DelPrice == undefined && menuItem.DeliveryPrice == undefined) ||
            (viewModel.orderType() == 'collection' && menuItem.ColPrice == undefined && menuItem.CollectionPrice == undefined))
        {
            // Item is not available for the current order type
            return false;
        }
        else
        {
            return true;
        }
    },
    isDealAvailable: function (dealItem)
    {
        // Is the deal available for collection or delivery?
        if ((viewModel.orderType() == 'delivery' && dealItem.ForDelivery != true) ||
            (viewModel.orderType() == 'collection' && dealItem.ForCollection != true))
        {
            // Deal is not available for the current order type
            return false;
        }
        else
        {
            return true;
        }
    },
    areDeals: function ()
    {
        return viewModel.menu == undefined ? false : (viewModel.menu.Deals != undefined && viewModel.menu.Deals.length > 0);
    },
    getTemplateName: function (data)
    {
        return data.templateName;
    },
    getSelectedMenuItem: function (item)
    {
        // Find the correct item
        if (item.menuItems().length == 1)
        {
            // There is only one item
            return item.menuItems()[0];
        }
        else
        {
            // Get the menu item for the selected category1 and category2
            for (var index = 0; index < item.menuItems().length; index++)
            {
                var checkItem = item.menuItems()[index];

                if ((item.selectedCategory1() == undefined || item.selectedCategory1().Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1)) &&
                    (item.selectedCategory2() == undefined || item.selectedCategory2().Id == (checkItem.Cat2 == undefined ? checkItem.Category2 : checkItem.Cat2)))
                {
                    return checkItem;
                }
            }
        }
    },
    menuItemLookup: undefined,
    dealLookup: undefined,
    dealSectionCount: 0,
    buildDealsSection: function ()
    {
        // NOTE:  Don't bind to this data structure.  When the user selects a deal the data will be copied into another data structure which we bind to

        // Clear the deals
        viewModel.deals.removeAll();

        if (menuHelper.areDeals())
        {
            var dealSections = [];

            // Process each deal
            for (var dealIndex = 0; dealIndex < viewModel.menu.Deals.length; dealIndex++)
            {
                // The deal to process
                var deal = viewModel.menu.Deals[dealIndex];
                var dealLineWrappers = [];

                // Process the deal lines
                for (var dealLineIndex = 0; dealLineIndex < deal.DealLines.length; dealLineIndex++)
                {
                    // The deal line
                    var dealLine = deal.DealLines[dealLineIndex];

                    // Wrap the deal line in another object to help us binding
                    var dealLineWrapper =
                    {
                        itemNo: dealLineIndex + 1,
                        dealLine: dealLine,
                        templateName: '',
                        items: [],
                        id: 'dl_' + dealLineIndex + 1
                    };

                    // Does the customer need to pick from a list of allowable menu items for this deal line?
                    if (dealLine.AllowableItemsIds != undefined && dealLine.AllowableItemsIds.length > 1)
                    {
                        // We'll need a drop down combo to display the allowable menu items
                        dealLineWrapper.templateName = 'popupDealLinePicker-template';

                        // The first item in drop down combo should be "Please select an item"
                        dealLineWrapper.items.push
                        (
                            {
                                menuItem: undefined,
                                name: textStrings.pleaseSelectAnItem
                            }
                        );

                        // Add the menu items that can be picked for this deal line
                        menuHelper.addItemsToDealLine(dealLineWrapper);
                    }
                    else if (dealLine.AllowableItemsIds != undefined && dealLine.AllowableItemsIds.length == 1)
                    {
                        // Add the menu items that can be picked for this deal line
                        menuHelper.addItemsToDealLine(dealLineWrapper);

                        // No drop down combo needed
                        dealLineWrapper.templateName = 'popupDealLineFixed-template';
                    }
                    else
                    {
                        dealLineWrapper.templateName = 'popupDealLine-template';
                    }

                    dealLineWrappers.push(dealLineWrapper);
                }

                // Is the deal available today?  Some deals are only available on specific days of the week
                var isAvailableToday = true; // Assume available today

                var availableTimes = deal.AvailableTimes[openingTimesHelper.todayPropertyName];
                if (availableTimes != undefined && availableTimes != null && availableTimes.length == 1)
                {
                    if (availableTimes[0].NotAvailableToday)
                    {
                        isAvailableToday = false;
                    }
                }

                var dealWrapper =
                {
                    deal: deal,
                    isEnabled: ko.observable(menuHelper.isDealItemEnabledForMenu(deal)),
                    isAvailableToday: isAvailableToday,
                    dealLineWrappers: dealLineWrappers,
                    minimumOrderValue: deal.MinimumOrderValue > 0 ? helper.formatPrice(deal.MinimumOrderValue) : ''
                };

                viewModel.deals.push(dealWrapper);

                var lookupItem = menuHelper.dealLookup[deal.Id];

                // Does the deal have any extensions?
                var sectionName = undefined;
                if (lookupItem.extension != undefined)
                {
                    // Do we need to move the deal to another section?
                    if (lookupItem.extension.moveToSection != undefined)
                    {
                        sectionName = lookupItem.extension.moveToSection;
                    }
                }

                // Is the deal being moved to a different section?
                if (sectionName == undefined) 
                {
                    // Deal should be in the default deals section
                    sectionName = textStrings.dealsSection;
                }
                
                // Try and find the section to move the deal to
                var dealSection = undefined;
                for (var sectionIndex = 0; sectionIndex < dealSections.length; sectionIndex++)
                {
                    var checkDealSection = dealSections[sectionIndex];
                    if (checkDealSection.name == sectionName)
                    {
                        dealSection = checkDealSection;
                        break;
                    }
                }

                // Does the section already exist?
                if (dealSection == undefined)
                {
                    // Couldn't find the section so create it
                    dealSection = { name: sectionName, deals: [] };

                    // Add the section
                    dealSections.push(dealSection);
                }

                // Add the deal to the section
                dealSection.deals.push(dealWrapper);
            }

            for (var dealSectionIndex = 0; dealSectionIndex < dealSections.length; dealSectionIndex++)
            {
                var dealSection = dealSections[dealSectionIndex];

                var section = { templateName: 'dealsSection-template', display: { Name: dealSection.name, displayOrder: dealSectionIndex }, deals: dealSection.deals, Index: viewModel.sections().length, Left: ko.observable(0) };
                viewModel.sections.push(section);
            }

            menuHelper.dealSectionCount = dealSections.length;
        }
    },
    addItemsToDealLine: function (dealLineWrapper)
    {
        // We need to build a list of menu items to pick from
        for (var itemIndex = 0; itemIndex < dealLineWrapper.dealLine.AllowableItemsIds.length; itemIndex++)
        {
            var menuId = dealLineWrapper.dealLine.AllowableItemsIds[itemIndex];

            // Lookup the menu item
            var menuItemWrapper = menuHelper.menuItemLookup[menuId];

            if (menuItemWrapper != undefined)
            {
                // Get the item name
                var itemName = viewModel.menu.ItemNames[menuItemWrapper.menuItem.Name == undefined ? menuItemWrapper.menuItem.ItemName : menuItemWrapper.menuItem.Name];

                var item = undefined;

                // Is the item already in the deal line (the same item can be in a deal line multiple times e.g. different sizes of the same item)
                // but we only want it to appear once
                for (var checkItemIndex = 0; checkItemIndex < dealLineWrapper.items.length; checkItemIndex++)
                {
                    var checkItem = dealLineWrapper.items[checkItemIndex];

                    if (checkItem.name == itemName)
                    {
                        item = checkItem;
                        break;
                    }
                }

                if (item == undefined)
                {
                    // Add the item to the deal line
                    item =
                    {
                        name: itemName,
                        isEnabled: ko.observable(menuHelper.isItemEnabledForMenu(menuItemWrapper.menuItem)),
                        displayOrder: menuItemWrapper.menuItem.DispOrder,
                        description: menuHelper.fixName(menuItemWrapper.menuItem.Desc == undefined ? menuItemWrapper.menuItem.Description : menuItemWrapper.menuItem.Desc),
                        category1s: ko.observableArray(),
                        category2s: ko.observableArray(),
                        menuItems: ko.observableArray(),
                        selectedCategory1: ko.observable(),
                        selectedCategory2: ko.observable(),
                        price: ko.observable(helper.formatPrice(menuHelper.getItemPrice(menuItemWrapper.menuItem))),
                        quantity: 1,
                        selectedMenuItem: undefined
                    };

                    dealLineWrapper.items.push(item);
                }

                // Does this item have a category 1 (e.g. Size)
                var cat1 = menuItemWrapper.menuItem.Cat1 == undefined ? menuItemWrapper.menuItem.Category1 : menuItemWrapper.menuItem.Cat1;
                if (cat1 != undefined && cat1 != -1)
                {
                    var category = helper.findById(cat1, viewModel.menu.Category1);
                    if (category != undefined && !helper.findCategory(category, item.category1s))
                    {
                        item.category1s.push(category);
                    }
                }

                // Does this item have a category 2 (e.g. Size)
                var cat2 = menuItemWrapper.menuItem.Cat2 == undefined ? menuItemWrapper.menuItem.Category2 : menuItemWrapper.menuItem.Cat2;
                if (cat2 != undefined && cat2 != -1)
                {
                    var category = helper.findById(cat2, viewModel.menu.Category2);
                    if (category != undefined && !helper.findCategory(category, item.category2s))
                    {
                        item.category2s.push(category);
                    }
                }

                // Add this menu item to the variations of this particular product
                item.menuItems.push(menuItemWrapper.menuItem);
            }
        }
    },
    buildItemLookup: function ()
    {
        // Clear the index
        menuHelper.menuItemLookup = {};

        // Process each menu item
        for (var index = 0; index < viewModel.menu.Items.length; index++)
        {
            var menuItem = viewModel.menu.Items[index];

            // Add the menu item to the lookup for later
            menuHelper.menuItemLookup[menuItem.Id == undefined ? menuItem.MenuId : menuItem.Id] =
            {
                name: viewModel.menu.ItemNames[menuItem.Name == undefined ? menuItem.ItemName : menuItem.Name],
                menuItem: menuItem,
                thumbnailHeight: 0,
                thumbnailWidth: 0,
                thumbnail: undefined,
                image: undefined,
                overlayImage: undefined,
                extension: undefined
            };
        }
    },
    buildDealLookup: function ()
    {
        // Clear the index
        menuHelper.dealLookup = {};

        // Process each deal
        for (var index = 0; index < viewModel.menu.Deals.length; index++)
        {
            var deal = viewModel.menu.Deals[index];

            // Add the deal to the lookup for later
            menuHelper.dealLookup[deal.Id] =
            {
                extension: undefined,
                deal: deal,
                thumbnailHeight: 0,
                thumbnailWidth: 0,
                thumbnail: undefined,
                image: undefined,
                overlayImage: undefined
            };
        }
    },
    lookupItemImages: function ()
    {
        if (menuHelper.menuDataThumbnails != undefined)
        {
            if (menuHelper.menuDataThumbnails.MenuItemThumbnails != undefined)
            {
                // Process each menu image
                for (var index = 0; index < menuHelper.menuDataThumbnails.MenuItemThumbnails.length; index++)
                {
                    var image = menuHelper.menuDataThumbnails.MenuItemThumbnails[index];

                    // Is the image small enough to be a thumbnail?
                    if (image.Height <= 130 && image.Width <= 130)
                    {
                        // The menu items that the image is for
                        if (image.ItemIds != undefined)
                        {
                            for (var itemIndex = 0; itemIndex < image.ItemIds.length; itemIndex++)
                            {
                                // Get the menu item id
                                var menuItemId = image.ItemIds[itemIndex];

                                // Is the menu item in this menu?
                                var menuItem = menuHelper.menuItemLookup[menuItemId];

                                if (menuItem != null)
                                {
                                    // Is this the biggest thumbnail image we've found so far?
                                    if (menuItem.thumbnail == undefined ||
                                        (image.Height > menuItem.thumbnail.Height && image.Width > menuItem.thumbnail.Width))
                                    {
                                        // We've got a thumbnail
                                        menuItem.thumbnail = image;
                                        menuItem.thumbnailWidth = menuItem.thumbnail.Width;
                                        menuItem.thumbnailHeight = menuItem.thumbnail.Height;
                                    }
                                }
                            }
                        }
                    }
                        // Is the image big enough?
                    else if (image.Height >= 130 && image.Width >= 130 && image.Height <= 800 && image.Width <= 800)
                    {
                        // The menu items that the image is for
                        if (image.ItemIds != undefined)
                        {
                            for (var itemIndex = 0; itemIndex < image.ItemIds.length; itemIndex++)
                            {
                                // Get the menu item id
                                var menuItemId = image.ItemIds[itemIndex];

                                // Is the menu item in this menu?
                                var menuItem = menuHelper.menuItemLookup[menuItemId];

                                if (menuItem != null)
                                {
                                    // Is this the biggest image we've found so far?
                                    if (menuItem.image == undefined ||
                                        (image.Height > menuItem.image.Height && image.Width > menuItem.image.Width))
                                    {
                                        // We've got a thumbnail
                                        menuItem.image = image;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    },
    lookupItemExtensions: function ()
    {
        if (menuHelper.menuDataExtended != undefined)
        {
            if (menuHelper.menuDataExtended.items != undefined)
            {
                // Process each extended menu item
                for (var index = 0; index < menuHelper.menuDataExtended.items.length; index++)
                {
                    var menuItemExtension = menuHelper.menuDataExtended.items[index];

                    // Is the menu item in this menu?
                    var menuItem = menuHelper.menuItemLookup[menuItemExtension.id];
                    if (menuItem != undefined)
                    {
                        menuItem.extension = menuItemExtension;

                        if (menuItemExtension.tags != undefined)
                        {
                            for (var tagIndex = 0; tagIndex < menuItemExtension.tags.length; tagIndex++)
                            {
                                var tag = menuItemExtension.tags[tagIndex];

                                if (settings.menuItemOverlayTag != undefined &&
                                    tag.toLowerCase() == settings.menuItemOverlayTag.toLowerCase())
                                {
                                    // Check for overlay images
                                    menuItem.overlayImage = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    },
    lookupDealExtensions: function ()
    {
        if (menuHelper.menuDataExtended != undefined && menuHelper.dealLookup)
        {
            if (menuHelper.menuDataExtended.deals != undefined)
            {
                // Process each deal
                for (var index = 0; index < menuHelper.menuDataExtended.deals.length; index++)
                {
                    var dealExtension = menuHelper.menuDataExtended.deals[index];

                    // Is the deal in this menu?
                    var deal = menuHelper.dealLookup[dealExtension.id];
                    if (deal != undefined)
                    {
                        deal.extension = dealExtension;
                    }
                }
            } 
        }
    },
    fixNames: function()
    {
        // Fix menu item names
        for (var menuItemNamesIndex = 0; menuItemNamesIndex < viewModel.menu.ItemNames.length; menuItemNamesIndex++)
        {
            viewModel.menu.ItemNames[menuItemNamesIndex] = menuHelper.fixName($.trim(viewModel.menu.ItemNames[menuItemNamesIndex]));
        }

        // Fix menu item descriptions
        // Process each menu item
        for (var menuItemIndex = 0; menuItemIndex < viewModel.menu.Items.length; menuItemIndex++)
        {
            var menuItem = viewModel.menu.Items[menuItemIndex];

            if (menuItem.Description != undefined)
            {
                menuItem.Description = menuHelper.fixName($.trim(menuItem.Description));
            }
            else
            {
                menuItem.Desc = menuHelper.fixName($.trim(menuItem.Desc));
            }
        }

        // Fix topping names
        for (var toppingNamesIndex = 0; toppingNamesIndex < viewModel.menu.Toppings.length; toppingNamesIndex++)
        {
            var topping = viewModel.menu.Toppings[toppingNamesIndex];

            if (topping.Name != undefined)
            {
                topping.Name = menuHelper.fixName($.trim(topping.Name));
            }
            else
            {
                topping.ToppingName = menuHelper.fixName($.trim(topping.ToppingName));
            }
        }

        // Fix deal names/descriptions
        for (var dealsIndex = 0; dealsIndex < viewModel.deals.length; dealsIndex++)
        {
            var deal = viewModel.deals[dealsIndex];
            deal.DealName = menuHelper.fixName($.trim(deal.DealName));
            deal.Description = menuHelper.fixName($.trim(deal.Description));
        }

    },
    buildItemsSections: function (callback)
    {
        var sections = [];

        // Process each menu item
        for (var index = 0; index < viewModel.menu.Items.length; index++)
        {
            var menuItem = viewModel.menu.Items[index];

            // Add the item to the correct section
            menuHelper.addItemToSection(sections, menuItem);
        }

        // Sort the sections
        var sortedSections = [];
        for (var sectionIndex = 0; sectionIndex < sections.length; sectionIndex++)
        {
            var section = sections[sectionIndex];

            // We need to insert the section in the correct position to maintain the display order
            var inserted = false;
            for (var insertIndex = 0; insertIndex < sortedSections.length; insertIndex++)
            {
                // Figure out where to insert the section
                var insertAt = sortedSections[insertIndex];
                if (section.display.displayOrder < insertAt.display.displayOrder)
                {
                    // We need to insert the section here
                    sortedSections.splice(insertIndex, 0, section);

                    inserted = true;
                    break;
                }
            }

            if (!inserted)
            {
                // Insert at end
                section.Index = sortedSections.length;
                sortedSections.push(section);
            }
        }

        // Fix the Index numbers since we've sorted the sections
        for (var sectionIndex = 0; sectionIndex < sortedSections.length; sectionIndex++)
        {
            var section = sortedSections[sectionIndex];
            section.Index = sectionIndex + menuHelper.dealSectionCount;
        }

        // Render the sections one by one so we can show the progress
        menuHelper.commitSection
        (
            0,
            sortedSections,
            function ()
            {
                menuHelper.sortItemSections();
                callback();
            }
        );
    },
    commitSection: function (index, sections, callback)
    {
        if (index >= sections.length)
        {
            callback();
        }
        else
        {
            // Get the section to render
            var section = sections[index];

            // Add the section the the list that is data bound to the UI
            viewModel.sections.push(section);

            // Work out how far we've got
            var percentage = Math.round(((index + 1) / sections.length) * 100);

            // Update the UI
            viewModel.pleaseWaitProgress(percentage + '%');

            // Let knockout update the UI
            setTimeout
            (
                function ()
                {
                    // Process the next section
                    menuHelper.commitSection(index + 1, sections, callback);
                },
                0
            );
        }
    },
    addItemToSection: function (sections, menuItem)
    {
        var display = helper.findById(menuItem.Display, viewModel.menu.Display);

        // Special case.  Some menu items are only available as part of a deal.  As a work around to ensure the
        // menu items are exported from the Rameses menu we're putting them in the "DealsOnly" section but we don't want this section to be visible
        if (display.Name.toUpperCase() == 'DEALSONLY') return;

        var section = undefined;

        // Is there already a section for this menu item?
        for (var sectionIndex = 0; sectionIndex < sections.length; sectionIndex++)
        {
            var checkSection = sections[sectionIndex];

            if (checkSection == undefined || checkSection.display == undefined || menuItem.Display == undefined)
            {
                return;
            }
            else
            {
                if (checkSection.display.displayId == menuItem.Display)
                {
                    section = checkSection;
                    break;
                }
            }
        }

        if (display == undefined)
        {
            display = { Name: '?' };
        }

        if (section == undefined)
        {
            // No such section - create it

            // Fix the display order to take into account the deal sections
            display.displayOrder = menuHelper.dealSectionCount + display.displayOrder;
            display.displayId = menuItem.Display;

            section = { templateName: 'menuSection-template', display: display, items: ko.observableArray(), Index: undefined, Left: ko.observable(0) };
            sections.push(section);
        }

        // Get the item name
        var originalItemName = viewModel.menu.ItemNames[menuItem.Name == undefined ? menuItem.ItemName : menuItem.Name];
        var itemName = originalItemName;
        var itemCat1 = undefined;

        // Does this item have a category 1 (e.g. Size)
        var cat1 = menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1;
        if (cat1 != undefined && cat1 != -1)
        {
            itemCat1 = helper.findById(cat1, viewModel.menu.Category1);

            if (itemCat1 != undefined && settings.invertItems)
            {
                itemName = menuHelper.fixName($.trim(itemCat1.Name));
            }
        }

        var item = undefined;

        // Is the item already in the section (the same item can be in a section multiple times e.g. different sizes of the same item)
        for (var itemIndex = 0; itemIndex < section.items().length; itemIndex++)
        {
            var checkItem = section.items()[itemIndex];

            if (checkItem.name == itemName)
            {
                item = checkItem;
                break;
            }
        }

        if (item == undefined)
        {
            // Add the item to the section
            item =
            {
                name: itemName,
                isEnabled: ko.observable(menuHelper.isItemEnabledForMenu(menuItem)),
                menuItem: menuItem,
                displayOrder: menuItem.DispOrder,
                description: menuHelper.fixName(menuItem.Desc == undefined ? menuItem.Description : menuItem.Desc),
                category1s: ko.observableArray(),
                category2s: ko.observableArray(),
                menuItems: ko.observableArray(),
                selectedCategory1: ko.observable(),
                selectedCategory2: ko.observable(),
                price: ko.observable(helper.formatPrice(menuHelper.getItemPrice(menuItem))),
                quantity: 1,
                isAvailableForDelivery: function ()
                {
                    return viewModel.orderType() == 'delivery' && this.DelPrice == undefined && this.DeliveryPrice == undefined;
                },
                isAvailableForCollection: function ()
                {
                    return viewModel.orderType() == 'collection' && this.ColPrice == undefined && this.CollectionPrice == undefined;
                },
                thumbnail: undefined,
                thumbnailWidth: 0,
                thumbnailHeight: 0,
                image: undefined,
                overlayImage: undefined
            };

            section.items.push(item);
        }

        if (settings.showMenuIds)
        {
            item.description += ' ' + menuItem.Id;
        }

        //// Does this item have a category 1 (e.g. Size)
        //var cat1 = menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1;
        //if (cat1 != undefined && cat1 != -1)
        //{
        //    var category = helper.findById(cat1, viewModel.menu.Category1);
        //    if (category != undefined && !helper.findCategory(category, item.category1s))
        //    {
        //        item.category1s.push(category);
        //    }
        //}

        if (itemCat1 != undefined && settings.invertItems)
        {
            itemCat1 =
            {
                Id: itemCat1.Id,
                Name: originalItemName,
                AddHAndHCat: itemCat1.AddHAndHCat,
                DealPremium: itemCat1.DealPremium,
                Parent: itemCat1.Parent
            };
        }

        // Does this item have a category 1 (e.g. Size)
        if (itemCat1 != undefined && !helper.findCategory(itemCat1, item.category1s))
        {
            item.category1s.push(itemCat1);
        }

        // Does this item have a category 2 (e.g. Size)
        var cat2 = menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2;
        if (cat2 != undefined && cat2 != -1)
        {
            var category = helper.findById(cat2, viewModel.menu.Category2);
            if (category != undefined && !helper.findCategory(category, item.category2s))
            {
                item.category2s.push(category);
            }
        }

        // Add this menu item to the variations of this particular product
        item.menuItems.push(menuItem);

        // Check to see if there is a thumbnail
        if (item.thumbnail == undefined || item.image == undefined)
        {
            var lookupItem = menuHelper.menuItemLookup[menuItem.MenuId == undefined ? menuItem.Id : menuItem.MenuId];

            item.thumbnail = lookupItem.thumbnail;
            item.thumbnailWidth = lookupItem.thumbnailWidth;
            item.thumbnailHeight = lookupItem.thumbnailHeight;
            item.image = lookupItem.image;
        }

        // Check to see if there is an overloay image
        if (item.overlayImage == undefined || item.overlayImage == undefined)
        {
            var lookupItem = menuHelper.menuItemLookup[menuItem.MenuId == undefined ? menuItem.Id : menuItem.MenuId];

            item.overlayImage = lookupItem.overlayImage;
        }
    },
    sortItemSections: function ()
    {
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];
            if (section.items != undefined)
            {
                section.items.sort(menuHelper.sortByDisplayOrder);
            }
        }
    },
    sortByDisplayOrder: function (a, b)
    {
        return Number(a.displayOrder) > Number(b.displayOrder) ? 1 : -1;
    },
    isItemEnabledForMenu: function (menuItem)
    {
        return menuHelper.isItemAvailable(menuItem);
    },
    isDealItemEnabledForMenu: function (deal)
    {
        return menuHelper.isDealAvailable(deal);
    },
    isItemEnabledForCart: function (menuItem)
    {
        return menuHelper.isItemAvailable(menuItem);
    },
    isDealItemEnabledForCart: function (deal)
    {
        return menuHelper.isDealAvailable(deal.deal.deal) && !deal.minimumOrderValueNotMet();
    },
    getItemToppings: function (menuItem)
    {
        var toppings = [];

        // Removable toppings
        for (var index = 0; index < menuItem.DefTopIds.length; index++)
        {
            var id = menuItem.DefTopIds[index];

            var topping = helper.findByMenuId(id, viewModel.menu.Toppings);
            var price = menuHelper.getToppingPrice(topping);

            var toppingWrapper =
            {
                type: 'removable',
                selectedSingle: ko.observable(true),
                selectedDouble: ko.observable(false),
                topping: topping,
                price: helper.formatPrice(0),
                doublePrice: helper.formatPrice(price),
                cartPrice: topping.price,
                quantity: ko.observable(1)
            };

            menuHelper.addTopping(toppingWrapper, toppings);
        }

        // Additional toppings
        for (var index = 0; index < menuItem.OptTopIds.length; index++)
        {
            var id = menuItem.OptTopIds[index];

            var topping = helper.findByMenuId(id, viewModel.menu.Toppings);
            var price = menuHelper.getToppingPrice(topping);

            var toppingWrapper =
            {
                type: 'additional',
                selectedSingle: ko.observable(false),
                selectedDouble: ko.observable(false),
                topping: topping,
                price: helper.formatPrice(price),
                doublePrice: helper.formatPrice(price * 2),
                cartPrice: topping.price,
                quantity: ko.observable(0)
            };

            menuHelper.addTopping(toppingWrapper, toppings);
        }

        // Sort the toppings
        toppings.sort
        (
            function (a, b)
            {
                var A = (a.ToppingName == undefined ? (a.topping.Name == undefined ? a.topping.ToppingName : a.topping.Name) : a.ToppingName).toLowerCase();
                var B = (b.ToppingName == undefined ? (b.topping.Name == undefined ? b.topping.ToppingName : b.topping.Name) : b.ToppingName).toLowerCase();

                if (A < B)
                {
                    return -1;
                }
                else if (A > B)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        );

        return toppings;
    },
    addTopping: function (topping, toppings)
    {
        var found = false;
        for (var index = 0; index < toppings.length; index++)
        {
            var checkTopping = toppings[index];

            if ((checkTopping.topping.Id == undefined ? checkTopping.topping.MenuId : checkTopping.topping.Id) ==
                (topping.topping.Id == undefined ? topping.topping.MenuId : topping.topping.Id))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            toppings.push(topping);
        }
    },
    getCartDisplayToppings: function (toppings)
    {
        var cartDisplayToppings = [];
        var usedToppings = 0;

        for (var index = 0; index < toppings.length; index++)
        {
            var topping = toppings[index];

            if (topping.type == 'removable')
            {
                if (!topping.selectedSingle() && !topping.selectedDouble())
                {
                    topping.cartName(textStrings.prefixRemove + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.cartPrice(helper.formatPrice(0));
                    topping.cartQuantity('-1');

                    cartDisplayToppings.push(topping);
                }
                else if (topping.selectedDouble())
                {
                    topping.cartName(textStrings.prefixAddTopping + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.cartPrice(helper.formatPrice(menuHelper.getToppingPrice(topping.topping) * topping.quantity));
                    topping.cartQuantity(topping.quantity);

                    cartDisplayToppings.push(topping);
                }
            }
            else
            {
                if (topping.selectedSingle() || topping.selectedDouble())
                {
                    topping.cartName((topping.selectedDouble() ? textStrings.prefixAddDoubleTopping : textStrings.prefixAddTopping) + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.cartPrice(helper.formatPrice(menuHelper.getToppingPrice(topping.topping) * topping.quantity));
                    topping.cartQuantity(topping.quantity);

                    cartDisplayToppings.push(topping);
                }
            }
        }

        return cartDisplayToppings;
    },
    getCartItemDisplayName: function (item)
    {
        var displayName = item.quantity() + ' x ';

        if (item.selectedCategory1() != undefined)
        {
            displayName += item.selectedCategory1().Name;
        }
        if (item.selectedCategory2() != undefined)
        {
            if (displayName.length > 0) { displayName += " "; }
            displayName += item.selectedCategory2().Name;
        }
        if (displayName.length > 0) { displayName += " "; }
        displayName += item.name()

        return displayName;
    }
};

var cartHelper =
{
    refreshCart: function ()
    {
        // Update the deals availability (depending on whether it's a collection or delivery order)
        menuHelper.refreshDealsAvailabilty();

        // Get the order value not including deals with a minimum order value
        var orderTotalForMinimumOrderValue = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice();

        var totalPrice = 0;
        var areDisabledItems = false;
        var checkoutEnabled = false;

        // Update the deal prices in the cart
        for (var index = 0; index < cartHelper.cart().deals().length; index++)
        {
            var deal = cartHelper.cart().deals()[index];

            // Has the minimum order value been met?
            var minimumOrderValueOk = orderTotalForMinimumOrderValue >= deal.deal.deal.MinimumOrderValue;
            deal.minimumOrderValueNotMet(!minimumOrderValueOk);

            // Is the deal enabled in the cart?
            deal.isEnabled(menuHelper.isDealItemEnabledForCart(deal));

            if (deal.isEnabled())
            {
                checkoutEnabled = true;
            }
            else
            {
                areDisabledItems = true;
            }

            // Set the display price depending on the current order type
            deal.displayPrice(helper.formatPrice(deal.price));

            var dealPrice = 0;

            // Does the deal have any deal lines?
            if (deal.dealLines != undefined)
            {
                // Update the prices of the deal lines in the deal
                for (var dealLineIndex = 0; dealLineIndex < deal.dealLines().length; dealLineIndex++)
                {
                    var dealLine = deal.dealLines()[dealLineIndex];
                    var dealLinePrice = menuHelper.calculateDealLinePrice(dealLine);

                    // Get the categories
                    var category1 = helper.findById(dealLine.selectedMenuItem.Cat1 == undefined ? dealLine.selectedMenuItem.Category1 : dealLine.selectedMenuItem.Cat1, viewModel.menu.Category1);
                    var category2 = helper.findById(dealLine.selectedMenuItem.Cat2 == undefined ? dealLine.selectedMenuItem.Category2 : dealLine.selectedMenuItem.Cat2, viewModel.menu.Category2);

                    // Add category deal premiums
                    var categoryPremium = ((category1 == undefined ? 0 : category1.DealPremium) + (category2 == undefined ? 0 : category2.DealPremium));

                    // Set the calculated category premium
                    if (categoryPremium == undefined || categoryPremium == 0)
                    {
                        dealLine.categoryPremium(undefined);
                        dealLine.categoryPremiumName(undefined);
                    }
                    else
                    {
                        dealLine.categoryPremium(helper.formatPrice(categoryPremium));
                        dealLine.categoryPremiumName(textStrings.premiumChargeFor.replace('{item}', category2.Name));
                    }

                    // Add item deal premium
                    var itemPremium = dealLine.selectedMenuItem.DealPricePremiumOverride == 0 ? dealLine.selectedMenuItem.DealPricePremium : dealLine.selectedMenuItem.DealPricePremiumOverride;

                    // Set the calculated item premium
                    if (itemPremium == undefined || itemPremium == 0)
                    {
                        dealLine.itemPremium(undefined);
                        dealLine.itemPremiumName(undefined);
                    }
                    else
                    {
                        dealLine.itemPremium(helper.formatPrice(premium));
                        dealLine.itemPremiumName(textStrings.premiumChargeFor.replace('{item}', dealLine.selectedMenuItem.Name));
                    }

                    // Add the deal line to the deal price
                    dealPrice += dealLinePrice;

                    if (deal.isEnabled())
                    {
                        totalPrice += dealLinePrice + categoryPremium + itemPremium;
                    }

                    // Does the deal line have any toppings?
                    if (dealLine.displayToppings != undefined)
                    {
                        // Update the prices of the toppings in the item
                        for (var toppingIndex = 0; toppingIndex < dealLine.displayToppings().length; toppingIndex++)
                        {
                            var topping = dealLine.displayToppings()[toppingIndex];
                            var toppingPrice = menuHelper.calculateToppingPrice(topping);
                            topping.cartPrice(helper.formatPrice(toppingPrice));

                            if (deal.isEnabled())
                            {
                                totalPrice += toppingPrice;
                            }
                        }
                    }
                }

                deal.price = dealPrice;
                deal.displayPrice(helper.formatPrice(dealPrice));
            }
        }

        // Update the item prices in the cart
        for (var index = 0; index < cartHelper.cart().cartItems().length; index++)
        {
            var item = cartHelper.cart().cartItems()[index];

            // Is the item enabled in the cart?
            item.isEnabled(menuHelper.isItemEnabledForCart(item.menuItem));

            if (item.isEnabled())
            {
                checkoutEnabled = true;
            }

            // Set the price depending on the current order type
            item.price = menuHelper.calculateItemPrice(item.menuItem, item.quantity(), undefined);

            // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
            if (item.isEnabled())
            {
                totalPrice += item.price;
            }
            else
            {
                areDisabledItems = true;
            }

            // Set the display price depending on the current order type
            item.displayPrice(helper.formatPrice(item.price));

            // Does the item have any toppings?
            if (item.displayToppings != undefined)
            {
                var toppingsPrice = 0;

                // Update the prices of the toppings in the item
                for (var toppingIndex = 0; toppingIndex < item.displayToppings().length; toppingIndex++)
                {
                    var topping = item.displayToppings()[toppingIndex];
                    var toppingPrice = menuHelper.calculateToppingPrice(topping);
                    topping.cartPrice(helper.formatPrice(toppingPrice * item.quantity()));

                    toppingsPrice += toppingPrice;
                }

                if (item.isEnabled())
                {
                    totalPrice += (toppingsPrice * item.quantity());
                }
            }
        }

        cartHelper.cart().areDisabledItems(areDisabledItems);

        cartHelper.cart().displayTotalPrice(helper.formatPrice(totalPrice));
        cartHelper.cart().totalPrice(totalPrice);

        // True when there are items (enabled or disabled) in the cart
        cartHelper.cart().hasItems(cartHelper.cart().cartItems().length != 0 || cartHelper.cart().deals().length != 0);

        // Is the delivery charge met?  Don't check if already disabled
        if (checkoutEnabled && viewModel.orderType() == 'delivery')
        {
            checkoutEnabled = cartHelper.cart().totalPrice() >= settings.minimumDeliveryOrder;
        }

        // True when there is at least one enabled item in the cart
        cartHelper.cart().checkoutEnabled(checkoutEnabled);
    },
    getOrderValueExcludingDealsWithAMinimumPrice: function ()
    {
        var totalPrice = 0;

        // Caclulate the deal prices
        for (var index = 0; index < cartHelper.cart().deals().length; index++)
        {
            var deal = cartHelper.cart().deals()[index];

            if (deal.deal.deal.MinimumOrderValue == 0)
            {
                // Does the deal have any deal lines?
                if (deal.dealLines != undefined)
                {
                    // Update the prices of the deal lines in the deal
                    for (var dealLineIndex = 0; dealLineIndex < deal.dealLines().length; dealLineIndex++)
                    {
                        var dealLine = deal.dealLines()[dealLineIndex];
                        var dealLinePrice = menuHelper.calculateDealLinePrice(dealLine);

                        totalPrice += dealLinePrice;
                    }
                }
            }
        }

        // Calculate the item prices
        for (var index = 0; index < cartHelper.cart().cartItems().length; index++)
        {
            var item = cartHelper.cart().cartItems()[index];

            // Set the price depending on the current order type
            var itemPrice = menuHelper.calculateItemPrice(item.menuItem, item.quantity(), undefined);

            // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
            if (menuHelper.isItemAvailable(item.menuItem))
            {
                totalPrice += itemPrice;
            }

            // Does the item have any toppings?
            if (item.displayToppings != undefined)
            {
                // Update the prices of the toppings in the item
                for (var toppingIndex = 0; toppingIndex < item.displayToppings().length; toppingIndex++)
                {
                    var topping = item.displayToppings()[toppingIndex];
                    var toppingPrice = menuHelper.calculateToppingPrice(topping);

                    if (menuHelper.isItemAvailable(item.menuItem))
                    {
                        totalPrice += toppingPrice;
                    }
                }
            }
        }

        return totalPrice;
    },
    clearCart: function ()
    {
        // Clear the cart
        cartHelper.cart().cartItems.removeAll();
        cartHelper.cart().deals.removeAll();

        // Update the total price (probably not needed)
        cartHelper.refreshCart();

        viewModel.timer = new Date();
    },
    checkout: function ()
    {
        // Is the customer logged in?
        if (settings.customerAccountsEnabled && !accountHelper.isLoggedIn())
        {
            // Show the login popup
            accountHelper.showPopup(cartHelper.checkout);
            return;
        }

        // Refresh the delivery/collection slots
        checkoutHelper.refreshTimes();

        if (guiHelper.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('menu', true);
            guiHelper.showView('checkoutView');
        }
        else
        {
            guiHelper.showMenuView('checkoutView');
        }

        checkoutHelper.initialiseCheckout();

        // Switch the cart to checkout mode
        guiHelper.isMobileMenuVisible(false);
        guiHelper.canChangeOrderType(false);
        guiHelper.cartActions(guiHelper.cartActionsCheckout);
    },
    getDealTemplate: function (cartItem)
    {
        if (guiHelper.isCartLocked())
        {
            return 'disabledCartDeal-template';
        }
        else
        {
            return 'cartDeal-template';
        }
    },
    getItemTemplate: function (cartItem)
    {
        if (guiHelper.isCartLocked())
        {
            return 'disabledCartItem-template';
        }
        else
        {
            return 'cartItem-template';
        }
    },
    editCartItem: function ()
    {
        // Get the menu item that the user has selected
        cartHelper.cart().selectedCartItem(this);

        var price = menuHelper.calculateItemPrice(this.menuItem, this.quantity(), this.toppings);

        // The item to show on the toppings popup
        viewModel.selectedItem.name(this.name);
        viewModel.selectedItem.description(menuHelper.fixName(this.menuItem.Desc == undefined ? this.menuItem.Description : this.menuItem.Desc));
        viewModel.selectedItem.quantity(this.quantity());
        viewModel.selectedItem.menuItem(this.menuItem);
        viewModel.selectedItem.instructions(this.instructions);
        viewModel.selectedItem.person(this.person);

        // Copy over the menu items
        viewModel.selectedItem.menuItems.removeAll();
        for (var index = 0; index < this.menuItems.length; index++)
        {
            viewModel.selectedItem.menuItems.push(this.menuItems[index]);
        }

        // Copy over the cat1 and cat2s
        viewModel.ignoreEvents = true;
        viewModel.selectedItem.category1s.removeAll();
        for (var index = 0; index < this.category1s().length; index++)
        {
            viewModel.selectedItem.category1s.push(this.category1s()[index]);
        }
        viewModel.selectedItem.category2s.removeAll();
        for (var index = 0; index < this.category2s().length; index++)
        {
            viewModel.selectedItem.category2s.push(this.category2s()[index]);
        }
        viewModel.ignoreEvents = false;

        viewModel.selectedItem.toppings(this.toppings());
        viewModel.selectedItem.selectedCategory1(this.selectedCategory1);
        viewModel.selectedItem.selectedCategory2(this.selectedCategory2);

        viewModel.selectedItem.price(helper.formatPrice(price));

        // Show the toppings popup
        popupHelper.returnToCart = true; // In mobile mode the cart is a seperate view so we need to show the view
        popupHelper.showPopup('editItem');
    },
    editCartDeal: function ()
    {
        // Get the deal that the user has selected
        cartHelper.cart().selectedCartItem(this);

        // The deal to show on the deal popup
        dealHelper.mode('editDeal');
        dealHelper.hasError(false);

        dealHelper.selectedDeal.name(this.name);
        dealHelper.selectedDeal.description(this.deal.description);

        // Build the deal lines
        dealHelper.selectedDeal.bindableDealLines.removeAll();

        for (var index = 0; index < this.dealLines.length; index++)
        {
            var dealLine = this.dealLines[index];

            var dealLineWrapper =
            {
                dealLine: dealLine.dealLine,
                items: undefined,
                selectedItem: ko.observable(),
                selectedMenuItem: dealLine.selectedMenuItem, // Each item could include multiple menu items for different cat1/cat2s - this is the menu item the customer wants
                hasError: ko.observable(false),
                toppings: dealLine.toppings
            };

            // Build the allowable items in the deal line
            var itemWrappers = [];
            for (var itemIndex = 0; itemIndex < dealLine.dealLine.items.length; itemIndex++)
            {
                var item = dealLine.dealLine.items[itemIndex];
                var itemWrapper =
                {
                    dealLine: dealLineWrapper, // The deal line that the item is in - we're data binding to the item but we need a way to get back to the deal line the item is in
                    item: itemIndex == 0 ? undefined : item, // Ignore the first item - it's always the dummy "plese select an item" item
                    name: item.name
                };

                itemWrappers.push(itemWrapper);
            }

            // Set the allowable items in the deal line
            dealLineWrapper.items = itemWrappers;

            // Add the deal line to the deal
            dealHelper.selectedDeal.dealLines.push(dealLineWrapper);
        }

        // Show the deal popup
        dealPopupHelper.returnToCart = true;  // When finished return to the cart (mobile only)
        dealPopupHelper.showDealPopup('editDeal', true);

        // Do this last as it triggers knockout
        dealHelper.selectedDeal.deal = this.deal;

        // Let knockout sort out the GUI before subscribing to events as the act of building the GUI triggers these events which we're not interested in
        setTimeout
        (
            function ()
            {
                dealHelper.subscribeToDealLineChanges();
            },
            0
        );
    },
    cart: ko.observable
    (
        {
            displayTotalPrice: ko.observable(),
            cartItems: ko.observableArray(),
            totalPrice: ko.observable(),
            selectedCartItem: ko.observable({}),
            mercuryPaymentId: ko.observable(),
            dataCashPaymentDetails: ko.observable(),
            deals: ko.observableArray(),
            areDisabledItems: ko.observable(),
            checkoutEnabled: ko.observable(),
            hasItems: ko.observable()
        }
    ),
    getCartItemName: function (menuItem, selectedCategory1, selectedCategory2)
    {
        // Build the item name for the cart (product name + category names)
        var itemCategoriesText = "";

        if (selectedCategory1 != undefined)
        {
            itemCategoriesText += " (" + selectedCategory1.Name;
        }

        if (selectedCategory2 != undefined)
        {
            if (itemCategoriesText.length > 0) itemCategoriesText += ", ";

            itemCategoriesText += selectedCategory2.Name;
        }

        if (itemCategoriesText.length > 0) itemCategoriesText += ")";

        return (typeof (menuItem.name) == 'function' ? menuItem.name() : menuItem.name) + itemCategoriesText;
    },
    orderTypeChanged: function ()
    {
        // Update the cart prices and availability
        cartHelper.refreshCart();

        return true;
    }
};

// Checkout
var checkoutHelper =
{
    clearCheckout: function ()
    {
        checkoutHelper.checkoutDetails.firstName('');
        checkoutHelper.checkoutDetails.surname('');
        checkoutHelper.checkoutDetails.telephoneNumber('');
        checkoutHelper.checkoutDetails.emailAddress('');
        checkoutHelper.checkoutDetails.deliveryTime('');
        checkoutHelper.checkoutDetails.payNow = false;
        checkoutHelper.checkoutDetails.address.prem1('');
        checkoutHelper.checkoutDetails.address.prem2('');
        checkoutHelper.checkoutDetails.address.prem3('');
        checkoutHelper.checkoutDetails.address.prem4('');
        checkoutHelper.checkoutDetails.address.prem5('');
        checkoutHelper.checkoutDetails.address.prem6('');
        checkoutHelper.checkoutDetails.address.org1('');
        checkoutHelper.checkoutDetails.address.org2('');
        checkoutHelper.checkoutDetails.address.org3('');
        checkoutHelper.checkoutDetails.address.roadNumber('');
        checkoutHelper.checkoutDetails.address.roadName('');
        checkoutHelper.checkoutDetails.address.city('');
        checkoutHelper.checkoutDetails.address.town('');
        checkoutHelper.checkoutDetails.address.zipCode('');
        checkoutHelper.checkoutDetails.address.county('');
        checkoutHelper.checkoutDetails.address.state('');
        checkoutHelper.checkoutDetails.address.locality('');
        checkoutHelper.checkoutDetails.address.country('');
        checkoutHelper.checkoutDetails.address.directions('');
        checkoutHelper.checkoutDetails.address.userLocality1('');
        checkoutHelper.checkoutDetails.address.userLocality2('');
        checkoutHelper.checkoutDetails.address.userLocality3('');
        checkoutHelper.checkoutDetails.orderNotes('');
        checkoutHelper.checkoutDetails.chefNotes('');
    },
    initialiseCheckout: function ()
    {
        checkoutHelper.nextButton.displayText(textStrings.next);
        checkoutHelper.backButton.displayText(textStrings.back);

        if (settings.customerAccountsEnabled && accountHelper.customerDetails != undefined)
        {
            // Pre-fill the checkout details from the customers account details
            checkoutHelper.checkoutDetails.firstName(accountHelper.customerDetails.firstname);
            checkoutHelper.checkoutDetails.surname(accountHelper.customerDetails.surname);
            checkoutHelper.checkoutDetails.emailAddress(accountHelper.emailAddress);

            // Are there any contact details in the customers profile?
            if (accountHelper.customerDetails.contacts != undefined)
            {
                for (var index = 0; index < accountHelper.customerDetails.contacts.length; index++)
                {
                    var contact = accountHelper.customerDetails.contacts[index];

                    if (contact.type == 'Email')
                    {
                        checkoutHelper.checkoutDetails.emailAddress(contact.value);
                    }
                    else if (contact.type == 'Mobile')
                    {
                        checkoutHelper.checkoutDetails.telephoneNumber(contact.value);
                        checkoutHelper.checkoutDetails.marketing(contact.marketingLevel == '3rdParty');
                    }
                }
            }

            // Is there a saved address?
            if (viewModel.orderType() == 'delivery' && accountHelper.customerDetails.address != undefined)
            {
                checkoutHelper.checkoutDetails.address.prem1(accountHelper.customerDetails.address.prem1);
                checkoutHelper.checkoutDetails.address.prem2(accountHelper.customerDetails.address.prem2);
                checkoutHelper.checkoutDetails.address.prem3(accountHelper.customerDetails.address.prem3);
                checkoutHelper.checkoutDetails.address.prem4(accountHelper.customerDetails.address.prem4);
                checkoutHelper.checkoutDetails.address.prem5(accountHelper.customerDetails.address.prem5);
                checkoutHelper.checkoutDetails.address.prem6(accountHelper.customerDetails.address.prem6);
                checkoutHelper.checkoutDetails.address.org1(accountHelper.customerDetails.address.org1);
                checkoutHelper.checkoutDetails.address.org2(accountHelper.customerDetails.address.org2);
                checkoutHelper.checkoutDetails.address.org3(accountHelper.customerDetails.address.org3);
                checkoutHelper.checkoutDetails.address.roadNumber(accountHelper.customerDetails.address.roadNum);
                checkoutHelper.checkoutDetails.address.roadName(accountHelper.customerDetails.address.roadName);
                checkoutHelper.checkoutDetails.address.city(accountHelper.customerDetails.address.city);
                checkoutHelper.checkoutDetails.address.town(accountHelper.customerDetails.address.town);
                checkoutHelper.checkoutDetails.address.zipCode(accountHelper.customerDetails.address.postcode);
                checkoutHelper.checkoutDetails.address.county(accountHelper.customerDetails.address.county);
                checkoutHelper.checkoutDetails.address.state(accountHelper.customerDetails.address.state);
                checkoutHelper.checkoutDetails.address.locality(accountHelper.customerDetails.address.locality);
                checkoutHelper.checkoutDetails.address.directions(accountHelper.customerDetails.address.directions);
                checkoutHelper.checkoutDetails.address.userLocality1(accountHelper.customerDetails.address.userLocality1);
                checkoutHelper.checkoutDetails.address.userLocality2(accountHelper.customerDetails.address.userLocality2);
                checkoutHelper.checkoutDetails.address.userLocality3(accountHelper.customerDetails.address.userLocality3);
            }
        }
        else
        {
            checkoutHelper.checkoutDetails.firstName(queryStringHelper.firstName == undefined ? '' : queryStringHelper.firstName);
            checkoutHelper.checkoutDetails.surname(queryStringHelper.lastName == undefined ? '' : queryStringHelper.lastName);
            checkoutHelper.checkoutDetails.emailAddress(queryStringHelper.email == undefined ? '' : queryStringHelper.email);
            checkoutHelper.checkoutDetails.telephoneNumber(queryStringHelper.telephoneNumber == undefined ? '' : queryStringHelper.telephoneNumber);
            checkoutHelper.checkoutDetails.marketing(queryStringHelper.marketing == undefined ? '' : queryStringHelper.marketing);
            checkoutHelper.checkoutDetails.address.town(queryStringHelper.town == undefined ? '' : queryStringHelper.town);
            checkoutHelper.checkoutDetails.address.city(queryStringHelper.town == undefined ? '' : queryStringHelper.town);
            checkoutHelper.checkoutDetails.address.zipCode(queryStringHelper.postcode == undefined ? '' : queryStringHelper.postcode);
            checkoutHelper.checkoutDetails.address.roadNumber(queryStringHelper.houseNumber == undefined ? '' : queryStringHelper.houseNumber);
            checkoutHelper.checkoutDetails.address.roadName(queryStringHelper.roadName == undefined ? '' : queryStringHelper.roadName);
        }

        // Use the store country
        // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
        // profile cannot be sent in the order JSON.  There is a hard coded country code for that.  We use proper ISO codes for the customer profile.
        checkoutHelper.checkoutDetails.address.country(viewModel.siteDetails().address.country);

        // Clear the checkout sections (tabs)
        checkoutHelper.checkoutSections.removeAll();

        // The page to add
        var index = 0;

        // Who are you
        if (settings.displayCustomerDetailsPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutContactDetailsContainer', index: index, templateName: 'checkoutContactDetails-template', displayName: (index + 1) + ') ' + textStrings.whoAreYou, validate: checkoutHelper.validateContactDetails });
            index++;
        }

        // Collection/delivery time
        if (viewModel.orderType() == 'collection')
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', index: index, templateName: 'checkoutDeliveryTime-template', displayName: (index + 1) + ') ' + textStrings.collectionTime, validate: checkoutHelper.validateDeliveryTime });
            index++;
        }
        else
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', index: index, templateName: 'checkoutDeliveryTime-template', displayName: (index + 1) + ') ' + textStrings.deliveryTime, validate: checkoutHelper.validateDeliveryTime });
            index++;
        }

        // Delivery address
        if (viewModel.orderType() == 'delivery' && settings.displayCustomerAddressPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryAddressContainer', index: index, templateName: 'checkoutDeliveryAddress-template', displayName: (index + 1) + ') ' + textStrings.deliveryAddress, validate: checkoutHelper.validateAddress });
            index++;
        }

        // Order notes
        checkoutHelper.checkoutSections.push({ id: 'checkoutOrderNotesContainer', index: index, templateName: 'checkoutOrderNotes-template', displayName: (index + 1) + ') ' + textStrings.orderNotes, validate: function () { return true; } });
        index++;

        // Show the first section
        checkoutHelper.showCheckoutSection(0);
    },
    showCheckoutSection: function (sectionIndex)
    {
        // Configure back button
        if (sectionIndex == 0)
        {
            checkoutHelper.backButton.visible(false);
            checkoutHelper.backButton.action = 'none';
        }
        else
        {
            checkoutHelper.backButton.visible(true);
            checkoutHelper.backButton.action = sectionIndex - 1;
        }

        // Configure next button
        if (sectionIndex == checkoutHelper.checkoutSections().length - 1)
        {
            checkoutHelper.nextButton.displayText(textStrings.placeOrder);
            checkoutHelper.nextButton.action = 'placeorder';
        }
        else
        {
            checkoutHelper.nextButton.displayText(textStrings.next);
            checkoutHelper.nextButton.action = sectionIndex + 1;
        }

        for (var index = 0; index < checkoutHelper.checkoutSections().length; index++)
        {
            var checkoutSection = checkoutHelper.checkoutSections()[index];

            if (checkoutSection.index == sectionIndex || guiHelper.isMobileMode())
            {
                // This section should be visible
                $('#' + checkoutSection.id).css('display', 'block');
            }
            else
            {
                // Hide this section
                $('#' + checkoutSection.id).css('display', 'none');
            }
        }

        // Used to highlight the correct section
        checkoutHelper.visibleCheckoutSection(sectionIndex);

        // Don't let the user modify the cart items
        guiHelper.isCartLocked(true);
    },
    refreshTimes: function ()
    {
        checkoutHelper.times.removeAll();
        checkoutHelper.times.push({ mode: undefined, time: undefined, text: textStrings.pleaseSelectATime });

        // Is the store open for ASAP?
        //if (viewModel.selectedSite().estDelivTime == undefined || viewModel.selectedSite().estDelivTime == 0)
        //{
        //    checkoutHelper.times.push({ mode: 'ASAP', time: undefined, text: textStrings.asSoonAsPossibleNoETD });
        //}
        //else
        //{
        //    checkoutHelper.times.push({ mode: 'ASAP', time: undefined, text: textStrings.asSoonAsPossible + ' (' + viewModel.selectedSite().estDelivTime + ')' });
        //}

        // Get todays opening times
        var openingTimes = openingTimesHelper.getTodaysOpeningTimes();

        var timeBlocks = [];
        var today = new Date();

        // The first available slot has to be at least EDT minutes from now.
        // If EDT is not available default to settings.defaultETD minutes.
        var offset = settings.defaultETD;
        if (viewModel.selectedSite().estDelivTime != undefined && viewModel.selectedSite().estDelivTime > 0)
        {
            offset = viewModel.selectedSite().estDelivTime;
        }

        // The earliest time that an order can be delivered/collected
        var today = new Date(new Date().getTime() + offset * 60000);

        var hourNow = today.getHours();
        var minuteNow = today.getMinutes();

        // Go through todays delivery times and get the delivery times after now
        for (var timespanIndex = 0; timespanIndex < openingTimes.length; timespanIndex++)
        {
            var timeSpan = openingTimes[timespanIndex];

            if (timeSpan.openAllDay)
            {
                // Store is open all day so delivery times are from now to midnight
                timeBlocks.push({ startHour: hourNow, startMinute: minuteNow, endHour: 0, endMinute: 0 });
                break;
            }
            else
            {
                // Work out if this opening time block is before now, after now or overlaps now
                var startBits = timeSpan.startTime.split(':');
                var startHour = Number(startBits[0]);
                var startMinute = Number(startBits[1]);

                var endBits = timeSpan.endTime.split(':');
                var endHour = Number(endBits[0]);
                var endMinute = Number(endBits[1]);

                if (startHour > hourNow || (startHour == hourNow && startMinute > minuteNow))
                {
                    // Time block is in the future
                    timeBlocks.push({ startHour: startHour, startMinute: startMinute, endHour: endHour, endMinute: endMinute });
                }
                else if (endHour > hourNow || (endHour == hourNow && endMinute > minuteNow))
                {
                    // Time block has already started but not ended
                    timeBlocks.push({ startHour: hourNow, startMinute: minuteNow, endHour: endHour, endMinute: endMinute });
                }
            }
        }

        var slotSize = 15;

        // Generate delivery slots for each block of time the store is open
        for (var timeBlockIndex = 0; timeBlockIndex < timeBlocks.length; timeBlockIndex++)
        {
            var timeSpan = timeBlocks[timeBlockIndex];

            // Add slots in start hour
            checkoutHelper.addDeliveryHourSlots(timeSpan.startHour, timeSpan.startMinute, timeSpan.endHour == timeSpan.startHour ? timeSpan.endMinute : 60, slotSize);

            // Add slots for the each hour inbetween the start and end hours
            var endHour = timeSpan.endHour == 0 ? 24 : timeSpan.endHour;
            for (var hour = timeSpan.startHour + 1; hour < endHour; hour++)
            {
                checkoutHelper.addDeliveryHourSlots(hour, 0, 60, slotSize);
            }

            if (timeSpan.endHour > timeSpan.startHour)
            {
                // Add slots for the last hour
                checkoutHelper.addDeliveryHourSlots(timeSpan.endHour, 0, timeSpan.endMinute, slotSize);
            }
        }
    },
    addDeliveryHourSlots: function (hour, startMinute, endMinute, slotSize)
    {
        // Get the first and last slots for this hour
        var startSlot = Math.ceil(startMinute / slotSize) * slotSize;
        var endSlot = Math.ceil(endMinute / slotSize) * slotSize;

        // Get all the slots between the start and end minutes
        for (var slot = startSlot; slot < endSlot; slot += slotSize)
        {
            checkoutHelper.addDeliverySlot(hour, slot, slotSize);
        }
    },
    addDeliverySlot: function (hour, minute, slotSize)
    {
        var hour12HourClock = hour > 12 ? hour - 12 : hour;
        var hourAMPM = hour > 12 ? 'pm' : 'am';
        var hourPlusOne = (hour + 1) > 23 ? 0 : (hour + 1);
        var hourPlusOne12HourClock = hourPlusOne > 12 ? hourPlusOne - 12 : hourPlusOne;
        var hourPlusOneAMPM = hourPlusOne > 12 ? 'pm' : 'am';

        if (minute + slotSize > 59)
        {
            // Slot overlaps into the next hour
            checkoutHelper.times.push
            (
                {
                    mode: 'TIMED',
                    time: hour + ':' + minute,
                    text: checkoutHelper.formatHour(helper.use24hourClock ? hour : hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + (helper.use24hourClock ? '' : hourAMPM) + ' - ' +
                          checkoutHelper.formatHour(helper.use24hourClock ? hour : hourPlusOne12HourClock) + ':' + checkoutHelper.formatMinute((slotSize - (60 - minute))) + (helper.use24hourClock ? '' : hourPlusOneAMPM)
                }
            );
        }
        else
        {
            // Slot is entirely within the hour
            checkoutHelper.times.push
            (
                {
                    mode: 'TIMED',
                    time: hour + ':' + minute,
                    text: checkoutHelper.formatHour(helper.use24hourClock ? hour : hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + (helper.use24hourClock ? '' : hourAMPM) + ' - ' +
                          checkoutHelper.formatHour(helper.use24hourClock ? hour : hour12HourClock) + ':' + checkoutHelper.formatMinute((minute + slotSize)) + (helper.use24hourClock ? '' : hourAMPM)
                }
            );
        }
    },
    formatMinute: function (minute)
    {
        if (minute < 10)
            return '0' + minute;
        else
            return minute;
    },
    formatHour: function (hour)
    {
        if (hour < 10)
            return ' ' + hour;
        else
            return hour;
    },
    visibleCheckoutSection: ko.observable(0),
    times: ko.observableArray(),
    checkoutDetails:
    {
        firstName: ko.observable(''),
        surname: ko.observable(''),
        telephoneNumber: ko.observable(''),
        emailAddress: ko.observable(''),
        deliveryTime: ko.observable(undefined),
        address:
        {
            prem1: ko.observable(''),
            prem2: ko.observable(''),
            prem3: ko.observable(''),
            prem4: ko.observable(''),
            prem5: ko.observable(''),
            prem6: ko.observable(''),
            org1: ko.observable(''),
            org2: ko.observable(''),
            org3: ko.observable(''),
            roadNumber: ko.observable(''),
            roadName: ko.observable(''),
            city: ko.observable(''),
            town: ko.observable(''),
            zipCode: ko.observable(''),
            county: ko.observable(''),
            state: ko.observable(''),
            locality: ko.observable(''),
            country: ko.observable(''),
            directions: ko.observable(''),
            userLocality1: ko.observable(''),
            userLocality2: ko.observable(''),
            userLocality3: ko.observable('')
        },
        payNow: false,
        marketing: ko.observable('true'),
        wantedTime: ko.observable(undefined),
        orderNotes: ko.observable(''),
        chefNotes: ko.observable(''),
        rememberAddress: true,
        rememberContactDetails: true
    },
    placeOrder: function ()
    {
        // Has the user entered the required information?
        if (checkoutHelper.validate())
        {
            // Do we need to save the contact details
            if (settings.customerAccountsEnabled && checkoutHelper.checkoutDetails.rememberContactDetails)
            {
                // Update the contact details
                accountHelper.customerDetails.firstName = checkoutHelper.checkoutDetails.firstName();
                accountHelper.customerDetails.surname = checkoutHelper.checkoutDetails.surname();
                accountHelper.customerDetails.telephoneNumber = checkoutHelper.checkoutDetails.telephoneNumber();
            }

            // Do we need to save the address in the customers details?
            if (settings.customerAccountsEnabled && checkoutHelper.checkoutDetails.rememberAddress)
            {
                // Update the address details
                accountHelper.customerDetails.address =
                {
                    prem1: checkoutHelper.checkoutDetails.address.prem1(),
                    prem2: checkoutHelper.checkoutDetails.address.prem2(),
                    prem3: checkoutHelper.checkoutDetails.address.prem3(),
                    prem4: checkoutHelper.checkoutDetails.address.prem4(),
                    prem5: checkoutHelper.checkoutDetails.address.prem5(),
                    prem6: checkoutHelper.checkoutDetails.address.prem6(),
                    org1: checkoutHelper.checkoutDetails.address.org1(),
                    org2: checkoutHelper.checkoutDetails.address.org2(),
                    org3: checkoutHelper.checkoutDetails.address.org3(),
                    roadNum: checkoutHelper.checkoutDetails.address.roadNumber(),
                    roadName: checkoutHelper.checkoutDetails.address.roadName(),
                    city: checkoutHelper.checkoutDetails.address.city(),
                    town: checkoutHelper.checkoutDetails.address.town(),
                    postcode: checkoutHelper.checkoutDetails.address.zipCode(),
                    county: checkoutHelper.checkoutDetails.address.county(),
                    state: checkoutHelper.checkoutDetails.address.state(),
                    locality: checkoutHelper.checkoutDetails.address.locality(),
                    country: checkoutHelper.checkoutDetails.address.country(),
                    directions: checkoutHelper.checkoutDetails.address.directions(),
                    userLocality1: checkoutHelper.checkoutDetails.address.userLocality1(),
                    userLocality2: checkoutHelper.checkoutDetails.address.userLocality2(),
                    userLocality3: checkoutHelper.checkoutDetails.address.userLocality3()
                };

                // Find the existing email contact - we need the marketing level
                var emailMarketingLevel = '3rdParty';
                for (var index = 0; index < accountHelper.customerDetails.contacts.length; index++)
                {
                    var contact = accountHelper.customerDetails.contacts[index];
                    if (contact.type == 'Email')
                    {
                        emailMarketingLevel = contact.marketingLevel;
                        break;
                    }
                }

                // Clear out the contacts
                accountHelper.customerDetails.contacts = [];

                // Add an email contact
                accountHelper.customerDetails.contacts.push
                (
                    {
                        type: 'Email',
                        value: accountHelper.emailAddress,
                        marketingLevel: emailMarketingLevel
                    }
                );

                // Is there already phone contact?
                if (checkoutHelper.checkoutDetails.telephoneNumber() != undefined && checkoutHelper.checkoutDetails.telephoneNumber().length > 0)
                {
                    // Add a phone contact
                    accountHelper.customerDetails.contacts.push
                    (
                        {
                            type: 'Mobile',
                            value: checkoutHelper.checkoutDetails.telephoneNumber(),
                            marketingLevel: checkoutHelper.checkoutDetails.marketing() ? '3rdParty' : 'OrderOnly'
                        }
                    );
                }

                // Save the customer account details
                acsapi.postCustomer
                (
                    accountHelper.emailAddress,
                    accountHelper.password,
                    accountHelper.customerDetails,
                    checkoutHelper.showPaymentPicker
                );
            }
            else
            {
                // Show the payment page
                checkoutHelper.showPaymentPicker();
            }
        }
    },
    showPaymentPicker: function ()
    {
        if (guiHelper.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('checkout', false);

            guiHelper.showView('paymentView');
        }
        else
        {
            guiHelper.showMenuView('paymentView');
        }

        // Switch the cart to checkout mode
        guiHelper.cartActions(guiHelper.cartActionsCheckout);

        if (settings.termsAndConditionsEnabled)
        {
            // Reset the agree flag - assume they haven't agreed yet
            tandcHelper.agree(undefined);
        }
        else
        {
            // There are no terms and conditions to agree to
            tandcHelper.agree(true);
        }

        guiHelper.canChangeOrderType(false);
    },
    payAtDoor: function ()
    {
        // Must agree to T&C before proceeding
        if (!tandcHelper.hasAgreed()) return;

        // Hide the mobile checkout menu
        checkoutMenuHelper.hideMenu();

        // Customer wants to pay now
        checkoutHelper.checkoutDetails.payNow = false;

        checkoutHelper.sendOrderToStore();
    },
    payNow: function ()
    {
        // Must agree to T&C before proceeding
        if (!tandcHelper.hasAgreed()) return;

        guiHelper.canChangeOrderType(false);

        // Customer wants to pay now
        checkoutHelper.checkoutDetails.payNow = true;

        viewModel.pleaseWaitMessage(textStrings.preparingForPayment);
        viewModel.pleaseWaitProgress('');
        guiHelper.showView('pleaseWaitView');

        if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
        {
            // Initialise the mercury payment
            acsapi.putMercuryPayment
            (
                viewModel.selectedSite().siteId,
                function ()
                {
                    if (guiHelper.isMobileMode())
                    {
                        guiHelper.isMobileMenuVisible(false);
                        checkoutMenuHelper.showMenu('pay', false);
                        guiHelper.showView('mercuryPaymentView');
                    }
                    else
                    {
                        guiHelper.showMenuView('mercuryPaymentView');
                    }

                    var iFrame = $('#mercuryPaymentIFrame');
                    iFrame.attr('src', 'https://hc.mercurydev.net/CheckoutIFrame.aspx?pid=' + cartHelper.cart().mercuryPaymentId());

                    iFrame.load
                    (
                        function ()
                        {
                            if (iFrame.contents().get(0) != undefined)
                            {
                                if (iFrame.contents().get(0).location.href)
                                {
                                    checkoutHelper.sendOrderToStore();
                                }
                            }
                        }
                    );
                }
            );
        }
        else if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHLIVE' ||
            viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHTEST')
        {
            // Initialise the DataCash payment
            acsapi.putDataCashPayment
            (
                viewModel.selectedSite().siteId,
                function ()
                {
                    if (guiHelper.isMobileMode())
                    {
                        guiHelper.isMobileMenuVisible(false);
                        checkoutMenuHelper.showMenu('pay', false);
                        guiHelper.showView('dataCashPaymentView');
                    }
                    else
                    {
                        guiHelper.showMenuView('dataCashPaymentView');
                    }

                    var iFrame = $('#dataCashPaymentIFrame');
                    iFrame.attr('src', cartHelper.cart().dataCashPaymentDetails().url + '?HPS_SessionID=' + cartHelper.cart().dataCashPaymentDetails().sessionId);

                    iFrame.load
                    (
                        function ()
                        {
                            if (iFrame.contents().get(0) != undefined)
                            {
                                if (iFrame.contents().get(0).location.href)
                                {
                                    checkoutHelper.sendOrderToStore();
                                }
                            }
                        }
                    );
                }
            );
        }
        else if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCANETLIVE' ||
        viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCANETTEST')
        {
            // Generate the order JSON
            var orderDetails = checkoutHelper.generateOrderJson();

            // Initialise the Mercanet payment
            acsapi.putMercanetPayment
            (
                viewModel.selectedSite().siteId,
                orderDetails,
                function (json)
                {
                    var jsonObject = jQuery.parseJSON(json);
                    var html = jsonObject.Html;
                    var orderRef = jsonObject.OrderReference;

                    // We need to save the order reference in the cookie as we're about to get redirected to another webpage and we'll lose state
                    cookieHelper.setCookie('or', jsonObject.OrderReference);

                    if (guiHelper.isMobileMode())
                    {
                        guiHelper.isMobileMenuVisible(false);
                        checkoutMenuHelper.showMenu('pay', false);
                        guiHelper.showView('mercanetPaymentView');
                    }
                    else
                    {
                        guiHelper.showMenuView('mercanetPaymentView');
                    }

                    // Inject the Mercanet HTML into the iFrame
                    var iFrame = $('#mercanetPaymentIFrame');

                    if (html == undefined || html.length == 0)
                    {
                        html = "Sorry there was problem.  Payment is unavailable at this time.";
                    }

                    var iFrameDoc = iFrame[0].contentDocument || iFrame[0].contentWindow.document;
                    iFrameDoc.write(html);
                    iFrameDoc.close();
                }
            );
        }
        else
        {
            // There is a problem with the payment provider - not supported
            guiHelper.showHome();
        }
    },
    sendOrderToStore: function ()
    {
        // Hide the mobile checkout menu
        checkoutMenuHelper.hideMenu();

        viewModel.pleaseWaitMessage(textStrings.sendingOrderToStore);
        viewModel.pleaseWaitProgress('');
        guiHelper.showView('pleaseWaitView');

        // Generate the order JSON
        var orderDetails = checkoutHelper.generateOrderJson();

        // Send the order to ACS
        acsapi.putOrder(viewModel.selectedSite().siteId, orderDetails);
    },
    generateOrderJson: function ()
    {
        // Work out how long the customer took to place the order
        var timeNow = new Date();
        var timeTakenMilliseconds = Math.abs(timeNow - viewModel.timer);
        var timeTakenSeconds = Math.round(timeTakenMilliseconds / 1000);

        var orderDetails =
        {
            toSiteId: viewModel.selectedSite().siteId,
            paymentType: checkoutHelper.checkoutDetails.payNow ? viewModel.siteDetails().paymentProvider : 'PayLater',
            paymentData: undefined,
            order:
            {
                partnerReference: '',
                type: viewModel.orderType(),
                orderTimeType: checkoutHelper.checkoutDetails.wantedTime().mode,
                orderWantedTime: checkoutHelper.checkoutDetails.wantedTime().mode == 'ASAP' ? helper.formatUTCDate(new Date()) : helper.formatUTCSlot(checkoutHelper.checkoutDetails.wantedTime().time),
                orderPlacedTime: helper.formatUTCDate(new Date()),
                timeToTake: timeTakenSeconds,
                chefNotes: checkoutHelper.checkoutDetails.chefNotes(),
                oneOffDirections: checkoutHelper.checkoutDetails.orderNotes(),
                estimatedDeliveryTime: 0,
                customer:
                {
                    title: '',
                    firstName: checkoutHelper.checkoutDetails.firstName(),
                    surname: checkoutHelper.checkoutDetails.surname(),
                    contacts:
                    [
                        {
                            type: 'Mobile',
                            value: checkoutHelper.checkoutDetails.telephoneNumber(),
                            marketingLevel: checkoutHelper.checkoutDetails.marketing() ? 'OrderOnly' : 'None'
                        },
                        {
                            type: 'Email',
                            value: checkoutHelper.checkoutDetails.emailAddress(),
                            marketingLevel: checkoutHelper.checkoutDetails.marketing() ? 'OrderOnly' : 'None'
                        }
                    ],
                    address:
                    {
                        prem1: checkoutHelper.checkoutDetails.address.prem1().length == 0 ? '' : textStrings.prem1Prefix + checkoutHelper.checkoutDetails.address.prem1(),
                        prem2: checkoutHelper.checkoutDetails.address.prem2().length == 0 ? '' : textStrings.prem2Prefix + checkoutHelper.checkoutDetails.address.prem2(),
                        prem3: checkoutHelper.checkoutDetails.address.prem3().length == 0 ? '' : textStrings.prem3Prefix + checkoutHelper.checkoutDetails.address.prem3(),
                        prem4: checkoutHelper.checkoutDetails.address.prem4(),
                        prem5: checkoutHelper.checkoutDetails.address.prem5(),
                        prem6: checkoutHelper.checkoutDetails.address.prem6(),
                        org1: checkoutHelper.checkoutDetails.address.org1(),
                        org2: checkoutHelper.checkoutDetails.address.org2(),
                        org3: checkoutHelper.checkoutDetails.address.org3(),
                        roadNumber: checkoutHelper.checkoutDetails.address.roadNumber(),
                        roadName: checkoutHelper.checkoutDetails.address.roadName(),
                        city: checkoutHelper.checkoutDetails.address.city(),
                        town: checkoutHelper.checkoutDetails.address.town(),
                        zipCode: checkoutHelper.checkoutDetails.address.zipCode(),
                        county: checkoutHelper.checkoutDetails.address.county(),
                        state: checkoutHelper.checkoutDetails.address.state(),
                        locality: checkoutHelper.checkoutDetails.address.locality(),
                        directions: checkoutHelper.checkoutDetails.address.directions(),
                        userLocality1: checkoutHelper.checkoutDetails.address.userLocality1().length == 0 ? '' : textStrings.userLocality1Prefix + checkoutHelper.checkoutDetails.address.userLocality1(),
                        userLocality2: checkoutHelper.checkoutDetails.address.userLocality2().length == 0 ? '' : textStrings.userLocality2Prefix + checkoutHelper.checkoutDetails.address.userLocality2(),
                        userLocality3: checkoutHelper.checkoutDetails.address.userLocality3().length == 0 ? '' : textStrings.userLocality3Prefix + checkoutHelper.checkoutDetails.address.userLocality3(),
                        // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
                        // profile or the stores country cannot be sent in the order JSON.  There is a hard coded country code for that.
                        country: settings.customerAddressCountryCode
                    },
                    accountNumber: accountHelper.customerDetails == undefined ? '' : accountHelper.customerDetails.accountNumber
                },
                pricing:
                {
                    priceBeforeDiscount: cartHelper.cart().totalPrice(),
                    pricesIncludeTax: 'false',
                    priceAfterDiscount: cartHelper.cart().totalPrice()
                },
                orderLines:
                [
                ],
                orderPayments:
                [
                ]
            }
        };

        // Pass back any payment reference numbers or info
        if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
        {
            orderDetails.paymentData = cartHelper.cart().mercuryPaymentId();
        }
        else if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHLIVE' || viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHTEST')
        {
            orderDetails.paymentData = cartHelper.cart().dataCashPaymentDetails();
        }

        var orderLineNumber = 0;

        // Deals
        for (var index = 0; index < cartHelper.cart().deals().length; index++)
        {
            var deal = cartHelper.cart().deals()[index];

            if (deal.isEnabled())
            {
                var orderLine =
                {
                    productId: deal.deal.deal.Id,
                    quantity: 1,
                    price: deal.price,
                    name: deal.name,
                    orderLineIndex: orderLineNumber,
                    lineType: 0, // Deal
                    instructions: '',
                    person: '',
                    inDealFlag: false,
                    addToppings:
                    [
                    ],
                    removeToppings:
                    [
                    ]
                };

                // We've generated an order line
                orderDetails.order.orderLines.push(orderLine);

                // Next order line
                orderLineNumber++;

                // Deal lines
                for (var dealLineIndex = 0; dealLineIndex < deal.dealLines().length; dealLineIndex++)
                {
                    var dealLine = deal.dealLines()[dealLineIndex];

                    var orderLine =
                    {
                        productId: dealLine.selectedMenuItem.Id,
                        quantity: 1,
                        price: dealLine.price,
                        name: dealLine.name,
                        orderLineIndex: orderLineNumber,
                        lineType: 0, // Normal item (deal line)
                        instructions: dealLine.instructions,
                        person: dealLine.person,
                        inDealFlag: true,
                        addToppings:
                        [
                        ],
                        removeToppings:
                        [
                        ]
                    };

                    // Add/remove toppings
                    for (var toppingIndex = 0; toppingIndex < dealLine.displayToppings().length; toppingIndex++)
                    {
                        var topping = dealLine.displayToppings()[toppingIndex];

                        if (topping.cartQuantity() < 0)
                        {
                            // Default topping has been removed
                            orderLine.removeToppings.push
                            (
                                {
                                    productId: topping.topping.Id,
                                    quantity: 1,
                                    name: topping.topping.ToppingName,
                                    price: 0
                                }
                            );
                        }
                        else if (topping.cartQuantity() > 0)
                        {
                            // Topping added
                            orderLine.addToppings.push
                            (
                                {
                                    productId: topping.topping.Id,
                                    quantity: topping.cartQuantity(),
                                    name: topping.topping.ToppingName,
                                    price: menuHelper.getToppingPrice(topping.topping) * topping.quantity
                                }
                            );
                        }
                    }

                    // We've generated an order line
                    orderDetails.order.orderLines.push(orderLine);

                    // Next order line
                    orderLineNumber++;
                }
            }
        }

        // Order lines
        for (var index = 0; index < cartHelper.cart().cartItems().length; index++)
        {
            var cartItem = cartHelper.cart().cartItems()[index];

            if (cartItem.isEnabled())
            {
                var orderLine =
                {
                    productId: cartItem.menuItem.Id == undefined ? cartItem.menuItem.MenuId : cartItem.menuItem.Id,
                    quantity: cartItem.quantity(),
                    price: cartItem.price,
                    name: cartItem.name,
                    orderLineIndex: orderLineNumber,
                    lineType: 1, // Normal item
                    instructions: cartItem.instructions,
                    person: cartItem.person,
                    inDealFlag: false,
                    addToppings:
                    [
                    ],
                    removeToppings:
                    [
                    ]
                };

                // Add/remove toppings
                for (var toppingIndex = 0; toppingIndex < cartItem.displayToppings().length; toppingIndex++)
                {
                    var topping = cartItem.displayToppings()[toppingIndex];

                    if (topping.cartQuantity() < 0)
                    {
                        // Default topping has been removed
                        orderLine.removeToppings.push
                        (
                            {
                                productId: topping.topping.Id,
                                quantity: 1,
                                name: topping.topping.ToppingName,
                                price: 0
                            }
                        );
                    }
                    else if (topping.cartQuantity() > 0)
                    {
                        // Topping added
                        orderLine.addToppings.push
                        (
                            {
                                productId: topping.topping.Id,
                                quantity: topping.cartQuantity(),
                                name: topping.topping.ToppingName,
                                price: menuHelper.getToppingPrice(topping.topping) * topping.quantity
                            }
                        );
                    }
                }

                // We've generated an order line
                orderDetails.order.orderLines.push(orderLine);

                // Next order line
                orderLineNumber++;
            }
        }

        return orderDetails;
    },
    checkoutSections: ko.observableArray(),
    getTemplateName: function (data)
    {
        return data.templateName;
    },
    nextButton: { displayText: ko.observable(''), action: 'none' },
    backButton: { visible: ko.observable(false), displayText: ko.observable(''), action: 'none' },
    nextButtonClicked: function ()
    {
        if (checkoutHelper.nextButton.action != undefined)
        {
            if (checkoutHelper.nextButton.action == 'placeorder')
            {
                // Place order
                checkoutHelper.placeOrder();
            }
            else
            {
                var section = checkoutHelper.checkoutSections()[Number(checkoutHelper.nextButton.action) - 1];

                // Has the user entered all the required information?
                if (section.validate())
                {
                    // Show a different section
                    checkoutHelper.showCheckoutSection(Number(checkoutHelper.nextButton.action));
                }
            }
        }
    },
    backButtonClicked: function ()
    {
        if (checkoutHelper.backButton.action != undefined && checkoutHelper.backButton.action != 'none')
        {
            // Show a different section
            checkoutHelper.showCheckoutSection(Number(checkoutHelper.backButton.action));
        }
    },
    errors:
    {
        // Contact details errors
        contactDetailsHasError: ko.observable(false),
        firstNameHasError: ko.observable(false),
        surnameHasError: ko.observable(false),
        telephoneHasError: ko.observable(false),
        emailHasError: ko.observable(false),

        // Delivery time errors
        deliveryTimeHasError: ko.observable(false),
        deliveryTimeError: ko.observable(false),

        // Address errors
        addressHasError: ko.observable(false),
        roadNameHasError: ko.observable(false),
        townHasError: ko.observable(false),
        zipCodeHasError: ko.observable(false),
        addressMissingDetails: ko.observable(false),
        outOfDeliveryArea: ko.observable(false)
    },
    validateContactDetails: function ()
    {
        checkoutHelper.errors.firstNameHasError(checkoutHelper.checkoutDetails.firstName().length == 0);
        checkoutHelper.errors.surnameHasError(checkoutHelper.checkoutDetails.surname().length == 0);
        checkoutHelper.errors.telephoneHasError(checkoutHelper.checkoutDetails.telephoneNumber().length == 0);

        if (checkoutHelper.checkoutDetails.emailAddress().length == 0)
        {
            checkoutHelper.errors.emailHasError(false);
        }
        else
        {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            checkoutHelper.errors.emailHasError(!regex.test(checkoutHelper.checkoutDetails.emailAddress()));
        }

        // Has the user entered all the required contact details?
        checkoutHelper.errors.contactDetailsHasError
        (
            checkoutHelper.errors.firstNameHasError() ||
            checkoutHelper.errors.surnameHasError() ||
            checkoutHelper.errors.telephoneHasError() ||
            checkoutHelper.errors.emailHasError()
        );

        // Was there an error?
        return !checkoutHelper.errors.contactDetailsHasError();
    },
    validateDeliveryTime: function ()
    {
        // Has the user selected a time?
        checkoutHelper.errors.deliveryTimeError(checkoutHelper.checkoutDetails.wantedTime().mode == undefined);

        // Has the user entered all the delivery time details?
        checkoutHelper.errors.deliveryTimeHasError(checkoutHelper.errors.deliveryTimeError());

        // Was there an error?
        return !checkoutHelper.errors.deliveryTimeHasError();
    },
    validateAddress: function ()
    {
        if (viewModel.siteDetails().address.country == "United Kingdom") return checkoutHelper.validateUKAddress();
        else if (viewModel.siteDetails().address.country == "France") return checkoutHelper.validateFrenchAddress();
        else return self.validateUKAddress();
    },
    validateUKAddress: function ()
    {
        // Make sure the required details were entered
        checkoutHelper.errors.roadNameHasError(checkoutHelper.checkoutDetails.address.roadName().length == 0);
        checkoutHelper.errors.townHasError(checkoutHelper.checkoutDetails.address.town().length == 0);

        // Has the user entered all the delivery address details?
        checkoutHelper.errors.addressMissingDetails
        (
            checkoutHelper.errors.roadNameHasError() ||
            checkoutHelper.errors.townHasError()
        );

        var isInDeliveryZone = checkoutHelper.validatePostcode();

        checkoutHelper.errors.outOfDeliveryArea(!isInDeliveryZone); // Display an error if NOT in delivery area
        checkoutHelper.errors.zipCodeHasError(!isInDeliveryZone);

        checkoutHelper.errors.addressHasError
        (
            checkoutHelper.errors.addressMissingDetails() ||
            checkoutHelper.errors.outOfDeliveryArea()
        );

        // Was there an error?
        return !checkoutHelper.errors.addressHasError();
    },
    validateFrenchAddress: function ()
    {
        // Make sure the required details were entered
        checkoutHelper.errors.roadNameHasError(checkoutHelper.checkoutDetails.address.roadName().length == 0);
        checkoutHelper.errors.townHasError(checkoutHelper.checkoutDetails.address.town().length == 0);

        // Has the user entered all the delivery address details?
        checkoutHelper.errors.addressMissingDetails
        (
            checkoutHelper.errors.roadNameHasError() ||
            checkoutHelper.errors.townHasError()
        );

        checkoutHelper.errors.addressHasError
        (
            checkoutHelper.errors.addressMissingDetails()
        );

        // Was there an error?
        return !checkoutHelper.errors.addressHasError();
    },
    validate: function ()
    {
        for (var index = 0; index < checkoutHelper.checkoutSections().length; index++)
        {
            var section = checkoutHelper.checkoutSections()[Number(index)];

            if (!section.validate())
            {
                if (guiHelper.isMobileMode())
                {
                    var sectionTop = $('#' + section.id).offset().top;
                    $(window).scrollTop(sectionTop);
                }
                else
                {
                    checkoutHelper.showCheckoutSection(Number(section.index));
                }
                return false;
            }
        }

        return true;
    },
    validatePostcode: function ()
    {
        var isInDeliveryZone = false;

        if (deliveryZoneHelper.deliveryZones() == undefined || deliveryZoneHelper.deliveryZones().length == 0)
        {
            isInDeliveryZone = true; // No postcodes so we can't validate it
        }
        else
        {
            // Remove all white space from the postcode
            var cleanedUpZipCode = checkoutHelper.checkoutDetails.address.zipCode().replace(/\s+/g, '');
            isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(cleanedUpZipCode);
        }

        //if (viewModel.siteDetails().address.country == "United Kingdom")
        //{
        //    // Remove all white space from the postcode
        //    var cleanedUpZipCode = checkoutHelper.checkoutDetails.address.zipCode().replace(/\s+/g, '');
        //    if (cleanedUpZipCode.length > 4) cleanedUpZipCode = cleanedUpZipCode.substring(0, 4);

        //    if (cleanedUpZipCode.length == 4)
        //    {
        //        isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(cleanedUpZipCode);
        //    }
        //}
        //else
        //{
        //    isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(checkoutHelper.checkoutDetails.address.zipCode());
        //}

        return isInDeliveryZone;
    },
    deliveryTimeChanged: function ()
    {
        // Fix for iPhone - hides the select popup
        $('#deliveryTime').blur();
    },
    modifyOrder: function ()
    {
        viewModel.resetOrderType();
        guiHelper.cartActions(guiHelper.cartActionsMenu);

        // Allow the user modify the cart items
        guiHelper.isCartLocked(false);

        if (guiHelper.isMobileMode())
        {
            // Show the cart
            guiHelper.isMobileMenuVisible(true);
            checkoutMenuHelper.hideMenu();
            guiHelper.showCart();
        }
        else
        {
            // Show the menu
            guiHelper.showMenuView('menuSectionsView');
        }

        setTimeout
        (
            function ()
            {
                guiHelper.resize();
            },
            0
        );
    }
}

// Cache management
var cacheHelper =
{
    loadCachedMenuForSiteId: function (siteId)
    {
        var menu = undefined;

        try
        {
            // Show the please wait view
            guiHelper.showPleaseWait
            (
                textStrings.loadingMenu,
                undefined,
                function ()
                {
                    // Get the menu index
                    var menuIndex = amplify.store('menuIndex');

                    // Did we find the menu index?
                    if (menuIndex != undefined)
                    {
                        // Find the menu index for this site
                        var menuIndexItem = undefined;
                        for (var index = 0; index < menuIndex.length; index++)
                        {
                            menuIndexItem = menuIndex[index];

                            // Do we already have the menu for this site and version in the local cache?
                            if (menuIndexItem.siteId === siteId &&
                                menuIndexItem.menuVersion === viewModel.selectedSite().menuVersion)
                            {
                                // We have the correct version of the menu in the cache for this site

                                // Get the menu out of the cache
                                menu = amplify.store('m_' + siteId);
                            }
                        }
                    }
                }
            );
        }
        catch (exception) { } // Ignore exceptions 

        return menu;
    },
    cacheMenu: function (menu)
    {
        var success = false;

        // Get the menu index
        var menuIndex = amplify.store('menuIndex');

        // If there is no menu index create a new empty one
        if (menuIndex === undefined)
        {
            menuIndex = [];
        }

        // Find the menu index for this site
        var menuIndexItem = undefined;
        for (var index = 0; index < menuIndex.length; index++)
        {
            var checkMenuIndexItem = menuIndex[index];

            if (checkMenuIndexItem.siteId === viewModel.selectedSite().siteId)
            {
                menuIndexItem = checkMenuIndexItem;
                break;
            }
        }

        // Have we got a menu for this site?
        if (menuIndexItem === undefined)
        {
            // No menu for the site
            menuIndexItem =
            {
                siteId: viewModel.selectedSite().siteId,
                lastUsed: new Date(),
                menuVersion: viewModel.selectedSite().menuVersion
            };

            // Add the menu to the index
            menuIndex.push(menuIndexItem);
        }
        else
        {
            // Update the last time the menu was accessed
            menuIndexItem.menuVersion = viewModel.selectedSite().menuVersion;
            menuIndexItem.lastUsed = new Date();
        }

        // Add/update the menu
        if (cacheHelper.cacheObject('m_' + viewModel.selectedSite().siteId, menu, menuIndex))
        {
            // Update the index
            if (cacheHelper.cacheObject('menuIndex', menuIndex, menuIndex))
            {
                success = true;
            }
        }

        return success;
    },
    cacheObject: function (keyName, object, menuIndex)
    {
        var keepTrying = true;
        var success = false;

        // If we run out of storage, keep deleting old menus until there is space
        while (keepTrying)
        {
            // Lets not get stuck in an infinite loop...
            keepTrying = false;

            try
            {
                // Update the menu index
                amplify.store(keyName, object);

                success = true;
            }
            catch (exception)
            {
                // Is the cache full?
                if (exception == 'amplify.store quota exceeded')
                {
                    // Try and clear out the oldest menu/s
                    if (cacheHelper.removeOldestMenuFromCache(menuIndex))
                    {
                        // We've successfully removed a menu.  There might be space now...
                        keepTrying = true;
                    }
                }
            }
        }

        return success;
    },
    removeOldestMenuFromCache: function (menuIndex)
    {
        // Find the oldest menu
        var oldestMenuIndex = undefined;
        for (var index = 0; index < menuIndex.length; index++)
        {
            var checkMenuIndexItem = menuIndex[index];

            if (oldestMenuIndex === undefined || checkMenuIndexItem.lastUsed < oldestMenuIndex.lastUsed)
            {
                oldestMenuIndex = checkMenuIndexItem;
            }
        }

        // Have we got a menu for this site?
        if (oldestMenuIndex != undefined)
        {
            // Remove the cached menu
            amplify.store('m_' + oldestMenuIndex.siteId, null);

            // Remove the menu from the index
            menuIndex.splice(oldestMenuIndex, 1);

            // Save the index
            amplify.store('menuIndex', menuIndex);

            // We've made some space
            return true;
        }
        else
        {
            // Nothing left to delete?
            return false;
        }
    }
};

var helper =
{
    markOfflineStores: function ()
    {
        for (var index = 0; index < viewModel.sites().length; index++)
        {
            var site = viewModel.sites()[index];

            var displayName = site.name;

            if (!site.isOpen)
            {
                displayName += textStrings.appendOffline;
            }

            site.displayText = ko.observable(displayName);
        }
    },
    newOrder: function ()
    {
        if (settings.returnToParentWebsiteAfterOrder)
        {
            viewModel.returnToHostWebsite();
        }
        else
        {
            // Empty out the checkout
            checkoutHelper.clearCheckout();

            // Empty out the cart
            cartHelper.clearCart();

            guiHelper.showHome();
        }
    },
    formatUTCDate: function (date)
    {
        return date.getUTCFullYear() + '-'
            + helper.pad(date.getUTCMonth() + 1) + '-'
            + helper.pad(date.getUTCDate()) + 'T'
            + helper.pad(date.getUTCHours()) + ':'
            + helper.pad(date.getUTCMinutes()) + ':'
            + helper.pad(date.getUTCSeconds()) + 'Z';
    },
    formatUTCSlot: function (slot)
    {
        var dividerIndex = slot.indexOf(':');
        var hours = slot.substring(0, dividerIndex);
        var minutes = slot.substring(dividerIndex + 1);

        var today = new Date();
        today.setHours(hours);
        today.setMinutes(minutes);

        var dateString = helper.formatUTCDate(today);
        return dateString;

        //var now = new Date();
        //var fixedSlot = slot;

        //if (slot.indexOf(':0', slot.length - 2) != -1)
        //{
        //    fixedSlot += '0';
        //}

        //return now.getUTCFullYear() + '-'
        //    + helper.pad(now.getUTCMonth() + 1) + '-'
        //    + helper.pad(now.getUTCDate()) + 'T'
        //    + fixedSlot + 'Z';
    },
    pad: function (source)
    {
        if ((typeof (source) == 'number' && source < 10) ||
            (typeof (source) == 'string' && source.length == 1))
        {
            return '0' + source;
        }
        else
        {
            return source;
        }
    },
    formatPrice: function (price)
    {
        if (price == undefined)
        {
            if (helper.curencyAfter)
            {
                return "- " + helper.currencySymbol;
            }
            else
            {
                return helper.currencySymbol + " -";
            }
        }
        else
        {
            var x = Number(price);
            var y = x / 100;

            var price = undefined;

            if (helper.curencyAfter)
            {
                price = y.toFixed(2) + helper.currencySymbol;
            }
            else
            {
                price = helper.currencySymbol + y.toFixed(2);
            }

            if (helper.useCommaDecimalPoint)
            {
                price = price.replace('.', ',');
            }

            return price;
        }
    },
    currencySymbol: '&pound;',
    useCommaDecimalPoint: false,
    curencyAfter: false,
    use24hourClock: false,
    findById: function (id, list)
    {
        for (var index = 0; index < list.length; index++)
        {
            var item = list[index];

            if (item.Id == id)
            {
                return item;
            }
        }

        return undefined;
    },
    findCategory: function (category, categories)
    {
        for (var index = 0; index < categories().length; index++)
        {
            var existingCategory = categories()[index];

            if (existingCategory.Name == category.Name)
            {
                return true;
            }
        }

        return false;
    },
    findByMenuId: function (id, list)
    {
        for (var index = 0; index < list.length; index++)
        {
            var item = list[index];

            if ((item.Id == undefined ? item.MenuId : item.Id) == id)
            {
                return item;
            }
        }

        return undefined;
    }
}

var openingTimesHelper =
{
    openingTimes:
    {
        monday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        tuesday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        wednesday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        thursday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        friday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        saturday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        sunday:
        {
            openingTimes: [],
            displayText: ko.observable()
        }
    },
    todayPropertyName: '',
    getToday: function ()
    {
        // Day of the week for getting the available times from a menu item or deal
        var today = new Date();
        var dayOfWeek = today.getDay();

        switch (dayOfWeek)
        {
            case 0:
                openingTimesHelper.todayPropertyName = 'Sun';
                break;
            case 1:
                openingTimesHelper.todayPropertyName = 'Mon';
                break;
            case 2:
                openingTimesHelper.todayPropertyName = 'Tue';
                break;
            case 3:
                openingTimesHelper.todayPropertyName = 'Wed';
                break;
            case 4:
                openingTimesHelper.todayPropertyName = 'Thu';
                break;
            case 5:
                openingTimesHelper.todayPropertyName = 'Fri';
                break;
            case 6:
                openingTimesHelper.todayPropertyName = 'Sat';
                break;
        }
    },
    getOpeningHours: function (openingTimes, day)
    {
        // Get the opening times
        for (var index = 0; index < viewModel.siteDetails().openingHours.length; index++)
        {
            var timeSpan = viewModel.siteDetails().openingHours[index];

            if (timeSpan.day == day)
            {
                openingTimes.openingTimes.push(timeSpan);
            }
        }

        // Sort the opening times
        openingTimes.openingTimes.sort(function (a, b) { return a.startTime > b.startTime ? 1 : -1 });

        // Generate a nicely formatted list of opening times to display on the page
        var displayText = '';

        for (var index = 0; index < openingTimes.openingTimes.length; index++)
        {
            var timeSpan = openingTimes.openingTimes[index];

            if (timeSpan.openAllDay)
            {
                displayText = textStrings.openAllDay;
                break;
            }
            else
            {
                if (displayText.length > 0) displayText += '<br />';

                displayText += textStrings.fromTo.replace('{from}', timeSpan.startTime).replace('{to}', timeSpan.endTime);
            }
        }

        if (displayText.length == 0)
        {
            openingTimes.displayText(textStrings.closed);
        }
        else
        {
            openingTimes.displayText(displayText);
        }
    },
    getTodaysOpeningTimes: function ()
    {
        var today = new Date();
        var dayOfWeek = today.getDay();

        // Get ordering times
        var openingTimes;
        switch (dayOfWeek)
        {
            case 0:
                openingTimes = openingTimesHelper.openingTimes.sunday.openingTimes;
                break;
            case 1:
                openingTimes = openingTimesHelper.openingTimes.monday.openingTimes;
                break;
            case 2:
                openingTimes = openingTimesHelper.openingTimes.tuesday.openingTimes;
                break;
            case 3:
                openingTimes = openingTimesHelper.openingTimes.wednesday.openingTimes;
                break;
            case 4:
                openingTimes = openingTimesHelper.openingTimes.thursday.openingTimes;
                break;
            case 5:
                openingTimes = openingTimesHelper.openingTimes.friday.openingTimes;
                break;
            case 6:
                openingTimes = openingTimesHelper.openingTimes.saturday.openingTimes;
                break;
        }

        return openingTimes;
    }
}

var dealHelper =
{
    returnToCart: false,
    mode: ko.observable('addDeal'),
    subscriptions: [],
    hasError: ko.observable(false),
    addToCart: function ()
    {
        // The deal to show on the deal popup
        dealHelper.mode('addDeal');
        dealHelper.hasError(false);
        dealHelper.selectedDeal.name(this.deal.DealName);
        dealHelper.selectedDeal.description(this.deal.Description);
        dealHelper.selectedDeal.deal = this.deal;

        // Build the deal lines
        dealHelper.selectedDeal.bindableDealLines.removeAll();

        for (var index = 0; index < this.dealLineWrappers.length; index++)
        {
            var dealLineWrapper = this.dealLineWrappers[index];

            var bindableDealLine =
            {
                dealLineWrapper: dealLineWrapper,
                itemWrappers: undefined,
                selectedItemWrapper: ko.observable(undefined),
                previouslySelectedItemWrapper: undefined,
                selectedMenuItem: undefined, // Each item could include multiple menu items for different cat1/cat2s - this is the menu item the customer wants
                hasError: ko.observable(false),
                toppings: undefined,
                instructions: '',
                person: '',
                id: 'dl_' + index
            };

            // Build the allowable items in the deal line
            var itemWrappers = [];
            for (var itemIndex = 0; itemIndex < dealLineWrapper.items.length; itemIndex++)
            {
                var item = dealLineWrapper.items[itemIndex];
                var itemWrapper =
                {
                    bindableDealLine: bindableDealLine, // The deal line that the item is in - we're data binding to the item but we need a way to get back to the deal line the item is in
                    item: item,
                    name: item.name
                };

                itemWrappers.push(itemWrapper);
            }

            // Set the allowable items in the deal line
            bindableDealLine.itemWrappers = itemWrappers;

            // Auto select the first item if there is only one
            if (bindableDealLine.itemWrappers.length == 1)
            {
                bindableDealLine.selectedItemWrapper(bindableDealLine.itemWrappers[0]);
                bindableDealLine.selectedMenuItem = bindableDealLine.selectedItemWrapper().item.menuItems()[0];

                // We'll also need the selected items toppings
                bindableDealLine.toppings = menuHelper.getItemToppings(bindableDealLine.selectedMenuItem);
            }

            // Add the deal line to the deal
            dealHelper.selectedDeal.bindableDealLines.push(bindableDealLine);
        }

        // Show the deal popup
        dealPopupHelper.returnToCart = false;  // If the user cancels show the menu not the cart (mobile only)
        dealPopupHelper.showDealPopup('addDeal', true);

        // Let knockout sort out the GUI before subscribing to events as the act of building the GUI triggers these events which we're not interested in
        setTimeout
        (
            function ()
            {
                dealHelper.subscribeToDealLineChanges();
            },
            0
        );
    },
    subscribeToDealLineChanges: function ()
    {
        for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
        {
            // We want to know when the selected menu item changes
            dealHelper.subscriptions.push(dealHelper.selectedDeal.bindableDealLines()[index].selectedItemWrapper.subscribe(dealHelper.selectedDealLineChanged));
        }
    },
    unSubscribeToDealLineChanges: function ()
    {
        for (var index = 0; index < dealHelper.subscriptions.length; index++)
        {
            var subscription = dealHelper.subscriptions[index];
            subscription.dispose();
        }
    },
    selectedDeal:
    {
        name: ko.observable(''),
        description: ko.observable(''),
        bindableDealLines: ko.observableArray(),
        deal: undefined
    },
    selectedBindableDealLine: undefined, // This is used to temporarily hold the deal line that is currently being customized in the toppings popup so when the popup is closed we know which deal line to copy the customization to
    dealPopupCancel: function ()
    {
        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    getDealLineTemplateName: function (bindableDealLine)
    {
        return bindableDealLine == undefined ? 'popupDealLine-template' : bindableDealLine.dealLineWrapper.templateName;
    },
    selectedDealLineChanged: function (itemWrapper)
    {
        if (itemWrapper.bindableDealLine != undefined)
        {
            // Different item - different toppings
            itemWrapper.bindableDealLine.toppings = undefined;

            // Different item - different menu item
            itemWrapper.bindableDealLine.menuItem = undefined; //itemWrapper.item.menuItems[1];
            itemWrapper.bindableDealLine.selectedMenuItem = undefined; //itemWrapper.item.menuItems[1];

            // Show the toppings popup
            dealHelper.showToppingsPopupForDealLine(itemWrapper.bindableDealLine, 'addDealItem', true);

            $('#' + itemWrapper.bindableDealLine.dealLineWrapper.id).blur();
        }
    },
    customizeDealLine: function (bindableDealLine)
    {
        dealHelper.showToppingsPopupForDealLine(bindableDealLine, 'addDealItem', false);
    },
    showToppingsPopupForDealLine: function (bindableDealLine, mode, onlyShowIfToppings)
    {
        // The newly selected item
        var selectedItemWrapper = bindableDealLine.selectedItemWrapper();

        if (selectedItemWrapper != undefined && selectedItemWrapper.name != textStrings.pleaseSelectAnItem)
        {
            // We don't need to know about deal line changes any more
            dealHelper.unSubscribeToDealLineChanges();

            // Get the price of the deal line (luckily, we keep a reference to it)
            bindableDealLine.price = menuHelper.calculateDealLinePrice(selectedItemWrapper.bindableDealLine.dealLineWrapper.dealLine);

            // Store for later
            dealHelper.selectedBindableDealLine = bindableDealLine;

            // Each allowable item in a deal line could consist of multiple menu items e.g. different sizes.  Default to the first one
            var item = bindableDealLine.selectedItemWrapper().item;

            if (bindableDealLine.selectedMenuItem == undefined)
            {
                // Default to the first menu item
                bindableDealLine.selectedMenuItem = item.menuItems()[0];
            }

            // Get the categories
            var category1 = helper.findById(bindableDealLine.selectedMenuItem.Cat1 == undefined ? bindableDealLine.selectedMenuItem.Category1 : bindableDealLine.selectedMenuItem.Cat1, viewModel.menu.Category1);
            var category2 = helper.findById(bindableDealLine.selectedMenuItem.Cat2 == undefined ? bindableDealLine.selectedMenuItem.Category2 : bindableDealLine.selectedMenuItem.Cat2, viewModel.menu.Category2);

            // The item to show on the toppings popup
            viewModel.selectedItem.name(item.name);

            // Copy over the menu items
            viewModel.selectedItem.menuItems.removeAll();
            for (var index = 0; index < item.menuItems().length; index++)
            {
                viewModel.selectedItem.menuItems.push(item.menuItems()[index]);
            }

            viewModel.selectedItem.description(item.description);
            viewModel.selectedItem.quantity(1);
            viewModel.selectedItem.menuItem(bindableDealLine.selectedMenuItem);
            viewModel.selectedItem.freeToppings(bindableDealLine.selectedMenuItem.FreeTops);
            // Free deal tops????
            viewModel.selectedItem.freeToppingsRemaining(bindableDealLine.selectedMenuItem.FreeTops);
            viewModel.selectedItem.instructions(bindableDealLine.instructions);
            viewModel.selectedItem.person(bindableDealLine.person);
            viewModel.selectedItem.price(helper.formatPrice(0));

            // Copy over the cat1 and cat2s
            viewModel.ignoreEvents = true;
            viewModel.selectedItem.category1s.removeAll();
            for (var index = 0; index < item.category1s().length; index++)
            {
                viewModel.selectedItem.category1s.push(item.category1s()[index]);
            }
            viewModel.selectedItem.category2s.removeAll();
            for (var index = 0; index < item.category2s().length; index++)
            {
                viewModel.selectedItem.category2s.push(item.category2s()[index]);
            }
            viewModel.ignoreEvents = false;

            viewModel.selectedItem.selectedCategory1(category1);
            viewModel.selectedItem.selectedCategory2(category2);

            // Get the toppings
            if (dealHelper.selectedBindableDealLine.toppings == undefined)
            {
                viewModel.selectedItem.toppings(menuHelper.getItemToppings(viewModel.selectedItem.menuItem()));
            }
            else
            {
                viewModel.selectedItem.toppings(dealHelper.cloneToppings(dealHelper.selectedBindableDealLine.toppings));
            }

            // Does the customer need to pick any toppings?
            if (onlyShowIfToppings &&
                viewModel.selectedItem.toppings().length == 0 &&
                viewModel.selectedItem.category1s().length < 2 &&
                viewModel.selectedItem.category2s().length < 2)
            {
                popupHelper.commitToDeal();
            }
            else
            {
                // Hide the deal popup
                dealPopupHelper.hideDealPopup();

                // Show the toppings popup
                popupHelper.showPopup(mode);
            }
        }
    },
    cloneToppings: function (sourceToppings)
    {
        var cloneToppings = [];
        for (var index = 0; index < sourceToppings.length; index++)
        {
            var topping = sourceToppings[index];

            var dealLineTopping =
            {
                type: topping.type,
                selectedSingle: ko.observable(topping.selectedSingle()),
                selectedDouble: ko.observable(topping.selectedDouble()),
                topping: topping.topping,
                price: topping.price,
                doublePrice: topping.doublePrice,
                freeQuantity: topping.freeQuantity,
                quantity: topping.quantity,
                cartPrice: ko.observable(''),
                cartName: ko.observable(''),
                cartQuantity: ko.observable('')
            };

            cloneToppings.push(dealLineTopping);
        }

        return cloneToppings;
    },
    acceptChanges: function ()
    {
        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    commitToCart: function ()
    {
        // Has the user entered all the required information?
        if (!dealHelper.checkForErrors())
        {
            // Cheapest free
            if (dealHelper.selectedDeal.deal.ForceCheapestFree && dealHelper.selectedDeal.dealLines().length > 1)
            {
                var dealLine1 = dealHelper.selectedDeal.dealLines()[0];
                var dealLine2 = dealHelper.selectedDeal.dealLines()[1];

                // Check the first two deal lines
                var dealLinePrice1 = menuHelper.calculateDealLinePrice(dealLine1, true);
                var dealLinePrice2 = menuHelper.calculateDealLinePrice(dealLine2, true);

                if (dealLinePrice2 > dealLinePrice1)
                {
                    // We need to switch the menu items around so the more expensive one is in deal line 1

                    // Remove the first two items in the array
                    dealHelper.selectedDeal.dealLines.splice(0, 1);
                    dealHelper.selectedDeal.dealLines.splice(0, 1);

                    // Insert the removed item 
                    dealHelper.selectedDeal.dealLines.push(dealLine2);
                    dealHelper.selectedDeal.dealLines.push(dealLine1);
                }
            }

            // Clone the selected deal
            var deal =
            {
                name: dealHelper.selectedDeal.name(),
                price: 0,
                displayPrice: ko.observable('-'),
                dealLines: ko.observableArray(),
                deal:
                {
                    name: dealHelper.selectedDeal.name(),
                    description: dealHelper.selectedDeal.description(),
                    deal: dealHelper.selectedDeal.deal
                },
                minimumOrderValue: helper.formatPrice(dealHelper.selectedDeal.deal.MinimumOrderValue),
                minimumOrderValueNotMet: ko.observable(true),
                isEnabled: ko.observable()
            };

            // Add deal lines
            for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
            {
                var bindableDealLine = dealHelper.selectedDeal.bindableDealLines()[index];

                // Copy the toppings to the cart object.  We have to actually clone the topping objects - if we just copy the object references it'll get screwed up later
                var cartToppings = [];
                for (var toppingIndex = 0; toppingIndex < bindableDealLine.toppings.length; toppingIndex++)
                {
                    var topping = bindableDealLine.toppings[toppingIndex];

                    var cartTopping =
                    {
                        type: topping.type,
                        selectedSingle: ko.observable(topping.selectedSingle()),
                        selectedDouble: ko.observable(topping.selectedDouble()),
                        topping: topping.topping,
                        price: topping.price,
                        doublePrice: topping.doublePrice,
                        freeQuantity: topping.freeQuantity,
                        quantity: topping.quantity,
                        cartPrice: ko.observable(''),
                        cartName: ko.observable(''),
                        cartQuantity: ko.observable('')
                    };

                    cartToppings.push(cartTopping);
                }

                // Get the categories
                var category1 = helper.findById(bindableDealLine.selectedMenuItem.Cat1 == undefined ? bindableDealLine.selectedMenuItem.Category1 : bindableDealLine.selectedMenuItem.Cat1, viewModel.menu.Category1);
                var category2 = helper.findById(bindableDealLine.selectedMenuItem.Cat2 == undefined ? bindableDealLine.selectedMenuItem.Category2 : bindableDealLine.selectedMenuItem.Cat2, viewModel.menu.Category2);

                deal.dealLines.push
                (
                    {
                        name: cartHelper.getCartItemName(bindableDealLine.selectedItemWrapper().item, category1, category2),
                        price: 0,
                        cartPrice: 0,
                        displayPrice: ko.observable('-'),
                        dealLine: bindableDealLine.dealLineWrapper.dealLine,
                        selectedMenuItem: bindableDealLine.selectedMenuItem,
                        displayToppings: ko.observableArray(menuHelper.getCartDisplayToppings(cartToppings)),
                        toppings: bindableDealLine.toppings,
                        categoryPremiumName: ko.observable(undefined),
                        categoryPremium: ko.observable(undefined),
                        itemPremiumName: ko.observable(undefined),
                        itemPremium: ko.observable(undefined),
                        instructions: bindableDealLine.instructions,
                        person: bindableDealLine.person
                    }
                );
            }

            // Add the selected deal to the cart
            cartHelper.cart().deals.push(deal);

            // Recalculate the cart price
            cartHelper.refreshCart();

            // Hide the deal popup
            dealPopupHelper.hideDealPopup();

            // If we are in mobile mode show the cart
            if (guiHelper.isMobileMode())
            {
                guiHelper.showCart();
            }
        }
    },
    removeFromCart: function ()
    {
        // Remove the item from the cart
        cartHelper.cart().deals.remove(cartHelper.cart().selectedCartItem());

        // Recalculate the total price
        cartHelper.refreshCart();

        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    checkForErrors: function ()
    {
        var errors = false;

        // Check each deal line and make sure the customer has selected an item
        for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
        {
            var bindableDealLine = dealHelper.selectedDeal.bindableDealLines()[index];

            bindableDealLine.hasError(false); // Reset to default

            // Does the user need to select anything for this deal line?
            if (bindableDealLine.itemWrappers.length > 0)
            {
                // Has the user actually selected anything?
                if (bindableDealLine.selectedMenuItem == undefined)
                {
                    bindableDealLine.hasError(true);
                    errors = true;
                }
            }
        }

        // Show or hide the error message
        dealHelper.hasError(errors);

        return errors;
    }
}

var popupHelper =
{
    returnToCart: false,
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    mode: ko.observable('addItem'),
    cancel: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    cancelDeal: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();

        // Show the deal popup
        dealPopupHelper.showDealPopup('addDeal', false);

        if (popupHelper.mode() == 'addDealItem')
        {
            // Were there multiple items to pick from?
            if (dealHelper.selectedBindableDealLine.itemWrappers.length > 1)
            {
                // Was an item already selected?
                if (dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper == undefined)
                {
                    // Display the "please select an item" item
                    dealHelper.selectedBindableDealLine.selectedItemWrapper(dealHelper.selectedBindableDealLine.itemWrappers[0]);
                    dealHelper.selectedBindableDealLine.selectedMenuItem = undefined;
                }
                else
                {
                    // Restore the previously selected item
                    dealHelper.selectedBindableDealLine.selectedItemWrapper(dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper);
                }
            }

            // Re-scbscribe to deal line changes
            dealHelper.subscribeToDealLineChanges();
        }
    },
    commitToDeal: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();

        // Show the deal popup
        dealPopupHelper.showDealPopup('addDeal', false);

        // Keep hold of the currently selected item so next time they change it but cancel we can set it back
        dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper = dealHelper.selectedBindableDealLine.selectedItemWrapper();

        // Record which menu item was selected
        dealHelper.selectedBindableDealLine.selectedMenuItem = viewModel.selectedItem.menuItem();

        // Check to see if any errors have now been resolved
        dealHelper.checkForErrors();

        dealHelper.selectedBindableDealLine.toppings = dealHelper.cloneToppings(viewModel.selectedItem.toppings());
        dealHelper.selectedBindableDealLine.instructions = viewModel.selectedItem.instructions();
        dealHelper.selectedBindableDealLine.person = viewModel.selectedItem.person();

        // Resubsribe to deal line changes
        dealHelper.subscribeToDealLineChanges();
    },
    acceptChanges: function ()
    {
        // Update the quantity
        cartHelper.cart().selectedCartItem().quantity(viewModel.selectedItem.quantity());

        // Update the cat1
        cartHelper.cart().selectedCartItem().selectedCategory1 = viewModel.selectedItem.selectedCategory1();

        // Update the cat2
        cartHelper.cart().selectedCartItem().selectedCategory2 = viewModel.selectedItem.selectedCategory2();

        // Update the menu item (will change if a different cat1 / cat2 are selected)
        cartHelper.cart().selectedCartItem().menuItem = viewModel.selectedItem.menuItem();

        // Update the chefs instructions
        cartHelper.cart().selectedCartItem().instructions = viewModel.selectedItem.instructions();

        // Update the person
        cartHelper.cart().selectedCartItem().person = viewModel.selectedItem.person();

        // Update the toppings
        var cartToppings = [];
        for (var index = 0; index < viewModel.selectedItem.toppings().length; index++)
        {
            var topping = viewModel.selectedItem.toppings()[index];

            var cartTopping =
            {
                type: topping.type,
                selectedSingle: ko.observable(topping.selectedSingle()),
                selectedDouble: ko.observable(topping.selectedDouble()),
                topping: topping.topping,
                price: topping.price,
                doublePrice: topping.doublePrice,
                freeQuantity: topping.freeQuantity,
                quantity: topping.quantity,
                cartPrice: ko.observable(''),
                cartName: ko.observable(''),
                cartQuantity: ko.observable('')
            };

            cartToppings.push(cartTopping);
        }
        cartHelper.cart().selectedCartItem().toppings.removeAll();
        cartHelper.cart().selectedCartItem().toppings(cartToppings);

        // Update the display name
        cartHelper.cart().selectedCartItem().displayName(menuHelper.getCartItemDisplayName(viewModel.selectedItem, viewModel.selectedItem.selectedCategory1(), viewModel.selectedItem.selectedCategory2()));

        // Recalculate the item price
        var price = menuHelper.calculateItemPrice(cartHelper.cart().selectedCartItem().menuItem, cartHelper.cart().selectedCartItem().quantity(), viewModel.selectedItem.toppings());
        cartHelper.cart().selectedCartItem().price = price;
        cartHelper.cart().selectedCartItem().displayPrice(helper.formatPrice(price));

        cartHelper.cart().selectedCartItem().displayToppings(menuHelper.getCartDisplayToppings(cartToppings));

        // Recalculate the total price of all items in the cart
        cartHelper.refreshCart();

        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    removeFromCart: function ()
    {
        // Remove the item from the cart
        cartHelper.cart().cartItems.remove(cartHelper.cart().selectedCartItem());

        // Recalculate the total price
        cartHelper.refreshCart();

        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    quantityChanged: function ()
    {
        var price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
        viewModel.selectedItem.price(helper.formatPrice(price));

        return true;
    },
    singleChanged: function ()
    {
        popupHelper.popupToppingChanged('single', this);

        return true;
    },
    doubleChanged: function ()
    {
        popupHelper.popupToppingChanged('double', this);

        return true;
    },
    showPopup: function (mode)
    {
        $(window).scrollTop(0);

        // Show the toppings popup
        popupHelper.mode(mode);
        viewModel.pickToppings(true);

        popupHelper.isBackgroundVisible(true);
        popupHelper.isPopupVisible(true);

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
        }

        // Give knockout time to do its thing (Javascript doesn't do not proper multi-threading - need to let the browser have the thread back)
        setTimeout
        (
            function ()
            {
                // Recalculate the item price
                var price = 0;
                if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
                {
                    price = menuHelper.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings(), true);
                }
                else
                {
                    price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
                }

                viewModel.selectedItem.price(helper.formatPrice(price));
            },
            0
        );
    },
    hidePopup: function ()
    {
        popupHelper.isBackgroundVisible(false);
        popupHelper.isPopupVisible(false);

        // We might be on top of the deal popup
        if (!dealPopupHelper.isPopupVisible())
        {
            guiHelper.isInnerMenuVisible(true);

            if (guiHelper.isMobileMode())
            {
                // In mobile mode the cart is a view and not a popup
                if (popupHelper.returnToCart)
                {
                    // Show the view
                    guiHelper.isViewVisible(true);
                    guiHelper.isInnerMenuVisible(false);
                }
                else
                {
                    // Show the menu
                    guiHelper.isViewVisible(false);
                    guiHelper.isInnerMenuVisible(true);
                }
            }
        }

        // Let knockout do its thing
        setTimeout
        (
            function ()
            {
                guiHelper.resize();
            },
            0
        );
    },
    commitToCart: function ()
    {
        var price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), false, true);

        // Get the item name to display in the cart
        var name = cartHelper.getCartItemName(viewModel.selectedItem, viewModel.selectedItem.selectedCategory1(), viewModel.selectedItem.selectedCategory2());

        // Copy the toppings to the cart object.  We have to actually clone the topping objects - if we just copy the object references it'll get screwed up later
        var cartToppings = [];
        for (var index = 0; index < viewModel.selectedItem.toppings().length; index++)
        {
            var topping = viewModel.selectedItem.toppings()[index];

            var cartTopping =
            {
                type: topping.type,
                selectedSingle: ko.observable(topping.selectedSingle()),
                selectedDouble: ko.observable(topping.selectedDouble()),
                topping: topping.topping,
                price: topping.price,
                doublePrice: topping.doublePrice,
                freeQuantity: topping.freeQuantity,
                quantity: topping.quantity,
                cartPrice: ko.observable(''),
                cartName: ko.observable(''),
                cartQuantity: ko.observable('')
            };

            cartToppings.push(cartTopping);
        }

        // Create a new item to add to the cart
        var cartItem =
        {
            menuItem: viewModel.selectedItem.menuItem(),
            name: viewModel.selectedItem.name(),
            displayName: ko.observable(menuHelper.getCartItemDisplayName(viewModel.selectedItem)),
            quantity: ko.observable(viewModel.selectedItem.quantity()),
            displayPrice: ko.observable(helper.formatPrice(price)),
            price: price,
            instructions: viewModel.selectedItem.instructions(),
            person: viewModel.selectedItem.person(),
            toppings: viewModel.selectedItem.toppings(),
            displayToppings: ko.observableArray(menuHelper.getCartDisplayToppings(cartToppings)),
            selectedCategory1: viewModel.selectedItem.selectedCategory1(),
            selectedCategory2: viewModel.selectedItem.selectedCategory2(),
            category1s: ko.observableArray(),
            category2s: ko.observableArray(),
            toppings: ko.observableArray(cartToppings),
            isEnabled: ko.observable(),
            menuItems: undefined
        };

        // Copy over the cat1 and cat2s
        viewModel.ignoreEvents = true;
        for (var index = 0; index < viewModel.selectedItem.category1s().length; index++)
        {
            cartItem.category1s.push(viewModel.selectedItem.category1s()[index]);
        }

        for (var index = 0; index < viewModel.selectedItem.category2s().length; index++)
        {
            cartItem.category2s.push(viewModel.selectedItem.category2s()[index]);
        }
        viewModel.ignoreEvents = false;

        // Copy over the menu items
        cartItem.menuItems = [];
        for (var index = 0; index < viewModel.selectedItem.menuItems().length; index++)
        {
            cartItem.menuItems.push(viewModel.selectedItem.menuItems()[index]);
        }

        // Add the item to the cart
        cartHelper.cart().cartItems.push(cartItem);

        // Recalculate the cart price
        cartHelper.refreshCart();

        // Hide the toppings popup
        popupHelper.hidePopup();

        // If we are in mobile mode show the cart
        if (guiHelper.isMobileMode())
        {
            guiHelper.showCart();
        }
    },
    popupToppingChanged: function (singleOrDouble, topping)
    {
        // Both double and single cannot be ticked:

        // If single was ticked and double is already ticked then untick double
        if (singleOrDouble == 'single' && topping.selectedSingle())
        {
            topping.selectedDouble(false);
        }
            // If double was ticked and single is already ticked then untick single
        else if (singleOrDouble == 'double' && topping.selectedSingle())
        {
            topping.selectedSingle(false);
        }

        if (topping.type == 'removable')
        {
            // Is the topping removed?
            if (!topping.selectedDouble() && !topping.selectedSingle())
            {
                topping.quantity = -1;
            }
                // Is the topping being doubled?
            else if (topping.selectedDouble())
            {
                // This topping is already on the item so doubling up just adds a single topping
                topping.quantity = 1;
            }
        }
        else
        {
            if (topping.selectedSingle())
            {
                topping.quantity = 1;
            }
            else if (topping.selectedDouble())
            {
                topping.quantity = 2;
            }
        }

        // Recalculate the item price
        var price = 0;
        if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
        {
            price = menuHelper.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings(), true);
        }
        else
        {
            price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
        }

        viewModel.selectedItem.price(helper.formatPrice(price));

        // Refresh free toppings
        popupHelper.refeshFreeToppings();
    },
    refeshFreeToppings: function ()
    {
        var remainingToppings = 0;

        if (viewModel.selectedItem.freeToppings() > 0)
        {
            // Does the item have any toppings?
            if (viewModel.selectedItem.toppings() != undefined)
            {
                var usedToppings = 0;

                for (var index = 0; index < viewModel.selectedItem.toppings().length; index++)
                {
                    var topping = viewModel.selectedItem.toppings()[index];

                    if (topping.type == 'removable')
                    {
                        // The customer has upgraded an included topping toa double
                        if (topping.selectedDouble())
                        {
                            usedToppings++;
                        }
                    }
                    else
                    {
                        if (topping.selectedDouble())
                        {
                            // Customer has added 2 of the topping
                            usedToppings += 2;
                        }
                        else if (topping.selectedSingle())
                        {
                            // Customer has a topping
                            usedToppings++;
                        }
                    }
                }

                remainingToppings = viewModel.selectedItem.freeToppings() - usedToppings;

                if (remainingToppings < 0) remainingToppings = 0;
            }
        }

        // Update UI
        viewModel.selectedItem.freeToppingsRemaining(remainingToppings);
    },
    popupCategory1Changed: function ()
    {
        if (!viewModel.ignoreEvents && viewModel.pickToppings())
        {
            viewModel.category1Changed(viewModel.selectedItem);

            // Get the menu item that the user has selected
            var menuItem = menuHelper.getSelectedMenuItem(viewModel.selectedItem);

            if (menuItem == undefined) return;

            // Change the selected menu item
            viewModel.selectedItem.menuItem(menuItem);
            viewModel.selectedItem.toppings(menuHelper.getItemToppings(viewModel.selectedItem.menuItem()));

            // Recalculate the item price
            var price = 0;
            if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
            {
                price = menuHelper.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings());
            }
            else
            {
                price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings());
            }

            viewModel.selectedItem.price(helper.formatPrice(price));
        }
    },
    popupSelectedItemChanged: function ()
    {
        if (!viewModel.ignoreEvents && viewModel.pickToppings())
        {
            // Get the menu item that the user has selected
            var menuItem = menuHelper.getSelectedMenuItem(viewModel.selectedItem);

            // Change the selected menu item
            viewModel.selectedItem.menuItem(menuItem);
            viewModel.selectedItem.toppings(menuHelper.getItemToppings(viewModel.selectedItem.menuItem()));

            if (popupHelper.mode() == 'addItem' || popupHelper.mode() == 'editItem')
            {
                guiHelper.selectedItemChanged(viewModel.selectedItem);
            }
            else
            {
                // Recalculate the item price
                var price = menuHelper.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings());

                viewModel.selectedItem.price(helper.formatPrice(price));
            }
        }
    }
}

var dealPopupHelper =
{
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    showDealPopup: function (mode, scrollTop)
    {
        if (scrollTop)
        {
            $(window).scrollTop(0);
        }

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
        }

        dealHelper.mode(mode);

        // Make the popup visible
        dealPopupHelper.isBackgroundVisible(true);
        dealPopupHelper.isPopupVisible(true);
    },
    hideDealPopup: function ()
    {
        // Make the popup visible
        dealPopupHelper.isBackgroundVisible(false);
        dealPopupHelper.isPopupVisible(false);

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            if (dealPopupHelper.returnToCart)
            {
                // Show the view
                guiHelper.isViewVisible(true);
                guiHelper.isInnerMenuVisible(false);
            }
            else
            {
                // Show the menu
                guiHelper.isViewVisible(false);
                guiHelper.isInnerMenuVisible(true);
            }
        }

        // Let knockout do its thing
        setTimeout
        (
            function ()
            {
                guiHelper.resize();
            },
            0
        );
    },
}

var deliveryZoneHelper =
{
    deliveryZones: ko.observable(undefined),
    isInDeliveryZone: function (targetDeliveryZone)
    {
        if (deliveryZoneHelper.deliveryZones() != undefined)
        {
            for (var index = 0; index < deliveryZoneHelper.deliveryZones().length; index++)
            {
                var possibleDeliveryZone = deliveryZoneHelper.deliveryZones()[index];

                // Does the customers delivery zone start with the possible delivery zone?
                return targetDeliveryZone.toUpperCase().slice(0, possibleDeliveryZone.length) == possibleDeliveryZone.toUpperCase();
            }

            return false;
        }
    }
}

var postcodeCheckHelper =
{
    isPopupVisible: ko.observable(false),
    isInDeliveryZone: ko.observable(undefined),
    showPopup: function ()
    {
        // Do we have the stores delivery zones?
        if (deliveryZoneHelper.deliveryZones != undefined)
        {
            $(window).scrollTop(0);

            // Make the popup visible
            popupHelper.isBackgroundVisible(true);
            postcodeCheckHelper.isPopupVisible(true);
        }
    },
    hidePopup: function (callback)
    {
        if (!guiHelper.isMobileMode())
        {
            popupHelper.isBackgroundVisible(false);
            postcodeCheckHelper.isPopupVisible(false);
        }

        if (callback != undefined)
        {
            callback();
        }
    },
    checkPostcode: function ()
    {
        postcodeCheckHelper.isInDeliveryZone(checkoutHelper.validatePostcode());
    },
    showMenu: function ()
    {
        if (guiHelper.isMobileMode())
        {
            // Continue showing the menu
            postcodeCheckHelper.hidePopup(guiHelper.showHome);
        }
        else
        {
            // Continue showing the menu
            postcodeCheckHelper.hidePopup();
        }
    },
    changeStore: function ()
    {
        // Continue showing the menu
        postcodeCheckHelper.hidePopup(viewModel.chooseStore);
    },
    checkPostcodeKeypress: function (data, event)
    {
        if (event.which == 13)
        {
            var postcode = $('#postcodeCheckTextbox').val();
            checkoutHelper.checkoutDetails.address.zipCode(postcode);

            setTimeout
            (
                postcodeCheckHelper.checkPostcode(),
                0
            );
        }
        else
        {
            postcodeCheckHelper.isInDeliveryZone(undefined);
        }

        return true;
    }
}

var tandcHelper =
{
    isPopupVisible: ko.observable(false),
    hasAgreed: function ()
    {
        if (tandcHelper.agree() == undefined)
        {
            tandcHelper.agree(false);
        }

        return tandcHelper.agree();
    },
    agree: ko.observable(undefined),
    showPopup: function ()
    {
        if (guiHelper.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('pay', false);
            guiHelper.showView('tandcView');
        }
        else
        {
            $(window).scrollTop(0);

            // Make the popup visible
            popupHelper.isBackgroundVisible(true);
            tandcHelper.isPopupVisible(true);
        }

        // Switch the cart to checkout mode
        guiHelper.canChangeOrderType(false);
        guiHelper.cartActions(guiHelper.cartActionsCheckout);
    },
    hidePopup: function (callback)
    {
        if (guiHelper.isMobileMode())
        {
            checkoutHelper.showPaymentPicker();
        }
        else
        {
            popupHelper.isBackgroundVisible(false);
            tandcHelper.isPopupVisible(false);
        }
    }
}

var mapHelper =
{
    map: undefined,
    storeMarker: undefined,
    markersLayer: undefined,
    urls:
    [
        "http://a.tile.openstreetmap.org/${z}/${x}/${y}.png",
        "http://b.tile.openstreetmap.org/${z}/${x}/${y}.png",
        "http://c.tile.openstreetmap.org/${z}/${x}/${y}.png"
    ],
    initialiseMap: function ()
    {
        try
        {
            mapHelper.map = new OpenLayers.Map
            (
                {
                    div: "map",
                    layers:
                    [
                        new OpenLayers.Layer.XYZ
                        (
                            "OSM (with buffer)",
                            mapHelper.urls,
                            {
                                transitionEffect: "resize",
                                buffer: 2, sphericalMercator: true,
                                attribution: ""
                            }
                        ),
                        new OpenLayers.Layer.XYZ
                        (
                            "OSM (without buffer)",
                            mapHelper.urls,
                            {
                                transitionEffect: "resize",
                                buffer: 0,
                                sphericalMercator: true,
                                attribution: ""
                            }
                        )
                    ],
                    controls:
                    [
                        new OpenLayers.Control.Navigation
                        (
                            {
                                dragPanOptions: { enableKinetic: true }
                            }
                        ),
                        new OpenLayers.Control.PanZoom(),
                        new OpenLayers.Control.Attribution()
                    ],
                    center: [0, 0],
                    zoom: 3
                }
            );

            var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
            var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection

            // allow testing of specific renderers via "?renderer=Canvas", etc
            var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
            renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;

            var wgs84 = new OpenLayers.Projection("EPSG:4326");
            mapHelper.markersLayer = new OpenLayers.Layer.Vector
            (
                "Markers",
                {
                    /*renderers: renderer*/
                    styleMap: new OpenLayers.StyleMap
                    (
                        {
                            externalGraphic: settings.mapMarker,
                            graphicOpacity: 1.0,
                            graphicWidth: 32,
                            graphicHeight: 37,
                            graphicYOffset: -37
                        }
                    ),
                    projection: wgs84
                }
            );
            window.mapped = "yes";

            mapHelper.map.addLayer(mapHelper.markersLayer);

            var longitude = 0;

            if (viewModel.siteDetails().address["long"] == undefined)
            {
                longitude = viewModel.siteDetails().address.longitude;
            }
            else
            {
                longitude = viewModel.siteDetails().address["long"];
            }

            var latitude = viewModel.siteDetails().address.lat == undefined ? viewModel.siteDetails().address.latitude : viewModel.siteDetails().address.lat;

            mapHelper.setStoreMarker(longitude, latitude);

            var position = new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection);

            mapHelper.map.setCenter(position, 17);
        }
        catch (exception) { }
    },
    setStoreMarker: function (longitude, latitude)
    {
        // Is there already a store feature on the map?
        if (typeof (mapHelper.storeMarker) == 'object')
        {
            // Remove the store feature
            mapHelper.markersLayer.removeAllFeatures();
            mapHelper.markersLayer.destroyFeatures();
            mapHelper.storeMarker = undefined;
        }

        var fromProjection = new OpenLayers.Projection("EPSG:4326"); // Transform from WGS 1984
        var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection

        var lonLat = new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection);

        // Create a new feature
        mapHelper.storeMarker =
        {
            "type": "Feature",
            "geometry":
            {
                "type": "Point",
                "coordinates": [lonLat.lon, lonLat.lat]
            }
        };

        // Add the feature to the map
        var features =
        {
            "type": "FeatureCollection",
            "features": [mapHelper.storeMarker]
        };

        var reader = new OpenLayers.Format.GeoJSON();

        var locator = reader.read(features);

        mapHelper.markersLayer.addFeatures(locator);

        // Center the map on the feature
        var latLonBounds = new OpenLayers.Bounds();

        latLonBounds.extend(new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection));
    }
}

var checkoutMenuHelper =
{
    showMenu: function (backTo, isPlaceOrderEnabled)
    {
        if (guiHelper.isMobileMode())
        {
            checkoutMenuHelper.isVisible(true);
            checkoutMenuHelper.backTo(backTo);
            checkoutMenuHelper.isPlaceOrderEnabled(isPlaceOrderEnabled);
        }
    },
    hideMenu: function ()
    {
        if (guiHelper.isMobileMode())
        {
            checkoutMenuHelper.isVisible(false);
        }
    },
    isVisible: ko.observable(false),
    backTo: ko.observable('menu'),
    isPlaceOrderEnabled: ko.observable(true)
}

var accountHelper =
{
    customerDetails: undefined,
    emailAddress: undefined,
    password: undefined,
    loggedInCallback: undefined,
    loginDetails:
    {
        errorMessage: ko.observable(''),
        emailAddress: ko.observable(''),
        password: ko.observable('')
    },
    newAccount:
    {
        errorMessage: ko.observable(''),
        firstName: ko.observable(''),
        surname: ko.observable(''),
        emailAddress: ko.observable(''),
        phoneNumber: ko.observable(''),
        password: ko.observable(''),
        reenterPassword: ko.observable(''),
        marketing: ko.observable(true),
        firstNameHasError: ko.observable(false),
        surnameHasError: ko.observable(false),
        emailAddressHasError: ko.observable(false),
        phoneNumberHasError: ko.observable(false),
        passwordHasError: ko.observable(false)
    },
    isRegisterOpen: false,
    isLoggedIn: ko.observable(false),
    loginRegister: function ()
    {
        accountHelper.showPopup();
    },
    isPopupVisible: ko.observable(false),
    showPopup: function (loggedInCallback)
    {
        if (guiHelper.isMobileMode())
        {
            accountHelper.loggedInCallback = loggedInCallback;

            guiHelper.showView('loginRegisterView');

            guiHelper.isMobileMenuVisible(false);
        }
        else
        {
            $(window).scrollTop(0);

            accountHelper.isRegisterOpen = false;

            $('#loginRegister').css('height', '260px');
            $('#registerContainer').css('paddingTop', '45px');

            accountHelper.loggedInCallback = loggedInCallback;

            // Make the popup visible
            popupHelper.isBackgroundVisible(true);
            accountHelper.isPopupVisible(true);

            if (guiHelper.isMobileMode())
            {
                // In mobile mode the cart is a view and not a popup
                guiHelper.isViewVisible(false);
                guiHelper.isInnerMenuVisible(false);
            }
        }
    },
    hidePopup: function (callback)
    {
        if (!guiHelper.isMobileMode())
        {
            popupHelper.isBackgroundVisible(false);
            accountHelper.isPopupVisible(false);
        }

        if (typeof (callback) == 'function')
        {
            callback();
        }

        accountHelper.loggedInCallback = undefined;
    },
    normalLogin: function ()
    {
        accountHelper.login(accountHelper.loginDetails.emailAddress(), accountHelper.loginDetails.password());
    },
    login: function (email, password)
    {
        accountHelper.loginDetails.errorMessage('');

        if (email == undefined || email.length == 0)
        {
            accountHelper.loginDetails.errorMessage('Please enter an email address');
            return;
        }

        if (password == undefined || password.length == 0)
        {
            accountHelper.loginDetails.errorMessage('Please enter a password');
            return;
        }

        acsapi.getCustomer
        (
            email,
            password,
            function (getCustomerResponse)
            {
                if (getCustomerResponse == undefined)
                {
                    // Failed!
                    accountHelper.loginDetails.errorMessage('Unable to login');
                    return;
                }
                else
                {
                    // Was an error returned
                    if (getCustomerResponse.errorCode == undefined)
                    {
                        // Success
                        accountHelper.isLoggedIn(true);
                        accountHelper.customerDetails = getCustomerResponse;
                        accountHelper.emailAddress = email;
                        accountHelper.password = password;
                        accountHelper.hidePopup(accountHelper.loggedInCallback);
                    }
                    else
                    {
                        // Error returned by server
                        if (getCustomerResponse.errorCode == 1036)
                        {
                            // Invalid username
                            accountHelper.loginDetails.errorMessage('Invalid username or password');
                            return;
                        }
                        else if (getCustomerResponse.errorCode == 1042)
                        {
                            // Invalid password
                            accountHelper.loginDetails.errorMessage('Invalid username or password');
                            return;
                        }
                        else
                        {
                            // Other error
                            accountHelper.loginDetails.errorMessage('Unable to login');
                            return;
                        }
                    }
                }
            }
        );
    },
    facebookLogin: function ()
    {
        accountHelper.loginDetails.errorMessage('');

        FB.login
        (
            function (response)
            {
                if (response.authResponse)
                {
                    FB.api('/me', function (response)
                    {
                        //   accountHelper.login(true, response.email, response.id);

                        acsapi.getCustomer
                        (
                            response.email,
                            response.id,
                            function (getCustomerResponse)
                            {
                                if (getCustomerResponse == undefined)
                                {
                                    // Failed!
                                    accountHelper.loginDetails.errorMessage('Unable to login');
                                    return;
                                }
                                else
                                {
                                    // Was an error returned
                                    if (getCustomerResponse.errorCode == undefined)
                                    {
                                        // Success
                                        accountHelper.isLoggedIn(true);
                                        accountHelper.customerDetails = getCustomerResponse;
                                        accountHelper.emailAddress = response.email;
                                        accountHelper.password = response.id;
                                        accountHelper.hidePopup(accountHelper.loggedInCallback);
                                    }
                                    else
                                    {
                                        // Error returned by server
                                        if (getCustomerResponse.errorCode == 1036 || getCustomerResponse.errorCode == 1042)
                                        {
                                            acsapi.putCustomer
                                            (
                                                response.email,
                                                response.id,
                                                {
                                                    title: '',
                                                    firstname: response.first_name,
                                                    surname: response.last_name,
                                                    contacts: undefined,
                                                    address: undefined
                                                },
                                                function (response)
                                                {
                                                    if (response == undefined)
                                                    {
                                                        accountHelper.isLoggedIn(true);
                                                        accountHelper.customerDetails = getCustomerResponse;
                                                        accountHelper.emailAddress = email;
                                                        accountHelper.password = password;
                                                        accountHelper.hidePopup(accountHelper.loggedInCallback);
                                                    }
                                                }
                                            );

                                            return;
                                        }
                                        else
                                        {
                                            // Other error
                                            accountHelper.loginDetails.errorMessage('Unable to login');
                                            return;
                                        }
                                    }
                                }
                            }
                        );
                    });
                }
            },
            {
                scope: 'email'
            }
        );
    },
    myAccount: function ()
    {
        alert("my account");
    },
    register: function ()
    {
        // No error yet
        accountHelper.newAccount.errorMessage('');
        accountHelper.newAccount.firstNameHasError(false);
        accountHelper.newAccount.surnameHasError(false);
        accountHelper.newAccount.emailAddressHasError(false);
        accountHelper.newAccount.phoneNumberHasError(false);
        accountHelper.newAccount.passwordHasError(false);

        // Check that the account details have been entered
        if (accountHelper.newAccount.firstName().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter your first name");
            accountHelper.newAccount.firstNameHasError(true);
            return;
        }

        if (accountHelper.newAccount.surname().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter your surname");
            accountHelper.newAccount.surnameHasError(true);
            return;
        }

        if (accountHelper.newAccount.emailAddress().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter your email address");
            accountHelper.newAccount.emailAddressHasError(true);
            return;
        }

        if (accountHelper.newAccount.phoneNumber().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter your phone number");
            accountHelper.newAccount.phoneNumberHasError(true);
            return;
        }

        if (accountHelper.newAccount.password().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter a password");
            accountHelper.newAccount.passwordHasError(true);
            return;
        }

        // Valid email address?
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        accountHelper.newAccount.emailAddressHasError(!regex.test(accountHelper.newAccount.emailAddress()));
        if (accountHelper.newAccount.emailAddressHasError())
        {
            accountHelper.newAccount.errorMessage("Please enter a valid email address");
            return;
        }

        // Check passwords match
        if (accountHelper.newAccount.password() != accountHelper.newAccount.reenterPassword())
        {
            accountHelper.newAccount.errorMessage("Passwords do not match");
            accountHelper.newAccount.passwordHasError(true);
            return;
        }

        // Minimum password length
        if (accountHelper.newAccount.password().length < 8)
        {
            accountHelper.newAccount.errorMessage("Password must be at least 8 characters");
            accountHelper.newAccount.passwordHasError(true);
            return;
        }

        var customerDetails =
        {
            title: '',
            firstname: accountHelper.newAccount.firstName(),
            surname: accountHelper.newAccount.surname(),
            contacts:
            [
                {
                    type: 'Email',
                    value: accountHelper.newAccount.emailAddress(),
                    marketingLevel: accountHelper.newAccount.marketing() ? '3rdParty' : 'OrderOnly'
                },
                {
                    type: 'Mobile',
                    value: accountHelper.newAccount.phoneNumber(),
                    marketingLevel: accountHelper.newAccount.marketing() ? '3rdParty' : 'OrderOnly'
                }
            ],
            address: undefined
        };

        // Validation passed - try and create a new customer account
        acsapi.putCustomer
        (
            accountHelper.newAccount.emailAddress(),
            accountHelper.newAccount.password(),
            customerDetails,
            function (putCustomerResponse)
            {
                if (putCustomerResponse != undefined && putCustomerResponse.errorCode != undefined)
                {
                    // Server returned an error
                    if (putCustomerResponse.errorCode == 1041)
                    {
                        accountHelper.newAccount.errorMessage("This email address has already been used");
                        accountHelper.newAccount.emailAddressHasError(true);
                        return;
                    }
                    else
                    {
                        accountHelper.newAccount.errorMessage("Unable to create account");
                    }

                    // TODO other errors
                }
                else
                {
                    // Success
                    accountHelper.customerDetails = customerDetails;
                    accountHelper.isLoggedIn(true);
                    accountHelper.emailAddress = accountHelper.newAccount.emailAddress();
                    accountHelper.password = accountHelper.newAccount.password();
                    accountHelper.hidePopup(accountHelper.loggedInCallback);
                }
            }
        );
    },
    showRegister: function ()
    {
        if (accountHelper.isRegisterOpen)
        {
            accountHelper.register();
        }
        else
        {
            $('#loginRegister').animate
            (
                {
                    height: 430
                },
                {
                    duration: 500,
                    queue: false
                }
            );

            $('#registerContainer').animate
            (
                {
                    paddingTop: 10
                },
                {
                    duration: 500,
                    queue: false
                }
            );

            accountHelper.isRegisterOpen = true;
        }
    },
    cancel: function ()
    {
        if (guiHelper.isMobileMode())
        {
            accountHelper.hidePopup(guiHelper.showCart);
        }
        else
        {
            accountHelper.hidePopup();
        }
    }
}

var mobileMenuHelper =
{
    isMobileMenuVisible: ko.observable(true),
    isPopupVisible: ko.observable(false),
    showPopup: function ()
    {
        guiHelper.enable(true);
        mobileMenuHelper.isPopupVisible(true);
    },
    showHome: function ()
    {
        guiHelper.enableDisableUI(true);

        mobileMenuHelper.title(textStrings.mmHome);
        mobileMenuHelper.titleClass('popupMobileMenuHomeButton');

        mobileMenuHelper.isPopupVisible(false);
        guiHelper.showHome();
    },
    showMenu: function (index)
    {
        mobileMenuHelper.title(textStrings.mmOrderNow);
        mobileMenuHelper.titleClass('popupMobileMenuOrderNowButton');

        mobileMenuHelper.isPopupVisible(false);
        guiHelper.showMenu(index);
    },
    showCart: function ()
    {
        mobileMenuHelper.title(textStrings.mmMyOrder);
        mobileMenuHelper.titleClass('popupMobileMenuCartButton');

        mobileMenuHelper.isPopupVisible(false);
        guiHelper.showCart();
    },
    showPostcodeCheck: function ()
    {
        mobileMenuHelper.title(textStrings.mmPostcodeCheck);
        mobileMenuHelper.titleClass('popupMobileMenuPostcodeCheckButton');

        mobileMenuHelper.isPopupVisible(false);
        guiHelper.showPostcodeCheck();
    },
    close: function ()
    {
        mobileMenuHelper.isPopupVisible(false);
    },
    title: ko.observable(''),
    titleClass: ko.observable('popupMobileMenuHomeButton')
}

var imageHelper =
{
    isPopupVisible: ko.observable(false),
    topOffset: ko.observable(false),
    leftOffset: ko.observable(false),
    image: ko.observable
    (
        {
            Src: '',
            Height: 130,
            Width: 130
        }
    ),
    showPopup: function (data)
    {
        imageHelper.image(data.image);

        if (guiHelper.isMobileMode())
        {
            $(window).scrollTop(0);

            guiHelper.showView('imageView');

            mobileMenuHelper.isMobileMenuVisible(false);
        }
        else
        {
            imageHelper.topOffset('-' + (data.image.Height / 2) + 'px');
            imageHelper.leftOffset('-' + (data.image.Width / 2) + 'px');

            // Make the popup visible
            popupHelper.isBackgroundVisible(true);
            imageHelper.isPopupVisible(true);
        }
    },
    hidePopup: function ()
    {
        if (guiHelper.isMobileMode())
        {
            mobileMenuHelper.isMobileMenuVisible(true);

            guiHelper.showMenu(undefined);
        }
        else
        {
            popupHelper.isBackgroundVisible(false);
            imageHelper.isPopupVisible(false);
        }
    }
}

var siteSelectorHelper =
{
    isPostcodeTextboxVisible: ko.observable(false),
    postcode: ko.observable(''),
    checkPostcode: function ()
    {
        siteSelectorHelper.isBadPostcode(false);

        if (siteSelectorHelper.postcode().length == 0)
        {
            siteSelectorHelper.isBadPostcode(true);
            return;
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

                        if (viewModel.sites() === undefined || viewModel.sites().length == 0)
                        {
                            siteSelectorHelper.isBadPostcode(true);

                            // Show the site list view
                            guiHelper.showView('siteSelectorView');
                        }
                        else if (viewModel.sites().length == 1)
                        {
                            // There is a single site that delivers to the customers postcode
                            viewModel.storeSelected(viewModel.sites()[0]);
                        }
                        else
                        {
                            // There are multiple sites that deliver to the customers postcode
                            viewModel.deliverySites(viewModel.sites());

                            // Show the site list view
                            guiHelper.showView('storeSelectorSitesView');
                        }
                    },
                    siteSelectorHelper.postcode()
                );
            }
        );
    },
    siteSelectorPostcodeKeypress: function (data, event)
    {
        if (event.which == 13)
        {
            var postcode = $('#siteSelectorPostcodeInput').val();
            siteSelectorHelper.postcode(postcode);

            //event.preventDefault();
            setTimeout
            (
                siteSelectorHelper.checkPostcode(),
                0
            );
        }

        return true;
    },
    isBadPostcode: ko.observable(false),
    hideBadPostcode: function ()
    {
        siteSelectorHelper.isBadPostcode(false);
    },
    showAllSites: function ()
    {
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
                        if (viewModel.sites() === undefined || viewModel.sites().length == 0)
                        {
                            guiHelper.showError
                            (
                                textStrings.noRestaurants,
                                function ()
                                {
                                    // Show the site list view
                                    guiHelper.showView('siteSelectorView');
                                },
                                undefined
                            );
                        }
                        else
                        {
                            // Mark all the offline stores for display purposes
                            helper.markOfflineStores();

                            // Show the site list view
                            guiHelper.showView('sitesView');
                        }
                    },
                    undefined
                );
            }
        );
    }
}

var queryStringHelper =
{
    queryString: {},
    email: undefined,
    firstName: undefined,
    lastName: undefined,
    telephoneNumber: undefined,
    marketing: undefined,
    city: undefined,
    postcode: undefined,
    houseNumber: undefined,
    roadName: undefined,
    orderType: undefined,
    initialise: function ()
    {
        // Extract the query string and parse it
        queryStringHelper.queryString = {};
        location.search.substr(1).split("&").forEach
        (
            function (pair)
            {
                if (pair === "") return;
                var parts = pair.split("=");
                queryStringHelper.queryString[parts[0]] = parts[1] && decodeURIComponent(parts[1].replace(/\+/g, " "));
            }
        );

        // Check to see if any other information was passed in through the query string
        queryStringHelper.extractData();
    },
    extractData: function ()
    {
        // Extract data from the query
        queryStringHelper.externalSiteId = queryStringHelper.queryString['s'];
        queryStringHelper.email = queryStringHelper.queryString['e'];
        queryStringHelper.firstName = queryStringHelper.queryString['f'];
        queryStringHelper.lastName = queryStringHelper.queryString['l'];
        queryStringHelper.telephoneNumber = queryStringHelper.queryString['t'];
        queryStringHelper.marketing = queryStringHelper.queryString['m'] == 'y' ? true : false;
        queryStringHelper.town = queryStringHelper.queryString['c'];
        queryStringHelper.postcode = queryStringHelper.queryString['p'];
        queryStringHelper.houseNumber = queryStringHelper.queryString['n'];
        queryStringHelper.roadName = queryStringHelper.queryString['r'];
        queryStringHelper.orderType = queryStringHelper.queryString['ot'];

        // Sanitise the url shown in the browser
        var url = window.location.href;
        var queryIndex = url.indexOf('/?');
        if (queryStringHelper.externalSiteId != undefined && queryIndex != -1)
        {
            url = url.substring(0, queryIndex);

            // Do we need to hide the passthrough query string?
            if (settings.hidePassthroughQueryString)
            {
                History.pushState(null, document.title, url);
            }
        }
    }
}

var cookieHelper = 
{
    setCookie: function(key, value) 
    {
        var expires = new Date();
        expires.setTime(expires.getTime() + (1 * 24 * 60 * 60 * 1000));
        document.cookie = key + '=' + value + ';expires=' + expires.toUTCString();
    },
    getCookie: function(key) 
    {
        var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
        return keyValue ? keyValue[2] : null;
    }
}