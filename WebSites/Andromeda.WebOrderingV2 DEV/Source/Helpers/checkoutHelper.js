// Checkout

/// <reference path="../App/Services/loyaltyHelper.js" />
var checkoutHelper =
{
    checkoutDetails:
    {
        firstName: ko.observable(''),
        surname: ko.observable(''),
        telephoneNumber: ko.observable(''),
        emailAddress: ko.observable(''),
        deliveryTime: ko.observable(undefined),
        address: new Address(),
        payNow: ko.observable(undefined),
        marketing: ko.observable('true'),
        wantedTime: ko.observable(undefined),
        orderNotes: ko.observable(''),
        chefNotes: ko.observable(''),
        rememberAddress: true,
        rememberContactDetails: true,
        voucherCodes: ko.observable([]),
        isMissingAddress: ko.observable(false),
        slotsRemoved: ko.observable(false),
        tableNumber: ko.observable('')
    },
    showCongratsLoyaltyWarningMessage: ko.observable(false),
    showUpdateCustomerAccount: ko.observable(false),
    checkoutViewModel: undefined,
    canApplyVouchers: ko.observable(false),
    telephoneNumber: ko.observable(''),
    payNowText: ko.observable(''),
    clearCheckout: function ()
    {
        checkoutHelper.checkoutDetails.firstName('');
        checkoutHelper.checkoutDetails.surname('');
        checkoutHelper.checkoutDetails.telephoneNumber('');
        checkoutHelper.checkoutDetails.emailAddress('');
        checkoutHelper.checkoutDetails.deliveryTime('');
        checkoutHelper.checkoutDetails.payNow(undefined);
        checkoutHelper.checkoutDetails.address = new Address();
        checkoutHelper.checkoutDetails.orderNotes('');
        checkoutHelper.checkoutDetails.chefNotes('');
        checkoutHelper.checkoutDetails.voucherCodes([]);
    },
    initialiseCheckout: function ()
    {
        viewModel.telemetry.sendTelemetryData('Sales', 'Checkout', 'Started');

        checkoutHelper.refreshUpsellCategories();

        addressHelper.bindableAddress(new Address(true));

        checkoutHelper.nextButton.displayText(textStrings.next);
        checkoutHelper.backButton.displayText(textStrings.back);

        // Do we need to display an asterix next to the "pay now" button
        if (settings.cardCharge !== undefined &&
            typeof settings.cardCharge === 'number' &&
            settings.cardCharge > 0)
        {
            checkoutHelper.payNowText(textStrings.checkSPayNowWithCharge);
        }
        else
        {
            checkoutHelper.payNowText(textStrings.checkSPayNow);
        }

        if (settings.customerAccountsEnabled && accountHelper.customerDetails != undefined)
        {
            // Pre-fill the checkout details from the customers account details
            checkoutHelper.checkoutDetails.firstName(accountHelper.customerDetails.firstname);
            checkoutHelper.checkoutDetails.surname(accountHelper.customerDetails.surname);
            checkoutHelper.checkoutDetails.emailAddress(accountHelper.emailAddress);

            // Are there any contact details in the customers profile?
            if (accountHelper.customerDetails.contacts != undefined)
            {
                for (var index = 0; index < accountHelper.customerDetails.contacts.length; index++)
                {
                    var contact = accountHelper.customerDetails.contacts[index];

                    if (contact.type == 'Email')
                    {
                        checkoutHelper.checkoutDetails.emailAddress(contact.value);
                    }
                    else if (contact.type == 'Mobile')
                    {
                        checkoutHelper.checkoutDetails.telephoneNumber(contact.value);
                        checkoutHelper.checkoutDetails.marketing(contact.marketingLevel == '3rdParty');
                    }
                }
            }

            // Is there a saved address?
            if (viewModel.orderType() == 'delivery' && accountHelper.customerDetails.address != undefined)
            {
                // We can pre-fill the checkout address using the customers profile
                checkoutHelper.checkoutDetails.address.fromPlainObject(accountHelper.customerDetails.address);
                checkoutHelper.checkoutDetails.address.isPostcodeLocked(false);

                checkoutHelper.checkoutDetails.isMissingAddress(checkoutHelper.isAddressMissing());
            }
        }
        else
        {           
            // TODO extract address from query string
            //checkoutHelper.checkoutDetails.firstName(queryStringHelper.firstName == undefined ? '' : queryStringHelper.firstName);
            //checkoutHelper.checkoutDetails.surname(queryStringHelper.lastName == undefined ? '' : queryStringHelper.lastName);
            //checkoutHelper.checkoutDetails.emailAddress(queryStringHelper.email == undefined ? '' : queryStringHelper.email);
            //checkoutHelper.checkoutDetails.telephoneNumber(queryStringHelper.telephoneNumber == undefined ? '' : queryStringHelper.telephoneNumber);
            //checkoutHelper.checkoutDetails.marketing(queryStringHelper.marketing == undefined ? '' : queryStringHelper.marketing);
            //checkoutHelper.checkoutDetails.address.town(queryStringHelper.town == undefined ? '' : queryStringHelper.town);
            //checkoutHelper.checkoutDetails.address.city(queryStringHelper.town == undefined ? '' : queryStringHelper.town);
            //checkoutHelper.checkoutDetails.address.postcode(queryStringHelper.postcode == undefined ? '' : queryStringHelper.postcode);
            //checkoutHelper.checkoutDetails.address.roadNum(queryStringHelper.houseNumber == undefined ? '' : queryStringHelper.houseNumber);
            //checkoutHelper.checkoutDetails.address.roadName(queryStringHelper.roadName == undefined ? '' : queryStringHelper.roadName);

            //checkoutHelper.checkoutDetails.address.prem1(queryStringHelper.prem1 == undefined ? '' : queryStringHelper.prem1);
            //checkoutHelper.checkoutDetails.address.prem2(queryStringHelper.prem2 == undefined ? '' : queryStringHelper.prem2);
            //checkoutHelper.checkoutDetails.address.prem3(queryStringHelper.prem3 == undefined ? '' : queryStringHelper.prem3);
            //checkoutHelper.checkoutDetails.address.org1(queryStringHelper.org1 == undefined ? '' : queryStringHelper.org1);
            //checkoutHelper.checkoutDetails.address.org2(queryStringHelper.org2 == undefined ? '' : queryStringHelper.org2);
            //checkoutHelper.checkoutDetails.address.userLocality1(queryStringHelper.userLocality1 == undefined ? '' : queryStringHelper.userLocality1);
            //checkoutHelper.checkoutDetails.address.userLocality2(queryStringHelper.userLocality2 == undefined ? '' : queryStringHelper.userLocality2);
            //checkoutHelper.checkoutDetails.address.userLocality3(queryStringHelper.userLocality3 == undefined ? '' : queryStringHelper.userLocality3);
            //checkoutHelper.checkoutDetails.address.directions(queryStringHelper.directions == undefined ? '' : queryStringHelper.directions);
        }

        // Use the store country
        // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
        // profile cannot be sent in the order JSON.  There is a hard coded country code for that.  We use proper ISO codes for the customer profile.
        checkoutHelper.checkoutDetails.address.country(viewModel.siteDetails().address.country);

        // Clear the checkout sections (tabs)
        checkoutHelper.checkoutSections.removeAll();

        // The page to add
        var index = 0;

        // Upselling
        if (settings.displayUpsellingPage && checkoutHelper.upsellCategories().length > 0)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutUpsellingContainer', noSelect: false, index: index, templateName: 'checkoutUpselling-template', displayName: textStrings.checkUpselling.replace('{index}', (index + 1)), validate: undefined });
            index++;
        }

        // Who are you
        if (settings.displayCustomerDetailsPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutContactDetailsContainer', noSelect: false, index: index, templateName: 'checkoutContactDetails-template', displayName: textStrings.whoAreYou.replace('{index}', (index + 1)), validate: checkoutHelper.validateContactDetails });
            index++;
        }

        // Delivery address
        if (viewModel.orderType() == 'delivery' && settings.displayCustomerAddressPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryAddressContainer', noSelect: false, index: index, templateName: 'checkoutDeliveryAddress-template', displayName: textStrings.deliveryAddress.replace('{index}', (index + 1)), validate: checkoutHelper.validateAddress });
            index++;
        }

        // Collection/delivery time
        if (viewModel.orderType() == 'collection')
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', noSelect: false, index: index, templateName: 'checkoutDeliveryTime-template', displayName: textStrings.collectionTime.replace('{index}', (index + 1)), validate: checkoutHelper.validateDeliveryTime });
            index++;
        }
        else
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', noSelect: false, index: index, templateName: 'checkoutDeliveryTime-template', displayName: textStrings.deliveryTime.replace('{index}', (index + 1)), validate: checkoutHelper.validateDeliveryTime });
            index++;
        }

        // Order summary
        if (settings.displayOrderSummaryPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutOrderSummaryContainer', noSelect: false, index: index, templateName: 'checkoutOrderSummary-template', displayName: textStrings.checkSSummary.replace('{index}', (index + 1)), validate: checkoutHelper.validateOrderSummary });
            index++;
        }

        // Payment
        if (settings.displayPaymentPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutPaymentContainer', noSelect: true, index: index, templateName: 'checkoutPayment-template', displayName: textStrings.checkPayment.replace('{index}', (index + 1)), validate: function () { return true; } });
            index++;
        }

        // Order notes
        if (settings.displayOrderNotesPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutOrderNotesContainer', noSelect: false, index: index, templateName: 'checkoutOrderNotes-template', displayName: textStrings.orderNotes.replace('{index}', (index + 1)), validate: function () { return true; } });
            index++;
        }

        // Tell the cart it's being shown on the checkout page
        AndroWeb.Helpers.CartHelper.isCheckoutMode(true);

        checkoutHelper.updateMultiLineDeliveryAddress();

        // See if the customer has provided a telephone number contact
        var telephoneNumber = '';
        if (accountHelper.customerDetails !== undefined && accountHelper.customerDetails.contacts !== undefined)
        {
            for (var contactIndex = 0; contactIndex < accountHelper.customerDetails.contacts.length; contactIndex++)
            {
                var contact = accountHelper.customerDetails.contacts[contactIndex];
                if (contact.type === 'Mobile')
                {
                    telephoneNumber = contact.value;
                }
            }
        }
        checkoutHelper.telephoneNumber(telephoneNumber);
    },
    isAddressMissing: function ()
    {
        if (checkoutHelper.checkoutDetails.address.roadName().length == 0 ||
            checkoutHelper.checkoutDetails.address.town().length == 0 ||
            checkoutHelper.checkoutDetails.address.postcode().length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    },
    refreshTimes: function ()
    {
        // Get todays opening times
        var openingTimes = openingTimesHelper.getTodaysOpeningTimes();

        var timeBlocks = [];
        var today = new Date();

        // The first available slot has to be at least EDT or ECT minutes from now.
        // If EDT/ECT is not available default to settings.defaultETD minutes.
        var offset = settings.defaultETD;

        if (viewModel.orderType() == "delivery")
        {
            // It's a delivery - use ETD if available
            if (viewModel.selectedSite().estDelivTime != undefined && viewModel.selectedSite().estDelivTime > 0)
            {
                offset = viewModel.selectedSite().estDelivTime;
            }
        }
        else if (viewModel.orderType() == "collection")
        {
            // It's a collection - use ECT if available
            if (viewModel.selectedSite().estCollTime != undefined && viewModel.selectedSite().estCollTime > 0)
            {
                offset = viewModel.selectedSite().estCollTime;
            }
        }

        // The earliest time that an order can be delivered/collected
        var today = new Date(new Date().getTime() + offset * 60000);

        var hourNow = today.getHours();
        var minuteNow = today.getMinutes();

        var isOpenNow = false;

        // Go through todays delivery times and get the delivery times after now
        for (var timespanIndex = 0; timespanIndex < openingTimes.length; timespanIndex++)
        {
            var timeSpan = openingTimes[timespanIndex];

            if (timeSpan.openAllDay)
            {
                // Store is open all day so delivery times are from now to 6am (well, 5:59am)
                timeBlocks.push({ startHour: hourNow, startMinute: minuteNow, endHour: 5, endMinute: 59 });
                isOpenNow = true;
                break;
            }
            else
            {
                // Work out if this opening time block is before now, after now or overlaps now
                var startBits = timeSpan.startTime.split(':');
                var startHour = Number(startBits[0]);
                var startMinute = Number(startBits[1]);

                var endBits = timeSpan.endTime.split(':');
                var endHour = Number(endBits[0]);
                var endMinute = Number(endBits[1]);

                if (startHour > hourNow || (startHour == hourNow && startMinute > minuteNow))
                {
                    // Time block is in the future
                    timeBlocks.push({ startHour: startHour, startMinute: startMinute, endHour: endHour, endMinute: endMinute });
                }
                else if (endHour > hourNow ||
                    (hourNow > 6 && endHour < 6) || // Before 6am is the previous day i.e. closes after midnight
                    (endHour == hourNow && endMinute > minuteNow))
                {
                    // Time block has already started but not ended
                    timeBlocks.push({ startHour: hourNow, startMinute: minuteNow, endHour: endHour, endMinute: endMinute });

                    isOpenNow = true;
                }
            }
        }

        var slotSize = 15;
        var slots = [];

        // Generate delivery slots for each block of time the store is open
        for (var timeBlockIndex = 0; timeBlockIndex < timeBlocks.length; timeBlockIndex++)
        {
            var timeSpan = timeBlocks[timeBlockIndex];

            // Add slots in start hour
            checkoutHelper.addDeliveryHourSlots(
                timeSpan.startHour,
                timeSpan.startMinute,
                timeSpan.endHour == timeSpan.startHour ? timeSpan.endMinute : 60,
                slotSize,
                slots);

            // Add slots for the each hour inbetween the start and end hours
            var endHour = timeSpan.endHour == 0 ? 24 : timeSpan.endHour;
            endHour = endHour < 6 ? 24 : endHour; // Before 6am is the previous day i.e. closes after midnight
            for (var hour = timeSpan.startHour + 1; hour < endHour; hour++)
            {
                checkoutHelper.addDeliveryHourSlots(hour, 0, 60, slotSize, slots);
            }

            // Do we need to add hours after midnight?
            if (timeSpan.endHour < 6)
            {
                for (var hour = 0; hour < timeSpan.endHour; hour++)
                {
                    checkoutHelper.addDeliveryHourSlots(hour, 0, 60, slotSize, slots);
                }
            }

            if (timeSpan.endHour > timeSpan.startHour)
            {
                // Add slots for the last hour
                checkoutHelper.addDeliveryHourSlots(timeSpan.endHour, 0, timeSpan.endMinute, slotSize, slots);
            }
        }

        if (isOpenNow && slots.length > 0)
        {
            var firstTimeSlot = slots[0];
            firstTimeSlot.text = textStrings.checkASAP.replace('{time}', firstTimeSlot.text);
            firstTimeSlot.mode = 'ASAP';
        }

        // Remove delivery slots for which one or more item are not available
        var wereSlotsRemoved = checkoutHelper.removeUnavailableDeliverySlots(slots, AndroWeb.Helpers.CartHelper.cart().cartItems(), AndroWeb.Helpers.CartHelper.cart().deals());
        checkoutHelper.checkoutDetails.slotsRemoved(wereSlotsRemoved);

        // Update the list
        checkoutHelper.times(slots);
    },
    refreshTimeslotsAndCheckSelected: function (previouslySelectedTime)
    {
        // Refresh timeslots
        checkoutHelper.refreshTimes();

        // Check to see if the previously selected timeslot is still in the list
        for (var timeslotIndex = 0; timeslotIndex < checkoutHelper.times().length; timeslotIndex++)
        {
            var timeslot = checkoutHelper.times()[timeslotIndex];

            if (timeslot.time === previouslySelectedTime)
            {
                checkoutHelper.checkoutDetails.wantedTime(timeslot);
                return true;
            }
        }

        return false;
    },
    addDeliveryHourSlots: function (hour, startMinute, endMinute, slotSize, slots)
    {
        // Get the first and last slots for this hour
        var startSlot = Math.ceil(startMinute / slotSize) * slotSize;
        var endSlot = Math.ceil(endMinute / slotSize) * slotSize;

        // Get all the slots between the start and end minutes
        for (var slot = startSlot; slot < endSlot; slot += slotSize)
        {
            checkoutHelper.addDeliverySlot(hour, slot, slotSize, slots);
        }
    },
    addDeliverySlot: function (hour, minute, slotSize, slots)
    {
        var hour12HourClock = hour > 12 ? hour - 12 : hour;
        hour12HourClock = (hour12HourClock == 0 ? 12 : hour12HourClock);
        var hourAMPM = hour >= 12 ? ' ' + textStrings.checkPM : ' ' + textStrings.checkAM;
        var hourPlusOne = (hour + 1) > 23 ? 12 : (hour + 1);
        var hourPlusOne12HourClock = hourPlusOne > 12 ? hourPlusOne - 12 : hourPlusOne;
        var hourPlusOneAMPM = hourPlusOne >= 12 ? ' ' + textStrings.checkPM : ' ' + textStrings.checkAM;

        if (minute + slotSize > 59)
        {
            // Slot overlaps into the next hour
            slots.push
            (
                {
                    mode: 'TIMED',
                    time: hour + ':' + minute,
                    startHour: hour,
                    startMinute: minute,
                    endHour: hour,
                    endMinute: (slotSize - (60 - minute)),
                    text: checkoutHelper.formatHour(helper.use24hourClock ? hour : hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + (helper.use24hourClock ? '' : hourAMPM) + ' - ' +
                          checkoutHelper.formatHour(helper.use24hourClock ? hour : hourPlusOne12HourClock) + ':' + checkoutHelper.formatMinute((slotSize - (60 - minute))) + (helper.use24hourClock ? '' : hourPlusOneAMPM)
                }
            );
        }
        else
        {
            // Slot is entirely within the hour
            slots.push
            (
                {
                    mode: 'TIMED',
                    time: hour + ':' + minute,
                    startHour: hour,
                    startMinute: minute,
                    endHour: hour,
                    endMinute: minute + slotSize,
                    text: checkoutHelper.formatHour(helper.use24hourClock ? hour : hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + (helper.use24hourClock ? '' : hourAMPM) + ' - ' +
                          checkoutHelper.formatHour(helper.use24hourClock ? hour : hour12HourClock) + ':' + checkoutHelper.formatMinute((minute + slotSize)) + (helper.use24hourClock ? '' : hourAMPM)
                }
            );
        }
    },
    formatMinute: function (minute)
    {
        if (minute < 10)
            return '0' + minute;
        else
            return minute;
    },
    formatHour: function (hour)
    {
        if (hour < 10)
            return ' ' + hour;
        else
            return hour;
    },
    removeUnavailableDeliverySlots: function(timeSlots, cartItems, dealCartItems)
    {
        var removeTimeSlots = [];

        // Check each time slot to see if any items or deals are not available in it
        for (var timeSlotIndex = 0; timeSlotIndex < timeSlots.length; timeSlotIndex++)
        {
            var timeSlot = timeSlots[timeSlotIndex];

            // Scan through each cart item and work out if it's available in this time slot
            for (var cartItemIndex = 0; cartItemIndex < cartItems.length; cartItemIndex++)
            {
                var cartItem = cartItems[cartItemIndex];
                this.checkTimeSlotAgainstItems(timeSlot, timeSlotIndex, cartItem, removeTimeSlots);
            }

            // Scan through each deal in the cart and work out if it's available in this time slot
            if (dealCartItems !== undefined)
            {
                for (var dealCartItemIndex = 0; dealCartItemIndex < dealCartItems.length; dealCartItemIndex++)
                {
                    var dealCartItem = dealCartItems[dealCartItemIndex];
                    this.checkTimeSlotAgainstItems(timeSlot, timeSlotIndex, dealCartItem.dealWrapper, removeTimeSlots);
                }
            }
        }

        // We need to remove time slots starting at the end of the array moving to the start of the array
        // The items in the remove array could be in any order so we need to sort them high to low
        removeTimeSlots.sort
        (
            function (a, b)
            {
                return Number(b) - Number(a)
            }
        );

        // Remove time slots
        for (var removeTimeSlotIndex = 0; removeTimeSlotIndex < removeTimeSlots.length; removeTimeSlotIndex++)
        {
             timeSlots.splice(removeTimeSlots[removeTimeSlotIndex], 1);
        }

        return (removeTimeSlots.length > 0);
    },
    checkTimeSlotAgainstItems: function (timeSlot, timeSlotIndex, item, removeTimeSlots)
    {
        // Assume that one of the items is not available in this time slot unless we find one
        var removeTimeSlot = true;

        if (item.availableTimes === undefined || item.availableTimes.availableAllDay === true)
        {
            // The item is available all day so the time slot is valid
            removeTimeSlot = false;
        }
        else
        {
            // Item is only available at specific times of the day so the time slot may not be valie
            for (var availableTimeIndex = 0; availableTimeIndex < item.availableTimes.times.length; availableTimeIndex++)
            {
                var availableTime = item.availableTimes.times[availableTimeIndex];

                var availableStartTime = menuHelper.getAvailableTime(availableTime.from, true);
                var availableStartHour = availableStartTime.getHours();
                var availableStartMinute = availableStartTime.getMinutes();

                var availableEndTime = menuHelper.getAvailableTime(availableTime.to, true);
                var availableEndHour = availableEndTime.getHours();
                var availableEndMinute = availableEndTime.getMinutes();

                var timeSlotStartHour = menuHelper.adjustForTradingDay(timeSlot.startHour);
                var timeSlotEndHour = menuHelper.adjustForTradingDay(timeSlot.endHour);

                // Time slot is wholey inside the available time
                if ((timeSlotStartHour > availableStartHour || (timeSlotStartHour === availableStartHour && timeSlot.startMinute >= availableStartMinute)) &&
                    (timeSlotStartHour < availableEndHour || (timeSlotStartHour == availableEndHour && timeSlot.startMinute < availableEndMinute)))
                {
                    // The item is available for the whole timeslot
                    removeTimeSlot = false;
                }
            }
        }

        if (removeTimeSlot)
        {
            // This item caused one or more timeslot to be removed
            item.removedTimeSlot(true);

            // Add the timeslot to the list of time slots that need to be removed
            if (removeTimeSlots.indexOf(timeSlotIndex) == -1)
            {
                removeTimeSlots.push(timeSlotIndex);
            }
        }
    },
    visibleCheckoutSection: ko.observable(0),
    times: ko.observableArray(),
    paymentProvider: ko.observable(undefined),
    placeOrder: function ()
    {
        // Has the user entered the required information?
        if (checkoutHelper.validate())
        {
            if (settings.checkoutUpdateCustomerAccount)
            {
                // Do we need to save the contact details
                if (settings.customerAccountsEnabled && checkoutHelper.checkoutDetails.rememberContactDetails)
                {
                    // Update the contact details
                    accountHelper.customerDetails.firstName = checkoutHelper.checkoutDetails.firstName();
                    accountHelper.customerDetails.surname = checkoutHelper.checkoutDetails.surname();
                    accountHelper.customerDetails.telephoneNumber = checkoutHelper.checkoutDetails.telephoneNumber();
                }

                // Do we need to save the address in the customers details?
                if (settings.customerAccountsEnabled && checkoutHelper.checkoutDetails.rememberAddress)
                {
                    // Update the address details
                    accountHelper.customerDetails.address =
                    {
                        prem1: checkoutHelper.checkoutDetails.address.prem1(),
                        prem2: checkoutHelper.checkoutDetails.address.prem2(),
                        prem3: checkoutHelper.checkoutDetails.address.prem3(),
                        prem4: checkoutHelper.checkoutDetails.address.prem4(),
                        prem5: checkoutHelper.checkoutDetails.address.prem5(),
                        prem6: checkoutHelper.checkoutDetails.address.prem6(),
                        org1: checkoutHelper.checkoutDetails.address.org1(),
                        org2: checkoutHelper.checkoutDetails.address.org2(),
                        org3: checkoutHelper.checkoutDetails.address.org3(),
                        roadNum: checkoutHelper.checkoutDetails.address.roadNum(),
                        roadName: checkoutHelper.checkoutDetails.address.roadName(),
                        city: checkoutHelper.checkoutDetails.address.city(),
                        town: checkoutHelper.checkoutDetails.address.town(),
                        postcode: checkoutHelper.checkoutDetails.address.postcode(),
                        county: checkoutHelper.checkoutDetails.address.county(),
                        state: checkoutHelper.checkoutDetails.address.state(),
                        locality: checkoutHelper.checkoutDetails.address.locality(),
                        country: checkoutHelper.checkoutDetails.address.country(),
                        directions: checkoutHelper.checkoutDetails.address.directions(),
                        userLocality1: checkoutHelper.checkoutDetails.address.userLocality1(),
                        userLocality2: checkoutHelper.checkoutDetails.address.userLocality2(),
                        userLocality3: checkoutHelper.checkoutDetails.address.userLocality3()
                    };

                    // Find the existing email contact - we need the marketing level
                    var emailMarketingLevel = '3rdParty';
                    for (var index = 0; index < accountHelper.customerDetails.contacts.length; index++)
                    {
                        var contact = accountHelper.customerDetails.contacts[index];
                        if (contact.type == 'Email')
                        {
                            emailMarketingLevel = contact.marketingLevel;
                            break;
                        }
                    }

                    // Clear out the contacts
                    accountHelper.customerDetails.contacts = [];

                    // Add an email contact
                    accountHelper.customerDetails.contacts.push
                    (
                        {
                            type: 'Email',
                            value: accountHelper.emailAddress,
                            marketingLevel: emailMarketingLevel
                        }
                    );

                    // Is there already phone contact?
                    if (checkoutHelper.checkoutDetails.telephoneNumber() != undefined && checkoutHelper.checkoutDetails.telephoneNumber().length > 0)
                    {
                        // Add a phone contact
                        accountHelper.customerDetails.contacts.push
                        (
                            {
                                type: 'Mobile',
                                value: checkoutHelper.checkoutDetails.telephoneNumber(),
                                marketingLevel: checkoutHelper.checkoutDetails.marketing() ? '3rdParty' : 'OrderOnly'
                            }
                        );
                    }

                    // Save the customer account details
                    acsapi.postCustomer
                    (
                        accountHelper.emailAddress,
                        accountHelper.password,
                        undefined,
                        accountHelper.customerDetails,
                        checkoutHelper.showPaymentPicker,
                        function ()
                        {
                            // Error!
                        }
                    );
                }
            }
            else
            {
                // Show the payment page
                checkoutHelper.showPaymentPicker();
            }
        }
    },
    showPaymentPicker: function ()
    {
        // Switch the cart to checkout mode
        guiHelper.cartActions(guiHelper.cartActionsCheckout);

        guiHelper.canChangeOrderType(false);

        viewModel.pageManager.showPage('Payment', true, undefined, true);
    },
    payAtDoor: function ()
    {
        // Make sure there is no card charge
        AndroWeb.Helpers.CartHelper.cart().cardCharge = 0;

        // Has the user provided an address
        if (viewModel.orderType() === 'delivery' && checkoutHelper.isAddressMissing())
        {
            checkoutHelper.changeAddress();
            return;
        }

        // Check that store delivers to the customers postcode
        if (!checkoutHelper.validatePostcode(checkoutHelper.checkoutDetails.address.postcode()))
        {
            alert(textStrings.hBadDeliveryZone);
            return;
        }

        // Check that the timeslot is still valid
        if (!checkoutHelper.refreshTimeslotsAndCheckSelected(checkoutHelper.checkoutDetails.wantedTime().time))
        {
            alert(textStrings.checkTimeslotExpired);
            return;
        }

        // Hide the mobile checkout menu
        checkoutMenuHelper.hideMenu();

        // Customer wants to pay now
        checkoutHelper.checkoutDetails.payNow(false);

        // Is the store still online?
        checkoutHelper.prePaymentOnlineCheck
        (
            function ()
            {
                viewModel.telemetry.sendTelemetryData('Sales', 'Checkout', 'PayCash');

                // Store is still online
                checkoutHelper.sendOrderToStore();
            }
        );
    },
    payNow: function ()
    {
        if (viewModel.orderType() === 'dinein')
        {
            // Has the user specified a table number? 
            if (checkoutHelper.checkoutDetails.tableNumber() == undefined || checkoutHelper.checkoutDetails.tableNumber() === "")
            {
              //  alert(textStrings.checkMissingTableNumber);
                return;
            }
        }
        else if (viewModel.orderType() === 'delivery')
        {
            // Has the user provided an address
            if (checkoutHelper.isAddressMissing())
            {
                checkoutHelper.changeAddress();
                return;
            }

            // Check that store delivers to the customers postcode
            if (!checkoutHelper.validatePostcode(checkoutHelper.checkoutDetails.address.postcode()))
            {
                alert(textStrings.hBadDeliveryZone);
                return;
            }
        }
        
        if (viewModel.orderType() === 'delivery' || viewModel.orderType() === 'collection')
        {
            // Check that the timeslot is still valid
            if (!checkoutHelper.refreshTimeslotsAndCheckSelected(checkoutHelper.checkoutDetails.wantedTime().time))
            {
                alert(textStrings.checkTimeslotExpired);
                return;
            }
        }

        // Stop updating wanted times
        if (checkoutHelper.checkoutViewModel !== undefined)
        {
            checkoutHelper.checkoutViewModel.updateTimeslots = false;
        }

        guiHelper.canChangeOrderType(false);

        // Customer wants to pay now
        checkoutHelper.checkoutDetails.payNow(true);

        // Is the store still online?
        checkoutHelper.prePaymentOnlineCheck
        (
            function ()
            {
                setTimeout
                (
                    function ()
                    {
                        viewModel.telemetry.sendTelemetryData('Sales', 'Checkout', 'PayCard');

                        viewModel.pleaseWaitMessage(textStrings.sovPreparingPayment);
                        viewModel.pleaseWaitProgress('');

                        // Add a card charge to the order
                        AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart(), true);

                        // Store is still online - process the payment
                        if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
                        {
                            checkoutHelper.mercuryPayment();
                        }
                        else if
                        (
                            viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHLIVE' ||
                            viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHTEST'
                        )
                        {
                            checkoutHelper.datacashPayment();
                        }
                        else if
                        (
                            viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCANETLIVE' ||
                            viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCANETTEST'
                        )
                        {
                            checkoutHelper.mercanetPayment();
                        }
                        else if 
                        (
                            viewModel.siteDetails().paymentProvider.toUpperCase() == 'PAYPALLIVE' ||
                            viewModel.siteDetails().paymentProvider.toUpperCase() == 'PAYPALTEST'
                        ) 
                        {
                            checkoutHelper.payPalPayment();
                        }
                        else
                        {
                            // There is a problem with the payment provider - not supported
                            viewHelper.showHome();
                        }
                    },
                    0
                )
            }
        );
    },
    prePaymentOnlineCheck: function(callback)
    {
        viewModel.pleaseWaitMessage(textStrings.sovCheckingStoreStatus);
        viewModel.pleaseWaitProgress('');

        var pleaseWaitViewModel = new AndroWeb.ViewModels.PleaseWaitViewModel
        (
            function ()
            {
                if (settings.alwaysOnline)
                {
                    callback();
                }
                else
                {
                    // Get the current status of the store from the server
                    acsapi.getSite
                    (
                        viewModel.selectedSite().siteId,
                        0,
                        function (data)
                        {
                            try
                            {
                                // Let the user know we're waiting for the server
                                viewModel.pleaseWaitMessage(textStrings.sovCheckingStoreStatus);
                                viewModel.pleaseWaitProgress('');

                                // Allow JavaScript to process events (kind of like doEvents)
                                setTimeout
                                (
                                    function ()
                                    {
                                        try
                                        {
                                            // Did that work?
                                            if (data == undefined || data.Details == undefined)
                                            {
                                                // No data - not good
                                                viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.storeSelected, undefined);
                                            }
                                            else
                                            {
                                                // Is the store online?
                                                if (data.Details.isOpen)
                                                {
                                                    // Store is online - continue with the payment...
                                                    callback();
                                                }
                                                else
                                                {
                                                    // Store is offline - can't send the order to the store
                                                    viewHelper.showError(textStrings.sOffline, checkoutHelper.showPaymentPicker, undefined);
                                                }
                                            }
                                        }
                                        catch (exception)
                                        {
                                            // Got an error
                                            viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.storeSelected, exception);
                                        }
                                    },
                                    0
                                );
                            }
                            catch (exception)
                            {
                                // Got an error
                                viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.storeSelected, exception);
                            }
                        },
                        function ()
                        {
                            viewModel.storeSelected();
                        },
                        true
                    );
                }
            }
        );

        viewModel.pageManager.showPage('PleaseWait', false, pleaseWaitViewModel, false);
    },
    mercuryPayment: function()
    {
        // Initialise the mercury payment
        acsapi.putMercuryPayment
        (
            viewModel.selectedSite().siteId,
            function ()
            {
                viewModel.pageManager.showPage('MercuryPayment', true, undefined, true);
            },
            function ()
            {
                guiHelper.showCheckoutAfterError();
            }
        );
    },
    datacashPayment: function(order)
    {
        // TESTING - Super fudgetastic
        checkoutHelper.simulateDataCashError = (checkoutHelper.checkVoucherCode() === 'FAIL1');

        // Generate the order JSON
        var orderDetails = checkoutHelper.generateOrderJson(AndroWeb.Helpers.CartHelper.cart().vouchers());

        // Initialise the DataCash payment
        acsapi.putDataCashPayment
        (
            viewModel.selectedSite().siteId,
            orderDetails,
            function (result)
            {
                if (result != undefined &&
                    result.data != undefined &&
                    result.data.reference != undefined)
                {
                    // Payment was succesfully setup
                    // Keep hold of the DataCash payment id for the second phase
                    AndroWeb.Helpers.CartHelper.cart().dataCashPaymentDetails(result.data);

                    // Show the customer facing DataCash payment page
                    viewModel.pageManager.showPage('DatacashPayment', true, undefined, true);
                }
                else
                {
                    // Assume that an attempt was made to send the order to the store
                    // Go to the send order post processing - display the success page or an error page
                    checkoutHelper.sendOrderToStoreCallback(result);
                }
            },
            function ()
            {
                guiHelper.showCheckoutAfterError();
            }
        );
    },
    mercanetPayment: function()
    {
        // Generate the order JSON
        var orderDetails = checkoutHelper.generateOrderJson(checkoutHelper.voucherCodes);

        // Initialise the Mercanet payment
        acsapi.putMercanetPayment
        (
            viewModel.selectedSite().siteId,
            orderDetails,
            function (json)
            {
                var jsonObject = typeof(json) == 'string' ? jQuery.parseJSON(json) : json;
                var html = jsonObject.Html;
                var orderRef = jsonObject.OrderReference;

                // We need to save the order reference in the cookie as we're about to get redirected to another webpage and we'll lose state
                cookieHelper.setCookie('or', jsonObject.OrderReference);

                viewModel.pageManager.showPage('MercanetPayment', true, undefined, true);
            },
            function ()
            {
                guiHelper.showCheckoutAfterError();
            }
        );
    },
    payPalPayment: function ()
    {
        // Generate the order JSON
        var orderDetails = checkoutHelper.generateOrderJson(checkoutHelper.voucherCodes);

        // Initialise the PayPal payment
        acsapi.putPayPalPayment
        (
            viewModel.selectedSite().siteId,
            orderDetails,
            function (result)
            {
                if (result != undefined &&
                    result.data != undefined &&
                    result.data.reference != undefined)
                {
                    var jsonObject = typeof (result.data) == 'string' ? jQuery.parseJSON(result.data) : result.data;
                    var url = jsonObject.url;
                    var orderRef = jsonObject.reference;

                    // We need to save the order reference in the cookie as we're about to get redirected to another webpage and we'll lose state
                    cookieHelper.setCookie('or', jsonObject.reference);

                    // Disable the warning alert
                    window.onbeforeunload = undefined;

                    window.location.href = url;
                }
                else
                {
                    // Assume that an attempt was made to send the order to the store
                    // Go to the send order post processing - display the success page or an error page
                    checkoutHelper.sendOrderToStoreCallback(result);
                }
            },
            function ()
            {
                guiHelper.showCheckoutAfterError();
            }
        );
    },
    sendOrderToStore: function ()
    {
        // Hide the mobile checkout menu
        checkoutMenuHelper.hideMenu();

        viewModel.pleaseWaitMessage(textStrings.sendingOrderToStore);
        viewModel.pleaseWaitProgress(textStrings.sendingOrderWarning);

        var pleaseWaitViewModel = new AndroWeb.ViewModels.PleaseWaitViewModel
        (
            function()
            {
                try
                {
                    // Generate the order JSON
                    var orderDetails = checkoutHelper.generateOrderJson(AndroWeb.Helpers.CartHelper.cart().vouchers());

                    // Send the order to ACS
                    acsapi.putOrder
                    (
                        viewModel.selectedSite().siteId,
                        orderDetails,
                        checkoutHelper.sendOrderToStoreCallback,
                        checkoutHelper.sendOrderToStoreCallback
                    );
                }
                catch (error)
                {
                    // Got an error
                    viewHelper.showError(textStrings.errDefaultWebErrorMessage, guiHelper.showCheckoutAfterError, error);
                }
            }
        );

        viewModel.pageManager.showPage('PleaseWait', false, pleaseWaitViewModel, false);
    },
    sendOrderToStoreCallback: function(result)
    {
        if (result === undefined || result.hasError)
        {
            viewModel.telemetry.sendTelemetryData('Sales', 'Checkout', 'Failed', result === undefined ? '' : result.errorCode);

            if (result.paymentReset)
            {
                AndroWeb.Helpers.CartHelper.cart().dataCashPaymentDetails(undefined);
            }

            // Let the user know there was an error
            viewHelper.showError
            (
                result === undefined ? textStrings.errDefaultWebErrorMessage : result.errorMessage,
                guiHelper.showCheckoutAfterError,
                undefined
            );
        }
        else
        {
            viewModel.telemetry.sendTelemetryData('Sales', 'Checkout', 'Completed', result.orderNumber);

            // The congrats page is shown after the order has been cleared but we need to know if a message should
            // be shown on the congrats page to tell the customer that there can be a delay gaining points
            checkoutHelper.showCongratsLoyaltyWarningMessage(loyaltyHelper.loyaltySession.spentPoints() === 0 && loyaltyHelper.loyaltySession.gainedPoints() > 0);

            // Clear the current order
            helper.clearOrder();

            // Make sure we switch the mobile menu back for the next page
            checkoutMenuHelper.showMenu('menu', true);

            // Hide the mobile checkout menu
            checkoutMenuHelper.hideMenu();

            var orderAcceptedViewModel = new OrderAcceptedViewModel();
            orderAcceptedViewModel.orderNumber = result.orderNumber;

            if (result.isProvisional)
            {
                // Show gprs congrats message
                orderAcceptedViewModel.congratsMessage = textStrings.oavOrderPending;
                orderAcceptedViewModel.congratsMessage = orderAcceptedViewModel.congratsMessage.replace('{orderNumber}', result.orderNumber);
            }
            else
            {
                // Show standard congrats message
                orderAcceptedViewModel.congratsMessage = textStrings.oavOrderAcepted;
                orderAcceptedViewModel.congratsMessage = orderAcceptedViewModel.congratsMessage.replace('{orderNumber}', result.orderNumber);
            }

            // Is loyalty enabled? 
            if (loyaltyHelper.IsEnabled())
            {
                // Get the customers profile from the server
                accountHelper.login
                (
                    accountHelper.username,
                    accountHelper.password,
                    function (success)
                    {
                        // Make sure the cuatomers loyalty details are upto date
                        loyaltyHelper.refreshCustomerSession();

                        viewModel.pageManager.showPage('OrderResult', true, orderAcceptedViewModel, true);
                    }
                );
            }
            else
            {
                // Make sure the cuatomers loyalty details are upto date
                loyaltyHelper.refreshCustomerSession();

                viewModel.pageManager.showPage('OrderResult', true, orderAcceptedViewModel, true);
            }
        }
    },
    generateOrderJson: function (vouchers)
    {
        // Work out how long the customer took to place the order
        var timeNow = new Date();
        var timeTakenMilliseconds = Math.abs(timeNow - viewModel.timer);
        var timeTakenSeconds = Math.round(timeTakenMilliseconds / 1000);
        var loyaltyJson = loyaltyHelper.ProduceJsonForCheckout();
        
        var orderDetails =
        {
            toSiteId: viewModel.selectedSite().siteId,
            paymentType: checkoutHelper.checkoutDetails.payNow() ? viewModel.siteDetails().paymentProvider : 'PayLater',
            paymentData: undefined,
            
            order:
            {
                loyalty: loyaltyJson,
                eventName: settings.generateTestEventName ? 'Test event' : '',
                vouchers: vouchers,
                partnerReference: '',
                type: viewModel.orderType(),
                orderTimeType: checkoutHelper.checkoutDetails.wantedTime() === undefined ? undefined : checkoutHelper.checkoutDetails.wantedTime().mode,
                orderWantedTime: checkoutHelper.checkoutDetails.wantedTime() === undefined ? undefined : helper.formatUTCSlot(checkoutHelper.checkoutDetails.wantedTime().time),
                orderPlacedTime: helper.formatUTCDate(new Date()),
                timeToTake: timeTakenSeconds,
                chefNotes: checkoutHelper.checkoutDetails.chefNotes(),
                oneOffDirections: checkoutHelper.checkoutDetails.orderNotes(),
                estimatedDeliveryTime: 0,
                customer:
                {
                    title: '',
                    firstName: checkoutHelper.checkoutDetails.firstName(),
                    surname: checkoutHelper.checkoutDetails.surname(),
                    contacts:
                    [
                        {
                            type: 'Mobile',
                            value: checkoutHelper.checkoutDetails.telephoneNumber(),
                            marketingLevel: checkoutHelper.checkoutDetails.marketing() ? 'OrderOnly' : 'None'
                        },
                        {
                            type: 'Email',
                            value: checkoutHelper.checkoutDetails.emailAddress(),
                            marketingLevel: checkoutHelper.checkoutDetails.marketing() ? 'OrderOnly' : 'None'
                        }
                    ],
                    customerId: accountHelper.customerDetails.accountNumber
                },
                pricing:
                {
                    pricesIncludeTax: 'true',
                    priceBeforeDiscount: AndroWeb.Helpers.CartHelper.cart().subTotalPrice(),
                    discounts: [],
                    priceAfterDiscount: AndroWeb.Helpers.CartHelper.cart().totalPrice() - (AndroWeb.Helpers.CartHelper.cart().deliveryCharge),
                    finalPrice: AndroWeb.Helpers.CartHelper.cart().totalPrice() - (AndroWeb.Helpers.CartHelper.cart().deliveryCharge),
                    deliveryCharge: AndroWeb.Helpers.CartHelper.cart().deliveryCharge
                },
                orderLines:
                [
                ],
                orderPayments:
                [
                ]
            }
        };

        if (viewModel.orderType() == "delivery" || viewModel.orderType() == "collection")
        {
            orderDetails.order.customer.address = {
                prem1: checkoutHelper.checkoutDetails.address.prem1() == undefined || checkoutHelper.checkoutDetails.address.prem1().length == 0 ? '' : textStrings.prem1Prefix + checkoutHelper.checkoutDetails.address.prem1(),
                prem2: checkoutHelper.checkoutDetails.address.prem2() == undefined || checkoutHelper.checkoutDetails.address.prem2().length == 0 ? '' : textStrings.prem2Prefix + checkoutHelper.checkoutDetails.address.prem2(),
                prem3: checkoutHelper.checkoutDetails.address.prem3() == undefined || checkoutHelper.checkoutDetails.address.prem3().length == 0 ? '' : textStrings.prem3Prefix + checkoutHelper.checkoutDetails.address.prem3(),
                prem4: checkoutHelper.checkoutDetails.address.prem4(),
                prem5: checkoutHelper.checkoutDetails.address.prem5(),
                prem6: checkoutHelper.checkoutDetails.address.prem6(),
                org1: checkoutHelper.checkoutDetails.address.org1(),
                org2: checkoutHelper.checkoutDetails.address.org2(),
                org3: checkoutHelper.checkoutDetails.address.org3(),
                roadNum: checkoutHelper.checkoutDetails.address.roadNum(),
                roadName: checkoutHelper.checkoutDetails.address.roadName(),
                city: checkoutHelper.checkoutDetails.address.city(),
                town: checkoutHelper.checkoutDetails.address.town(),
                postcode: checkoutHelper.checkoutDetails.address.postcode(),
                county: checkoutHelper.checkoutDetails.address.county(),
                state: checkoutHelper.checkoutDetails.address.state(),
                locality: checkoutHelper.checkoutDetails.address.locality(),
                directions: checkoutHelper.checkoutDetails.address.directions(),
                userLocality1: checkoutHelper.checkoutDetails.address.userLocality1() == undefined || checkoutHelper.checkoutDetails.address.userLocality1().length == 0 ? '' : textStrings.userLocality1Prefix + checkoutHelper.checkoutDetails.address.userLocality1(),
                userLocality2: checkoutHelper.checkoutDetails.address.userLocality2() == undefined || checkoutHelper.checkoutDetails.address.userLocality2().length == 0 ? '' : textStrings.userLocality2Prefix + checkoutHelper.checkoutDetails.address.userLocality2(),
                userLocality3: checkoutHelper.checkoutDetails.address.userLocality3() == undefined || checkoutHelper.checkoutDetails.address.userLocality3().length == 0 ? '' : textStrings.userLocality3Prefix + checkoutHelper.checkoutDetails.address.userLocality3(),

                // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
                // profile or the stores country cannot be sent in the order JSON.  There is a hard coded country code for that.
                country: settings.customerAddressCountryCode
            };
        }
        else if (viewModel.orderType() == "dinein")
        {
            orderDetails.order.tableNumber = checkoutHelper.checkoutDetails.tableNumber();
            orderDetails.order.orderTimeType = 'ASAP';
            orderDetails.order.orderWantedTime = new Date();

            if (settings.sendDineInOrdersAsDelivery === true)
            {
                orderDetails.order.type = 'delivery';
                orderDetails.order.customer.address = {
                    prem1: '',
                    prem2: '',
                    prem3: '',
                    prem4: '',
                    prem5: '',
                    prem6: '',
                    org1: '',
                    org2: '',
                    org3: '',
                    roadNum: '',
                    roadName: 'DINE IN for table ' + orderDetails.order.tableNumber,
                    city: '',
                    town: '',
                    postcode: '',
                    county: '',
                    state: '',
                    locality: '',
                    directions: '',
                    userLocality1: '',
                    userLocality2: '',
                    userLocality3: '',


                    // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
                    // profile or the stores country cannot be sent in the order JSON.  There is a hard coded country code for that.
                    country: settings.customerAddressCountryCode
                };
            }
            else
            {
                // Sadly, we still need to send a blank address - otherwise ACS explodes...
                orderDetails.order.customer.address = {
                    prem1: '',
                    prem2: '',
                    prem3: '',
                    prem4: '',
                    prem5: '',
                    prem6: '',
                    org1: '',
                    org2: '',
                    org3: '',
                    roadNum: '',
                    roadName: '',
                    city: '',
                    town: '',
                    postcode: '',
                    county: '',
                    state: '',
                    locality: '',
                    directions: '',
                    userLocality1: '',
                    userLocality2: '',
                    userLocality3: '',


                    // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
                    // profile or the stores country cannot be sent in the order JSON.  There is a hard coded country code for that.
                    country: settings.customerAddressCountryCode
                };
            }
        }

        // Do we need to deliberately cause a failure at the store end?
        if (settings.simulateBadOrder)
        {
            // Rameses doesn't like fractions
            orderDetails.order.orderTimeType = 'xxxxxxxxx';
            orderDetails.order.orderWantedTime = 'xxxxxxxxx';
            orderDetails.order.orderPlacedTime = 'xxxxxxxxx';

      //      orderDetails.order.pricing.finalPrice = 'xxxxx';
        }
        else
        {
            // Was a full order discount applied?
            if (AndroWeb.Helpers.CartHelper.cart().discountAmount() != undefined &&
                AndroWeb.Helpers.CartHelper.cart().discountAmount() > 0 &&
                menuHelper.fullOrderDiscountDeal != undefined)
            {
                var discount =
                    {
                        type: 'FullOrderDiscount',
                        // Add the full order discount details
                        discountType: menuHelper.fullOrderDiscountDeal.FullOrderDiscountType,
                        discountTypeAmount: undefined,
                        discountAmount: AndroWeb.Helpers.CartHelper.cart().discountAmount(),
                        initialDiscountReason: AndroWeb.Helpers.CartHelper.cart().discountName()
                    };

                // Add the full order discount details
                if (viewModel.orderType() == 'collection')
                {
                    discount.discountTypeAmount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount;
                }
                else
                {
                    discount.discountTypeAmount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount;
                }

                orderDetails.order.pricing.discounts.push(discount);
            }
        }

        // Apply vouchers (if there are any)
        if (vouchers != undefined && vouchers.length > 0)
        {
            for (var voucherCodeIndex = 0; voucherCodeIndex < vouchers.length; voucherCodeIndex++)
            {
                var voucherCode = vouchers[voucherCodeIndex];

                if (voucherCode.IsValid)
                {
                    var discount =
                    {
                        type: 'Voucher',
                        discountType: voucherCode.EffectType,
                        discountTypeAmount: voucherCode.EffectType === 'Fixed' ? (voucherCode.EffectValue * 100) : voucherCode.EffectValue,
                        discountAmount: voucherCode.discount(),
                        initialDiscountReason: voucherCode.VoucherCode
                    };

                    orderDetails.order.pricing.discounts.push(discount);
                }
            }
        }

        // Apply loyalty
        if (loyaltyHelper.IsEnabled())
        {
            if (loyaltyHelper.loyaltySession.spentPointsValue() != undefined && loyaltyHelper.loyaltySession.spentPointsValue() > 0)
            {
                var discount =
                {
                    type: 'Loyalty',
                    discountType: 'Fixed',
                    discountTypeAmount: loyaltyHelper.loyaltySession.spentPointsValue(),
                    discountAmount: loyaltyHelper.loyaltySession.spentPointsValue(),
                    initialDiscountReason: 'Loyalty'
                };

                orderDetails.order.pricing.discounts.push(discount);
            }
        }

        // Apply discount
        // There can only be one single discount so we'll need to merge all discounts together
        var discountAmount = 0;
        var initialDiscountReason = '';

        for (var discountIndex = 0; discountIndex < orderDetails.order.pricing.discounts.length; discountIndex++)
        {
            var discount = orderDetails.order.pricing.discounts[discountIndex];

            discountAmount += discount.discountAmount;

            if (initialDiscountReason.length > 0)
            {
                initialDiscountReason += ',';
            }

            initialDiscountReason += discount.initialDiscountReason;
        }

        if (loyaltyHelper.IsEnabled())
        {
            if (loyaltyHelper.loyaltySession.spentPointsValue() != undefined && loyaltyHelper.loyaltySession.spentPointsValue() > 0)
            {
                initialDiscountReason = (discount.initialDiscountReason.length > 0 ? '' : ', ') + 'Loyalty';
            }
        }

        orderDetails.order.pricing.discountType = 'Fixed';
        orderDetails.order.pricing.discountAmount = discountAmount;
        orderDetails.order.pricing.initialDiscountReason = initialDiscountReason;

        // Pass back any payment reference numbers or info
        if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
        {
            orderDetails.paymentData = AndroWeb.Helpers.CartHelper.cart().mercuryPaymentId();
        }
        else if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHLIVE' || viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHTEST')
        {
// KEEP THE ORIGINAL PRICE
            orderDetails.paymentData = AndroWeb.Helpers.CartHelper.cart().dataCashPaymentDetails();
        }
        else if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'PAYPALLIVE' || viewModel.siteDetails().paymentProvider.toUpperCase() == 'PAYPALTEST')
        {
            orderDetails.paymentData = {};
        }

        if (orderDetails.paymentData !== undefined)
        {
            // Card charge (already factored into the final price but we need to make a note of the actual card charge)
            orderDetails.paymentData.paymentCharge = (AndroWeb.Helpers.CartHelper.cart().cardCharge === null || AndroWeb.Helpers.CartHelper.cart().cardCharge === undefined || AndroWeb.Helpers.CartHelper.cart().cardCharge === 'null' ? 0 : AndroWeb.Helpers.CartHelper.cart().cardCharge);
        }

        var orderLineNumber = 0;

        // Deals
        for (var index = 0; index < AndroWeb.Helpers.CartHelper.cart().deals().length; index++)
        {
            var cartDeal = AndroWeb.Helpers.CartHelper.cart().deals()[index];

            if (cartDeal.isEnabled())
            {
                var orderLine =
                {
                    productId: cartDeal.dealWrapper.deal.Id,
                    quantity: 1,
                    price: cartDeal.finalPrice,
                    name: cartDeal.name(),
                    orderLineIndex: orderLineNumber,
                    lineType: 0, // Deal
                    instructions: '',
                    person: '',
                    inDealFlag: false,
                    addToppings:
                    [
                    ],
                    removeToppings:
                    [
                    ]
                };

                // We've generated an order line
                orderDetails.order.orderLines.push(orderLine);

                // Next order line
                orderLineNumber++;

                // Deal lines
                for (var dealLineIndex = 0; dealLineIndex < cartDeal.cartDealLines().length; dealLineIndex++)
                {
                    var cartDealLine = cartDeal.cartDealLines()[dealLineIndex];
                    var cat1 = helper.findById(cartDealLine.cartItem().menuItem.Cat1, viewModel.menu.Category1);
                    var cat2 = helper.findById(cartDealLine.cartItem().menuItem.Cat2, viewModel.menu.Category2);

                    var orderLine =
                    {
                        productId: cartDealLine.cartItem().menuItem.Id,
                        menuSection: 'Deals',
                        cat1: cat1 != undefined ? cat1.Name : '',
                        cat2: cat2 != undefined ? cat2.Name : '',
                        quantity: 1,
                        price: 0,
                        name: cartDealLine.cartItem().name,
                        orderLineIndex: orderLineNumber,
                        lineType: 0, // Normal item (deal line)
                        instructions: cartDealLine.instructions,
                        person: settings.generateTestPerson ? accountHelper.customerDetails.firstname : '', //'Mr Test' : '', //cartDealLine.person,
                        inDealFlag: true,
                        addToppings:
                        [
                        ],
                        removeToppings:
                        [
                        ]
                    };

                    // Add/remove toppings
                    for (var toppingIndex = 0; toppingIndex < cartDealLine.cartItem().displayToppings().length; toppingIndex++)
                    {
                        var topping = cartDealLine.cartItem().displayToppings()[toppingIndex];

                        if (topping.cartQuantity() < 0)
                        {
                            // Default topping has been removed
                            orderLine.removeToppings.push
                            (
                                {
                                    productId: topping.topping.Id,
                                    quantity: 1,
                                    name: topping.topping.ToppingName == undefined ? topping.topping.Name : topping.topping.ToppingName,
                                    price: 0,
                                    itemPrice: topping.itemPrice
                                }
                            );
                        }
                        else if (topping.cartQuantity() > 0)
                        {
                            // Topping added
                            orderLine.addToppings.push
                            (
                                {
                                    productId: topping.topping.Id,
                                    quantity: topping.cartQuantity(),
                                    name: topping.topping.ToppingName == undefined ? topping.topping.Name : topping.topping.ToppingName,
                                    price: 0, //topping.price,
                                    itemPrice: topping.itemPrice
                                }
                            );
                        }
                    }

                    // We've generated an order line
                    orderDetails.order.orderLines.push(orderLine);

                    // Next order line
                    orderLineNumber++;
                }
            }
        }

        // Order lines
        for (var index = 0; index < AndroWeb.Helpers.CartHelper.cart().cartItems().length; index++)
        {
            var cartItem = AndroWeb.Helpers.CartHelper.cart().cartItems()[index];

            if (cartItem.isEnabled())
            {
                var orderLine =
                {
                    productId: cartItem.menuItem.Id == undefined ? cartItem.menuItem.MenuId : cartItem.menuItem.Id,
                    cat1: cartItem.selectedCategory1 != undefined && cartItem.selectedCategory1() != undefined ? cartItem.selectedCategory1().Name : '',
                    cat2: cartItem.selectedCategory2 != undefined && cartItem.selectedCategory2() != undefined ? cartItem.selectedCategory2().Name : '',
                    menuSection: menuHelper.getMenuSection(cartItem.menuItem.Display),
                    quantity: cartItem.quantity(),
                    price: cartItem.price,
                    name: cartItem.name,
                    orderLineIndex: orderLineNumber,
                    lineType: 1, // Normal item
                    instructions: cartItem.instructions,
                    person: settings.generateTestPerson ? accountHelper.customerDetails.firstname : '', // 'Mr Test' : '', //cartItem.person,
                    inDealFlag: false,
                    addToppings:
                    [
                    ],
                    removeToppings:
                    [
                    ]
                };

                // Add/remove toppings
                for (var toppingIndex = 0; toppingIndex < cartItem.displayToppings().length; toppingIndex++)
                {
                    var topping = cartItem.displayToppings()[toppingIndex];

                    if (topping.cartQuantity() < 0)
                    {
                        // Default topping has been removed
                        orderLine.removeToppings.push
                        (
                            {
                                productId: topping.topping.Id,
                                quantity: 1,
                                name: topping.topping.ToppingName == undefined ? topping.topping.Name : topping.topping.ToppingName,
                                price: topping.price,
                                itemPrice: topping.itemPrice
                            }
                        );
                    }
                    else if (topping.cartQuantity() > 0)
                    {
                        // Topping added
                        orderLine.addToppings.push
                        (
                            {
                                productId: topping.topping.Id,
                                quantity: topping.cartQuantity(),
                                name: topping.topping.ToppingName == undefined ? topping.topping.Name : topping.topping.ToppingName,
                                price: topping.price,
                                itemPrice: topping.itemPrice
                            }
                        );
                    }
                }

                // We've generated an order line
                orderDetails.order.orderLines.push(orderLine);

                // Next order line
                orderLineNumber++;
            }
        }

        return orderDetails;
    },
    checkoutSections: ko.observableArray(),
    getTemplateName: function (data)
    {
        return data.templateName;
    },
    nextButton: { displayText: ko.observable(''), action: 'none', isSendOrderButton: ko.observable(false) },
    backButton: { visible: ko.observable(false), displayText: ko.observable(''), action: 'none' },
    displayAddressMultiLine: ko.observable(''),
    nextButtonClicked: function ()
    {
        if (checkoutHelper.nextButton.action != undefined)
        {
            if (checkoutHelper.nextButton.action == 'placeorder')
            {
                // Place order
                checkoutHelper.placeOrder();
            }
            else if (checkoutHelper.nextButton.action == 'confirmorder')
            {
                if (checkoutHelper.validateOrderSummary())
                {
                    // Place order
                    checkoutHelper.placeOrder();
                }
            }
            else
            {
                var section = checkoutHelper.checkoutSections()[Number(checkoutHelper.nextButton.action) - 1];

                // Has the user entered all the required information?
                if (section.validate == undefined || section.validate())
                {
                    // Show a different section
                    checkoutHelper.showCheckoutSection(Number(checkoutHelper.nextButton.action));
                }
            }
        }
    },
    updateMultiLineDeliveryAddress: function()
    {
        // Little bit of a fudge but update the multi line delivery address
        //checkoutHelper.displayAddressMultiLine(addressHelper.generateMultiLineAddress(addressHelper.bindableAddress()));
        checkoutHelper.displayAddressMultiLine(addressHelper.generateMultiLineAddress(checkoutHelper.checkoutDetails.address));
        if (checkoutHelper.displayAddressMultiLine().length == 0)
        {
            checkoutHelper.displayAddressMultiLine(textStrings.checkSNoAddress);
        }
    },
    backButtonClicked: function ()
    {
        if (checkoutHelper.backButton.action != undefined && checkoutHelper.backButton.action != 'none')
        {
            // Show a different section
            checkoutHelper.showCheckoutSection(Number(checkoutHelper.backButton.action));
        }
    },
    errors:
    {
        // Contact details errors
        contactDetailsHasError: ko.observable(false),
        firstNameHasError: ko.observable(false),
        surnameHasError: ko.observable(false),
        telephoneHasError: ko.observable(false),
        emailHasError: ko.observable(false),

        // Delivery time errors
        deliveryTimeHasError: ko.observable(false),
        deliveryTimeError: ko.observable(false),

        // Address errors
        addressHasError: ko.observable(false),
        roadNameHasError: ko.observable(false),
        townHasError: ko.observable(false),
        postcodeHasError: ko.observable(false),
        addressMissingDetails: ko.observable(false),
        outOfDeliveryArea: ko.observable(false),

        // Order summary errors
        payTypeHasError: ko.observable(false),
        orderSummaryHasError: ko.observable(false)
    },
    validateContactDetails: function ()
    {
        checkoutHelper.errors.firstNameHasError(checkoutHelper.checkoutDetails.firstName().length == 0);
        checkoutHelper.errors.surnameHasError(checkoutHelper.checkoutDetails.surname().length == 0);
        checkoutHelper.errors.telephoneHasError(checkoutHelper.checkoutDetails.telephoneNumber().length == 0);

        if (checkoutHelper.checkoutDetails.emailAddress().length == 0)
        {
            checkoutHelper.errors.emailHasError(false);
        }
        else
        {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            checkoutHelper.errors.emailHasError(!regex.test(checkoutHelper.checkoutDetails.emailAddress()));
        }

        // Has the user entered all the required contact details?
        checkoutHelper.errors.contactDetailsHasError
        (
            checkoutHelper.errors.firstNameHasError() ||
            checkoutHelper.errors.surnameHasError() ||
            checkoutHelper.errors.telephoneHasError() ||
            checkoutHelper.errors.emailHasError()
        );

        // Was there an error?
        return !checkoutHelper.errors.contactDetailsHasError();
    },
    validateDeliveryTime: function ()
    {
        // Has the user selected a time?
        checkoutHelper.errors.deliveryTimeError(checkoutHelper.checkoutDetails.wantedTime().mode == undefined);

        // Has the user entered all the delivery time details?
        checkoutHelper.errors.deliveryTimeHasError(checkoutHelper.errors.deliveryTimeError());

        // Was there an error?
        return !checkoutHelper.errors.deliveryTimeHasError();
    },
    validateAddress: function ()
    {
        return addressHelper.validateAddress(addressHelper.bindableAddress(), true);
    },
    validateOrderSummary: function()
    {
        // Has the user selected a pay type?
        checkoutHelper.errors.payTypeHasError(checkoutHelper.checkoutDetails.payNow() == undefined);

        // Has the user entered all the delivery time details?
        checkoutHelper.errors.orderSummaryHasError(checkoutHelper.errors.payTypeHasError());

        // Was there an error?
        return !checkoutHelper.errors.orderSummaryHasError();
    },
    validate: function ()
    {
        for (var index = 0; index < checkoutHelper.checkoutSections().length; index++)
        {
            var section = checkoutHelper.checkoutSections()[Number(index)];

            if (section.validate != undefined && !section.validate())
            {
                if (guiHelper.isMobileMode())
                {
                    var sectionTop = $('#' + section.id).offset().top;
                    $(window).scrollTop(sectionTop);
                }
                else
                {
                    checkoutHelper.showCheckoutSection(Number(section.index));
                }
                return false;
            }
        }

        return true;
    },
    validatePostcode: function (postcode)
    {
        var isInDeliveryZone = false;

        if (viewModel.orderType() == 'collection')
        {
            return true;
        }

        var cleanedUpPostcode = postcode === undefined ? checkoutHelper.checkoutDetails.address.postcode() : postcode;

        if (deliveryZoneHelper.deliveryZones() == undefined || deliveryZoneHelper.deliveryZones().length == 0)
        {
            isInDeliveryZone = true; // No postcodes so we can't validate it
        }
        else
        {
            isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(cleanedUpPostcode);
        }

        if (!isInDeliveryZone)
        {
            viewModel.telemetry.sendTelemetryData('Sales', 'Checkout', 'PostcodeUnknown', cleanedUpPostcode);
        }

        return isInDeliveryZone;
    },
    deliveryTimeChanged: function ()
    {
        // Fix for iPhone - hides the select popup
        $('#deliveryTime').blur();
    },
    modifyOrder: function ()
    {
        // Make sure warning is enabled (might have been disabled during payment if need to be diverted to an external website)
        window.onbeforeunload = self.closeSiteWarning;

        guiHelper.cartActions(guiHelper.cartActionsMenu);

        // Remove any voucher codes
        checkoutHelper.removeVouchers();

        // Allow the user modify the cart items
        guiHelper.isCartLocked(false);

        if (queryStringHelper.orderType == undefined)
        {
            guiHelper.canChangeOrderType(true);
        }

        guiHelper.showMenu();

        setTimeout
        (
            function ()
            {
                guiHelper.resize();
            },
            0
        );
    },
    upsellCategories: ko.observableArray(),
    refreshUpsellCategories: function()
    {
        checkoutHelper.upsellCategories.removeAll();

        var categories = [];
        for (var index = 0; index < AndroWeb.Helpers.CartHelper.cart().cartItems().length; index++)
        {
            var cartItem = AndroWeb.Helpers.CartHelper.cart().cartItems()[index];

            if (categories.indexOf(cartItem.menuItem.Display) == -1)
            {
                categories.push(cartItem.menuItem.Display);
            }
        }

        if (categories.indexOf(229) == -1)
        {
            checkoutHelper.upsellCategories.push({ imageUrl: '', text: 'add some soup?', id: 229 });
        }

        if (categories.indexOf(215) == -1)
        {
            checkoutHelper.upsellCategories.push({ imageUrl: '', text: 'add a snack?', id: 215 });
        }

        if (categories.indexOf(211) == -1)
        {
            checkoutHelper.upsellCategories.push({ imageUrl: '', text: 'add a pudding?', id: 211 });
        }
    },
    upsell: function (category)
    {
        if (category.id == 229)
        {
            checkoutHelper.removeVouchers();
            guiHelper.showMenu(0);
        }

        if (category.id == 215)
        {
            checkoutHelper.removeVouchers();
            guiHelper.showMenu(7);
        }

        if (category.id == 211)
        {
            checkoutHelper.removeVouchers();
            guiHelper.showMenu(6);
        }
    },
    payTypeChanged: function ()
    {
        if (checkoutHelper.nextButton.action == 'confirmorder')
        {
            if (checkoutHelper.checkoutDetails.payNow() != undefined)
            {
                if (checkoutHelper.checkoutDetails.payNow())
                {
                    checkoutHelper.nextButton.displayText(textStrings.payNow);
                }
                else
                {
                    checkoutHelper.nextButton.displayText(textStrings.sendOrderToStore);
                }
            }
        }

        return true;
    },
    checkVoucherCode: ko.observable(''),
    voucherError: ko.observable(undefined),
    addVoucherCode: function ()
    {
        // Copy the voucher codes already added
        var checkVoucherCodes = checkoutHelper.checkoutDetails.voucherCodes().slice(0);

        // Add the voucher code to check
        checkVoucherCodes.push({ "voucherCode": checkoutHelper.checkVoucherCode() });

        // Re-check all voucher codes add apply them to the order if they are valid or remove any that aren't valid
        checkoutHelper.checkVouchers(checkVoucherCodes);
    },
    recheckVouchers: function ()
    {
        // Re-check all existing voucher codes
        checkoutHelper.checkVouchers(checkoutHelper.checkoutDetails.voucherCodes());
    },
    checkVouchers: function (checkVoucherCodes, returnToPage)
    {
        // Does the order contain any vouchers
        if (checkVoucherCodes === undefined || checkVoucherCodes.length === 0) return;

        // Generate the order JSON
        var orderDetails = checkoutHelper.generateOrderJson(checkVoucherCodes);

        // Let the user know we're waiting for the server
        viewModel.pleaseWaitMessage(textStrings.sovCheckingVoucherCode);
        viewModel.pleaseWaitProgress('');

        var pleaseWaitViewModel = new AndroWeb.ViewModels.PleaseWaitViewModel
        (
            function ()
            {
                acsapi.checkOrderVouchers
                (
                    viewModel.selectedSite().siteId,
                    orderDetails.order,
                    function (data)
                    {
                        try
                        {
                            // Show the checkout view using the existing checkout view model
                            var returnToPage = 'Checkout';
                            var currentPage = viewModel.pageManager.currentPage;

                            if (currentPage === 'MENU')
                            {
                                var section = viewModel.sections()[viewModel.selectedSection().Index];
                                returnToPage = 'Menu/' + section.display.Name;
                            }

                            viewModel.pageManager.showPage(returnToPage, true);

                            // Allow JavaScript to process events (kind of like doEvents)
                            setTimeout
                            (
                                function ()
                                {
                                    try
                                    {
                                        // Did that work?
                                        if (data == undefined || data.hasError == undefined)
                                        {
                                            checkoutHelper.voucherError(textStrings.errProblemVoucherErrorGeneral);
                                        }
                                        else if (data.hasError)
                                        {
                                            checkoutHelper.voucherError(textStrings.errProblemVoucherErrorGeneral);
                                        }
                                        else
                                        {
                                            // Make sure there's only one voucher (we may support multiple vouchers in future)
                                            if (data.data.length == 1)
                                            {
                                                var voucher = data.data[0];
                                                if (voucher != undefined)
                                                {
                                                    if (voucher.VoucherCode.trim().length == 0)
                                                    {
                                                        checkoutHelper.voucherError(textStrings.checkSIsBlankVoucher);
                                                    }
                                                    else if (voucher.IsValid)
                                                    {
                                                        // Add a discount property to each voucher which will be used to display the discount applied to the order
                                                        voucher.discount = ko.observable('');

                                                        // Keep hold of the vouchers
                                                        checkoutHelper.checkoutDetails.voucherCodes(data.data);
                                                        AndroWeb.Helpers.CartHelper.cart().vouchers(data.data);

                                                        // No error to display!
                                                        checkoutHelper.voucherError('');

                                                        // Update the price in the cart
                                                        AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());
                                                    }
                                                    else
                                                    {
                                                        // Make sure the vouchers are not being applied to the order
                                                        // NOTE we're removing all vouchers - if we ever support multiple vouchers we'll
                                                        // need to modify this code to remove only the invalid vouchers
                                                        checkoutHelper.checkoutDetails.voucherCodes([]);
                                                        AndroWeb.Helpers.CartHelper.cart().vouchers([]);

                                                        if (voucher.ErrorCode == undefined)
                                                        {
                                                            // No error code.  Shouldn't be possible but show a default error message anyway
                                                            checkoutHelper.voucherError(textStrings.checkSInvalidVoucher);
                                                        }
                                                        else
                                                        {
                                                            // Get an error message to show the customer
                                                            checkoutHelper.voucherError(checkoutHelper.getVoucherErrorMessage(voucher.ErrorCode));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // Shouldn't be possible but show a default error message anyway
                                                    checkoutHelper.voucherError(textStrings.checkSInvalidVoucher);
                                                }
                                            }
                                            else
                                            {
                                                // Shouldn't be possible but show a default error message anyway
                                                checkoutHelper.voucherError(textStrings.checkSInvalidVoucher);
                                            }
                                        }
                                    }
                                    catch (exception)
                                    {
                                        // Got an error
                                        viewHelper.showError(textStrings.errProblemGettingVoucherDetails, checkoutHelper.getVoucherErrorCallback, exception);
                                    }
                                },
                                0
                            );
                        }
                        catch (exception)
                        {
                            // Got an error
                            viewHelper.showError(textStrings.errProblemGettingVoucherDetails, checkoutHelper.getVoucherErrorCallback, exception);
                        }
                    },
                    function (data)
                    {
                        // Bad HTTP error
                        viewHelper.showError(textStrings.errProblemGettingVoucherDetails, checkoutHelper.getVoucherErrorCallback, undefined);
                    }
                )
            }
        );

        viewModel.pageManager.showPage('PleaseWait', false, pleaseWaitViewModel, false);
    },
    getVoucherErrorCallback: function()
    {
        // Show the checkout view using the existing checkout view model
        viewModel.pageManager.showPage('Checkout', true, undefined, true);
    },
    getVoucherErrorMessage: function(errorCode)
    {
        switch (errorCode)
        {
            case 3003:
                // Not valid for occasion
                if (viewModel.orderType() == 'collection')
                {
                    return textStrings.checkSVoucherNotForCollection;
                }
                else
                {
                    return textStrings.checkSVoucherNotForDelivery;
                }
                break;

            case 3004:
                // Minimum order amount not met
                return textStrings.checkSVoucherMinOrderAmountNotMet;

                break;

            case 3005:
                // Not valid on this date
                return textStrings.checkSVoucherNotValidOnThisDate;

                break;

            case 3006:
                // Not valid on this day of the week
                return textStrings.checkSVoucherNotValidOnThisDayOfTheWeek;

                break;

            case 3007:
                // Not valid for this time of the day
                return textStrings.checkSVoucherNotValidOnThisTimeOfTheDay;

                break;

            case 3008:
                // Customer has used this voucher too many times
                return textStrings.checkSVoucherUsedTooManyTimesForCustomer;

                break;

            default:
                return textStrings.checkSInvalidVoucher;
                break;
        }
    },
    removeVouchers: function()
    {
        // Remove vouchers
        AndroWeb.Helpers.CartHelper.cart().vouchers([]);
        checkoutHelper.checkoutDetails.voucherCodes([]);

        // Clear errors
        checkoutHelper.voucherError('');

        // Make sure the cart price doesn't include any vouchers
        AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart());
    },
    changeAddress: function()
    {
        // Copy the current address to the address helper
        var address = new Address();
        address.fromAddressObject(checkoutHelper.checkoutDetails.address);
        addressHelper.bindableAddress(address);
        addressHelper.bindableAddress().requiresValidation(true);

        // By default, don't update the user profile with the new delivery address
        checkoutHelper.checkoutDetails.rememberAddress = false;

        // By default don't show the checkbox to update the user profile with the new delivery address
        checkoutHelper.showUpdateCustomerAccount(false);

        if (settings.checkoutUpdateCustomerAccount)
        {
            if (checkoutHelper.isAddressMissing())
            {
                // No address - the delivery address will become the customers my profile address
                checkoutHelper.checkoutDetails.rememberAddress = true;
            }
            else
            {
                // Customer already has an address - allow the customer to change it to the new one
                checkoutHelper.showUpdateCustomerAccount(true);
            }
        }

        popupHelper.showPopup
        (
            'customerAddressView',
            new AddressViewModel(),
            function () { }
        );
    },
    useAddress: function()
    {
        if (addressHelper.bindableAddress().errors().length > 0)
        {
            return;  // Error in addresss
        }

        // Copy the address helper address details into the checkout address
        checkoutHelper.checkoutDetails.address.fromAddressObject(addressHelper.bindableAddress());

        // IMPORTANT the validateAddress function adds a space to the postcode
        addressHelper.validateAddress(checkoutHelper.checkoutDetails.address);
        // Copy the postcode back as we actually save the bindable address
        addressHelper.bindableAddress().postcode(checkoutHelper.checkoutDetails.address.postcode());

        // Refresh the display address
        checkoutHelper.updateMultiLineDeliveryAddress();

        // Do we need to save the new address back to the users profile?
        if (checkoutHelper.checkoutDetails.rememberAddress)
        {
            accountHelper.customerDetails.address = new Address();
            accountHelper.customerDetails.address.fromAddressObject(checkoutHelper.checkoutDetails.address);

            popupHelper.hidePopup();

            setTimeout
            (
                function ()
                {
                    // Save the users customer account to the server
                    guiHelper.showPleaseWait
                    (
                        textStrings.mpUpdatingUserProfile,
                        '',
                        function (pleaseWaitViewModel)
                        {
                            // Make sure the bindable customer contains the currently logged in users details
                            accountHelper.initialiseBindableCustomer();

                            // Save the address to the server
                            accountHelper.save
                            (
                                function ()
                                {
                                    viewModel.pageManager.showPage("Checkout", true);
                                }
                            );
                        }
                    );
                }, 0
            );
        }
        else
        {
            // Hide the delivery address and keep the address
            viewModel.pageManager.showPage("Checkout", true, true);
        }
    },
    cancelChangeAddress: function()
    {
        popupHelper.hidePopup();
        viewModel.pageManager.showPage("Checkout", true);
    },
    onMessageEvent: function(message)
    {

    },
    shareOnFacebook: function()
    {
        FB.ui(
            {
                method: 'feed',
                name: '',
                link: location.origin,
                picture: settings.companyLogoUrl,
                caption: settings.facebookShareCaption,
                description: settings.facebookShareDescription,
                message: ''
            });
    }
}