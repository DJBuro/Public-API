/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="./CartItemTopping.ts" />

module AndroWeb.Models
{
    export class CartItem
    {
        public itemWrapper: any;
        public menuItem: any;

        public ignoreCategoryChanges: boolean = false;

        public additionsOnly: boolean = false;
        public includeDealLinePremium: boolean = false;

        public cartId: string;
        public price: number = 0;
        public name: string;
        public description: string;
        public thumbnail: any;
        public category1s: KnockoutObservableArray<any> = ko.observableArray([]); 
        public category2s: KnockoutObservableArray<any> = ko.observableArray([]);

        public isEnabled: KnockoutObservable<boolean> = ko.observable(true);
        public notes: KnockoutObservable<string> = ko.observable("");

        // These can be changed using the popup UI
        public displayPrice: KnockoutObservable<string> = ko.observable('');
        public quantity: KnockoutObservable<number> = ko.observable(0);
        // These will include any playing around of toppings made but the user
        public freeToppings: KnockoutObservable<any> = ko.observable(0);
        public freeToppingsRemaining: KnockoutObservable<any> = ko.observable(0);

        public toppings: KnockoutObservableArray<CartItemTopping> = ko.observableArray([]);
        public includedToppings: KnockoutObservableArray<CartItemTopping> = ko.observableArray([]);
        public optionalToppings: KnockoutObservableArray<CartItemTopping> = ko.observableArray([]);

        public toppingsIndex: any = {};
        public toppingsByNameIndex: any = {};
        public instructions: KnockoutObservable<string> = ko.observable('');
        public person: KnockoutObservable<string> = ko.observable('');

        public addedToppingCount: KnockoutObservable<number> = ko.observable(0);
        public remainingToppingCount: KnockoutObservable<number> = ko.observable(0);

        public selectedCategory1: KnockoutObservable<any> = ko.observable();
        public selectedCategory2: KnockoutObservable<any> = ko.observable();

        public menuItems: any[] = [];
        
        public displayName: KnockoutObservable<string> = ko.observable('');
        public displayToppings: KnockoutObservable<any> = ko.observableArray();

        public removedTimeSlot: KnockoutObservable<boolean> = ko.observable(false);

        public availableTimes: any;
        public availabilityText: string = '';

        constructor(dummy, itemWrapper, initialMenuItem, additionsOnly: boolean, includeDealLinePremium: boolean = true)
        {
            this.itemWrapper = itemWrapper;
            this.menuItem = initialMenuItem;
            this.additionsOnly = additionsOnly;
            this.includeDealLinePremium = includeDealLinePremium;

            this.cartId = this.itemWrapper.cartId;
            this.name = this.itemWrapper.name;
            this.description = this.itemWrapper.description
            this.thumbnail = this.itemWrapper.thumbnail; // Reference copy but this should never change
            this.category1s = this.itemWrapper.category1s; // Reference copy but these should never change

            this.quantity(this.itemWrapper.quantity());
            this.freeToppings(this.itemWrapper.menuItem === undefined ? 0 : this.itemWrapper.menuItem.FreeTops);
            this.freeToppingsRemaining(this.itemWrapper.menuItem === undefined ? 0 : this.itemWrapper.menuItem.FreeTops);

            this.selectedCategory1(this.cartItemSelectedCat1());
            this.selectedCategory2(this.cartItemSelectedCat2());

            this.displayName(menuHelper.getCartItemDisplayName(self));

            this.availableTimes = itemWrapper.availableTimes;
            this.availabilityText = itemWrapper.availabilityText;

            for (var index = 0; index < this.itemWrapper.menuItems().length; index++)
            {
                this.menuItems.push(this.itemWrapper.menuItems()[index]);
            }

            // Cat2s will depend on which cat1 is selected
            this.refreshCategory2s();

            this.selectMenuItemBySelectedCategories();

            // Initialise the toppings
            this.initialiseToppings();

            // Initialise the price
            this.recalculatePrice();
        }

        public cartItemSelectedCat1 = function()
        {
            if (this.menuItem === undefined)
            {
                return undefined;
            }
            else
            {
                var cat1Id = this.menuItem.Cat1 == undefined
                    ? this.menuItem.Category1
                    : this.menuItem.Cat1;

                var selectedCat1 = cat1Id === undefined ? undefined : helper.findById(cat1Id, viewModel.menu.Category1);
                return selectedCat1;
            }
        }

        public cartItemSelectedCat2 = function()
        {
            if (this.menuItem === undefined)
            {
                return undefined;
            }
            else
            {
                var cat2Id = this.menuItem.Cat2 == undefined
                    ? this.menuItem.Category2
                    : this.menuItem.Cat2;

                var selectedCat2 = cat2Id === undefined ? undefined : helper.findById(cat2Id, viewModel.menu.Category2);
                return selectedCat2;
            }
        }

        public initialiseToppings = function()
        {
            var newToppings = [];
            var toppingsIndex = {};
            var toppingsByNameIndex = {};

            // Removable toppings
            var selectedMenuItem = this.menuItem;
            for (var index = 0; index < selectedMenuItem.DefTopIds.length; index++)
            {
                var id = selectedMenuItem.DefTopIds[index];
                var topping = helper.findByMenuId(id, viewModel.menu.Toppings);

                var cartTopping = new CartItemTopping(topping);
                cartTopping.type = 'removable';
                cartTopping.selectedSingle(true);
                cartTopping.selectedDouble(false);
                cartTopping.originalPrice = menuHelper.getToppingPrice(topping);
                cartTopping.singlePrice = helper.formatPrice(0);
                cartTopping.doublePrice = helper.formatPrice(cartTopping.originalPrice);

                cartTopping.recalculatePriceAndQuantity();

                this.addTopping(cartTopping, newToppings, toppingsIndex, toppingsByNameIndex);
            }

            // Additional toppings
            for (var index = 0; index < selectedMenuItem.OptTopIds.length; index++)
            {
                var id = selectedMenuItem.OptTopIds[index];

                var topping = helper.findByMenuId(id, viewModel.menu.Toppings);

                var cartTopping = new CartItemTopping(topping);
                cartTopping.type = 'additional';
                cartTopping.selectedSingle(false);
                cartTopping.selectedDouble(false);
                cartTopping.originalPrice = menuHelper.getToppingPrice(topping);
                cartTopping.singlePrice = helper.formatPrice(cartTopping.originalPrice);
                cartTopping.doublePrice = helper.formatPrice(cartTopping.originalPrice * 2);

                cartTopping.recalculatePriceAndQuantity();

                this.addTopping(cartTopping, newToppings, toppingsIndex, toppingsByNameIndex);
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

            // Commit the new toppings
            this.toppingsIndex = toppingsIndex;
            this.toppingsByNameIndex = toppingsByNameIndex;
            this.toppings.removeAll();
            this.toppings(newToppings);

            // We need seperate lists of included and optional toppings
            this.splitOutIncludedAndOptionalToppings();

            this.displayToppings(menuHelper.getCartDisplayToppings(this.toppings()));
        }

        public addTopping = function(addTopping, toppings, toppingsIndex, toppingsByNameIndex)
        {
            // Has the topping already been added?
            var addToppingId = addTopping.topping.Id == undefined ? addTopping.topping.MenuId : addTopping.topping.Id;
            addToppingId = typeof (addToppingId) === 'number' ? addToppingId.toString() : addToppingId;
            var existingTopping = toppingsIndex[addToppingId];

            if (existingTopping === undefined)
            {
                // Topping hasn't already been added so add it
                toppingsIndex[addToppingId] = addTopping;
                toppingsByNameIndex[addTopping.topping.Name] = addTopping;

                toppings.push(addTopping);

                // Lookup the existing toppings and see if we can copy over the quantities
                var existingTopping = this.toppingsByNameIndex[addTopping.topping.Name];
                if (existingTopping !== undefined)
                {
                    // There is an existing topping with the same name
                    // If the toppings are the same type (optional or removable) then copy the quantities across
                    if (addTopping.type == existingTopping.type)
                    {
                        addTopping.quantity(existingTopping.quantity());
                        addTopping.selectedDouble(existingTopping.selectedDouble());
                        addTopping.selectedSingle(existingTopping.selectedSingle());
                    }
                }
            }
        }

        public splitOutIncludedAndOptionalToppings()
        {
            var optionalToppings = [];
            var includedToppings = [];

            for (var toppingIndex = 0; toppingIndex < this.toppings().length; toppingIndex++)
            {
                var topping = this.toppings()[toppingIndex];
                
                if (topping.type === 'removable')
                {
                    includedToppings.push(topping);
                }
                else
                {
                    optionalToppings.push(topping);
                }
            }

            this.includedToppings.removeAll();
            this.optionalToppings.removeAll();
            this.includedToppings(includedToppings);
            this.optionalToppings(optionalToppings);
        }

        public changePrice = function(newPrice)
        {
            this.price = newPrice;
            this.displayPrice(helper.formatPrice(this.price));
        }

        public quantityChanged = function ()
        {
            this.recalculatePrice();
        }

        public category1Changed = function ()
        {
            // Change category
            this.refreshCategory2s();

            // Ensure the correct menu item is selected
            this.selectMenuItemBySelectedCategories();

            // Re-initialise toppings
            this.initialiseToppings();

            // Recalculate price
            this.recalculatePrice();
        }

        public selectMenuItemBySelectedCategories = function()
        {
            // Find the correct item
            if (this.menuItems.length == 1)
            {
                // There is only one item -- and will always be menuitems[0]
                this.itemWrapper.menuItem = this.menuItems[0];
                this.menuItem = this.menuItems[0];
            }
            else
            {
                // Get the menu item for the selected category1 and category2
                for (var index = 0; index < this.menuItems.length; index++)
                {
                    var checkItem = this.menuItems[index];

                    if ((this.selectedCategory1() == undefined || this.selectedCategory1().Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1)) &&
                        (this.selectedCategory2() == undefined || this.selectedCategory2().Id == (checkItem.Cat2 == undefined ? checkItem.Category2 : checkItem.Cat2)))
                    {
                        this.menuItem = checkItem;
                    }
                }
            }
        }

        public category2Changed = function ()
        {
            // Ensure the correct menu item is selected
            this.selectMenuItemBySelectedCategories();

            // Re-initialise toppings
            this.initialiseToppings();

            // Recalculate price
            this.recalculatePrice();
        }

        public refreshCategory2s = function ()
        {
            // We don't want any infinite loops...
            if (this.ignoreCategoryChanges == false)
            {
                this.ignoreCategoryChanges = true;

                var result = menuHelper.rebuildCategory2List(this.itemWrapper, this.selectedCategory1(), this.selectedCategory2());

                this.category2s(result.category2s);
                this.selectedCategory2(result.selectedCategory2);

                this.ignoreCategoryChanges = false;
            }
        }

        public findCategory = function (category, categories)
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
        }

        public recalculatePrice = function ()
        {
            var removedToppingCount = 0;
            var toppingsSortedByPrice = [];

            var categoryPremium = 0;
            var itemPremium = 0;

            if (this.additionsOnly && this.includeDealLinePremium)
            {
                // Get the categories
                var category1 = helper.findById(this.cartItemSelectedCat1() == undefined
                    ? this.menuItem.Category1
                    : this.menuItem.Cat1,
                    viewModel.menu.Category1);
                var category2 = helper.findById(this.cartItemSelectedCat2() == undefined
                    ? this.menuItem.Category2
                    : this.menuItem.Cat2,
                    viewModel.menu.Category2);

                // Add category deal premiums
                categoryPremium = ((category1 == undefined ? 0 : category1.DealPremium) + (category2 == undefined ? 0 : category2.DealPremium));

                // Add item deal premium
                itemPremium = this.menuItem.DealPricePremiumOverride == 0
                    ? this.menuItem.DealPricePremium
                    : this.menuItem.DealPricePremiumOverride;
            }

            if (this.toppings() !== undefined)
            {
                for (var toppingIndex = 0; toppingIndex < this.toppings().length; toppingIndex++)
                {
                    var topping = this.toppings()[toppingIndex];

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
                this.freeToppings(this.menuItem.FreeTops + removedToppingCount);
                var freeToppingsRemaining = this.freeToppings();

                // Sort the toppings by price - low to high so we remove the cheapest ones first
                toppingsSortedByPrice.sort(function (a, b) { return a.originalPrice - b.originalPrice });

                // No additional (no swapped) toppings yet
                this.addedToppingCount(0);

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
                            this.addedToppingCount(this.addedToppingCount() + 1);
                        }
                    }

                    // Recalculate the toppings price taking into account free toppings
                    topping.recalculatePriceAndQuantity();
                }

                // Tell the customer how many free toppings are left
                this.freeToppingsRemaining(freeToppingsRemaining);

                // The remaining number of toppings that the customer can add to this item
                // If ItemToppingRules.MaxToppings is zero that means there's no limit to the number of toppings that can be added
                // We'll use -1 as a magic number to indicate there's no limit
                this.remainingToppingCount
                    (
                    this.menuItem.ItemToppingRules === undefined ? -1 :
                        (
                            this.menuItem.ItemToppingRules.Max === undefined ? -1 :
                                (
                                    this.menuItem.ItemToppingRules.Max === 0 ? -1 : this.menuItem.ItemToppingRules.Max
                                    )
                            )
                    );

                if (this.remainingToppingCount() > -1)
                {
                    this.remainingToppingCount(this.remainingToppingCount() - this.addedToppingCount());
                    this.remainingToppingCount(this.remainingToppingCount() < 0 ? 0 : this.remainingToppingCount());
                }

                // Lastly, add the topping prices
                var fullPrice = menuHelper.getItemPrice(this.menuItem);
                var additionsPrice = categoryPremium + itemPremium;

                for (var toppingIndex = 0; toppingIndex < this.toppings().length; toppingIndex++)
                {
                    var topping = this.toppings()[toppingIndex];

                    fullPrice += topping.price;
                    additionsPrice += topping.price;
                }
            }

            // Don't forget the quantity :)
            fullPrice = fullPrice * this.quantity();
            additionsPrice = additionsPrice * this.quantity();

            // Are we in additions only mode?
            if (this.additionsOnly === true)
            {
                // The price of the cart item is the additional only price
                this.changePrice(additionsPrice);
            }
            else
            {
                // The price of the cart item is the full item price
                this.changePrice(fullPrice);
            }
        }
    }
}































