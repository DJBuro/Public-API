/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/facebook/fbsdk.d.ts" />
/// <reference path="../models/customer.ts" />
/// <reference path="../ViewModels/PleaseWaitViewModel.ts" />

module AndroWeb.ViewModels
{
    export class LoginViewModel extends BaseViewModel
    {        
        // Mobile mode
        public title: KnockoutObservable<string> = ko.observable(textStrings.mmChangeStoreButton); // Current section name - shown in the header
        public titleClass: KnockoutObservable<string> = ko.observable('mobileSectionSelectStore'); // Class to use to style the section name - used for showing an icon for the section

        public isLoginMode: KnockoutObservable<boolean> = ko.observable(true);
        public static isCheckoutLogin: boolean = false; 
        public canModifyEmailAddress: boolean = true;
        public isEmailAddressVisible: boolean = true;
        public showMarketingOptions: boolean = false;
        public isCurrentPasswordVisible: boolean = false;

        public previousViewName: string;
        public previousContentViewModel: any;

        constructor(isCheckoutLogin: boolean)
        {
            super();

            this.isShowStoreDetailsButtonVisible = ko.observable(true);
            this.isShowHomeButtonVisible = ko.observable(true);
            this.isShowMenuButtonVisible = ko.observable(true);
            this.isShowCartButtonVisible = ko.observable(true);

            this.isHeaderVisible = ko.observable(true);
            this.isPostcodeSelectorVisible = ko.observable(settings.storeSelectorMode && settings.storeSelectorInHeader);
            this.areHeaderOptionsVisible = ko.observable(true);
            this.isHeaderLoginVisible = ko.observable(true);

            customerHelper.bindableCustomer(new AndroWeb.Models.Customer());
            accountHelper.registerError(undefined);

            LoginViewModel.isCheckoutLogin = isCheckoutLogin;

            if (viewModel.contentViewModel() != undefined && viewModel.contentViewModel().previousViewName != undefined && viewModel.contentViewModel().previousViewName.length > 0)
            {
                this.previousViewName = viewModel.contentViewModel().previousViewName;
                this.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
            }
            else
            {
                this.previousViewName = guiHelper.getCurrentViewName();
                this.previousContentViewModel = viewModel.contentViewModel();
            }
        }

        public onLogout() : void
        {
        }
        
        public onShown(): void
        {
            $('#loginFormInput').focus();

            accountHelper.loginDetails.password("");
        }

        public toggleToLogin(): void
        {
            this.isLoginMode(true);
        }

        public toggleToRegister(): void
        {
            this.isLoginMode(false);
        }

        public register(): void
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
                facebookUsername: customer.facebookUsername(),
                accountNumber: undefined,
                loyalties: undefined
            };

            var p =
            {
                userName: customer.emailAddress(),
                password: customer.password()
            };

            var pleaseWaitViewModel = new AndroWeb.ViewModels.PleaseWaitViewModel();

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

                                viewModel.pageManager.showPage('Login', true, this, false);
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
                                        this.isFacebookRegister
                                    );

                                    // Return to the previous view or go to the checkout page?
                                    // Are we logging in to checkout?
                                    if (this.isCheckoutLogin)
                                    {
                                        AndroWeb.Helpers.CartHelper.checkout();

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

                                    viewModel.pageManager.showPage('Login', true, this, false);
                                }
                            }
                        },
                        function ()
                        {
                            viewModel.pageManager.showPage('Login', true, this, false);
                        }
                    );
                }
            );
        }

        public putCustomerCallback(): void
        {
        }

        public cancel(): void
        {
            viewModel.pageManager.showPreviousPage(true, undefined, true);
        }

        public loginRegisterKeyPress = (data, event): boolean =>
        {
            // Did the user press enter?
            if (event.which == 13 || event.keyCode == 13)
            {
                this.login();
                return false;
            }

            return true;
        }

        public facebookLogin(response): void
        {
            FB.login(LoginViewModel.facebookLoginCallback, { scope: 'email' });
        }

        public static facebookLoginCallback(response): void
        {
            return accountHelper.facebookLogin
            (
                response,
                LoginViewModel.loginCallback,
                LoginViewModel.isCheckoutLogin
            );
        }

        public login(): void
        {
            return accountHelper.login
            (
                accountHelper.loginDetails.emailAddress(),
                accountHelper.loginDetails.password(),
                LoginViewModel.loginCallback
            );
        }

        public static loginCallback(success, pleaseWaitViewModel): void
        {
            if (success)
            {
                // Are we logging in to checkout?
                if (LoginViewModel.isCheckoutLogin)
                {
                    // Proceed to the checkout view
                    AndroWeb.Helpers.CartHelper.checkout();

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

                viewModel.pageManager.showPage('Login', true, undefined);
            }
        }
    }
}







































