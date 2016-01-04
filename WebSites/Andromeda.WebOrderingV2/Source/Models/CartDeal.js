/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function CartDeal(dealWrapper)
{
    "use strict";

    var self = this;

    this.name = ko.observable(dealWrapper.deal.DealName);
    this.description = ko.observable(dealWrapper.deal.Description);
    this.dealWrapper = dealWrapper;
    this.cartDealLines = ko.observableArray();
    this.hasError = ko.observable(false);
    this.isEnabled = ko.observable(true);
    this.minimumOrderValueNotMet = ko.observable(true);
    this.displayPrice = ko.observable('');
    this.price = 0;
    this.minimumOrderValue = 0;

    // Build the cart deal lines
    for (var index = 0; index < dealWrapper.dealLineWrappers.length; index++)
    {
        var dealLineWrapper = dealWrapper.dealLineWrappers[index];

        var cartDealLine = new CartDealLine(dealLineWrapper, 'dl_' + index);

        // Add the deal line to the deal
        this.cartDealLines.push(cartDealLine);
    }

    this.checkForErrors = function ()
    {
        var errors = false;

        // Check each deal line and make sure the customer has selected an item
        for (var index = 0; index < self.cartDealLines().length; index++)
        {
            var cartDealLine = self.cartDealLines()[index];

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
        self.hasError(errors);

        return errors;
    }
};































