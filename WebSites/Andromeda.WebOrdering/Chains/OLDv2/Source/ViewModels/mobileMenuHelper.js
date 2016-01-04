var mobileMenuHelper =
{
    isMobileMenuVisible: ko.observable(true),
    isPopupVisible: ko.observable(false),
    showPopup: function ()
    {
        guiHelper.enable(true);
        mobileMenuHelper.isPopupVisible(true);
    },
    showHome: function ()
    {
        guiHelper.enableDisableUI(true);

        mobileMenuHelper.title(textStrings.mmHome);
        mobileMenuHelper.titleClass('popupMobileMenuHomeButton');

        mobileMenuHelper.isPopupVisible(false);
        guiHelper.showHome();
    },
    showMenu: function (index)
    {
        mobileMenuHelper.title(textStrings.mmOrderNow);
        mobileMenuHelper.titleClass('popupMobileMenuOrderNowButton');

        mobileMenuHelper.isPopupVisible(false);
        guiHelper.showMenu(index);
    },
    showCart: function ()
    {
        mobileMenuHelper.title(textStrings.mmMyOrder);
        mobileMenuHelper.titleClass('popupMobileMenuCartButton');

        mobileMenuHelper.isPopupVisible(false);
        guiHelper.showCart();
    },
    showPostcodeCheck: function ()
    {
        mobileMenuHelper.title(textStrings.mmPostcodeCheck);
        mobileMenuHelper.titleClass('popupMobileMenuPostcodeCheckButton');

        mobileMenuHelper.isPopupVisible(false);
        guiHelper.showPostcodeCheck();
    },
    close: function ()
    {
        mobileMenuHelper.isPopupVisible(false);
    },
    title: ko.observable(''),
    titleClass: ko.observable('popupMobileMenuHomeButton')
}