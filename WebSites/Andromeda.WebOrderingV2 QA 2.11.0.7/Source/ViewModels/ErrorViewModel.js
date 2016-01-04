/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function ErrorViewModel()
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(false);
    self.isShowHomeButtonVisible = ko.observable(false);
    self.isShowMenuButtonVisible = ko.observable(false);
    self.isShowCartButtonVisible = ko.observable(false);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(false);
    self.areHeaderOptionsVisible = ko.observable(false);
    self.isHeaderLoginVisible = ko.observable(false);

    // Mobile mode
    self.title = ko.observable(textStrings.mmMyOrder); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionCart'); // Class to use to style the section name - used for showing an icon for the section

    self.onLogout = function ()
    {
        viewModel.chooseStore();
    }
};






































