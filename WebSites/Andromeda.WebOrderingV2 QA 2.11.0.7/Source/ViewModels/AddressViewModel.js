/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function AddressViewModel()
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

    popupViewModel: ko.observable(),

    self.onLogout = function ()
    {
    }

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

    self.cancel = function ()
    {
        // Clearup
        accountHelper.newAccount.password('');
        accountHelper.newAccount.reenterPassword('');
        
        // Go back to the previous view
        guiHelper.showView(self.previousViewName, self.previousContentViewModel);
    };

    self.keyPress = function (data, event)
    {
        // Did the user press enter?
        if ((event.which >= 48 && event.which <= 57) ||
            (event.keyCode >= 48 && event.keyCode <= 57))
        {
            return true;
        }

        return false;
    }
};






































