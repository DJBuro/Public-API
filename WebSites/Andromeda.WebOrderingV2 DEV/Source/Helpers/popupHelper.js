var popupHelper =
{
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    
    viewName: ko.observable(),
    viewModelObject: ko.observable(),
    closedCallback: undefined,

    cancel: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    showPopup: function (viewName, viewModelObject, closedCallback)
    {
        $(window).scrollTop(0);

        viewModel.pageManager.showPage("DeliveryAddress", true, { viewName: viewName, viewModelObject: viewModelObject, closedCallback: closedCallback });
    },
    showPopupUI: function (viewModelObject)
    {
        $(window).scrollTop(0);

        if (guiHelper.isMobileMode())
        {
            // In mobile mode popups are a page - not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
            popupHelper.isPopupVisible(false);

            // The toppings popup view model needs to go into a wrapper object.  This is because the same toppings popup html is used in both a popup as
            // part of the menu view and a view in it's own right depending on whether we're in mobile mode or not.  
            // The wrapper object is needed so the same html can bind in both scenarios
            guiHelper.showView(viewModelObject.viewName, viewModelObject.viewModelObject);
        }
        else
        {
            // Google analytics
            ga
            (
                'send',
                {
                    'hitType': 'pageview',
                    'page': '/' + viewModelObject.viewName,
                    'title': viewModelObject.viewName
                }
            );

            popupHelper.isBackgroundVisible(true);
            popupHelper.isPopupVisible(true);

            popupHelper.viewModelObject(new PopupViewModel(viewModelObject.viewModelObject, viewModelObject.closedCallback));
            popupHelper.viewName(viewModelObject.viewName);

            // Give knockout time to do its thing (Javascript doesn't do not proper multi-threading - need to let the browser have the thread back)
            setTimeout
            (
                popupHelper.showPopupCallback,
                0
            );
        }
    },
    showPopupCallback : function()
    {

    },
    mobileModeTransition: function (isMobileMode)
    {
        // When the popup is shown it'll automatically show the PC or mobile version as needed
        popupHelper.hidePopup(!isMobileMode);
        popupHelper.showPopupUI(popupHelper.viewName, popupHelper.viewModelObject, popupHelper.closedCallback);
    },
    hidePopup: function (isMobileMode)
    {
        if (isMobileMode == undefined) isMobileMode = guiHelper.isMobileMode();

        if (popupHelper.mobileModeTransitionContext != undefined)
        {
            popupHelper.mobileModeTransitionContext.dispose();
            popupHelper.mobileModeTransitionContext = undefined;
        }

        if (!isMobileMode)
        {
            popupHelper.isBackgroundVisible(false);
            popupHelper.isPopupVisible(false);

            // Let knockout do its thing
            setTimeout
            (
                function ()
                {
                    guiHelper.resize();
                    if (popupHelper.closedCallback !== undefined)
                    {
                        popupHelper.closedCallback();
                    }
                },
                0
            );
        }
    },
    acceptChanges: function ()
    {
        // Recalculate the total price of all items in the cart
        AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());

        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    keyPress: function (data, event)
    {
        //// Did the user press enter?
        //if (event.which == 13 || event.keyCode == 13)
        //{
        //    if (popupHelper.mode() == 'editItem')
        //    {
        //        popupHelper.acceptChanges();
        //    }
        //    else if (popupHelper.mode() == 'addItem')
        //    {
        //        popupHelper.commitToCart(data);
        //    }
        //    else if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
        //    {
        //        popupHelper.commitToDeal();
        //    }

        //    return false;
        //}
        //// Did the user press escape?
        //else
            if (event.which == 27 || event.keyCode == 27)
        {
            this.cancel();
        }

        return true;
    }
}