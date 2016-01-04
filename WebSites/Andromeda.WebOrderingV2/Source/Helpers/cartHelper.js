/// <reference path="../App/Services/loyaltyHelper.js" />


function CartHelper()
{
    "use strict";

    var self = this;

    this.isCheckoutMode = ko.observable(false);
    this.ignoreOrderTypeChangeEvents = true;

    this.cart = ko.observable
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
            displayDiscountAmount: ko.observable(),
            vouchers: ko.observable([]),
            totalItemCount: ko.observable(0)
        }
    );

    this.isCartEmpty = function()
    {
        return cartHelper.cart().cartItems().length === 0 && cartHelper.cart().deals().length === 0;
    };
    this.refreshCart = function ()
    {
        // Reset total items in cart
        cartHelper.cart().totalItemCount(0);

        // Update the deals availability (depending on whether it's a collection or delivery order)
        menuHelper.refreshDealsAvailabilty();

        // Get the order value not including deals with a minimum order value
        var orderTotalForMinimumOrderValue = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice();

        var cartDetails =
            {
                totalPrice: 0,
                areDisabledItems: false,
                checkoutEnabled: false,
                totalPriceItemsOnly: 0
            };

        // Calculate deal prices
        self.refreshCartDeals(cartDetails);

        // Calculate item prices
        self.refreshCartLineItems(cartDetails)
        
        // Disabled cart items
        cartHelper.cart().areDisabledItems(cartDetails.areDisabledItems);

        // Calc sub total
        cartHelper.cart().displaySubTotalPrice(helper.formatPrice(cartDetails.totalPrice));
        cartHelper.cart().subTotalPrice(cartDetails.totalPrice);

        // Voucher codes
        self.refreshCartVouchers(cartDetails);
        
        //loyalty discount has been done here ... 
//        loyaltyHelper.refreshCartTotal(cartDetails);

        // Calculate the full order discount
        var fullOrderDiscountAmount = cartHelper.calculateFullOrderDiscount(cartDetails.totalPriceItemsOnly);

        // Discount
        var discountName = menuHelper.fullOrderDiscountDeal == undefined ? undefined : menuHelper.fullOrderDiscountDeal.DealName;
        cartHelper.cart().discountName(discountName);
        cartHelper.cart().discountAmount(fullOrderDiscountAmount);
        cartHelper.cart().displayDiscountAmount(helper.formatPrice(fullOrderDiscountAmount * -1));

        // Apply the full order discount
        cartDetails.totalPrice -= fullOrderDiscountAmount;

        // Grand total
        cartHelper.cart().displayTotalPrice(helper.formatPrice(cartDetails.totalPrice));
        cartHelper.cart().totalPrice(cartDetails.totalPrice);

        // True when there are items (enabled or disabled) in the cart
        cartHelper.cart().hasItems(cartHelper.cart().cartItems().length != 0 || cartHelper.cart().deals().length != 0);

        // Is the delivery charge met?  Don't check if already disabled
        if (cartDetails.checkoutEnabled && viewModel.orderType() == 'delivery')
        {
            cartDetails.checkoutEnabled = cartHelper.cart().totalPrice() >= settings.minimumDeliveryOrder;
        }

        // True when there is at least one enabled item in the cart
        cartHelper.cart().checkoutEnabled(cartDetails.checkoutEnabled);
    };
    this.refreshCartDeals = function (cartDetails)
    {
        var totalItemCount = cartHelper.cart().totalItemCount();

        // Update the deal prices in the cart
        for (var cartDealIndex = 0; cartDealIndex < cartHelper.cart().deals().length; cartDealIndex++)
        {
            var cartDeal = cartHelper.cart().deals()[cartDealIndex];
            cartDeal.finalPrice = 0;

            // Has the minimum order value been met?
            var minimumOrderValueOk = cartDetails.orderTotalForMinimumOrderValue == undefined || cartDetails.orderTotalForMinimumOrderValue >= cartDeal.dealWrapper.deal.MinimumOrderValue;
            cartDeal.minimumOrderValueNotMet(!minimumOrderValueOk);

            // Is the deal enabled in the cart?
            cartDeal.isEnabled(menuHelper.isDealItemEnabledForCart(cartDeal));

            if (cartDeal.isEnabled())
            {
                cartDetails.checkoutEnabled = true;
            }
            else
            {
                cartDetails.areDisabledItems = true;
            }

            // Set the display price depending on the current order type
            cartDeal.displayPrice(helper.formatPrice(cartDeal.price));

            var dealPrice = 0;

            // Does the deal have any deal lines?
            if (cartDeal.cartDealLines() != undefined)
            {
                // Update the prices of the deal lines in the deal
                for (var cartDealLineIndex = 0; cartDealLineIndex < cartDeal.cartDealLines().length; cartDealLineIndex++)
                {
                    totalItemCount++;

                    var cartDealLine = cartDeal.cartDealLines()[cartDealLineIndex];
                    var dealLinePrice = menuHelper.calculateDealLinePrice(cartDealLine);

                    // Add category deal premiums
                    var category1 = cartDealLine.cartItem().selectedCategory1();
                    var category2 = cartDealLine.cartItem().selectedCategory2();
                    var categoryPremium = ((category1 == undefined ? 0 : category1.DealPremium) + (category2 == undefined ? 0 : category2.DealPremium));

                    // Prefix the item name with the categories
                    var categoryText = category1 === undefined ? '' : category1.Name;
                    categoryText += categoryText.length > 0 ? ' ' : '';
                    categoryText += category2 === undefined ? '' : category2.Name;
                    categoryText += categoryText.length > 0 ? ' ' : '';

                    cartDealLine.displayName(categoryText + cartDealLine.cartItem().name);

                    // Set the calculated category premium
                    if (categoryPremium == undefined || categoryPremium == 0)
                    {
                        cartDealLine.categoryPremium(undefined);
                        cartDealLine.categoryPremiumName(undefined);
                    }
                    else
                    {
                        cartDealLine.categoryPremium(helper.formatPrice(categoryPremium));
                        cartDealLine.categoryPremiumName(textStrings.premiumChargeFor.replace('{item}', category2.Name));
                    }

                    // Add item deal premium
                    var itemPremium = cartDealLine.cartItem().menuItem.DealPricePremiumOverride == 0
                        ? cartDealLine.cartItem().menuItem.DealPricePremium
                        : cartDealLine.cartItem().menuItem.DealPricePremiumOverride;

                    // Set the calculated item premium
                    if (itemPremium == undefined || itemPremium == 0)
                    {
                        cartDealLine.itemPremium(undefined);
                        cartDealLine.itemPremiumName(undefined);
                    }
                    else
                    {
                        cartDealLine.itemPremium(helper.formatPrice(premium));
                        cartDealLine.itemPremiumName(textStrings.premiumChargeFor.replace('{item}', cartDealLine.cartItem().menuItem.Name));
                    }

                    // Add the deal line to the deal price
                    dealPrice += dealLinePrice;

                    if (cartDeal.isEnabled())
                    {
                        cartDeal.finalPrice += dealLinePrice + categoryPremium + itemPremium;
                        cartDetails.totalPrice += dealLinePrice + categoryPremium + itemPremium;
                    }

                    // Does the deal line have any toppings?
                    if (cartDealLine.cartItem().toppings() != undefined)
                    {
                        cartDealLine.cartItem().displayToppings(menuHelper.getCartDisplayToppings(cartDealLine.cartItem().toppings()));

                        var toppingsPrice = 0;

                        // Update the prices of the toppings in the item
                        for (var toppingIndex = 0; toppingIndex < cartDealLine.cartItem().displayToppings().length; toppingIndex++)
                        {
                            var topping = cartDealLine.cartItem().displayToppings()[toppingIndex];
                            var toppingPrice = topping.price;
                            var finalPrice = toppingPrice * cartDealLine.cartItem().quantity();
                            topping.cartPrice(helper.formatPrice(finalPrice));
                            
                            toppingsPrice += toppingPrice;
                        }

                        if (cartDeal.isEnabled())
                        {
                            cartDetails.totalPrice += (toppingsPrice * cartDealLine.cartItem().quantity());
                            cartDetails.totalPriceItemsOnly += (toppingsPrice * cartDealLine.cartItem().quantity());

                            cartDeal.finalPrice += (toppingsPrice * cartDealLine.cartItem().quantity());
                        }
                    }
                }

                cartDeal.price = dealPrice;
                cartDeal.displayPrice(helper.formatPrice(dealPrice));
            }
        }

        // Update the total count of items in the basket
        cartHelper.cart().totalItemCount(totalItemCount);
    };
    this.refreshCartLineItems = function (cartDetails)
    {
        var totalItemCount = cartHelper.cart().totalItemCount();

        // Update the item prices in the cart
        for (var index = 0; index < cartHelper.cart().cartItems().length; index++)
        {
            var item = cartHelper.cart().cartItems()[index];

            totalItemCount += Number(item.quantity());

            // Is the item enabled in the cart?
            item.isEnabled(menuHelper.isItemEnabledForCart(item.menuItem));

            if (item.isEnabled())
            {
                cartDetails.checkoutEnabled = true;
            }

            // Update the item name
            item.displayName(menuHelper.getCartItemDisplayName(item));

            // Set the price depending on the current order type
            item.price = menuHelper.calculateItemPrice(item.menuItem, item.quantity(), undefined);

            // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
            if (item.isEnabled())
            {
                cartDetails.totalPrice += item.price;
                cartDetails.totalPriceItemsOnly += item.price;
            }
            else
            {
                cartDetails.areDisabledItems = true;
            }

            // Set the display price depending on the current order type
            item.displayPrice(helper.formatPrice(item.price));

            item.displayToppings(menuHelper.getCartDisplayToppings(item.toppings()));

            // Does the item have any toppings?
            if (item.displayToppings() != undefined)
            {
                var toppingsPrice = 0;

                // Update the prices of the toppings in the item
                for (var toppingIndex = 0; toppingIndex < item.displayToppings().length; toppingIndex++)
                {
                    var topping = item.displayToppings()[toppingIndex];
                    var toppingPrice = topping.price; //menuHelper.calculateToppingPrice(topping);
                    var finalPrice = toppingPrice * item.quantity();
                    topping.cartPrice(helper.formatPrice(finalPrice));

                    toppingsPrice += toppingPrice;
                }

                if (item.isEnabled())
                {
                    cartDetails.totalPrice += (toppingsPrice * item.quantity());
                    cartDetails.totalPriceItemsOnly += (toppingsPrice * item.quantity());
                }
            }
        }

        // Update the total count of items in the basket
        cartHelper.cart().totalItemCount(totalItemCount);
    };
    this.refreshCartVouchers = function (cartDetails)
    {
        if (cartHelper.cart().vouchers() != undefined && cartHelper.cart().vouchers().length > 0)
        {
            // Check each voucher code
            for (var voucherCodeIndex = 0; voucherCodeIndex < cartHelper.cart().vouchers().length; voucherCodeIndex++)
            {
                var voucherCode = cartHelper.cart().vouchers()[voucherCodeIndex];

                // Apply the voucher code discount
                cartDetails.totalPrice = self.calculateVoucherDiscount(voucherCode, cartDetails.totalPrice);
            }
        }
    };
    this.getOrderValueExcludingDealsWithAMinimumPrice = function ()
    {
        var totalPrice = 0;

        // Calculate the deal prices
        for (var index = 0; index < cartHelper.cart().deals().length; index++)
        {
            var cartDeal = cartHelper.cart().deals()[index];

            if (cartDeal.dealWrapper.deal.MinimumOrderValue == 0)
            {
                // Does the deal have any deal lines?
                if (cartDeal.cartDealLines != undefined)
                {
                    // Update the prices of the deal lines in the deal
                    for (var dealLineIndex = 0; dealLineIndex < cartDeal.cartDealLines().length; dealLineIndex++)
                    {
                        var dealLine = cartDeal.cartDealLines()[dealLineIndex];
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
    this.calculateFullOrderDiscount = function (itemsPrice)
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
    this.calculateVoucherDiscount = function (voucherCode, totalPrice)
    {
        var afterDiscount = totalPrice;

        if (voucherCode.IsValid)
        {
            switch (voucherCode.EffectType.toLowerCase())
            {
                case 'percentage':
                    afterDiscount = Math.round(totalPrice * (voucherCode.EffectValue / 100));
                    voucherCode.discount(totalPrice - afterDiscount);
                    break;

                case 'fixed':
                    afterDiscount = totalPrice - (voucherCode.EffectValue * 100);
                    if (afterDiscount < 0) afterDiscount = 0;
                    voucherCode.discount(voucherCode.EffectValue);
                    break;
            }
        }

        return afterDiscount;
    };
    this.calculateLoyaltyDiscount = function (subTotalPrice) {
        
    };
    this.clearCart = function ()
    {
        ga
        (
            "send",
            "event",
            {
                eventCategory: "Sales",
                eventAction: "ClearCart",
                eventLabel: "",
                eventValue: 1,
                metric1: 1
            }
        );

        // Clear the cart
        cartHelper.cart().cartItems.removeAll();
        cartHelper.cart().deals.removeAll();

        // clear vouchers if any
        cartHelper.cart().vouchers([]);
        checkoutHelper.checkoutDetails.voucherCodes([]);
        checkoutHelper.voucherError('');

        // Update the total price (probably not needed)
        cartHelper.refreshCart();

        viewModel.timer = new Date();
    };
    this.checkout = function ()
    {
        // Is the customer logged in?
        if (settings.customerAccountsEnabled && !accountHelper.isLoggedIn())
        {
            // Show the login popup
            viewModel.pageManager.showPage('Login', true, new LoginViewModel(true));
        }
        // Has the customer filled in all the required my profile details
        else if (accountHelper.customerDetails.contacts.length != 2)
        {
            viewModel.pageManager.showPage('MyProfile', true, true);
        }
        else
        {
            cartHelper.showCheckoutView();
        }
    };
    this.showCheckoutView = function ()
    {
        // Refresh the delivery/collection slots
        checkoutHelper.refreshTimes();

        // Is the store open?
        if (checkoutHelper.times == undefined || checkoutHelper.times().length == 0)
        {
            alert(textStrings.sStoreClosed);
            
            viewModel.pageManager.showPage('Menu', true);

            return false;
        }

        guiHelper.cartActions(guiHelper.cartActionsCheckout);

        guiHelper.canChangeOrderType(false);

        guiHelper.isMobileMenuVisible(false);

        checkoutHelper.checkoutViewModel = new CheckoutViewModel();

        viewModel.pageManager.showPage('Checkout', true);

        return true;
    };

    this.getDealTemplate = function (cartItem)
    {
        //if (guiHelper.isCartLocked())
        //{
        //    return 'disabledCartDeal-template';
        //}
        //else
        //{
        //    return 'cartDeal-template';
        //}

        if (cartHelper.isCheckoutMode())
        {
            return 'disabledCartDeal-template';
        }
        else
        {
            return 'cartDeal-template';
        }
    };
    this.getItemTemplate = function (cartItem)
    {
        //if (guiHelper.isCartLocked())
        //{
        //    return 'disabledCartItem-template';
        //}
        //else
        //{
        //    return 'cartItem-template';
        //}

        if (cartHelper.isCheckoutMode())
        {
            return 'disabledCartItem-template';
        }
        else
        {
            return 'cartItem-template';
        }
    };
    this.editCartItem = function ()
    {
        self.editCartItem(this)
    };
    this.editCartItem = function (cartItem)
    {
        // Show the toppings popup
        toppingsPopupHelper.returnToCart = true; // In mobile mode the cart is a seperate view so we need to show the view
        toppingsPopupHelper.showPopup(cartItem, false, true);
    };
    this.editCartDeal = function ()
    {
        // Customer wants to add a deal to the cart
      //  var deal = context == null ? this : context;

     //   var cartDeal = new CartDeal(this);

        // Show the deal popup
        dealPopupHelper.returnToCart = false;  // If the user cancels show the menu not the cart (mobile only)
        dealPopupHelper.showDealPopup('editDeal', true, this);

        // Get the deal that the user has selected
        //cartHelper.cart().selectedCartItem(this);

        //// The deal to show on the deal popup
        //dealHelper.mode('editDeal');
        //dealHelper.hasError(false);

        //dealHelper.selectedDeal.name(this.name);
        //dealHelper.selectedDeal.description(this.deal.description);

        //// Build the deal lines
        //dealHelper.selectedDeal.cartDealLines.removeAll();

        //for (var index = 0; index < this.dealLines.length; index++)
        //{
        //    var dealLine = this.dealLines[index];

        //    var dealLineWrapper =
        //    {
        //        dealLine: dealLine.dealLine,
        //        items: undefined,
        //        selectedItem: ko.observable(),
        //        selectedMenuItem: dealLine.selectedMenuItem, // Each item could include multiple menu items for different cat1/cat2s - this is the menu item the customer wants
        //        hasError: ko.observable(false),
        //        toppings: dealLine.toppings
        //    };

        //    // Build the allowable items in the deal line
        //    var itemWrappers = [];
        //    for (var itemIndex = 0; itemIndex < dealLine.dealLine.items.length; itemIndex++)
        //    {
        //        var item = dealLine.dealLine.items[itemIndex];
        //        var itemWrapper =
        //        {
        //            dealLine: dealLineWrapper, // The deal line that the item is in - we're data binding to the item but we need a way to get back to the deal line the item is in
        //            item: itemIndex == 0 ? undefined : item, // Ignore the first item - it's always the dummy "plese select an item" item
        //            name: item.name
        //        };

        //        itemWrappers.push(itemWrapper);
        //    }

        //    // Set the allowable items in the deal line
        //    dealLineWrapper.menuItemWrappers = itemWrappers;

        //    // Add the deal line to the deal
        //    dealHelper.selectedDeal.dealLines.push(dealLineWrapper);
        //}

        //// Show the deal popup
        //dealPopupHelper.returnToCart = true;  // When finished return to the cart (mobile only)
        //dealPopupHelper.showDealPopup('editDeal', true);

        //// Do this last as it triggers knockout
        //dealHelper.selectedDeal.deal = this.deal;

        //// Let knockout sort out the GUI before subscribing to events as the act of building the GUI triggers these events which we're not interested in
        //setTimeout
        //(
        //    function ()
        //    {
        //        dealHelper.subscribeToDealLineChanges();
        //    },
        //    0
        //);
    };
    this.getCartItemName = function (menuItem, selectedCategory1, selectedCategory2)
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
    this.orderTypeChanged = function ()
    {
        if (self.ignoreOrderTypeChangeEvents) return true;

        // Update the item prices
        menuHelper.updatePrices();

        // Update the cart prices and availability
        cartHelper.refreshCart();

        return true;
    };
    this.removeCartItem = function (cartItem)
    {
        // Remove the item from the cart
        cartHelper.cart().cartItems.remove(cartItem);

        // Recalculate the total price
        cartHelper.refreshCart();

        // Check that the vouchers are valid
        checkoutHelper.recheckVouchers();

        // Recalculate the total price
        cartHelper.refreshCart();
    },
    this.removeDealItem = function (dealItem)
    {
        // Remove the item from the cart
        cartHelper.cart().deals.remove(dealItem);

        // Recalculate the total price
        cartHelper.refreshCart();
    }
};

var cartHelper = new CartHelper();