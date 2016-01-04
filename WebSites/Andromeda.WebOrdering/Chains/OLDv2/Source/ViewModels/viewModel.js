/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

// This object is data bound to the HTML
function ViewModel()
{
    var self = this;

    self.queryString = {};
    self.initialise = function ()
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
    };
    self.serverUrl = 'http://' + location.host + '/Services/WebOrdering/webordering';
    self.viewName = ko.observable(undefined);
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
    self.chooseStore = function ()
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
                                viewModel.resetOrderType();

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
    };
    self.storeSelected = function (store)
    {
        siteSelectorHelper.isPostcodeTextboxVisible(false);

        viewModel.selectedSite(store);

        guiHelper.showStoreMenu();
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
        price: ko.observable(undefined)
    };
    self.orderType = ko.observable('delivery');
    self.maxSectionHeight = ko.observable(0);
    self.changeSection = function ()
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
    };
    self.addItemToCart = function ()
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
    };
    self.selectedItemChanged = function ()
    {
        if (!viewModel.ignoreEvents)
        {
            guiHelper.selectedItemChanged(this);
        }
    };
    self.category1Changed = function (item)
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
        if (viewModel.selectedSite().estDelivTime != undefined && viewModel.selectedSite().estDelivTime > 0)
        {
            etd = viewModel.selectedSite().estDelivTime;
        }

        return etd;
    };
    self.resetOrderType = function ()
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
    };
    self.returnToHostWebsite = function ()
    {
        window.location.href = settings.parentWebsite;
    };
};

var viewModel = new ViewModel();






































