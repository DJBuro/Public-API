/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function DealViewModel(cartDeal, callback, mode)
{
    "use strict";

    var self = this;

    this.isShowStoreDetailsButtonVisible = ko.observable(true);
    this.isShowHomeButtonVisible = ko.observable(true);
    this.isShowMenuButtonVisible = ko.observable(true);
    this.isShowCartButtonVisible = ko.observable(true);

    this.isHeaderVisible = ko.observable(viewModel.isMobileMode() ? false : true);
    this.isPostcodeSelectorVisible = ko.observable(settings.storeSelectorMode && settings.storeSelectorInHeader);
    this.areHeaderOptionsVisible = ko.observable(false);
    this.isHeaderLoginVisible = ko.observable(true);

    // Mobile mode
    this.title = ko.observable(textStrings.mmChangeStoreButton); // Current section name - shown in the header
    this.titleClass = ko.observable('mobileSectionSelectStore'); // Class to use to style the section name - used for showing an icon for the section

    // Create a new address & customer that we can bind to
    addressHelper.bindableAddress(new Address());
    customerHelper.bindableCustomer(new Customer());

    this.subscriptions = [];

    this.cartDeal = ko.observable(cartDeal);

    this.mode = ko.observable(mode);
    this.ignoreDealLineChanges = false;

    this.onShown = function ()
    {
    }

    this.onLogout = function ()
    {
    }

    this.acceptChanges = function ()
    {
        // Has the user entered all the required information?
        if (!self.cartDeal().checkForErrors())
        {
            self.unSubscribeToDealLineChanges();

            self.sortDealLinesCheapestLast();

            // Recalculate the cart price
            cartHelper.refreshCart();

            // Hide the deal popup
            dealPopupHelper.hideDealPopup(true);

            // If we are in mobile mode show the cart
            if (guiHelper.isMobileMode())
            {
                viewHelper.showCart();
            }
        }
    }
    this.removeFromCart = function ()
    {
        self.unSubscribeToDealLineChanges();

        // Remove the item from the cart
        cartHelper.cart().deals.remove(self.cartDeal());

        // Recalculate the total price
        cartHelper.refreshCart();

        // Hide the deal popup
        dealPopupHelper.hideDealPopup(true);

        // If we are in mobile mode show the cart
        if (guiHelper.isMobileMode())
        {
            viewHelper.showCart();
        }
    }
    this.commitToCart = function ()
    {
        // Has the user entered all the required information?
        if (!self.cartDeal().checkForErrors())
        {
            self.unSubscribeToDealLineChanges();

            self.sortDealLinesCheapestLast();

            cartHelper.cart().deals.push(self.cartDeal());

            // Recalculate the cart price
            cartHelper.refreshCart();

            // Hide the deal popup
            dealPopupHelper.hideDealPopup(false);
        }
    }
    this.dealPopupCancel = function ()
    {
        self.unSubscribeToDealLineChanges();

        // Hide the deal popup
        dealPopupHelper.hideDealPopup(true);
    }
    
    this.sortDealLinesCheapestLast = function ()
    {
        // Is there more than one deal line?
        if (self.cartDeal().cartDealLines().length > 1)
        {
            // Starting with the second deal line, check each deal line before it to see if we need to swap them around so the cheapest is last
            for (var dealLineIndex = 1; dealLineIndex < self.cartDeal().cartDealLines().length; dealLineIndex++)
            {
                var bindableDealLine = self.cartDeal().cartDealLines()[dealLineIndex];

                for (var checkDealLineIndex = dealLineIndex - 1; checkDealLineIndex >= 0; checkDealLineIndex--)
                {
                    var checkDealLine = self.cartDeal().cartDealLines()[checkDealLineIndex];

                    // Check each of the allowable menu items for the other deal line
                    // If the selected menu item is also an allowable item in the other deal line then we need to compare prices and possibly swap them
                    for (var menuIdIndex = 0; menuIdIndex < checkDealLine.dealLineWrapper.dealLine.AllowableItemsIds.length; menuIdIndex++)
                    {
                        var allowedItem = checkDealLine.dealLineWrapper.dealLine.AllowableItemsIds[menuIdIndex];

                        // Is this allowable menu item the same menu item as the customer selected in the target deal line?
                        if (bindableDealLine.selectedMenuItem.Id == allowedItem)
                        {
                            // This deal line's list of allowable menu items does contain the target menu item

                            // Check if we need to swap them

                            // Calculate deal line price
                            var baseDealLinePrice = menuHelper.calculateDealLinePrice(bindableDealLine, true);
                            var additionalDealLineCost = menuHelper.calculateDealItemAdditionalCosts(bindableDealLine.selectedMenuItem, bindableDealLine.toppings, true);
                            var dealLinePrice = baseDealLinePrice + additionalDealLineCost;

                            // Calculate deal line price to compare to
                            var baseCheckDealLinePrice = menuHelper.calculateDealLinePrice(checkDealLine, true);
                            var additionalCheckDealLineCost = menuHelper.calculateDealItemAdditionalCosts(checkDealLine.selectedMenuItem, checkDealLine.toppings, true);
                            var checkDealLinePrice = baseCheckDealLinePrice + additionalCheckDealLineCost;

                            if (dealLinePrice > checkDealLinePrice)
                            {
                                // We need to switch the menu items around so the more expensive one is first

                                var swapMenuItem = checkDealLine.selectedMenuItem;
                                var swapMenuItemWrapper = checkDealLine.selectedAllowableMenuItemWrapper;
                                var swapToppings = checkDealLine.toppings;

                                checkDealLine.selectedMenuItem = bindableDealLine.selectedMenuItem;
                                checkDealLine.selectedAllowableMenuItemWrapper = bindableDealLine.selectedAllowableMenuItemWrapper;
                                checkDealLine.toppings = bindableDealLine.toppings;

                                bindableDealLine.selectedMenuItem = swapMenuItem;
                                bindableDealLine.selectedAllowableMenuItemWrapper = swapMenuItemWrapper;
                                bindableDealLine.toppings = swapToppings;

                                // Since we've moved the selected menu item to a different deal line we need to continue checking from 
                                // the new deal line
                                bindableDealLine = checkDealLine;

                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    this.getDealLineTemplateName = function (bindableDealLine)
    {
        return bindableDealLine == undefined ? 'popupDealLine-template' : bindableDealLine.dealLineWrapper.templateName;
    }
    this.subscribeToDealLineChanges = function ()
    {
        self.ignoreDealLineChanges = true;

        for (var index = 0; index < self.cartDeal().cartDealLines().length; index++)
        {
            var dealLine = self.cartDeal().cartDealLines()[index];

            if (dealLine.selectedMenuItem !== undefined)
            {
                var selectedCategoriesText = '';
                var cat1Id = dealLine.selectedMenuItem.Cat1 === undefined ? dealLine.selectedMenuItem.Category1 : dealLine.selectedMenuItem.Cat1;
                var cat2Id = dealLine.selectedMenuItem.Cat2 === undefined ? dealLine.selectedMenuItem.Category2 : dealLine.selectedMenuItem.Cat2;

                if (cat1Id !== undefined)
                {
                    var category1 = helper.findById(cat1Id, viewModel.menu.Category1);

                    if (category1 !== undefined) selectedCategoriesText = category1.Name;
                }

                if (cat2Id !== undefined)
                {
                    var category2 = helper.findById(cat2Id, viewModel.menu.Category2);

                    if (category2 !== undefined) selectedCategoriesText += (selectedCategoriesText.length === 0 ? '' : ' ' + category2.Name);
                }

                dealLine.selectedCategoriesText(selectedCategoriesText);
            }

            // We want to know when the selected menu item changes
            self.subscriptions.push(dealLine.selectedAllowableMenuItemWrapper.subscribe(self.onSelectedAllowableMenuItemWrapperChanged));
        }

        setTimeout
        (
            function ()
            {
                self.ignoreDealLineChanges = false;
            },
            0
        );
    }
    this.unSubscribeToDealLineChanges = function ()
    {
        self.ignoreDealLineChanges = true;

        for (var index = 0; index < self.subscriptions.length; index++)
        {
            var subscription = self.subscriptions[index];
            subscription.dispose();
        }

        self.ignoreDealLineChanges = false;
    }
    
    this.onSelectedAllowableMenuItemWrapperChanged = function (allowableMenuItemWrapper)
    {
        if (self.ignoreDealLineChanges) return;

        self.ignoreDealLineChanges = true;

        allowableMenuItemWrapper.cartDealLine.currentAllowableMenuItemWrapper = allowableMenuItemWrapper.cartDealLine.selectedAllowableMenuItemWrapper();

        // Make sure the cart deal line state is up to date after the change
        allowableMenuItemWrapper.cartDealLine.refreshSelectedAllowableMenuItemWrapper();

        // Are there any toppings or selectable cats?
        if (allowableMenuItemWrapper.cartDealLine.selectedMenuItem.OptTopIds.length > 0 ||
            allowableMenuItemWrapper.cartDealLine.selectedMenuItem.DefTopIds.length > 0 ||
            (allowableMenuItemWrapper.cartDealLine.selectedAllowableMenuItemWrapper().menuItemWrapper.category1s() !== undefined &&
            allowableMenuItemWrapper.cartDealLine.selectedAllowableMenuItemWrapper().menuItemWrapper.category1s().length > 1) ||
            (allowableMenuItemWrapper.cartDealLine.selectedAllowableMenuItemWrapper().menuItemWrapper.category2s() !== undefined &&
            allowableMenuItemWrapper.cartDealLine.selectedAllowableMenuItemWrapper().menuItemWrapper.category2s().length > 1))
        {
            // Show the toppings popup
            self.showToppingsPopupForCartDealLine(allowableMenuItemWrapper.cartDealLine, 'addDealItem', true);
        }

        $('#' + allowableMenuItemWrapper.cartDealLine.dealLineWrapper.id).blur();

        self.ignoreDealLineChanges = false;
    }

    this.customizeDealLine = function (cartDealLine)
    {
        self.showToppingsPopupForCartDealLine(cartDealLine, 'addDealItem', false);
    }

    this.showToppingsPopupForCartDealLine = function (cartDealLine, mode, onlyShowIfToppings)
    {
        // The newly selected item
        var selectedAllowableMenuItemWrapper = cartDealLine.selectedAllowableMenuItemWrapper();
        
        if (!onlyShowIfToppings ||
            (selectedAllowableMenuItemWrapper != undefined && selectedAllowableMenuItemWrapper.name != textStrings.pleaseSelectAnItem))
        {
            // We don't need to know about deal line changes any more
            self.unSubscribeToDealLineChanges();
            
            setTimeout
            (
                function ()
                {
                    // Show the toppings popup
                    toppingsPopupHelper.showPopup
                    (
                        cartDealLine.cartItem(),
                        true,
                        true,
                        {
                            cartDeal: self.cartDeal(),
                            cartDealLine: cartDealLine,
                            mode: self.mode()
                        }
                    );
                },
                0
            );
        }
    }

    // MUST DO THIS LAST SO THE METHOD HAS BEEN DEFINED
    // Allow knockout to do it's thing before subscribing to events
    setTimeout(this.subscribeToDealLineChanges(), 0);
};





























