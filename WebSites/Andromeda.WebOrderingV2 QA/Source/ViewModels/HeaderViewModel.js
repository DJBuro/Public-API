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
        viewModel.pageManager.showPage('Home', true, undefined, false);
    };

    self.showMenu = function ()
    {
        viewModel.pageManager.showPage('Menu', true, undefined, true);
    };

    self.showCart = function ()
    {
        viewModel.pageManager.showPage('Cart', true, undefined, true);
    };

    self.showStoreDetails = function () {
        viewModel.pageManager.showPage('StoreDetails', true, undefined, true);
    };

    self.showFeedback = function () {
        viewModel.pageManager.showPage('Feedback', true, undefined, false);
    };
    self.cmsPage = function (cmsPage) {
        var name = 'Pages/' + cmsPage.Title;
        viewModel.pageManager.showPage(name, true, undefined, false);
    };

    self.login = function ()
    {
        if (!accountHelper.loginEnabled()) return;

        viewModel.pageManager.showPage('Login', true, undefined, false);

        self.isHomeSelected(false);
        self.isMenuSelected(false);
        self.isCartSelected(false);
        self.isMyProfileSelected(true);
        self.isStoreDetailsSelected(false);
    };

    self.mobileLogin = function ()
    {
        if (!accountHelper.loginEnabled()) return;

        viewModel.pageManager.showPage('Login', true, undefined, false);

        self.isHomeSelected(false);
        self.isMenuSelected(false);
        self.isCartSelected(false);
        self.isMyProfileSelected(true);
        self.isStoreDetailsSelected(false);
    };

    self.myProfile = function ()
    {
        viewModel.pageManager.showPage('MyProfile', true, undefined, false);
    };

    self.myOrders = function ()
    {
        viewModel.pageManager.showPage('MyOrders', true, undefined, false);
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
        else
        {
            // Show the home page
            viewHelper.showHome();
        }
    }

    self.showMobileMenu = function ()
    {
        if (viewModel.contentViewModel().isMobileMenuVisible === undefined)
        {
            viewModel.pageManager.showPage('MobileMenu', false, undefined, false);
        }
        else
        {
            viewModel.contentViewModel().mobileCancel();
        }
    }

    self.mobileForgotPassword = function ()
    {
        viewModel.pageManager.showPage('PasswordResetRequest', true, undefined, false);
    }
};






































