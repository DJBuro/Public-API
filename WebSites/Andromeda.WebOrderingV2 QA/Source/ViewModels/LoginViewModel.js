/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function LoginViewModel(isCheckoutLogin)
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

    self.isLoginMode = ko.observable(true);

    self.toggleToLogin = function ()
    {
        self.isLoginMode(true);
    };

    self.toggleToRegister = function ()
    {
        self.isLoginMode(false);
    };

    // Mobile mode
    self.title = ko.observable(textStrings.mmChangeStoreButton); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionSelectStore'); // Class to use to style the section name - used for showing an icon for the section

    self.isCheckoutLogin = isCheckoutLogin;

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

    customerHelper.bindableCustomer(new Customer());
    accountHelper.registerError(undefined);
    self.canModifyEmailAddress = true;
    self.isEmailAddressVisible = true;
    self.showMarketingOptions = false;
    self.isCurrentPasswordVisible = false;

    self.onShown = function ()
    {
        $('#loginFormInput').focus();
    };

    self.register = function()
    {
        // Touch all the fields to make sure the errors labels are shown
        var customer = customerHelper.bindableCustomer();
        if (customer.firstName() === undefined) customer.firstName('');
        if (customer.emailAddress() === undefined) customer.emailAddress('');
        if (customer.password() === undefined) customer.password('');
        if (customer.phoneNumber() === undefined) customer.phoneNumber('');

        // Customer wants to register
        if (!customerHelper.bindableCustomer().validate(true)) return;

        var customer = customerHelper.bindableCustomer();

        var customerDetails =
        {
            title: '',
            firstname: customer.firstName(),
            surname: customer.surname(),
            contacts:
            [
                {
                    type: 'Email',
                    value: customer.emailAddress(),
                    marketingLevel: customer.marketing() ? '3rdParty' : 'OrderOnly'
                },
                {
                    type: 'Mobile',
                    value: customer.phoneNumber(),
                    marketingLevel: customer.smsMarketing() ? '3rdParty' : 'OrderOnly'
                }
            ],
            address: undefined,
            facebookId: customer.facebookId(),
            facebookUsername: customer.facebookUsername()
        };

        var p =
        {
            userName: customer.emailAddress(),
            password: customer.password()
        };

        guiHelper.showPleaseWait
        (
            textStrings.lrRegistering,
            '',
            function (pleaseWaitViewModel)
            {
                // Validation passed - try and create a new customer account
                acsapi.putCustomer
                (
                    viewModel.selectedSite() === undefined ? undefined : viewModel.selectedSite().siteId,
                    p.userName,
                    p.password,
                    customerDetails,
                    function (putCustomerResponse)
                    {
                        if (putCustomerResponse === undefined || putCustomerResponse === null)
                        {
                            // No response from server - shouldn't be possible so assume error
                            customer.errorMessage(textStrings.lrUnableToCreateAccount);
                            accountHelper.registerError(textStrings.lrUnableToCreateAccount);

                            viewModel.pageManager.showPage('Login', true, self, false);
                        }
                        else
                        {
                            // Was an error returned?
                            if (putCustomerResponse.errorCode === undefined)
                            {
                                viewModel.telemetry.sendTelemetryData('Account', 'Register', 'Completed');

                                accountHelper.registerError(undefined);

                                // We need to keep hold of the account number returned by the server
                                customerDetails.accountNumber = putCustomerResponse.accountNumber;
                                customerDetails.loyalties = putCustomerResponse.loyalties;

                                // Success
                                accountHelper.loggedIn
                                (
                                    customerDetails,
                                    p.userName,
                                    customer.emailAddress(),
                                    p.password,
                                    self.isFacebookRegister
                                );

                                // Return to the previous view or go to the checkout page?
                                // Are we logging in to checkout?
                                if (self.isCheckoutLogin)
                                {
                                    cartHelper.checkout();

                                    // Switch the cart to checkout mode
                                    guiHelper.isMobileMenuVisible(false);
                                    guiHelper.canChangeOrderType(false);
                                    guiHelper.cartActions(guiHelper.cartActionsCheckout);
                                }
                                else
                                {
                                    // Just a normal login.  Return to the previous view
                                    viewModel.pageManager.showPreviousPage(true, undefined, true);
                                }
                            }
                            else
                            {
                                // Server returned an error
                                if (putCustomerResponse.errorCode == 1041)
                                {
                                    customer.errorMessage(textStrings.lrEmailAlreadyUsed);
                                    customer.emailAddressHasError(true);
                                    accountHelper.registerError(textStrings.lrEmailAlreadyUsed);
                                }
                                else
                                {
                                    customer.errorMessage(textStrings.lrUnableToCreateAccount);
                                    accountHelper.registerError(textStrings.lrUnableToCreateAccount);
                                }

                                viewModel.pageManager.showPage('Login', true, self, false);
                            }
                        }
                    },
                    function ()
                    {
                        viewModel.pageManager.showPage('Login', true, self, false);
                    }
                );
            }
        );
    }

    self.cancel = function()
    {
        viewModel.pageManager.showPreviousPage(true, undefined, true);
    }

    self.loginRegisterKeyPress = function (data, event)
    {
        // Did the user press enter?
        if (event.which == 13 || event.keyCode == 13)
        {
            self.login();
            return false;
        }

        return true;
    }

    self.facebookLogin = function (response)
    {
        FB.login(self.facebookLoginCallback, { scope: 'email' });
    }

    self.facebookLoginCallback = function (response)
    {
        return accountHelper.facebookLogin
        (
            response,
            self.loginCallback,
            self.isCheckoutLogin
        );
    }

    self.login = function ()
    {
        return accountHelper.login
        (
            accountHelper.loginDetails.emailAddress(),
            accountHelper.loginDetails.password(),
            self.loginCallback
        );
    }

    self.loginCallback = function (success, pleaseWaitViewModel)
    {
        if (success)
        {
            // Are we logging in to checkout?
            if (self.isCheckoutLogin)
            {
                // Proceed to the checkout view
                cartHelper.checkout();

                // Switch the cart to checkout mode
                guiHelper.isMobileMenuVisible(false);
                guiHelper.canChangeOrderType(false);
                guiHelper.cartActions(guiHelper.cartActionsCheckout);
            }
            else
            {
                // Just a normal login.  Return to the previous view
                viewModel.pageManager.showPreviousPage(true, undefined, true);
            }
        }
        else
        {
            viewModel.telemetry.sendTelemetryData('Account', 'Login', 'Failed');

            viewModel.pageManager.showPage('Login', true, self, false);
        }
    }
};








































