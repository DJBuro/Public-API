var dealPopupHelper =
{
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    showDealPopup: function (mode, scrollTop)
    {
        if (scrollTop)
        {
            $(window).scrollTop(0);
        }

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
        }

        dealHelper.mode(mode);

        // Make the popup visible
        dealPopupHelper.isBackgroundVisible(true);
        dealPopupHelper.isPopupVisible(true);
    },
    hideDealPopup: function ()
    {
        // Make the popup visible
        dealPopupHelper.isBackgroundVisible(false);
        dealPopupHelper.isPopupVisible(false);

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            if (dealPopupHelper.returnToCart)
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
}