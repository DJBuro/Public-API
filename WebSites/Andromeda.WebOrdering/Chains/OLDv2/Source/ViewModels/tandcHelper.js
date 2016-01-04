var tandcHelper =
{
    isPopupVisible: ko.observable(false),
    hasAgreed: function ()
    {
        if (tandcHelper.agree() == undefined)
        {
            tandcHelper.agree(false);
        }

        return tandcHelper.agree();
    },
    agree: ko.observable(undefined),
    showPopup: function ()
    {
        if (guiHelper.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('pay', false);
            guiHelper.showView('tandcView');
        }
        else
        {
            $(window).scrollTop(0);

            // Make the popup visible
            popupHelper.isBackgroundVisible(true);
            tandcHelper.isPopupVisible(true);
        }

        // Switch the cart to checkout mode
        guiHelper.canChangeOrderType(false);
        guiHelper.cartActions(guiHelper.cartActionsCheckout);
    },
    hidePopup: function (callback)
    {
        if (guiHelper.isMobileMode())
        {
            checkoutHelper.showPaymentPicker();
        }
        else
        {
            popupHelper.isBackgroundVisible(false);
            tandcHelper.isPopupVisible(false);
        }
    }
}