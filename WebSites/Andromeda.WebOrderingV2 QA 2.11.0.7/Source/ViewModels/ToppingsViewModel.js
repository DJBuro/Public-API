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
    customerHelper.bindableCustomer(new AndroWeb.Models.Customer());

    self.cartItem = ko.observable(forCartItem);
    self.dealDetails = forDealDetails;

    self.onShown = function()
    {
        // Recalculate the items price taking into account added, removed and free toppings
        self.cartItem().recalculatePrice();

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
        // Commit the changes
        self.dealDetails.commitChanges(self.cartItem());

        // Hide the toppings popup
        toppingsPopupHelper.hidePopup({ mode: 'addDeal', cartDeal: self.dealDetails.cartDeal });
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
        // Cancel the changes
        self.dealDetails.cancelChanges(self.cartItem());

        // Hide the toppings popup
        toppingsPopupHelper.hidePopup(undefined, self.dealDetails);
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
        if (self.ignoreEvents) return true;

        return self.popupToppingChanged('single', this);
    }
    self.doubleChanged = function ()
    {
        if (self.ignoreEvents) return true;

        return self.popupToppingChanged('double', this);
    }
    self.popupToppingChanged = function (singleOrDouble, topping)
    {
        if (singleOrDouble === 'single')
        {
            // Did they tick or untick single?
            if (topping.selectedSingle() && self.cartItem().remainingToppingCount() === 0)
            {
                // Customer wants to add a topping but no more can be added - deny
                topping.selectedSingle(false);
                return false;
            }
        }
        else if (singleOrDouble === 'double')
        {
            // Did they tick or untick double?
            if (topping.selectedDouble() && (self.cartItem().remainingToppingCount() === 0 || self.cartItem().remainingToppingCount() === 1))
            {
                // Customer wants to add two toppings but there's only one or none left - deny
                topping.selectedDouble(false);
                return false;
            }
        }

        // Check the maximum number of duplicates
        if (self.cartItem().menuItem.ItemToppingRules !== undefined && self.cartItem().menuItem.ItemToppingRules.MaxDuplicates != undefined)
        {
            if (singleOrDouble == 'double' && self.cartItem().menuItem.ItemToppingRules.MaxDuplicates === 1)
            {
                topping.selectedDouble(false);
                return false;
            }
        }

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

        return true;
    }

    self.reset = function()
    {
        // Reset included toppings
        for (var index = 0; index < self.cartItem().includedToppings().length; index++)
        {
            var topping = self.cartItem().includedToppings()[index];

            topping.selectedSingle(true);
            topping.selectedDouble(false);
        }

        // Reset extra toppings
        for (var index = 0; index < self.cartItem().optionalToppings().length; index++)
        {
            var topping = self.cartItem().optionalToppings()[index];

            topping.selectedSingle(false);
            topping.selectedDouble(false);
        }

        // Recalculate the cart price based on the reset toppings
        self.cartItem().recalculatePrice();
    }
};































