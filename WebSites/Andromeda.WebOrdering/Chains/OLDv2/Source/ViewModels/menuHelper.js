// Menu helper functions
function MenuHelper()
{
    var self = this;

    self.menuDataThumbnails = undefined;
    self.menuDataExtended = undefined;
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
    };
    self.refreshDealsAvailabilty = function ()
    {
        // Change each deal
        for (var dealIndex = 0; dealIndex < viewModel.deals().length; dealIndex++)
        {
            var dealItem = viewModel.deals()[dealIndex];
            dealItem.isEnabled(menuHelper.isDealItemEnabledForMenu(dealItem.deal));
        }
    };
    self.calculateItemPrice = function (menuItem, quantity, toppings, addToppingPrices, recalcFreeQuantities)
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
    };
    self.calculateDealItemPrice = function (menuItem, toppings, recalcFreeQuantities)
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
    };
    self.calculateDealLinePrice = function (bindableDealLine, excludeDealCalculation)
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
    self.getDealLinePrice = function (dealLine)
    {
        if (viewModel.orderType().toLowerCase() == 'delivery')
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
    };
    self.menuItemLookup = undefined;
    self.dealLookup = undefined;
    self.dealSectionCount = 0;
    self.buildDealsSection = function ()
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
    };
    self.addItemsToDealLine = function (dealLineWrapper)
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
    };
    self.lookupItemImages = function ()
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
    };
    self.sortItemSections = function ()
    {
        for (var sectionIndex = 0; sectionIndex < viewModel.sections().length; sectionIndex++)
        {
            var section = viewModel.sections()[sectionIndex];
            if (section.items != undefined)
            {
                section.items.sort(menuHelper.sortByDisplayOrder);
            }
        }
    };
    self.sortByDisplayOrder = function (a, b)
    {
        return Number(a.displayOrder) > Number(b.displayOrder) ? 1 : -1;
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
    self.isDealItemEnabledForCart = function (deal)
    {
        return menuHelper.isDealAvailable(deal.deal.deal) && !deal.minimumOrderValueNotMet();
    };
    self.getItemToppings = function (menuItem)
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
    };
    self.getCartItemDisplayName = function (item)
    {
        var displayName = item.quantity() + ' x ';
        var category1 = item.selectedCategory1(), 
            category2 = item.selectedCategory2();

        if (category1 != undefined)
        {
            displayName += category1.Name;
        }

        if (category2 != undefined)
        {
            if (displayName.length > 0) { displayName += " "; }
            displayName += category2.Name;
        }

        if (displayName.length > 0) { displayName += " "; }
        displayName += item.name()

        return displayName;
    };
};

var menuHelper = new MenuHelper();
