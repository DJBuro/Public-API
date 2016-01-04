/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

// This object is data bound to the HTML
var viewModel =
{
    invertItems: false, // Switch the item name with cat1
    initialise: function ()
    {
        ko.setTemplateEngine(new ko.nativeTemplateEngine());

        $(window).resize(guiHelper.resize);

        ko.bindingHandlers.fastClick =
        {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel)
            {
                new FastButton
                (
                    element,
                    function ()
                    {
                        valueAccessor()(viewModel, event);
                    }
                );
            }
        };

        guiHelper.refreshIsMobileMode();
    },
    testMode:
    {
        // Instead of downloading the menu from ACS use the one in testMenu.js
        useTestmenu: false,
        // Override the status of the selected store so it's always online
        alwaysOnline: false,
        debugText: ko.observable()
    },
    serverUrl: 'http://' + location.host + '/Services/WebOrdering/webordering',
    viewName: ko.observable(undefined),
    pleaseWaitMessage: ko.observable(''),
    pleaseWaitProgress: ko.observable(undefined),
    sitesMode: ko.observable('pleaseWait'), // The mode that the 'sites' page is currently in
    sites: ko.observableArray(), // A list of sites (from the site list web service call)
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
    chooseStore: function()
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

            // Show the please wait view
            guiHelper.showPleaseWait
            (
                text_gettingStoreDetails,
                undefined, 
                function ()
                {
                    // Clear out the store details
                    viewModel.siteDetails(undefined);

                    // Refresh the site list from the server
                    apiHelper.getSiteList
                    (
                        function ()
                        {
                            // Mark all the offline stores for display purposes
                            helper.markOfflineStores();

                            if (viewModel.sites() === undefined || viewModel.sites().length === 0)
                            {
                                // No stores!!
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
        catch (exception)
        {
            // Got an error
            guiHelper.showError(text_problemGettingSiteDetails, viewModel.chooseStore, exception);
        }
    },
    storeSelected: function(store)
    {
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
            viewModel.selectedItem.menuItems(this.menuItems());
            viewModel.selectedItem.description(this.description);
            viewModel.selectedItem.quantity(this.quantity);
            viewModel.selectedItem.menuItem(menuItem);
            viewModel.selectedItem.freeToppings(menuItem.FreeTops);
            viewModel.selectedItem.freeToppingsRemaining(menuItem.FreeTops);
            viewModel.selectedItem.instructions('');
            viewModel.selectedItem.person('');
            viewModel.selectedItem.selectedCategory1(this.selectedCategory1());
            viewModel.selectedItem.selectedCategory2(this.selectedCategory2());
            viewModel.selectedItem.price(helper.formatPrice(this.price()));

            // Do these last becuase the UI is data bound to them
            viewModel.selectedItem.category1s(this.category1s());
            viewModel.selectedItem.category2s(this.category2s());
            viewModel.selectedItem.category1(category1);
            viewModel.selectedItem.category2(category2);

            // Get the toppings
            viewModel.selectedItem.toppings(menuHelper.getItemToppings(viewModel.selectedItem.menuItem()));

            // Calculate the price
            var price = menuHelper.calculateItemPrice(menuItem, this.quantity, viewModel.selectedItem.toppings());

            // Set the price
            viewModel.selectedItem.price(helper.formatPrice(price));

            // Does the customer need to pick any toppings?
            if (viewModel.selectedItem.toppings().length == 0 &&
                viewModel.selectedItem.category1s().length < 2 &&
                viewModel.selectedItem.category2s().length < 2)
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
        guiHelper.selectedItemChanged(this);
    },
    category1Changed: function (item)
    {
        if (item != undefined)
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
    selectedSection: ko.observable(undefined) // Used for the mobile section selector
};

var guiHelper =
{
    isCartLocked: ko.observable(false),
    isMobileMenuVisible: ko.observable(true),
    canChangeOrderType: ko.observable(false),
    cartActionsCheckout: 'Checkout',
    cartActionsPayment: 'Payment',
    cartActionsMenu: 'Menu',
    cartActions: ko.observable('Menu'),
    isMobileMode: ko.observable(false),
    refreshIsMobileMode: function()
    {
        var newPageWidth = $(window).width();

        if (Number(newPageWidth) < template_mobileWidth)
        {
            guiHelper.isMobileMode(true);
        }
        else
        {
            guiHelper.isMobileMode(false);
        }
    },
    isMenuBuilt: false,
    isViewVisible: ko.observable(true),
    isMenuVisible: ko.observable(false),
    isInnerMenuVisible: ko.observable(false),
    showHome: function ()
    {
        guiHelper.isMobileMenuVisible(true);
        guiHelper.showView('homeView');
        guiHelper.canChangeOrderType(true);

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

        guiHelper.isMobileMenuVisible(false);
    },
    sectionWidth: template_menuSectionWidth,
    setSectionsContainerWidth: function ()
    {
        if (!guiHelper.isMobileMode())
        {
            var sectionsContainerWidth = guiHelper.sectionWidth * viewModel.sections().length; // The sections will never be wider than 650px each
            var newWidth = sectionsContainerWidth + 'px';
            $('#sectionsContainer').css('width', newWidth);
            $('#checkoutSectionsContainer').css('width', newWidth);
        }
    },
    getMaxSectionHeight: function ()
    {
        if (!guiHelper.isMobileMode())
        {
            var maxSectionHeight = 0;
            for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
            {
                var section = $('#section' + sectionIndex);
                var sectionHeight = section.height();

                if (sectionHeight > maxSectionHeight)
                {
                    maxSectionHeight = sectionHeight;
                }
            }

            viewModel.maxSectionHeight(maxSectionHeight + 'px');
        }
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

            if (guiHelper.isMobileMode())
            {
                for (var index = 0; index < viewModel.sections().length; index++)
                {
                    if (index == sectionIndex)
                    {
                        // This section should be visible
                        $('#section' + index).css('display','block');
                    }
                    else
                    {
                        // Hide this section
                        $('#section' + index).css('display','none');
                    }
                }

                $(window).scrollTop(0);
            }
            else
            {
                // Show the section
                var marginLeft = (guiHelper.sectionWidth * -1) * sectionIndex;

                $('#sections').animate
                (
                    {
                        scrollTop: 0
                    },
                    {
                        duration: 500,
                        queue: false
                    }
                );

                $('#sectionsContainer').animate
                (
                    {
                        marginLeft: marginLeft
                    },
                    {
                        duration: 500,
                        queue: false,
                        always: callback
                    }
                );
            }
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
    positionSections: function ()
    {
        var offset = 0;

        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];

            section.Index = sectionIndex + offset; 
            section.Left(((sectionIndex + offset) * guiHelper.sectionWidth) + 'px');
        }
    },
    showView: function (viewName)
    {
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
            viewModel.viewName(viewName);
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
    defaultWebErrorMessage: text_defaultWebErrorMessage,
    defaultInternalErrorMessage: text_defaultInternalErrorMessage,
    defaultPaymentErrorMessage: text_defaultPaymentErrorMessage,
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
        if (guiHelper.isMobileMode())
        {
            cartHelper.checkout();
        }
        else
        {
            guiHelper.showView('menuView');
            guiHelper.showMenuView('checkoutView');
        }
    },
    resize: function ()
    {
        guiHelper.refreshIsMobileMode();

        var offset = 0;
        var newPageWidth = $(window).width();
        var newSectionWidth = guiHelper.sectionWidth;

        if (guiHelper.isMobileMode())
        {
            newSectionWidth = $('#' + css_menu).width();
        }

        guiHelper.sectionWidth = newSectionWidth;

        $('.' + css_sections).css('width', guiHelper.sectionWidth + 'px');
        $('.' + css_section).css('width', guiHelper.sectionWidth + 'px');

        if (!guiHelper.isMobileMode())
        {
            var marginLeft = ((newSectionWidth * -1) * viewModel.currentSectionIndex) + 'px';
            $('#' + css_sectionsContainer).css('marginLeft', marginLeft);

            for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
            {
                var section = viewModel.sections()[sectionIndex];

                section.Left(((sectionIndex + offset) * guiHelper.sectionWidth) + 'px');
            }

            // Let knockout do its thing
            setTimeout
            (
                function ()
                {
                    // Resizing the sections may have affected their height
                    guiHelper.getMaxSectionHeight();
                },
                0
            );
        }
    },
    showStoreMenu: function ()
    {
        // Gets the site details (address etc...) and then downloads the menu
        try
        {
            // Show the please wait view
            guiHelper.showPleaseWait
            (
                text_gettingStoreDetails, 
                undefined,
                function()
                {
                    // Test mode
                    if (viewModel.testMode.alwaysOnline)
                    {
                        viewModel.isTakingOrders(true);
                    }
                    else
                    {
                        viewModel.isTakingOrders(viewModel.selectedSite().isOpen);
                    }

                    // Get the site details
                    apiHelper.getSiteDetails
                    (
                        viewModel.selectedSite().siteId,
                        function ()
                        {
                            // Get the delivery zones
                            apiHelper.getDeliveryZones
                            (
                                viewModel.selectedSite().siteId,
                                function ()
                                {
                                    try
                                    {
                                        // Get the menu from the cache
                                        var menu = cacheHelper.loadCachedMenuForSiteId(viewModel.selectedSite().siteId)

                                        if (menu != undefined)
                                        {
                                            // Rendering the menu can take a couple of seconds on a mobile
                                            guiHelper.showPleaseWait
                                            (
                                                text_loadingMenu,
                                                '',
                                                function ()
                                                {
                                                    // Keep hold of the menu that we just got from the server
                                                    viewModel.menu = menu;

                                                    // Render the menu 
                                                    guiHelper.renderMenu
                                                    (
                                                        viewModel.menu,
                                                        function ()
                                                        {
                                                            // Show the homne page
                                                            guiHelper.showHome();
                                                        }
                                                    );
                                                }
                                            );
                                        }
                                        else
                                        {
                                            // Show the please wait view
                                            guiHelper.showPleaseWait
                                            (
                                                text_downloadingStoreMenu,
                                                '0%',
                                                function ()
                                                {
                                                    // Get the site menu from the server
                                                    apiHelper.getMenu
                                                    (
                                                        viewModel.selectedSite().siteId,
                                                        function (menu)
                                                        {
                                                            try
                                                            {
                                                                // Rendering the menu can take a couple of seconds on a mobile
                                                                viewModel.pleaseWaitMessage(text_savingMenuToDevice);
                                                                viewModel.pleaseWaitProgress('');

                                                                // Allow JavaScript to process events (kind of like doEvents)
                                                                setTimeout
                                                                (
                                                                    function ()
                                                                    {
                                                                        try
                                                                        {
                                                                            // Cache the menu locally.  If this fails just carry on anyway
                                                                            cacheHelper.cacheMenu(menu);

                                                                            // Rendering the menu can take a couple of seconds on a mobile
                                                                            viewModel.pleaseWaitMessage(text_loadingMenu);
                                                                            viewModel.pleaseWaitProgress('');

                                                                            setTimeout
                                                                            (
                                                                                function ()
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        // Keep hold of the menu that we just got from the server
                                                                                        viewModel.menu = menu;

                                                                                        // Render the menu 
                                                                                        guiHelper.renderMenu
                                                                                        (
                                                                                            viewModel.menu,
                                                                                            function ()
                                                                                            {
                                                                                                // Show the homne page
                                                                                                guiHelper.showHome();
                                                                                            }
                                                                                        );
                                                                                    }
                                                                                    catch (exception)
                                                                                    {
                                                                                        // Got an error
                                                                                        guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
                                                                                    }
                                                                                },
                                                                                0
                                                                            );
                                                                        }
                                                                        catch (exception)
                                                                        {
                                                                            // Got an error
                                                                            guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
                                                                        }
                                                                    },
                                                                    0
                                                                );
                                                            }
                                                            catch (exception)
                                                            {
                                                                // Got an error
                                                                guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
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
                                        guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
                                    }
                                }
                            );
                        }
                    );
                }
            );
        }
        catch (exception)
        {
            // Got an error
            guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
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
            text_renderingMenu,
            undefined,
            function ()
            {
                // This is the menu we will be working with
                viewModel.menu = menu;

                if (viewModel.siteDetails().address.country == "United Kingdom")
                {
                    viewModel.addressType('ukAddress-template');
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
                        helper.currencySymbol = '€';
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
                if (todaysOpeningTimes.length == 0 && !viewModel.testMode.alwaysOnline)
                {
                    // Store is closed so can't take any orders
                    viewModel.isTakingOrders(false);
                }

                // Set the display address
                viewModel.displayAddress(displayAddress);
                viewModel.displayAddressMultiLine(displayAddressMultiLine);

                // Clear the menu sections
                viewModel.sections.removeAll();

                // Build an index of items
                menuHelper.buildItemLookup();

                // Build the deals section
                menuHelper.buildDealsSection();

                // Build the sections that contain menu items
                menuHelper.buildItemsSections
                (
                    function ()
                    {
                        // Make the first section visible
                        viewModel.visibleSection(viewModel.sections()[0].display.Name);

                        // We need to set the x position of each section as they are absolutely positioned
                        // We have to do this dynamically in Javascript because the sections not know before hand and we can't float them
                        guiHelper.positionSections();

                        // Make the sections container div wide enough to fit all the sections side by side
                        guiHelper.setSectionsContainerWidth();

                        callback();
                    }
                );
            }
        );
    },
    showMenu: function (index)
    {
        //try
        //{
            var timer = new Date();

            // Show the menu view
            //guiHelper.showView('menuView');
            guiHelper.showMenuView('menuSectionsView');

            guiHelper.isMobileMenuVisible(true);
            guiHelper.canChangeOrderType(true);
            guiHelper.cartActions(guiHelper.cartActionsMenu);

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
                        typeof(index) == 'number' ? index : undefined,
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
        //}
        //catch (exception)
        //{
        //    alert(exception);
        //}

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
        viewModel.selectedSection(this);
        guiHelper.showMenu(this.Index);
    }
}

// Menu helper functions
var menuHelper =
{
    fixName : function(name)
    {
        if (name == undefined)
        {
            return '';
        }

        return name.replace("&#146;", "'");
    },
    refreshItemPrices : function ()
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
    refreshDealsAvailabilty : function()
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
    calculateToppingPrice : function(toppingWrapper, prices)
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
                        price = Math.round(basePrice * percentage);

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

                if ((item.selectedCategory1() == undefined || item.selectedCategory1().Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1))  &&
                    (item.selectedCategory2() == undefined || item.selectedCategory2().Id == (checkItem.Cat2 == undefined ? checkItem.Category2 : checkItem.Cat2)))
                {
                    return checkItem;
                }
            }
        }
    },
    menuItemLookup: undefined,
    buildDealsSection: function ()
    {
        // NOTE:  Don't bind to this data structure.  When the user selects a deal the data will be copied into another data structure which we bind to

        // Clear the deals
        viewModel.deals.removeAll();

        if (menuHelper.areDeals())
        {
            // Process each deal
            for (var dealIndex = 0; dealIndex < viewModel.menu.Deals.length; dealIndex++)
            {
                // The deal to process
                var deal = viewModel.menu.Deals[dealIndex];

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

                // Is this a full order deal?
                if (deal.FullOrderDiscountType == 'Percentage' || deal.FullOrderDiscountType == 'Fixed' || deal.FullOrderDiscountType == 'Discount')
                {
                    // Full order deal is automatically added and should not appear in the deal sections
                    // Is the full order deal available today?
                    if (isAvailableToday)
                    {
                        // We can only have one full order deal applied at a time
                        if (menuHelper.fullOrderDiscountDeal == undefined)
                        {
                            menuHelper.fullOrderDiscountDeal = deal;
                        }
                    }
                }
                else
                {
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
                                    name: text_pleaseSelectAnItem
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

                    var dealWrapper =
                    {
                        deal: deal,
                        isEnabled: ko.observable(menuHelper.isDealItemEnabledForMenu(deal)),
                        isAvailableToday: isAvailableToday,
                        dealLineWrappers: dealLineWrappers,
                        minimumOrderValue: deal.MinimumOrderValue > 0 ? helper.formatPrice(deal.MinimumOrderValue) : ''
                    };

                    viewModel.deals.push(dealWrapper);
                }
            }

            // Add the deals section
            var section = { templateName: 'dealsSection-template', display: { Name: 'Deals' }, deals: viewModel.deals, Index: undefined, Left: ko.observable(0) };
            viewModel.sections.push(section);
        }
    },
    addItemsToDealLine: function(dealLineWrapper)
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
                var itemName = $.trim(viewModel.menu.ItemNames[menuItemWrapper.menuItem.Name == undefined ? menuItemWrapper.menuItem.ItemName : menuItemWrapper.menuItem.Name]);

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
                        name: menuHelper.fixName(itemName),
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
    buildItemLookup: function()
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
                menuItem: menuItem
            };
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

        menuHelper.commitSection
        (
            0,
            sections,
            function ()
            {
                menuHelper.sortItemSections();
                callback();
            }
        );

//        alert('about to add sections');
        //for (var index = 0; index < sections.length; index++)
        //{
        //    var section = sections[index];
        //    viewModel.sections.push(section);

        //    viewModel.pleaseWaitProgress(index * 10 + '%');
        //}
 //       alert('sections added');
        // Sort the items in each section by display order
        //menuHelper.sortItemSections();
    },
    commitSection: function(index, sections, callback)
    {
        if (index >= sections.length)
        {
            callback();
        }
        else
        {
            var section = sections[index];
            viewModel.sections.push(section);

            var percentage = Math.round(((index + 1) / sections.length) * 100);

            viewModel.pleaseWaitProgress(percentage + '%');

            setTimeout
            (
                function ()
                {
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
                if (checkSection.display.Id == menuItem.Display)
                {
                    section = checkSection;
                    break;
                }
            }
        }

        if (display == undefined)
        {
            display = { Name:'?' };
        }
    
        if (section == undefined)
        {
            // No such section - create it
            section = { templateName: 'menuSection-template', display: display, items: ko.observableArray(), Index: undefined, Left: ko.observable(0) };
            //viewModel.sections.push(section);
            sections.push(section);
        }

        // Get the item name
        //var itemName = $.trim(viewModel.menu.ItemNames[menuItem.Name == undefined ? menuItem.ItemName : menuItem.Name]);
        // Get the item name
        var originalItemName = $.trim(viewModel.menu.ItemNames[menuItem.Name == undefined ? menuItem.ItemName : menuItem.Name]);
        var itemName = originalItemName;
        var itemCat1 = undefined;

        // Does this item have a category 1 (e.g. Size)
        var cat1 = menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1;
        if (cat1 != undefined && cat1 != -1)
        {
            itemCat1 = helper.findById(cat1, viewModel.menu.Category1);

            if (itemCat1 != undefined && viewModel.invertItems)
            {
                itemName = $.trim(itemCat1.Name);
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
                name: menuHelper.fixName(itemName),
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
                }
            };
        
            section.items.push(item);
        }

        //// Does this item have a category 1 (e.g. Size)
        //var cat1 = menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1; 
        //if (cat1!= undefined && cat1 != -1)
        //{
        //    var category = helper.findById(cat1, viewModel.menu.Category1);
        //    if (category != undefined && !helper.findCategory(category, item.category1s))
        //    {
        //        item.category1s.push(category);
        //    }
        //}

        if (itemCat1 != undefined && viewModel.invertItems)
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
    },
    sortItemSections : function()
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
    sortByDisplayOrder: function(a, b)
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
    getItemToppings: function(menuItem)
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
                price:  helper.formatPrice(0),
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
                var A = (a.ToppingName == undefined ? a.topping.Name : a.ToppingName).toLowerCase();
                var B = (b.ToppingName == undefined ? b.topping.Name : b.ToppingName).toLowerCase();

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
    addTopping: function(topping, toppings)
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
                    topping.cartName(text_prefixRemove + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.cartPrice(helper.formatPrice(0));
                    topping.cartQuantity('-1');

                    cartDisplayToppings.push(topping);
                }
                else if (topping.selectedDouble())
                {
                    topping.cartName(text_prefixAddTopping + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.cartPrice(helper.formatPrice(menuHelper.getToppingPrice(topping.topping) * topping.quantity));
                    topping.cartQuantity(topping.quantity);

                    cartDisplayToppings.push(topping);
                }
            }
            else
            {
                if (topping.selectedSingle() || topping.selectedDouble())
                {
                    topping.cartName((topping.selectedDouble() ? text_prefixAddDoubleTopping : text_prefixAddTopping) + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
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
    },
    fullOrderDiscountDeal: undefined
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
                        dealLine.categoryPremiumName(text_premiumChargeFor.replace('{item}', category2.Name));
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
                        dealLine.itemPremiumName(text_premiumChargeFor.replace('{item}', dealLine.selectedMenuItem.Name));
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

        var totalPriceItemsOnly = 0;

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
                totalPriceItemsOnly += item.price;
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
                    totalPriceItemsOnly += (toppingsPrice * item.quantity());
                }
            }
        }

        cartHelper.cart().areDisabledItems(areDisabledItems);

        // Calculate the full order discount
        var fullOrderDiscountAmount = cartHelper.calculateFullOrderDiscount(totalPriceItemsOnly);

        // Sub total
        cartHelper.cart().displaySubTotalPrice(helper.formatPrice(totalPrice));
        cartHelper.cart().subTotalPrice(totalPrice);

        // Discount
        var discountName = menuHelper.fullOrderDiscountDeal == undefined ? undefined : menuHelper.fullOrderDiscountDeal.DealName;
        cartHelper.cart().discountName(discountName);
        cartHelper.cart().discountAmount(fullOrderDiscountAmount);
        cartHelper.cart().displayDiscountAmount(helper.formatPrice(fullOrderDiscountAmount * -1));

        // Apply the full order discount
        totalPrice -= fullOrderDiscountAmount;

        // Grand total
        cartHelper.cart().displayTotalPrice(helper.formatPrice(totalPrice));
        cartHelper.cart().totalPrice(totalPrice);

        // True when there are items (enabled or disabled) in the cart
        cartHelper.cart().hasItems(cartHelper.cart().cartItems().length != 0 || cartHelper.cart().deals().length != 0);

        // Is the delivery charge met?  Don't check if already disabled
        if (checkoutEnabled && viewModel.orderType() == 'delivery')
        {
            checkoutEnabled = cartHelper.cart().totalPrice() >= template_minimumDeliveryOrder;
        }

        // True when there is at least one enabled item in the cart
        cartHelper.cart().checkoutEnabled(checkoutEnabled);
    },
    getOrderValueExcludingDealsWithAMinimumPrice: function()
    {
        var totalPrice = 0;

        // Calculate the deal prices
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

        var totalPriceItemsOnly = 0;

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
                totalPriceItemsOnly += itemPrice;
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
                        totalPriceItemsOnly += toppingPrice;
                    }
                }
            }
        }

        // Apply any full order discount deal
        totalPrice -= cartHelper.calculateFullOrderDiscount(totalPriceItemsOnly);

        if (totalPrice < 0) totalPrice = 0;

        return totalPrice;
    },
    calculateFullOrderDiscount: function(itemsPrice)
    {
        var fullOrderDiscount = 0;

        if (menuHelper.fullOrderDiscountDeal != undefined)
        {
            switch (menuHelper.fullOrderDiscountDeal.FullOrderDiscountType.toLowerCase())
            {
                case 'percentage':

                    var percentage = 100;

                    if (viewModel.orderType().toLowerCase() == 'collection')
                    {
                        percentage = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount / 100;
                    }
                    else
                    {
                        percentage = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount / 100;
                    }

                    // Deduct the percentage from the item price
                    fullOrderDiscount = itemsPrice - (itemsPrice * percentage);

                    break;

                case 'discount':

                    if (viewModel.orderType().toLowerCase() == 'collection')
                    {
                        fullOrderDiscount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount;
                    }
                    else
                    {
                        fullOrderDiscount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount;
                    }

                    // Deduct the discount from the item price
                    fullOrderDiscount = itemsPrice - fullOrderDiscount;

                    break;
            }
        }

        return Math.round(fullOrderDiscount);
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
        checkoutHelper.refreshTimes();

        checkoutHelper.initialiseCheckout();

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
        viewModel.selectedItem.quantity(this.quantity);
        viewModel.selectedItem.price(helper.formatPrice(price));
        viewModel.selectedItem.menuItem(this.menuItem);
        viewModel.selectedItem.instructions(this.instructions);
        viewModel.selectedItem.person(this.person);
        viewModel.selectedItem.category1s(this.category1s());
        viewModel.selectedItem.category2s(this.category2s());
        viewModel.selectedItem.toppings(this.toppings());
        viewModel.selectedItem.selectedCategory1(this.selectedCategory1);
        viewModel.selectedItem.selectedCategory2(this.selectedCategory2);

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
        dealHelper.selectedDeal.deal = this.deal;

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
            hasItems: ko.observable(),
            displaySubTotalPrice: ko.observable(),
            subTotalPrice: ko.observable(),
            discountName: ko.observable(),
            discountAmount: ko.observable(),
            displayDiscountAmount: ko.observable()
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
    },
    initialiseCheckout: function()
    {
        var index = 0;

        checkoutHelper.checkoutSections.removeAll();

        // Who are you
        checkoutHelper.checkoutSections.push({ id: 'checkoutContactDetailsContainer', left: ko.observable('0px'), index: index, templateName: 'checkoutContactDetails-template', displayName: (index + 1) + ') ' + text_whoAreYou, validate: checkoutHelper.validateContactDetails });
        index++;

        // Collection/delivery time
        if (viewModel.orderType() == 'collection')
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', left: guiHelper.isMobileMode() ? ko.observable('0px') : ko.observable(guiHelper.sectionWidth + 'px'), index: index, templateName: 'checkoutDeliveryTime-template', displayName: (index + 1) + ') ' + text_collectionTime, validate: checkoutHelper.validateDeliveryTime });
            index++;
        }
        else
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', left: guiHelper.isMobileMode() ? ko.observable('0px') : ko.observable(guiHelper.sectionWidth + 'px'), index: index, templateName: 'checkoutDeliveryTime-template', displayName: (index + 1) + ') ' + text_deliveryTime, validate: checkoutHelper.validateDeliveryTime });
            index++;
        }

        // Delivery address
        if (viewModel.orderType() == 'delivery')
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryAddressContainer', left: guiHelper.isMobileMode() ? ko.observable('0px') : ko.observable((guiHelper.sectionWidth * index) + 'px'), index: index, templateName: 'checkoutDeliveryAddress-template', displayName: (index + 1) + ') ' + text_deliveryAddress, validate: checkoutHelper.validateUKAddress });
            index++;
        }

        // Order notes
        checkoutHelper.checkoutSections.push({ id: 'checkoutOrderNotesContainer', left: guiHelper.isMobileMode() ? ko.observable('0px') : ko.observable((guiHelper.sectionWidth * index) + 'px'), index: index, templateName: 'checkoutOrderNotes-template', displayName: (index + 1) + ') ' + text_orderNotes, validate: function () { return true; } });
        index++;

        // Show the first section
        checkoutHelper.showCheckoutSection(0);
    },
    showCheckoutSection: function (sectionIndex)
    {
        // Back button
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
        
        // Next button
        if (sectionIndex == checkoutHelper.checkoutSections().length - 1)
        {
            checkoutHelper.nextButton.displayText(text_placeOrder);
            checkoutHelper.nextButton.action = 'placeorder';
        }
        else
        {
            checkoutHelper.nextButton.displayText(text_next);
            checkoutHelper.nextButton.action = sectionIndex + 1;
        }

        checkoutHelper.visibleCheckoutSection(sectionIndex);

        var marginLeft = (guiHelper.sectionWidth * -1) * sectionIndex;

        // Don't let the user modify the cart items
        guiHelper.isCartLocked(true);

        $('#checkoutSectionsContainer').animate
        (
            {
                marginLeft: marginLeft
            },
            {
                duration: 500,
                queue: false
            }
        );

        $('#checkoutPageInner').animate
        (
            {
                scrollTop: 0
            },
            {
                duration: 500,
                queue: false
            }
        );
    },
    refreshTimes: function()
    {
        checkoutHelper.times.removeAll();
        checkoutHelper.times.push({ mode: undefined, time: undefined, text: text_pleaseSelectATime });

        // Is the store open for ASAP?
        //if (viewModel.selectedSite().estDelivTime == undefined || viewModel.selectedSite().estDelivTime == 0)
        //{
        //    checkoutHelper.times.push({ mode: 'ASAP', time: undefined, text: text_asSoonAsPossibleNoETD });
        //}
        //else
        //{
        //    checkoutHelper.times.push({ mode: 'ASAP', time: undefined, text: text_asSoonAsPossible + ' (' + viewModel.selectedSite().estDelivTime + ')' });
        //}

        // Get todays opening times
        var openingTimes = openingTimesHelper.getTodaysOpeningTimes();

        var timeBlocks = [];
        var today = new Date();

        // The first available slot has to be at least EDT minutes from now.
        // If EDT is not available default to 15 minutes.
        var offset = 15;
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
                else if (endHour > hourNow ||
                    (hourNow > 6 && endHour < 6) || // Before 6am is the previous day i.e. closes after midnight
                    (endHour == hourNow && endMinute > minuteNow))
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
            endHour = endHour < 6 ? 24 : endHour; // Before 6am is the previous day i.e. closes after midnight

            for (var hour = timeSpan.startHour + 1; hour < endHour; hour++)
            {
                checkoutHelper.addDeliveryHourSlots(hour, 0, 60, slotSize);
            }

            // Do we need to add hours after midnight?
            if (timeSpan.endHour < 6)
            {
                for (var hour = 0; hour < timeSpan.endHour; hour++)
                {
                    checkoutHelper.addDeliveryHourSlots(hour, 0, 60, slotSize);
                }
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
                    text: checkoutHelper.formatHour(hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + hourAMPM + ' - ' +
                          checkoutHelper.formatHour(hourPlusOne12HourClock) + ':' + checkoutHelper.formatMinute((slotSize - (60 - minute))) + hourPlusOneAMPM
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
                    text: checkoutHelper.formatHour(hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + hourAMPM + ' - ' +
                          checkoutHelper.formatHour(hour12HourClock) + ':' + checkoutHelper.formatMinute((minute + slotSize)) + hourAMPM
                }
            );
        }
    },
    formatMinute: function(minute)
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
        chefNotes: ko.observable('')
    },
    placeOrder: function ()
    {
        // Has the user entered the required information?
        if (checkoutHelper.validate())
        {
            checkoutHelper.showPaymentPicker();
        }
    },
    showPaymentPicker: function()
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
        guiHelper.canChangeOrderType(false);
        guiHelper.cartActions(guiHelper.cartActionsCheckout);

        // Reset the agree flag - assume they haven't agreed yet
        tandcHelper.agree(undefined);

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

        viewModel.pleaseWaitMessage(text_preparingForPayment);
        viewModel.pleaseWaitProgress('');
        guiHelper.showView('pleaseWaitView');

        if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
        {
            // Initialise the mercury payment
            apiHelper.putMercuryPayment
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
                        guiHelper.showView('menuView');
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
            apiHelper.putDataCashPayment
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
                        guiHelper.showView('menuView');
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
        else
        {
            // There is a problem with the payment provider - not supported
        }
    },
    sendOrderToStore: function()
    {
        viewModel.pleaseWaitMessage(text_sendingOrderToStore);
        viewModel.pleaseWaitProgress('');
        guiHelper.showView('pleaseWaitView');

        // Work out how long the customer took to place the order
        var timeNow = new Date();
        var timeTakenMilliseconds = Math.abs(timeNow - viewModel.timer);
        var timeTakenSeconds = Math.round(timeTakenMilliseconds / 1000);

        var orderDetails =
        {
            paymentType: checkoutHelper.checkoutDetails.payNow ? viewModel.siteDetails().paymentProvider : 'PayLater',
            paymentData : undefined,
            order :
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
                        prem1: checkoutHelper.checkoutDetails.address.prem1(),
                        prem2: checkoutHelper.checkoutDetails.address.prem2(),
                        prem3: checkoutHelper.checkoutDetails.address.prem3(),
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
                        userLocality1: checkoutHelper.checkoutDetails.address.userLocality1(),
                        userLocality2: checkoutHelper.checkoutDetails.address.userLocality2(),
                        userLocality3: checkoutHelper.checkoutDetails.address.userLocality3(),
                        country: template_customerAddressCountryCode
                    }
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

        // Was a full order discount applied?
        if (cartHelper.cart().discountAmount() != undefined && 
            cartHelper.cart().discountAmount() > 0 &&
            menuHelper.fullOrderDiscountDeal != undefined)
        {
            // Make sure price before discount doesn't include the full order discount (the total price already includes the discount)
            orderDetails.order.pricing.priceBeforeDiscount = cartHelper.cart().subTotalPrice();

            // Add the full order discount details
            orderDetails.order.pricing.discountType = menuHelper.fullOrderDiscountDeal.FullOrderDiscountType;
            if (viewModel.orderType() == 'collection')
            {
                orderDetails.order.pricing.discountTypeAmount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount;
            }
            else
            {
                orderDetails.order.pricing.discountTypeAmount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount;
            }
            orderDetails.order.pricing.discountAmount = cartHelper.cart().discountAmount();
            orderDetails.order.pricing.initialDiscountReason = cartHelper.cart().discountName();
        }

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

        // Send the order to ACS
        apiHelper.putOrder(viewModel.selectedSite().siteId, orderDetails);
    },
    checkoutSections: ko.observableArray(),
    getTemplateName: function (data)
    {
        return data.templateName;
    },
    nextButton: { displayText: ko.observable(text_next), action: 'none' },
    backButton: { visible: ko.observable(false), displayText: ko.observable(text_back), action: 'none' },
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

            // Check if the cleaned up postcode is in the list
            isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(cleanedUpZipCode);
        }

        return isInDeliveryZone;
    },
    deliveryTimeChanged: function ()
    {
        // Fix for iPhone - hides the select popup
        $('#deliveryTime').blur();
    },
    modifyOrder: function ()
    {
        guiHelper.canChangeOrderType(true);
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
    loadCachedMenuForSiteId : function (siteId)
    {
        var menu = undefined;

        try
        {
            // Show the please wait view
            guiHelper.showPleaseWait
            (
                text_loadingMenu,
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
    cacheMenu : function (menu)
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
    cacheObject : function (keyName, object, menuIndex)
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
    removeOldestMenuFromCache : function (menuIndex)
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

var apiHelper =
{
    getSiteList: function (callback)
    {
        // Call the SiteList web service
        var jqxhr = $.ajax(viewModel.serverUrl + "/SiteList?key=x")
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(text_problemGettingSiteDetails, viewModel.chooseStore, exception);
                    }
                    else
                    {
                        // Keep hold of the list of sites we just got from the server
                        viewModel.sites(data);

                        // Let the caller know we're finished
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(text_problemGettingSiteDetails, viewModel.chooseStore, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(text_problemGettingSiteDetails, viewModel.chooseStore, errorThrown);
            }
        );
    },
    getSiteDetails : function (siteId, callback)
    {
        // Call the SiteDetails web service
        var jqxhr = $.ajax(viewModel.serverUrl + "/SiteDetails/" + siteId + "?key=x")
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Keep hold of the site details we just got from the server
                        viewModel.siteDetails(data);

                        // Finished - let the caller know
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, errorThrown);
            }
        );
    },
    getMenu : function (siteId, callback)
    {
        // Show the site list drop down combo
        viewModel.sitesMode('pleaseWait');

        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + "/menu/" + siteId + "?key=x",
                xhr: 
                    function ()
                    {
                        var xhr = new window.XMLHttpRequest();

                        if (xhr.addEventListener != undefined)
                        {
                            xhr.addEventListener
                            (
                                'progress',
                                function (event)
                                {
                                    try
                                    {
                                        if (event.lengthComputable)
                                        {
                                            var percentageComplete = (event.loaded / event.total) * 100;
                                            var progress = parseInt(percentageComplete) + '%';
                                            viewModel.pleaseWaitProgress(progress);
                                            setTimeout(function () { }, 0);
                                        }
                                    }
                                    catch (exception)
                                    {
                                        // Got an error
                                        guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
                                    }
                                },
                                false
                            );
                        }

                        return xhr;
                    }
            }
        )
        .always
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (textStatus === 'error')
                    {
                        // Got an error
                        guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(data);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function ()
            {
                guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, undefined);
            }
        );
    },
    putMercuryPayment: function (siteId, callback)
    {
        // Initialise a Mercury payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/MercuryPayment/' + siteId + '?key=x',
                type: 'PUT',
                contentType: "application/json",
                data: '{ "amount": "' + cartHelper.cart().totalPrice() + '", "returnUrl": "' + window.location.href + '/Done.html"}'
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                    }
                    else
                    {
                        // Keep hold of the mercury payment id
                        cartHelper.cart().mercuryPaymentId(data.paymentId)

                        // Let the caller know we're finished
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, errorThrown);
            }
        );
    },
    putDataCashPayment: function (siteId, callback)
    {
        // Initialise a DataCash payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/DataCashPayment/' + siteId + '?key=x',
                type: 'PUT',
                contentType: "application/json",
                data: '{ "amount": "' + cartHelper.cart().totalPrice() + '", "returnUrl": "' + window.location.href + '/Done.html"}'
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                    }
                    else
                    {
                        // Keep hold of the DataCash payment id
                        cartHelper.cart().dataCashPaymentDetails(data)

                        // Let the caller know we're finished
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, errorThrown);
            }
        );
    },
    putOrder: function (siteId, order)
    {
        // Call the order web service
        var jqxhr =
        $.ajax
        (
            {
                url: viewModel.serverUrl + "/Order/" + siteId + "?key=x",
                type: 'PUT',
                contentType: "application/json",
                data: JSON.stringify(order)
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    // Was there an error?
                    var errorMessage = apiHelper.checkForError(data, false);
                    if (errorMessage.length > 0)
                    {
                        // Let the user know there was an error
                        guiHelper.showError(errorMessage, undefined, guiHelper.showCheckoutAfterError, errorThrown);
                    }
                    else
                    {
                        // Show the order accepted view
                        guiHelper.showView('orderAcceptedView', true);
                    }
                }
                catch (exception)
                {
                    // Let the user know there was an error
                    guiHelper.showError(guiHelper.defaultInternalErrorMessage, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Get an displayable error message
                var errorMessage = apiHelper.checkForError(jqXHR.responseText, true);

                // Let the user know there was an error
                guiHelper.showError (errorMessage, guiHelper.showCheckoutAfterError, errorThrown);
            }
        );
    },
    checkForError: function (data, alwaysReturnAnError)
    {
        var errorMessage = "";

        // Anything to check?
        if (typeof (data) === 'string')
        {
            // Convert the JSON to an object
            var error = undefined;

            try
            {
                error = JSON.parse(data);
            }
            catch (error) {}

            // Is there an error code?
            if (error != undefined && error.errorCode != undefined)
            {
                // There was an error.  Get an error message that we can display
                switch (error.errorCode)
                {
                    case "-1000":
                    case "-1001":
                    case "-1002":

                        if (error.errorMessage != undefined)
                        {
                            errorMessage = text_cardProcessingError.replace('{ErrorMessage}', error.errorMessage);
                        }
                        else
                        {
                            errorMessage = guiHelper.defaultPaymentErrorMessage + text_appendErrorCode + error.errorCode;
                        }
                        break;

                    case "-1004":

                        errorMessage = text_commsProblem;
                        break;

                    case "1":
                    case "-1": //Internal server error
                        errorMessage = guiHelper.defaultWebErrorMessage + text_appendErrorCode + error.errorCode;
                        break;

                    case "1000": // Missing partnerId parameter in request
                    case "1001": // Missing longitude parameter in request
                    case "1002": // Missing latitude parameter in request
                    case "1003": // Missing maxDistance parameter in request
                    case "1004": // No partner found for the specified partner id
                    case "1005": // No group found for the specified group id
                    case "1006": // Missing siteId parameter in request
                    case "1007": // No site found for the specified siteId
                    case "1008": // There is no menu for the specified site id
                    case "1014": // Missing order parameter in request
                    case "1015": // The format of the order id is not valid
                    case "1016": // No order found for the specified siteId
                    case "1017": // PartnerId is not authorized to access this siteId
                    case "1019": // Missing order resource in request
                    case "1028": // Missing applicationId parameter in request
                    case "2501": // Order Wanted Time in order data is invalid format
                    case "2502": // Order Payments element is missing from the order data

                        // DON'T ROLLBACK AUTH - DON'T TRY ANOTHER SERVER
                        errorMessage = guiHelper.defaultWebErrorMessage + text_appendErrorCode + error.errorCode;
                        break;

                    case "2010": // Unable to deliver order to site
                    case "2020": // Unable to deliver order to site
                    case "2100": // Unable to deliver order to site – unspecified error
                    case "2125": // Order number already exists
                    case "2300": // Order Failed – Store Offline

                        // ROLLBACK AUTH - TRY ANOTHER SERVER
                        errorMessage = text_apiProblem + error.errorCode;
                        break;

                    case "2150": // Unable to deliver order to site - timeout
                    case "2200": // Unable to deliver order to site – Await confirmation

                        // DON'T ROLLBACK AUTH - TRY ANOTHER SERVER
                        errorMessage = text_cantDeliverOrder + error.errorCode;
                        break;

                    case "2101": // Unable to parse XML/JSON order data
                    case "2260": // POS system rejected the order.  Price check failed
                    case "2270": // POS system rejected the order.  Invalid order data

                        // ROLLBACK AUTH - DON'T TRY ANOTHER SERVER
                        errorMessage = text_orderRejected + error.errorCode;
                        break;

                    default:
                        errorMessage = guiHelper.defaultWebErrorMessage + text_appendErrorCode + error.errorCode;
                };
            }
            else
            {
                errorMessage = guiHelper.defaultWebErrorMessage;
            }
        }

        if (alwaysReturnAnError && errorMessage.length == 0)
        {
            errorMessage = guiHelper.defaultWebErrorMessage + text_appendUnknownErrorCode;
        }

        return errorMessage;
    },
    getDeliveryZones: function (siteId, callback)
    {
        // Call the DeliveryZones web service
        var jqxhr = $.ajax(viewModel.serverUrl + "/DeliveryZones/" + siteId + "?key=x")
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Keep hold of the delivery zones we just got from the server
                        if (data != undefined)
                        {
                            for (var i = 0; i < data.length; i++)
                            {
                                data[i] = data[i].replace(/\s+/g, '');
                            }
                        }

                        deliveryZoneHelper.deliveryZones(data);

                        // Finished - let the caller know
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(text_problemGettingSiteDetails, viewModel.storeSelected, errorThrown);
            }
        );
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
                displayName += text_appendOffline;
            }

            site.displayText = ko.observable(displayName);
        }
    },
    newOrder: function ()
    {
        // Empty out the checkout
        checkoutHelper.clearCheckout();

        // Empty out the cart
        cartHelper.clearCart();

        guiHelper.showMenu();

        //// Switch back to the menu
        //guiHelper.showView('menuView');
        //guiHelper.showMenuView('menuSectionsView');
        
        //guiHelper.canChangeOrderType(true);

        //setTimeout
        //(
        //    function ()
        //    {
        //        guiHelper.resize();
        //    },
        //    0
        //);
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
            return helper.currencySymbol + " -"
        }
        else
        {
            var x = Number(price);
            var y = x / 100;

            var price = helper.currencySymbol + y.toFixed(2);

            if (helper.useCommaDecimalPoint)
            {
                price = price.replace('.', ',');
            }

            return price;
        }
    },
    currencySymbol: '&pound;',
    useCommaDecimalPoint: false,
    findById: function(id, list)
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
    findCategory: function(category, categories)
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
                displayText = text_openAllDay;
                break;
            }
            else
            {
                if (displayText.length > 0) displayText += ', ';

                displayText += text_fromTo.replace('{from}', timeSpan.startTime).replace('{to}', timeSpan.endTime);
            }
        }

        if (displayText.length == 0)
        {
            openingTimes.displayText(text_closed);
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
                openingTimes = openingTimesHelper.openingTimes.sunday.openingTimes; //alert('sunday');
                break;
            case 1:
                openingTimes = openingTimesHelper.openingTimes.monday.openingTimes; //alert('monday');
                break;
            case 2:
                openingTimes = openingTimesHelper.openingTimes.tuesday.openingTimes; //alert('tuesday');
                break;
            case 3:
                openingTimes = openingTimesHelper.openingTimes.wednesday.openingTimes; //alert('wednesday');
                break;
            case 4:
                openingTimes = openingTimesHelper.openingTimes.thursday.openingTimes; //alert('thursday');
                break;
            case 5:
                openingTimes = openingTimesHelper.openingTimes.friday.openingTimes; //alert('friday');
                break;
            case 6:
                openingTimes = openingTimesHelper.openingTimes.saturday.openingTimes; //alert('saturday');
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
    subscribeToDealLineChanges: function()
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

        if (selectedItemWrapper != undefined && selectedItemWrapper.name != text_pleaseSelectAnItem)
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
            viewModel.selectedItem.menuItems(item.menuItems());
            viewModel.selectedItem.description(item.description);
            viewModel.selectedItem.quantity(1);
            viewModel.selectedItem.menuItem(bindableDealLine.selectedMenuItem);
            viewModel.selectedItem.freeToppings(bindableDealLine.selectedMenuItem.FreeTops);
// Free deal tops????
            viewModel.selectedItem.freeToppingsRemaining(bindableDealLine.selectedMenuItem.FreeTops);
            viewModel.selectedItem.instructions(bindableDealLine.instructions);
            viewModel.selectedItem.person(bindableDealLine.person);
            viewModel.selectedItem.selectedCategory1(category1);
            viewModel.selectedItem.selectedCategory2(category2);
            viewModel.selectedItem.price(helper.formatPrice(0));

            // Do these last becuase the UI is data bound to them
            viewModel.selectedItem.category1s(item.category1s());
            viewModel.selectedItem.category2s(item.category2s());

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
    cloneToppings: function(sourceToppings)
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
                        itemPremium: ko.observable(undefined)
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
            category1s: viewModel.selectedItem.category1s,
            category2s: viewModel.selectedItem.category2s,
            toppings: ko.observableArray(cartToppings),
            isEnabled: ko.observable()
        };

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
    refeshFreeToppings: function()
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
                            usedToppings+=2;
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
        if (viewModel.pickToppings())
        {
            viewModel.category1Changed(viewModel.selectedItem);

            // Get the menu item that the user has selected
            var menuItem = menuHelper.getSelectedMenuItem(viewModel.selectedItem);

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
        if (viewModel.pickToppings())
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
                if (targetDeliveryZone.toUpperCase().slice(0, possibleDeliveryZone.length) == possibleDeliveryZone.toUpperCase())
                {
                    return true;
                }
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
            if (guiHelper.isMobileMode())
            {
                guiHelper.isMobileMenuVisible(false);
                guiHelper.showView('postcodeCheckView');
            }
            else
            {
                popupHelper.isBackgroundVisible(true);
                postcodeCheckHelper.isPopupVisible(true);
            }
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

        guiHelper.canChangeOrderType(false);
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
    initialiseMap: function()
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
                            externalGraphic: template_mapMarker,
                            graphicOpacity: 1.0,
                            graphicWidth: 32,
                            graphicHeight: 37,
                            graphicYOffset: -37
                        }
                    ),
                    projection: wgs84
                }
            );
            window.mapped="yes";

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
        catch(exception) {}
    },
    setStoreMarker: function(longitude, latitude)
    {
        // Is there already a store feature on the map?
        if (typeof(mapHelper.storeMarker) == 'object')
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
            "type" : "Feature",
            "geometry" : 
            {
                "type": "Point", 
                "coordinates": [ lonLat.lon, lonLat.lat ]
            }
        };

        // Add the feature to the map
        var features = 
        {
            "type": "FeatureCollection",
            "features": [ mapHelper.storeMarker ]
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
    hideMenu: function()
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