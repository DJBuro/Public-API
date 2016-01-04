/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function MercanetPaymentViewModel()
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
    self.title = ko.observable(textStrings.mmHome); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionHome'); // Class to use to style the section name - used for showing an icon for the section

    self.onShown = function ()
    {
        // Inject the Mercanet HTML into the iFrame
        var iFrame = $('#mercanetPaymentIFrame');

        if (html == undefined || html.length == 0)
        {
            html = "Sorry there was problem.  Payment is unavailable at this time.";
        }

        var iFrameDoc = iFrame[0].contentDocument || iFrame[0].contentWindow.document;
        iFrameDoc.write(html);
        iFrameDoc.close();

        // Disable the warning alert
        window.onbeforeunload = undefined;
    }

    self.onLogout = function ()
    {
        guiHelper.showMenu();
    }
};






































