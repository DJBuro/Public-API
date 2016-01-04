/// <reference path="addressHelper.js" />
/// <reference path="../ViewModels/RegisterFullViewModel.js" />
/// <reference path="customerHelper.js" />

var accountHelper =
{
    loginEnabled: ko.observable(true),
    helloText: ko.observable(),
    loyaltyHelloText: ko.observable(),
    customerDetails: undefined,
    emailAddress: undefined,
    username: undefined,
    password: undefined,
    isFacebookLogin: false,
    facebookId: undefined,
    loggedInCallback: undefined,
    loginDetails:
    {
        errorMessage: ko.observable(''),
        emailAddress: ko.observable(''),
        password: ko.observable('')
    },
    registerError: ko.observable(),
    isRegisterOpen: false,
    isLoggedIn: ko.observable(false),
    androLogin: function ()
    {
        return accountHelper.login
        (
            accountHelper.loginDetails.emailAddress(),
            accountHelper.loginDetails.password(),
            undefined
        );
    },
    normalLogin: function (callback)
    {
        return accountHelper.login
        (
            accountHelper.loginDetails.emailAddress(),
            accountHelper.loginDetails.password(),
            callback
        );
    },
    login: function (email, password, callback)
    {
        accountHelper.loginDetails.errorMessage('');

        if (email == undefined || email.length == 0)
        {
            accountHelper.loginDetails.errorMessage(textStrings.hMissingEmail);
            if (callback != undefined) callback(false, undefined);
            return;
        }

        if (password == undefined || password.length == 0)
        {
            accountHelper.loginDetails.errorMessage(textStrings.hMissingPassword);
            if (callback != undefined) callback(false, undefined);
            return;
        }

        // Make sure the login popup is hidden (even if it wasn't visible anyway)
        toppingsPopupHelper.isBackgroundVisible(false);

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(true);
            guiHelper.isInnerMenuVisible(true);
        }

        guiHelper.showPleaseWait
        (
            textStrings.hLoggingIn,
            '',
            function (pleaseWaitViewModel)
            {
                acsapi.getCustomer
                (
                    viewModel.selectedSite() === undefined ? undefined : viewModel.selectedSite().siteId,
                    email,
                    password,
                    function (getCustomerResponse)
                    {
                        if (getCustomerResponse === undefined)
                        {
                            // Failed!
                            accountHelper.loginDetails.errorMessage(textStrings.hUnableToLogin);

                            // Return to previous view
                            if (callback !== undefined)
                            {
                                callback(false, pleaseWaitViewModel);
                            }
                        }
                        else
                        {
                            // Was an error returned?
                            if (getCustomerResponse.errorCode === undefined)
                            {
                                // Success
                                accountHelper.loggedIn(getCustomerResponse, email, email, password, false, undefined);

                                // Return to previous view
                                if (callback !== undefined)
                                {
                                    callback(true, pleaseWaitViewModel);
                                }
                            }
                            else
                            {
                                // Error returned by server
                                if (getCustomerResponse.errorCode === 1036)
                                {
                                    // Invalid username
                                    accountHelper.loginDetails.errorMessage(textStrings.hInvalidUserPass);

                                    // Return to previous view
                                    if (callback !== undefined)
                                    {
                                        callback(false, pleaseWaitViewModel);
                                    }
                                }
                                else if (getCustomerResponse.errorCode === 1042)
                                {
                                    // Invalid password
                                    accountHelper.loginDetails.errorMessage(textStrings.hInvalidUserPass);

                                    // Return to previous view
                                    if (callback !== undefined)
                                    {
                                        callback(false, pleaseWaitViewModel);
                                    }
                                }
                                else
                                {
                                    // Other error
                                    accountHelper.loginDetails.errorMessage(textStrings.hUnableToLogin);

                                    // Return to previous view
                                    if (callback !== undefined)
                                    {
                                        callback(false, pleaseWaitViewModel);
                                    }
                                }
                            }
                        }
                    },
                    function (error)
                    {
                        callback(false, pleaseWaitViewModel);
                    }
                );
            }
        );
    },
    loggedIn: function (customerDetails, username, email, password, isFacebookLogin, facebookId)
    {
        accountHelper.customerDetails = customerDetails;
        accountHelper.username = username;
        accountHelper.emailAddress = email;
        accountHelper.password = password;
        accountHelper.isFacebookLogin = (isFacebookLogin === undefined ? false : isFacebookLogin);
        accountHelper.facebookId = facebookId;

        // i need this at the end of the method as i subscribe to it and expect the above. 
        accountHelper.isLoggedIn(true);

        // Refresh the loyalty details
        loyaltyHelper.refreshCustomerSession();

        // Generate the welcome message. We need to know if the user is logged on
        accountHelper.generateWelcomeMessage();
        

        if (accountHelper.isFacebookLogin)
        {
            viewModel.telemetry.sendTelemetryData('Account', 'Login', 'Facebook', accountHelper.username);
        }
        else
        {
            viewModel.telemetry.sendTelemetryData('Account', 'Login', 'Andro', accountHelper.username);
        }
    },
    generateWelcomeMessage: function ()
    {
        var helloText = '';
        var loyaltyHelloText = '';

        // Is the user logged in?
        if (accountHelper.isLoggedIn())
        {
            if (viewModel.siteDetails() === undefined)
            {
                helloText = textStrings.hHelloLoggedInNoStore;
            }
            else
            {
                helloText = textStrings.hHelloLoggedIn;

                // Customer has already selected a store so inject the store name
                helloText = helloText.replace('{storeName}', viewModel.siteDetails().name);
            }

            // Inject customers first name into the welcome message
            if (accountHelper.customerDetails != undefined)
            {
                helloText = helloText.replace('{firstName}', accountHelper.customerDetails.firstname);
            }

            // Loyalty welcome message
            if (accountHelper.customerDetails != undefined && loyaltyHelper.IsEnabled() && loyaltyHelper.loyaltySession != undefined)
            {
                loyaltyHelloText = textStrings.hLoyaltyWelcome.replace('{points}', loyaltyHelper.loyaltySession.customersAvailableLoyaltyPoints());
                loyaltyHelloText = loyaltyHelloText.replace('{value}', helper.formatPrice(loyaltyHelper.loyaltySession.customersAvailableLoyaltyPointsValue()));
            }
        }
        else
        {
            if (viewModel.siteDetails() !== undefined)
            {
                helloText = textStrings.hHelloNotLoggedIn.replace('{storeName}', viewModel.siteDetails().name);
            }

            loyaltyHelloText = '';
        }  

        accountHelper.helloText(helloText);
        accountHelper.loyaltyHelloText(loyaltyHelloText);
    },
    facebookLogin: function (response, callback, isCheckoutLogin)
    {
        accountHelper.loginDetails.errorMessage('');

        if (response == undefined || response.authResponse == undefined || response.authResponse === false)
        {
            accountHelper.loginDetails.errorMessage(textStrings.hUnableToLogin);
            if (callback != undefined) callback(false, undefined);
            return;
        }

        // Make sure the login popup is hidden (even if it wasn't visible anyway)
        toppingsPopupHelper.isBackgroundVisible(false);

        if (guiHelper.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(true);
            guiHelper.isInnerMenuVisible(true);
        }

        guiHelper.showPleaseWait
        (
            textStrings.hLoggingIn,
            '',
            function (pleaseWaitViewModel)
            {
                FB.api
                (
                    '/me', 
                    function (response)
                    {
                        var username = (response.email === undefined || response.email === null || response.email.length === 0 ? response.id : response.email);

                        acsapi.getCustomer
                        (
                            viewModel.selectedSite() === undefined ? undefined : viewModel.selectedSite().siteId,
                            username,
                            response.id,
                            function (getCustomerResponse)
                            {
                                if (getCustomerResponse == undefined)
                                {
                                    // Failed!
                                    accountHelper.loginDetails.errorMessage(textStrings.hUnableToLogin);
                                    
                                    // Return to previous view
                                    if (callback != undefined)
                                    {
                                        callback(false, pleaseWaitViewModel);
                                    }
                                }
                                else
                                {
                                    // Was an error returned
                                    if (getCustomerResponse.errorCode == undefined)
                                    {
                                        // Success
                                        accountHelper.loggedIn(getCustomerResponse, username, response.email, response.id, true, response.id);

                                        // Return to previous view
                                        if (callback != undefined)
                                        {
                                            callback(true, pleaseWaitViewModel);
                                        }
                                    }
                                    else
                                    {
                                        // Error returned by server
                                        if (getCustomerResponse.errorCode == 1036 || getCustomerResponse.errorCode == 1042)
                                        {
                                            // No account so we'll need to create one - ask the user to register
                                            var customerDetails =
                                            {
                                                title: '',
                                                firstname: 
                                                    (response.first_name === undefined ? "" : response.first_name) + ' ' +
                                                    (response.last_name === undefined ? "" : response.last_name),
                                                surname: '',
                                                contacts:
                                                [
                                                    {
                                                        type: 'Email',
                                                        value: response.email,
                                                        marketingLevel: 'OrderOnly'
                                                    }
                                                ],
                                                address: undefined,
                                                facebookId: response.id,
                                                facebookUsername: response.username
                                            };

                                            // Validation passed - try and create a new customer account
                                            acsapi.putCustomer
                                            (
                                                viewModel.selectedSite() === undefined ? undefined : viewModel.selectedSite().siteId,
                                                username,
                                                response.id,
                                                customerDetails,
                                                function (putCustomerResponse)
                                                {
                                                    if (putCustomerResponse === undefined || putCustomerResponse === null)
                                                    {
                                                        // No response from server - shouldn't be possible so assume error
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

                                                            // Success
                                                            accountHelper.loggedIn
                                                            (
                                                                customerDetails,
                                                                username,
                                                                response.email,
                                                                response.id,
                                                                true,
                                                                response.id
                                                            );

                                                            // Return to the previous view or go to the checkout page?
                                                            // Are we logging in to checkout?
                                                            if (isCheckoutLogin)
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
                                                                accountHelper.registerError(textStrings.lrFacebookAlreadyUsed);
                                                            }
                                                            else
                                                            {
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
                                           
                                            return;
                                        }
                                        else
                                        {
                                            // Other error
                                            viewModel.pageManager.showPage('Login', true, false);
                                            return;
                                        }
                                    }
                                }
                            },
                            function ()
                            {
                                viewModel.pageManager.showPage('Login', true, false);
                                return;
                            }
                        );
                    }
                );
            }
        );
    },
    register: function (checkInDeliveryArea, callback)
    {
        var customer = customerHelper.bindableCustomer();

        // Validate customer details
        if (!customerHelper.validateCustomer(customer))
        {
            //already a error message ... as per validateCustomer
            if (callback !== undefined)
                callback(false);
            return;
        }

        // Check passwords match -- should validate within knockout validation
        if (customer.password() !== customer.reenterPassword()) {
            customer.errorMessage(textStrings.lrPasswordMismatch);
            customer.passwordHasError(true);
            if (callback !== undefined)
                callback(false);
            return;
        }

        // Minimum password length
        if (!accountHelper.validatePassword(customer.password()))
        {
            customer.errorMessage(textStrings.lrPassToShort);
            customer.passwordHasError(true);
            if (callback !== undefined)
                callback(false);
            return;
        }
    },
    logout: function ()
    {
        accountHelper.isLoggedIn(false);
        accountHelper.customerDetails = undefined;
        accountHelper.emailAddress = undefined;
        accountHelper.username = undefined;
        accountHelper.password = undefined;
        accountHelper.isFacebookLogin = false;
        accountHelper.facebookId = undefined;
        accountHelper.loginDetails.errorMessage('');
        accountHelper.loginDetails.emailAddress('');
        accountHelper.loginDetails.password('');

        accountHelper.generateWelcomeMessage();

        if (viewModel.selectedSite() !== undefined)
        {
            // A store has been selected - show the menu
            viewModel.pageManager.showPage('Menu', true, undefined, true);
        }
        else
        {
            // No store selected - choose a store
            viewModel.pageManager.showPage('ChooseStore', true, undefined, false);
        }
    },
    validateEmail: function(email)
    {
        // Valid email address?
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    },
    validatePassword: function (password)
    {
        return password.length >= 4;
    },
    save: function (callback)
    {
        var customer = customerHelper.bindableCustomer();
        var address = addressHelper.bindableAddress();

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
            address: address.toPlainObject(),
            accountNumber: accountHelper.customerDetails.accountNumber
        };

        // Validation passed - try and create a new customer account
        var p =
        {
            userName: accountHelper.username,
            password: accountHelper.password,
            newPassword: customerHelper.bindableCustomer().password(),
            customerDetails: customerDetails
        };

        if (accountHelper.isFacebookLogin && customer.facebookId() !== undefined)
        {
            p.userName = customer.facebookId();
        }

        acsapi.postCustomer
        (
            p.userName,
            p.password,
            p.newPassword,
            p.customerDetails,
            function (putCustomerResponse)
            {
                if (putCustomerResponse != undefined && putCustomerResponse.errorCode != undefined)
                {
                    // Server returned an error
                    if (putCustomerResponse.errorCode == 1041)
                    {
                        customerHelper.bindableCustomer().errorMessage(textStrings.lrEmailAlreadyUsed);
                        customerHelper.bindableCustomer().emailAddressHasError(true);
                    }
                    else
                    {
                        customerHelper.bindableCustomer().errorMessage(textStrings.mpUnableToCreateAccount);
                    }
                }
                else
                {
                    // Has the password changed?
                    var password = accountHelper.password;
                    if (customerHelper.bindableCustomer().password !== undefined &&
                        customerHelper.bindableCustomer().password() !== undefined &&
                        customerHelper.bindableCustomer().password().length > 0)
                    {
                        password = customerHelper.bindableCustomer().password();
                    }

                    // Success
                    accountHelper.loggedIn
                    (
                        customerDetails,
                        accountHelper.username,
                        customerHelper.bindableCustomer().emailAddress(),
                        password,
                        accountHelper.isFacebookLogin,
                        accountHelper.facebookId
                    );

                    accountHelper.previousPostcode = accountHelper.customerDetails.address.postcode;

                    callback();
                }
            },
            function ()
            {
                customerHelper.bindableCustomer().errorMessage(textStrings.mpUnableToCreateAccount);
            }
        );
    },
    initialiseBindableCustomer: function ()
    {
        customerHelper.bindableCustomer().firstName(accountHelper.customerDetails.firstname);
        customerHelper.bindableCustomer().surname(accountHelper.customerDetails.surname);

        if (accountHelper.customerDetails.contacts != undefined)
        {
            for (var index = 0; index < accountHelper.customerDetails.contacts.length; index++)
            {
                var contact = accountHelper.customerDetails.contacts[index];
                if (contact.type === 'Mobile')
                {
                    customerHelper.bindableCustomer().phoneNumber(contact.value);
                    customerHelper.bindableCustomer().smsMarketing(contact.marketingLevel === '3rdParty');
                }
                if (contact.type === 'Email')
                {
                    customerHelper.bindableCustomer().emailAddress(contact.value);
                    customerHelper.bindableCustomer().marketing(contact.marketingLevel === '3rdParty');
                }
            }
        }
    }
}
