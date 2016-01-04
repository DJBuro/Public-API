/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function PleaseWaitViewModel(onShownCallback)
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(false);
    self.isShowHomeButtonVisible = ko.observable(false);
    self.isShowMenuButtonVisible = ko.observable(false);
    self.isShowCartButtonVisible = ko.observable(false);

    self.isHeaderVisible = ko.observable(false);
    self.isPostcodeSelectorVisible = ko.observable(false);
    self.areHeaderOptionsVisible = ko.observable(false);
    self.isHeaderLoginVisible = ko.observable(false);

    // Mobile mode
    self.title = ko.observable(textStrings.mmMyOrder); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionCart'); // Class to use to style the section name - used for showing an icon for the section

    self.cannotBePrevious = true;

    self.onShownCallback = onShownCallback;

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

    self.onLogout = function ()
    {
        viewModel.chooseStore();
    }

    self.onShown = function()
    {
        if (self.onShownCallback !== undefined) self.onShownCallback();
    }

    self.back = function ()
    {
        guiHelper.showView(self.previousViewName, self.previousContentViewModel);
    }
};






































