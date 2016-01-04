/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
module AndroWeb.Models
{
    export class Customer
    {
        public enteringPassword: boolean = false;

        public firstName: KnockoutObservable<string> = ko.observable(undefined);
        public surname: KnockoutObservable<string> = ko.observable(undefined);
        public errorMessage: KnockoutObservable<string> = ko.observable('');
        public currentPassword: KnockoutObservable<string> = ko.observable('');
        public currentPasswordHasError: KnockoutObservable<boolean> = ko.observable(false);
        public passwordHasError: KnockoutObservable<boolean> = ko.observable(false);

        public errors: KnockoutValidationGroup;

        public facebookUsername: KnockoutObservable<string> = ko.observable(undefined);
        public facebookId: KnockoutObservable<string> = ko.observable(undefined);

        public emailAddress: KnockoutObservable<string> = ko.observable(undefined);
        public phoneNumber: KnockoutObservable<number> = ko.observable(undefined);

        public marketing: KnockoutObservable<boolean> = ko.observable(true);
        public smsMarketing: KnockoutObservable<boolean> = ko.observable(true);
        public firstNameHasError: KnockoutObservable<boolean> = ko.observable(false);
        public surnameHasError: KnockoutObservable<boolean> = ko.observable(false);
        public emailAddressHasError: KnockoutObservable<boolean> = ko.observable(false);
        public phoneNumberHasError: KnockoutObservable<boolean> = ko.observable(false);
        public requiresPassword: KnockoutObservable<boolean> = ko.observable(true);

        public password: KnockoutObservable<string> = ko.observable(undefined);
        public reenterPassword: KnockoutObservable<string> = ko.observable(undefined);

        constructor()
        {
            var self = this;

            this.errors = ko.validation.group(self, { deep: false, observable: false });

            this.password.subscribe
            (
                function (v)
                {
                    v || (v = "");

                    if (v && v, length > 0)
                    {
                        self.enteringPassword = true;
                    } else
                    {
                        self.enteringPassword = false;
                    }

                    var required = self.enteringPassword || self.requiresPassword();
                    self.requiresPassword(required);
                }
                );

            this.firstName.extend
            (
                {
                    required:
                    {
                        message: "Name is required"
                    }
                }
            );

            this.emailAddress.extend
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

            this.phoneNumber.extend
            (
                {
                    required: { message: "Telephone number is required." },
                    digit: true,
                    maxLength: 15,
                    minLength: 1
                }
            );

            this.password.extend
            (
                {
                    required:
                    {
                        message: "Password is required",
                        onlyIf: self.requiresPassword
                    },
                    minLength:
                    {
                        params: 4,
                        onlyIf: self.requiresPassword,
                        message: "At least 4 characters"
                    }
                }
            );

            this.reenterPassword.extend
            (
                {
                    required:
                    {
                        message: "Password is required",
                        onlyIf: self.requiresPassword
                    },
                    minLength:
                    {
                        params: 4,
                        onlyIf: self.requiresPassword,
                        message: "At least 4 characters"
                    },
                    areSame:
                    {
                        params: self.password,
                        message: "Double check the passwords match.",
                        onlyIf: self.requiresPassword
                    }
                }
            );
        }

        public static requirdPassword = function ()
        {
            return this.requiresPassword();
        };

        public validate = function (setErrorMessage)
        {
            var errors = this.errors();
            var valid = errors.length === 0;

            return valid;
        };
        public enablePassowrdValidation = function ()
        {
        };
        public removePasswordValidation = function ()
        {
            this.requiresPassword(false);
        };
    }
}