/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function CartDealLine(dealLineWrapper, id)
{
    "use strict";

    var self = this;

    this.dealLineWrapper = dealLineWrapper;
    this.allowableMenuItemWrappers = [];
    this.selectedAllowableMenuItemWrapper = ko.observable(undefined); // This is the selected item but NOT the specific menu item variation e.g. Size
    this.previouslySelectedAllowableMenuItemWrapper = undefined;
    this.selectedMenuItem = undefined, // This is the ACTUAL menu item selected - each item could include multiple menu items for different cat1/cat2s - this is the menu item the customer wants
    this.hasError = ko.observable(false);
    this.instructions = '';
    this.person = '';
    this.id = id;
    this.cartItem = ko.observable();
    this.name = ko.observable('');
    this.displayPrice = ko.observable('');
    this.categoryPremium = ko.observable();
    this.categoryPremiumName = ko.observable();
    this.itemPremium = ko.observable();
    this.itemPremiumName = ko.observable();
    this.selectedCategoriesText = ko.observable('');
    this.displayName = ko.observable('');

    // Build the allowable items in the deal line
    for (var itemIndex = 0; itemIndex < dealLineWrapper.menuItemWrappers.length; itemIndex++)
    {
        var menuItemWrapper = dealLineWrapper.menuItemWrappers[itemIndex];
        var allowableMenuItemWrapper =
        {
            cartDealLine: this, // The deal line that the item is in - we're data binding to the item but we need a way to get back to the deal line the item is in
            menuItemWrapper: menuItemWrapper,
            name: menuItemWrapper.name
        };

        this.allowableMenuItemWrappers.push(allowableMenuItemWrapper);
    }

    this.refreshSelectedAllowableMenuItemWrapper = function()
    {
        // Each menu item wrapper can contain multiple menu items (sizes) - default to the first
        if (self.selectedAllowableMenuItemWrapper() === undefined)
        {
            // The "please select an item" option was selected - not a readl menu item
            self.selectedMenuItem = undefined;
            self.cartItem(undefined);
        }
        else
        {
            if (self.selectedMenuItem === undefined)
            {
                self.selectedMenuItem = self.selectedAllowableMenuItemWrapper().menuItemWrapper.menuItems()[0];
            }

            // Create a cart item for the selected menu item
            var cartItem = new CartItem(self.selectedAllowableMenuItemWrapper().menuItemWrapper, self.selectedMenuItem, true);
            self.cartItem(cartItem);
        }
    }

    // Auto select the first menu item if there is only one
    if (this.allowableMenuItemWrappers.length == 1)
    {
        // Select the first menu item
        this.selectedAllowableMenuItemWrapper(this.allowableMenuItemWrappers[0]);

        // Make sure the cart deal line state is upto date after the change
        this.refreshSelectedAllowableMenuItemWrapper();
    }
};































