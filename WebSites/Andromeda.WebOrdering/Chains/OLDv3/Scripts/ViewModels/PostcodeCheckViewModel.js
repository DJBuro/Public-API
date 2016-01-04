/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function postcodeCheckViewModel()
{
    var self = this;

    self.postcode = '';
    self.isInDeliveryZone = ko.observable(undefined);
    
    self.onNavigateCallback = function (direction)
    {
        if (direction == 'backFrom')
        {
            app.viewEngine.hidePopup();
        }
    }

    self.onTransitionToMode = function (toMode)
    {
        if (toMode == 'mobile')
        {
            app.viewEngine.hidePopup();
            app.viewEngine.showView('postcodeCheckView', self, undefined, true);
        }
        else if (toMode == 'tabletPc')
        {
            app.viewEngine.showView('homeView', new homeViewModel(), undefined, true);
            app.viewEngine.showPopup('postcodeCheckView', self);
        }
    };
    self.checkPostcode = function ()
    {
        self.isInDeliveryZone(app.site().isInDeliveryZone(self.postcode));
    };
    self.showMenu = function ()
    {
        if (app.isMobileMode())
        {
            // Continue showing the menu
            //postcodeCheckHelper.hidePopup(guiHelper.showHome);
        }
        else
        {
            // Continue showing the menu
            app.viewEngine.hidePopup();
        }
    };
    self.changeStore = function ()
    {
        // Continue showing the menu
//        postcodeCheckHelper.hidePopup(siteHelper.chooseStore);
    };
}