/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function MyOrdersViewModel()
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(true);
    self.isShowHomeButtonVisible = ko.observable(true);
    self.isShowMenuButtonVisible = ko.observable(true);
    self.isShowCartButtonVisible = ko.observable(true);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(settings.storeSelectorMode && settings.storeSelectorInHeader);
    self.areHeaderOptionsVisible = ko.observable(true);
    self.isHeaderLoginVisible = ko.observable(true);

    // Mobile mode
    self.title = ko.observable(textStrings.mmChangeStoreButton); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionSelectStore'); // Class to use to style the section name - used for showing an icon for the section

    if (viewModel.contentViewModel() != undefined && viewModel.contentViewModel().previousViewName != undefined && viewModel.contentViewModel().previousViewName.length > 0)
    {
        self.previousViewName = viewModel.contentViewModel().previousViewName;
        self.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
    }
    else
    {
        self.previousViewName = guiHelper.getCurrentViewName();
        self.previousContentViewModel = viewModel.contentViewModel();
    }

    self.onShown = function ()
    {
        myOrdersHelper.refresh();
    };

    self.onLogout = function ()
    {
        this.backToMenu();
        if (typeof (self.previousContentViewModel.onLogout) == 'function')
        {
            self.previousContentViewModel.onLogout();
        }
    };

    self.displayOrderDetails = function (order)
    {
        myOrdersHelper.getOrder(order);
    };

    self.myProfile = function ()
    {
        guiHelper.showView('myProfileView', new MyProfileViewModel());
    };

    self.logout = function ()
    {
        viewModel.headerViewModel().logout();
    }

    self.backToMenu = function ()
    {
        guiHelper.showView(self.previousViewName, self.previousContentViewModel);
    };

    self.repeatOrder = function (order)
    {
        myOrdersHelper.repeatOrder({ orderTotal: order.total(), newOrderTotal: undefined, orderLines: [] });

        if (viewModel.selectedSite() == undefined)
        {
            alert('please select a store to order from first');
        }
        else
        {
            for (var orderLineIndex = 0; orderLineIndex < order.orderLines().length; orderLineIndex++)
            {
                var orderLine = order.orderLines()[orderLineIndex];

                // Try and find the menu item in the currently loaded menu
                var menuItem = menuHelper.menuItemLookup[orderLine.id];
                
                // Does the menu contain the menu item?
                if (menuItem == undefined || menuItem.name.toUpperCase() != orderLine.name.toUpperCase())
                {
                    // The menu item does not exist in the menu
                    myOrdersHelper.repeatOrder().orderLines.push
                    (
                        {
                            problem: textStrings.roDiscontinued,
                            menuItem: undefined,
                            orderLine: orderLine,
                            newPrice: undefined
                        }
                    );
                }
                else
                {
                    var newPrice = menuHelper.getItemPrice(menuItem.menuItem);
                    if (newPrice != orderLine.price)
                    {
                        // The menu item exists in the menu but the prices are different
                        myOrdersHelper.repeatOrder().orderLines.push
                        (
                            {
                                problem: '',
                                menuItem: menuItem,
                                orderLine: orderLine,
                                newPrice: newPrice
                            }
                        );
                    }
                    else
                    {
                        // The mneu item exists in the menu and appears to be identical
                        myOrdersHelper.repeatOrder().orderLines.push
                        (
                            {
                                problem: '',
                                menuItem: menuItem,
                                orderLine: orderLine,
                                newPrice: undefined
                            }
                        );
                    }
                }
            }

            guiHelper.showView('repeatOrderView', new RepeatOrderViewModel());
        }
    };
};






































