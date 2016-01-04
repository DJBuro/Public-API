/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.Models.d.ts" />
/// <reference path="../../source/models/cart.ts" />
/// <reference path="baseviewmodel.ts" />
/// <reference path="../Models/Cart.ts" />
/// <reference path="../Models/CartItem.ts" />
/// <reference path="../Models/CartItemTopping.ts" />
/// <reference path="../../scripts/typings/react/react.d.ts" />
/// <reference path="../models/dealwrapper.ts" />
/// <reference path="../components/cartcomponent.ts" />
/// <reference path="../models/deal.ts" />

module AndroWeb.ViewModels
{
    import React = __React;

    export class RepeatOrderViewModel extends BaseViewModel
    {
        public originalOrder: any;
        public static orderToRepeat =
        {
            orderTotal: ko.observable(0),
            newOrderTotal: ko.observable(0),
            orderLines: ko.observableArray(),
            deals: ko.observableArray()
        }

        // Mobile mode
        public title: KnockoutObservable<string> = ko.observable(textStrings.mmChangeStoreButton); // Current section name - shown in the header
        public titleClass: KnockoutObservable<string> = ko.observable('mobileSectionSelectStore'); // Class to use to style the section name - used for showing an icon for the section

        public previousViewName: string;
        public previousContentViewModel: any;

        public cart: AndroWeb.Models.Cart;

        public static canBeRepeated: KnockoutObservable<boolean> = ko.observable(false);

        constructor(orderToRepeat: RepeatOrderLine)
        {
            super();

            this.isShowStoreDetailsButtonVisible(true);
            this.isShowHomeButtonVisible(true);
            this.isShowMenuButtonVisible(true);
            this.isShowCartButtonVisible(true);

            this.isHeaderVisible(true);
            this.isPostcodeSelectorVisible(false);
            this.areHeaderOptionsVisible(true);
            this.isHeaderLoginVisible(true);

            this.originalOrder = orderToRepeat;

            this.isPostcodeSelectorVisible(settings.storeSelectorMode && settings.storeSelectorInHeader)

            if (viewModel.contentViewModel() != undefined && viewModel.contentViewModel().previousViewName != undefined && viewModel.contentViewModel().previousViewName.length > 0)
            {
                this.previousViewName = viewModel.contentViewModel().previousViewName;
                this.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
            }
            else
            {
                this.previousViewName = guiHelper.getCurrentViewName();
                this.previousContentViewModel = viewModel.contentViewModel();
            }
        }
       
        public onShown() : void
        {
            this.cart = this.processOrder();

            var cartProps: AndroWeb.Components.CartProps = { cart: this.cart };

            var cartComponent = React.createElement(AndroWeb.Components.CartComponent, cartProps, {});
            React.render(cartComponent, document.getElementById("repeatOrderDetails"));
        }

        public processOrder(): AndroWeb.Models.Cart
        {
            // We'll use this temporary cart to do price calculations etc...
            // This is so that we don't have to duplicate any of the price calculation code
            var cart = new AndroWeb.Models.Cart();

            // Cos it's static, we need to clear out the order lines
            RepeatOrderViewModel.orderToRepeat.orderLines.removeAll();

            var validCartCount: number = 0;

            // Check each order line
            for (var orderLineIndex = 0; orderLineIndex < this.originalOrder.orderLines().length; orderLineIndex++)
            {
                var originalOrderLine = this.originalOrder.orderLines()[orderLineIndex];

                // We need something to bind the UI to
                var repeatOrderLine = new RepeatOrderLine(originalOrderLine);

                // Generate a cart item for the order line
                this.generateCartItem(repeatOrderLine);

                // Add to the bindable item to the list of order lines to repeat
                RepeatOrderViewModel.orderToRepeat.orderLines.push(repeatOrderLine);

                // Add the cart item to the temporary cart
                cart.cartItems.push(repeatOrderLine.cartItem);

                // Was this a valid item?
                if (repeatOrderLine.cartItem.isEnabled()) validCartCount++;
            }

            // Check each deal
            for (var dealIndex = 0; dealIndex < this.originalOrder.deals().length; dealIndex++)
            {
                var originalDeal = this.originalOrder.deals()[dealIndex];

                // We need something to bind the UI to
                var repeatDeal = new RepeatDealOrderLine(originalDeal);

                // Generate a cart item for the deal
                this.generateDealCartItem(repeatDeal);

                // Add to the bindable item to the list of order lines to repeat
                RepeatOrderViewModel.orderToRepeat.deals.push(repeatDeal);

                // Add the cart item to the temporary cart
                cart.deals.push(repeatDeal.cartDeal);

                // Was this a valid item?
                if (repeatDeal.cartDeal.isEnabled()) validCartCount++;
            }

            // Figure out the final price
            AndroWeb.Helpers.CartHelper.refreshCart(cart, false);

            //.....
            RepeatOrderViewModel.orderToRepeat.orderTotal(cart.totalPrice());
            RepeatOrderViewModel.orderToRepeat.newOrderTotal(undefined);

            // Can the order be repeated?
            RepeatOrderViewModel.canBeRepeated(validCartCount > 0);

            return cart;
        }

        public generateCartItem(repeatOrderLine: RepeatOrderLine) : void
        {
            var cartItem = undefined;

            // Try and find the menu item in the currently loaded menu
            var menuItemWrapper : AndroWeb.Models.IMenuItemWrapper = menuHelper.menuItemLookup[repeatOrderLine.originalOrderLine.id];
            if (menuItemWrapper !== undefined)
            {
                // Found a menu item in the current menu with the same id
                var rawMenuItem = menuItemWrapper.menuItem;
                var menuItemName = viewModel.menu.ItemNames[rawMenuItem.Name == undefined ? rawMenuItem.ItemName : rawMenuItem.Name];
                var menuItemWrapper2 = menuHelper.menuItemWrapperLookup[menuItemName];

                // Somthing very screwy going on with quantity
                // The knockout observable is doing some funky stuff so reset it to a new observable 
                menuItemWrapper2.quantity = ko.observable(repeatOrderLine.originalOrderLine.quantity);
            //    menuItemWrapper2.quantity(repeatOrderLine.originalOrderLine.quantity);

                // Build a cart item
                cartItem = new AndroWeb.Models.CartItem
                (
                    viewModel,
                    menuItemWrapper2,
                    rawMenuItem,
                    false
                );

                // Has the price changed?
                if (cartItem.price != repeatOrderLine.originalOrderLine.price)
                {
                    cartItem.notes(textStrings.roPriceChanged.replace("{originalPrice}", helper.formatPrice(repeatOrderLine.originalOrderLine.price)));
                }

                cartItem.isEnabled(true);
            }
            else
            {
                // The menu item in the original order cannot be found in the currently loaded menu
                // We're going to create fake menuItemrapper and menuItems that we can use for the CartItem
                var fakeMenuItem =
                    {
                        Id: -1,
                        DefTopIds: [],
                        OptTopIds: [],
                        FreeTops: 0,
                        DelPrice: 0,
                        ColPrice: 0,
                        DineInPrice: 0
                    };
                var fakeMenuItemWrapper =
                    {
                        name: repeatOrderLine.originalOrderLine.name,
                        quantity: ko.observable(repeatOrderLine.originalOrderLine.quantity),
                        menuItem: fakeMenuItem,
                        menuItems: ko.observableArray([fakeMenuItem]),
                        isEnabled: ko.observable(false),
                        notes: ko.observable(textStrings.roDiscontinued)
                    };

                // Build a cart item
                cartItem = new AndroWeb.Models.CartItem
                (
                    viewModel,
                    fakeMenuItemWrapper,
                    fakeMenuItem,
                    false
                );

                // This is not a valid cart item - it will shown as invalid and not included in the final price or order JSON
                cartItem.isEnabled(false);
                cartItem.notes(textStrings.roDiscontinued);
            }

            // We've now got a fully featured cart item
            repeatOrderLine.cartItem = cartItem;

            // Add toppings to the cart item
            var toppings: any[] = this.processToppings(repeatOrderLine.originalOrderLine.modifiers, repeatOrderLine.cartItem, repeatOrderLine.originalOrderLine.quantity);
            repeatOrderLine.cartItem.toppings(toppings);

            // Make sure the order line price is correct
            repeatOrderLine.cartItem.recalculatePrice();
        }

        public processToppings(originalToppings: any[], cartItem: any, quantity): AndroWeb.Models.CartItemTopping[]
        {
            var toppings: any[] = [];

            // Were any toppings added or removed from the original order line being repeated?
            if (originalToppings === undefined || originalToppings.length === 0) return;

            // We're going to add or remove each topping from the new cart item to match those that were originally added or removed 
            // from the order line being repeated
            // Note that the cart itself will do all the calculations - the exact same code that the check out cart uses so
            // in theory the repeat order totals should match the checkout totals
            for (var index = 0; index < originalToppings.length; index++)
            {
                // A topping that was added or removed from the original order line
                var originalModifier = originalToppings[index];

                // Try and find the same topping on the new cart item so we can add or remove it
                var found = false;
                var isEnabled: boolean = true;
                var notes: string = "";
                 
                // Does todays menu item have any toppings?
                if (cartItem.toppings() != undefined)
                {
                    // Go through the toppings for the menu item as it is today (bearing in mind that the menu might have changed)
                    // and try find the same topping that was originally added or removed from the order line being repeated
                    for (var menuItemToppingIndex = 0; menuItemToppingIndex < cartItem.toppings().length; menuItemToppingIndex++)
                    {
                        //  Is this the same topping that was originally added or removed?
                        var existingCartItemTopping = cartItem.toppings()[menuItemToppingIndex];
                        if (existingCartItemTopping.topping.Id === originalModifier.id)
                        {
                            // Has the topping type changed?
                            if ((originalModifier.removed && existingCartItemTopping.type === "removable") || // The topping was removeable
                                (!originalModifier.removed)) // The topping was added (or an included topping was doubled up)
                            {
                                // Found a match - add or remove the topping as needed
                                if (originalModifier.removed === false)
                                {
                                    if (originalModifier.quantity === 1)
                                    {
                                        existingCartItemTopping.selectedSingle(true);
                                    }
                                    else if (originalModifier.quantity === 2)
                                    {
                                        existingCartItemTopping.selectedDouble(true);
                                    }
                                }
                                else
                                {
                                    existingCartItemTopping.selectedSingle(false);
                                    existingCartItemTopping.selectedDouble(false);
                                }

                                // Has the price changed since the order was originally placed?
                                var newItemPrice: number = ((existingCartItemTopping.itemPrice * originalModifier.quantity) * quantity);

                                if (originalModifier.removed !== false)
                                {
                                    if (newItemPrice !== originalModifier.price)
                                    {
                                        // Just warn the customer that the price has changed
                                        existingCartItemTopping.notes = textStrings.roPriceChanged.replace("{originalPrice}", helper.formatPrice(originalModifier.price));
                                    }
                                }

                                // The topping is still valid today and will be added/removed from the cart item
                                existingCartItemTopping.isEnabled = true;

                                toppings.push(existingCartItemTopping);

                                found = true;
                                break;
                            }
                        }
                    }

                    // Update the topping price and quantity
                    existingCartItemTopping.recalculatePriceAndQuantity();
                }

                if (!found)
                {
                    // We couldn't find the topping on the menu item as it is today
                    // We will create a fake topping so we can tell the customer the topping doesn't exist and won't be included
                    // in the new order.  Note that the topping is set as isEnabled false so it won't actually be included in the order
                    var fakeTopping =
                        {
                            Name: originalModifier.originalModifier.Description,
                            Price: originalModifier.price,
                            Quantity: originalModifier.quantity,
                            Removed: originalModifier.removed
                        };

                    // Create a cart topping and add it to the cart item
                    var cartItemTopping = new AndroWeb.Models.CartItemTopping(fakeTopping);
                    cartItemTopping.isEnabled = false;
                    cartItemTopping.notes = textStrings.roDiscontinued;
                    cartItemTopping.quantity(fakeTopping.Quantity);

                    if (originalModifier.removed === false)
                    {
                        if (originalModifier.quantity === 1)
                        {
                            cartItemTopping.selectedSingle(true);
                        }
                        else if (originalModifier.quantity === 2)
                        {
                            cartItemTopping.selectedDouble(true);
                        }
                    }

                    toppings.push(cartItemTopping);
                }
            }

            return toppings;
        }

        public generateDealCartItem(repeatDeal: RepeatDealOrderLine): void
        {
            var cartDeal: AndroWeb.Models.CartDeal = undefined;

            // Try and find the deal in the currently loaded menu
            var dealWrapper: AndroWeb.Models.DealWrapper = menuHelper.dealLookup[repeatDeal.originalDealOrderLine.id];
            if (dealWrapper !== undefined)
            {
                cartDeal = new AndroWeb.Models.CartDeal(dealWrapper);
                cartDeal.cartDealLines.removeAll();
                cartDeal.isEnabled(false);

                // We've got a deal line but is the originally selected menu item still in the list of allowable menu items?
                this.processDealLines(cartDeal, dealWrapper, repeatDeal);
            }
            else
            {
                // The deal is no longer valid so we need to return a fake "placeholder" deal
                // so we can tell the customer that it's no longer valid
                cartDeal = this.createFakeDeal(repeatDeal);
            }

            // We've now got a fully featured deal
            repeatDeal.cartDeal = cartDeal;
        }

        public createFakeDeal(repeatDeal: RepeatDealOrderLine): AndroWeb.Models.CartDeal
        {
            var fakeDeal = new AndroWeb.Models.Deal();
            fakeDeal.AvailableTimes = [];
            fakeDeal.CollectionAmount = repeatDeal.originalDealOrderLine.price;
            fakeDeal.DealLines = [];
            fakeDeal.DealName = repeatDeal.originalDealOrderLine.name;
            fakeDeal.DeliveryAmount = repeatDeal.originalDealOrderLine.price;
            fakeDeal.Description = repeatDeal.originalDealOrderLine.description;
            fakeDeal.Display = 0;
            fakeDeal.DisplayOrder = 1;
            fakeDeal.ForceCheapestFree = false;
            fakeDeal.ForCollection = true;
            fakeDeal.ForDelivery = true;
            fakeDeal.ForDineIn = true;
            fakeDeal.FullOrderDiscountCollectionAmount = 0;
            fakeDeal.FullOrderDiscountDeliveryAmount = 0;
            fakeDeal.FullOrderDiscountMagicNumber = 0;
            fakeDeal.FullOrderDiscountType = repeatDeal.originalDealOrderLine.type;
            fakeDeal.Id = repeatDeal.originalDealOrderLine.id;
            fakeDeal.MinimumOrderValue = 0;

            var fakeDealWrapper = new AndroWeb.Models.DealWrapper();
            fakeDealWrapper.deal = fakeDeal;
            fakeDealWrapper.notes(textStrings.roDiscontinued);
            fakeDealWrapper.isEnabled(false);

            var cartDeal = new AndroWeb.Models.CartDeal(fakeDealWrapper);
            cartDeal.isEnabled(false);
            cartDeal.notes(textStrings.roDiscontinued);

            return cartDeal;
        }

        public processDealLines(cartDeal: AndroWeb.Models.CartDeal, dealWrapper: AndroWeb.Models.DealWrapper, repeatDeal: RepeatDealOrderLine): void
        {
            // Deal lines
            for (var originalDealLineIndex: number = 0; originalDealLineIndex < repeatDeal.originalDealOrderLine.dealLines.length; originalDealLineIndex++)
            {
                var originalCartDealLine = repeatDeal.originalDealOrderLine.dealLines[originalDealLineIndex];

                // In theory, each deal line in the original should match the deal line with the same indice in the menu deal
                if ((dealWrapper.dealLineWrappers.length - 1) < originalDealLineIndex)
                {
                    // There is no matching deal line in the menu deal
                    cartDeal.notes(textStrings.roDealChanged);
                    cartDeal.isEnabled(false);
                    return;
                }
                else
                {
                    var dealLineWrapper = dealWrapper.dealLineWrappers[originalDealLineIndex];

                    // Search through the allowable menu items and see if the original menu item is in there
                    var foundMenuItemWrapper = null;
                    var foundMenuItem = null;
                    for (var menuItemWrapperIndex = 0; menuItemWrapperIndex < dealLineWrapper.menuItemWrappers.length; menuItemWrapperIndex++)
                    {
                        var menuItemWrapper = dealLineWrapper.menuItemWrappers[menuItemWrapperIndex];

                        for (var menuItemIndex = 0; menuItemIndex < menuItemWrapper.menuItems().length; menuItemIndex++)
                        {
                            var menuItem = menuItemWrapper.menuItems()[menuItemIndex];

                            if (menuItem.Id === originalCartDealLine.id)
                            {
                                // Found a matching menu item
                                foundMenuItemWrapper = menuItemWrapper;
                                foundMenuItem = menuItem;

                                break;
                            }
                        }

                        if (foundMenuItemWrapper !== null) break;
                    }

                    if (foundMenuItemWrapper === null)
                    {
                        alert(originalCartDealLine.id);
                        // One of the menu items is no longer available as part of the deal
                        cartDeal.notes(textStrings.roNoLongerInDeal);
                        cartDeal.isEnabled(false);
                        return;
                    }
                    else
                    {
                        // The menu item is still in the allowable list so select it
                        var cartItem: AndroWeb.Models.CartItem = new AndroWeb.Models.CartItem
                        (
                            null,
                            foundMenuItemWrapper,
                            foundMenuItem,
                            false
                        );

                        // Process toppings
                        var toppings: any[] = this.processToppings(originalCartDealLine.modifiers, cartItem, 1);
                        cartItem.displayToppings(toppings);

                        var cartDealLine: AndroWeb.Models.CartDealLine = new AndroWeb.Models.CartDealLine(dealLineWrapper, originalCartDealLine.id);
                        cartDealLine.cartItem(cartItem);
                        cartDeal.cartDealLines.push(cartDealLine);
                    } 
                }
            }
            
            cartDeal.isEnabled(true);           
        }

        public addToCart = () =>
        {
            // Add items to cart
            this.addItemsToCart();

            // Return to the previous page
            guiHelper.showMenu(undefined);
        }

        public addToCartAndCheckout = () =>
        {
            // Add items to the cart
            this.addItemsToCart();

            // Show the checkout page
            AndroWeb.Helpers.CartHelper.checkout();
        }

        public addItemsToCart = () =>
        {
            // Menu items
            for (var cartItemIndex = 0; cartItemIndex < this.cart.cartItems().length; cartItemIndex++)
            {
                var cartItem = this.cart.cartItems()[cartItemIndex];

                if (cartItem.isEnabled())
                {
                    AndroWeb.Helpers.CartHelper.cart().cartItems.push(cartItem);

                    if (cartItem.toppings() != undefined)
                    {
                        // Remove any problem toppings (need to do this backwards as otherwise we'll screw up the array indexing)
                        for (var toppingIndex: number = cartItem.toppings().length - 1; toppingIndex > -1; toppingIndex--)
                        {
                            var topping: Models.CartItemTopping = cartItem.toppings()[toppingIndex];
                            if (!topping.isEnabled)
                            {
                                cartItem.toppings.splice(toppingIndex, 1);
                            }
                        }
                    }
                }
            }

            // Deals
            for (var dealIndex = 0; dealIndex < this.cart.deals().length; dealIndex++)
            {
                var dealCartItem = this.cart.deals()[dealIndex];

                if (dealCartItem.isEnabled())
                {
                    AndroWeb.Helpers.CartHelper.cart().deals.push(dealCartItem);

                    for (var dealLineIndex = 0; dealLineIndex < dealCartItem.cartDealLines().length; dealLineIndex++)
                    {
                        var dealLineCartItem = dealCartItem.cartDealLines()[dealLineIndex];

                        if (dealLineCartItem.toppings != undefined && dealLineCartItem.toppings() != undefined)
                        {
                            // Remove any problem toppings (need to do this backwards as otherwise we'll screw up the array indexing)
                            for (var toppingIndex: number = dealLineCartItem.toppings().length - 1; toppingIndex > -1; toppingIndex--)
                            {
                                var topping: Models.CartItemTopping = dealLineCartItem.toppings()[toppingIndex];
                                if (!topping.isEnabled)
                                {
                                    dealLineCartItem.toppings.splice(toppingIndex, 1);
                                }
                            }
                        }
                    }
                }
            }

            // Recalculate the cart price
            AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart(), false);
        }

        public cancel = () =>
        {
            guiHelper.showView(this.previousViewName, this.previousContentViewModel);
        }
    };

    export class RepeatOrderLine
    {
        public originalOrderLine: any;
        public cartItem: AndroWeb.Models.CartItem;
        public invalidPrice: number;

        constructor(originalOrderLine: any)
        {
            this.originalOrderLine = originalOrderLine;
        }
    };

    export class RepeatDealOrderLine
    {
        public originalDealOrderLine: any;
        public cartDeal: AndroWeb.Models.CartDeal;
        public invalidPrice: number;

        constructor(originalDealOrderLine: any)
        {
            this.originalDealOrderLine = originalDealOrderLine;
        }
    };
}








































