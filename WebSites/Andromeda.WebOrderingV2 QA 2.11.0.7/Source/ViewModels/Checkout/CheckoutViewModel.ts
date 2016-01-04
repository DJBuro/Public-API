/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../../scripts/typings/androwebordering/androwebordering.Models.d.ts" />
/// <reference path="../baseviewmodel.ts" />

module AndroWeb.ViewModels
{
    export class CheckoutViewModel extends BaseViewModel
    {
        // Mobile mode
        public title: KnockoutObservable<string> = ko.observable(textStrings.mmHome); // Current section name - shown in the header
        public titleClass: KnockoutObservable<string> = ko.observable('mobileSectionHome'); // Class to use to style the section name - used for showing an icon for the section

        public isInitialised: boolean = false;

        public updateTimeslots: boolean = false;
        public updateFrequencyMilliseconds: number = 60000; //1000;

        public autoCheckVoucher: boolean = true; // We only want to apply the voucher once otherwise we'll get into an infinite loop

        public onLogout () : void
        {
            guiHelper.showMenu(undefined);
        }

        public onShown () : void
        {
            checkoutHelper.voucherError('');

            // The checkout page can be shown, hidden and reshown with the same viewmodel
            // but we only want to initialise the first time in
            if (!this.isInitialised)
            {
                this.isInitialised = true;

                checkoutHelper.initialiseCheckout();

                // Make sure the cart prices are up to date
                AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart(), undefined);

                // Do we need to auto select a voucher?
                if (this.autoCheckVoucher)
                {
                    this.autoSelectVoucher();
                }

                if (viewModel.isDineInMode() === true)
                {
                    checkoutHelper.checkoutDetails.tableNumber(queryStringHelper.tableNumber);
                }
                else
                {
                    // Start refreshing the timeslots every X seconds
                    this.updateTimeslots = true;
                    setTimeout(this.refreshTimeSlots, this.updateFrequencyMilliseconds);
                }
            }
        }

        public onClosed = function () : void
        {
            this.updateTimeslots = false;
        }

        public autoSelectVoucher = function() : boolean
        {
            // Only do this once or we'll get stuck in a nasty infinite loop
            if (!this.autoCheckVoucher) return;

            if (queryStringHelper.voucherCode !== undefined && queryStringHelper.voucherCode.length > 0)
            {
                var found = false;

                // Has the voucher already been applied?
                if (AndroWeb.Helpers.CartHelper.cart().vouchers.length > 0)
                {
                    for (var voucherIndex = 0; voucherIndex < AndroWeb.Helpers.CartHelper.cart().vouchers.length; voucherIndex++)
                    {
                        var voucher = AndroWeb.Helpers.CartHelper.cart().vouchers[voucherIndex];
                        if (voucher.name == queryStringHelper.voucherCode)
                        {
                            // Yep - the voucher has already been applied
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    // Voucher hasn't already been applied - try and apply it
                    checkoutHelper.checkVoucherCode(queryStringHelper.voucherCode);

                    // Submit the voucher code
                    checkoutHelper.addVoucherCode();
                }
            }

            this.autoCheckVoucher = false;

            return true;
        }

        public refreshTimeSlots = function() : void
        {
            if (this.updateTimeslots)
            {
                // Refresh the timeslots
                checkoutHelper.refreshTimeslotsAndCheckSelected(checkoutHelper.checkoutDetails.wantedTime().time);

                // Update time slots again in X seconds
                setTimeout(this.refreshTimeSlots, this.updateFrequencyMilliseconds);
            }
        }
    }
}






































