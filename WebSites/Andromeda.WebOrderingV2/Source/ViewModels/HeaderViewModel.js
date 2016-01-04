/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function HeaderViewModel()
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(true);
    self.isShowHomeButtonVisible = ko.observable(true);
    self.isShowMenuButtonVisible = ko.observable(true);
    self.isShowCartButtonVisible = ko.observable(true);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(false);
    self.areHeaderOptionsVisible = ko.observable(true);
    self.isHeaderLoginVisible = ko.observable(true);

    // Mobile mode
    self.title = ko.observable(''); // Current section name - shown in the header
    self.titleClass = ko.observable(''); // Class to use to style the section name - used for showing an icon for the section

    self.username = ko.observable('');
    self.password = ko.observable('');

    self.isHomeSelected = ko.observable(true);
    self.isMenuSelected = ko.observable(false);
    self.isCartSelected = ko.observable(false);
    self.isMyProfileSelected = ko.observable(false);
    self.isStoreDetailsSelected = ko.observable(false);

    self.showHome = function ()
    {
        viewModel.pageManager.showPage('Home', true);
    };

    self.showMenu = function ()
    {
        viewModel.pageManager.showPage('Menu', true);
    };

    self.showCart = function ()
    {
        viewModel.pageManager.showPage('Cart', true);
    };

    self.showStoreDetails = function()
    {
        viewModel.pageManager.showPage('StoreDetails', true);
    }

    self.showFeedback = function ()
    {
        viewModel.pageManager.showPage('Feedback', true);
    }

    self.login = function ()
    {
        if (!accountHelper.loginEnabled()) return;

        viewModel.pageManager.showPage('Login', true);

        self.isHomeSelected(false);
        self.isMenuSelected(false);
        self.isCartSelected(false);
        self.isMyProfileSelected(true);
        self.isStoreDetailsSelected(false);
    };

    self.mobileLogin = function ()
    {
        if (!accountHelper.loginEnabled()) return;

        viewModel.pageManager.showPage('Login', true);

        self.isHomeSelected(false);
        self.isMenuSelected(false);
        self.isCartSelected(false);
        self.isMyProfileSelected(true);
        self.isStoreDetailsSelected(false);
    };

    self.myProfile = function ()
    {
        viewModel.pageManager.showPage('MyProfile', true);
    };

    self.myOrders = function ()
    {
        viewModel.pageManager.showPage('MyOrders', true);
    };

    self.logout = function ()
    {
        self.username('');
        self.password('');
        accountHelper.logout();
    };

    self.keyPress = function (data, event)
    {
        // Did the user press enter?
        if (event.which == 13 || event.keyCode == 13)
        {
            self.login();
            return false;
        }

        return true;
    }

    self.logoClicked = function ()
    {
        if (settings.logoClickReturnToStoreSelector)
        {
            // Show the store selector
            viewModel.chooseStore();
        }
        else if (settings.logoClickGotoParentWebsite)
        {
            // Redirect the browser to the parent website
            window.location.href = settings.logoClickUrl;
        }
        else if (viewModel.selectedSite() !== undefined)
        {
            // Show the home page
            viewHelper.showHome();
        }
    }

    self.showMobileMenu = function ()
    {
        viewModel.pageManager.showPage('MobileMenu', false);
    }

    self.mobileForgotPassword = function ()
    {
        viewModel.pageManager.showPage('PasswordResetRequest', true);
    }
};






































