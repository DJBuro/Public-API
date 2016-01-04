var toppingsPopupHelper =
{
    mobileModeTransitionContext: undefined,
    returnToCart: false,
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    isPageVisible: false,
    mode: ko.observable('addItem'),
    cancel: function ()
    {
        // Hide the toppings popup
        toppingsPopupHelper.hidePopup();
    },
    cancelDeal: function (cartDeal, cartDealLine)
    {
        toppingsPopupHelper.hidePopup();
    },
    commitToDeal: function (dealDetails)
    {
        dealDetails.cartDealLine.previouslySelectedAllowableMenuItemWrapper = dealDetails.cartDealLine.selectedAllowableMenuItemWrapper();
        dealDetails.cartDealLine.previouslySelectedMenuItem = dealDetails.cartDealLine.cartItem().menuItem;

        if (dealDetails.cartDealLine.cartItem().toppings() != undefined)
        {
            dealDetails.cartDealLine.previouslySelectedToppings = [];
            for (var index = 0; index < dealDetails.cartDealLine.cartItem().toppings().length; index++)
            {
                var topping = dealDetails.cartDealLine.cartItem().toppings()[index];

                var previousTopping = new cartItemTopping();
                previousTopping.cartName(topping.cartName());
                previousTopping.cartPrice(topping.cartPrice());
                previousTopping.cartQuantity(topping.cartQuantity());
                previousTopping.doublePrice = topping.doublePrice;
                previousTopping.finalPrice = topping.finalPrice;
                previousTopping.freeToppings = topping.freeToppings;
                previousTopping.itemPrice = topping.itemPrice;
                previousTopping.originalPrice = topping.originalPrice;
                previousTopping.price = topping.price;
                previousTopping.quantity(topping.quantity());
                previousTopping.singlePrice = topping.singlePrice;
                previousTopping.topping = topping.topping;
                previousTopping.type = topping.type;

                dealDetails.cartDealLine.previouslySelectedToppings.push(previousTopping);
            }
        }

        // The toppings popup binds to a cart item but we're dealing with a deal line so copy over the selected menu item
        dealDetails.cartDealLine.selectedMenuItem = dealDetails.cartDealLine.cartItem().menuItem;

        // Hide the toppings popup
        toppingsPopupHelper.hidePopup({ mode: 'addDeal', cartDeal: dealDetails.cartDeal });

        // Show the deal popup
 //       dealPopupHelper.showDealPopup(dealDetails.mode, false, dealDetails.cartDeal);
    },
    removeFromCart: function (cartItem)
    {
        // Remove the item from the cart
        cartHelper.cart().cartItems.remove(cartItem);

        // Recalculate the total price
        cartHelper.refreshCart();

        // Hide the toppings popup
        toppingsPopupHelper.hidePopup();
    },
    showPopup: function (showCartItem, isDealMode, isCartItem, dealDetails)
    {
        $(window).scrollTop(0);

        if (isDealMode === true)
        {
            toppingsPopupHelper.mode('addDealItem');
        }
        else if (isCartItem === true)
        {
            toppingsPopupHelper.mode('editItem');
        }
        else
        {
            // Set the correct mode
            if (showCartItem.cartId === undefined)
            {
                toppingsPopupHelper.mode('addItem');
            }
            else
            {
                toppingsPopupHelper.mode('editItem');
            }
        }

        // Show the toppings popup
        viewModel.pickToppings(true);

        viewModel.pageManager.showPage("CustomizeMenuItem", true, new ToppingsWrapperViewModel(showCartItem, undefined, dealDetails));
    },
    showPopupUI: function (popupViewModel)
    {
        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
            toppingsPopupHelper.isPopupVisible(false);
            toppingsPopupHelper.isPageVisible = true;

            // The toppings popup view model needs to go into a wrapper object.  This is because the same toppings popup html is used in both a popup as
            // part of the menu view and a view in it's own right depending on whether we're in mobile mode or not.  
            // The wrapper object is needed so the same html can bind in both scenarios
            guiHelper.showView('toppingsView', popupViewModel);
        }
        else
        {
            // Google analytics
            ga
            (
                'send',
                {
                    'hitType': 'pageview',
                    'page': '/toppingsView',
                    'title': 'toppingsView'
                }
            );

            toppingsPopupHelper.isBackgroundVisible(true);
            toppingsPopupHelper.isPopupVisible(true);
            toppingsPopupHelper.isPageVisible = false;

            // We're showing a popup box as part of the existing menu view rather than showing new view
            // The same popup html is used and it needs something to bind to
            viewModel.contentViewModel().popupViewModel(popupViewModel.popupViewModel());

            // Give knockout time to do its thing (Javascript doesn't do not proper multi-threading - need to let the browser have the thread back)
            setTimeout
            (
                function ()
                {
                    if (toppingsPopupHelper.showPopupCallback !== undefined)
                    {
                        toppingsPopupHelper.showPopupCallback();
                    }
                },
                0
            );
        }
    },
    mobileModeTransition: function (isMobileMode)
    {
        // When the popup is shown it'll automatically show the PC or mobile version as needed
        toppingsPopupHelper.hidePopup(!isMobileMode);
        toppingsPopupHelper.showPopupUI();
    },
    hidePopup: function (isMobileMode, state)
    {      
        if (isMobileMode == undefined) isMobileMode = guiHelper.isMobileMode();

        if (toppingsPopupHelper.mobileModeTransitionContext != undefined)
        {
            toppingsPopupHelper.mobileModeTransitionContext.dispose();
            toppingsPopupHelper.mobileModeTransitionContext = undefined;
        }

        if (toppingsPopupHelper.isPopupVisible() || toppingsPopupHelper.isPageVisible)
        {
            toppingsPopupHelper.isPopupVisible(false);
            toppingsPopupHelper.isPageVisible = false;
            toppingsPopupHelper.isBackgroundVisible(false);

            // Go back to the previous url
            viewModel.pageManager.showPreviousPage(true, state);
        }

        if (!isMobileMode)
        {
            // We might be on top of the deal popup
            if (!dealPopupHelper.isPopupVisible())
            {
                guiHelper.isInnerMenuVisible(true);
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
        }
    },
    commitToCart: function (cartItem)
    {
        // Add the item to the cart
        cartHelper.cart().cartItems.push(cartItem);

        // Recalculate the cart price
        cartHelper.refreshCart();

        ga
        (
            "send",
            "event",
            {
                eventCategory: "Sales",
                eventAction: "AddToCart",
                eventLabel: cartItem.name,
                eventValue: Number(cartItem.quantity()),
                metric1: Number(cartItem.price) / 100
            }
        );

        // Hide the toppings popup
        toppingsPopupHelper.hidePopup();
    },
    acceptChanges: function ()
    {
        // Recalculate the total price of all items in the cart
        cartHelper.refreshCart();

        // Hide the toppings popup
        toppingsPopupHelper.hidePopup();

    },
    keyPress: function (data, event)
    {
        // Did the user press enter?
        if (event.which == 13 || event.keyCode == 13)
        {
            if (toppingsPopupHelper.mode() == 'editItem')
            {
                toppingsPopupHelper.acceptChanges();
            }
            else if (toppingsPopupHelper.mode() == 'addItem')
            {
                toppingsPopupHelper.commitToCart(data);
            }
            else if (toppingsPopupHelper.mode() == 'addDealItem' || toppingsPopupHelper.mode() == 'editDealItem')
            {
                toppingsPopupHelper.commitToDeal();
            }

            return false;
        }
        // Did the user press escape?
        else if (event.which == 27 || event.keyCode == 27)
        {
            this.cancel();
        }

        return true;
    }
}