/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />

module AndroWeb.Models
{
    export class CartDealLine
    {
        public dealLineWrapper: any;
        public allowableMenuItemWrappers: any[] = [];
        public selectedAllowableMenuItemWrapper: KnockoutObservable<any> = ko.observable(undefined); // This is the selected item but NOT the specific menu item variation e.g. Size
        public cartItem: KnockoutObservable<any> = ko.observable(); // This is the ACTUAL menu item selected - each item could include multiple menu items for different cat1/cat2s - this is the menu item the customer wants
        public hasError: KnockoutObservable<boolean> = ko.observable(false);
        public instructions: string = "";
        public person: string = "";
        public id: string = "";
        public name: KnockoutObservable<string> = ko.observable('');
        public displayPrice: KnockoutObservable<string> = ko.observable("");
        public categoryPremium: KnockoutObservable<number> = ko.observable(0);
        public categoryPremiumName: KnockoutObservable<string> = ko.observable("");
        public itemPremium: KnockoutObservable<number> = ko.observable(0);
        public itemPremiumName: KnockoutObservable<string> = ko.observable("");
        public selectedCategoriesText: KnockoutObservable<string> = ko.observable("");
        public displayName: KnockoutObservable<string> = ko.observable("");

        constructor(dealLineWrapper, id)
        {
            this.dealLineWrapper = dealLineWrapper;
            this.id = id;

            // Build the allowable items in the deal line
            for (var itemIndex = 0; itemIndex < dealLineWrapper.menuItemWrappers.length; itemIndex++)
            {
                var menuItemWrapper = dealLineWrapper.menuItemWrappers[itemIndex];

                if (!menuItemWrapper.isEnabled) continue

                if (menuItemWrapper.price)
                {
                    var items = menuItemWrapper.menuItems();
                    var selectedCategory = menuItemWrapper.selectedCategory1();

                    if (selectedCategory !== undefined)
                    {
                        var rightType = items.filter
                            (
                                function (item)
                                {
                                    return item.Cat1 == selectedCategory.Id;
                                }
                            );
                        menuItemWrapper.menuItem = rightType[0];
                    }
                }

                var allowableMenuItemWrapper =
                    {
                        cartDealLine: this, // The deal line that the item is in - we're data binding to the item but we need a way to get back to the deal line the item is in
                        menuItemWrapper: menuItemWrapper,
                        name: menuItemWrapper.name
                    };

                this.allowableMenuItemWrappers.push(allowableMenuItemWrapper);
            }

            this.allowableMenuItemWrappers.sort(function (a, b)
            {
                if (!a.menuItemWrapper.menuItem || !b.menuItemWrapper.menuItem)
                {
                    return -1;
                }

                if (a.menuItemWrapper.menuItem.DelPrice < b.menuItemWrapper.menuItem.DelPrice)
                {
                    return -1;
                }

                if (a.menuItemWrapper.menuItem.DelPrice > b.menuItemWrapper.menuItem.DelPrice)
                {
                    return 1;
                }

                return 0;
            });

            // Auto select the first menu item if there is only one
            if (this.allowableMenuItemWrappers.length == 1)
            {
                // Select the first menu item
                this.selectedAllowableMenuItemWrapper(this.allowableMenuItemWrappers[0]);

                // Make sure the cart deal line state is upto date after the change
                this.refreshSelectedAllowableMenuItemWrapper(dealLineWrapper.dealLine);
            }
        }

        refreshSelectedAllowableMenuItemWrapper(dealLine: any): void
        {
            // Each menu item wrapper can contain multiple menu items (sizes) - default to the first
            if (this.selectedAllowableMenuItemWrapper() === undefined)
            {
                // The "please select an item" option was selected - not a real menu item
                this.cartItem(undefined);
            }
            else
            {
                var selectedMenuItem = this.selectedAllowableMenuItemWrapper().menuItemWrapper.menuItems()[0];

                // For percentage deals we don't apply deal premiums
                var includeDealLinePremium: boolean = (dealLine.Type.toLowerCase() != "percentage");

                // Create a cart item for the selected menu item
                var cartItem = new AndroWeb.Models.CartItem
                (
                    viewModel,
                    this.selectedAllowableMenuItemWrapper().menuItemWrapper,
                    selectedMenuItem,
                    true,
                    includeDealLinePremium
                );
                this.cartItem(cartItem);
            }
        }
    }
}






























