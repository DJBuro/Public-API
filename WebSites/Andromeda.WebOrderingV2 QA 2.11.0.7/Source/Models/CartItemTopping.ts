/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />

module AndroWeb.Models
{
    export class CartItemTopping
    {
        public topping: any;
        public type: string = '';
        public selectedSingle: KnockoutObservable<boolean> = ko.observable(false);
        public selectedDouble: KnockoutObservable<boolean> = ko.observable(false);
        public originalPrice: number = 0;
        public price: number = 0;
        public singlePrice: string = '';
        public doublePrice: string = '';
        public quantity: KnockoutObservable<number> = ko.observable(0);
        public freeToppings: number = 0;
        public itemPrice: number = 0;
        public finalPrice: number = 0;
        public cartName: KnockoutObservable<string> = ko.observable('');
        public cartPrice: KnockoutObservable<number> = ko.observable(0);
        public cartQuantity: KnockoutObservable<number> = ko.observable(0);
        public isEnabled: boolean = true;
        public notes: string = "";

        constructor(rawTopping: any)
        {
            // Reference copy but this should never change
            this.topping = rawTopping;
            this.cartName(rawTopping.Name);
        }

        public recalculatePriceAndQuantity()
        {
            // Set quantity
            if (!this.selectedSingle() && !this.selectedDouble())
            {
                this.quantity(0);
            }
            else if (this.selectedSingle() && !this.selectedDouble())
            {
                this.quantity(1);
            }
            else if (!this.selectedSingle() && this.selectedDouble())
            {
                this.quantity(2);
            }

            // Figure out the price
            if (this.type == 'removable')
            {
                if (this.quantity() < 2)
                {
                    // One is free
                    this.price = 0;
                }
                else
                {
                    // As a single quantity of this topping comes with the item by default, we only need to charge a single for a double quantity
                    this.price = this.originalPrice;
                }
            }
            else if (this.type == 'additional')
            {
                this.price = this.originalPrice * this.quantity();
            }

            // Adjust for free toppings
            var freePrice = this.freeToppings * this.originalPrice;
            this.price = this.price - freePrice;
        }
    }
}































