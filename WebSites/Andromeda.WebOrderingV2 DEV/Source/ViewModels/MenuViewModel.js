/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function MenuViewModel()
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(true);
    self.isShowHomeButtonVisible = ko.observable(true);
    self.isShowMenuButtonVisible = ko.observable(true);
    self.isShowCartButtonVisible = ko.observable(true);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(false);
    self.areHeaderOptionsVisible = ko.observable(true);
    self.isHeaderLoginVisible = ko.observable(true);

    // Mobile mode
    self.title = ko.observable(textStrings.mmOrderNow); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionOrderNow'); // Class to use to style the section name - used for showing an icon for the section

    // Toppings popup
    self.popupViewModel = ko.observable(undefined);

    // Deal popup
    self.dealPopupViewModel = ko.observable(undefined);

    self.onShown = function ()
    {
        if (!guiHelper.lockToOrderType())
        {
            guiHelper.canChangeOrderType(true);
        }
    }

    self.onLogout = function ()
    {
    }

    this.enterMobileMode = function ()
    {
        if (toppingsPopupHelper.isPopupVisible())
        {
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
        }

        if (dealPopupHelper.isPopupVisible())
        {
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
        }
    }

    this.exitMobileMode = function ()
    {
        if (toppingsPopupHelper.isPopupVisible())
        {
            guiHelper.isViewVisible(true);
            guiHelper.isInnerMenuVisible(true);
        }

        if (dealPopupHelper.isPopupVisible())
        {
            guiHelper.isViewVisible(true);
            guiHelper.isInnerMenuVisible(true);
        }
    }
};






































