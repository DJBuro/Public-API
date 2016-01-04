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
        this.hideMenu();

        viewModel.pageManager.showPage('Home', true, undefined, false);
    };

    this.showMenu = function ()
    {
        this.hideMenu();

        viewModel.pageManager.showPage('Menu', true, undefined, true);
    };

    this.showCart = function ()
    {
        this.hideMenu();

        viewModel.headerViewModel().showCart();
    }

    this.showFeedback = function ()
    {
        this.hideMenu();

        viewModel.pageManager.showPage('Feedback', true, undefined, false);
    };

    this.showStoreDetails = function ()
    {
        this.hideMenu();

        viewModel.pageManager.showPage('StoreDetails', true, undefined, true);
    };

    this.myProfile = function ()
    {
        this.hideMenu();

        viewModel.pageManager.showPage('MyProfile', true, undefined, false);
    };

    this.myOrders = function ()
    {
        this.hideMenu();

        viewModel.pageManager.showPage('MyOrders', true, undefined, false);
    };

    this.logout = function ()
    {
        this.hideMenu();

        accountHelper.logout();
    };

    this.mobileLogin = function ()
    {
        if (!accountHelper.loginEnabled()) return;

        this.hideMenu();

        viewModel.pageManager.showPage('Login', true, undefined, false);
    };

    this.exitMobileMode = function ()
    {
        this.hideMenu();
    };

    this.mobileCancel = function ()
    {
        this.hideMenu();
    };

    this.showMobileMenu = function ()
    {
        // Reset the mobile menu classes ready for animation

        // Background
        $('#mobileMenuBackground').removeClass("mobileMenuBackgroundFadeIn");
        $('#mobileMenuBackground').removeClass("mobileMenuBackgroundFadeOut");
        $('#mobileMenuBackground').removeClass("mobileMenuBackgroundVisible");
        $('#mobileMenuBackground').addClass("mobileMenuBackgroundHidden");

        // Mobile menu
        $('#mobileMenuInner').removeClass("mobileMenuSlideOut");
        $('#mobileMenuInner').removeClass("mobileMenuSlideIn");
        $('#mobileMenuInner').removeClass("mobileMenuOut");
        $('#mobileMenuInner').addClass("mobileMenuIn");

        // Make sure the mobile menu is visible before starting the animations
        $('#mobileMenu').css("display", "block");

        // Make sure the class changes have taken affect
        setTimeout
        (
            function ()
            {
                // Setup the animations
                $('#mobileMenuBackground').addClass("mobileMenuBackgroundFadeIn");
                $('#mobileMenuInner').addClass("mobileMenuSlideOut");

                setTimeout
                (
                    function (mobileMenuBackgroundVisible)
                    {
                        // Start the animations
                        $('#mobileMenuBackground').addClass("mobileMenuBackgroundVisible");
                        $('#mobileMenuInner').addClass("mobileMenuOut");
                    }, 0
                );

            }, 0
        );
    };

    this.hideMenu = function ()
    {
        // Reset the mobile menu classes ready for animation

        // Background
        $('#mobileMenuBackground').addClass("mobileMenuBackgroundVisible");

        // Mobile menu
        $('#mobileMenuInner').addClass("mobileMenuOut");

        // Make sure the mobile menu is visible before starting the animations
        $('#mobileMenu').css("display", "block");

        // Make sure the class changes have taken affect
        setTimeout
        (
            function ()
            {
                // Setup the animations
                $('#mobileMenuBackground').addClass("mobileMenuBackgroundFadeOut");
                $('#mobileMenuInner').addClass("mobileMenuSlideIn");

                setTimeout
                (
                    function (mobileMenuBackgroundVisible)
                    {
                        // We need to hide the mobile menu when the animation has finished
                        $('#mobileMenuInner').bind
                        (
                            'animationEnd webkitAnimationEnd',
                            function ()
                            {
                                $('#mobileMenu').css("display", "none");
                                $('#mobileMenuInner').unbind("animationEnd webkitAnimationEnd");
                            }
                        );

                        // Start the animations
                        $('#mobileMenuBackground').addClass("mobileMenuBackgroundHidden");
                        $('#mobileMenuInner').addClass("mobileMenuIn");
                    }, 0
                );

            }, 0
        ); 
    };
};






































