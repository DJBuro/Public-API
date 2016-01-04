/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function headerViewModel()
{
    var self = this;

    self.phone = ko.observable(undefined);
    self.isMainMenuVisible = ko.observable(false);
    self.displayAddress = ko.observable('');

    self.onTransitionToMode = function (toMode)
    {
        if (toMode == 'tabletPc')
        {
            // Hide the mobile menu popup
            app.viewEngine.hidePopup();
        }
    };

    self.checkPostcode = function ()
    {
        //postcodeCheckHelper.showPopup();
        app.viewEngine.showPopup('postcodeCheckView', new postcodeCheckViewModel())
    };

    self.changeStore = function ()
    {
        // Show the sites view
        var viewModel = new sitesViewModel();
    };

    self.menu = function ()
    {
        // Show the menu view
        app.viewEngine.showView('menuView', new menuViewModel(), undefined, true);
    };

    self.home = function ()
    {
        // Show home view
        app.viewEngine.showView('homeView', new homeViewModel(), undefined, true);
    };

    self.mobileMenu = function ()
    {
        self.showMobileMenu();
    };

    self.showMobileMenu = function ()
    {
        app.viewEngine.showPopup('mobileMenuView', this);
    };

    self.mobileMenuShowHome = function ()
    {
        self.mobileMenuTitle('HOME');
        self.mobileMenuTitleClass('popupMobileMenuHomeButton');

        app.viewEngine.hidePopup();
        app.viewEngine.showView('homeView', new homeViewModel(), undefined, true);
    };

    self.mobileMenuShowMenu = function ()
    {
        self.mobileMenuTitle('ORDER NOW');
        self.mobileMenuTitleClass('popupMobileMenuOrderNowButton');
        
        app.viewEngine.hidePopup();
        app.viewEngine.showView('menuView', new menuViewModel(), undefined, true);
    };

    self.mobileMenuShowCart = function ()
    {
        self.mobileMenuTitle('MY ORDER');
        self.mobileMenuTitleClass('popupMobileMenuCartButton');

        app.viewEngine.hidePopup();
        app.viewEngine.showView('cartView', new cartViewModel(), undefined, true);
    };

    self.mobileMenuShowPostcodeCheck = function ()
    {
        self.mobileMenuTitle('POSTCODE CHECK');
        self.mobileMenuTitleClass('popupMobileMenuPostcodeCheckButton');

        app.viewEngine.hidePopup();
        app.viewEngine.showView('postcodeCheckView', new postcodeCheckViewModel());
    };

    self.closeMobileMenu = function ()
    {
        self.mobileMenuHelper.isPopupVisible(false);
    };

    self.mobileMenuTitle = ko.observable('Home');

    self.mobileMenuTitleClass = ko.observable('popupMobileMenuHomeButton');
}