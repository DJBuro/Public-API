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
    customerHelper.bindableCustomer(new AndroWeb.Models.Customer());

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

     //       self.sortDealLinesCheapestLast();

            // Recalculate the cart price
            AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());

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
        AndroWeb.Helpers.CartHelper.cart().deals.remove(self.cartDeal());

        // Recalculate the total price
        AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());

        // Hide the deal pop-up
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

         //   self.sortDealLinesCheapestLast();

            AndroWeb.Helpers.CartHelper.cart().deals.push(self.cartDeal());

            // Recalculate the cart price
            AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());

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
                        if (bindableDealLine.cartItem().menuItem.Id == allowedItem)
                        {
                            // This deal line's list of allowable menu items does contain the target menu item

                            // Check if we need to swap them

                            // Calculate deal line price
                            var baseDealLinePrice = menuHelper.calculateDealLinePrice(bindableDealLine, true);
                            var additionalDealLineCost = menuHelper.calculateDealItemAdditionalCosts
                            (
                                bindableDealLine,
                                bindableDealLine.cartItem().menuItem,
                                bindableDealLine.cartItem().toppings(),
                                true
                            );
                            var dealLinePrice = baseDealLinePrice + additionalDealLineCost;

                            // Calculate deal line price to compare to
                            var baseCheckDealLinePrice = menuHelper.calculateDealLinePrice(checkDealLine, true);
                            var additionalCheckDealLineCost = menuHelper.calculateDealItemAdditionalCosts
                            (
                                checkDealLine,
                                checkDealLine.cartItem().menuItem,
                                checkDealLine.cartItem().toppings(),
                                true
                            );
                            var checkDealLinePrice = baseCheckDealLinePrice + additionalCheckDealLineCost;

                            if (dealLinePrice > checkDealLinePrice)
                            {
                                // We need to switch the menu items around so the more expensive one is first

                                var swapMenuItem = checkDealLine.cartItem().menuItem;
                                var swapMenuItemWrapper = checkDealLine.selectedAllowableMenuItemWrapper;
                                var swapToppings = checkDealLine.cartItem().toppings();

                                checkDealLine.cartItem().menuItem = bindableDealLine.cartItem().menuItem;
                                checkDealLine.selectedAllowableMenuItemWrapper = bindableDealLine.selectedAllowableMenuItemWrapper;
                                checkDealLine.cartItem().toppings() = bindableDealLine.cartItem().toppings();

                                bindableDealLine.cartItem().menuItem = swapMenuItem;
                                bindableDealLine.selectedAllowableMenuItemWrapper = swapMenuItemWrapper;
                                bindableDealLine.cartItem().toppings() = swapToppings;

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

            if (dealLine.cartItem() !== undefined)
            {
                var selectedCategoriesText = '';
                var cat1Id = dealLine.cartItem().menuItem.Cat1 === undefined ? dealLine.cartItem().menuItem.Category1 : dealLine.cartItem().menuItem.Cat1;
                var cat2Id = dealLine.cartItem().menuItem.Cat2 === undefined ? dealLine.cartItem().menuItem.Category2 : dealLine.cartItem().menuItem.Cat2;

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

        // Make sure the cart deal line state is up to date after the change
        allowableMenuItemWrapper.cartDealLine.refreshSelectedAllowableMenuItemWrapper(allowableMenuItemWrapper.cartDealLine.dealLineWrapper.dealLine); // Creates a new CartItem

        // Are there any toppings or selectable cats?  If so we need to show the toppings popup
        if (allowableMenuItemWrapper.cartDealLine.cartItem().menuItem.OptTopIds.length > 0 ||
            allowableMenuItemWrapper.cartDealLine.cartItem().menuItem.DefTopIds.length > 0 ||
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
        if (!onlyShowIfToppings ||
            (cartDealLine.selectedAllowableMenuItemWrapper() != undefined &&
             cartDealLine.selectedAllowableMenuItemWrapper().name != textStrings.pleaseSelectAnItem))
        {
            // We don't need to know about deal line changes any more
            self.unSubscribeToDealLineChanges();
            
            // Build a CartItem that the toppings popup will use 
            // If the customer accepts the toppings then we will copy the selected toppings etc... into the deal lines CartItem
            // Otherwise, if the customer cancels we just keep the existing cartitem in the deal line
            var toppingPopupCartItem = new AndroWeb.Models.CartItem
            (
                viewModel,
                cartDealLine.cartItem().itemWrapper,
                cartDealLine.cartItem().menuItem,
                true,
                (cartDealLine.dealLineWrapper.dealLine.Type.toLowerCase() != "percentage")
            );

            // Set the topping quantities of the cartitem we're going to send to the toppings popup
            for (var index = 0; index < cartDealLine.cartItem().toppings().length; index++)
            {
                var currentTopping = cartDealLine.cartItem().toppings()[index];

                var toppingPopupTopping = toppingPopupCartItem.toppingsIndex[currentTopping.topping.Id];
                if (toppingPopupTopping !== undefined)
                {
                    toppingPopupTopping.quantity(currentTopping.quantity());
                    toppingPopupTopping.selectedSingle(currentTopping.selectedSingle());
                    toppingPopupTopping.selectedDouble(currentTopping.selectedDouble());
                }
            }

            // If there is a single allowable menu item and it's already selected make it the previously selected   
            // menu item so cancel will work
            if (cartDealLine.dealLineWrapper.dealLine.AllowableItemsIds.length == 1)
            {
                // Keep hold of the previously selected allowableMenuWrapper so that we can revert back to later if needed
                cartDealLine.previouslySelectedAllowableMenuItemWrapper = cartDealLine.selectedAllowableMenuItemWrapper();
                cartDealLine.previousCartItem = cartDealLine.cartItem();
            }

            setTimeout
            (
                function ()
                {
                    // Show the toppings popup
                    toppingsPopupHelper.showPopup
                    (
                        toppingPopupCartItem,
                        true,
                        true,
                        new DealDetails(self.cartDeal(), cartDealLine, self.mode())
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

function DealDetails(cartDeal, cartDealLine, mode)
{
    "use strict";

    var self = this;

    this.cartDeal = cartDeal;
    this.cartDealLine = cartDealLine;
    this.mode = mode;

    this.commitChanges = function (cartItem)
    {
        // Copy the cart item details to commit into the deal lines cart item
        self.cartDealLine.cartItem().menuItem = cartItem.menuItem;
        self.cartDealLine.cartItem().itemWrapper = cartItem.itemWrapper;
        self.cartDealLine.cartItem().selectedCategory1(cartItem.selectedCategory1());
        self.cartDealLine.cartItem().selectedCategory2(cartItem.selectedCategory2());

        // Since different menu items can have different toppings we need to refresh the deal cart item toppings
        self.cartDealLine.cartItem().initialiseToppings();

        // Set the topping quantities of the cartitem we're going to send to the toppings popup
        for (var index = 0; index < self.cartDealLine.cartItem().toppings().length; index++)
        {
            var dealTopping = self.cartDealLine.cartItem().toppings()[index];

            var toppingId = typeof (dealTopping.topping.Id) === 'number' ? dealTopping.topping.Id.toString() : dealTopping.topping.Id;
            var toppingPopupTopping = cartItem.toppingsIndex[toppingId];
            if (toppingPopupTopping !== undefined)
            {
                dealTopping.quantity(toppingPopupTopping.quantity());
                dealTopping.selectedSingle(toppingPopupTopping.selectedSingle());
                dealTopping.selectedDouble(toppingPopupTopping.selectedDouble());
            }
        }

        // Make sure the cart item prices are updated
        self.cartDealLine.cartItem().recalculatePrice();

        // Keep hold of the previously selected allowableMenuWrapper so that we can revert back to later if needed
        self.cartDealLine.previouslySelectedAllowableMenuItemWrapper = self.cartDealLine.selectedAllowableMenuItemWrapper();
        self.cartDealLine.previousCartItem = self.cartDealLine.cartItem();
    }

    this.cancelChanges = function (cartItem)
    {
        // Revert back to the originally selected menu item wrapper in the drop down combo
        self.ignoreDealLineChanges = true;
        self.cartDealLine.selectedAllowableMenuItemWrapper(self.cartDealLine.previouslySelectedAllowableMenuItemWrapper);
        self.cartDealLine.cartItem(self.cartDealLine.previousCartItem);
    }
}



























