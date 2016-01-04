var imageHelper =
{
    isPopupVisible: ko.observable(false),
    topOffset: ko.observable(false),
    leftOffset: ko.observable(false),
    image: ko.observable
    (
        {
            Src: '',
            Height: 130,
            Width: 130
        }
    ),
    showPopup: function (data)
    {
        imageHelper.image(data.image);

        if (guiHelper.isMobileMode())
        {
            $(window).scrollTop(0);

            guiHelper.showView('imageView');

            mobileMenuHelper.isMobileMenuVisible(false);
        }
        else
        {
            imageHelper.topOffset('-' + (data.image.Height / 2) + 'px');
            imageHelper.leftOffset('-' + (data.image.Width / 2) + 'px');

            // Make the popup visible
            popupHelper.isBackgroundVisible(true);
            imageHelper.isPopupVisible(true);
        }
    },
    hidePopup: function ()
    {
        if (guiHelper.isMobileMode())
        {
            mobileMenuHelper.isMobileMenuVisible(true);

            guiHelper.showMenu(undefined);
        }
        else
        {
            popupHelper.isBackgroundVisible(false);
            imageHelper.isPopupVisible(false);
        }
    }
}