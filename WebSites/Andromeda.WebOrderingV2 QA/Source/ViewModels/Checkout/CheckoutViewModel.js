/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function CheckoutViewModel()
{
    "use strict";

    var self = this;

    this.isShowStoreDetailsButtonVisible = ko.observable(true);
    this.isShowHomeButtonVisible = ko.observable(true);
    this.isShowMenuButtonVisible = ko.observable(true);
    this.isShowCartButtonVisible = ko.observable(true);

    this.isHeaderVisible = ko.observable(true);
    this.isPostcodeSelectorVisible = ko.observable(false);
    this.areHeaderOptionsVisible = ko.observable(true);
    this.isHeaderLoginVisible = ko.observable(true);

    // Mobile mode
    this.title = ko.observable(textStrings.mmHome); // Current section name - shown in the header
    this.titleClass = ko.observable('mobileSectionHome'); // Class to use to style the section name - used for showing an icon for the section

    this.isInitialised = false;

    this.updateTimeslots = false;
    this.updateFrequencyMilliseconds = 60000; //1000;

    this.onLogout = function ()
    {
        guiHelper.showMenu();
    }

    this.onShown = function ()
    {
        checkoutHelper.voucherError('');

        checkoutHelper.initialiseCheckout();
        self.isInitialised = true;

        // Make sure the cart prices are up to date
        cartHelper.refreshCart();

        self.updateTimeslots = true;
        setTimeout(self.refreshTimeSlots, self.updateFrequencyMilliseconds);
    }

    this.onClosed = function ()
    {
        self.updateTimeslots = false;
    }

    this.refreshTimeSlots = function()
    {
        if (self.updateTimeslots)
        {
            // Refresh the timeslots
            checkoutHelper.refreshTimeslotsAndCheckSelected(checkoutHelper.checkoutDetails.wantedTime().time);

            // Update time slots again in X seconds
            setTimeout(self.refreshTimeSlots, self.updateFrequencyMilliseconds);
        }
    }
};






































