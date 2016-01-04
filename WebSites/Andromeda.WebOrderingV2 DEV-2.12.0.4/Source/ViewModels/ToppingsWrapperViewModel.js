/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function ToppingsWrapperViewModel(showCartItem, showPopupCallback, dealDetails)
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(true);
    self.isShowHomeButtonVisible = ko.observable(true);
    self.isShowMenuButtonVisible = ko.observable(true);
    self.isShowCartButtonVisible = ko.observable(true);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(settings.storeSelectorMode && settings.storeSelectorInHeader);
    self.areHeaderOptionsVisible = ko.observable(true);
    self.isHeaderLoginVisible = ko.observable(true);

    self.popupViewModel = ko.observable(new ToppingsViewModel(showCartItem, showPopupCallback, dealDetails));

    self.onShown = function ()
    {
        self.popupViewModel().onShown();
    }
};































