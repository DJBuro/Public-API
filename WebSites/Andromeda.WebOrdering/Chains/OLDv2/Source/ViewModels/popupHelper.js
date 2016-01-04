var popupHelper =
{
    returnToCart: false,
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    mode: ko.observable('addItem'),
    cancel: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    cancelDeal: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();

        // Show the deal popup
        dealPopupHelper.showDealPopup('addDeal', false);

        if (popupHelper.mode() == 'addDealItem')
        {
            // Were there multiple items to pick from?
            if (dealHelper.selectedBindableDealLine.itemWrappers.length > 1)
            {
                // Was an item already selected?
                if (dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper == undefined)
                {
                    // Display the "please select an item" item
                    dealHelper.selectedBindableDealLine.selectedItemWrapper(dealHelper.selectedBindableDealLine.itemWrappers[0]);
                    dealHelper.selectedBindableDealLine.selectedMenuItem = undefined;
                }
                else
                {
                    // Restore the previously selected item
                    dealHelper.selectedBindableDealLine.selectedItemWrapper(dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper);
                }
            }

            // Re-scbscribe to deal line changes
            dealHelper.subscribeToDealLineChanges();
        }
    },
    commitToDeal: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();

        // Show the deal popup
        dealPopupHelper.showDealPopup('addDeal', false);

        // Keep hold of the currently selected item so next time they change it but cancel we can set it back
        dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper = dealHelper.selectedBindableDealLine.selectedItemWrapper();

        // Record which menu item was selected
        dealHelper.selectedBindableDealLine.selectedMenuItem = viewModel.selectedItem.menuItem();

        // Check to see if any errors have now been resolved
        dealHelper.checkForErrors();

        dealHelper.selectedBindableDealLine.toppings = dealHelper.cloneToppings(viewModel.selectedItem.toppings());
        dealHelper.selectedBindableDealLine.instructions = viewModel.selectedItem.instructions();
        dealHelper.selectedBindableDealLine.person = viewModel.selectedItem.person();

        // Resubsribe to deal line changes
        dealHelper.subscribeToDealLineChanges();
    },
    acceptChanges: function ()
    {
        // Update the quantity
        cartHelper.cart().selectedCartItem().quantity(viewModel.selectedItem.quantity());

        // Update the cat1
        cartHelper.cart().selectedCartItem().selectedCategory1 = viewModel.selectedItem.selectedCategory1();

        // Update the cat2
        cartHelper.cart().selectedCartItem().selectedCategory2 = viewModel.selectedItem.selectedCategory2();

        // Update the menu item (will change if a different cat1 / cat2 are selected)
        cartHelper.cart().selectedCartItem().menuItem = viewModel.selectedItem.menuItem();

        // Update the chefs instructions
        cartHelper.cart().selectedCartItem().instructions = viewModel.selectedItem.instructions();

        // Update the person
        cartHelper.cart().selectedCartItem().person = viewModel.selectedItem.person();

        // Update the toppings
        var cartToppings = [];
        for (var index = 0; index < viewModel.selectedItem.toppings().length; index++)
        {
            var topping = viewModel.selectedItem.toppings()[index];

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
        cartHelper.cart().selectedCartItem().toppings.removeAll();
        cartHelper.cart().selectedCartItem().toppings(cartToppings);

        // Update the display name
        cartHelper.cart().selectedCartItem().displayName(menuHelper.getCartItemDisplayName(viewModel.selectedItem, viewModel.selectedItem.selectedCategory1(), viewModel.selectedItem.selectedCategory2()));

        // Recalculate the item price
        var price = menuHelper.calculateItemPrice(cartHelper.cart().selectedCartItem().menuItem, cartHelper.cart().selectedCartItem().quantity(), viewModel.selectedItem.toppings());
        cartHelper.cart().selectedCartItem().price = price;
        cartHelper.cart().selectedCartItem().displayPrice(helper.formatPrice(price));

        cartHelper.cart().selectedCartItem().displayToppings(menuHelper.getCartDisplayToppings(cartToppings));

        // Recalculate the total price of all items in the cart
        cartHelper.refreshCart();

        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    removeFromCart: function ()
    {
        // Remove the item from the cart
        cartHelper.cart().cartItems.remove(cartHelper.cart().selectedCartItem());

        // Recalculate the total price
        cartHelper.refreshCart();

        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    quantityChanged: function ()
    {
        var price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
        viewModel.selectedItem.price(helper.formatPrice(price));

        return true;
    },
    singleChanged: function ()
    {
        popupHelper.popupToppingChanged('single', this);

        return true;
    },
    doubleChanged: function ()
    {
        popupHelper.popupToppingChanged('double', this);

        return true;
    },
    showPopup: function (mode)
    {
        $(window).scrollTop(0);

        // Show the toppings popup
        popupHelper.mode(mode);
        viewModel.pickToppings(true);

        popupHelper.isBackgroundVisible(true);
        popupHelper.isPopupVisible(true);

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
        }

        // Give knockout time to do its thing (Javascript doesn't do not proper multi-threading - need to let the browser have the thread back)
        setTimeout
        (
            function ()
            {
                // Recalculate the item price
                var price = 0;
                if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
                {
                    price = menuHelper.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings(), true);
                }
                else
                {
                    price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
                }

                viewModel.selectedItem.price(helper.formatPrice(price));
            },
            0
        );
    },
    hidePopup: function ()
    {
        popupHelper.isBackgroundVisible(false);
        popupHelper.isPopupVisible(false);

        // We might be on top of the deal popup
        if (!dealPopupHelper.isPopupVisible())
        {
            guiHelper.isInnerMenuVisible(true);

            if (guiHelper.isMobileMode())
            {
                // In mobile mode the cart is a view and not a popup
                if (popupHelper.returnToCart)
                {
                    // Show the view
                    guiHelper.isViewVisible(true);
                    guiHelper.isInnerMenuVisible(false);
                }
                else
                {
                    // Show the menu
                    guiHelper.isViewVisible(false);
                    guiHelper.isInnerMenuVisible(true);
                }
            }
        }

        // Let knockout do its thing
        setTimeout
        (
            function ()
            {
                guiHelper.resize();
            },
            0
        );
    },
    commitToCart: function ()
    {
        var price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), false, true);

        // Get the item name to display in the cart
        var name = cartHelper.getCartItemName(viewModel.selectedItem, viewModel.selectedItem.selectedCategory1(), viewModel.selectedItem.selectedCategory2());

        // Copy the toppings to the cart object.  We have to actually clone the topping objects - if we just copy the object references it'll get screwed up later
        var cartToppings = [];
        for (var index = 0; index < viewModel.selectedItem.toppings().length; index++)
        {
            var topping = viewModel.selectedItem.toppings()[index];

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
            menuItem: viewModel.selectedItem.menuItem(),
            name: viewModel.selectedItem.name(),
            displayName: ko.observable(menuHelper.getCartItemDisplayName(viewModel.selectedItem)),
            quantity: ko.observable(viewModel.selectedItem.quantity()),
            displayPrice: ko.observable(helper.formatPrice(price)),
            price: price,
            instructions: viewModel.selectedItem.instructions(),
            person: viewModel.selectedItem.person(),
            toppings: viewModel.selectedItem.toppings(),
            displayToppings: ko.observableArray(menuHelper.getCartDisplayToppings(cartToppings)),
            selectedCategory1: viewModel.selectedItem.selectedCategory1(),
            selectedCategory2: viewModel.selectedItem.selectedCategory2(),
            category1s: ko.observableArray(),
            category2s: ko.observableArray(),
            toppings: ko.observableArray(cartToppings),
            isEnabled: ko.observable(),
            menuItems: undefined
        };

        // Copy over the cat1 and cat2s
        viewModel.ignoreEvents = true;
        for (var index = 0; index < viewModel.selectedItem.category1s().length; index++)
        {
            cartItem.category1s.push(viewModel.selectedItem.category1s()[index]);
        }

        for (var index = 0; index < viewModel.selectedItem.category2s().length; index++)
        {
            cartItem.category2s.push(viewModel.selectedItem.category2s()[index]);
        }
        viewModel.ignoreEvents = false;

        // Copy over the menu items
        cartItem.menuItems = [];
        for (var index = 0; index < viewModel.selectedItem.menuItems().length; index++)
        {
            cartItem.menuItems.push(viewModel.selectedItem.menuItems()[index]);
        }

        // Add the item to the cart
        cartHelper.cart().cartItems.push(cartItem);

        // Recalculate the cart price
        cartHelper.refreshCart();

        // Hide the toppings popup
        popupHelper.hidePopup();

        // If we are in mobile mode show the cart
        if (guiHelper.isMobileMode())
        {
            guiHelper.showCart();
        }
    },
    popupToppingChanged: function (singleOrDouble, topping)
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

        if (topping.type == 'removable')
        {
            // Is the topping removed?
            if (!topping.selectedDouble() && !topping.selectedSingle())
            {
                topping.quantity = -1;
            }
                // Is the topping being doubled?
            else if (topping.selectedDouble())
            {
                // This topping is already on the item so doubling up just adds a single topping
                topping.quantity = 1;
            }
        }
        else
        {
            if (topping.selectedSingle())
            {
                topping.quantity = 1;
            }
            else if (topping.selectedDouble())
            {
                topping.quantity = 2;
            }
        }

        // Recalculate the item price
        var price = 0;
        if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
        {
            price = menuHelper.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings(), true);
        }
        else
        {
            price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
        }

        viewModel.selectedItem.price(helper.formatPrice(price));

        // Refresh free toppings
        popupHelper.refeshFreeToppings();
    },
    refeshFreeToppings: function ()
    {
        var remainingToppings = 0;

        if (viewModel.selectedItem.freeToppings() > 0)
        {
            // Does the item have any toppings?
            if (viewModel.selectedItem.toppings() != undefined)
            {
                var usedToppings = 0;

                for (var index = 0; index < viewModel.selectedItem.toppings().length; index++)
                {
                    var topping = viewModel.selectedItem.toppings()[index];

                    if (topping.type == 'removable')
                    {
                        // The customer has upgraded an included topping toa double
                        if (topping.selectedDouble())
                        {
                            usedToppings++;
                        }
                    }
                    else
                    {
                        if (topping.selectedDouble())
                        {
                            // Customer has added 2 of the topping
                            usedToppings += 2;
                        }
                        else if (topping.selectedSingle())
                        {
                            // Customer has a topping
                            usedToppings++;
                        }
                    }
                }

                remainingToppings = viewModel.selectedItem.freeToppings() - usedToppings;

                if (remainingToppings < 0) remainingToppings = 0;
            }
        }

        // Update UI
        viewModel.selectedItem.freeToppingsRemaining(remainingToppings);
    },
    popupCategory1Changed: function ()
    {
        if (!viewModel.ignoreEvents && viewModel.pickToppings())
        {
            viewModel.category1Changed(viewModel.selectedItem);

            // Get the menu item that the user has selected
            var menuItem = menuHelper.getSelectedMenuItem(viewModel.selectedItem);

            if (menuItem == undefined) return;

            // Change the selected menu item
            viewModel.selectedItem.menuItem(menuItem);
            viewModel.selectedItem.toppings(menuHelper.getItemToppings(viewModel.selectedItem.menuItem()));

            // Recalculate the item price
            var price = 0;
            if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
            {
                price = menuHelper.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings());
            }
            else
            {
                price = menuHelper.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings());
            }

            viewModel.selectedItem.price(helper.formatPrice(price));
        }
    },
    popupSelectedItemChanged: function ()
    {
        if (!viewModel.ignoreEvents && viewModel.pickToppings())
        {
            // Get the menu item that the user has selected
            var menuItem = menuHelper.getSelectedMenuItem(viewModel.selectedItem);

            // Change the selected menu item
            viewModel.selectedItem.menuItem(menuItem);
            viewModel.selectedItem.toppings(menuHelper.getItemToppings(viewModel.selectedItem.menuItem()));

            if (popupHelper.mode() == 'addItem' || popupHelper.mode() == 'editItem')
            {
                guiHelper.selectedItemChanged(viewModel.selectedItem);
            }
            else
            {
                // Recalculate the item price
                var price = menuHelper.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings());

                viewModel.selectedItem.price(helper.formatPrice(price));
            }
        }
    }
}