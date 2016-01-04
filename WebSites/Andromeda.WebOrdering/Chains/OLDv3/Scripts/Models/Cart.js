/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function cart()
{
    var self = this;

    self.orderType = ko.observable('delivery');    
    self.canChangeOrderType = ko.observable(true);
    self.cartActions = guiHelper.cartActions();
    self.cartActionsCheckout = guiHelper.cartActionsCheckout;
    self.cartActionsPayment = guiHelper.cartActionsPayment;
    self.cartItems = ko.observableArray();
    self.deals = ko.observableArray();
    self.displayTotalPrice = ko.observable();
    self.totalPrice = ko.observable();
    self.selectedCartItem = ko.observable({});
    self.mercuryPaymentId = ko.observable();
    self.dataCashPaymentDetails = ko.observable();
    self.areDisabledItems = ko.observable();
    self.checkoutEnabled = ko.observable();
    self.hasItems = ko.observable();

    self.refreshCart = function ()
    {
        // Update the deals availability (depending on whether it's a collection or delivery order)
        app.site().menu.refreshDealsAvailabilty();

        // Get the order value not including deals with a minimum order value
        var orderTotalForMinimumOrderValue = self.getOrderValueExcludingDealsWithAMinimumPrice();

        var totalPrice = 0;
        var areDisabledItems = false;
        var checkoutEnabled = false;

        // Update the deal prices in the cart
        for (var index = 0; index < self.deals().length; index++)
        {
            var deal = self.deals()[index];

            // Has the minimum order value been met?
            var minimumOrderValueOk = orderTotalForMinimumOrderValue >= deal.deal.deal.MinimumOrderValue;
            deal.minimumOrderValueNotMet(!minimumOrderValueOk);

            // Is the deal enabled in the cart?
            deal.isEnabled(app.site().menu.isDealItemEnabledForCart(deal));

            if (deal.isEnabled())
            {
                checkoutEnabled = true;
            }
            else
            {
                areDisabledItems = true;
            }

            // Set the display price depending on the current order type
            deal.displayPrice(helper.formatPrice(deal.price));

            var dealPrice = 0;

            // Does the deal have any deal lines?
            if (deal.dealLines != undefined)
            {
                // Update the prices of the deal lines in the deal
                for (var dealLineIndex = 0; dealLineIndex < deal.dealLines().length; dealLineIndex++)
                {
                    var dealLine = deal.dealLines()[dealLineIndex];
                    var dealLinePrice = app.site().menu.calculateDealLinePrice(dealLine);

                    // Get the categories
                    var category1 = helper.findById(dealLine.selectedMenuItem.Cat1 == undefined ? dealLine.selectedMenuItem.Category1 : dealLine.selectedMenuItem.Cat1, app.site().menu.rawMenu.Category1);
                    var category2 = helper.findById(dealLine.selectedMenuItem.Cat2 == undefined ? dealLine.selectedMenuItem.Category2 : dealLine.selectedMenuItem.Cat2, app.site().menu.rawMenu.Category2);

                    // Add category deal premiums
                    var categoryPremium = ((category1 == undefined ? 0 : category1.DealPremium) + (category2 == undefined ? 0 : category2.DealPremium));

                    // Set the calculated category premium
                    if (categoryPremium == undefined || categoryPremium == 0)
                    {
                        dealLine.categoryPremium(undefined);
                        dealLine.categoryPremiumName(undefined);
                    }
                    else
                    {
                        dealLine.categoryPremium(helper.formatPrice(categoryPremium));
                        dealLine.categoryPremiumName(text_premiumChargeFor.replace('{item}', category2.Name));
                    }

                    // Add item deal premium
                    var itemPremium = dealLine.selectedMenuItem.DealPricePremiumOverride == 0 ? dealLine.selectedMenuItem.DealPricePremium : dealLine.selectedMenuItem.DealPricePremiumOverride;

                    // Set the calculated item premium
                    if (itemPremium == undefined || itemPremium == 0)
                    {
                        dealLine.itemPremium(undefined);
                        dealLine.itemPremiumName(undefined);
                    }
                    else
                    {
                        dealLine.itemPremium(helper.formatPrice(premium));
                        dealLine.itemPremiumName(text_premiumChargeFor.replace('{item}', dealLine.selectedMenuItem.Name));
                    }

                    // Add the deal line to the deal price
                    dealPrice += dealLinePrice;

                    if (deal.isEnabled())
                    {
                        totalPrice += dealLinePrice + categoryPremium + itemPremium;
                    }

                    // Does the deal line have any toppings?
                    if (dealLine.displayToppings != undefined)
                    {
                        // Update the prices of the toppings in the item
                        for (var toppingIndex = 0; toppingIndex < dealLine.displayToppings().length; toppingIndex++)
                        {
                            var topping = dealLine.displayToppings()[toppingIndex];
                            var toppingPrice = app.site().menu.calculateToppingPrice(topping);
                            topping.cartPrice(helper.formatPrice(toppingPrice));

                            if (deal.isEnabled())
                            {
                                totalPrice += toppingPrice;
                            }
                        }
                    }
                }

                deal.price = dealPrice;
                deal.displayPrice(helper.formatPrice(dealPrice));
            }
        }

        // Update the item prices in the cart
        for (var index = 0; index < self.cartItems().length; index++)
        {
            var item = self.cartItems()[index];

            // Is the item enabled in the cart?
            item.isEnabled(app.site().menu.isItemEnabledForCart(item.menuItem));

            if (item.isEnabled())
            {
                checkoutEnabled = true;
            }

            // Set the price depending on the current order type
            item.price = app.site().menu.calculateItemPrice(item.menuItem, item.quantity(), undefined);

            // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
            if (item.isEnabled())
            {
                totalPrice += item.price;
            }
            else
            {
                areDisabledItems = true;
            }

            // Set the display price depending on the current order type
            item.displayPrice(helper.formatPrice(item.price));

            // Does the item have any toppings?
            if (item.displayToppings != undefined)
            {
                var toppingsPrice = 0;

                // Update the prices of the toppings in the item
                for (var toppingIndex = 0; toppingIndex < item.displayToppings().length; toppingIndex++)
                {
                    var topping = item.displayToppings()[toppingIndex];
                    var toppingPrice = app.site().menu.calculateToppingPrice(topping);
                    topping.cartPrice(helper.formatPrice(toppingPrice * item.quantity()));

                    toppingsPrice += toppingPrice;
                }

                if (item.isEnabled())
                {
                    totalPrice += (toppingsPrice * item.quantity());
                }
            }
        }

        self.areDisabledItems(areDisabledItems);

        self.displayTotalPrice(helper.formatPrice(totalPrice));
        self.totalPrice(totalPrice);

        // True when there are items (enabled or disabled) in the cart
        self.hasItems(self.cartItems().length != 0 || self.deals().length != 0);

        // Is the delivery charge met?  Don't check if already disabled
        if (checkoutEnabled && app.site().cart.orderType() == 'delivery')
        {
            checkoutEnabled = self.totalPrice() >= template_minimumDeliveryOrder;
        }

        // True when there is at least one enabled item in the cart
        self.checkoutEnabled(checkoutEnabled);
    };
    self.orderTypeChanged = function ()
    {
        // Update the cart prices and availability
        self.refreshCart();

        return true;
    };
    self.getOrderValueExcludingDealsWithAMinimumPrice = function ()
    {
        var totalPrice = 0;

        // Caclulate the deal prices
        for (var index = 0; index < self.deals().length; index++)
        {
            var deal = self.deals()[index];

            if (deal.deal.deal.MinimumOrderValue == 0)
            {
                // Does the deal have any deal lines?
                if (deal.dealLines != undefined)
                {
                    // Update the prices of the deal lines in the deal
                    for (var dealLineIndex = 0; dealLineIndex < deal.dealLines().length; dealLineIndex++)
                    {
                        var dealLine = deal.dealLines()[dealLineIndex];
                        var dealLinePrice = app.site().menu.calculateDealLinePrice(dealLine);

                        totalPrice += dealLinePrice;
                    }
                }
            }
        }

        // Calculate the item prices
        for (var index = 0; index < self.cartItems().length; index++)
        {
            var item = self.cartItems()[index];

            // Set the price depending on the current order type
            var itemPrice = app.site().menu.calculateItemPrice(item.menuItem, item.quantity(), undefined);

            // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
            if (app.site().menu.isItemAvailable(item.menuItem))
            {
                totalPrice += itemPrice;
            }

            // Does the item have any toppings?
            if (item.displayToppings != undefined)
            {
                // Update the prices of the toppings in the item
                for (var toppingIndex = 0; toppingIndex < item.displayToppings().length; toppingIndex++)
                {
                    var topping = item.displayToppings()[toppingIndex];
                    var toppingPrice = app.site().menu.calculateToppingPrice(topping);

                    if (app.site().menu.isItemAvailable(item.menuItem))
                    {
                        totalPrice += toppingPrice;
                    }
                }
            }
        }

        return totalPrice;
    };
    self.clearCart = function ()
    {
        // Clear the cart
        self.cartItems.removeAll();
        self.deals.removeAll();

        // Update the total price (probably not needed)
        self.refreshCart();

        viewModel.timer = new Date();
    };
    self.checkout = function ()
    {
        checkoutHelper.refreshTimes();

        checkoutHelper.initialiseCheckout();

        if (guiHelper.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('menu', true);
            app.viewEngine.showView('checkoutView');
        }
        else
        {
            guiHelper.showMenuView('checkoutView');
        }

        // Switch the cart to checkout mode
        guiHelper.isMobileMenuVisible(false);
        guiHelper.canChangeOrderType(false);
        guiHelper.cartActions(guiHelper.cartActionsCheckout);
    };
    self.getDealTemplate = function (cartItem)
    {
        if (guiHelper.isCartLocked())
        {
            return 'disabledCartDeal-template';
        }
        else
        {
            return 'cartDeal-template';
        }
    };
    self.getItemTemplate = function (cartItem)
    {
        if (guiHelper.isCartLocked())
        {
            return 'disabledCartItem-template';
        }
        else
        {
            return 'cartItem-template';
        }
    };
    
    self.editCartDeal = function ()
    {
        // Get the deal that the user has selected
        self.selectedCartItem(this);

        // The deal to show on the deal popup
        dealHelper.mode('editDeal');
        dealHelper.hasError(false);

        dealHelper.selectedDeal.name(this.name);
        dealHelper.selectedDeal.description(this.deal.description);

        // Build the deal lines
        dealHelper.selectedDeal.bindableDealLines.removeAll();

        for (var index = 0; index < this.dealLines.length; index++)
        {
            var dealLine = this.dealLines[index];

            var dealLineWrapper =
            {
                dealLine: dealLine.dealLine,
                items: undefined,
                selectedItem: ko.observable(),
                selectedMenuItem: dealLine.selectedMenuItem, // Each item could include multiple menu items for different cat1/cat2s - this is the menu item the customer wants
                hasError: ko.observable(false),
                toppings: dealLine.toppings
            };

            // Build the allowable items in the deal line
            var itemWrappers = [];
            for (var itemIndex = 0; itemIndex < dealLine.dealLine.items.length; itemIndex++)
            {
                var item = dealLine.dealLine.items[itemIndex];
                var itemWrapper =
                {
                    dealLine: dealLineWrapper, // The deal line that the item is in - we're data binding to the item but we need a way to get back to the deal line the item is in
                    item: itemIndex == 0 ? undefined : item, // Ignore the first item - it's always the dummy "plese select an item" item
                    name: item.name
                };

                itemWrappers.push(itemWrapper);
            }

            // Set the allowable items in the deal line
            dealLineWrapper.items = itemWrappers;

            // Add the deal line to the deal
            dealHelper.selectedDeal.dealLines.push(dealLineWrapper);
        }

        // Show the deal popup
        dealPopupHelper.returnToCart = true;  // When finished return to the cart (mobile only)
        dealPopupHelper.showDealPopup('editDeal', true);

        // Do this last as it triggers knockout
        dealHelper.selectedDeal.deal = this.deal;

        // Let knockout sort out the GUI before subscribing to events as the act of building the GUI triggers these events which we're not interested in
        setTimeout
        (
            function ()
            {
                dealHelper.subscribeToDealLineChanges();
            },
            0
        );
    };
    self.getCartItemName = function (menuItem, selectedCategory1, selectedCategory2)
    {
        // Build the item name for the cart (product name + category names)
        var itemCategoriesText = "";

        if (selectedCategory1 != undefined)
        {
            itemCategoriesText += " (" + selectedCategory1.Name;
        }

        if (selectedCategory2 != undefined)
        {
            if (itemCategoriesText.length > 0) itemCategoriesText += ", ";

            itemCategoriesText += selectedCategory2.Name;
        }

        if (itemCategoriesText.length > 0) itemCategoriesText += ")";

        return (typeof (menuItem.name) == 'function' ? menuItem.name() : menuItem.name) + itemCategoriesText;
    };
    self.showmenu = function ()
    {
    }
    self.commitToCart = function ()
    {
        var price = app.site().menu.calculateItemPrice(app.selectedItem.menuItem(), app.selectedItem.quantity(), app.selectedItem.toppings(), false, true);

        // Get the item name to display in the cart
        var name = app.site().cart.getCartItemName(app.selectedItem, app.selectedItem.selectedCategory1(), app.selectedItem.selectedCategory2());

        // Copy the toppings to the cart object.  We have to actually clone the topping objects - if we just copy the object references it'll get screwed up later
        var cartToppings = [];
        for (var index = 0; index < app.selectedItem.toppings().length; index++)
        {
            var topping = app.selectedItem.toppings()[index];

            var cartTopping =
            {
                type: topping.type,
                selectedSingle: ko.observable(topping.selectedSingle()),
                selectedDouble: ko.observable(topping.selectedDouble()),
                topping: topping.topping,
                price: topping.price,
                doublePrice: topping.doublePrice,
                freeQuantity: topping.freeQuantity,
                quantity: topping.quantity,
                cartPrice: ko.observable(''),
                cartName: ko.observable(''),
                cartQuantity: ko.observable('')
            };

            cartToppings.push(cartTopping);
        }

        // Create a new item to add to the cart
        var cartItem =
        {
            menuItem: app.selectedItem.menuItem(),
            name: app.selectedItem.name(),
            displayName: ko.observable(app.site().menu.getCartItemDisplayName(app.selectedItem)),
            quantity: ko.observable(app.selectedItem.quantity()),
            displayPrice: ko.observable(helper.formatPrice(price)),
            price: price,
            instructions: app.selectedItem.instructions(),
            person: app.selectedItem.person(),
            toppings: app.selectedItem.toppings(),
            displayToppings: ko.observableArray(app.site().menu.getCartDisplayToppings(cartToppings)),
            selectedCategory1: app.selectedItem.selectedCategory1(),
            selectedCategory2: app.selectedItem.selectedCategory2(),
            category1s: ko.observableArray(),
            category2s: ko.observableArray(app.selectedItem.category2s()),
            toppings: ko.observableArray(cartToppings),
            isEnabled: ko.observable()
        };

        for (var index = 0; index < app.selectedItem.category1s().length; index++)
        {
            cartItem.category1s.push(app.selectedItem.category1s()[index]);
        }

        for (var index = 0; index < app.selectedItem.category2s().length; index++)
        {
            cartItem.category2s.push(app.selectedItem.category2s()[index]);
        }

        // Add the item to the cart
        app.site().cart.cartItems.push(cartItem);

        // Recalculate the cart price
        app.site().cart.refreshCart();

        //// Hide the toppings popup
        //popupHelper.hidePopup();

        //// If we are in mobile mode show the cart
        //if (app.isMobileMode())
        //{
        //    guiHelper.showCart();
        //}
    };
};