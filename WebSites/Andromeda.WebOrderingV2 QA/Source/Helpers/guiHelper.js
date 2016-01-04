var guiHelper =
{
    map: undefined,
    areHeaderOptionsVisible: ko.observable(false),
    isMobileMode: ko.observable(false),
    isMobileMenuVisible: ko.observable(true),
    isCartLocked: ko.observable(false),
    canChangeOrderType: ko.observable(false),
    cartActionsCheckout: 'Checkout',
    cartActionsPayment: 'Payment',
    cartActionsMenu: 'Menu',
    cartActions: ko.observable('Menu'),
    refreshIsMobileMode: function ()
    {
        var newPageWidth = $(window).width();

        if (Number(newPageWidth) <= settings.mobileWidth)
        {
            // Are we switching from mobile to normal mode?
            if (!guiHelper.isMobileMode())
            {
                // Do we need to tell the header view model that we've entered mobile mode?
                if (viewModel.headerViewModel() != null &&
                    viewModel.headerViewModel().enterMobileMode != undefined)
                {
                    viewModel.headerViewModel().enterMobileMode();
                }

                // Do we need to tell the content view model that we've entered mobile mode?
                if (viewModel.contentViewModel() != null &&
                    viewModel.contentViewModel().enterMobileMode != undefined)
                {
                    viewModel.contentViewModel().enterMobileMode();
                }
            }

            // Switch to mobile mode
            guiHelper.isMobileMode(true);

            $('body').on("touchstart", function (event)
            {
                event.stopPropagation();
                $(this).click();
            });
        }
        else
        {
            // Normal mode
            // Are we switching from mobile to normal mode?
            if (guiHelper.isMobileMode())
            {
                // Do we need to tell the header view model that we've exited mobile mode?
                if (viewModel.headerViewModel() != null &&
                    viewModel.headerViewModel().exitMobileMode != undefined)
                {
                    viewModel.headerViewModel().exitMobileMode();
                }

                // Do we need to tell the content view model that we've exited mobile mode?
                if (viewModel.contentViewModel() != null &&
                    viewModel.contentViewModel().exitMobileMode != undefined)
                {
                    viewModel.contentViewModel().exitMobileMode();
                }
            }

            // Switch to normal mode
            guiHelper.isMobileMode(false);

            // Sliding menu not supported in mobile mode
            settings.useSlideMenu = false;
        }
    },
    isMenuBuilt: false,
    isViewVisible: ko.observable(true),
    isMenuVisible: ko.observable(false),
    isInnerMenuVisible: ko.observable(false),
    showView: function (viewName, newContentViewModel, forceShowSameView)
    {
        var start = new Date();
        forceShowSameView = forceShowSameView === undefined ? false : forceShowSameView;

        if (!forceShowSameView && viewName === guiHelper.getCurrentViewName())
        {
            // Already showing the view
            if (newContentViewModel !== undefined && 
                typeof (newContentViewModel.onShown) === "function")
            {
                newContentViewModel.onShown();
            }

            $(window).scrollTop(0);

            return;
        }

        if (newContentViewModel === undefined)
        {
            newContentViewModel = new DefaultViewModel();
        }

        if (viewModel.contentViewModel() !== undefined)
        {
            if (typeof (newContentViewModel.onClosed) == "function")
            {
                newContentViewModel.onClosed();
            }
        }

        // Google analytics
        ga
        (
            'send',
            {
                'hitType': 'pageview',
                'page': '/' + viewName,
                'title': viewName
            }
        );

        if (viewModel.contentViewModel() !== undefined &&
            viewModel.contentViewModel().cannotBePrevious === true &&
            viewModel.contentViewModel().previousContentViewModel !== undefined)
        {
            // Use the previous view from the current view i.e. skip back another view
            viewModel.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
            viewModel.previousViewName = viewModel.contentViewModel().previousViewName;
        }
        else
        {
            viewModel.previousContentViewModel = viewModel.contentViewModel();
            viewModel.previousViewName = guiHelper.getCurrentViewName();
        }

        viewModel.contentViewModel(newContentViewModel);

        // Do we need to show the menu?  This is a bit of a fudge - the menu used to be a view
        // but using a template to hide and show it triggers a shed load of knockout events which
        // causes a several second delay on the stock Android (and possibly other) browsers.
        // Instead, we do the slow menu bindings once and and hide/show it
        if (viewName === 'menuView')
        {
            
            // Hide the view
            guiHelper.isViewVisible(false);

            // Show the menu
            guiHelper.isMenuVisible(true);
            guiHelper.isInnerMenuVisible(true);
            
            $('#viewContainerOuter').css('display', 'none');
            $('#menuSectionWrapper').css('display', 'block');
            
        }
        else
        {
            viewModel.viewName(viewName);

            // Hide the menu
            guiHelper.isMenuVisible(false);
            guiHelper.isInnerMenuVisible(false);
            $('#menuSectionWrapper').css('display', 'none');
            $('#viewContainerOuter').css('display', 'block');

            //Bug 7410:GPRS\Andro Web: Map is displayed blank once the data is changed in Profile page
            if (viewName == 'storeDetailsView')
            {
                setTimeout
                     (
                         function () {
                             // Initialise the map
                             guiHelper.map = mapHelper.initialiseMap('map', viewModel.siteDetails());

                             guiHelper.finished();
                         },
                         0
                     );
            }

            // Show the view
            guiHelper.isViewVisible(true);
        }

        if (typeof (newContentViewModel.onShown) === "function")
        {
            newContentViewModel.onShown();
        }

        $(window).scrollTop(0);

        var end = new Date();

   //     alert((end.getTime() - start.getTime()) / 1000);
    },
    getCurrentViewName: function()
    {
        if (guiHelper.isMenuVisible())
        {
            return 'menuView';
        }
        else
        {
            return viewModel.viewName();
        }
    },
    showPleaseWait: function (message, progress, callback)
    {
        viewModel.pleaseWaitMessage(message);
        viewModel.pleaseWaitProgress(progress);

        var pleaseWaitViewModel = new PleaseWaitViewModel
        (
            function ()
            {
                if (typeof (callback) == 'function')
                {
                    // Let knockout do its thing
                    setTimeout
                    (
                        function ()
                        {
                            callback(pleaseWaitViewModel);
                        },
                        0
                    );
                }
            }
        );

        viewModel.pageManager.showPage('PleaseWait', false, pleaseWaitViewModel, false);
    },
    showCheckoutAfterError: function ()
    {
        if (cartHelper.showCheckoutView())
        {
            viewModel.pageManager.showPage('Checkout', true, undefined, true);
        }
    },
    showMenu: function (index)
    {
        var timer = new Date();

        guiHelper.scrollTopOfMenu();

        // Make the main menu visible
        guiHelper.areHeaderOptionsVisible(true);

        // Make sure that neither the toppings or deal popup are showing (only possible if back button clicked)
        toppingsPopupHelper.isBackgroundVisible(false);
        toppingsPopupHelper.isPopupVisible(false);
        dealPopupHelper.isBackgroundVisible(false);
        dealPopupHelper.isPopupVisible(false);

        guiHelper.showView('menuView', new MenuViewModel());

        guiHelper.cartActions(guiHelper.cartActionsMenu);
        guiHelper.isCartLocked(false);

        // make sure the user can switch order types
        guiHelper.canChangeOrderType(true);

        // Tell the cart it's not on the checkout page
        cartHelper.isCheckoutMode(false);

        // Let knockout update the bindings
        setTimeout
        (
            function ()
            {
                var timeNow = new Date();
                var sv = Math.abs(timeNow - timer);

                // Make sure the sections are layed out correctly
                guiHelper.resize();

                guiHelper.showMenuSection
                (
                    typeof (index) == 'number' ? index : undefined,
                    function ()
                    {
                        // When the customer started placing the order
                        viewModel.timer = new Date();

                        timeNow = new Date();
                        var tot = Math.abs(timeNow - timer);
                        //                   alert('showview:' + sv + ' Total:' + tot);

                        guiHelper.finished();

                        $('#mobileMenuSectionsSelect').blur();
                    }
                );
            },
            0
        );

        // DON'T ADD CODE HERE - ADD IT EITHER BEFORE setTimeout OR IN THE setTimeout CALLBACK FUNCTION
    },
    showAddDealPopup: function (state)
    {
        // If the back butto
        if (state === undefined)
        {
            state = { mode: 'addDeal', cartDeal: dealPopupHelper.cartDeal };
        }

        dealPopupHelper.showDealPopup(state.mode, true, state.cartDeal);
    },
    showMenuSection: function (sectionIndex, callback)
    {
        sectionIndex = (sectionIndex === undefined ? 0 : sectionIndex);

        if (sectionIndex != undefined)
        {
            sectionIndex = typeof (sectionIndex) == 'number' ? sectionIndex : 0;

            var section = viewModel.sections()[sectionIndex];

            ga
            (
                'send',
                {
                    'hitType': 'pageview',
                    'page': '/menuView/' + section.display.Name,
                    'title': section.display.Name
                }
            );

            viewModel.selectedSection(section);

            viewModel.currentSectionIndex = sectionIndex;
            viewModel.visibleSection(section.display.Name);

            for (var index = 0; index < viewModel.sections().length; index++)
            {
                var targetSection = viewModel.sections()[index];

                if (index == sectionIndex)
                {
                    // This section should be visible
                    $('#section' + targetSection.Index).css('display', 'block');
                }
                else
                {
                    // Hide this section
                    $('#section' + targetSection.Index).css('display', 'none');
                }
            }

            $('#sections').scrollTop(0);
        }
        else
        {
            if (callback != undefined) callback();
        }

        guiHelper.finished();
    },
    showDefaultMenuSection: function ()
    {
        if (guiHelper.isMobileMode())
        {
            guiHelper.showMenu(this.Index);
        }
        else
        {
            viewModel.selectedSection(this);
            guiHelper.showMenu(this.Index);
        }
    },
    resize: function ()
    {
        guiHelper.refreshIsMobileMode();
    },
    downloadAndShowStoreMenu: function (lockToOrderType, callback)
    {
        // Gets the site details (address etc...) and then downloads the menu
        try
        {
            // Initialise telemetry
            viewModel.telemetry = new AndroWeb.telemetryHelper();

            // Test mode
            if (settings.alwaysOnline)
            {
                viewModel.isTakingOrders(true);
            }
            else
            {
                viewModel.isTakingOrders(viewModel.selectedSite().isOpen);
            }

            // Make sure we're set to the correct order type
            if (lockToOrderType != undefined)
            {
                viewModel.orderType(lockToOrderType);
                guiHelper.canChangeOrderType(false);
            }
            else
            {
                viewModel.resetOrderType();
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
                                                viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.storeSelected, undefined);
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
                                                        menuHelper.menuDataThumbnails(JSON.parse(data.Menu.MenuDataThumbnails));
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
                                                                    accountHelper.generateWelcomeMessage();
                                                                    
                                                                    if (settings.isHomePageEnabled)
                                                                    {
                                                                        // Show the home page
                                                                        viewModel.pageManager.showPage('Home', true, undefined, false);
                                                                    }
                                                                    else
                                                                    {
                                                                        // Show the menu page
                                                                        viewModel.pageManager.showPage('Menu', true, undefined, true);
                                                                    }
                                                                }
                                                            );
                                                        }
                                                        catch (exception)
                                                        {
                                                            // Got an error
                                                            viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.storeSelected, exception);
                                                        }
                                                    },
                                                    0
                                                );
                                            }
                                        }
                                        catch (exception)
                                        {
                                            // Got an error
                                            viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.storeSelected, exception);
                                        }
                                    },
                                    0
                                );
                            }
                            catch (exception)
                            {
                                // Got an error
                                viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.storeSelected, exception);
                            }
                        },
                        function ()
                        {
                            viewModel.storeSelected();
                        },
                        false
                    );
                }
            );
        }
        catch (exception)
        {
            // Got an error
            viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.storeSelected, exception);
        }
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
    renderMenu: function (menu, callback)
    {
        guiHelper.showPleaseWait
        (
            textStrings.renderingMenu,
            undefined,
            function ()
            {
                var rmStart = new Date();

                // This is the menu we will be working with
                viewModel.menu = menu;

                // Figure out what address format to use
                if (viewModel.siteDetails().address.country == "United Kingdom")
                {
                    viewModel.addressType('ukAddress-min-template');
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
                if (settings.showStoreName)
                {
                    displayAddress += viewModel.siteDetails().name + ', ';
                }
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

                // Get the store address
                var displayAddressMultiLine = guiHelper.generateMultiLineAddress(viewModel.siteDetails().address);

                // Do we need to show the store name?
                if (settings.showStoreName)
                {
                    if (displayAddressMultiLine.length > 0)
                    {
                        displayAddressMultiLine = '<br />' + displayAddressMultiLine;
                    }

                    displayAddressMultiLine = viewModel.siteDetails().name + displayAddressMultiLine;
                }

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

                // See if any menu items have been temporarily disabled
                menuHelper.lookupDisabledMenuItems();

                // Build the sections that contain menu items
                var preEnd = new Date();
                menuHelper.buildItemsSections
                (
                    function ()
                    {
                        // Use the new menu html generation rather than knockout binding?
                        var genStart = new Date();
                        if (!settings.useMenuKnockoutBinding)
                        {
                            guiHelper.generateMenuHtml();
                        }
                        var genEnd = new Date();

                        var dealStart = new Date();
                        // Build the deals section
                        menuHelper.buildDealsSection();

                        var mutStart = new Date();
                        viewModel.sections.valueHasMutated();
                        viewModel.deals.valueHasMutated();
                        var mutEnd = new Date();

                        var rmEnd = new Date();
                        if (settings.diagnosticsMode)
                        {
                            alert('total render menu: ' + (rmEnd.getTime() - rmStart.getTime()) / 1000 + '\r\n' +
                                  'pre: ' + (preEnd.getTime() - rmStart.getTime()) / 1000 + '\r\n' +
                                  'deal: ' + (preEnd.getTime() - dealStart.getTime()) / 1000 + '\r\n' +
                                  'gen html: ' + (genEnd.getTime() - genStart.getTime()) / 1000 + '\r\n' +
                                  'valueHasMutated: ' + (mutEnd.getTime() - mutStart.getTime()) / 1000);
                        }

                        callback();
                    }
                );
            }
        );
    },
    generateMultiLineAddress: function(address)
    {
        var displayAddressMultiLine = "";
        displayAddressMultiLine += address.roadNum != null && address.roadNum.length > 0 ? address.roadNum + ' ' : '';
        displayAddressMultiLine += address.roadName != null && address.roadName.length > 0 ? address.roadName + '<br />' : '';
        displayAddressMultiLine += address.org1 != null && address.org1.length > 0 ? address.org1 + '<br />' : '';
        displayAddressMultiLine += address.org2 != null && address.org2.length > 0 ? address.org2 + '<br />' : '';
        displayAddressMultiLine += address.org3 != null && address.org3.length > 0 ? address.org3 + '<br />' : '';
        displayAddressMultiLine += address.prem1 != null && address.prem1.length > 0 ? address.prem1 + '<br />' : '';
        displayAddressMultiLine += address.prem2 != null && address.prem2.length > 0 ? address.prem2 + '<br />' : '';
        displayAddressMultiLine += address.prem3 != null && address.prem3.length > 0 ? address.prem3 + '<br />' : '';
        displayAddressMultiLine += address.prem4 != null && address.prem4.length > 0 ? address.prem4 + '<br />' : '';
        displayAddressMultiLine += address.prem5 != null && address.prem5.length > 0 ? address.prem5 + '<br />' : '';
        displayAddressMultiLine += address.prem6 != null && address.prem6.length > 0 ? address.prem6 + '<br />' : '';
        displayAddressMultiLine += address.locality != null && address.locality.length > 0 ? address.locality + '<br />' : '';
        displayAddressMultiLine += address.town != null && address.town.length > 0 ? address.town + '<br />' : '';
        displayAddressMultiLine += address.county != null && address.county.length > 0 ? address.county + '<br />' : '';
        displayAddressMultiLine += address.state != null && address.state.length > 0 ? address.state + '<br />' : '';
        displayAddressMultiLine += address.postcode != null && address.postcode.length > 0 ? address.postcode + '<br />' : '';

        return displayAddressMultiLine;
    },
    selectedItemChanged: function (item)
    {
        if (item != undefined)
        {
            // Get the menu item that the user has selected
            item.menuItem = menuHelper.getSelectedMenuItem(item);

            if (item.menuItem != undefined)
            {
                var quantity = (typeof (item.quantity) == 'function' ? item.quantity() : item.quantity);
                var toppings = (typeof (item.toppings) == 'function' ? item.toppings() : undefined);

                // Update the price
                //item.price(helper.formatPrice(menuHelper.calculateItemPrice(item.menuItem, quantity, toppings)));

                // Bug: 9069	GPRS\Andro Web: Price of an item is not updated based on the selected crust and quantity
                item.price(helper.formatPrice(menuHelper.calculateItemPrice(item.menuItem, 1, toppings)));
            }
        }
    },
    enableUI: function (enable)
    {
        var viewContainer = $('#viewContainer');
        guiHelper.enableDisableElement(viewContainer, enable);

        var menuContainer = $('#menuSectionWrapper');
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
    },
    generateMenuHtml: function ()
    {
        var menuHtml = '';
        
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];

            if (section.items != undefined)
            {
                menuHtml += guiHelper.generateMenuSectionHtml(section, sectionIndex);
            }
        }
        $('#sectionsContainer').html(menuHtml);
    },
    generateMenuSectionHtml: function (section, sectionIndex)
    {
        var menuSectionHtml = $('#menuSection2-template').html();
        var menuItemHtml = $('#menuSectionItem-template').html();
        var menuItemCategoryQuantityHtml = $('#menuSectionItemCategoryQuantity-template').html();

        var itemsHtml = '';
        for (var itemIndex = 0; itemIndex < section.items().length; itemIndex++)
        {
            var item = section.items()[itemIndex];

            var categoryQuantityHtml = '';
            if (item.isEnabled && viewModel.isTakingOrders)
            {
                categoryQuantityHtml = menuItemCategoryQuantityHtml
                    .replace('{quantityLabel}', textStrings.miQuantity);
            }

            var thumbnailHtml = '';
            if (item.thumbnail != undefined && item.thumbnail.Src.length > 0)
            {
                thumbnailHtml =
                    '<img' +
                        ' src="' + menuHelper.menuDataThumbnails().Server.Endpoint + item.thumbnail.Src + '"' +
                        ' height="' + item.thumbnailHeight + 'px"' +
                        ' width="' + item.thumbnailWidth + 'px"' + ' />';
                //                        '<img src="#" data-bind="click: imageHelper.showPopup, 'attr': { src: menuHelper.menuDataThumbnails().Server.Endpoint + thumbnail.Src, height: thumbnailHeight + 'px', width: thumbnailWidth + 'px' }"  />';

                if (item.overlayImage)
                {
                    thumbnailHtml += '<div class="itemImageOverlay"></div>';
                    // <div class="itemImageOverlay" data-bind="click: imageHelper.showPopup"></div>
                }
            }

            var category2s = '';
            if (item.category2s().length > 1)
            {
                category2s = '<select>';
                for (var category2Index = 0; category2Index < item.category2s().length; category2Index++)
                {
                    var category2 = item.category2s()[category2Index];
                    category2s += '<option>' + category2.Name + '</option>';
                }
                category2s += '</select>';
                // <select data-bind="enable: isEnabled, options: category2s, optionsText: 'Name', value: selectedCategory2, event: { change: $root.selectedItemChanged }"></select>
            }

            var category1s = '';
            if (item.category1s().length > 1)
            {
                category1s = '<select>';
                for (var category1Index = 0; category1Index < item.category1s().length; category1Index++)
                {
                    var category1 = item.category1s()[category1Index];
                    category1s += '<option>blah' + category1.Name + '</option>';
                }
                category1s += '</select>';
                //category1s = '<select data-bind="enable: isEnabled, options: category1s, optionsText: 'Name', value: selectedCategory1, event: { change: $root.category1Changed }"></select>
                // <select data-bind="enable: isEnabled, options: category1s, optionsText: 'Name', value: selectedCategory1, event: { change: $root.category1Changed }"></select>
            }

            itemsHtml += menuItemHtml
                .replace('{itemName}', item.name)
                .replace('{itemDescription}', item.description)
                .replace('{categoryQuantity}', categoryQuantityHtml)
                .replace('{thumbnailHeight}', item.thumbnailHeight)
                .replace('{thumbnailWidth}', item.thumbnailWidth)
                .replace('{thumbnail}', thumbnailHtml)
                .replace('{addLabel}', textStrings.miAdd)
                .replace('{price}', item.price())
                .replace('{isTakingOrders}', viewModel.isTakingOrders ? 'block' : 'none')
                .replace('{isMobileMode}', guiHelper.isMobileMode() ? 'block' : 'none')
                .replace('{isNotMobileMode}', !guiHelper.isMobileMode() ? 'block' : 'none')
                .replace('{itemClass}', item.thumbnail == undefined ? 'itemDetails' : 'itemDetailsWithImage')
                .replace('{category2s}', category2s)
                .replace('{category1s}', category1s);
        }

        return menuSectionHtml
            .replace('{sectionItems}', itemsHtml)
            .replace('{sectionId}', sectionIndex);
    },
    onWindowScroll: function()
    {
        var scrollTop = $(window).scrollTop();
        guiHelper.isSidebarFixed(scrollTop > 140);
    },
    isSidebarFixed: ko.observable(false),
    finished: function ()
    {
        setTimeout
        (
            function () { window.isFinished = true; },
            0
        );
    },
    /* visible view helpers */
    scrollTopOfMenu: function () {
        var scrollTop = $(window).scrollTop();

        if (scrollTop < 140) { return; }
        //scroll to the top of the menu section. 
        var menuTop = $("#sectionsWrapper").offset().top;
        $("html,body").animate({ scrollTop: menuTop }, "fast");

        //$(window).scrollTop(0);
        return false;
    },
    scrollTopOfPage: function (element) {
        element || (element = "html,body");
        
        var scrollTop = $(window).scrollTop();

        if (scrollTop < 140) { return; }

        $("html,body").animate({ scrollTop: element === "html,body" ? 0 : $(element).offset().top }, "fast")
    }
}
