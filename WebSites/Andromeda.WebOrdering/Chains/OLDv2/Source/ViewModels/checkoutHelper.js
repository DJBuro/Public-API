// Checkout
var checkoutHelper =
{
    clearCheckout: function ()
    {
        checkoutHelper.checkoutDetails.firstName('');
        checkoutHelper.checkoutDetails.surname('');
        checkoutHelper.checkoutDetails.telephoneNumber('');
        checkoutHelper.checkoutDetails.emailAddress('');
        checkoutHelper.checkoutDetails.deliveryTime('');
        checkoutHelper.checkoutDetails.payNow = false;
        checkoutHelper.checkoutDetails.address.prem1('');
        checkoutHelper.checkoutDetails.address.prem2('');
        checkoutHelper.checkoutDetails.address.prem3('');
        checkoutHelper.checkoutDetails.address.prem4('');
        checkoutHelper.checkoutDetails.address.prem5('');
        checkoutHelper.checkoutDetails.address.prem6('');
        checkoutHelper.checkoutDetails.address.org1('');
        checkoutHelper.checkoutDetails.address.org2('');
        checkoutHelper.checkoutDetails.address.org3('');
        checkoutHelper.checkoutDetails.address.roadNumber('');
        checkoutHelper.checkoutDetails.address.roadName('');
        checkoutHelper.checkoutDetails.address.city('');
        checkoutHelper.checkoutDetails.address.town('');
        checkoutHelper.checkoutDetails.address.zipCode('');
        checkoutHelper.checkoutDetails.address.county('');
        checkoutHelper.checkoutDetails.address.state('');
        checkoutHelper.checkoutDetails.address.locality('');
        checkoutHelper.checkoutDetails.address.country('');
        checkoutHelper.checkoutDetails.address.directions('');
        checkoutHelper.checkoutDetails.address.userLocality1('');
        checkoutHelper.checkoutDetails.address.userLocality2('');
        checkoutHelper.checkoutDetails.address.userLocality3('');
        checkoutHelper.checkoutDetails.orderNotes('');
        checkoutHelper.checkoutDetails.chefNotes('');
    },
    initialiseCheckout: function ()
    {
        checkoutHelper.nextButton.displayText(textStrings.next);
        checkoutHelper.backButton.displayText(textStrings.back);

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
                checkoutHelper.checkoutDetails.address.prem1(accountHelper.customerDetails.address.prem1);
                checkoutHelper.checkoutDetails.address.prem2(accountHelper.customerDetails.address.prem2);
                checkoutHelper.checkoutDetails.address.prem3(accountHelper.customerDetails.address.prem3);
                checkoutHelper.checkoutDetails.address.prem4(accountHelper.customerDetails.address.prem4);
                checkoutHelper.checkoutDetails.address.prem5(accountHelper.customerDetails.address.prem5);
                checkoutHelper.checkoutDetails.address.prem6(accountHelper.customerDetails.address.prem6);
                checkoutHelper.checkoutDetails.address.org1(accountHelper.customerDetails.address.org1);
                checkoutHelper.checkoutDetails.address.org2(accountHelper.customerDetails.address.org2);
                checkoutHelper.checkoutDetails.address.org3(accountHelper.customerDetails.address.org3);
                checkoutHelper.checkoutDetails.address.roadNumber(accountHelper.customerDetails.address.roadNum);
                checkoutHelper.checkoutDetails.address.roadName(accountHelper.customerDetails.address.roadName);
                checkoutHelper.checkoutDetails.address.city(accountHelper.customerDetails.address.city);
                checkoutHelper.checkoutDetails.address.town(accountHelper.customerDetails.address.town);
                checkoutHelper.checkoutDetails.address.zipCode(accountHelper.customerDetails.address.postcode);
                checkoutHelper.checkoutDetails.address.county(accountHelper.customerDetails.address.county);
                checkoutHelper.checkoutDetails.address.state(accountHelper.customerDetails.address.state);
                checkoutHelper.checkoutDetails.address.locality(accountHelper.customerDetails.address.locality);
                checkoutHelper.checkoutDetails.address.directions(accountHelper.customerDetails.address.directions);
                checkoutHelper.checkoutDetails.address.userLocality1(accountHelper.customerDetails.address.userLocality1);
                checkoutHelper.checkoutDetails.address.userLocality2(accountHelper.customerDetails.address.userLocality2);
                checkoutHelper.checkoutDetails.address.userLocality3(accountHelper.customerDetails.address.userLocality3);
            }
        }
        else
        {
            checkoutHelper.checkoutDetails.firstName(queryStringHelper.firstName == undefined ? '' : queryStringHelper.firstName);
            checkoutHelper.checkoutDetails.surname(queryStringHelper.lastName == undefined ? '' : queryStringHelper.lastName);
            checkoutHelper.checkoutDetails.emailAddress(queryStringHelper.email == undefined ? '' : queryStringHelper.email);
            checkoutHelper.checkoutDetails.telephoneNumber(queryStringHelper.telephoneNumber == undefined ? '' : queryStringHelper.telephoneNumber);
            checkoutHelper.checkoutDetails.marketing(queryStringHelper.marketing == undefined ? '' : queryStringHelper.marketing);
            checkoutHelper.checkoutDetails.address.town(queryStringHelper.town == undefined ? '' : queryStringHelper.town);
            checkoutHelper.checkoutDetails.address.city(queryStringHelper.town == undefined ? '' : queryStringHelper.town);
            checkoutHelper.checkoutDetails.address.zipCode(queryStringHelper.postcode == undefined ? '' : queryStringHelper.postcode);
            checkoutHelper.checkoutDetails.address.roadNumber(queryStringHelper.houseNumber == undefined ? '' : queryStringHelper.houseNumber);
            checkoutHelper.checkoutDetails.address.roadName(queryStringHelper.roadName == undefined ? '' : queryStringHelper.roadName);
        }

        // Use the store country
        // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
        // profile cannot be sent in the order JSON.  There is a hard coded country code for that.  We use proper ISO codes for the customer profile.
        checkoutHelper.checkoutDetails.address.country(viewModel.siteDetails().address.country);

        // Clear the checkout sections (tabs)
        checkoutHelper.checkoutSections.removeAll();

        // The page to add
        var index = 0;

        // Who are you
        if (settings.displayCustomerDetailsPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutContactDetailsContainer', index: index, templateName: 'checkoutContactDetails-template', displayName: (index + 1) + ') ' + textStrings.whoAreYou, validate: checkoutHelper.validateContactDetails });
            index++;
        }

        // Collection/delivery time
        if (viewModel.orderType() == 'collection')
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', index: index, templateName: 'checkoutDeliveryTime-template', displayName: (index + 1) + ') ' + textStrings.collectionTime, validate: checkoutHelper.validateDeliveryTime });
            index++;
        }
        else
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', index: index, templateName: 'checkoutDeliveryTime-template', displayName: (index + 1) + ') ' + textStrings.deliveryTime, validate: checkoutHelper.validateDeliveryTime });
            index++;
        }

        // Delivery address
        if (viewModel.orderType() == 'delivery' && settings.displayCustomerAddressPage)
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryAddressContainer', index: index, templateName: 'checkoutDeliveryAddress-template', displayName: (index + 1) + ') ' + textStrings.deliveryAddress, validate: checkoutHelper.validateAddress });
            index++;
        }

        // Order notes
        checkoutHelper.checkoutSections.push({ id: 'checkoutOrderNotesContainer', index: index, templateName: 'checkoutOrderNotes-template', displayName: (index + 1) + ') ' + textStrings.orderNotes, validate: function () { return true; } });
        index++;

        // Show the first section
        checkoutHelper.showCheckoutSection(0);
    },
    showCheckoutSection: function (sectionIndex)
    {
        // Configure back button
        if (sectionIndex == 0)
        {
            checkoutHelper.backButton.visible(false);
            checkoutHelper.backButton.action = 'none';
        }
        else
        {
            checkoutHelper.backButton.visible(true);
            checkoutHelper.backButton.action = sectionIndex - 1;
        }

        // Configure next button
        if (sectionIndex == checkoutHelper.checkoutSections().length - 1)
        {
            checkoutHelper.nextButton.displayText(textStrings.placeOrder);
            checkoutHelper.nextButton.action = 'placeorder';
        }
        else
        {
            checkoutHelper.nextButton.displayText(textStrings.next);
            checkoutHelper.nextButton.action = sectionIndex + 1;
        }

        for (var index = 0; index < checkoutHelper.checkoutSections().length; index++)
        {
            var checkoutSection = checkoutHelper.checkoutSections()[index];

            if (checkoutSection.index == sectionIndex || guiHelper.isMobileMode())
            {
                // This section should be visible
                $('#' + checkoutSection.id).css('display', 'block');
            }
            else
            {
                // Hide this section
                $('#' + checkoutSection.id).css('display', 'none');
            }
        }

        // Used to highlight the correct section
        checkoutHelper.visibleCheckoutSection(sectionIndex);

        // Don't let the user modify the cart items
        guiHelper.isCartLocked(true);
    },
    refreshTimes: function ()
    {
        checkoutHelper.times.removeAll();
        checkoutHelper.times.push({ mode: undefined, time: undefined, text: textStrings.pleaseSelectATime });

        // Is the store open for ASAP?
        //if (viewModel.selectedSite().estDelivTime == undefined || viewModel.selectedSite().estDelivTime == 0)
        //{
        //    checkoutHelper.times.push({ mode: 'ASAP', time: undefined, text: textStrings.asSoonAsPossibleNoETD });
        //}
        //else
        //{
        //    checkoutHelper.times.push({ mode: 'ASAP', time: undefined, text: textStrings.asSoonAsPossible + ' (' + viewModel.selectedSite().estDelivTime + ')' });
        //}

        // Get todays opening times
        var openingTimes = openingTimesHelper.getTodaysOpeningTimes();

        var timeBlocks = [];
        var today = new Date();

        // The first available slot has to be at least EDT minutes from now.
        // If EDT is not available default to settings.defaultETD minutes.
        var offset = settings.defaultETD;
        if (viewModel.selectedSite().estDelivTime != undefined && viewModel.selectedSite().estDelivTime > 0)
        {
            offset = viewModel.selectedSite().estDelivTime;
        }

        // The earliest time that an order can be delivered/collected
        var today = new Date(new Date().getTime() + offset * 60000);

        var hourNow = today.getHours();
        var minuteNow = today.getMinutes();

        // Go through todays delivery times and get the delivery times after now
        for (var timespanIndex = 0; timespanIndex < openingTimes.length; timespanIndex++)
        {
            var timeSpan = openingTimes[timespanIndex];

            if (timeSpan.openAllDay)
            {
                // Store is open all day so delivery times are from now to midnight
                timeBlocks.push({ startHour: hourNow, startMinute: minuteNow, endHour: 0, endMinute: 0 });
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
                else if (endHour > hourNow || (endHour == hourNow && endMinute > minuteNow))
                {
                    // Time block has already started but not ended
                    timeBlocks.push({ startHour: hourNow, startMinute: minuteNow, endHour: endHour, endMinute: endMinute });
                }
            }
        }

        var slotSize = 15;

        // Generate delivery slots for each block of time the store is open
        for (var timeBlockIndex = 0; timeBlockIndex < timeBlocks.length; timeBlockIndex++)
        {
            var timeSpan = timeBlocks[timeBlockIndex];

            // Add slots in start hour
            checkoutHelper.addDeliveryHourSlots(timeSpan.startHour, timeSpan.startMinute, timeSpan.endHour == timeSpan.startHour ? timeSpan.endMinute : 60, slotSize);

            // Add slots for the each hour inbetween the start and end hours
            var endHour = timeSpan.endHour == 0 ? 24 : timeSpan.endHour;
            for (var hour = timeSpan.startHour + 1; hour < endHour; hour++)
            {
                checkoutHelper.addDeliveryHourSlots(hour, 0, 60, slotSize);
            }

            if (timeSpan.endHour > timeSpan.startHour)
            {
                // Add slots for the last hour
                checkoutHelper.addDeliveryHourSlots(timeSpan.endHour, 0, timeSpan.endMinute, slotSize);
            }
        }
    },
    addDeliveryHourSlots: function (hour, startMinute, endMinute, slotSize)
    {
        // Get the first and last slots for this hour
        var startSlot = Math.ceil(startMinute / slotSize) * slotSize;
        var endSlot = Math.ceil(endMinute / slotSize) * slotSize;

        // Get all the slots between the start and end minutes
        for (var slot = startSlot; slot < endSlot; slot += slotSize)
        {
            checkoutHelper.addDeliverySlot(hour, slot, slotSize);
        }
    },
    addDeliverySlot: function (hour, minute, slotSize)
    {
        var hour12HourClock = hour > 12 ? hour - 12 : hour;
        var hourAMPM = hour > 12 ? 'pm' : 'am';
        var hourPlusOne = (hour + 1) > 23 ? 0 : (hour + 1);
        var hourPlusOne12HourClock = hourPlusOne > 12 ? hourPlusOne - 12 : hourPlusOne;
        var hourPlusOneAMPM = hourPlusOne > 12 ? 'pm' : 'am';

        if (minute + slotSize > 59)
        {
            // Slot overlaps into the next hour
            checkoutHelper.times.push
            (
                {
                    mode: 'TIMED',
                    time: hour + ':' + minute,
                    text: checkoutHelper.formatHour(helper.use24hourClock ? hour : hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + (helper.use24hourClock ? '' : hourAMPM) + ' - ' +
                          checkoutHelper.formatHour(helper.use24hourClock ? hour : hourPlusOne12HourClock) + ':' + checkoutHelper.formatMinute((slotSize - (60 - minute))) + (helper.use24hourClock ? '' : hourPlusOneAMPM)
                }
            );
        }
        else
        {
            // Slot is entirely within the hour
            checkoutHelper.times.push
            (
                {
                    mode: 'TIMED',
                    time: hour + ':' + minute,
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
    visibleCheckoutSection: ko.observable(0),
    times: ko.observableArray(),
    checkoutDetails:
    {
        firstName: ko.observable(''),
        surname: ko.observable(''),
        telephoneNumber: ko.observable(''),
        emailAddress: ko.observable(''),
        deliveryTime: ko.observable(undefined),
        address:
        {
            prem1: ko.observable(''),
            prem2: ko.observable(''),
            prem3: ko.observable(''),
            prem4: ko.observable(''),
            prem5: ko.observable(''),
            prem6: ko.observable(''),
            org1: ko.observable(''),
            org2: ko.observable(''),
            org3: ko.observable(''),
            roadNumber: ko.observable(''),
            roadName: ko.observable(''),
            city: ko.observable(''),
            town: ko.observable(''),
            zipCode: ko.observable(''),
            county: ko.observable(''),
            state: ko.observable(''),
            locality: ko.observable(''),
            country: ko.observable(''),
            directions: ko.observable(''),
            userLocality1: ko.observable(''),
            userLocality2: ko.observable(''),
            userLocality3: ko.observable('')
        },
        payNow: false,
        marketing: ko.observable('true'),
        wantedTime: ko.observable(undefined),
        orderNotes: ko.observable(''),
        chefNotes: ko.observable(''),
        rememberAddress: true,
        rememberContactDetails: true
    },
    placeOrder: function ()
    {
        // Has the user entered the required information?
        if (checkoutHelper.validate())
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
                    roadNum: checkoutHelper.checkoutDetails.address.roadNumber(),
                    roadName: checkoutHelper.checkoutDetails.address.roadName(),
                    city: checkoutHelper.checkoutDetails.address.city(),
                    town: checkoutHelper.checkoutDetails.address.town(),
                    postcode: checkoutHelper.checkoutDetails.address.zipCode(),
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
                    accountHelper.customerDetails,
                    checkoutHelper.showPaymentPicker
                );
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
        if (guiHelper.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('checkout', false);

            guiHelper.showView('paymentView');
        }
        else
        {
            guiHelper.showMenuView('paymentView');
        }

        // Switch the cart to checkout mode
        guiHelper.cartActions(guiHelper.cartActionsCheckout);

        if (settings.termsAndConditionsEnabled)
        {
            // Reset the agree flag - assume they haven't agreed yet
            tandcHelper.agree(undefined);
        }
        else
        {
            // There are no terms and conditions to agree to
            tandcHelper.agree(true);
        }

        guiHelper.canChangeOrderType(false);
    },
    payAtDoor: function ()
    {
        // Must agree to T&C before proceeding
        if (!tandcHelper.hasAgreed()) return;

        // Hide the mobile checkout menu
        checkoutMenuHelper.hideMenu();

        // Customer wants to pay now
        checkoutHelper.checkoutDetails.payNow = false;

        checkoutHelper.sendOrderToStore();
    },
    payNow: function ()
    {
        // Must agree to T&C before proceeding
        if (!tandcHelper.hasAgreed()) return;

        guiHelper.canChangeOrderType(false);

        // Customer wants to pay now
        checkoutHelper.checkoutDetails.payNow = true;

        viewModel.pleaseWaitMessage(textStrings.preparingForPayment);
        viewModel.pleaseWaitProgress('');
        guiHelper.showView('pleaseWaitView');

        if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
        {
            // Initialise the mercury payment
            acsapi.putMercuryPayment
            (
                viewModel.selectedSite().siteId,
                function ()
                {
                    if (guiHelper.isMobileMode())
                    {
                        guiHelper.isMobileMenuVisible(false);
                        checkoutMenuHelper.showMenu('pay', false);
                        guiHelper.showView('mercuryPaymentView');
                    }
                    else
                    {
                        guiHelper.showMenuView('mercuryPaymentView');
                    }

                    var iFrame = $('#mercuryPaymentIFrame');
                    iFrame.attr('src', 'https://hc.mercurydev.net/CheckoutIFrame.aspx?pid=' + cartHelper.cart().mercuryPaymentId());

                    iFrame.load
                    (
                        function ()
                        {
                            if (iFrame.contents().get(0) != undefined)
                            {
                                if (iFrame.contents().get(0).location.href)
                                {
                                    checkoutHelper.sendOrderToStore();
                                }
                            }
                        }
                    );
                }
            );
        }
        else if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHLIVE' ||
            viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHTEST')
        {
            // Initialise the DataCash payment
            acsapi.putDataCashPayment
            (
                viewModel.selectedSite().siteId,
                function ()
                {
                    if (guiHelper.isMobileMode())
                    {
                        guiHelper.isMobileMenuVisible(false);
                        checkoutMenuHelper.showMenu('pay', false);
                        guiHelper.showView('dataCashPaymentView');
                    }
                    else
                    {
                        guiHelper.showMenuView('dataCashPaymentView');
                    }

                    var iFrame = $('#dataCashPaymentIFrame');
                    iFrame.attr('src', cartHelper.cart().dataCashPaymentDetails().url + '?HPS_SessionID=' + cartHelper.cart().dataCashPaymentDetails().sessionId);

                    iFrame.load
                    (
                        function ()
                        {
                            if (iFrame.contents().get(0) != undefined)
                            {
                                if (iFrame.contents().get(0).location.href)
                                {
                                    checkoutHelper.sendOrderToStore();
                                }
                            }
                        }
                    );
                }
            );
        }
        else if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCANETLIVE' ||
        viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCANETTEST')
        {
            // Generate the order JSON
            var orderDetails = checkoutHelper.generateOrderJson();

            // Initialise the Mercanet payment
            acsapi.putMercanetPayment
            (
                viewModel.selectedSite().siteId,
                orderDetails,
                function (json)
                {
                    var jsonObject = jQuery.parseJSON(json);
                    var html = jsonObject.Html;
                    var orderRef = jsonObject.OrderReference;

                    // We need to save the order reference in the cookie as we're about to get redirected to another webpage and we'll lose state
                    cookieHelper.setCookie('or', jsonObject.OrderReference);

                    if (guiHelper.isMobileMode())
                    {
                        guiHelper.isMobileMenuVisible(false);
                        checkoutMenuHelper.showMenu('pay', false);
                        guiHelper.showView('mercanetPaymentView');
                    }
                    else
                    {
                        guiHelper.showMenuView('mercanetPaymentView');
                    }

                    // Inject the Mercanet HTML into the iFrame
                    var iFrame = $('#mercanetPaymentIFrame');

                    if (html == undefined || html.length == 0)
                    {
                        html = "Sorry there was problem.  Payment is unavailable at this time.";
                    }

                    var iFrameDoc = iFrame[0].contentDocument || iFrame[0].contentWindow.document;
                    iFrameDoc.write(html);
                    iFrameDoc.close();
                }
            );
        }
        else
        {
            // There is a problem with the payment provider - not supported
            guiHelper.showHome();
        }
    },
    sendOrderToStore: function ()
    {
        // Hide the mobile checkout menu
        checkoutMenuHelper.hideMenu();

        viewModel.pleaseWaitMessage(textStrings.sendingOrderToStore);
        viewModel.pleaseWaitProgress('');
        guiHelper.showView('pleaseWaitView');

        // Generate the order JSON
        var orderDetails = checkoutHelper.generateOrderJson();

        // Send the order to ACS
        acsapi.putOrder(viewModel.selectedSite().siteId, orderDetails);
    },
    generateOrderJson: function ()
    {
        // Work out how long the customer took to place the order
        var timeNow = new Date();
        var timeTakenMilliseconds = Math.abs(timeNow - viewModel.timer);
        var timeTakenSeconds = Math.round(timeTakenMilliseconds / 1000);

        var orderDetails =
        {
            toSiteId: viewModel.selectedSite().siteId,
            paymentType: checkoutHelper.checkoutDetails.payNow ? viewModel.siteDetails().paymentProvider : 'PayLater',
            paymentData: undefined,
            order:
            {
                partnerReference: '',
                type: viewModel.orderType(),
                orderTimeType: checkoutHelper.checkoutDetails.wantedTime().mode,
                orderWantedTime: checkoutHelper.checkoutDetails.wantedTime().mode == 'ASAP' ? helper.formatUTCDate(new Date()) : helper.formatUTCSlot(checkoutHelper.checkoutDetails.wantedTime().time),
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
                    address:
                    {
                        prem1: textStrings.prem1Prefix + checkoutHelper.checkoutDetails.address.prem1(),
                        prem2: textStrings.prem2Prefix + checkoutHelper.checkoutDetails.address.prem2(),
                        prem3: textStrings.prem3Prefix + checkoutHelper.checkoutDetails.address.prem3(),
                        prem4: checkoutHelper.checkoutDetails.address.prem4(),
                        prem5: checkoutHelper.checkoutDetails.address.prem5(),
                        prem6: checkoutHelper.checkoutDetails.address.prem6(),
                        org1: checkoutHelper.checkoutDetails.address.org1(),
                        org2: checkoutHelper.checkoutDetails.address.org2(),
                        org3: checkoutHelper.checkoutDetails.address.org3(),
                        roadNumber: checkoutHelper.checkoutDetails.address.roadNumber(),
                        roadName: checkoutHelper.checkoutDetails.address.roadName(),
                        city: checkoutHelper.checkoutDetails.address.city(),
                        town: checkoutHelper.checkoutDetails.address.town(),
                        zipCode: checkoutHelper.checkoutDetails.address.zipCode(),
                        county: checkoutHelper.checkoutDetails.address.county(),
                        state: checkoutHelper.checkoutDetails.address.state(),
                        locality: checkoutHelper.checkoutDetails.address.locality(),
                        directions: checkoutHelper.checkoutDetails.address.directions(),
                        userLocality1: textStrings.userLocality1Prefix + checkoutHelper.checkoutDetails.address.userLocality1(),
                        userLocality2: textStrings.userLocality2Prefix + checkoutHelper.checkoutDetails.address.userLocality2(),
                        userLocality3: textStrings.userLocality3Prefix + checkoutHelper.checkoutDetails.address.userLocality3(),
                        // IMPORTANT - ACS uses ISO standard country codes whereas Rameses doesn't (don't ask).  This means that the country name saved in the users
                        // profile or the stores country cannot be sent in the order JSON.  There is a hard coded country code for that.
                        country: settings.customerAddressCountryCode
                    },
                    accountNumber: accountHelper.customerDetails == undefined ? '' : accountHelper.customerDetails.accountNumber
                },
                pricing:
                {
                    priceBeforeDiscount: cartHelper.cart().totalPrice(),
                    pricesIncludeTax: 'false',
                    priceAfterDiscount: cartHelper.cart().totalPrice()
                },
                orderLines:
                [
                ],
                orderPayments:
                [
                ]
            }
        };

        // Was a full order discount applied?
        if (cartHelper.cart().discountAmount() != undefined &&
            cartHelper.cart().discountAmount() > 0 &&
            menuHelper.fullOrderDiscountDeal != undefined)
        {
            // Make sure price before discount doesn't include the full order discount (the total price already includes the discount)
            orderDetails.order.pricing.priceBeforeDiscount = cartHelper.cart().subTotalPrice();

            // Add the full order discount details
            orderDetails.order.pricing.discountType = menuHelper.fullOrderDiscountDeal.FullOrderDiscountType;
            if (viewModel.orderType() == 'collection')
            {
                orderDetails.order.pricing.discountTypeAmount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount;
            }
            else
            {
                orderDetails.order.pricing.discountTypeAmount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount;
            }
            orderDetails.order.pricing.discountAmount = cartHelper.cart().discountAmount();
            orderDetails.order.pricing.initialDiscountReason = cartHelper.cart().discountName();
        }

        // Pass back any payment reference numbers or info
        if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
        {
            orderDetails.paymentData = cartHelper.cart().mercuryPaymentId();
        }
        else if (viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHLIVE' || viewModel.siteDetails().paymentProvider.toUpperCase() == 'DATACASHTEST')
        {
            orderDetails.paymentData = cartHelper.cart().dataCashPaymentDetails();
        }

        var orderLineNumber = 0;

        // Deals
        for (var index = 0; index < cartHelper.cart().deals().length; index++)
        {
            var deal = cartHelper.cart().deals()[index];

            if (deal.isEnabled())
            {
                var orderLine =
                {
                    productId: deal.deal.deal.Id,
                    quantity: 1,
                    price: deal.price,
                    name: deal.name,
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
                for (var dealLineIndex = 0; dealLineIndex < deal.dealLines().length; dealLineIndex++)
                {
                    var dealLine = deal.dealLines()[dealLineIndex];

                    var orderLine =
                    {
                        productId: dealLine.selectedMenuItem.Id,
                        quantity: 1,
                        price: dealLine.price,
                        name: dealLine.name,
                        orderLineIndex: orderLineNumber,
                        lineType: 0, // Normal item (deal line)
                        instructions: dealLine.instructions,
                        person: dealLine.person,
                        inDealFlag: true,
                        addToppings:
                        [
                        ],
                        removeToppings:
                        [
                        ]
                    };

                    // Add/remove toppings
                    for (var toppingIndex = 0; toppingIndex < dealLine.displayToppings().length; toppingIndex++)
                    {
                        var topping = dealLine.displayToppings()[toppingIndex];

                        if (topping.cartQuantity() < 0)
                        {
                            // Default topping has been removed
                            orderLine.removeToppings.push
                            (
                                {
                                    productId: topping.topping.Id,
                                    quantity: 1,
                                    name: topping.topping.ToppingName,
                                    price: 0
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
                                    name: topping.topping.ToppingName,
                                    price: menuHelper.getToppingPrice(topping.topping) * topping.quantity
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
        for (var index = 0; index < cartHelper.cart().cartItems().length; index++)
        {
            var cartItem = cartHelper.cart().cartItems()[index];

            if (cartItem.isEnabled())
            {
                var orderLine =
                {
                    productId: cartItem.menuItem.Id == undefined ? cartItem.menuItem.MenuId : cartItem.menuItem.Id,
                    quantity: cartItem.quantity(),
                    price: cartItem.price,
                    name: cartItem.name,
                    orderLineIndex: orderLineNumber,
                    lineType: 1, // Normal item
                    instructions: cartItem.instructions,
                    person: cartItem.person,
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
                                name: topping.topping.ToppingName,
                                price: 0
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
                                name: topping.topping.ToppingName,
                                price: menuHelper.getToppingPrice(topping.topping) * topping.quantity
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
    nextButton: { displayText: ko.observable(''), action: 'none' },
    backButton: { visible: ko.observable(false), displayText: ko.observable(''), action: 'none' },
    nextButtonClicked: function ()
    {
        if (checkoutHelper.nextButton.action != undefined)
        {
            if (checkoutHelper.nextButton.action == 'placeorder')
            {
                // Place order
                checkoutHelper.placeOrder();
            }
            else
            {
                var section = checkoutHelper.checkoutSections()[Number(checkoutHelper.nextButton.action) - 1];

                // Has the user entered all the required information?
                if (section.validate())
                {
                    // Show a different section
                    checkoutHelper.showCheckoutSection(Number(checkoutHelper.nextButton.action));
                }
            }
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
        zipCodeHasError: ko.observable(false),
        addressMissingDetails: ko.observable(false),
        outOfDeliveryArea: ko.observable(false)
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
        if (viewModel.siteDetails().address.country == "United Kingdom") return checkoutHelper.validateUKAddress();
        else if (viewModel.siteDetails().address.country == "France") return checkoutHelper.validateFrenchAddress();
        else return self.validateUKAddress();
    },
    validateUKAddress: function ()
    {
        // Make sure the required details were entered
        checkoutHelper.errors.roadNameHasError(checkoutHelper.checkoutDetails.address.roadName().length == 0);
        checkoutHelper.errors.townHasError(checkoutHelper.checkoutDetails.address.town().length == 0);

        // Has the user entered all the delivery address details?
        checkoutHelper.errors.addressMissingDetails
        (
            checkoutHelper.errors.roadNameHasError() ||
            checkoutHelper.errors.townHasError()
        );

        var isInDeliveryZone = checkoutHelper.validatePostcode();

        checkoutHelper.errors.outOfDeliveryArea(!isInDeliveryZone); // Display an error if NOT in delivery area
        checkoutHelper.errors.zipCodeHasError(!isInDeliveryZone);

        checkoutHelper.errors.addressHasError
        (
            checkoutHelper.errors.addressMissingDetails() ||
            checkoutHelper.errors.outOfDeliveryArea()
        );

        // Was there an error?
        return !checkoutHelper.errors.addressHasError();
    },
    validateFrenchAddress: function ()
    {
        // Make sure the required details were entered
        checkoutHelper.errors.roadNameHasError(checkoutHelper.checkoutDetails.address.roadName().length == 0);
        checkoutHelper.errors.townHasError(checkoutHelper.checkoutDetails.address.town().length == 0);

        // Has the user entered all the delivery address details?
        checkoutHelper.errors.addressMissingDetails
        (
            checkoutHelper.errors.roadNameHasError() ||
            checkoutHelper.errors.townHasError()
        );

        checkoutHelper.errors.addressHasError
        (
            checkoutHelper.errors.addressMissingDetails()
        );

        // Was there an error?
        return !checkoutHelper.errors.addressHasError();
    },
    validate: function ()
    {
        for (var index = 0; index < checkoutHelper.checkoutSections().length; index++)
        {
            var section = checkoutHelper.checkoutSections()[Number(index)];

            if (!section.validate())
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
    validatePostcode: function ()
    {
        var isInDeliveryZone = false;

        if (deliveryZoneHelper.deliveryZones() == undefined || deliveryZoneHelper.deliveryZones().length == 0)
        {
            isInDeliveryZone = true; // No postcodes so we can't validate it
        }
        else
        {
            // Remove all white space from the postcode
            var cleanedUpZipCode = checkoutHelper.checkoutDetails.address.zipCode().replace(/\s+/g, '');
            isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(cleanedUpZipCode);
        }

        //if (viewModel.siteDetails().address.country == "United Kingdom")
        //{
        //    // Remove all white space from the postcode
        //    var cleanedUpZipCode = checkoutHelper.checkoutDetails.address.zipCode().replace(/\s+/g, '');
        //    if (cleanedUpZipCode.length > 4) cleanedUpZipCode = cleanedUpZipCode.substring(0, 4);

        //    if (cleanedUpZipCode.length == 4)
        //    {
        //        isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(cleanedUpZipCode);
        //    }
        //}
        //else
        //{
        //    isInDeliveryZone = deliveryZoneHelper.isInDeliveryZone(checkoutHelper.checkoutDetails.address.zipCode());
        //}

        return isInDeliveryZone;
    },
    deliveryTimeChanged: function ()
    {
        // Fix for iPhone - hides the select popup
        $('#deliveryTime').blur();
    },
    modifyOrder: function ()
    {
        viewModel.resetOrderType();
        guiHelper.cartActions(guiHelper.cartActionsMenu);

        // Allow the user modify the cart items
        guiHelper.isCartLocked(false);

        if (guiHelper.isMobileMode())
        {
            // Show the cart
            guiHelper.isMobileMenuVisible(true);
            checkoutMenuHelper.hideMenu();
            guiHelper.showCart();
        }
        else
        {
            // Show the menu
            guiHelper.showMenuView('menuSectionsView');
        }

        setTimeout
        (
            function ()
            {
                guiHelper.resize();
            },
            0
        );
    }
}