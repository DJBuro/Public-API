// Menu helper functions
function MenuHelper()
{
    var self = this;

    self.menuDataThumbnails = ko.observable(undefined);
    self.menuDataExtended = undefined;
    self.menuItemLookup = undefined;
    self.menuItemWrapperLookup = [];
    self.dealLookup = undefined;
    self.dealSectionCount = 0;
    self.toppingLookup = {};

    self.fixName = function (name)
    {
        if (name == undefined)
        {
            return '';
        }

        return name.replace("&#146;", "'");
    };
    self.refreshDealsAvailabilty = function ()
    {
        // Change each deal
        for (var dealIndex = 0; dealIndex < viewModel.deals().length; dealIndex++)
        {
            var dealItem = viewModel.deals()[dealIndex];
            dealItem.isEnabled(menuHelper.isDealItemEnabledForMenu(dealItem.deal));
            dealItem.isAvailableToday(self.isAvailableToday(dealItem.deal));
        }
    };
    self.isAvailableToday = function (deal)
    {
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

        return isAvailableToday;
    };
    self.calculateItemPrice = function (menuItem, quantity, toppings, addToppingPrices, recalcFreeQuantities)
    {
        var price = menuHelper.getItemPrice(menuItem);

        if (price == undefined) return;

        price = price * quantity;

        return price;
    };
    self.calculateToppingPrice = function (toppingWrapper, prices, removeToppingPrices)
    {
        var toppingPrice = 0;

        if (toppingWrapper.type == 'removable')
        {
            // The item already comes with this topping (for free) but does the customer want to double up the quantity?
            if (!toppingWrapper.selectedSingle() && !toppingWrapper.selectedDouble())
            {
                // This topping is already on the item so doubling up just adds a single topping
                var removeToppingPrice = (menuHelper.getToppingPrice(toppingWrapper.topping));

                if (prices != undefined && removeToppingPrice > 0)
                {
                    removeToppingPrices.push
                    (
                        {
                            price: removeToppingPrice,
                            toppingWrapper: toppingWrapper,
                            isFree: false
                        }
                    );
                }
            }
            else if (toppingWrapper.selectedDouble() && toppingWrapper.freeQuantity == 0)
            {
                // This topping is already on the item so doubling up just adds a single topping
                toppingPrice = (menuHelper.getToppingPrice(toppingWrapper.topping));

                if (prices != undefined && toppingPrice > 0)
                {
                    prices.push
                    (
                        {
                            price: toppingPrice,
                            toppingWrapper: toppingWrapper,
                            isFree: false
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
                    toppingPrice = menuHelper.getToppingPrice(toppingWrapper.topping);

                    if (prices != undefined && toppingPrice > 0)
                    {
                        prices.push
                        (
                            {
                                price: toppingPrice,
                                toppingWrapper: toppingWrapper,
                                isFree: false
                            }
                        );
                    }
                }
            }
            else if (toppingWrapper.selectedDouble())
            {
                toppingPrice = (menuHelper.getToppingPrice(toppingWrapper.topping) * (2 - toppingWrapper.freeQuantity));

                if (prices != undefined && toppingPrice > 0)
                {
                    // It's a double - seperate them out as only one might be eligable for a free topping
                    if (toppingWrapper.freeQuantity <= 1)
                    {
                        prices.push
                        (
                            {
                                price: menuHelper.getToppingPrice(toppingWrapper.topping),
                                toppingWrapper: toppingWrapper,
                                isFree: false
                            }
                        );
                    }

                    if (toppingWrapper.freeQuantity == 0)
                    {
                        prices.push
                        (
                            {
                                price: menuHelper.getToppingPrice(toppingWrapper.topping),
                                toppingWrapper: toppingWrapper,
                                isFree: false
                            }
                        );
                    }
                }
            }
        }

        return toppingPrice;
    };
    self.calculateDealItemAdditionalCosts = function (dealLine, menuItem, toppings, recalcFreeQuantities)
    {
        var price = 0

        // Get the categories
        var category1 = helper.findById(menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1, viewModel.menu.Category1);
        var category2 = helper.findById(menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2, viewModel.menu.Category2);

        // Deal premiums are not applied to percentage deal lines
        if (dealLine.dealLineWrapper.dealLine.Type.toLowerCase() != "percentage")
        {
            // Add category deal premiums
            price += ((category1 == undefined ? 0 : category1.DealPremium) + (category2 == undefined ? 0 : category2.DealPremium));

            // Add item deal premium
            price += menuItem.DealPricePremiumOverride == 0 ? menuItem.DealPricePremium : menuItem.DealPricePremiumOverride;
        }

        // Does the item have any toppings?
        if (toppings != undefined)
        {
            var toppingPrices = [];
            var removeToppingPrices = [];

            // Adjust the price based on the toppings
            for (var index = 0; index < toppings.length; index++)
            {
                var topping = toppings[index];
                if (recalcFreeQuantities) topping.freeQuantity = 0;

                // IMPORTANT - READ THIS
                // "toppingPrices" is a list of SINGLE toppings.  
                // For doubles there will be two items in the list both linked back to the same topping
                // This allows us to deal with double toppings where only one is free
                // Note that there is a maximum number of free toppings per topping!!!
                menuHelper.calculateToppingPrice(topping, toppingPrices, removeToppingPrices);
            }

            // Swaps
            if (settings.allowSwaps)
            {
                var numberOfSwaps = removeToppingPrices.length;

                for (var index = 0; index < (toppingPrices.length < numberOfSwaps ? toppingPrices.length : numberOfSwaps) ; index++)
                {
                    // This topping is free as another topping has been removed
                    toppingPrices[index].isFree = true;
                }
            }

            // Free toppings
            if (menuItem.FreeTops > 0)
            {
                // Are there more toppings than free toppings (i.e. there are some free and some non-free toppings)
                if (toppingPrices.length > menuItem.FreeTops)
                {
                    // Sort the prices low to high
                    toppingPrices.sort(function (a, b) { return a.price - b.price });

                    // Free toppings are free!
                    var freeItemsRemainingCount = menuItem.FreeTops;
                    for (var index = 0; index < toppingPrices.length; index++)
                    {
                        var topping = toppingPrices[index];

                        // Is the topping already free?
                        if (!topping.isFree)
                        {
                            // This topping is free
                            topping.isFree = true;
                            freeItemsRemainingCount = freeItemsRemainingCount - 1;
                        }

                        if (freeItemsRemainingCount <= 0) break;
                    }

                    price += self.calculateToppingPrice(true, toppingPrices, removeToppingPrices);
                }
                else
                {
                    // All toppings are free
                    for (var index = 0; index < toppingPrices.length; index++)
                    {
                        var topping = toppingPrices[index];
                        topping.isFree = true;
                    }
                }
            }
            else
            {
                price += self.calculateToppingPrice(true, toppingPrices, removeToppingPrices);
            }

            // Make sure each topping has the correct number of free toppings
            // We need to do this because we broke down double toppings into multiple single toppings for pricing
            for (var index = 0; index < toppingPrices.length; index++)
            {
                var toppingPrice = toppingPrices[index];
                if (toppingPrice.isFree) toppingPrice.toppingWrapper.freeQuantity++;
            }
        }

        return price;
    };
    self.calculateDealLinePrice = function (cartDealLine, excludeDealCalculation)
    {
        var price = 0;

        if (cartDealLine.cartItem() != undefined && cartDealLine.cartItem().menuItem != undefined)
        {
            // Get the price of the selected item
            var basePrice = menuHelper.getItemPrice(cartDealLine.cartItem().menuItem);

            if (excludeDealCalculation != undefined && excludeDealCalculation == true)
            {
                price = basePrice;
            }
            else
            {
                switch (cartDealLine.dealLineWrapper.dealLine.Type.toLowerCase())
                {
                    case 'fixed':
                        // Get the fixed price
                        price = menuHelper.getDealLinePrice(cartDealLine);
                        break;
                    case 'discount':
                        // Get the discount amount
                        var discountAmount = menuHelper.getDealLinePrice(cartDealLine);

                        // Deduct the discount from the item price
                        price = basePrice - discountAmount;

                        // No refunds!
                        if (price < 0) price = 0;

                        break;
                    case 'percentage':
                        // !!! IMPORTANT !!! This is the percentage of the price that is included NOT the a percentage discount
                        // So 100% means 100% of the price is used NOT a 10% discount

                        // Get the percentage to use
                        var percentageofPriceToUse = menuHelper.getDealLinePrice(cartDealLine) / 100;

                        // Since this is the percentage of the price to use we need to inverse the percentage
                        percentageofPriceToUse = percentageofPriceToUse;

                        // Deduct the percentage from the item price
                        price = Math.round(basePrice * percentageofPriceToUse);

                        break;
                }
            }
        }

        return price;
    };
    self.getItemPrice = function (menuItem)
    {
        if (viewModel.orderType().toLowerCase() == 'delivery')
        {
            return menuItem.DelPrice == undefined ? menuItem.DeliveryPrice : menuItem.DelPrice == undefined ? menuItem.DeliveryPrice : menuItem.DelPrice;
        }
        else if (viewModel.orderType().toLowerCase() == 'collection')
        {
            return menuItem.ColPrice == undefined ? menuItem.CollectionPrice : menuItem.ColPrice == undefined ? menuItem.CollectionPrice : menuItem.ColPrice;
        }
        else if (viewModel.orderType().toLowerCase() == 'dinein')
        {
            return menuItem.DineInPrice;
        }
    };
    self.getDealLinePrice = function (cartDealLine)
    {
        if (viewModel.orderType().toLowerCase() == 'delivery')
        {
            return cartDealLine.dealLineWrapper.dealLine.DelAmount == undefined ? cartDealLine.dealLineWrapper.dealLine.DeliveryAmount : cartDealLine.dealLineWrapper.dealLine.DelAmount;
        }
        else
        {
            return cartDealLine.dealLineWrapper.dealLine.ColAmount == undefined ? cartDealLine.dealLineWrapper.dealLine.CollectionAmount : cartDealLine.dealLineWrapper.dealLine.ColAmount;
        }
    };
    self.getToppingPrice = function (topping)
    {
        if (viewModel.orderType().toLowerCase() == 'delivery')
        {
            return topping.DelPrice == undefined ? topping.DeliveryPrice : topping.DelPrice;
        }
        else if (viewModel.orderType().toLowerCase() == 'collection')
        {
            return topping.ColPrice == undefined ? topping.CollectionPrice : topping.ColPrice;
        }
        else if (viewModel.orderType().toLowerCase() == 'dinein')
        {
            return topping.DineInPrice;
        }
    };
    self.isItemAvailable = function (menuItemWrapper)
    {
        // Is it a known menu item?
        if (menuItemWrapper.menuItem.Id == -1)
        {
            menuItemWrapper.isEnabled(false);
            return menuItemWrapper.isEnabled();
        }

        // If the menu item is temporarily disabled then it cannot be enabled
        if (menuItemWrapper.isTemporarilyDisabled)
        {
            menuItemWrapper.isEnabled(false);
            return menuItemWrapper.isEnabled();
        }
        
        // Is the item available for collection or delivery?
        if (viewModel.orderType().toLowerCase() == 'delivery' &&
            ((menuItemWrapper.menuItem.DelPrice == undefined && menuItemWrapper.menuItem.DeliveryPrice == undefined) ||
            menuItemWrapper.menuItem.DelPrice == -1 || menuItemWrapper.menuItem.DeliveryPrice == -1))
        {
            // Item is not available for the current order type
            menuItemWrapper.isEnabled(false);
            menuItemWrapper.notAvailableText(textStrings.miNotAvailableForDelivery);
        }
        else if (viewModel.orderType().toLowerCase() == 'collection' &&
            ((menuItemWrapper.menuItem.ColPrice == undefined && menuItemWrapper.menuItem.CollectionPrice == undefined) ||
            menuItemWrapper.menuItem.ColPrice == -1 || menuItemWrapper.menuItem.CollectionPrice == -1))
        {
            // Item is not available for the current order type
            menuItemWrapper.isEnabled(false);
            menuItemWrapper.notAvailableText(textStrings.miNotAvailableForCollection);
        }
        else if (viewModel.orderType().toLowerCase() == 'dinein' &&
            ((menuItemWrapper.menuItem.DineInPrice == undefined) || menuItemWrapper.menuItem.DineInPrice == -1))
        {
            // Item is not available for the current order type
            menuItemWrapper.isEnabled(false);
            menuItemWrapper.notAvailableText(textStrings.miNotAvailableForDineIn);
        }
        else
        {
            menuItemWrapper.isEnabled(true);
            menuItemWrapper.notAvailableText('');
        }

        return menuItemWrapper.isEnabled();
    };
    self.isDealAvailable = function (dealItem)
    {
        // Is the deal available for collection or delivery?
        if ((viewModel.orderType() == 'delivery' && dealItem.ForDelivery != true) ||
            (viewModel.orderType() == 'collection' && dealItem.ForCollection != true) ||
            (viewModel.orderType() == 'dinein' && dealItem.ForDineIn != true))
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
        return viewModel.menu == undefined ? false : (viewModel.menu.Deals != undefined && viewModel.menu.Deals.length > 0);
    };
    self.getTemplateName = function (data)
    {
        return data.templateName;
    };
    self.getSelectedMenuItem = function (item)
    {
        var varients = item.menuItems();
        // Find the correct item
        if (varients.length == 1)
        {
            // There is only one item
            return varients[0];
        }
        else
        {
            var itemCats = {
                cat1: item.selectedCategory1(),
                cat2: item.selectedCategory2(),
            };

            // Get the menu item for the selected category1 and category2
            var validVarients = varients.filter(function (varient)
            {
                var matchCat1 = false, matchCat2 = false;

                //fix for pronta pizza ... odd tiny pizza with no variants
                matchCat1 = (itemCats.cat1 === undefined && varient.Cat1 === undefined) || (itemCats.cat1 && itemCats.cat1.Id === varient.Cat1);
                matchCat2 = (itemCats.cat2 === undefined && varient.Cat2 === undefined) || (itemCats.cat2 && itemCats.cat2.Id === varient.Cat2);

                return matchCat1 && matchCat2;
            });

            if (validVarients.length === 0) { return; }

            return validVarients[0];

        }
    };
    self.buildDealsSection = function ()
    {
        // NOTE:  Don't bind to this data structure.  When the user selects a deal the data will be copied into another data structure which we bind to
        var start = new Date();
        var startpush = undefined;
        var lineTimes = [];
        var dealTimes = [];

        // Clear the deals
        viewModel.deals.removeAll();

        if (menuHelper.areDeals())
        {
            var dealSections = [];

            // Process each deal
            for (var dealIndex = 0; dealIndex < viewModel.menu.Deals.length; dealIndex++)
            {
                var dealTime = { start: new Date(), end: undefined, linesEnd: undefined };

                // The deal to process
                var deal = viewModel.menu.Deals[dealIndex];
                var dealLineWrappers = [];

                // Process the deal lines
                for (var dealLineIndex = 0; dealLineIndex < deal.DealLines.length; dealLineIndex++)
                {
                    var lineTime = { start: new Date(), end: undefined };

                    // The deal line
                    var dealLine = deal.DealLines[dealLineIndex];

                    // Wrap the deal line in another object to help us binding
                    var dealLineWrapper =
                    {
                        itemNo: dealLineIndex + 1,
                        dealLine: dealLine,
                        templateName: '',
                        menuItemWrappers: [],
                        menuItemWrapperLookup: {},
                        id: 'dl_' + dealLineIndex + 1
                    };

                    // Does the customer need to pick from a list of allowable menu items for this deal line?
                    if (dealLine.AllowableItemsIds != undefined && dealLine.AllowableItemsIds.length > 1)
                    {
                        // We'll need a drop down combo to display the allowable menu items
                        dealLineWrapper.templateName = 'popupDealLinePicker-template';

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

                    lineTime.end = new Date();
                    lineTimes.push(lineTime);
                }

                dealTime.linesEnd = new Date();

                var dealWrapper = menuHelper.dealLookup[deal.Id];
                dealWrapper.templateName = 'deal-template';
                dealWrapper.deal = deal;
                dealWrapper.isEnabled(menuHelper.isDealItemEnabledForMenu(deal));
                dealWrapper.isAvailableToday(menuHelper.isAvailableToday(deal));
                dealWrapper.dealLineWrappers = dealLineWrappers;
                dealWrapper.minimumOrderValue = deal.MinimumOrderValue > 0 ? helper.formatPrice(deal.MinimumOrderValue) : '';
                dealWrapper.isNotAvailableForRestOfDay(false);
                dealWrapper.availabilityText('');
                dealWrapper.removedTimeSlot(false);

                viewModel.deals().push(dealWrapper);

                // Does the deal have any extensions?
                var sectionName = undefined;
                if (dealWrapper.extension != undefined)
                {
                    // Do we need to move the deal to another section?
                    if (dealWrapper.extension.moveToSection != undefined)
                    {
                        sectionName = dealWrapper.extension.moveToSection;
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

                // Do we need to move the deal to a menu section?
                if (dealWrapper.deal.Display !== undefined && dealWrapper.deal.Display != 20)
                {
                    self.addDealToMenuSection(dealWrapper);
                }
                else
                {
                    // Add the deal to the section
                    dealSection.deals.push(dealWrapper);
                }

                dealTime.end = new Date();
                dealTimes.push(dealTime);

                // Check to see if the item is available today
                if (dealLineWrapper.availableTimes === undefined)
                {
                    if (deal.AvailableTimes !== undefined)
                    {
                        var day = deal.AvailableTimes[openingTimesHelper.todayPropertyName];

                        if (day !== undefined && day !== null)
                        {
                            dealWrapper.availableTimes = self.getAvailableTimes(day);
                            dealWrapper.isNotAvailableForRestOfDay(self.isNotAvailableForRestOfDay(dealWrapper.availableTimes, day));

                            if ((dealWrapper.availableTimes.displayText === undefined ||
                                dealWrapper.availableTimes.displayText.length == 0))
                            {
                                dealWrapper.availabilityText = textStrings.miNotAvailableAllDay;
                            }
                            else
                            {
                                dealWrapper.availabilityText = textStrings.miNotAvailableForHours.replace('{hours}', dealWrapper.availableTimes.displayText);
                            }
                        }
                    }
                }
            }

            startpush = new Date();

            var dealDisplayOrder = settings.dealSectionDisplayOrder == null ? 0 : settings.dealSectionDisplayOrder;

            for (var dealSectionIndex = 0; dealSectionIndex < dealSections.length; dealSectionIndex++)
            {
                var dealSection = dealSections[dealSectionIndex];

                var section = { templateName: 'dealsSection-template', display: { Name: dealSection.name, displayOrder: dealDisplayOrder++ }, deals: dealSection.deals, Index: viewModel.sections().length + 1, Left: ko.observable(0) };
                viewModel.sections().push(section);
            }

            menuHelper.dealSectionCount = dealSections.length;
        }
    };
    self.addDealToMenuSection = function (dealWrapper)
    {
        var display = helper.findById(dealWrapper.deal.Display, viewModel.menu.Display);

        var section = undefined;
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var checkSection = viewModel.sections()[sectionIndex];

            if (checkSection.display.displayId == dealWrapper.deal.Display)
            {
                section = checkSection;
                break;
            }
        }

        if (section !== undefined)
        {
            if (display == undefined) { display = { Name: '?' }; }

            section.items.push(dealWrapper);
        }
    };
    self.addItemsToDealLine = function (dealLineWrapper)
    {
        // We need to build a list of menu items to pick from
        for (var itemIndex = 0; itemIndex < dealLineWrapper.dealLine.AllowableItemsIds.length; itemIndex++)
        {
            var menuId = dealLineWrapper.dealLine.AllowableItemsIds[itemIndex];

            // Lookup the menu item
            var menuItemDetails = menuHelper.menuItemLookup[menuId];

            if (menuItemDetails != undefined)
            {
                // Get the item name
                var itemName = viewModel.menu.ItemNames[menuItemDetails.menuItem.Name == undefined ? menuItemDetails.menuItem.ItemName : menuItemDetails.menuItem.Name];

                // Is the item disabled?
                var lookupMenuItemWrapper = menuHelper.menuItemWrapperLookup[itemName];
                if (lookupMenuItemWrapper !== undefined && !lookupMenuItemWrapper.isEnabled()) continue; // Skip this item - it's disabled

                // Is the item already in the deal line (the same item can be in a deal line multiple times e.g. different sizes of the same item)
                // but we only want it to appear once
                var menuItemWrapper = dealLineWrapper.menuItemWrapperLookup[itemName];

                if (menuItemWrapper == undefined)
                {
                    // Add the menu item wrapper to the deal line
                    menuItemWrapper = new MenuItemWrapper(menuItemDetails.menuItem, itemName);

                    dealLineWrapper.menuItemWrappers.push(menuItemWrapper);
                    dealLineWrapper.menuItemWrapperLookup[itemName] = menuItemWrapper;
                }
                
                // Does this item have a category 1 (e.g. Size)
                var cat1 = menuItemDetails.menuItem.Cat1 == undefined ? menuItemDetails.menuItem.Category1 : menuItemDetails.menuItem.Cat1;
                if (cat1 != undefined && cat1 != undefined)
                {
                    var category = helper.findById(cat1, viewModel.menu.Category1);
                    if (category != undefined && !helper.findCategory(category, menuItemWrapper.category1s))
                    {
                        menuItemWrapper.category1s.push(category);
                    }
                }

                // Does this item have a category 2 (e.g. Size)
                var cat2 = menuItemDetails.menuItem.Cat2 == undefined ? menuItemDetails.menuItem.Category2 : menuItemDetails.menuItem.Cat2;
                if (cat2 != undefined && cat2 != undefined)
                {
                    var category = helper.findById(cat2, viewModel.menu.Category2);
                    if (category != undefined && !helper.findCategory(category, menuItemWrapper.category2s))
                    {
                        menuItemWrapper.category2s.push(category);
                    }
                }

                // Add this menu item to the variations of this particular product
                menuItemWrapper.menuItems.push(menuItemDetails.menuItem);
            }
        }

        // Sort the item categories
        for (var index = 0; index < dealLineWrapper.menuItemWrappers.length; index++)
        {
            var menuItemWrapper = dealLineWrapper.menuItemWrappers[index];

            if (menuItemWrapper.category1s !== undefined)
            {
                var options = menuItemWrapper.menuItems();

                menuItemWrapper.category1s().sort
                (
                    function (a, b)
                    {
                        options || (options = []);

                        var menuItemA = options.filter
                        (
                            function (item)
                            {
                                return item.Cat1 === a.Id;
                            }
                        ).sort
                        (
                            function (a, b)
                            {
                                if (a.DelPrice === b.DelPrice)
                                {
                                    return 0;
                                }
                                return a.DelPrice < b.DelPrice ? -1 : 1;
                            }
                        );

                        var menuItemB = options.filter
                        (
                            function (item)
                            {
                                return item.Cat1 === b.Id;
                            }
                        ).sort
                        (
                            function (a, b)
                            {
                                if (a.DelPrice === b.DelPrice)
                                {
                                    return 0;
                                }
                                return a.DelPrice < b.DelPrice ? -1 : 1;
                            }
                        );

                        if (menuItemA.length > 0 && menuItemB.length > 0)
                        {
                            if (menuItemA[0].DelPrice === menuItemB[0].DelPrice)
                            {
                                return 0;
                            }

                            return menuItemA[0].DelPrice < menuItemB[0].DelPrice ? -1 : 1;
                        } 
                    }
                );

                if (menuItemWrapper.category1s().length > 0)
                {
                    var selected = menuItemWrapper.category1s()[0];

                    menuItemWrapper.selectedCategory1(selected);
                }

                menuItemWrapper.category2s().sort(menuHelper.sortBySortOrder);

                if (menuItemWrapper.category2s().length > 0)
                {
                    menuItemWrapper.selectedCategory2(menuItemWrapper.category2s()[0]);
                }

                // Get the menu item for the selected category1 and category2
                for (var menuItemIndex = 0; menuItemIndex < menuItemWrapper.menuItems().length; menuItemIndex++)
                {
                    var checkItem = menuItemWrapper.menuItems()[menuItemIndex];

                    if ((menuItemWrapper.selectedCategory1() == undefined || menuItemWrapper.selectedCategory1().Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1)) &&
                        (menuItemWrapper.selectedCategory2() == undefined || menuItemWrapper.selectedCategory2().Id == (checkItem.Cat2 == undefined ? checkItem.Category2 : checkItem.Cat2)))
                    {

                        menuItemWrapper.menuItem = checkItem;
                        break;
                    }
                }
            }
        }

    };
    self.buildItemLookup = function ()
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
                extension: undefined,
                isEnabled: true
            };
        }
    };
    self.buildDealLookup = function ()
    {
        // Clear the index
        menuHelper.dealLookup = {};

        // Process each deal
        for (var dealIndex = 0; dealIndex < viewModel.menu.Deals.length; dealIndex++)
        {
            var deal = viewModel.menu.Deals[dealIndex];

            // Add the deal to the lookup for later
            var dealIndexItem = new AndroWeb.Models.DealWrapper();
            dealIndexItem.deal = deal;

            menuHelper.dealLookup[deal.Id] = dealIndexItem;
        }
    };
    self.buildToppingLookup = function ()
    {
        // Clear the index
        menuHelper.toppingLookup = {};

        // Process each menu item
        for (var index = 0; index < viewModel.menu.Toppings.length; index++)
        {
            var topping = viewModel.menu.Toppings[index];

            // Add the menu item to the lookup for later
            menuHelper.toppingLookup[topping.Id == undefined ? topping.MenuId : topping.Id] = topping;
        }
    };
    self.lookupItemImages = function ()
    {
        if (menuHelper.menuDataThumbnails() != undefined)
        {
            if (menuHelper.menuDataThumbnails().MenuItemThumbnails != undefined)
            {
                // Process each menu image
                for (var index = 0; index < menuHelper.menuDataThumbnails().MenuItemThumbnails.length; index++)
                {
                    var image = menuHelper.menuDataThumbnails().MenuItemThumbnails[index];

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
    };
    self.lookupItemExtensions = function ()
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
    };
    self.lookupDisabledMenuItems = function ()
    {
        if (menuHelper.menuDataThumbnails() != undefined)
        {
            if (menuHelper.menuDataThumbnails().DisabledItems != undefined)
            {
                // Process each disabled menu item
                for (var index = 0; index < menuHelper.menuDataThumbnails().DisabledItems.length; index++)
                {
                    var disabledMenuItem = menuHelper.menuDataThumbnails().DisabledItems[index];

                    // Lookup the menu item
                    var menuItem = menuHelper.menuItemLookup[disabledMenuItem.Id];

                    if (menuItem !== undefined)
                    {
                        // The menu item is disabled
                        menuItem.isEnabled = false;
                    }
                }
            }
        }
    };
    self.lookupDealExtensions = function ()
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
    };
    self.fixNames = function ()
    {
        // Fix menu item names
        for (var menuItemNamesIndex = 0; menuItemNamesIndex < viewModel.menu.ItemNames.length; menuItemNamesIndex++)
        {
            var fixedName = menuHelper.fixName($.trim(viewModel.menu.ItemNames[menuItemNamesIndex]));
            viewModel.menu.ItemNames[menuItemNamesIndex] = fixedName;
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

    };
    self.buildItemsSections = function (callback)
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
    };
    self.commitSection = function (index, sections, callback)
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
            viewModel.sections().push(section);

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
    };
    self.getMenuSection = function (findDisplayId)
    {
        for (var displayIndex = 0; displayIndex < viewModel.menu.Display.length; displayIndex++)
        {
            var display = viewModel.menu.Display[displayIndex];
            if (display.Id == findDisplayId)
            {
                return display.Name;
            }
        }

        return '';
    };
    self.getMenuSectionIndex = function (findDisplayName)
    {
        for (var displayIndex = 0; displayIndex < viewModel.sections().length; displayIndex++)
        {
            var section = viewModel.sections()[displayIndex];
            if (section.display.Name.toUpperCase() == findDisplayName.toUpperCase())
            {
                return displayIndex;
            }
        }

        return '';
    };
    self.addItemToSection = function (sections, menuItem)
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

        if (display == undefined) { display = { Name: '?' }; }

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
        if (menuItem.Cat1 == -1) menuItem.Cat1 = undefined;
        if (menuItem.Category1 == -1) menuItem.Category1 = undefined;

        var cat1 = menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1;
        if (cat1 != undefined && cat1 != -1)
        {
            itemCat1 = helper.findById(cat1, viewModel.menu.Category1);

            if (itemCat1 != undefined && settings.invertItems)
            {
                itemName = menuHelper.fixName($.trim(itemCat1.Name));
            }

            if (itemCat1.SortOrder === undefined || menuItem.DelPrice < itemCat1.SortOrder)
            {
                itemCat1.SortOrder = menuItem.DelPrice;
            }
        }

        var menuItemWrapper = undefined;

        // Is the item already in the section (the same item can be in a section multiple times e.g. different sizes of the same item)
        for (var itemIndex = 0; itemIndex < section.items().length; itemIndex++)
        {
            var checkItem = section.items()[itemIndex];

            if (checkItem.name == itemName)
            {
                menuItemWrapper = checkItem;
                break;
            }
        }

        if (menuItemWrapper == undefined)
        {
            // Add the item to the section
            menuItemWrapper = new MenuItemWrapper(menuItem, itemName);

            // We may need to get a menu item wrapper later
            menuHelper.menuItemWrapperLookup[menuItemWrapper.name] = menuItemWrapper;

            section.items.push(menuItemWrapper);
        }

        if (settings.showMenuIds)
        {
            menuItemWrapper.description += ' ' + menuItem.Id;
        }

        if (itemCat1 != undefined && settings.invertItems)
        {
            itemCat1 =
            {
                Id: itemCat1.Id,
                Name: originalItemName,
                AddHAndHCat: itemCat1.AddHAndHCat,
                DealPremium: itemCat1.DealPremium,
                Parent: itemCat1.Parent,
                SortOrder: menuItemWrapper.price
            };
        }

        // Does this item have a category 1 (e.g. Size)
        if (itemCat1 != undefined && !helper.findCategory(itemCat1, menuItemWrapper.category1s))
        {
            menuItemWrapper.category1s.push(itemCat1);
        }

        // Does this item have a category 2 (e.g. Size)
        if (menuItem.Cat2 == -1) menuItem.Cat2 = undefined;
        if (menuItem.Category2 == -1) menuItem.Category2 = undefined;

        // Add this menu item to the variations of this particular product
        menuItemWrapper.menuItems.push(menuItem);

        // Check to see if there is a thumbnail
        if (menuItemWrapper.thumbnail == undefined || menuItemWrapper.image == undefined)
        {
            var lookupItem = menuHelper.menuItemLookup[menuItem.MenuId == undefined ? menuItem.Id : menuItem.MenuId];

            menuItemWrapper.thumbnail = lookupItem.thumbnail;
            menuItemWrapper.thumbnailWidth = lookupItem.thumbnailWidth;
            menuItemWrapper.thumbnailHeight = lookupItem.thumbnailHeight;
            menuItemWrapper.image = lookupItem.image;

            // Disable the entire menu item wrapper if any one of the menu items is disabled
            menuItemWrapper.isTemporarilyDisabled = (menuItemWrapper.isTemporarilyDisabled === true ? true : !lookupItem.isEnabled);
        }

        // Check to see if the item is available today
        if (menuItemWrapper.availableTimes === undefined)
        {
            if (menuItem.Times !== undefined)
            {
                var day = menuItem.Times[openingTimesHelper.todayPropertyName];

                if (day !== undefined && day !== null)
                {
                    menuItemWrapper.availableTimes = self.getAvailableTimes(day);
                    menuItemWrapper.isNotAvailableForRestOfDay(self.isNotAvailableForRestOfDay(menuItemWrapper.availableTimes, day));

                    if ((menuItemWrapper.availableTimes.displayText === undefined ||
                        menuItemWrapper.availableTimes.displayText.length == 0))
                    {
                        menuItemWrapper.availabilityText = textStrings.miNotAvailableAllDay;
                    }
                    else
                    {
                        menuItemWrapper.availabilityText = textStrings.miNotAvailableForHours.replace('{hours}', menuItemWrapper.availableTimes.displayText);
                    }
                }
            }
        }

        // Check to see if there is an overlay image
        if (menuItemWrapper.overlayImage == undefined || menuItemWrapper.overlayImage == undefined)
        {
            var lookupItem = menuHelper.menuItemLookup[menuItem.MenuId == undefined ? menuItem.Id : menuItem.MenuId];

            menuItemWrapper.overlayImage = lookupItem.overlayImage;
        }
    };
    self.getAvailableTimes = function (times)
    {
        var availableTimes =
        {
            times: [],
            displayText: '',
            notAvailableToday: false,
            availableAllDay: true
        };

        // Is the item not available today?
        if (times.length === 1)
        {
            var time = times[0];

            if (time.NotAvailableToday !== undefined && time.NotAvailableToday === true)
            {
                availableTimes.notAvailableToday = true;
                availableTimes.availableAllDay = false;
                availableTimes.displayText = ""
            }
        }

        if (!availableTimes.notAvailableToday && times.length > 0)
        {
            // Item is available today - figure out what times
            availableTimes.availableAllDay = false;

            var startsAtMidnightIndex = -1;
            var endsAtMidnightIndex = -1;

            for (var timesIndex = 0; timesIndex < times.length; timesIndex++)
            {
                var time = times[timesIndex];

                if (time.From == "00:00") startsAtMidnightIndex = timesIndex;
                if (time.To == "00:00") endsAtMidnightIndex = timesIndex;

                availableTimes.times.push
                (
                    { from: time.From, to: time.To }
                );
            }

            if (startsAtMidnightIndex != -1 && endsAtMidnightIndex != -1)
            {
                // We need to merge the two times
                var fromTime = times[startsAtMidnightIndex];
                var toTime = times[endsAtMidnightIndex];

                // Remove the two times - we must do this from the end of the list first or it won't work
                availableTimes.times.splice(startsAtMidnightIndex > endsAtMidnightIndex ? startsAtMidnightIndex : endsAtMidnightIndex, 1);
                availableTimes.times.splice(startsAtMidnightIndex > endsAtMidnightIndex ? endsAtMidnightIndex : startsAtMidnightIndex, 1);

                // Add the merged time
                availableTimes.times.push
                (
                    {
                        from: toTime.From,
                        to: fromTime.To
                    }
                )
            }
        }

        // Sort the available times
        availableTimes.times.sort
        (
            function (a, b)
            {
                var a1 = self.getAvailableTime(a.from);
                if (a1.getHours() == 0)
                {
                    a1.setDate(a1.getDate() + 1);
                }
                var b1 = self.getAvailableTime(b.from);
                if (b1.getHours() == 0)
                {
                    b1.setDate(b1.getDate() + 1);
                }
                return a1 - b1;
            }
        );

        for (var availableTimesIndex = 0; availableTimesIndex < availableTimes.times.length; availableTimesIndex++)
        {
            var availableTime = availableTimes.times[availableTimesIndex];

            if (availableTime.from != undefined && availableTime.to != undefined)
            {
                availableTimes.displayText += (availableTimes.displayText.length > 0 ? ', ' : '');
                availableTimes.displayText +=
                    menuHelper.convert24to12Hour(availableTime.from) +
                    ' to ' +
                    menuHelper.convert24to12Hour(availableTime.to);
            }
        }

        return availableTimes;
    },
    self.convert24to12Hour = function (time)
    {
        var result = '';
        var timeText = time.split(':');
        var hour = Number(timeText[0]);
        var minuteChunks = timeText[1].split(' ');
        var minutes = Number(minuteChunks[0]);


        // Was a 12 hour time actually passed in?
        if (minuteChunks.length > 1)
        {
            // Switch it back to 24hour so we can format the time properly - it's a bit of a hack but such is life
            if (minuteChunks[1] === 'PM')
            {
                hour = hour + 12;
            }
        }

        var minutesText = minutes < 10 ? '0' + minutes.toString() : minutes.toString();

        if (hour > 12)
        {
            hour -= 12;
            result = hour.toString() + ":" + minutesText + " p.m.";
        }
        else
        {
            result = hour.toString() + ":" + minutesText + " a.m.";
        }

        return result;
    },
    self.isNotAvailableForRestOfDay = function (availableTimes, day)
    {
        var dateNow = new Date();
        // var timeNow = dateNow.getTime();

        // Adjust for trading day
        dateNow.setHours(dateNow.getHours() - 6);

        var timeNow = dateNow.getTime();

        if (availableTimes.notAvailableToday) return true;
        if (availableTimes.availableAllDay) return false;

        for (var timeIndex = 0; timeIndex < day.length; timeIndex++)
        {
            // Get the start and end times
            var time = day[timeIndex];
            var startTime = self.getAvailableTime(time.From, true);
            var endTime = self.getAvailableTime(time.To, true);

            // Is the item available now?
            if (timeNow < startTime.getTime() || (timeNow > startTime.getTime() && timeNow < endTime.getTime()))
            {
                // It's available now or at some point later today
                return false;
            }
        }

        return true;
    },
    self.getAvailableTime = function (time, adjustForTradingDay)
    {
        // "09:25 AM"
        var dividerIndex = time.indexOf(':');
        var hour = Number(time.substring(0, dividerIndex)); // XX:25 AM
        var minute = Number(time.substring(dividerIndex + 1, dividerIndex + 3)); //  09:XX AM
        var amPm = time.substring(dividerIndex + 4, dividerIndex + 6); // 09:25 XX

        // Convert to 24 hour clock
        hour = (amPm === 'PM' ? (hour + 12) : hour);

        if (hour == 12 && amPm === 'AM') hour = 0;

        if (adjustForTradingDay === true) hour = self.adjustForTradingDay(hour);

        var availableTime = new Date();
        availableTime.setHours(hour, minute, 0, 0);

        return availableTime;
    },
    self.adjustForTradingDay = function (hour)
    {
        // Minus 6 hours because a trading day is 6am to 6am
        return hour <= 6 ? 18 + hour : hour - 6;
    },
    self.rebuildCategory2List = function (itemWrapper, selectedCategory1, previousSelectedCategory2)
    {
        var result =
            {
                category2s: [],
                selectedCategory2: undefined
            }

        if (selectedCategory1 === undefined) return result;

        // Rebuild the category2 list based on the selected category1
        for (var index = 0; index < itemWrapper.menuItems().length ; index++)
        {
            var checkItem = itemWrapper.menuItems()[index];

            // Does the menu item have the selected category1?
            if (selectedCategory1.Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1))
            {
                // Is this items category 2 already in the list?
                var cetegory2Id = checkItem.Cat2 == undefined
                    ? checkItem.Category2
                    : checkItem.Cat2;

                var category = helper.findById(cetegory2Id, viewModel.menu.Category2);
                if (category != undefined && !helper.findCategory(category, result.category2s))
                {
                    // No, it's not already in the list
                    result.category2s.push(category);
                }
            }
        }

        // If possible select the same category 2 that was previously selected using the category name
        var set = false;
        if (previousSelectedCategory2 !== undefined && result.category2s.length > 0)
        {
            for (var index = 0; index < result.category2s.length; index++)
            {
                var checkCategory = result.category2s[index];

                if (checkCategory.Name === previousSelectedCategory2.Name)
                {
                    result.selectedCategory2 = checkCategory;
                    set = true;
                    break;
                }
            }
        }

        // Did we set the category 2?
        if (!set)
        {
            if (result.category2s.length > 0)
            {
                // Default to the first category 2
                result.selectedCategory2 = result.category2s[0];
            }
        }

        return result;
    };
    self.sortItemSections = function ()
    {
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];
            if (section.items != undefined)
            {
                // Sort the inner array so we don't trigger binding notifications
                section.items().sort(menuHelper.sortByDisplayOrder);

                for (var index = 0; section.items()[index]; index++)
                {
                    var menuItem = section.items()[index];

                    menuItem.category1s().sort(menuHelper.sortBySortOrder);

                    if (menuItem.category1s().length > 0)
                    {
                        menuItem.selectedCategory1(menuItem.category1s()[0]);
                        viewModel.category1Changed(menuItem)
                    }

                    menuItem.category2s().sort(menuHelper.sortBySortOrder);
                }
            }
        }
    };
    self.sortByDisplayOrder = function (a, b)
    {
        return Number(a.displayOrder) > Number(b.displayOrder) ? 1 : -1;
    };
    self.sortBySortOrder = function (a, b)
    {
        return Number(a.SortOrder) > Number(b.SortOrder) ? 1 : -1;
    };
    self.isItemEnabledForMenu = function (menuItem)
    {
        return menuHelper.isItemAvailable(menuItem);
    };
    self.isDealItemEnabledForMenu = function (deal)
    {
        return menuHelper.isDealAvailable(deal);
    };
    self.isItemEnabledForCart = function (menuItemWrapper)
    {
        return menuHelper.isItemAvailable(menuItemWrapper);
    };
    self.isDealItemEnabledForCart = function (cartDeal)
    {
        return menuHelper.isDealAvailable(cartDeal.dealWrapper.deal) && !cartDeal.minimumOrderValueNotMet();
    };
    self.getCartDisplayToppings = function (toppings)
    {
        var cartDisplayToppings = [];

        if (toppings === undefined) return cartDisplayToppings;

        for (var index = 0; index < toppings.length; index++)
        {
            var topping = toppings[index];

            topping.itemPrice = menuHelper.getToppingPrice(topping.topping);

            if (topping.type == 'removable')
            {
                if (!topping.selectedSingle() && !topping.selectedDouble())
                {
                    topping.cartName(textStrings.prefixRemove + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.finalPrice = 0;
                    topping.cartPrice(helper.formatPrice(topping.finalPrice));
                    topping.cartQuantity('-1');

                    cartDisplayToppings.push(topping);
                }
                else if (topping.selectedDouble())
                {
                    topping.cartName(textStrings.prefixAddTopping + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.finalPrice = topping.itemPrice * topping.quantity();
                    topping.cartPrice(helper.formatPrice(topping.finalPrice));
                    topping.cartQuantity(topping.quantity());

                    cartDisplayToppings.push(topping);
                }
            }
            else
            {
                if (topping.selectedSingle() || topping.selectedDouble())
                {
                    topping.cartName((topping.selectedDouble() ? textStrings.prefixAddDoubleTopping : textStrings.prefixAddTopping) + (topping.topping.Name == undefined ? topping.topping.ToppingName : topping.topping.Name));
                    topping.finalPrice = topping.itemPrice * topping.quantity();
                    topping.cartPrice(helper.formatPrice(topping.finalPrice));
                    topping.cartQuantity(topping.quantity());

                    cartDisplayToppings.push(topping);
                }
            }
        }

        return cartDisplayToppings;
    };
    self.getCartItemDisplayName = function (item)
    {
        var displayName = '';

        var cat1 = typeof (item.selectedCategory1) == 'function' ? item.selectedCategory1() : item.selectedCategory1;
        var cat2 = typeof (item.selectedCategory2) == 'function' ? item.selectedCategory2() : item.selectedCategory2;

        if (cat1 != undefined)
        {
            displayName += cat1.Name;
        }
        if (cat2 != undefined)
        {
            if (displayName.length > 0) { displayName += " "; }

            displayName += cat2.Name;
        }
        if (displayName.length > 0) { displayName += " "; }
        displayName += typeof (item.name) == 'string' ? item.name : item.name()

        return displayName;
    };

    self.clearSelectables = function ()
    {
        // Reset menu item quantites to 1
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var items = viewModel.sections()[0].items();
            if (items != undefined)
            {
                for (var itemIndex = 0; itemIndex < viewModel.sections()[0].items().length; itemIndex++)
                {
                    var item = viewModel.sections()[0].items()[itemIndex];
                    if (item != undefined)
                    {
                        item.quantity(1);
                    }
                }
            }
        }
    };

    self.getCategory1 = function (menuItem)
    {
        return helper.findById(menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1, viewModel.menu.Category1);
    }
    self.getCategory2 = function (menuItem)
    {
        return helper.findById(menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2, viewModel.menu.Category2);
    }

    self.updatePrices = function ()
    {
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];

            if (section.items != undefined)
            {
                for (var itemIndex = 0; itemIndex < section.items().length; itemIndex++)
                {
                    menuItemWrapper = section.items()[itemIndex];

                    if (menuItemWrapper.menuItem == undefined) continue;

                    var price = self.getItemPrice(menuItemWrapper.menuItem);
                    
                    menuHelper.isItemEnabledForMenu(menuItemWrapper);

                    menuItemWrapper.price(price === -1 ? '' : helper.formatPrice(price));
                }
            }
        }
    }
};

var menuHelper = new MenuHelper();
