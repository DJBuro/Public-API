
var postcodeCheckHelper =
{
    isPopupVisible: ko.observable(false),
    isInDeliveryZone: ko.observable(undefined),
    showPopup: function ()
    {
        // Do we have the stores delivery zones?
        if (deliveryZoneHelper.deliveryZones != undefined)
        {
            $(window).scrollTop(0);

            // Make the popup visible
            if (guiHelper.isMobileMode())
            {
                guiHelper.isMobileMenuVisible(false);
                guiHelper.showView('postcodeCheckView');
            }
            else
            {
                toppingsPopupHelper.isBackgroundVisible(true);
                postcodeCheckHelper.isPopupVisible(true);
            }
        }
    },
    hidePopup: function (callback)
    {
        if (!guiHelper.isMobileMode())
        {
            toppingsPopupHelper.isBackgroundVisible(false);
            postcodeCheckHelper.isPopupVisible(false);
        }

        if (callback != undefined)
        {
            callback();
        }
    },
    checkPostcode: function ()
    {
        postcodeCheckHelper.isInDeliveryZone(checkoutHelper.validatePostcode());
    },
    showMenu: function ()
    {
        if (guiHelper.isMobileMode())
        {
            // Continue showing the menu
            postcodeCheckHelper.hidePopup(guiHelper.showHome);
        }
        else
        {
            // Continue showing the menu
            postcodeCheckHelper.hidePopup();
        }
    },
    changeStore: function ()
    {
        // Continue showing the menu
        postcodeCheckHelper.hidePopup(viewModel.chooseStore);
    },
    checkPostcodeKeypress: function (data, event)
    {
        if (event.which == 13)
        {
            var postcode = $('#postcodeCheckTextbox').val();
            checkoutHelper.checkoutDetails.address.postcode(postcode);

            setTimeout
            (
                postcodeCheckHelper.checkPostcode(),
                0
            );
        }
        else
        {
            postcodeCheckHelper.isInDeliveryZone(undefined);
        }

        return true;
    }
}