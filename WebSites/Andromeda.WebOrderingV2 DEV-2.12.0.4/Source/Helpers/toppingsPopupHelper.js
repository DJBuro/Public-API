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
    removeFromCart: function (cartItem)
    {
        // Remove the item from the cart
        AndroWeb.Helpers.CartHelper.cart().cartItems.remove(cartItem);

        // Recalculate the total price
        AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());

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

        // Show the toppings pop-up
        viewModel.pickToppings(true);

        // Note: The page manager will call showPopupUI
        viewModel.pageManager.showPage("CustomizeMenuItem", true, new ToppingsWrapperViewModel(showCartItem, undefined, dealDetails));
    },
    showPopupUI: function (popupViewModel)
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
                    // Fake a call to onShown just the same as showing a view would do
                    if (popupViewModel.onShown !== undefined)
                    {
                        popupViewModel.onShown();
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
        AndroWeb.Helpers.CartHelper.cart().cartItems.push(cartItem);

        // Recalculate the cart price
        AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());

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
        AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());

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