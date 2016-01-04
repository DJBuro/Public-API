/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function cartItemTopping(rawTopping)
{
    "use strict";

    var self = this;

    self.topping = rawTopping; // Reference copy but this should never change
    self.type = '',
    self.selectedSingle = ko.observable(false);
    self.selectedDouble = ko.observable(false);
    self.originalPrice = 0;
    self.price = 0;
    self.singlePrice = '';
    self.doublePrice = '';
    self.quantity = ko.observable(0);
    self.freeToppings = 0;
    self.itemPrice = 0;
    self.finalPrice = 0;

    self.cartName = ko.observable();
    self.cartPrice = ko.observable();
    self.cartQuantity = ko.observable();

    self.recalculatePriceAndQuantity = function()
    {
        // Set quantity
        if (!self.selectedSingle() && !self.selectedDouble())
        {
            self.quantity(0);
        }
        else if (self.selectedSingle() && !self.selectedDouble())
        {
            self.quantity(1);
        }
        else if (!self.selectedSingle() && self.selectedDouble())
        {
            self.quantity(2);
        }

        // Figure out the price
        if (self.type == 'removable')
        {
            if (self.quantity() < 2)
            {
                // One is free
                self.price = 0;
            }
            else
            {
                // As a single quantity of this topping comes with the item by default, we only need to charge a single for a double quantity
                self.price = self.originalPrice;
            }
        }
        else if (self.type == 'additional')
        {
            self.price = self.originalPrice * self.quantity();
        }

        // Adjust for free toppings
        var freePrice = self.freeToppings * self.originalPrice;
        self.price = self.price - freePrice;
    }
};































