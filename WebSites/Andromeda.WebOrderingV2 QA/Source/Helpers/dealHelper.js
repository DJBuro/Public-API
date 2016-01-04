var dealHelper =
{
    returnToCart: false,
    selectedDeal: undefined,
    mobileAddToCart: function (context)
    {
        if (guiHelper.isMobileMode())
        {
            dealHelper.addToCart(this);
        }
    },
    addToCart: function (context)
    {
        // Customer wants to add a deal to the cart
        var deal = context == null ? this : context;    

        if (deal.isEnabled() &&
            deal.isAvailableToday() &&
            !deal.isNotAvailableForRestOfDay() &&
            viewModel.isTakingOrders() && 
            ((viewModel.orderType() == 'delivery' && deal.deal.ForDelivery) || (viewModel.orderType() == 'collection' && deal.deal.ForCollection)))
        {
            var cartDealWrapper = { mode: 'addDeal', cartDeal: new CartDeal(deal) };

            // Show the deal popup
            dealPopupHelper.returnToCart = false;  // If the user cancels show the menu not the cart (mobile only)  

            viewModel.pageManager.showPage('AddDeal', true, cartDealWrapper, true);
        }
    }
}