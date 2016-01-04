/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function homeViewModel()
{
    var self = this;
    self.etd = function ()
    {
        var etd = template_defaultETD;
        if (app.site().estDelivTime != undefined && app.site().estDelivTime > 0)
        {
            etd = app.site().estDelivTime;
        }

        return etd;
    };

    self.onAfterViewShown = function ()
    {
        app.headerViewModel.isMainMenuVisible(true);

        // Initialise the map
        mapHelper.initialiseMap();
    };

    self.onNavigateCallback = function (direction)
    {
        if (direction == 'backTo')
        {
            app.viewEngine.showView('homeView', self, undefined, false);
        }
    }

    self.showMenuSection = function ()
    {
        var viewModel = new menuViewModel();
        viewModel.currentSectionIndex = this.Index;
        app.viewEngine.showView('menuView', viewModel, undefined, true);
    };

    self.showPostcodeCheck = function ()
    {
        guiHelper.showPostcodeCheck();
    };
}