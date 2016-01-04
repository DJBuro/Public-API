/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function menuViewModel()
{
    var self = this;

    self.selectedSection = ko.observable(undefined);
    self.changeToSection = ko.observable(undefined);
    self.currentSection = ko.observable('');
    self.currentSectionIndex = 0;
    self.sections = 0;
    self.eventsEnabled = true;
    self.cartViewModel = new cartViewModel();

    self.onBeforeViewShown = function (viewHtml)
    {
        // Stop knockout triggering a shed load of events during the data binding
        self.eventsEnabled = false;
        return self.showMenuSection(self.currentSectionIndex, viewHtml);
    };
    self.onAfterViewShown = function (viewHtml)
    {
        // Start listneing to events
        self.eventsEnabled = true;
    };
    self.onNavigateCallback = function (direction)
    {
        if (direction == 'backTo')
        {
            app.viewEngine.showView('menuView', self, undefined, false);
        }
    }

    self.showMenuSection = function (sectionIndex, viewHtml)
    {
        if (sectionIndex != undefined)
        {
            sectionIndex = typeof (sectionIndex) == 'number' ? sectionIndex : 0;

            var section = app.site().menu.sections()[sectionIndex];

            // The currently selected section
            self.selectedSection(section);

            self.currentSectionIndex = sectionIndex;

            var menuItemTemplateHtml = $('#menuItem-template').html();
            var menuItemsHtml = '';

            // Inject the html for each menu item into the section
            for (var index = 0; index < self.selectedSection().items().length; index++)
            {
                var menuItem = self.selectedSection().items()[index];

                var itemHtml = menuItemTemplateHtml.replace('<!--NAME-->', menuItem.name);
                itemHtml = itemHtml.replace('<!--DESCRIPTION-->', menuItem.description);
                itemHtml = itemHtml.replace(new RegExp('INDEX', 'g'), index);

                menuItemsHtml += itemHtml;
            }

            // Get the section html
            var html = $('#' + section.templateName).html();
            html = html.replace('<!--MENUITEMS-->', menuItemsHtml);

            // Inject the menu items into the view html
            viewHtml.viewHtml = viewHtml.viewHtml.replace('<!--MENUSECTION-->', html);
        }
        else
        {
//            if (callback != undefined) callback();
        }
    };
    self.changeSection = function ()
    {
        if (this != undefined && this.Index != undefined)
        {
            // Change the section
            self.currentSectionIndex = this.Index;

            // Show the menu view
            app.viewEngine.showView('menuView', self, undefined, true);
        }
        else if (self.changeToSection() != undefined)
        {
            // Change the section
            self.currentSectionIndex = self.changeToSection().Index;

            // Show the menu view
            app.viewEngine.showView('menuView', self, undefined, true);
        }

        $('#mobileMenuSectionsSelect').blur();

        return true;
    };
    self.addItemToCart = function (index, data)
    {
        // Get the item that was selected
        var item = self.selectedSection().items()[index];

        // Get the menu item that the user has selected
        var menuItem = app.site().menu.getSelectedMenuItem(item);

        if (menuItem != null)
        {
            // Get the categories
            var category1 = helper.findById(menuItem.Cat1 == undefined ? menuItem.Category1 : menuItem.Cat1, app.site().menu.rawMenu.Category1);
            var category2 = helper.findById(menuItem.Cat2 == undefined ? menuItem.Category2 : menuItem.Cat2, app.site().menu.rawMenu.Category2);

            // The item to show on the toppings popup
            app.selectedItem.name(item.name);
            app.selectedItem.menuItems(item.menuItems());
            app.selectedItem.description(item.description);
            app.selectedItem.quantity(item.quantity);
            app.selectedItem.menuItem(menuItem);
            app.selectedItem.freeToppings(menuItem.FreeTops);
            app.selectedItem.freeToppingsRemaining(menuItem.FreeTops);
            app.selectedItem.instructions('');
            app.selectedItem.person('');
            app.selectedItem.price(helper.formatPrice(item.price()));

            // Do these last becuase the UI is data bound to them
            app.selectedItem.category1s.removeAll();
            for (var index = 0; index < item.category1s().length; index++)
            {
                app.selectedItem.category1s.push(item.category1s()[index]);
            }
            app.selectedItem.category2s.removeAll();
            for (var index = 0; index < item.category2s().length; index++)
            {
                app.selectedItem.category2s.push(item.category2s()[index]);
            }
            app.selectedItem.category1(category1);
            app.selectedItem.category2(category2);
            app.selectedItem.selectedCategory1(item.selectedCategory1());
            app.selectedItem.selectedCategory2(item.selectedCategory2());

            // Get the toppings
            app.selectedItem.toppings(app.site().menu.getItemToppings(app.selectedItem.menuItem()));

            // Calculate the price
            var price = app.site().menu.calculateItemPrice(menuItem, item.quantity, app.selectedItem.toppings());

            // Set the price
            app.selectedItem.price(helper.formatPrice(price));

            // Does the customer need to pick any toppings?
            if (app.selectedItem.toppings().length == 0)
            {
                // Add the the cart
                app.site().cart.commitToCart();

                // If we are in mobile mode show the cart
                if (app.isMobileMode())
                {
                    guiHelper.showCart();
                }
            }
            else
            {
                // Show the toppings popup
                popupHelper.returnToCart = false;
                popupHelper.showPopup('addItem');
            }
        }
    };
    self.category1Changed = function (index, data)
    {
        if (self.eventsEnabled)
        {
            var item = self.selectedSection().items()[index];
            self.selectedItemChanged(item);

            // Clear the category2 list
            item.category2s.removeAll();

            // Rebuild the category2 list based on the selected category1
            for (var index = 0; index < item.menuItems().length ; index++)
            {
                var checkItem = item.menuItems()[index];

                // Does the menu item have the selected category1?
                if (item.selectedCategory1().Id == (checkItem.Cat1 == undefined ? checkItem.Category1 : checkItem.Cat1))
                {
                    // Is this items category 2 already in the list?
                    var category = helper.findById(checkItem.Cat2 == undefined ? checkItem.Category2 : checkItem.Cat2, app.site().menu.rawMenu.Category2);
                    if (category != undefined && !helper.findCategory(category, item.category2s))
                    {
                        item.category2s.push(category);
                    }
                }
            }

            // Figure out which menu item the categories relate to
            self.selectedItemChanged(item);
        }
    };
    self.selectedItemIndexChanged = function (index, data)
    {
        if (self.eventsEnabled)
        {
            var item = self.selectedSection().items()[index];
            self.selectedItemChanged(item);
        }
    }
    self.selectedItemChanged = function (item)
    {
        if (self.eventsEnabled && item != undefined)
        {
            // Get the menu item that the user has selected
            var menuItem = app.site().menu.getSelectedMenuItem(item);

            if (menuItem != undefined)
            {
                var quantity = (typeof (item.quantity) == 'function' ? item.quantity() : item.quantity);
                var toppings = (typeof (item.toppings) == 'function' ? item.toppings() : undefined);

                // Update the price
                item.price(helper.formatPrice(app.site().menu.calculateItemPrice(menuItem, quantity, toppings)));
            }
        }
    };
    self.modifyOrder = function ()
    {
        self.cartViewModel.modifyOrder();
    };
    self.checkout = function ()
    {
        self.cartViewModel.checkout();
    };
    self.showMenu = function ()
    {
        self.cartViewModel.showMenu();
    };
}