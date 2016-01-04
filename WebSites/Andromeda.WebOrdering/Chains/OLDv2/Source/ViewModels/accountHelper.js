var accountHelper =
{
    customerDetails: undefined,
    emailAddress: undefined,
    password: undefined,
    loggedInCallback: undefined,
    loginDetails:
    {
        errorMessage: ko.observable(''),
        emailAddress: ko.observable(''),
        password: ko.observable('')
    },
    newAccount:
    {
        errorMessage: ko.observable(''),
        firstName: ko.observable(''),
        surname: ko.observable(''),
        emailAddress: ko.observable(''),
        phoneNumber: ko.observable(''),
        password: ko.observable(''),
        reenterPassword: ko.observable(''),
        marketing: ko.observable(true),
        firstNameHasError: ko.observable(false),
        surnameHasError: ko.observable(false),
        emailAddressHasError: ko.observable(false),
        phoneNumberHasError: ko.observable(false),
        passwordHasError: ko.observable(false)
    },
    isRegisterOpen: false,
    isLoggedIn: ko.observable(false),
    loginRegister: function ()
    {
        accountHelper.showPopup();
    },
    isPopupVisible: ko.observable(false),
    showPopup: function (loggedInCallback)
    {
        if (guiHelper.isMobileMode())
        {
            accountHelper.loggedInCallback = loggedInCallback;

            guiHelper.showView('loginRegisterView');

            guiHelper.isMobileMenuVisible(false);
        }
        else
        {
            $(window).scrollTop(0);

            accountHelper.isRegisterOpen = false;

            $('#loginRegister').css('height', '260px');
            $('#registerContainer').css('paddingTop', '45px');

            accountHelper.loggedInCallback = loggedInCallback;

            // Make the popup visible
            popupHelper.isBackgroundVisible(true);
            accountHelper.isPopupVisible(true);

            if (guiHelper.isMobileMode())
            {
                // In mobile mode the cart is a view and not a popup
                guiHelper.isViewVisible(false);
                guiHelper.isInnerMenuVisible(false);
            }
        }
    },
    hidePopup: function (callback)
    {
        if (!guiHelper.isMobileMode())
        {
            popupHelper.isBackgroundVisible(false);
            accountHelper.isPopupVisible(false);
        }

        if (typeof (callback) == 'function')
        {
            callback();
        }

        accountHelper.loggedInCallback = undefined;
    },
    normalLogin: function ()
    {
        accountHelper.login(accountHelper.loginDetails.emailAddress(), accountHelper.loginDetails.password());
    },
    login: function (email, password)
    {
        accountHelper.loginDetails.errorMessage('');

        if (email == undefined || email.length == 0)
        {
            accountHelper.loginDetails.errorMessage('Please enter an email address');
            return;
        }

        if (password == undefined || password.length == 0)
        {
            accountHelper.loginDetails.errorMessage('Please enter a password');
            return;
        }

        acsapi.getCustomer
        (
            email,
            password,
            function (getCustomerResponse)
            {
                if (getCustomerResponse == undefined)
                {
                    // Failed!
                    accountHelper.loginDetails.errorMessage('Unable to login');
                    return;
                }
                else
                {
                    // Was an error returned
                    if (getCustomerResponse.errorCode == undefined)
                    {
                        // Success
                        accountHelper.isLoggedIn(true);
                        accountHelper.customerDetails = getCustomerResponse;
                        accountHelper.emailAddress = email;
                        accountHelper.password = password;
                        accountHelper.hidePopup(accountHelper.loggedInCallback);
                    }
                    else
                    {
                        // Error returned by server
                        if (getCustomerResponse.errorCode == 1036)
                        {
                            // Invalid username
                            accountHelper.loginDetails.errorMessage('Invalid username or password');
                            return;
                        }
                        else if (getCustomerResponse.errorCode == 1042)
                        {
                            // Invalid password
                            accountHelper.loginDetails.errorMessage('Invalid username or password');
                            return;
                        }
                        else
                        {
                            // Other error
                            accountHelper.loginDetails.errorMessage('Unable to login');
                            return;
                        }
                    }
                }
            }
        );
    },
    facebookLogin: function ()
    {
        accountHelper.loginDetails.errorMessage('');

        FB.login
        (
            function (response)
            {
                if (response.authResponse)
                {
                    FB.api('/me', function (response)
                    {
                        //   accountHelper.login(true, response.email, response.id);

                        acsapi.getCustomer
                        (
                            response.email,
                            response.id,
                            function (getCustomerResponse)
                            {
                                if (getCustomerResponse == undefined)
                                {
                                    // Failed!
                                    accountHelper.loginDetails.errorMessage('Unable to login');
                                    return;
                                }
                                else
                                {
                                    // Was an error returned
                                    if (getCustomerResponse.errorCode == undefined)
                                    {
                                        // Success
                                        accountHelper.isLoggedIn(true);
                                        accountHelper.customerDetails = getCustomerResponse;
                                        accountHelper.emailAddress = response.email;
                                        accountHelper.password = response.id;
                                        accountHelper.hidePopup(accountHelper.loggedInCallback);
                                    }
                                    else
                                    {
                                        // Error returned by server
                                        if (getCustomerResponse.errorCode == 1036 || getCustomerResponse.errorCode == 1042)
                                        {
                                            acsapi.putCustomer
                                            (
                                                response.email,
                                                response.id,
                                                {
                                                    title: '',
                                                    firstname: response.first_name,
                                                    surname: response.last_name,
                                                    contacts: undefined,
                                                    address: undefined
                                                },
                                                function (response)
                                                {
                                                    if (response == undefined)
                                                    {
                                                        accountHelper.isLoggedIn(true);
                                                        accountHelper.customerDetails = getCustomerResponse;
                                                        accountHelper.emailAddress = email;
                                                        accountHelper.password = password;
                                                        accountHelper.hidePopup(accountHelper.loggedInCallback);
                                                    }
                                                }
                                            );

                                            return;
                                        }
                                        else
                                        {
                                            // Other error
                                            accountHelper.loginDetails.errorMessage('Unable to login');
                                            return;
                                        }
                                    }
                                }
                            }
                        );
                    });
                }
            },
            {
                scope: 'email'
            }
        );
    },
    myAccount: function ()
    {
        alert("my account");
    },
    register: function ()
    {
        // No error yet
        accountHelper.newAccount.errorMessage('');
        accountHelper.newAccount.firstNameHasError(false);
        accountHelper.newAccount.surnameHasError(false);
        accountHelper.newAccount.emailAddressHasError(false);
        accountHelper.newAccount.phoneNumberHasError(false);
        accountHelper.newAccount.passwordHasError(false);

        // Check that the account details have been entered
        if (accountHelper.newAccount.firstName().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter your first name");
            accountHelper.newAccount.firstNameHasError(true);
            return;
        }

        if (accountHelper.newAccount.surname().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter your surname");
            accountHelper.newAccount.surnameHasError(true);
            return;
        }

        if (accountHelper.newAccount.emailAddress().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter your email address");
            accountHelper.newAccount.emailAddressHasError(true);
            return;
        }

        if (accountHelper.newAccount.phoneNumber().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter your phone number");
            accountHelper.newAccount.phoneNumberHasError(true);
            return;
        }

        if (accountHelper.newAccount.password().length == 0)
        {
            accountHelper.newAccount.errorMessage("Please enter a password");
            accountHelper.newAccount.passwordHasError(true);
            return;
        }

        // Valid email address?
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        accountHelper.newAccount.emailAddressHasError(!regex.test(accountHelper.newAccount.emailAddress()));
        if (accountHelper.newAccount.emailAddressHasError())
        {
            accountHelper.newAccount.errorMessage("Please enter a valid email address");
            return;
        }

        // Check passwords match
        if (accountHelper.newAccount.password() != accountHelper.newAccount.reenterPassword())
        {
            accountHelper.newAccount.errorMessage("Passwords do not match");
            accountHelper.newAccount.passwordHasError(true);
            return;
        }

        // Minimum password length
        if (accountHelper.newAccount.password().length < 8)
        {
            accountHelper.newAccount.errorMessage("Password must be at least 8 characters");
            accountHelper.newAccount.passwordHasError(true);
            return;
        }

        var customerDetails =
        {
            title: '',
            firstname: accountHelper.newAccount.firstName(),
            surname: accountHelper.newAccount.surname(),
            contacts:
            [
                {
                    type: 'Email',
                    value: accountHelper.newAccount.emailAddress(),
                    marketingLevel: accountHelper.newAccount.marketing() ? '3rdParty' : 'OrderOnly'
                },
                {
                    type: 'Mobile',
                    value: accountHelper.newAccount.phoneNumber(),
                    marketingLevel: accountHelper.newAccount.marketing() ? '3rdParty' : 'OrderOnly'
                }
            ],
            address: undefined
        };

        // Validation passed - try and create a new customer account
        acsapi.putCustomer
        (
            accountHelper.newAccount.emailAddress(),
            accountHelper.newAccount.password(),
            customerDetails,
            function (putCustomerResponse)
            {
                if (putCustomerResponse != undefined && putCustomerResponse.errorCode != undefined)
                {
                    // Server returned an error
                    if (putCustomerResponse.errorCode == 1041)
                    {
                        accountHelper.newAccount.errorMessage("This email address has already been used");
                        accountHelper.newAccount.emailAddressHasError(true);
                        return;
                    }
                    else
                    {
                        accountHelper.newAccount.errorMessage("Unable to create account");
                    }

                    // TODO other errors
                }
                else
                {
                    // Success
                    accountHelper.customerDetails = customerDetails;
                    accountHelper.isLoggedIn(true);
                    accountHelper.emailAddress = accountHelper.newAccount.emailAddress();
                    accountHelper.password = accountHelper.newAccount.password();
                    accountHelper.hidePopup(accountHelper.loggedInCallback);
                }
            }
        );
    },
    showRegister: function ()
    {
        if (accountHelper.isRegisterOpen)
        {
            accountHelper.register();
        }
        else
        {
            $('#loginRegister').animate
            (
                {
                    height: 430
                },
                {
                    duration: 500,
                    queue: false
                }
            );

            $('#registerContainer').animate
            (
                {
                    paddingTop: 10
                },
                {
                    duration: 500,
                    queue: false
                }
            );

            accountHelper.isRegisterOpen = true;
        }
    },
    cancel: function ()
    {
        if (guiHelper.isMobileMode())
        {
            accountHelper.hidePopup(guiHelper.showCart);
        }
        else
        {
            accountHelper.hidePopup();
        }
    }
}