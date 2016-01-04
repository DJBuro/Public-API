/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function cartViewModel()
{
    var self = this;

    self.onTransitionToMode = function (toMode)
    {
        if (toMode == 'mobile')
        {
            app.viewEngine.hidePopup();
            app.viewEngine.showView('cartView', self, undefined, true);
        }
        else if (toMode == 'tabletPc')
        {
            var viewModel = new menuViewModel();
            viewModel.cartViewModel = self;

            app.viewEngine.showView('menuView', viewModel, undefined, true);
        }
    };

    self.modifyOrder = function ()
    {
        guiHelper.canChangeOrderType(true);
        guiHelper.cartActions(guiHelper.cartActionsMenu);

        // Allow the user modify the cart items
        guiHelper.isCartLocked(false);

        if (guiHelper.isMobileMode())
        {
            // Show the cart
            guiHelper.isMobileMenuVisible(true);
            checkoutMenuHelper.hideMenu();
            guiHelper.showCart();
        }
        else
        {
            // Show the menu
            guiHelper.showMenuView('menuSectionsView');
        }
    };

    self.checkout = function ()
    {
    };

    self.showMenu = function ()
    {
    };

    self.editCartItem = function ()
    {
        // Get the menu item that the user has selected
        app.site().cart.selectedCartItem(this);

        var price = app.site().menu.calculateItemPrice(this.menuItem, this.quantity(), this.toppings);

        // The item to show on the toppings popup
        app.selectedItem.name(this.name);
        app.selectedItem.description(app.site().menu.fixName(this.menuItem.Desc == undefined ? this.menuItem.Description : this.menuItem.Desc));
        app.selectedItem.quantity(this.quantity);
        app.selectedItem.menuItem(this.menuItem);
        app.selectedItem.instructions(this.instructions);
        app.selectedItem.person(this.person);
        app.selectedItem.category1s.removeAll();
        for (var index = 0; index < this.category1s().length; index++)
        {
            app.selectedItem.category1s.push(this.category1s()[index]);
        }
        app.selectedItem.category2s.removeAll();
        for (var index = 0; index < this.category2s().length; index++)
        {
            app.selectedItem.category2s.push(this.category2s()[index]);
        }
        app.selectedItem.toppings(this.toppings());
        app.selectedItem.selectedCategory1(this.selectedCategory1);
        app.selectedItem.selectedCategory2(this.selectedCategory2);

        app.selectedItem.price(helper.formatPrice(price));

        // Show the toppings popup
        popupHelper.returnToCart = true; // In mobile mode the cart is a seperate view so we need to show the view
        popupHelper.showPopup('editItem');
    };
}