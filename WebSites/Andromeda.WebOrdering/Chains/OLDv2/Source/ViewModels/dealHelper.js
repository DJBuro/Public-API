var dealHelper =
{
    returnToCart: false,
    mode: ko.observable('addDeal'),
    subscriptions: [],
    hasError: ko.observable(false),
    addToCart: function ()
    {
        // The deal to show on the deal popup
        dealHelper.mode('addDeal');
        dealHelper.hasError(false);
        dealHelper.selectedDeal.name(this.deal.DealName);
        dealHelper.selectedDeal.description(this.deal.Description);
        dealHelper.selectedDeal.deal = this.deal;

        // Build the deal lines
        dealHelper.selectedDeal.bindableDealLines.removeAll();

        for (var index = 0; index < this.dealLineWrappers.length; index++)
        {
            var dealLineWrapper = this.dealLineWrappers[index];

            var bindableDealLine =
            {
                dealLineWrapper: dealLineWrapper,
                itemWrappers: undefined,
                selectedItemWrapper: ko.observable(undefined),
                previouslySelectedItemWrapper: undefined,
                selectedMenuItem: undefined, // Each item could include multiple menu items for different cat1/cat2s - this is the menu item the customer wants
                hasError: ko.observable(false),
                toppings: undefined,
                instructions: '',
                person: '',
                id: 'dl_' + index
            };

            // Build the allowable items in the deal line
            var itemWrappers = [];
            for (var itemIndex = 0; itemIndex < dealLineWrapper.items.length; itemIndex++)
            {
                var item = dealLineWrapper.items[itemIndex];
                var itemWrapper =
                {
                    bindableDealLine: bindableDealLine, // The deal line that the item is in - we're data binding to the item but we need a way to get back to the deal line the item is in
                    item: item,
                    name: item.name
                };

                itemWrappers.push(itemWrapper);
            }

            // Set the allowable items in the deal line
            bindableDealLine.itemWrappers = itemWrappers;

            // Auto select the first item if there is only one
            if (bindableDealLine.itemWrappers.length == 1)
            {
                bindableDealLine.selectedItemWrapper(bindableDealLine.itemWrappers[0]);
                bindableDealLine.selectedMenuItem = bindableDealLine.selectedItemWrapper().item.menuItems()[0];

                // We'll also need the selected items toppings
                bindableDealLine.toppings = menuHelper.getItemToppings(bindableDealLine.selectedMenuItem);
            }

            // Add the deal line to the deal
            dealHelper.selectedDeal.bindableDealLines.push(bindableDealLine);
        }

        // Show the deal popup
        dealPopupHelper.returnToCart = false;  // If the user cancels show the menu not the cart (mobile only)
        dealPopupHelper.showDealPopup('addDeal', true);

        // Let knockout sort out the GUI before subscribing to events as the act of building the GUI triggers these events which we're not interested in
        setTimeout
        (
            function ()
            {
                dealHelper.subscribeToDealLineChanges();
            },
            0
        );
    },
    subscribeToDealLineChanges: function ()
    {
        for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
        {
            // We want to know when the selected menu item changes
            dealHelper.subscriptions.push(dealHelper.selectedDeal.bindableDealLines()[index].selectedItemWrapper.subscribe(dealHelper.selectedDealLineChanged));
        }
    },
    unSubscribeToDealLineChanges: function ()
    {
        for (var index = 0; index < dealHelper.subscriptions.length; index++)
        {
            var subscription = dealHelper.subscriptions[index];
            subscription.dispose();
        }
    },
    selectedDeal:
    {
        name: ko.observable(''),
        description: ko.observable(''),
        bindableDealLines: ko.observableArray(),
        deal: undefined
    },
    selectedBindableDealLine: undefined, // This is used to temporarily hold the deal line that is currently being customized in the toppings popup so when the popup is closed we know which deal line to copy the customization to
    dealPopupCancel: function ()
    {
        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    getDealLineTemplateName: function (bindableDealLine)
    {
        return bindableDealLine == undefined ? 'popupDealLine-template' : bindableDealLine.dealLineWrapper.templateName;
    },
    selectedDealLineChanged: function (itemWrapper)
    {
        if (itemWrapper.bindableDealLine != undefined)
        {
            // Different item - different toppings
            itemWrapper.bindableDealLine.toppings = undefined;

            // Different item - different menu item
            itemWrapper.bindableDealLine.menuItem = undefined; //itemWrapper.item.menuItems[1];
            itemWrapper.bindableDealLine.selectedMenuItem = undefined; //itemWrapper.item.menuItems[1];

            // Show the toppings popup
            dealHelper.showToppingsPopupForDealLine(itemWrapper.bindableDealLine, 'addDealItem', true);

            $('#' + itemWrapper.bindableDealLine.dealLineWrapper.id).blur();
        }
    },
    customizeDealLine: function (bindableDealLine)
    {
        dealHelper.showToppingsPopupForDealLine(bindableDealLine, 'addDealItem', false);
    },
    showToppingsPopupForDealLine: function (bindableDealLine, mode, onlyShowIfToppings)
    {
        // The newly selected item
        var selectedItemWrapper = bindableDealLine.selectedItemWrapper();

        if (selectedItemWrapper != undefined && selectedItemWrapper.name != textStrings.pleaseSelectAnItem)
        {
            // We don't need to know about deal line changes any more
            dealHelper.unSubscribeToDealLineChanges();

            // Get the price of the deal line (luckily, we keep a reference to it)
            bindableDealLine.price = menuHelper.calculateDealLinePrice(selectedItemWrapper.bindableDealLine.dealLineWrapper.dealLine);

            // Store for later
            dealHelper.selectedBindableDealLine = bindableDealLine;

            // Each allowable item in a deal line could consist of multiple menu items e.g. different sizes.  Default to the first one
            var item = bindableDealLine.selectedItemWrapper().item;

            if (bindableDealLine.selectedMenuItem == undefined)
            {
                // Default to the first menu item
                bindableDealLine.selectedMenuItem = item.menuItems()[0];
            }

            // Get the categories
            var category1 = helper.findById(bindableDealLine.selectedMenuItem.Cat1 == undefined ? bindableDealLine.selectedMenuItem.Category1 : bindableDealLine.selectedMenuItem.Cat1, viewModel.menu.Category1);
            var category2 = helper.findById(bindableDealLine.selectedMenuItem.Cat2 == undefined ? bindableDealLine.selectedMenuItem.Category2 : bindableDealLine.selectedMenuItem.Cat2, viewModel.menu.Category2);

            // The item to show on the toppings popup
            viewModel.selectedItem.name(item.name);

            // Copy over the menu items
            viewModel.selectedItem.menuItems.removeAll();
            for (var index = 0; index < item.menuItems().length; index++)
            {
                viewModel.selectedItem.menuItems.push(item.menuItems()[index]);
            }

            viewModel.selectedItem.description(item.description);
            viewModel.selectedItem.quantity(1);
            viewModel.selectedItem.menuItem(bindableDealLine.selectedMenuItem);
            viewModel.selectedItem.freeToppings(bindableDealLine.selectedMenuItem.FreeTops);
            // Free deal tops????
            viewModel.selectedItem.freeToppingsRemaining(bindableDealLine.selectedMenuItem.FreeTops);
            viewModel.selectedItem.instructions(bindableDealLine.instructions);
            viewModel.selectedItem.person(bindableDealLine.person);
            viewModel.selectedItem.price(helper.formatPrice(0));

            // Copy over the cat1 and cat2s
            viewModel.ignoreEvents = true;
            viewModel.selectedItem.category1s.removeAll();
            for (var index = 0; index < item.category1s().length; index++)
            {
                viewModel.selectedItem.category1s.push(item.category1s()[index]);
            }
            viewModel.selectedItem.category2s.removeAll();
            for (var index = 0; index < item.category2s().length; index++)
            {
                viewModel.selectedItem.category2s.push(item.category2s()[index]);
            }
            viewModel.ignoreEvents = false;

            viewModel.selectedItem.selectedCategory1(category1);
            viewModel.selectedItem.selectedCategory2(category2);

            // Get the toppings
            if (dealHelper.selectedBindableDealLine.toppings == undefined)
            {
                viewModel.selectedItem.toppings(menuHelper.getItemToppings(viewModel.selectedItem.menuItem()));
            }
            else
            {
                viewModel.selectedItem.toppings(dealHelper.cloneToppings(dealHelper.selectedBindableDealLine.toppings));
            }

            // Does the customer need to pick any toppings?
            if (onlyShowIfToppings &&
                viewModel.selectedItem.toppings().length == 0 &&
                viewModel.selectedItem.category1s().length < 2 &&
                viewModel.selectedItem.category2s().length < 2)
            {
                popupHelper.commitToDeal();
            }
            else
            {
                // Hide the deal popup
                dealPopupHelper.hideDealPopup();

                // Show the toppings popup
                popupHelper.showPopup(mode);
            }
        }
    },
    cloneToppings: function (sourceToppings)
    {
        var cloneToppings = [];
        for (var index = 0; index < sourceToppings.length; index++)
        {
            var topping = sourceToppings[index];

            var dealLineTopping =
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

            cloneToppings.push(dealLineTopping);
        }

        return cloneToppings;
    },
    acceptChanges: function ()
    {
        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    commitToCart: function ()
    {
        // Has the user entered all the required information?
        if (!dealHelper.checkForErrors())
        {
            // Cheapest free
            if (dealHelper.selectedDeal.deal.ForceCheapestFree && dealHelper.selectedDeal.dealLines().length > 1)
            {
                var dealLine1 = dealHelper.selectedDeal.dealLines()[0];
                var dealLine2 = dealHelper.selectedDeal.dealLines()[1];

                // Check the first two deal lines
                var dealLinePrice1 = menuHelper.calculateDealLinePrice(dealLine1, true);
                var dealLinePrice2 = menuHelper.calculateDealLinePrice(dealLine2, true);

                if (dealLinePrice2 > dealLinePrice1)
                {
                    // We need to switch the menu items around so the more expensive one is in deal line 1

                    // Remove the first two items in the array
                    dealHelper.selectedDeal.dealLines.splice(0, 1);
                    dealHelper.selectedDeal.dealLines.splice(0, 1);

                    // Insert the removed item 
                    dealHelper.selectedDeal.dealLines.push(dealLine2);
                    dealHelper.selectedDeal.dealLines.push(dealLine1);
                }
            }

            // Clone the selected deal
            var deal =
            {
                name: dealHelper.selectedDeal.name(),
                price: 0,
                displayPrice: ko.observable('-'),
                dealLines: ko.observableArray(),
                deal:
                {
                    name: dealHelper.selectedDeal.name(),
                    description: dealHelper.selectedDeal.description(),
                    deal: dealHelper.selectedDeal.deal
                },
                minimumOrderValue: helper.formatPrice(dealHelper.selectedDeal.deal.MinimumOrderValue),
                minimumOrderValueNotMet: ko.observable(true),
                isEnabled: ko.observable()
            };

            // Add deal lines
            for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
            {
                var bindableDealLine = dealHelper.selectedDeal.bindableDealLines()[index];

                // Copy the toppings to the cart object.  We have to actually clone the topping objects - if we just copy the object references it'll get screwed up later
                var cartToppings = [];
                for (var toppingIndex = 0; toppingIndex < bindableDealLine.toppings.length; toppingIndex++)
                {
                    var topping = bindableDealLine.toppings[toppingIndex];

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

                // Get the categories
                var category1 = helper.findById(bindableDealLine.selectedMenuItem.Cat1 == undefined ? bindableDealLine.selectedMenuItem.Category1 : bindableDealLine.selectedMenuItem.Cat1, viewModel.menu.Category1);
                var category2 = helper.findById(bindableDealLine.selectedMenuItem.Cat2 == undefined ? bindableDealLine.selectedMenuItem.Category2 : bindableDealLine.selectedMenuItem.Cat2, viewModel.menu.Category2);

                deal.dealLines.push
                (
                    {
                        name: cartHelper.getCartItemName(bindableDealLine.selectedItemWrapper().item, category1, category2),
                        price: 0,
                        cartPrice: 0,
                        displayPrice: ko.observable('-'),
                        dealLine: bindableDealLine.dealLineWrapper.dealLine,
                        selectedMenuItem: bindableDealLine.selectedMenuItem,
                        displayToppings: ko.observableArray(menuHelper.getCartDisplayToppings(cartToppings)),
                        toppings: bindableDealLine.toppings,
                        categoryPremiumName: ko.observable(undefined),
                        categoryPremium: ko.observable(undefined),
                        itemPremiumName: ko.observable(undefined),
                        itemPremium: ko.observable(undefined),
                        instructions: bindableDealLine.instructions,
                        person: bindableDealLine.person
                    }
                );
            }

            // Add the selected deal to the cart
            cartHelper.cart().deals.push(deal);

            // Recalculate the cart price
            cartHelper.refreshCart();

            // Hide the deal popup
            dealPopupHelper.hideDealPopup();

            // If we are in mobile mode show the cart
            if (guiHelper.isMobileMode())
            {
                guiHelper.showCart();
            }
        }
    },
    removeFromCart: function ()
    {
        // Remove the item from the cart
        cartHelper.cart().deals.remove(cartHelper.cart().selectedCartItem());

        // Recalculate the total price
        cartHelper.refreshCart();

        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    checkForErrors: function ()
    {
        var errors = false;

        // Check each deal line and make sure the customer has selected an item
        for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
        {
            var bindableDealLine = dealHelper.selectedDeal.bindableDealLines()[index];

            bindableDealLine.hasError(false); // Reset to default

            // Does the user need to select anything for this deal line?
            if (bindableDealLine.itemWrappers.length > 0)
            {
                // Has the user actually selected anything?
                if (bindableDealLine.selectedMenuItem == undefined)
                {
                    bindableDealLine.hasError(true);
                    errors = true;
                }
            }
        }

        // Show or hide the error message
        dealHelper.hasError(errors);

        return errors;
    }
}