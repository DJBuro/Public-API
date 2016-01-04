/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function RepeatOrderViewModel()
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

    if (viewModel.contentViewModel() != undefined && viewModel.contentViewModel().previousViewName != undefined && viewModel.contentViewModel().previousViewName.length > 0)
    {
        self.previousViewName = viewModel.contentViewModel().previousViewName;
        self.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
    }
    else
    {
        self.previousViewName = guiHelper.getCurrentViewName();
        self.previousContentViewModel = viewModel.contentViewModel();
    }

    self.onShown = function ()
    {
    };

    self.onLogout = function ()
    {
        this.backToMenu();
        if (typeof (self.previousContentViewModel.onLogout) == 'function')
        {
            self.previousContentViewModel.onLogout();
        }
    };

    self.back = function ()
    {
        guiHelper.showView(self.previousViewName, self.previousContentViewModel);
    };

    self.repeatOrder = function ()
    {
        // Add each order line to the cart
        for (var orderLineIndex = 0; orderLineIndex < myOrdersHelper.repeatOrder().orderLines.length; orderLineIndex++)
        {
            var orderLine = myOrdersHelper.repeatOrder().orderLines[orderLineIndex];

            // Is there a menu item to add to the cart?
            if (orderLine.menuItem == undefined) continue;

            // Create a new item to add to the cart
            var cartItem =
            {
                menuItem: orderLine.menuItem.menuItem,
                name: orderLine.menuItem.name,
                displayName: ko.observable(orderLine.orderLine.quantity + ' x ' + orderLine.orderLine.name),
                quantity: ko.observable(orderLine.orderLine.quantity),
                displayPrice: ko.observable(helper.formatPrice(menuHelper.getItemPrice(orderLine.menuItem.menuItem))),
                price: menuHelper.getItemPrice(orderLine.menuItem.menuItem),
                instructions: orderLine.orderLine.chefNotes,
                person: orderLine.orderLine.person,
                toppings: ko.observableArray(), //viewModel.selectedItem.toppings(),
                displayToppings: ko.observableArray(),
                selectedCategory1: undefined, //viewModel.selectedItem.selectedCategory1(),
                selectedCategory2: undefined, //viewModel.selectedItem.selectedCategory2(),
                category1s: ko.observableArray(),
                category2s: ko.observableArray(),
                isEnabled: ko.observable(),
                menuItems: undefined
            };

            // Copy over the menu items
            cartItem.menuItems = [];
            cartItem.menuItems.push(orderLine.menuItem.menuItem);

            // Add the item to the cart
            cartHelper.cart().cartItems.push(cartItem);
        }

        // Recalculate the cart price
        cartHelper.refreshCart();

        // Exit the repeat order page
        guiHelper.showMenu();
    };
};






































