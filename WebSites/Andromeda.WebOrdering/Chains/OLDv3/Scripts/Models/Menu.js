/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function menu()
{
    var self = this;

    self.rawMenu = undefined;
    self.menu = undefined;
    self.sections = ko.observableArray();
    self.render = function (rawMenu, callback, progressCallback)
    {
        // This is the menu we will be working with
        self.rawMenu = rawMenu;

        if (app.site().siteDetails().address.country == "United Kingdom")
        {
            app.site().addressType('ukAddress-template');
        }
        else
        {
            app.site().addressType('genericAddress-template');
        }

        // Build a store address we can display on the page
        var displayAddress = "";
        displayAddress += app.site().siteDetails().address.roadNum != null && app.site().siteDetails().address.roadNum.length > 0 ? app.site().siteDetails().address.roadNum + ' ' : '';
        displayAddress += app.site().siteDetails().address.roadName != null && app.site().siteDetails().address.roadName.length > 0 ? app.site().siteDetails().address.roadName + ', ' : '';
        displayAddress += app.site().siteDetails().address.org1 != null && app.site().siteDetails().address.org1.length > 0 ? app.site().siteDetails().address.org1 + ', ' : '';
        displayAddress += app.site().siteDetails().address.org2 != null && app.site().siteDetails().address.org2.length > 0 ? app.site().siteDetails().address.org2 + ', ' : '';
        displayAddress += app.site().siteDetails().address.org3 != null && app.site().siteDetails().address.org3.length > 0 ? app.site().siteDetails().address.org3 + ', ' : '';
        displayAddress += app.site().siteDetails().address.prem1 != null && app.site().siteDetails().address.prem1.length > 0 ? app.site().siteDetails().address.prem1 + ', ' : '';
        displayAddress += app.site().siteDetails().address.prem2 != null && app.site().siteDetails().address.prem2.length > 0 ? app.site().siteDetails().address.prem2 + ', ' : '';
        displayAddress += app.site().siteDetails().address.prem3 != null && app.site().siteDetails().address.prem3.length > 0 ? app.site().siteDetails().address.prem3 + ', ' : '';
        displayAddress += app.site().siteDetails().address.prem4 != null && app.site().siteDetails().address.prem4.length > 0 ? app.site().siteDetails().address.prem4 + ', ' : '';
        displayAddress += app.site().siteDetails().address.prem5 != null && app.site().siteDetails().address.prem5.length > 0 ? app.site().siteDetails().address.prem5 + ', ' : '';
        displayAddress += app.site().siteDetails().address.prem6 != null && app.site().siteDetails().address.prem6.length > 0 ? app.site().siteDetails().address.prem6 + ', ' : '';
        displayAddress += app.site().siteDetails().address.locality != null && app.site().siteDetails().address.locality.length > 0 ? app.site().siteDetails().address.locality + ', ' : '';
        displayAddress += app.site().siteDetails().address.town != null && app.site().siteDetails().address.town.length > 0 ? app.site().siteDetails().address.town + ', ' : '';
        displayAddress += app.site().siteDetails().address.county != null && app.site().siteDetails().address.county.length > 0 ? app.site().siteDetails().address.county + ', ' : '';
        displayAddress += app.site().siteDetails().address.state != null && app.site().siteDetails().address.state.length > 0 ? app.site().siteDetails().address.state + ', ' : '';
        displayAddress += app.site().siteDetails().address.postcode != null && app.site().siteDetails().address.postcode.length > 0 ? app.site().siteDetails().address.postcode + ', ' : '';

        if (displayAddress[displayAddress.length - 2] == ',')
        {
            displayAddress = displayAddress.substr(0, displayAddress.length - 2);
        }

        var displayAddressMultiLine = "";
        displayAddressMultiLine += app.site().siteDetails().address.roadNum != null && app.site().siteDetails().address.roadNum.length > 0 ? app.site().siteDetails().address.roadNum + ' ' : '';
        displayAddressMultiLine += app.site().siteDetails().address.roadName != null && app.site().siteDetails().address.roadName.length > 0 ? app.site().siteDetails().address.roadName + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.org1 != null && app.site().siteDetails().address.org1.length > 0 ? app.site().siteDetails().address.org1 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.org2 != null && app.site().siteDetails().address.org2.length > 0 ? app.site().siteDetails().address.org2 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.org3 != null && app.site().siteDetails().address.org3.length > 0 ? app.site().siteDetails().address.org3 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.prem1 != null && app.site().siteDetails().address.prem1.length > 0 ? app.site().siteDetails().address.prem1 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.prem2 != null && app.site().siteDetails().address.prem2.length > 0 ? app.site().siteDetails().address.prem2 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.prem3 != null && app.site().siteDetails().address.prem3.length > 0 ? app.site().siteDetails().address.prem3 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.prem4 != null && app.site().siteDetails().address.prem4.length > 0 ? app.site().siteDetails().address.prem4 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.prem5 != null && app.site().siteDetails().address.prem5.length > 0 ? app.site().siteDetails().address.prem5 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.prem6 != null && app.site().siteDetails().address.prem6.length > 0 ? app.site().siteDetails().address.prem6 + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.locality != null && app.site().siteDetails().address.locality.length > 0 ? app.site().siteDetails().address.locality + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.town != null && app.site().siteDetails().address.town.length > 0 ? app.site().siteDetails().address.town + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.county != null && app.site().siteDetails().address.county.length > 0 ? app.site().siteDetails().address.county + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.state != null && app.site().siteDetails().address.state.length > 0 ? app.site().siteDetails().address.state + '<br />' : '';
        displayAddressMultiLine += app.site().siteDetails().address.postcode != null && app.site().siteDetails().address.postcode.length > 0 ? app.site().siteDetails().address.postcode + '<br />' : '';

        // Currency symbol
        switch (app.site().siteDetails().address.country)
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
        app.site().getToday();
        app.site().getOpeningHours(app.site().openingTimes.monday, 'Monday');
        app.site().getOpeningHours(app.site().openingTimes.tuesday, 'Tuesday');
        app.site().getOpeningHours(app.site().openingTimes.wednesday, 'Wednesday');
        app.site().getOpeningHours(app.site().openingTimes.thursday, 'Thursday');
        app.site().getOpeningHours(app.site().openingTimes.friday, 'Friday');
        app.site().getOpeningHours(app.site().openingTimes.saturday, 'Saturday');
        app.site().getOpeningHours(app.site().openingTimes.sunday, 'Sunday');

        // Is the store open today?
        var todaysOpeningTimes = app.site().getTodaysOpeningTimes();
        if (todaysOpeningTimes.length == 0)
        {
            // Store is closed so can't take any orders
            app.site().isTakingOrders(false);
        }

        // Set the display address
        app.site().displayAddress(displayAddress);
        app.site().displayAddressMultiLine(displayAddressMultiLine);

        // Show the phone number and address in the header
        app.headerViewModel.displayAddress(displayAddress);
        app.headerViewModel.phone(app.site().siteDetails().phone);

        // Clear the menu sections
        self.sections.removeAll();

        // Build an index of items
        self.buildItemLookup();

        // Build the deals section
        self.buildDealsSection();

        // Build the sections that contain menu items
        self.buildItemsSections(callback, progressCallback);

        // Set the indexes
        for (var sectionIndex = 0; sectionIndex < app.site().menu.sections().length; sectionIndex++)
        {
            var section = app.site().menu.sections()[sectionIndex];

            section.Index = sectionIndex;
        }
    };
    self.buildItemsSections = function (callback, progressCallback)
    {
        var sections = [];

        // Process each menu item
        for (var index = 0; index < self.rawMenu.Items.length; index++)
        {
            var menuItem = self.rawMenu.Items[index];

            // Add the item to the correct section
            self.addItemToSection(sections, menuItem);
        }

        self.commitSection
        (
            0,
            sections,
            function ()
            {
                self.sortItemSections();
                callback();
            },
            progressCallback
        );

        //        alert('about to add sections');
        //for (var index = 0; index < sections.length; index++)
        //{
        //    var section = sections[index];
        //    self.sections.push(section);

        //    guiHelper.pleaseWaitViewModel.pleaseWaitProgress(index * 10 + '%');
        //}
        //       alert('sections added');
        // Sort the items in each section by display order
        //self.sortItemSections();
    };
    self.addItemToSection = function (sections, menuItem)
    {
        var display = helper.findById(menuItem.Display, self.rawMenu.Display);

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
            display = { Name: '?' };
        }

        if (section == undefined)
        {
            // No such section - create it
            section = { templateName: 'menuSection-template', display: display, items: ko.observableArray(), Index: undefined, Left: ko.observable(0) };

            sections.push(section);
        }

        // Get the item name
        var itemName = $.trim(self.rawMenu.ItemNames[menuItem.Name == undefined ? menuItem.ItemName : menuItem.Name]);

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
                name: self.fixName(itemName),
                isEnabled: ko.observable(self.isItemEnabledForMenu(menuItem)),
                menuItem: menuItem,
                displayOrder: menuItem.DispOrder,
                description: self.fixName(menuItem.Desc == undefined ? menuItem.Description : menuItem.Desc),
                category1s: ko.observableArray(),
                category2s: ko.observableArray(),
                menuItems: ko.observableArray(),
                selectedCategory1: ko.observable(),
                selectedCategory2: ko.observable(),
                price: ko.observable(helper.formatPrice(self.getItemPrice(menuItem))),
                quantity: 1,
                isAvailableForDelivery: function ()
                {
                    return app.site().cart.orderType() == 'delivery' && this.DelPrice == undefined && this.DeliveryPrice == undefined;
                },
                isAvailableForCollection: function ()
                {
                    return app.site().cart.orderType() == 'collection' && this.ColPrice == undefined && this.CollectionPrice == undefined;
                }
            };

            section.items.push(item);
        }

        // Does this item have a category 1 (e.g. Size)
        var cat1 = menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1;
        if (cat1 != undefined && cat1 != -1)
        {
            var category = helper.findById(cat1, self.rawMenu.Category1);
            if (category != undefined && !helper.findCategory(category, item.category1s))
            {
                item.category1s.push(category);
            }
        }

        // Does this item have a category 2 (e.g. Size)
        var cat2 = menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2;
        if (cat2 != undefined && cat2 != -1)
        {
            var category = helper.findById(cat2, self.rawMenu.Category2);
            if (category != undefined && !helper.findCategory(category, item.category2s))
            {
                item.category2s.push(category);
            }
        }

        // Add this menu item to the variations of this particular product
        item.menuItems.push(menuItem);
    };
    self.commitSection = function (index, sections, callback, progressCallback)
    {
        if (index >= sections.length)
        {
            callback();
        }
        else
        {
            var section = sections[index];
            app.site().menu.sections.push(section);

            var percentage = Math.round(((index + 1) / sections.length) * 100);

            progressCallback(percentage + '%');

            self.commitSection(index + 1, sections, callback, progressCallback);
        }
    };
    self.fixName = function (name)
    {
        if (name == undefined)
        {
            return '';
        }

        return name.replace("&#146;", "'");
    };
    self.refreshItemPrices = function ()
    {
        // Change each section
        for (var sectionIndex = 0; sectionIndex < self.sections().length; sectionIndex++)
        {
            var section = self.sections()[sectionIndex];

            // Does this section have any items?
            if (section.items != undefined)
            {
                // Change the items in the section
                for (var itemIndex = 0; itemIndex < section.items().length; itemIndex++)
                {
                    var item = section.items()[itemIndex];

                    // Set the price depending on the current order type
                    item.price(helper.formatPrice(self.getItemPrice(item.menuItem)));

                    // Is the item available for the current order type
                    item.isEnabled(self.isItemEnabledForMenu(item.menuItem));

                    // Does the item have any toppings?
                    if (item.toppings != undefined)
                    {
                        // Change the toppings in the item
                        for (var toppingIndex = 0; toppingIndex < item.toppings().length; toppingIndex++)
                        {
                            var topping = item.toppings()[itemIndex];

                            var price = self.getToppingPrice(topping);

                            topping.price(helper.formatPrice(price));
                            topping.doublePrice(helper.formatPrice(price * 2));
                        }
                    }
                }
            }
        }
    };
    self.refreshDealsAvailabilty = function ()
    {
        // Change each deal
        for (var dealIndex = 0; dealIndex < viewModel.deals().length; dealIndex++)
        {
            var dealItem = viewModel.deals()[dealIndex];
            dealItem.isEnabled(self.isDealItemEnabledForMenu(dealItem.deal));
        }
    };
    self.calculateItemPrice = function (menuItem, quantity, toppings, addToppingPrices, recalcFreeQuantities)
    {
        var price = self.getItemPrice(menuItem);

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

                self.calculateToppingPrice(topping, toppingPrices);
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
    };
    self.calculateToppingPrice = function (toppingWrapper, prices)
    {
        var price = 0;

        if (toppingWrapper.type == 'removable')
        {
            // The item already comes with this topping (for free) but does the customer want to double up the quantity?
            if (toppingWrapper.selectedDouble() && toppingWrapper.freeQuantity == 0)
            {
                // This topping is already on the item so doubling up just adds a single topping
                price = (self.getToppingPrice(toppingWrapper.topping));

                if (prices != undefined && price > 0)
                {
                    prices.push
                    (
                        {
                            price: self.getToppingPrice(toppingWrapper.topping),
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
                    price = self.getToppingPrice(toppingWrapper.topping);

                    if (prices != undefined && price > 0)
                    {
                        prices.push
                        (
                            {
                                price: self.getToppingPrice(toppingWrapper.topping),
                                toppingWrapper: toppingWrapper
                            }
                        );
                    }
                }
            }
            else if (toppingWrapper.selectedDouble())
            {
                price = (self.getToppingPrice(toppingWrapper.topping) * (2 - toppingWrapper.freeQuantity));

                if (prices != undefined && price > 0)
                {
                    // It's a double - seperate them out as only one might be eligable for a free topping
                    if (toppingWrapper.freeQuantity <= 1)
                    {
                        prices.push
                        (
                            {
                                price: self.getToppingPrice(toppingWrapper.topping),
                                toppingWrapper: toppingWrapper
                            }
                        );
                    }

                    if (toppingWrapper.freeQuantity == 0)
                    {
                        prices.push
                        (
                            {
                                price: self.getToppingPrice(toppingWrapper.topping),
                                toppingWrapper: toppingWrapper
                            }
                        );
                    }
                }
            }
        }

        return price;
    };
    self.calculateDealItemPrice = function (menuItem, toppings, recalcFreeQuantities)
    {
        var price = 0

        // Get the categories
        var category1 = helper.findById(menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1, self.rawMenu.Category1);
        var category2 = helper.findById(menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2, self.rawMenu.Category2);

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

                self.calculateToppingPrice(topping, toppingPrices);
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
    };
    self.calculateDealLinePrice = function (bindableDealLine, excludeDealCalculation)
    {
        var price = 0;

        if (bindableDealLine.selectedMenuItem != undefined)
        {
            // Get the price of the selected item
            var basePrice = self.getItemPrice(bindableDealLine.selectedMenuItem);

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
                        price = self.getDealLinePrice(bindableDealLine);
                        break;
                    case 'Discount':
                        // Get the discount amount
                        var discountAmount = self.getDealLinePrice(bindableDealLine);

                        // Deduct the discount from the item price
                        price = basePrice - discountAmount;

                        // No refunds!
                        if (price < 0) price = 0;

                        break;
                    case 'Percentage':
                        // Get the percentage
                        var percentage = self.getDealLinePrice(bindableDealLine) / 100;

                        // Deduct the percentage from the item price
                        price = basePrice * percentage;

                        break;
                }
            }
        }

        return price;
    };
    self.getItemPrice = function (menuItem)
    {
        if (app.site().cart.orderType() == 'delivery')
        {
            return menuItem.DelPrice == undefined ? menuItem.DeliveryPrice : menuItem.DelPrice == undefined ? menuItem.DeliveryPrice : menuItem.DelPrice;
        }
        else
        {
            return menuItem.ColPrice == undefined ? menuItem.CollectionPrice : menuItem.ColPrice == undefined ? menuItem.CollectionPrice : menuItem.ColPrice;
        }
    };
    self.getDealLinePrice = function (dealLine)
    {
        if (app.site().cart.orderType() == 'delivery')
        {
            return dealLine.dealLine.DelAmount == undefined ? dealLine.dealLine.DeliveryAmount : dealLine.dealLine.DelAmount;
        }
        else
        {
            return dealLine.dealLine.ColAmount == undefined ? dealLine.dealLine.CollectionAmount : dealLine.dealLine.ColAmount;
        }
    };
    self.getToppingPrice = function (topping)
    {
        if (app.site().cart.orderType() == 'delivery')
        {
            return topping.DelPrice == undefined ? topping.DeliveryPrice : topping.DelPrice;
        }
        else
        {
            return topping.ColPrice == undefined ? topping.CollectionPrice : topping.ColPrice;
        }
    };
    self.isItemAvailable = function (menuItem)
    {
        // Is the item available for collection or delivery?
        if ((app.site().cart.orderType() == 'delivery' && menuItem.DelPrice == undefined && menuItem.DeliveryPrice == undefined) ||
            (app.site().cart.orderType() == 'collection' && menuItem.ColPrice == undefined && menuItem.CollectionPrice == undefined))
        {
            // Item is not available for the current order type
            return false;
        }
        else
        {
            return true;
        }
    };
    self.isDealAvailable = function (dealItem)
    {
        // Is the deal available for collection or delivery?
        if ((app.site().cart.orderType() == 'delivery' && dealItem.ForDelivery != true) ||
            (app.site().cart.orderType() == 'collection' && dealItem.ForCollection != true))
        {
            // Deal is not available for the current order type
            return false;
        }
        else
        {
            return true;
        }
    };
    self.areDeals = function ()
    {
        return self.rawMenu == undefined ? false : (self.rawMenu.Deals != undefined && self.rawMenu.Deals.length > 0);
    };
    self.getTemplateName = function (data)
    {
        return data.templateName;
    };
    self.getSelectedMenuItem = function (item)
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

                if (item.selectedCategory1() == undefined || item.selectedCategory1().Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1) &&
                    (item.selectedCategory2() == undefined || item.selectedCategory2().Id == (checkItem.Cat2 == undefined ? checkItem.Category2 : checkItem.Cat2)))
                {
                    return checkItem;
                }
            }
        }
    };
    self.menuItemLookup = undefined,
    self.buildDealsSection = function ()
    {
        // NOTE:  Don't bind to this data structure.  When the user selects a deal the data will be copied into another data structure which we bind to

        // Clear the deals
        viewModel.deals.removeAll();

        if (self.areDeals())
        {
            // Process each deal
            for (var dealIndex = 0; dealIndex < self.rawMenu.Deals.length; dealIndex++)
            {
                // The deal to process
                var deal = self.rawMenu.Deals[dealIndex];
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
                        self.addItemsToDealLine(dealLineWrapper);
                    }
                    else if (dealLine.AllowableItemsIds != undefined && dealLine.AllowableItemsIds.length == 1)
                    {
                        // Add the menu items that can be picked for this deal line
                        self.addItemsToDealLine(dealLineWrapper);

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

                var availableTimes = deal.AvailableTimes[app.site().todayPropertyName];
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
                    isEnabled: ko.observable(self.isDealItemEnabledForMenu(deal)),
                    isAvailableToday: isAvailableToday,
                    dealLineWrappers: dealLineWrappers,
                    minimumOrderValue: deal.MinimumOrderValue > 0 ? helper.formatPrice(deal.MinimumOrderValue) : ''
                };

                viewModel.deals.push(dealWrapper);
            }

            // Add the deals section
            var section = { templateName: 'dealsSection-template', display: { Name: 'Deals' }, deals: viewModel.deals, Index: undefined, Left: ko.observable(0) };
            self.sections.push(section);
        }
    };
    self.addItemsToDealLine = function (dealLineWrapper)
    {
        // We need to build a list of menu items to pick from
        for (var itemIndex = 0; itemIndex < dealLineWrapper.dealLine.AllowableItemsIds.length; itemIndex++)
        {
            var menuId = dealLineWrapper.dealLine.AllowableItemsIds[itemIndex];

            // Lookup the menu item
            var menuItemWrapper = self.menuItemLookup[menuId];

            if (menuItemWrapper != undefined)
            {
                // Get the item name
                var itemName = $.trim(self.rawMenu.ItemNames[menuItemWrapper.menuItem.Name == undefined ? menuItemWrapper.menuItem.ItemName : menuItemWrapper.menuItem.Name]);

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
                        name: self.fixName(itemName),
                        isEnabled: ko.observable(self.isItemEnabledForMenu(menuItemWrapper.menuItem)),
                        displayOrder: menuItemWrapper.menuItem.DispOrder,
                        description: self.fixName(menuItemWrapper.menuItem.Desc == undefined ? menuItemWrapper.menuItem.Description : menuItemWrapper.menuItem.Desc),
                        category1s: ko.observableArray(),
                        category2s: ko.observableArray(),
                        menuItems: ko.observableArray(),
                        selectedCategory1: ko.observable(),
                        selectedCategory2: ko.observable(),
                        price: ko.observable(helper.formatPrice(self.getItemPrice(menuItemWrapper.menuItem))),
                        quantity: 1,
                        selectedMenuItem: undefined
                    };

                    dealLineWrapper.items.push(item);
                }

                // Does this item have a category 1 (e.g. Size)
                var cat1 = menuItemWrapper.menuItem.Cat1 == undefined ? menuItemWrapper.menuItem.Category1 : menuItemWrapper.menuItem.Cat1;
                if (cat1 != undefined && cat1 != -1)
                {
                    var category = helper.findById(cat1, self.rawMenu.Category1);
                    if (category != undefined && !helper.findCategory(category, item.category1s))
                    {
                        item.category1s.push(category);
                    }
                }

                // Does this item have a category 2 (e.g. Size)
                var cat2 = menuItemWrapper.menuItem.Cat2 == undefined ? menuItemWrapper.menuItem.Category2 : menuItemWrapper.menuItem.Cat2;
                if (cat2 != undefined && cat2 != -1)
                {
                    var category = helper.findById(cat2, self.rawMenu.Category2);
                    if (category != undefined && !helper.findCategory(category, item.category2s))
                    {
                        item.category2s.push(category);
                    }
                }

                // Add this menu item to the variations of this particular product
                item.menuItems.push(menuItemWrapper.menuItem);
            }
        }
    };
    self.buildItemLookup = function ()
    {
        // Clear the index
        self.menuItemLookup = {};

        // Process each menu item
        for (var index = 0; index < self.rawMenu.Items.length; index++)
        {
            var menuItem = self.rawMenu.Items[index];

            // Add the menu item to the lookup for later
            self.menuItemLookup[menuItem.Id == undefined ? menuItem.MenuId : menuItem.Id] =
            {
                name: self.rawMenu.ItemNames[menuItem.Name == undefined ? menuItem.ItemName : menuItem.Name],
                menuItem: menuItem
            };
        }
    };
    self.sortItemSections = function ()
    {
        for (var sectionIndex = 0; sectionIndex < self.sections().length; sectionIndex++)
        {
            var section = self.sections()[sectionIndex];
            if (section.items != undefined)
            {
                section.items.sort(self.sortByDisplayOrder);
            }
        }
    };
    self.sortByDisplayOrder = function (a, b)
    {
        return Number(a.displayOrder) > Number(b.displayOrder) ? 1 : -1;
    };
    self.isItemEnabledForMenu = function (menuItem)
    {
        return self.isItemAvailable(menuItem);
    };
    self.isDealItemEnabledForMenu = function (deal)
    {
        return self.isDealAvailable(deal);
    };
    self.isItemEnabledForCart = function (menuItem)
    {
        return self.isItemAvailable(menuItem);
    };
    self.isDealItemEnabledForCart = function (deal)
    {
        return self.isDealAvailable(deal.deal.deal) && !deal.minimumOrderValueNotMet();
    };
    self.getItemToppings = function (menuItem)
    {
        var toppings = [];

        // Removable toppings
        for (var index = 0; index < menuItem.DefTopIds.length; index++)
        {
            var id = menuItem.DefTopIds[index];

            var topping = helper.findByMenuId(id, self.rawMenu.Toppings);
            var price = self.getToppingPrice(topping);

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

            self.addTopping(toppingWrapper, toppings);
        }

        // Additional toppings
        for (var index = 0; index < menuItem.OptTopIds.length; index++)
        {
            var id = menuItem.OptTopIds[index];

            var topping = helper.findByMenuId(id, self.rawMenu.Toppings);
            var price = self.getToppingPrice(topping);

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

            self.addTopping(toppingWrapper, toppings);
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
    };
    self.addTopping = function (topping, toppings)
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
    };
    self.getCartDisplayToppings = function (toppings)
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
                    topping.cartPrice(helper.formatPrice(self.getToppingPrice(topping.topping) * topping.quantity));
                    topping.cartQuantity(topping.quantity);

                    cartDisplayToppings.push(topping);
                }
            }
            else
            {
                if (topping.selectedSingle() || topping.selectedDouble())
                {
                    topping.cartName((topping.selectedDouble() ? text_prefixAddDoubleTopping : text_prefixAddTopping) + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.cartPrice(helper.formatPrice(self.getToppingPrice(topping.topping) * topping.quantity));
                    topping.cartQuantity(topping.quantity);

                    cartDisplayToppings.push(topping);
                }
            }
        }

        return cartDisplayToppings;
    };
    self.getCartItemDisplayName = function (item)
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
    };
    
};