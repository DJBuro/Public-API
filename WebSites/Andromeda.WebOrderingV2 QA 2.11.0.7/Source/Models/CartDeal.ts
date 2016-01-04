/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="cartdealline.ts" />

module AndroWeb.Models
{
    export class CartDeal
    {
        public dealWrapper: any;
        public name: KnockoutObservable<string> = ko.observable("");
        public description: KnockoutObservable<string> = ko.observable("");        
        public cartDealLines: KnockoutObservableArray<CartDealLine> = ko.observableArray([]);
        public hasError: KnockoutObservable<boolean> = ko.observable(false);
        public isEnabled: KnockoutObservable<boolean> = ko.observable(true);
        public minimumOrderValueNotMet: KnockoutObservable<boolean> = ko.observable(true);
        public displayPrice: KnockoutObservable<string> = ko.observable('');
        public price: number = 0;
        public minimumOrderValue: number = 0;
        public notes: KnockoutObservable<string> = ko.observable("");

        constructor(dealWrapper: any)
        {
            this.dealWrapper = dealWrapper;

            this.name(dealWrapper.deal.DealName);
            this.description(dealWrapper.deal.Description);

            // Build the cart deal lines
            if (dealWrapper.dealLineWrappers !== undefined)
            {
                for (var index = 0; index < dealWrapper.dealLineWrappers.length; index++)
                {
                    var dealLineWrapper = dealWrapper.dealLineWrappers[index];

                    var cartDealLine = new AndroWeb.Models.CartDealLine(dealLineWrapper, 'dl_' + index);

                    // Add the deal line to the deal
                    this.cartDealLines().push(cartDealLine);
                }
            }
        }

        checkForErrors(): boolean
        {
            var errors: boolean = false;

            // Check each deal line and make sure the customer has selected an item
            for (var index = 0; index < this.cartDealLines().length; index++)
            {
                var cartDealLine = this.cartDealLines()[index];

                cartDealLine.hasError(false); // Reset to default

                // Does the user need to select anything for this deal line?
                if (cartDealLine.allowableMenuItemWrappers === undefined)
                {
                    errors = true;
                }
                else if (cartDealLine.allowableMenuItemWrappers.length > 0)
                {
                    // Has the user actually selected anything?
                    if (cartDealLine.selectedAllowableMenuItemWrapper() === undefined ||
                        cartDealLine.selectedAllowableMenuItemWrapper().menuItemWrapper === undefined ||
                        cartDealLine.selectedAllowableMenuItemWrapper().menuItemWrapper.menuItem === undefined)
                    {
                        cartDealLine.hasError(true);
                        errors = true;
                    }
                }
            }

            // Show or hide the error message
            this.hasError(errors);

            return errors;
        }
    }
}































