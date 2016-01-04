/// <reference path="../Scripts/knockout-3.0.0.js" />

var addressHelper =
{
    bindableAddress: ko.observable(undefined),
    validateAddress: function (address, checkInDeliveryArea)
    {
        address.errorMessage('');

        if (settings.culture === "en-GB") {
            return addressHelper.validateUKAddress(address, checkInDeliveryArea);
        }
        else if (settings.culture === "en-US") {
            return addressHelper.validateUSAddress(address, checkInDeliveryArea);
        }
        else if (settings.culture === "fr-FR") {
            return addressHelper.validateFrenchAddress(address, checkInDeliveryArea);
        }
        else
        {
            return false;
        }
    },
    validateUKAddress: function (address, checkInDeliveryArea)
    {
        var success = true;

        // No errors yet
        address.roadNameHasError(false);
        address.townHasError(false);
        address.postcodeHasError(false);

        success = address.validate();

        if (success && settings.postcodeRequired)
        {
            if (address.postcode() === undefined)
            {
                address.errorMessage(textStrings.lrInvalidPostcode);
                address.postcodeHasError(true);
                success = false;
            }
            else
            {
                var cleanedUpPostcode = address.postcode().replace(/\s+/g, '').toUpperCase();
                var correctedPostCode = cleanedUpPostcode.substring(0, cleanedUpPostcode.length - 3) + " " + cleanedUpPostcode.slice(-3);
                if (!addressHelper.validatePostcode(correctedPostCode))
                {
                    address.errorMessage(textStrings.lrInvalidPostcode);
                    address.postcodeHasError(true);
                    success = false;
                }
                else
                {
                    address.postcode(correctedPostCode);
                }
            }
        }

        if (success && checkInDeliveryArea && !address.postcodeHasError())
        {
            var isInDeliveryZone = addressHelper.validateIsPostcodeInDeliveryZone(address);

            address.outOfDeliveryArea(!isInDeliveryZone);
            address.postcodeHasError(!isInDeliveryZone);
            if (!isInDeliveryZone)
            {
                address.errorMessage(textStrings.checkBadPostcode);

                ga
                (
                    "send",
                    "event",
                    {
                        eventCategory: "Sales",
                        eventAction: "Checkout-PostcodeUnknown",
                        eventLabel: address.postcode(),
                        eventValue: 1,
                        metric1: 1
                    }
                );
            }

            address.addressHasError
            (
                address.addressMissingDetails() ||
                address.outOfDeliveryArea()
            );

            success = isInDeliveryZone ? success : false;
        }

        // Was there an error?
        return success;
    },
    validatePostcode: function (postcode) 
    {
        var regex = /^([A-PR-UWYZ0-9][A-HK-Y0-9][AEHMNPRTVXY0-9]?[ABEHMNPRVWXY0-9]? {1,2}[0-9][ABD-HJLN-UW-Z]{2}|GIR 0AA)$/i;
        return regex.test(postcode.toUpperCase());
    },
    validatePostcodeWithOptionalSpace: function (postcode) 
    {
        var regex = /^([A-PR-UWYZ0-9][A-HK-Y0-9][AEHMNPRTVXY0-9]?[ABEHMNPRVWXY0-9]?\s?){1,2}([0-9][ABD-HJLN-UW-Z]{2}|GIR 0AA)$/i;
        return regex.test(postcode.toUpperCase());
    },
    validateIsPostcodeInDeliveryZone: function (address)
    {
        var isInDeliveryZone = false;

        if (deliveryZoneHelper.deliveryZones() == undefined || deliveryZoneHelper.deliveryZones().length == 0)
        {
            isInDeliveryZone = true; // No postcodes so we can't validate it
        }
        else
        {
            isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(address.postcode().toUpperCase());
        }

        return isInDeliveryZone;
    },
    generateMultiLineAddress: function (address)
    {
        var displayAddressMultiLine = "";
   //     displayAddressMultiLine += address.roadNum() != null && address.roadNum().length > 0 ? address.roadNum() + '<br />' : '';
        displayAddressMultiLine += address.roadName() != null && address.roadName().length > 0 ? address.roadName() + '<br />' : '';
    //    displayAddressMultiLine += address.org1() != null && address.org1().length > 0 ? address.org1() + '<br />' : '';
    //    displayAddressMultiLine += address.org2() != null && address.org2().length > 0 ? address.org2() + '<br />' : '';
    //    displayAddressMultiLine += address.org3() != null && address.org3().length > 0 ? address.org3() + '<br />' : '';
        displayAddressMultiLine += address.prem1() != null && address.prem1().length > 0 ? address.prem1() + '<br />' : '';
    //    displayAddressMultiLine += address.prem2() != null && address.prem2().length > 0 ? address.prem2() + '<br />' : '';
    //    displayAddressMultiLine += address.prem3() != null && address.prem3().length > 0 ? address.prem3() + '<br />' : '';
    //    displayAddressMultiLine += address.prem4() != null && address.prem4().length > 0 ? address.prem4() + '<br />' : '';
    //    displayAddressMultiLine += address.prem5() != null && address.prem5().length > 0 ? address.prem5() + '<br />' : '';
    //    displayAddressMultiLine += address.prem6() != null && address.prem6().length > 0 ? address.prem6() + '<br />' : '';
    //    displayAddressMultiLine += address.locality() != null && address.locality().length > 0 ? address.locality() + '<br />' : '';
        displayAddressMultiLine += address.town() != null && address.town().length > 0 ? address.town() + '<br />' : '';
     //   displayAddressMultiLine += address.county() != null && address.county().length > 0 ? address.county() + '<br />' : '';
     //   displayAddressMultiLine += address.state() != null && address.state().length > 0 ? address.state() + '<br />' : '';
        displayAddressMultiLine += address.postcode() != null && address.postcode().length > 0 ? address.postcode() + '<br />' : '';

        return displayAddressMultiLine;
    },
    addressType: function ()
    {
        // Figure out what address format to use
        if (settings.culture == "en-GB")
        {
            return 'ukAddress-min-template';
        }
        else if (settings.culture == "en-US")
        {
            return 'usAddress-template';
        }
        else if (settings.culture == "fr-FR")
        {
            return 'frenchAddress-template';
        }
        else
        {
            return 'genericAddress-template';
        }
    }
}