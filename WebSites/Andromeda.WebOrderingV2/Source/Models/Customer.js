/// <reference path="../Scripts/knockout-3.0.0.js" />
/// <reference path="../Scripts/knockout.validation.min.js" />

function Customer()
{
    "use strict";

    var self = this;
    var hide =
    {
        requiresPassword: true,
        enteringPassword: false
    };

    self.errorMessage = ko.observable(''),

    self.firstName = ko.observable().extend
    (
        {
            required:
            {
                message: "Name is required"
            }
        }
    );
    self.surname = ko.observable();
    
    self.facebookUsername = ko.observable(); 
    self.facebookId = ko.observable();

    self.emailAddress = ko.observable().extend
    (
        {
            required: { message: "Email address is required." },
            pattern:
            {
                message: 'A valid email is required',
                params: /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/
            }
        }
    );
    self.phoneNumber = ko.observable().extend
    (
        {
            required: { message: "Telephone number is required." },
            digit: true,
            maxLength: 15,
            minLength: 1
        }
    );

    self.marketing = ko.observable(true);
    self.smsMarketing = ko.observable(true);
    self.firstNameHasError = ko.observable(false);
    self.surnameHasError = ko.observable(false);
    self.emailAddressHasError = ko.observable(false);
    self.phoneNumberHasError = ko.observable(false);
    self.requiresPassword = ko.observable(true);

    var requirdPassword = function () { return self.requiresPassword(); };

    self.password = ko.observable().extend
    (
        {
            required:
            {
                message: "Password is required",
                onlyIf: requirdPassword
            },
            minLength:
            {
                params: 4,
                onlyIf: requirdPassword,
                message: "At least 4 characters"
            }
        }
    ),
    self.reenterPassword = ko.observable().extend
    (
        {
            required: { 
                message: "Password is required",
                onlyIf: requirdPassword
            },
            minLength: {
                params: 4,
                onlyIf: requirdPassword,
                message: "At least 4 characters"
            },
            areSame: {
                params: self.password,
                message: "Double check the passwords match.",
                onlyIf: requirdPassword
            }
        }
    ),
    self.errorMessage = ko.observable(''),
    self.currentPassword = ko.observable(''),
    self.currentPasswordHasError = ko.observable(false),
    self.passwordHasError = ko.observable(false),

    //validation things. 
    self.errors = ko.validation.group(self, { deep: false, observable: false });
    
    self.validate = function (setErrorMessage)
    {
        var errors = self.errors();
        var valid = errors.length === 0;

        return valid;
    };
    self.enablePassowrdValidation = function ()
    {
    };
    self.removePasswordValidation = function ()
    {
        self.requiresPassword(false);
    };

    self.password.subscribe
    (
        function (v)
        {
            v || (v = "");
      
            if (v && v, length > 0) {
                hide.enteringPassword = true;
            } else {
                hide.enteringPassword = false;
            }

            var required = hide.enteringPassword || hide.requiresPassword;
            self.requiresPassword(required);
        }
    );
};