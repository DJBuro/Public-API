/// <reference path="../Scripts/knockout-3.0.0.js" />

function Address(isPostcodeLocked)
{
    "use strict";

    var self = this;

    var expectsValidation = function ()
    {
        if (self.requiresValidation())
        {
            return true;
        }

        var addressLine1 = self.roadName(),
            addressLine2 = self.prem1,
            town = self.town(),
            postcode = self.postcode();

        return (addressLine1 && addressLine1.length > 0)
            || (addressLine2 && addressLine2.length > 0)
            || (town && town.length > 0)
            || (postcode && postcode.length > 0);
    };

    self.errorMessage = ko.observable(''),
    self.addressMissingDetails = ko.observable(false); // True when any of the required address elements are missing
    self.addressHasError = ko.observable(false); // True when there are any problems with the address
    self.outOfDeliveryArea = ko.observable(false); // True when the address is not in the delivery area of the currently selected store

    self.prem1 = ko.observable('');
    self.prem2 = ko.observable('');
    self.prem3 = ko.observable('');
    self.prem4 = ko.observable('');
    self.prem5 = ko.observable('');
    self.prem6 = ko.observable('');
    self.org1 = ko.observable('');
    self.org2 = ko.observable('');
    self.org3 = ko.observable('');
    self.roadNum = ko.observable();
    self.roadName = ko.observable('');
    self.roadNameHasError = ko.observable(false);
    self.city = ko.observable('');
    self.town = ko.observable();

    self.townHasError = ko.observable(false);
    self.postcode = ko.observable();

    self.postcodeHasError = ko.observable(false);
    self.county = ko.observable('');
    self.state = ko.observable('');
    self.locality = ko.observable('');
    self.country = ko.observable(settings.customerAddressCountry);
    self.directions = ko.observable('');
    self.userLocality1 = ko.observable('');
    self.userLocality2 = ko.observable('');
    self.userLocality3 = ko.observable('');
    self.isPostcodeLocked = ko.observable(isPostcodeLocked === undefined ? false : isPostcodeLocked);

    self.requiresValidation = ko.observable(false);
    self.roadName.extend({
        required: {
            //todo: move to engb list
            message: "Address line 1 is required",
            onlyIf: expectsValidation
        }
    });

    self.town.extend({
        required: {
            //todo: move to engb list
            message: "A town name is required",
            onlyIf: expectsValidation
        }
    });

    self.postcode.extend({
        required: {
            //todo: move to engb list
            message: "The post code is required",
            onlyIf: expectsValidation
        },
        pattern: {
            //todo: move to engb list 
            message: 'Must be a valid UK postcode',
            params: /^([A-PR-UWYZ0-9][A-HK-Y0-9][AEHMNPRTVXY0-9]?[ABEHMNPRVWXY0-9]?\s?){1,2}([0-9][ABD-HJLN-UW-Z]{2}|GIR 0AA)$/i
        }
    });

    self.errors = ko.validation.group(self, { deep: false, observable: false });
    self.validate = function ()
    {
        var errors = self.errors();
        var valid = errors.length === 0;

        return valid;
    };

    self.toPlainObject = function ()
    {
        var address =
            {
                prem1: self.prem1(),
                prem2: self.prem2(),
                prem3: self.prem3(),
                prem4: self.prem4(),
                prem5: self.prem5(),
                prem6: self.prem6(),
                org1: self.org1(),
                org2: self.org2(),
                org3: self.org3(),
                roadNum: self.roadNum(),
                roadName: self.roadName(),
                city: self.city(),
                town: self.town(),
                postcode: self.postcode(),
                county: self.county(),
                state: self.state(),
                locality: self.locality(),
                country: self.country(),
                directions: self.directions(),
                userLocality1: self.userLocality1(),
                userLocality2: self.userLocality2(),
                userLocality3: self.userLocality3()
            };

        return address;
    }

    self.fromPlainObject = function (address)
    {
        if (address !== undefined && address !== null)
        {
            self.prem1(self.cleanupAddressField(address.prem1));
            self.prem2(self.cleanupAddressField(address.prem2));
            self.prem3(self.cleanupAddressField(address.prem3));
            self.prem4(self.cleanupAddressField(address.prem4));
            self.prem5(self.cleanupAddressField(address.prem5));
            self.prem6(self.cleanupAddressField(address.prem6));
            self.org1(self.cleanupAddressField(address.org1));
            self.org2(self.cleanupAddressField(address.org2));
            self.org3(self.cleanupAddressField(address.org3));
            self.roadNum(self.cleanupAddressField(address.roadNum));
            self.roadName(self.cleanupAddressField(address.roadName));
            self.city(self.cleanupAddressField(address.city));
            self.town(self.cleanupAddressField(address.town));
            self.postcode(self.cleanupAddressField(address.postcode));
            self.county(self.cleanupAddressField(address.county));
            self.state(self.cleanupAddressField(address.state));
            self.locality(self.cleanupAddressField(address.locality));
            self.country(self.cleanupAddressField(address.country));
            self.directions(self.cleanupAddressField(address.directions));
            self.userLocality1(self.cleanupAddressField(address.userLocality1));
            self.userLocality2(self.cleanupAddressField(address.userLocality2));
            self.userLocality3(self.cleanupAddressField(address.userLocality3));
        }

        if (self.country.length == 0)
        {
            self.country(settings.customerAddressCountry);
        }
    }

    self.fromAddressObject = function (address)
    {
        self.prem1(self.cleanupAddressField(address.prem1()));
        self.prem2(self.cleanupAddressField(address.prem2()));
        self.prem3(self.cleanupAddressField(address.prem3()));
        self.prem4(self.cleanupAddressField(address.prem4()));
        self.prem5(self.cleanupAddressField(address.prem5()));
        self.prem6(self.cleanupAddressField(address.prem6()));
        self.org1(self.cleanupAddressField(address.org1()));
        self.org2(self.cleanupAddressField(address.org2()));
        self.org3(self.cleanupAddressField(address.org3()));
        self.roadNum(self.cleanupAddressField(address.roadNum()));
        self.roadName(self.cleanupAddressField(address.roadName()));
        self.city(self.cleanupAddressField(address.city()));
        self.town(self.cleanupAddressField(address.town()));
        self.postcode(self.cleanupAddressField(address.postcode()));
        self.county(self.cleanupAddressField(address.county()));
        self.state(self.cleanupAddressField(address.state()));
        self.locality(self.cleanupAddressField(address.locality()));
        self.country(self.cleanupAddressField(address.country()));
        self.directions(self.cleanupAddressField(address.directions()));
        self.userLocality1(self.cleanupAddressField(address.userLocality1()));
        self.userLocality2(self.cleanupAddressField(address.userLocality2()));
        self.userLocality3(self.cleanupAddressField(address.userLocality3()));
    }

    self.cleanupAddressField = function (addressField)
    {
        var cleanedUp = addressField === undefined ? '' : (addressField === null ? '' : addressField);
        return $.isFunction(cleanedUp) ? cleanedUp() : cleanedUp;
    }
};