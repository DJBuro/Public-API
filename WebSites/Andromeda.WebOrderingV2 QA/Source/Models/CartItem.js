﻿/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function CartItem(viewModel, itemWrapper, initialMenuItem, additionsOnly)
{
    "use strict";
    var self = this;
    var hide = {
        itemWrapper: itemWrapper,
        cartItemSelectedCat1: function ()
        {
            if (hide.itemWrapper.menuItem === undefined)
            {
                return undefined;
            }
            else
            {
                var cat1Id = hide.itemWrapper.menuItem.Cat1 == undefined
                    ? hide.itemWrapper.Category1
                    : hide.itemWrapper.menuItem.Cat1;

                var selectedCat1 = cat1Id === undefined ? undefined : helper.findById(cat1Id, viewModel.menu.Category1);
                return selectedCat1;
            }
        },
        cartItemSelectedCat2: function ()
        {
            if (hide.itemWrapper.menuItem === undefined)
            {
                return undefined;
            }
            else
            {
                var cat2Id = hide.itemWrapper.menuItem.Cat2 == undefined
                    ? hide.itemWrapper.Category2
                    : hide.itemWrapper.menuItem.Cat2;

                var selectedCat2 = cat2Id === undefined ? undefined : helper.findById(cat2Id, viewModel.menu.Category2);
                return selectedCat2;
            }
        }
    };

    self.itemWrapper = itemWrapper;
    self.ignoreCategoryChanges = false;

    self.additionsOnly = additionsOnly;

    //i want to hide the context after everything it needs is taken. context (menu item wrapper changes as to the user on the menu items)
    //self.menuItemWrapper = menuItemWrapper; // Reference copy but this should never change
    self.menuItem = initialMenuItem;

    self.cartId = hide.itemWrapper.cartId;
    self.price = 0;
    self.name = hide.itemWrapper.name;
    self.description = hide.itemWrapper.description;
    self.thumbnail = hide.itemWrapper.thumbnail; // Reference copy but this should never change
    self.category1s = hide.itemWrapper.category1s; // Reference copy but these should never change
    self.category2s = ko.observableArray();

    self.isEnabled = ko.observable(true);

    // These can be changed using the popup UI
    self.displayPrice = ko.observable('');
    self.quantity = ko.observable(hide.itemWrapper.quantity);
    //these will include any playing around of toppings made but the user
    self.freeToppings = ko.observable(hide.itemWrapper.menuItem === undefined ? 0 : hide.itemWrapper.menuItem.FreeTops);
    self.freeToppingsRemaining = ko.observable(hide.itemWrapper.menuItem === undefined ? 0 : hide.itemWrapper.menuItem.FreeTops);
    self.toppings = ko.observableArray();
    self.instructions = ko.observable('');
    self.person = ko.observable('');

    self.addedToppingCount = ko.observable(0);
    self.remainingToppingCount = ko.observable(0);

    self.selectedCategory1 = ko.observable(hide.cartItemSelectedCat1());
    self.selectedCategory2 = ko.observable(hide.cartItemSelectedCat2());

    self.menuItems = [];
    for (var index = 0; index < hide.itemWrapper.menuItems().length; index++)
    {
        self.menuItems.push(hide.itemWrapper.menuItems()[index]);
    }

    self.displayName = ko.observable(menuHelper.getCartItemDisplayName(self));
    self.displayToppings = ko.observableArray();

    self.availableTimes = itemWrapper.availableTimes;

    self.removedTimeSlot = ko.observable(false);
    self.availabilityText = itemWrapper.availabilityText;

    self.initialiseToppings = function ()
    {
        self.toppings.removeAll();

        var newToppings = [];

        // Removable toppings
        var selectedMenuItem = self.menuItem;
        for (var index = 0; index < selectedMenuItem.DefTopIds.length; index++)
        {
            var id = selectedMenuItem.DefTopIds[index];
            var topping = helper.findByMenuId(id, viewModel.menu.Toppings);

            var cartTopping = new cartItemTopping(topping);
            cartTopping.type = 'removable';
            cartTopping.selectedSingle(true);
            cartTopping.selectedDouble(false);
            cartTopping.originalPrice = menuHelper.getToppingPrice(topping);
            cartTopping.singlePrice = helper.formatPrice(0);
            cartTopping.doublePrice = helper.formatPrice(cartTopping.originalPrice);

            cartTopping.recalculatePriceAndQuantity();

            //newToppings.push(cartTopping);
            self.addTopping(cartTopping, newToppings);
        }

        // Additional toppings
        for (var index = 0; index < selectedMenuItem.OptTopIds.length; index++)
        {
            var id = selectedMenuItem.OptTopIds[index];

            var topping = helper.findByMenuId(id, viewModel.menu.Toppings);

            var cartTopping = new cartItemTopping(topping);
            cartTopping.type = 'additional';
            cartTopping.selectedSingle(false);
            cartTopping.selectedDouble(false);
            cartTopping.originalPrice = menuHelper.getToppingPrice(topping);
            cartTopping.singlePrice = helper.formatPrice(cartTopping.originalPrice);
            cartTopping.doublePrice = helper.formatPrice(cartTopping.originalPrice * 2);

            cartTopping.recalculatePriceAndQuantity();

            //newToppings.push(cartTopping);
            self.addTopping(cartTopping, newToppings);
        }

        // Sort the toppings
        newToppings.sort
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

        self.toppings(newToppings);

        self.displayToppings(menuHelper.getCartDisplayToppings(self.toppings()));
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

    self.changePrice = function (newPrice)
    {
        self.price = newPrice;
        self.displayPrice(helper.formatPrice(self.price));
    };

    self.quantityChanged = function ()
    {
        self.recalculatePrice();
    };

    self.category1Changed = function ()
    {
        // Change category
        self.refreshCategory2s();

        // Ensure the correct menu item is selected
        self.selectMenuItemBySelectedCategories();

        // Re-initialise toppings
        self.initialiseToppings();

        // Recalculate price
        self.recalculatePrice();
    };

    self.selectMenuItemBySelectedCategories = function()
    {
        // Find the correct item
        if (self.menuItems.length == 1)
        {
            // There is only one item -- and will always be menuitems[0]
            //menuItemWrapper.menuItem = self.menuItems[0];
            hide.itemWrapper.menuItem = self.menuItems[0];
            self.menuItem = self.menuItems[0];
        }
        else
        {
            // Get the menu item for the selected category1 and category2
            for (var index = 0; index < self.menuItems.length; index++)
            {
                var checkItem = self.menuItems[index];

                if ((self.selectedCategory1() == undefined || self.selectedCategory1().Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1)) &&
                    (self.selectedCategory2() == undefined || self.selectedCategory2().Id == (checkItem.Cat2 == undefined ? checkItem.Category2 : checkItem.Cat2)))
                {
                    self.menuItem = checkItem;
                }
            }
        }
    };

    self.category2Changed = function ()
    {
        // Ensure the correct menu item is selected
        self.selectMenuItemBySelectedCategories();

        // Re-initialise toppings
        self.initialiseToppings();

        // Recalculate price
        self.recalculatePrice();
    };

    self.refreshCategory2s = function ()
    {
        // We don't want any infinite loops...
        if (self.ignoreCategoryChanges == false)
        {
            self.ignoreCategoryChanges = true;

            var result = menuHelper.rebuildCategory2List(hide.itemWrapper, self.selectedCategory1(), self.selectedCategory2());

            self.category2s(result.category2s);
            self.selectedCategory2(result.selectedCategory2);

            self.ignoreCategoryChanges = false;
        }
    };
    self.findCategory = function (category, categories)
    {
        for (var index = 0; index < categories.length; index++)
        {
            var existingCategory = categories[index];

            if (existingCategory.Name == category.Name)
            {
                return true;
            }
        }

        return false;
    };
    self.recalculatePrice = function ()
    {
        var removedToppingCount = 0;
        var toppingsSortedByPrice = [];

        var categoryPremium = 0;
        var itemPremium = 0;

        if (self.additionsOnly)
        {
            // Get the categories
            var category1 = helper.findById(hide.cartItemSelectedCat1() == undefined
                ? self.menuItem.Category1
                : self.menuItem.Cat1,
                viewModel.menu.Category1);
            var category2 = helper.findById(hide.cartItemSelectedCat2() == undefined
                ? self.menuItem.Category2
                : self.menuItem.Cat2,
                viewModel.menu.Category2);

            // Add category deal premiums
            categoryPremium = ((category1 == undefined ? 0 : category1.DealPremium) + (category2 == undefined ? 0 : category2.DealPremium));

            // Add item deal premium
            itemPremium = self.menuItem.DealPricePremiumOverride == 0
                ? self.menuItem.DealPricePremium
                : self.menuItem.DealPricePremiumOverride;
        }

        for (var toppingIndex = 0; toppingIndex < self.toppings().length; toppingIndex++)
        {
            var topping = self.toppings()[toppingIndex];

            // Reset free count - we will be redistributing free toppings next
            topping.freeToppings = 0;

            // We're gonna sort the toppings later
            toppingsSortedByPrice.push(topping);

            if (settings.areToppingSwapsEnabled === true)
            {
                // Has the topping been removed?
                if (topping.type == 'removable' && !topping.selectedSingle() && !topping.selectedDouble())
                {
                    removedToppingCount++; // Default toppings can only be single toppings
                }
            }
        }

        // Calculate total free toppings
        self.freeToppings(self.menuItem.FreeTops + removedToppingCount);
        var freeToppingsRemaining = self.freeToppings();

        // Sort the toppings by price - low to high so we remove the cheapest ones first
        toppingsSortedByPrice.sort(function (a, b) { return a.originalPrice - b.originalPrice });

        // No additional (no swapped) toppings yet
        self.addedToppingCount(0);

        // Go through all added toppings and allocate free toppings (cheapest first)
        for (var toppingIndex = 0; toppingIndex < toppingsSortedByPrice.length; toppingIndex++)
        {
            var topping = toppingsSortedByPrice[toppingIndex];

            var eligableToppings = 0;
            if (topping.type == 'removable' && !topping.selectedSingle() && topping.selectedDouble())
            {
                // Customer wants to double up a default single topping
                eligableToppings = 1;
            }
            else if (topping.type == 'additional' && topping.selectedSingle() && !topping.selectedDouble())
            {
                // Customer wants to add a single topping
                eligableToppings = 1;
            }
            else if (topping.type == 'additional' && !topping.selectedSingle() && topping.selectedDouble())
            {
                // Customer wants to add a double topping
                eligableToppings = 2;
            }

            for (var singleToppingIndex = 0; singleToppingIndex < eligableToppings; singleToppingIndex++)
            {
                if (freeToppingsRemaining > 0)
                {
                    // Customer gets one topping free!
                    topping.freeToppings++;

                    // We've used up a free topping
                    freeToppingsRemaining--;
                }
                else
                {
                    // This topping has been added and isn't a swap
                    self.addedToppingCount(self.addedToppingCount() + 1);
                }
            }

            // Recalculate the toppings price taking into account free toppings
            topping.recalculatePriceAndQuantity();
        }

        // Tell the customer how many free toppings are left
        self.freeToppingsRemaining(freeToppingsRemaining);

        // The remaining number of toppings that the customer can add to this item
        // If ItemToppingRules.MaxToppings is zero that means there's no limit to the number of toppings that can be added
        // We'll use -1 as a magic number to indicate there's no limit
        self.remainingToppingCount
        (
            self.menuItem.ItemToppingRules === undefined ? -1 :
            (
                self.menuItem.ItemToppingRules.Max === undefined ? -1 :
                (
                    self.menuItem.ItemToppingRules.Max === 0 ? -1 : self.menuItem.ItemToppingRules.Max
                )
            )
        );

        if (self.remainingToppingCount() > -1)
        {
            self.remainingToppingCount(self.remainingToppingCount() - self.addedToppingCount());
            self.remainingToppingCount(self.remainingToppingCount() < 0 ? 0 : self.remainingToppingCount());
        }

        // Lastly, add the topping prices
        var newPrice = self.additionsOnly === true ? (categoryPremium + itemPremium) : menuHelper.getItemPrice(self.menuItem);

        for (var toppingIndex = 0; toppingIndex < self.toppings().length; toppingIndex++)
        {
            var topping = self.toppings()[toppingIndex];

            newPrice += topping.price;
        }

        // Don't forget the quantity :)
        newPrice = newPrice * self.quantity();

        // And update the menu items price
        self.changePrice(newPrice);
    };

    // ***** THE FOLLOWING INITIALISATION MUST BE DONE LAST TO ENSURE THE METHODS EXIST *****

    // Cat2s will depend on which cat1 is selected
    self.refreshCategory2s();

    self.selectMenuItemBySelectedCategories();

    // Initialise the toppings
    self.initialiseToppings();

    // Initialise the price
    self.recalculatePrice();
};































