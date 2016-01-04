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

        var cartDealWrapper = { mode: 'addDeal', cartDeal: new CartDeal(deal) };

        // Show the deal popup
        dealPopupHelper.returnToCart = false;  // If the user cancels show the menu not the cart (mobile only)        

        viewModel.pageManager.showPage('AddDeal', true, cartDealWrapper);
    }
}