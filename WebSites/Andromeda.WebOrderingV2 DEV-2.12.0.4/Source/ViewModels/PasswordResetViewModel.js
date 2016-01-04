/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function PasswordResetViewModel()
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(true);
    self.isShowHomeButtonVisible = ko.observable(true);
    self.isShowMenuButtonVisible = ko.observable(true);
    self.isShowCartButtonVisible = ko.observable(true);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(settings.storeSelectorMode && settings.storeSelectorInHeader);
    self.areHeaderOptionsVisible = ko.observable(false);
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

    self.password = ko.observable('');
    self.repeatPassword = ko.observable('');
    self.message = ko.observable('');
    self.resetPassword = function ()
    {
        // Did the user enter a password?
        if (self.password().length == 0)
        {
            self.message(textStrings.prNoPassword);
            return;
        }
        
        // Did the user repeat the password?
        if (self.repeatPassword().length == 0)
        {
            self.message(textStrings.prNoRepeatPassword);
            return;
        }

        // Passwords do not match
        if (self.password() != self.repeatPassword())
        {
            self.message(textStrings.prPasswordsDoNotMatch);
            return;
        }

        if (!accountHelper.validatePassword(self.password()))
        {
            self.message(textStrings.prPasswordToShort);
            return;
        }

        // Request a password reset
        acsapi.postPasswordResetRequest
        (
            queryStringHelper.forUser,
            queryStringHelper.passwordReset,
            self.password(),
            function (errorMessage)
            {
                // Did it work?
                if (errorMessage != undefined && errorMessage.length > 0)
                {
                    self.message(errorMessage);
                }
                else
                {
                    //if (settings.parentWebsite === undefined || settings.parentWebsite === '')
                    //{
                        window.location = window.location.origin;
                    //}
                    //else
                    //{
                    //    window.location = settings.parentWebsite;
                    //}
                }
            },
            function (errorMessage)
            {
                if (errorMessage != undefined && errorMessage.length > 0)
                {
                    self.message(errorMessage);
                }
                else
                {
                    self.message(textStrings.prGenericError);
                }
            }
        );
    };

    self.resetPasswordCancel = function ()
    {
        guiHelper.showView(self.previousViewName, self.previousContentViewModel);
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








































