/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function PasswordResetRequestViewModel()
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

    self.onLogout = function ()
    {
    };

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

    self.emailAddress = ko.observable('');
    self.message = ko.observable('');
    self.resetPassword = function ()
    {
        // Did the user enter an email address?
        if (self.emailAddress().length == 0)
        {
            self.message(textStrings.prNoEmail);
            return;
        }
        else
        {
            // Did the user enter a valid email address?
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!regex.test(self.emailAddress()))
            {
                self.message(textStrings.prInvalidEmail);
                return;
            }
        }

        // Tell Google analytics that a password reset has been requested
        ga
        (
            "send",
            "event",
            {
                eventCategory: "Account",
                eventAction: "PasswordResetRequest",
                eventLabel: "",
                eventValue: 1,
                metric1: 1
            }
        );

        // Request a password reset
        acsapi.putPasswordResetRequest
        (
            self.emailAddress(),
            function ()
            {
                self.message(textStrings.prEmailSent);
            },
            function (errorMessage)
            {
                if (typeof (errorMessage) !== "string")
                {
                    errorMessage = errorMessage.message !== undefined ? errorMessage.message : errorMessage;
                }

                // Did it work?
                if (errorMessage != undefined && errorMessage.length > 0)
                {
                    self.message(errorMessage);

                    // Tell Google analytics that a password reset request failed
                    ga
                    (
                        "send",
                        "event",
                        {
                            eventCategory: "Account",
                            eventAction: "PasswordResetFailed",
                            eventLabel: "",
                            eventValue: 1,
                            metric1: 1
                        }
                    );
                }
                else
                {
                    self.message(textStrings.prGenericError);
                    self.emailAddress('');
                }
            }
        );
    };

    self.resetPasswordCancel = function ()
    {
        viewModel.pageManager.showPreviousPage(true);
    };

    self.keyPress = function (data, event)
    {
        // Did the user press enter?
        if (event.which == 13 || event.keyCode == 13)
        {
            self.resetPassword();
            return false;
        }

        return true;
    };
};








































