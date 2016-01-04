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

        // Make the main menu visible
        guiHelper.isMainMenuVisible(true);

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
