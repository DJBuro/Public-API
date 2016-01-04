function CartHelper ()
{
    var self = this;

    self.cart = ko.observable
    (
        {
            displayTotalPrice: ko.observable(),
            cartItems: ko.observableArray(),
            totalPrice: ko.observable(),
            selectedCartItem: ko.observable({}),
            mercuryPaymentId: ko.observable(),
            dataCashPaymentDetails: ko.observable(),
            deals: ko.observableArray(),
            areDisabledItems: ko.observable(),
            checkoutEnabled: ko.observable(),
            hasItems: ko.observable(),
            displaySubTotalPrice: ko.observable(),
            subTotalPrice: ko.observable(),
            discountName: ko.observable(),
            discountAmount: ko.observable(),
            displayDiscountAmount: ko.observable()
        }
    );

    self.refreshCart = function ()
    {
        // Update the deals availability (depending on whether it's a collection or delivery order)
        menuHelper.refreshDealsAvailabilty();

        // Get the order value not including deals with a minimum order value
        var orderTotalForMinimumOrderValue = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice();

        var totalPrice = 0;
        var areDisabledItems = false;
        var checkoutEnabled = false;

        // Update the deal prices in the cart
        for (var index = 0; index < cartHelper.cart().deals().length; index++)
        {
            var deal = cartHelper.cart().deals()[index];

            // Has the minimum order value been met?
            var minimumOrderValueOk = orderTotalForMinimumOrderValue >= deal.deal.deal.MinimumOrderValue;
            deal.minimumOrderValueNotMet(!minimumOrderValueOk);

            // Is the deal enabled in the cart?
            deal.isEnabled(menuHelper.isDealItemEnabledForCart(deal));

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
                    var dealLinePrice = menuHelper.calculateDealLinePrice(dealLine);

                    // Get the categories
                    var category1 = helper.findById(dealLine.selectedMenuItem.Cat1 == undefined ? dealLine.selectedMenuItem.Category1 : dealLine.selectedMenuItem.Cat1, viewModel.menu.Category1);
                    var category2 = helper.findById(dealLine.selectedMenuItem.Cat2 == undefined ? dealLine.selectedMenuItem.Category2 : dealLine.selectedMenuItem.Cat2, viewModel.menu.Category2);

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
                        dealLine.categoryPremiumName(textStrings.premiumChargeFor.replace('{item}', category2.Name));
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
                        dealLine.itemPremiumName(textStrings.premiumChargeFor.replace('{item}', dealLine.selectedMenuItem.Name));
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
                            var toppingPrice = menuHelper.calculateToppingPrice(topping);
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

        var totalPriceItemsOnly = 0;

        // Update the item prices in the cart
        for (var index = 0; index < cartHelper.cart().cartItems().length; index++)
        {
            var item = cartHelper.cart().cartItems()[index];

            // Is the item enabled in the cart?
            item.isEnabled(menuHelper.isItemEnabledForCart(item.menuItem));

            if (item.isEnabled())
            {
                checkoutEnabled = true;
            }

            // Set the price depending on the current order type
            item.price = menuHelper.calculateItemPrice(item.menuItem, item.quantity(), undefined);

            // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
            if (item.isEnabled())
            {
                totalPrice += item.price;
                totalPriceItemsOnly += item.price;
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
                    var toppingPrice = menuHelper.calculateToppingPrice(topping);
                    topping.cartPrice(helper.formatPrice(toppingPrice * item.quantity()));

                    toppingsPrice += toppingPrice;
                }

                if (item.isEnabled())
                {
                    totalPrice += (toppingsPrice * item.quantity());
                    totalPriceItemsOnly += (toppingsPrice * item.quantity());
                }
            }
        }

        cartHelper.cart().areDisabledItems(areDisabledItems);

        // Calculate the full order discount
        var fullOrderDiscountAmount = cartHelper.calculateFullOrderDiscount(totalPriceItemsOnly);

        // Sub total
        cartHelper.cart().displaySubTotalPrice(helper.formatPrice(totalPrice));
        cartHelper.cart().subTotalPrice(totalPrice);

        // Discount
        var discountName = menuHelper.fullOrderDiscountDeal == undefined ? undefined : menuHelper.fullOrderDiscountDeal.DealName;
        cartHelper.cart().discountName(discountName);
        cartHelper.cart().discountAmount(fullOrderDiscountAmount);
        cartHelper.cart().displayDiscountAmount(helper.formatPrice(fullOrderDiscountAmount * -1));

        // Apply the full order discount
        totalPrice -= fullOrderDiscountAmount;

        // Grand total
        cartHelper.cart().displayTotalPrice(helper.formatPrice(totalPrice));
        cartHelper.cart().totalPrice(totalPrice);

        // True when there are items (enabled or disabled) in the cart
        cartHelper.cart().hasItems(cartHelper.cart().cartItems().length != 0 || cartHelper.cart().deals().length != 0);

        // Is the delivery charge met?  Don't check if already disabled
        if (checkoutEnabled && viewModel.orderType() == 'delivery')
        {
            checkoutEnabled = cartHelper.cart().totalPrice() >= settings.minimumDeliveryOrder;
        }

        // True when there is at least one enabled item in the cart
        cartHelper.cart().checkoutEnabled(checkoutEnabled);
    };
    self.getOrderValueExcludingDealsWithAMinimumPrice = function ()
    {
        var totalPrice = 0;

        // Calculate the deal prices
        for (var index = 0; index < cartHelper.cart().deals().length; index++)
        {
            var deal = cartHelper.cart().deals()[index];

            if (deal.deal.deal.MinimumOrderValue == 0)
            {
                // Does the deal have any deal lines?
                if (deal.dealLines != undefined)
                {
                    // Update the prices of the deal lines in the deal
                    for (var dealLineIndex = 0; dealLineIndex < deal.dealLines().length; dealLineIndex++)
                    {
                        var dealLine = deal.dealLines()[dealLineIndex];
                        var dealLinePrice = menuHelper.calculateDealLinePrice(dealLine);

                        totalPrice += dealLinePrice;
                    }
                }
            }
        }

        var totalPriceItemsOnly = 0;

        // Calculate the item prices
        for (var index = 0; index < cartHelper.cart().cartItems().length; index++)
        {
            var item = cartHelper.cart().cartItems()[index];

            // Set the price depending on the current order type
            var itemPrice = menuHelper.calculateItemPrice(item.menuItem, item.quantity(), undefined);

            // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
            if (menuHelper.isItemAvailable(item.menuItem))
            {
                totalPrice += itemPrice;
                totalPriceItemsOnly += itemPrice;
            }

            // Does the item have any toppings?
            if (item.displayToppings != undefined)
            {
                // Update the prices of the toppings in the item
                for (var toppingIndex = 0; toppingIndex < item.displayToppings().length; toppingIndex++)
                {
                    var topping = item.displayToppings()[toppingIndex];
                    var toppingPrice = menuHelper.calculateToppingPrice(topping);

                    if (menuHelper.isItemAvailable(item.menuItem))
                    {
                        totalPrice += toppingPrice;
                        totalPriceItemsOnly += toppingPrice;
                    }
                }
            }
        }

        // Apply any full order discount deal
        totalPrice -= cartHelper.calculateFullOrderDiscount(totalPriceItemsOnly);

        if (totalPrice < 0) totalPrice = 0;

        return totalPrice;
    };
    self.calculateFullOrderDiscount = function (itemsPrice)
    {
        var fullOrderDiscount = 0;

        if (menuHelper.fullOrderDiscountDeal != undefined)
        {
            switch (menuHelper.fullOrderDiscountDeal.FullOrderDiscountType.toLowerCase())
            {
                case 'percentage':

                    var percentage = 100;

                    if (viewModel.orderType().toLowerCase() == 'collection')
                    {
                        percentage = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount / 100;
                    }
                    else
                    {
                        percentage = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount / 100;
                    }

                    // Deduct the percentage from the item price
                    fullOrderDiscount = itemsPrice - (itemsPrice * percentage);

                    break;

                case 'discount':

                    if (viewModel.orderType().toLowerCase() == 'collection')
                    {
                        fullOrderDiscount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount;
                    }
                    else
                    {
                        fullOrderDiscount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount;
                    }

                    // Deduct the discount from the item price
                    fullOrderDiscount = itemsPrice - fullOrderDiscount;

                    break;
            }
        }

        return Math.round(fullOrderDiscount);
    };
    self.clearCart = function ()
    {
        // Clear the cart
        cartHelper.cart().cartItems.removeAll();
        cartHelper.cart().deals.removeAll();

        // Update the total price (probably not needed)
        cartHelper.refreshCart();

        viewModel.timer = new Date();
    };
    self.checkout = function ()
    {
        // Is the customer logged in?
        if (settings.customerAccountsEnabled && !accountHelper.isLoggedIn())
        {
            // Show the login popup
            accountHelper.showPopup(cartHelper.checkout);
            return;
        }

        // Refresh the delivery/collection slots
        checkoutHelper.refreshTimes();

        if (guiHelper.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('menu', true);
            guiHelper.showView('checkoutView');
        }
        else
        {
            guiHelper.showMenuView('checkoutView');
        }

        checkoutHelper.initialiseCheckout();

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
    self.editCartItem = function ()
    {
        // Get the menu item that the user has selected
        cartHelper.cart().selectedCartItem(this);

        var price = menuHelper.calculateItemPrice(this.menuItem, this.quantity(), this.toppings);

        // The item to show on the toppings popup
        viewModel.selectedItem.name(this.name);
        viewModel.selectedItem.description(menuHelper.fixName(this.menuItem.Desc == undefined ? this.menuItem.Description : this.menuItem.Desc));
        viewModel.selectedItem.quantity(this.quantity());
        viewModel.selectedItem.menuItem(this.menuItem);
        viewModel.selectedItem.instructions(this.instructions);
        viewModel.selectedItem.person(this.person);

        // Copy over the menu items
        viewModel.selectedItem.menuItems.removeAll();
        for (var index = 0; index < this.menuItems.length; index++)
        {
            viewModel.selectedItem.menuItems.push(this.menuItems[index]);
        }

        // Copy over the cat1 and cat2s
        viewModel.ignoreEvents = true;
        viewModel.selectedItem.category1s.removeAll();
        for (var index = 0; index < this.category1s().length; index++)
        {
            viewModel.selectedItem.category1s.push(this.category1s()[index]);
        }
        viewModel.selectedItem.category2s.removeAll();
        for (var index = 0; index < this.category2s().length; index++)
        {
            viewModel.selectedItem.category2s.push(this.category2s()[index]);
        }
        viewModel.ignoreEvents = false;

        viewModel.selectedItem.toppings(this.toppings());
        viewModel.selectedItem.selectedCategory1(this.selectedCategory1);
        viewModel.selectedItem.selectedCategory2(this.selectedCategory2);

        viewModel.selectedItem.price(helper.formatPrice(price));

        // Show the toppings popup
        popupHelper.returnToCart = true; // In mobile mode the cart is a seperate view so we need to show the view
        popupHelper.showPopup('editItem');
    };
    self.editCartDeal = function ()
    {
        // Get the deal that the user has selected
        cartHelper.cart().selectedCartItem(this);

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
    self.orderTypeChanged = function ()
    {
        // Update the cart prices and availability
        cartHelper.refreshCart();

        return true;
    };
};

var cartHelper = new CartHelper();