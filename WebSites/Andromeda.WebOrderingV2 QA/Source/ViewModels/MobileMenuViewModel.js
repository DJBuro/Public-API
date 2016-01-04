/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function MobileMenuViewModel()
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
    self.titleClass = ko.observable('mobileSectionHome'); // Class to use to style the section name - used for showing an icon for the section

    self.isMobileMenuVisible = true;

    if (viewModel.contentViewModel() != undefined && viewModel.contentViewModel().previousViewName != undefined && viewModel.contentViewModel().previousViewName.length > 0)
    {
        self.previousViewName = viewModel.contentViewModel().previousViewName;
        self.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
    }
    else
    {
        self.previousViewName = guiHelper.getCurrentViewName();
        self.previousContentViewModel = viewModel.contentViewModel();
    };

    self.onLogout = function ()
    {
    };

    this.showHome = function ()
    {
        viewModel.pageManager.showPage('Home', true, undefined, false);
    };

    this.showMenu = function ()
    {
        viewModel.pageManager.showPage('Menu', true, undefined, true);
    };

    this.showFeedback = function ()
    {
        viewModel.pageManager.showPage('Feedback', true, undefined, false);
    };

    this.showStoreDetails = function ()
    {
        viewModel.pageManager.showPage('StoreDetails', true, undefined, true);
    };

    this.myProfile = function ()
    {
         viewModel.pageManager.showPage('MyProfile', true, undefined, false);
    };

    this.myOrders = function ()
    {
        viewModel.pageManager.showPage('MyOrders', true, undefined, false);
    };

    this.logout = function ()
    {
        accountHelper.logout();
    };

    this.mobileLogin = function ()
    {
        if (!accountHelper.loginEnabled()) return;
        viewModel.pageManager.showPage('Login', true, undefined, false);
    };

    this.exitMobileMode = function ()
    {
        guiHelper.showView(viewModel.previousViewName, viewModel.previousContentViewModel);
    };

    this.mobileCancel = function ()
    {
        guiHelper.showView(self.previousViewName, self.previousContentViewModel);
    };
};






































