/// <reference path="../Scripts/knockout-3.0.0.js" />
/// <reference path="../Scripts/knockout.validation.min.js" />

var customerHelper;
(
    function (helper)
    {
        helper =
        {
            bindableCustomer: ko.observable(undefined),

            validateCustomer: function (customer) {

                var success = true;

                // No error yet
                customer.errorMessage('');
                customer.firstNameHasError(false);
                customer.surnameHasError(false);
                customer.emailAddressHasError(false);
                customer.phoneNumberHasError(false);

                success = customer.validate();

                // All good!
                return success;
            },

            setError: function (customer, errorMessage) {
                // Only set the first error
                if (customer.errorMessage().length == 0) {
                    customer.errorMessage(errorMessage);
                }
            },

            validatePhoneNumber: function (phoneNumber) {
                var regex = /^\d{1,15}$/i; // numbers only upto 15.
                return regex.test(phoneNumber);
            }
        };

        helper.bindableCustomer.subscribe(function (customer) {
            customer || (customer = new Customer(true));
            if (!customer) { return; }

            //initial validation cue.
            helper.validateCustomer(customer);
        });

        customerHelper = helper;
    }
    (customerHelper)
);

