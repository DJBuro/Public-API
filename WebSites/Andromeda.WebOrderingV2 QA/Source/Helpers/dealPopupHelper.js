var dealPopupHelper =
{
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    cartDeal: undefined,

    showDealPopup: function (mode, scrollTop, cartDeal)
    {
        dealPopupHelper.cartDeal = cartDeal;

        if (scrollTop)
        {
            $(window).scrollTop(0);
        }

        // Make sure the toppings popup is not visible
        toppingsPopupHelper.isBackgroundVisible(false);
        toppingsPopupHelper.isPopupVisible(false);
        toppingsPopupHelper.isPageVisible = false;

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the deal dialog is a view and not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);

            // The deal pop-up view model needs to go into a wrapper object.  This is because the same toppings popup html is used in both a popup as
            // part of the menu view and a view in it's own right depending on whether we're in mobile mode or not.  
            // The wrapper object is needed so the same HTML can bind in both scenarios
            guiHelper.showView
            (
                'dealsView',
                {
                    isShowStoreDetailsButtonVisible: ko.observable(true),
                    isShowHomeButtonVisible: ko.observable(true),
                    isShowMenuButtonVisible: ko.observable(true),
                    isShowCartButtonVisible: ko.observable(true),

                    isHeaderVisible: ko.observable(viewModel.isMobileMode() ? false : true),
                    isPostcodeSelectorVisible: ko.observable(settings.storeSelectorMode && settings.storeSelectorInHeader),
                    areHeaderOptionsVisible: ko.observable(false),
                    isHeaderLoginVisible: ko.observable(true),
                    dealPopupViewModel: ko.observable(new DealViewModel(cartDeal, undefined, mode))
                }
            );
        }
        else
        {
            // Make the popup visible
            dealPopupHelper.isBackgroundVisible(true);
            dealPopupHelper.isPopupVisible(true);

            // We're showing a popup box as part of the existing menu view rather than showing new view
            // The same popup html is used and it needs something to bind to
            viewModel.contentViewModel().dealPopupViewModel(new DealViewModel(cartDeal, undefined, mode));
        }
    },
    hideDealPopup: function (isCancel)
    {
        // Make the popup visible
        dealPopupHelper.isBackgroundVisible(false);
        dealPopupHelper.isPopupVisible(false);

        // If we are in mobile mode show the cart
        if (guiHelper.isMobileMode() && !isCancel)
        {
            viewModel.pageManager.showPage('Cart', true, undefined, true);
        }
        else
        {
            var section = viewModel.sections()[viewModel.currentSectionIndex];
            viewModel.pageManager.showPage('Menu/' + section.display.Name, true, undefined, true);
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