/// <reference path="../Helpers/accountHelper.js" />
/// <reference path="../Helpers/ACSAPI.js" />
/// <reference path="../Helpers/customerHelper.js" />
/// <reference path="../ResourceStrings/en-GB.js" />
/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function MyProfileViewModel(returnToCheckout)
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

    // Create a new address & customer that we can bind to
    addressHelper.bindableAddress(new Address());
    
    self.returnToCheckout = returnToCheckout === undefined ? false : returnToCheckout;

    self.onLogout = function ()
    {
        this.backToMenu();
        if (typeof (self.previousContentViewModel.onLogout) == 'function')
        {
            self.previousContentViewModel.onLogout();
        }
    }

    if (viewModel.contentViewModel() !== undefined
        && viewModel.contentViewModel().previousViewName !== undefined
        && viewModel.contentViewModel().previousViewName.length > 0)
    {
        self.previousViewName = viewModel.contentViewModel().previousViewName;
        self.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
    }
    else
    {
        self.previousViewName = guiHelper.getCurrentViewName();
        self.previousContentViewModel = viewModel.contentViewModel();
    }

    var customer = new AndroWeb.Models.Customer()
    customerHelper.bindableCustomer(customer);
    customer.errorMessage('');
    customer.currentPassword('');
    customer.currentPasswordHasError('');
    customer.password('');
    customer.reenterPassword('');
    customer.removePasswordValidation();
    var passwordEntered = function ()
    {
        var p1 = customer.password().length;
        var p2 = customer.reenterPassword().length;

        var enteringPassword = p1 > 0 || p2 > 0;
        if (enteringPassword)
        {
            customer.enablePassowrdValidation();
        }
        else
        {
            customer.removePasswordValidation();
        }
    };
    customer.password.subscribe(passwordEntered);
    customer.reenterPassword.subscribe(passwordEntered);

    // Copy account details over to the bindable customer
    accountHelper.initialiseBindableCustomer();

    customerHelper.bindableCustomer().passwordHasError('');
    customerHelper.bindableCustomer().errorMessage(''),
    customerHelper.bindableCustomer().firstNameHasError(false);
    customerHelper.bindableCustomer().surnameHasError(false);
    customerHelper.bindableCustomer().emailAddressHasError(false);
    customerHelper.bindableCustomer().phoneNumberHasError(false);

    if (accountHelper.customerDetails.address)
    {
        addressHelper.bindableAddress().fromPlainObject(accountHelper.customerDetails.address);
    }
    else
    {
        accountHelper.customerDetails.address = addressHelper.bindableAddress();
    }

    addressHelper.bindableAddress().errorMessage(''),
    addressHelper.bindableAddress().addressMissingDetails(false); // True when any of the required address elements are missing
    addressHelper.bindableAddress().addressHasError(false); // True when there are any problems with the address
    addressHelper.bindableAddress().outOfDeliveryArea(false); // True when the address is not in the delivery area of the currently selected store
    addressHelper.bindableAddress().roadNameHasError(false);
    addressHelper.bindableAddress().townHasError(false);
    addressHelper.bindableAddress().postcodeHasError(false);

    // For facebook if the facebook account already has an email address then use it but don't let the user change it
    self.canModifyEmailAddress = false;
    self.isEmailAddressVisible = true;
    self.isCurrentPasswordVisible = true;

    if (accountHelper.isFacebookLogin &&
        customerHelper.bindableCustomer().emailAddress() != undefined &&
        customerHelper.bindableCustomer().emailAddress().length > 0)
    {
        // ACS doesn't let you change your email address
        self.canModifyEmailAddress = false;
        self.isEmailAddressVisible = false;
    }

    self.addressType = function ()
    {
        // Figure out what address format to use
        if (settings.culture === "en-GB") {
            return 'ukAddress-min-template';
        }
        else if (settings.culture === "en-US") {
            return 'usAddress-template';
        }
        else if (settings.culture === "fr-FR") {
            return 'frenchAddress-template';
        }
        else
        {
            return 'genericAddress-template';
        }
    };

    self.saveChanges = function ()
    {
        if (!self.validate()) return;

        accountHelper.save
        (
            function()
            {
                if (self.returnToCheckout)
                {
                    AndroWeb.Helpers.CartHelper.checkout();
                }
                else
                {
                    viewModel.pageManager.showPreviousPage(true);
                }
            }
        );
    };

    self.myOrders = function ()
    {
        viewModel.pageManager.showPage('MyOrders', true, undefined, false);
    };

    self.logout = function ()
    {
        viewModel.headerViewModel().logout();
    }

    self.backToMenu = function ()
    {
        viewModel.pageManager.showPage('Menu', true, undefined, true);
    };

    self.validate = function ()
    {
        // Clear errors
        customerHelper.bindableCustomer().errorMessage('');
        customerHelper.bindableCustomer().passwordHasError(false);

        // validate the customer model
        var valid = customerHelper.bindableCustomer().validate();
        if (!valid) return valid;

        // Validate customer details
        if (!customerHelper.validateCustomer(customerHelper.bindableCustomer()))
        {
            self.setError(customerHelper.bindableCustomer().errorMessage());
        }

        if (customerHelper.bindableCustomer().currentPassword().length > 0 ||
            customerHelper.bindableCustomer().password().length > 0 ||
            customerHelper.bindableCustomer().reenterPassword().length > 0)
        {
            // Check existing password
            if (customerHelper.bindableCustomer().currentPassword().length == 0)
            {
                self.setError(textStrings.mpMissingPasswords);
                customerHelper.bindableCustomer().currentPasswordHasError(true);
            }

            // Check re-entered password
            if (customerHelper.bindableCustomer().reenterPassword().length == 0)
            {
                self.setError(textStrings.mpMissingPasswords);
                customerHelper.bindableCustomer().passwordHasError(true);
            }

            // Check new password
            if (customerHelper.bindableCustomer().password().length == 0)
            {
                self.setError(textStrings.mpMissingPasswords);
                customerHelper.bindableCustomer().passwordHasError(true);
            }

            // Check existing password is correct
            if (customerHelper.bindableCustomer().currentPassword() != accountHelper.password)
            {
                self.setError(textStrings.mpWrongCurrentPassword);
                customerHelper.bindableCustomer().currentPasswordHasError(true);
            }

            // Check passwords match
            if (customerHelper.bindableCustomer().password() != customerHelper.bindableCustomer().reenterPassword())
            {
                self.setError(textStrings.lrPasswordsDontMatch);
                customerHelper.bindableCustomer().passwordHasError(true);
            }

            // Minimum password length
            if (!accountHelper.validatePassword(customerHelper.bindableCustomer().password()))
            {
                self.setError(textStrings.lrPasswordTooShort);
                customerHelper.bindableCustomer().passwordHasError(true);
            }
        }

        // Validate address
        if (!addressHelper.validateAddress(addressHelper.bindableAddress()))
        {
            self.setError(addressHelper.bindableAddress().errorMessage());
        }

        // Were there any errors?
        return customerHelper.bindableCustomer().errors().length === 0 && addressHelper.bindableAddress().errors().length === 0;
    };
    self.setError = function (errorMessage)
    {
        // Only set the first error
        if (customerHelper.bindableCustomer().errorMessage().length == 0)
        {
            customerHelper.bindableCustomer().errorMessage(errorMessage);
        }
    };
};






































