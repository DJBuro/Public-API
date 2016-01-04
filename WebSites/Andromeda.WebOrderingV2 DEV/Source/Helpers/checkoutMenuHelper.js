var checkoutMenuHelper =
{
    showMenu: function (backTo, isPlaceOrderEnabled)
    {
        if (guiHelper.isMobileMode())
        {
            checkoutMenuHelper.isVisible(true);
            checkoutMenuHelper.backTo(backTo);
            checkoutMenuHelper.isPlaceOrderEnabled(isPlaceOrderEnabled);
        }
    },
    hideMenu: function ()
    {
        if (guiHelper.isMobileMode())
        {
            checkoutMenuHelper.isVisible(false);
        }
    },
    isVisible: ko.observable(false),
    backTo: ko.observable('menu'),
    isPlaceOrderEnabled: ko.observable(true)
}