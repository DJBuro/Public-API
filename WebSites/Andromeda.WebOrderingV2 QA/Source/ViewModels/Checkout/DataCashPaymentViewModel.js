/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function DataCashPaymentViewModel()
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
    self.isHeaderLoginVisible = ko.observable(false);

    // Mobile mode
    self.title = ko.observable(textStrings.mmHome); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionHome'); // Class to use to style the section name - used for showing an icon for the section

    self.onShown = function ()
    {
        var iFrame = $('#dataCashPaymentIFrame');

        iFrame.attr('src', cartHelper.cart().dataCashPaymentDetails().url + '?HPS_SessionID=' + cartHelper.cart().dataCashPaymentDetails().sessionId);

        iFrame.load
        (
            function ()
            {
                if (iFrame.contents().get(0) != undefined)
                {
                    var href = iFrame.contents().get(0).location.href;
                    if (href !== undefined)
                    {
                        if (checkoutHelper.simulateDataCashError)
                        {
                            viewHelper.showError('SIMULATED DATACASH ERROR', guiHelper.showCheckoutAfterError, undefined);
                        }
                        else
                        {
                            checkoutHelper.sendOrderToStore();
                        }
                    }
                }
            }
        );
    }

    self.onLogout = function ()
    {
        guiHelper.showMenu();
    }
};






































