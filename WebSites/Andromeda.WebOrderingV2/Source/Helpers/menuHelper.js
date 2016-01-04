// Menu helper functions
function MenuHelper()
{
    var self = this;

    self.menuDataThumbnails = ko.observable(undefined);
    self.menuDataExtended = undefined;
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
    self.calculateDealItemAdditionalCosts = function (menuItem, toppings, recalcFreeQuantities)
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

                    price += self.applyToppingPrices(true, toppingPrices, removeToppingPrices);
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
                price += self.applyToppingPrices(true, toppingPrices, removeToppingPrices);
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

        if (cartDealLine.selectedMenuItem != undefined)
        {
            // Get the price of the selected item
            var basePrice = menuHelper.getItemPrice(cartDealLine.selectedMenuItem);

            if (excludeDealCalculation != undefined && excludeDealCalculation == true)
            {
                price = basePrice;
            }
            else
            {
                switch (cartDealLine.dealLineWrapper.dealLine.Type)
                {
                    case 'Fixed':
                        // Get the fixed price
                        price = menuHelper.getDealLinePrice(cartDealLine);
                        break;
                    case 'Discount':
                        // Get the discount amount
                        var discountAmount = menuHelper.getDealLinePrice(cartDealLine);

                        // Deduct the discount from the item price
                        price = basePrice - discountAmount;

                        // No refunds!
                        if (price < 0) price = 0;

                        break;
                    case 'Percentage':
                        // Get the percentage
                        var percentage = menuHelper.getDealLinePrice(cartDealLine) / 100;

                        // Deduct the percentage from the item price
                        price = Math.round(basePrice * percentage);

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
        else
        {
            return menuItem.ColPrice == undefined ? menuItem.CollectionPrice : menuItem.ColPrice == undefined ? menuItem.CollectionPrice : menuItem.ColPrice;
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
        else
        {
            return topping.ColPrice == undefined ? topping.CollectionPrice : topping.ColPrice;
        }
    };
    self.isItemAvailable = function (menuItem)
    {
        // Is the item available for collection or delivery?
        if ((viewModel.orderType().toLowerCase() == 'delivery' && menuItem.DelPrice == undefined && menuItem.DeliveryPrice == undefined) ||
            (viewModel.orderType().toLowerCase() == 'collection' && menuItem.ColPrice == undefined && menuItem.CollectionPrice == undefined))
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
            var validVarients = varients.filter(function (varient) {
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
    self.menuItemLookup = undefined;
    self.dealLookup = undefined;
    self.dealSectionCount = 0;
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

                        // The first item in drop down combo should be "Please select an item"
                        dealLineWrapper.menuItemWrappers.push
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

                    lineTime.end = new Date();
                    lineTimes.push(lineTime);
                }

                dealTime.linesEnd = new Date();

                var dealWrapper =
                {
                    deal: deal,
                    isEnabled: ko.observable(menuHelper.isDealItemEnabledForMenu(deal)),
                    isAvailableToday: ko.observable(menuHelper.isAvailableToday(deal)), // isAvailableToday,
                    dealLineWrappers: dealLineWrappers,
                    minimumOrderValue: deal.MinimumOrderValue > 0 ? helper.formatPrice(deal.MinimumOrderValue) : ''
                };

                viewModel.deals().push(dealWrapper);

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

                dealTime.end = new Date();
                dealTimes.push(dealTime);
            }

            startpush = new Date();
            
            var dealDisplayOrder = settings.dealSectionDisplayOrder == null ? 0 : settings.dealSectionDisplayOrder;

            for (var dealSectionIndex = 0; dealSectionIndex < dealSections.length; dealSectionIndex++)
            {
                var dealSection = dealSections[dealSectionIndex];

                var section = { templateName: 'dealsSection-template', display: { Name: dealSection.name, displayOrder: dealDisplayOrder++ }, deals: dealSection.deals, Index: viewModel.sections().length, Left: ko.observable(0) };
                //              viewModel.sections.push(section);
                viewModel.sections().push(section);
            }

            menuHelper.dealSectionCount = dealSections.length;
        }

        var end = new Date();
        if (settings.diagnosticsMode)
        {
            //var lineTimesText = '';
            //for (var lineTimeIndex = 0; lineTimeIndex < lineTimes.length; lineTimeIndex++)
            //{
            //    var lineTimeItem = lineTimes[lineTimeIndex];
            //    lineTimesText += 'lt' + lineTimeIndex + ' ' + (lineTimeItem.end.getTime() - lineTimeItem.start.getTime()) / 1000 + '\r\n'
            //}

            var dealTimesText = '';
            for (var dealTimeIndex = 0; dealTimeIndex < dealTimes.length; dealTimeIndex++)
            {
                var dealTimeItem = dealTimes[dealTimeIndex];
                dealTimesText += 'deal ' + dealTimeIndex + ' ' + (dealTimeItem.end.getTime() - dealTimeItem.start.getTime()) / 1000 +
                    ' lines ' + (dealTimeItem.linesEnd.getTime() - dealTimeItem.start.getTime()) / 1000  + '\r\n'
            }

            alert('total deal: ' + (end.getTime() - start.getTime()) / 1000 + '\r\n' +
                'push deal: ' + (end.getTime() - startpush.getTime()) / 1000 + '\r\n' +
                dealTimesText);
//                lineTimesText);
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

                // Is the item already in the deal line (the same item can be in a deal line multiple times e.g. different sizes of the same item)
                // but we only want it to appear once
                var menuItemWrapper = dealLineWrapper.menuItemWrapperLookup[itemName];

                if (menuItemWrapper == undefined)
                {
                    // Add the menu item wrapper to the deal line
                    menuItemWrapper = new MenuItemWrapper(menuItemDetails.menuItem, itemName, menuHelper.isItemEnabledForMenu(menuItemDetails.menuItem));

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
                menuItemWrapper.category1s().sort(menuHelper.sortBySortOrder);

                if (menuItemWrapper.category1s().length > 0)
                {
                    menuItemWrapper.selectedCategory1(menuItemWrapper.category1s()[0]);
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
                extension: undefined
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
            var dealIndexItem =
            {
                extension: undefined,
                deal: deal,
                thumbnailHeight: 0,
                thumbnailWidth: 0,
                thumbnail: undefined,
                image: undefined,
                overlayImage: undefined
            };

            menuHelper.dealLookup[deal.Id] = dealIndexItem;
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
  //          ko.utils.arrayPushAll(viewModel.sections, section);
  //          viewModel.sections.push(section);

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
            menuItemWrapper = new MenuItemWrapper(menuItem, itemName, menuHelper.isItemEnabledForMenu(menuItem));

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

        //var cat2 = menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2;
        //if (cat2 != undefined && cat2 != undefined)
        //{
        //    var category = helper.findById(cat2, viewModel.menu.Category2);
        //    if (category != undefined && !helper.findCategory(category, menuItemWrapper.category2s))
        //    {
        //        menuItemWrapper.category2s.push
        //        (
        //            {
        //                AdditionalHalfAndHalfCategories: category.AdditionalHalfAndHalfCategories,
        //                DealPremium: category.DealPremium,
        //                Id: category.Id,
        //                Name: category.Name,
        //                Parent: category.Parent,
        //                SortOrder: menuItem.DelPrice
        //            }
        //        );
        //    }
        //}

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
        }

        // Check to see if there is an overlay image
        if (menuItemWrapper.overlayImage == undefined || menuItemWrapper.overlayImage == undefined)
        {
            var lookupItem = menuHelper.menuItemLookup[menuItem.MenuId == undefined ? menuItem.Id : menuItem.MenuId];

            menuItemWrapper.overlayImage = lookupItem.overlayImage;
        }
    };
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
    self.isItemEnabledForCart = function (menuItem)
    {
        return menuHelper.isItemAvailable(menuItem);
    };
    self.isDealItemEnabledForCart = function (cartDeal)
    {
        return menuHelper.isDealAvailable(cartDeal.dealWrapper.deal) && !cartDeal.minimumOrderValueNotMet();
    };
    self.getCartDisplayToppings = function (toppings)
    {
        var cartDisplayToppings = [];
        var usedToppings = 0;

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
        var displayName = ''; //item.quantity() + ' x ';

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
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];
            if (section.items != undefined)
            {
                for (var itemIndex = 0; itemIndex < section.items().length; itemIndex++)
                {
                    var item = section.items()[itemIndex];
                    item.quantity = 1;
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

    self.updatePrices = function()
    {
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];

            if (section.items != undefined)
            {
                for (var itemIndex = 0; itemIndex < section.items().length; itemIndex++)
                {
                    menuItemWrapper = section.items()[itemIndex];

                    var price = self.getItemPrice(menuItemWrapper.menuItem);

                    menuItemWrapper.price(helper.formatPrice(price));
                }
            }
        }
    }
};

var menuHelper = new MenuHelper();
