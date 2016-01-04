/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function ToppingsViewModel(forCartItem, callback, forDealDetails)
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(true);
    self.isShowHomeButtonVisible = ko.observable(true);
    self.isShowMenuButtonVisible = ko.observable(true);
    self.isShowCartButtonVisible = ko.observable(true);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(settings.storeSelectorMode && settings.storeSelectorInHeader);
    self.areHeaderOptionsVisible = ko.observable(true);
    self.isHeaderLoginVisible = ko.observable(true);

    // Mobile mode
    self.title = ko.observable(textStrings.mmChangeStoreButton); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionSelectStore'); // Class to use to style the section name - used for showing an icon for the section

    // Create a new address & customer that we can bind to
    addressHelper.bindableAddress(new Address());
    customerHelper.bindableCustomer(new Customer());

    self.cartItem = ko.observable(forCartItem);
    self.dealDetails = forDealDetails;

    self.onShown = function()
    {
        if (callback != undefined)
        {
            callback();
        }
    }

    self.onLogout = function ()
    {
    }

    self.acceptChanges = function ()
    {
        toppingsPopupHelper.acceptChanges();
    }
    self.commitToCart = function ()
    {
        toppingsPopupHelper.commitToCart(self.cartItem());
    }
    self.commitToDeal = function ()
    {
        toppingsPopupHelper.commitToDeal(self.dealDetails);
    }
    self.removeFromCart = function ()
    {
        toppingsPopupHelper.removeFromCart(self.cartItem());
    }
    self.cancel = function ()
    {
        toppingsPopupHelper.cancel();
    }
    self.cancelDeal = function ()
    {
        if (toppingsPopupHelper.mode() == 'addDealItem')
        {
            // Were there multiple items to pick from?
            if (self.dealDetails.cartDealLine.allowableMenuItemWrappers.length > 1)
            {
                // Was an item already selected?
                if (self.dealDetails.cartDealLine.previouslySelectedAllowableMenuItemWrapper == undefined)
                {
                    // Display the "please select an item" item
                    self.dealDetails.cartDealLine.selectedAllowableMenuItemWrapper(self.dealDetails.cartDealLine.allowableMenuItemWrappers[0]);
                    self.dealDetails.cartDealLine.selectedMenuItem = undefined;
                }
                else
                {
                    // Restore the previously selected item
                    self.dealDetails.cartDealLine.selectedAllowableMenuItemWrapper(self.dealDetails.cartDealLine.previouslySelectedAllowableMenuItemWrapper);
                    self.dealDetails.cartDealLine.selectedMenuItem = self.dealDetails.cartDealLine.previouslySelectedMenuItem;
                }

                // Make sure the original menu item is selected for the cart
                if (self.dealDetails.cartDealLine.selectedMenuItem != null)
                {
                    var cartItem = new CartItem
                    (
                        self.dealDetails.cartDealLine.selectedAllowableMenuItemWrapper().menuItemWrapper,
                        self.dealDetails.cartDealLine.selectedMenuItem,
                        true
                    );

                    cartItem.menuItem = self.dealDetails.cartDealLine.selectedMenuItem;
                    cartItem.selectedCategory1(menuHelper.getCategory1(cartItem.menuItem));
                    cartItem.selectedCategory2(menuHelper.getCategory2(cartItem.menuItem));
                    cartItem.selectedCategory2(menuHelper.getCategory2(cartItem.menuItem));

                    // Restore selected toppings
                    if (self.dealDetails.cartDealLine.previouslySelectedToppings != undefined)
                    {
                        cartItem.toppings.removeAll();
                        for (var index = 0; index < self.dealDetails.cartDealLine.previouslySelectedToppings.length; index++)
                        {
                            var previousTopping = self.dealDetails.cartDealLine.previouslySelectedToppings[index];

                            var restoredTopping = new cartItemTopping();
                            restoredTopping.cartName(previousTopping.cartName());
                            restoredTopping.cartPrice(previousTopping.cartPrice());
                            restoredTopping.cartQuantity(previousTopping.cartQuantity());
                            restoredTopping.doublePrice = previousTopping.doublePrice;
                            restoredTopping.finalPrice = previousTopping.finalPrice;
                            restoredTopping.freeToppings = previousTopping.freeToppings;
                            restoredTopping.itemPrice = previousTopping.itemPrice;
                            restoredTopping.originalPrice = previousTopping.originalPrice;
                            restoredTopping.price = previousTopping.price;
                            restoredTopping.quantity(previousTopping.quantity());
                            restoredTopping.singlePrice = previousTopping.singlePrice;
                            restoredTopping.topping = previousTopping.topping;
                            restoredTopping.type = previousTopping.type;

                            cartItem.toppings.push(restoredTopping);
                        }
                    }

                    self.dealDetails.cartDealLine.cartItem(cartItem);
                }
            }
        }

        // Hide the toppings popup
        toppingsPopupHelper.hidePopup(undefined, self.dealDetails);

        // Show the deal popup
 //       dealPopupHelper.showDealPopup(self.dealDetails.mode, false, self.dealDetails.cartDeal);
    }
    self.decreaseQuantity = function ()
    {
        var quantity = Number(self.cartItem().quantity());

        if (quantity > 1)
        {
            self.cartItem().quantity(quantity - 1);
            self.cartItem().quantityChanged(toppingsPopupHelper.mode());
        }
    }
    self.increaseQuantity = function ()
    {
        var quantity = Number(self.cartItem().quantity());

        if (quantity < settings.maxQuantity)
        {
            self.cartItem().quantity(quantity + 1);
            self.cartItem().quantityChanged(toppingsPopupHelper.mode());
        }
    }
    self.popupCategory1Changed = function ()
    {
        self.cartItem().category1Changed();
    }
    self.popupSelectedItemChanged = function ()
    {
        self.cartItem().category2Changed();
    }
    self.singleChanged = function ()
    {
        self.popupToppingChanged('single', this);

        return true;
    }
    self.doubleChanged = function ()
    {
        self.popupToppingChanged('double', this);

        return true;
    }
    self.popupToppingChanged = function (singleOrDouble, topping)
    {
        // Both double and single cannot be ticked:

        // If single was ticked and double is already ticked then untick double
        if (singleOrDouble == 'single' && topping.selectedSingle())
        {
            topping.selectedDouble(false);
        }
            // If double was ticked and single is already ticked then untick single
        else if (singleOrDouble == 'double' && topping.selectedSingle())
        {
            topping.selectedSingle(false);
        }

        // Recalculate the items price taking into account added, removed and free toppings
        self.cartItem().recalculatePrice();
    }
};































