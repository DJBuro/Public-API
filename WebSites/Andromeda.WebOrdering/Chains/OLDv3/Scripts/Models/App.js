/// <reference path="Classes/Menu.js" />

/// <reference path="ViewModels/ErrorViewModel.js" />
/// <reference path="ViewModels/HeaderViewModel.js" />
/// <reference path="ViewModels/HomeViewModel.js" />
/// <reference path="ViewModels/MenuViewModel.js" />
/// <reference path="ViewModels/PleaseWaitViewModel.js" />
/// <reference path="ViewModels/SitesViewModel.js" />

/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function webOrderingApp()
{
    var self = this;

    self.viewEngine = new viewEngine();
    self.cache = new cache();
    self.headerViewModel = undefined;
    self.sites = ko.observableArray(); // A list of sites (from the site list web service call)
    self.site = ko.observable(undefined); // The currently selected site
    self.isMobileMode = ko.observable(false); // True when rendering for a mobile device
    self.isMobileMenuVisible = ko.observable(true); // True when the mobile "menu" button should be shown

    // Main entrance point for the application
    self.start = function ()
    {
        ko.setTemplateEngine(new ko.nativeTemplateEngine());

        // A trick to speed up button clicks on Android (and possible iPhone)
        ko.bindingHandlers.fastClick =
        {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel)
            {
                new FastButton
                (
                    element,
                    function ()
                    {
                        valueAccessor()(viewModel, event);
                    }
                );
            }
        };

        // The DOM element that contains the header 
        var viewElement = document.getElementById('header');

        // Create the header view model
        self.headerViewModel = new headerViewModel();

        // Data bind the header view model
        ko.applyBindings(self.headerViewModel, viewElement);

        // Initialise the view engine
        app.viewEngine.initialise();

        // Show the sites view
        app.viewEngine.showView('sitesView', new sitesViewModel());
    };
    self.showError = function (message, returnCallback, retryCallback, exception)
    {
        // Show the error view
        app.viewEngine.showView('errorView', new errorViewModel(message, returnCallback, retryCallback, exception));

        console.error(exception == undefined ? undefined : exception.message);
    };
    self.selectedItem =
    {
        menuItem: ko.observable(undefined),
        menuItems: ko.observableArray(),
        name: ko.observable(''),
        description: ko.observable(''),
        quantity: ko.observable(1),
        price: ko.observable(undefined),
        category1: ko.observable(undefined),
        category2: ko.observable(undefined),
        freeToppings: ko.observable(0),
        freeToppingsRemaining: ko.observable(0),
        toppings: ko.observableArray(),
        instructions: ko.observable(''),
        person: ko.observable(''),
        category1s: ko.observableArray(),
        category2s: ko.observableArray(),
        selectedCategory1: ko.observable(),
        selectedCategory2: ko.observable(),
        price: ko.observable(undefined)
    };
};

// This object is data bound to the HTML
var viewModel =
{
    serverUrl: 'http://' + location.host + '/Services/WebOrdering/webordering',
    sitesMode: ko.observable('pleaseWait'), // The mode that the 'sites' page is currently in   
    sitesError: ko.observable(false), // True when there is a problem getting the site details or menu from the server
    
    deals: ko.observableArray(),
    pickToppings: ko.observable(false),
       
    changeDeliveryTime: function ()
    {
    },
    timer: undefined,
    
    showCheckoutSection: function (data)
    {
        checkoutHelper.showCheckoutSection(data.index);
    },
    
};

var guiHelper =
{
    viewName: ko.observable(undefined),
    menuViewName: ko.observable(''),
    isCartLocked: ko.observable(false),
    
    canChangeOrderType: ko.observable(false),
    cartActionsCheckout: 'Checkout',
    cartActionsPayment: 'Payment',
    cartActionsMenu: 'Menu',
    cartActions: ko.observable('Menu'),
    
    
    isMenuBuilt: false,
    isViewVisible: ko.observable(true),
    isMenuVisible: ko.observable(false),
    isInnerMenuVisible: ko.observable(false),
    showHome: function ()
    {
    },
    showCart: function ()
    {
        $(window).scrollTop(0);
        app.viewEngine.showView('cartView');
    },
    getMaxSectionHeight: function ()
    {
        if (template_useSlideMenu)
        {
            var maxSectionHeight = 0;
            for (var sectionIndex = 0; sectionIndex < app.menu.sections().length; sectionIndex++)
            {
                var section = $('#section' + sectionIndex);
                var sectionHeight = section.height();

                if (sectionHeight > maxSectionHeight)
                {
                    maxSectionHeight = sectionHeight;
                }
            }

            viewModel.maxSectionHeight(maxSectionHeight + 'px');
        }
    },
    
    
    defaultWebErrorMessage: text_defaultWebErrorMessage,
    defaultInternalErrorMessage: text_defaultInternalErrorMessage,
    defaultPaymentErrorMessage: text_defaultPaymentErrorMessage,
    errorReturn: function ()
    {
        // Is there a callback function?
        if (typeof (viewModel.error.returnCallback) === 'function')
        {
            // Callback
            viewModel.error.returnCallback();
        }
    },
    showCheckoutAfterError: function ()
    {
        if (app.isMobileMode())
        {
            app.cart.checkout();
        }
        else
        {
            guiHelper.showMenuView('checkoutView');
        }
    },
    
    showPleaseWait: function (message, progress, callback)
    {
        guiHelper.pleaseWaitViewModel = new pleaseWaitViewModel(message, progress);

        app.viewEngine.showView('pleaseWaitView', guiHelper.pleaseWaitViewModel);

        if (typeof (callback) == 'function')
        {
            // Let knockout do its thing
            setTimeout
            (
                function ()
                {
                    callback();
                },
                0
            );
        }
    },
    pleaseWaitViewModel: undefined,
    
    
    showMenuView: function (viewName)
    {
        // Hide the view
        $('#viewContainer').css('display', 'none');

        // Show the menu
        $('#menuSection').css('display', 'block');

        if (viewName == 'menuSectionsView')
        {
            guiHelper.isInnerMenuVisible(true);
        }
        else
        {
            guiHelper.isInnerMenuVisible(false);

            // Show the menu sub view
            guiHelper.menuViewName(viewName);
        }

        $(window).scrollTop(0);
    },
    showError: function (message, returnCallback, exception)
    {
        // Show the error view
        app.viewEngine.showView('errorView', new errorViewModel(message, returnCallback, undefined, exception));
    }
}

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
    },
    initialiseCheckout: function()
    {
        var index = 0;

        checkoutHelper.checkoutSections.removeAll();

        // Who are you
        checkoutHelper.checkoutSections.push({ id: 'checkoutContactDetailsContainer', left: ko.observable('0px'), index: index, templateName: 'checkoutContactDetails-template', displayName: (index + 1) + ') ' + text_whoAreYou, validate: checkoutHelper.validateContactDetails });
        index++;

        // Collection/delivery time
        if (app.cart.orderType() == 'collection')
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', left: app.isMobileMode() ? ko.observable('0px') : ko.observable(template_menuSectionWidth + 'px'), index: index, templateName: 'checkoutDeliveryTime-template', displayName: (index + 1) + ') ' + text_collectionTime, validate: checkoutHelper.validateDeliveryTime });
            index++;
        }
        else
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryTimeContainer', left: app.isMobileMode() ? ko.observable('0px') : ko.observable(template_menuSectionWidth + 'px'), index: index, templateName: 'checkoutDeliveryTime-template', displayName: (index + 1) + ') ' + text_deliveryTime, validate: checkoutHelper.validateDeliveryTime });
            index++;
        }

        // Delivery address
        if (app.menu.ordertype() == 'delivery')
        {
            checkoutHelper.checkoutSections.push({ id: 'checkoutDeliveryAddressContainer', left: app.isMobileMode() ? ko.observable('0px') : ko.observable((template_menuSectionWidth * index) + 'px'), index: index, templateName: 'checkoutDeliveryAddress-template', displayName: (index + 1) + ') ' + text_deliveryAddress, validate: checkoutHelper.validateUKAddress });
            index++;
        }

        // Order notes
        checkoutHelper.checkoutSections.push({ id: 'checkoutOrderNotesContainer', left: app.isMobileMode() ? ko.observable('0px') : ko.observable((template_menuSectionWidth * index) + 'px'), index: index, templateName: 'checkoutOrderNotes-template', displayName: (index + 1) + ') ' + text_orderNotes, validate: function () { return true; } });
        index++;

        // Show the first section
        checkoutHelper.showCheckoutSection(0);
    },
    showCheckoutSection: function (sectionIndex)
    {
        // Back button
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
        
        // Next button
        if (sectionIndex == checkoutHelper.checkoutSections().length - 1)
        {
            checkoutHelper.nextButton.displayText(text_placeOrder);
            checkoutHelper.nextButton.action = 'placeorder';
        }
        else
        {
            checkoutHelper.nextButton.displayText(text_next);
            checkoutHelper.nextButton.action = sectionIndex + 1;
        }

        checkoutHelper.visibleCheckoutSection(sectionIndex);

        var marginLeft = (template_menuSectionWidth * -1) * sectionIndex;

        // Don't let the user modify the cart items
        guiHelper.isCartLocked(true);

        $('#checkoutSectionsContainer').animate
        (
            {
                marginLeft: marginLeft
            },
            {
                duration: 500,
                queue: false
            }
        );

        $('#checkoutPageInner').animate
        (
            {
                scrollTop: 0
            },
            {
                duration: 500,
                queue: false
            }
        );
    },
    refreshTimes: function()
    {
        checkoutHelper.times.removeAll();
        checkoutHelper.times.push({ mode: undefined, time: undefined, text: text_pleaseSelectATime });

        // Is the store open for ASAP?
        //if (app.site().estDelivTime == undefined || app.site().estDelivTime == 0)
        //{
        //    checkoutHelper.times.push({ mode: 'ASAP', time: undefined, text: text_asSoonAsPossibleNoETD });
        //}
        //else
        //{
        //    checkoutHelper.times.push({ mode: 'ASAP', time: undefined, text: text_asSoonAsPossible + ' (' + app.site().estDelivTime + ')' });
        //}

        // Get todays opening times
        var openingTimes = openingTimesHelper.getTodaysOpeningTimes();

        var timeBlocks = [];
        var today = new Date();

        // The first available slot has to be at least EDT minutes from now.
        // If EDT is not available default to 15 minutes.
        var offset = 15;
        if (app.site().estDelivTime != undefined && app.site().estDelivTime > 0)
        {
            offset = app.site().estDelivTime;
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
                    text: checkoutHelper.formatHour(hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + hourAMPM + ' - ' +
                          checkoutHelper.formatHour(hourPlusOne12HourClock) + ':' + checkoutHelper.formatMinute((slotSize - (60 - minute))) + hourPlusOneAMPM
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
                    text: checkoutHelper.formatHour(hour12HourClock) + ':' + checkoutHelper.formatMinute(minute) + hourAMPM + ' - ' +
                          checkoutHelper.formatHour(hour12HourClock) + ':' + checkoutHelper.formatMinute((minute + slotSize)) + hourAMPM
                }
            );
        }
    },
    formatMinute: function(minute)
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
        chefNotes: ko.observable('')
    },
    placeOrder: function ()
    {
        // Has the user entered the required information?
        if (checkoutHelper.validate())
        {
            checkoutHelper.showPaymentPicker();
        }
    },
    showPaymentPicker: function()
    {
        if (app.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('checkout', false);

            app.viewEngine.showView('paymentView');
        }
        else
        {
            guiHelper.showMenuView('paymentView');
        }

        // Switch the cart to checkout mode
        guiHelper.canChangeOrderType(false);
        guiHelper.cartActions(guiHelper.cartActionsCheckout);

        // Reset the agree flag - assume they haven't agreed yet
        tandcHelper.agree(undefined);

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

        guiHelper.pleaseWaitViewModel.pleaseWaitMessage(text_preparingForPayment);
        guiHelper.pleaseWaitViewModel.pleaseWaitProgress('');
        app.viewEngine.showView('pleaseWaitView');

        if (app.site().siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
        {
            // Initialise the mercury payment
            acsApi.putMercuryPayment
            (
                app.site().siteId,
                function ()
                {
                    if (app.isMobileMode())
                    {
                        guiHelper.isMobileMenuVisible(false);
                        checkoutMenuHelper.showMenu('pay', false);
                        app.viewEngine.showView('mercuryPaymentView');
                    }
                    else
                    {
                        guiHelper.showMenuView('mercuryPaymentView');
                    }

                    var iFrame = $('#mercuryPaymentIFrame');
                    iFrame.attr('src', 'https://hc.mercurydev.net/CheckoutIFrame.aspx?pid=' + app.cart.cart().mercuryPaymentId());

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
        else if (app.site().siteDetails().paymentProvider.toUpperCase() == 'DATACASHLIVE' ||
            app.site().siteDetails().paymentProvider.toUpperCase() == 'DATACASHTEST')
        {
            // Initialise the DataCash payment
            acsApi.putDataCashPayment
            (
                app.site().siteId,
                function ()
                {
                    if (app.isMobileMode())
                    {
                        guiHelper.isMobileMenuVisible(false);
                        checkoutMenuHelper.showMenu('pay', false);
                        app.viewEngine.showView('dataCashPaymentView');
                    }
                    else
                    {
                        guiHelper.showMenuView('dataCashPaymentView');
                    }

                    var iFrame = $('#dataCashPaymentIFrame');
                    iFrame.attr('src', app.cart.cart().dataCashPaymentDetails().url + '?HPS_SessionID=' + app.cart.cart().dataCashPaymentDetails().sessionId);

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
        else
        {
            // There is a problem with the payment provider - not supported
        }
    },
    sendOrderToStore: function()
    {
        guiHelper.pleaseWaitViewModel.pleaseWaitMessage(text_sendingOrderToStore);
        guiHelper.pleaseWaitViewModel.pleaseWaitProgress('');
        app.viewEngine.showView('pleaseWaitView');

        // Work out how long the customer took to place the order
        var timeNow = new Date();
        var timeTakenMilliseconds = Math.abs(timeNow - viewModel.timer);
        var timeTakenSeconds = Math.round(timeTakenMilliseconds / 1000);

        var orderDetails =
        {
            paymentType: checkoutHelper.checkoutDetails.payNow ? app.site().siteDetails().paymentProvider : 'PayLater',
            paymentData : undefined,
            order :
            {
                partnerReference: '',
                type: app.menu.ordertype(),
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
                        prem1: checkoutHelper.checkoutDetails.address.prem1(),
                        prem2: checkoutHelper.checkoutDetails.address.prem2(),
                        prem3: checkoutHelper.checkoutDetails.address.prem3(),
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
                        userLocality1: checkoutHelper.checkoutDetails.address.userLocality1(),
                        userLocality2: checkoutHelper.checkoutDetails.address.userLocality2(),
                        userLocality3: checkoutHelper.checkoutDetails.address.userLocality3(),
                        country: template_customerAddressCountryCode
                    }
                },
                pricing:
                {
                    priceBeforeDiscount: app.cart.cart().totalPrice(),
                    pricesIncludeTax: 'false',
                    priceAfterDiscount: app.cart.cart().totalPrice()
                },
                orderLines:
                [
                ],
                orderPayments:
                [
                ]
            }
        };

        // Pass back any payment reference numbers or info
        if (app.site().siteDetails().paymentProvider.toUpperCase() == 'MERCURY')
        {
            orderDetails.paymentData = app.cart.cart().mercuryPaymentId();
        }
        else if (app.site().siteDetails().paymentProvider.toUpperCase() == 'DATACASHLIVE' || app.site().siteDetails().paymentProvider.toUpperCase() == 'DATACASHTEST')
        {
            orderDetails.paymentData = app.cart.cart().dataCashPaymentDetails();
        }

        var orderLineNumber = 0;

        // Deals
        for (var index = 0; index < app.cart.cart().deals().length; index++)
        {
            var deal = app.cart.cart().deals()[index];

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
                                    price: app.menu.getToppingPrice(topping.topping) * topping.quantity
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
        for (var index = 0; index < app.cart.cart().cartItems().length; index++)
        {
            var cartItem = app.cart.cart().cartItems()[index];

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
                                price: app.menu.getToppingPrice(topping.topping) * topping.quantity
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

        // Send the order to ACS
        acsApi.putOrder(app.site().siteId, orderDetails);
    },
    checkoutSections: ko.observableArray(),
    getTemplateName: function (data)
    {
        return data.templateName;
    },
    nextButton: { displayText: ko.observable(text_next), action: 'none' },
    backButton: { visible: ko.observable(false), displayText: ko.observable(text_back), action: 'none' },
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
    validateUKAddress: function ()
    {
        // Make sure the required details were entered
        checkoutHelper.errors.roadNameHasError(checkoutHelper.checkoutDetails.address.roadName().length == 0);
        checkoutHelper.errors.townHasError(checkoutHelper.checkoutDetails.address.town().length == 0);
        checkoutHelper.errors.zipCodeHasError(checkoutHelper.checkoutDetails.address.zipCode().length < 4);

        // Has the user entered all the delivery address details?
        checkoutHelper.errors.addressMissingDetails
        (
            checkoutHelper.errors.roadNameHasError() ||
            checkoutHelper.errors.townHasError() ||
            checkoutHelper.errors.zipCodeHasError()
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
    validate: function ()
    {
        for (var index = 0; index < checkoutHelper.checkoutSections().length; index++)
        {
            var section = checkoutHelper.checkoutSections()[Number(index)];
            
            if (!section.validate())
            {
                if (app.isMobileMode())
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
    deliveryTimeChanged: function ()
    {
        // Fix for iPhone - hides the select popup
        $('#deliveryTime').blur();
    }
}

// Cache management
var cacheHelper =
{
    loadCachedMenuForSiteId : function (siteId)
    {
        var menu = undefined;

        try
        {
            // Show the please wait view
            guiHelper.showPleaseWait
            (
                text_loadingMenu,
                undefined,
                function ()
                {
                    // Get the menu index
                    var menuIndex = amplify.store('menuIndex');

                    // Did we find the menu index?
                    if (menuIndex != undefined)
                    {
                        // Find the menu index for this site
                        var menuIndexItem = undefined;
                        for (var index = 0; index < menuIndex.length; index++)
                        {
                            menuIndexItem = menuIndex[index];

                            // Do we already have the menu for this site and version in the local cache?
                            if (menuIndexItem.siteId === siteId &&
                                menuIndexItem.menuVersion === app.site().menuVersion)
                            {
                                // We have the correct version of the menu in the cache for this site

                                // Get the menu out of the cache
                                menu = amplify.store('m_' + siteId);
                            }
                        }
                    }
                }
            );
        }
        catch (exception) { } // Ignore exceptions 

        return menu;
    },
    cacheMenu : function (menu)
    {
        var success = false;

        // Get the menu index
        var menuIndex = amplify.store('menuIndex');

        // If there is no menu index create a new empty one
        if (menuIndex === undefined)
        {
            menuIndex = [];
        }

        // Find the menu index for this site
        var menuIndexItem = undefined;
        for (var index = 0; index < menuIndex.length; index++)
        {
            var checkMenuIndexItem = menuIndex[index];

            if (checkMenuIndexItem.siteId === app.site().siteId)
            {
                menuIndexItem = checkMenuIndexItem;
                break;
            }
        }

        // Have we got a menu for this site?
        if (menuIndexItem === undefined)
        {
            // No menu for the site
            menuIndexItem =
            {
                siteId: app.site().siteId,
                lastUsed: new Date(),
                menuVersion: app.site().menuVersion
            };

            // Add the menu to the index
            menuIndex.push(menuIndexItem);
        }
        else
        {
            // Update the last time the menu was accessed
            menuIndexItem.menuVersion = app.site().menuVersion;
            menuIndexItem.lastUsed = new Date();
        }

        // Add/update the menu
        if (cacheHelper.cacheObject('m_' + app.site().siteId, menu, menuIndex))
        {
            // Update the index
            if (cacheHelper.cacheObject('menuIndex', menuIndex, menuIndex))
            {
                success = true;
            }
        }

        return success;
    },
    cacheObject : function (keyName, object, menuIndex)
    {
        var keepTrying = true;
        var success = false;

        // If we run out of storage, keep deleting old menus until there is space
        while (keepTrying)
        {
            // Lets not get stuck in an infinite loop...
            keepTrying = false;

            try
            {
                // Update the menu index
                amplify.store(keyName, object);

                success = true;
            }
            catch (exception)
            {
                // Is the cache full?
                if (exception == 'amplify.store quota exceeded')
                {
                    // Try and clear out the oldest menu/s
                    if (cacheHelper.removeOldestMenuFromCache(menuIndex))
                    {
                        // We've successfully removed a menu.  There might be space now...
                        keepTrying = true;
                    }
                }
            }
        }

        return success;
    },
    removeOldestMenuFromCache : function (menuIndex)
    {
        // Find the oldest menu
        var oldestMenuIndex = undefined;
        for (var index = 0; index < menuIndex.length; index++)
        {
            var checkMenuIndexItem = menuIndex[index];

            if (oldestMenuIndex === undefined || checkMenuIndexItem.lastUsed < oldestMenuIndex.lastUsed)
            {
                oldestMenuIndex = checkMenuIndexItem;
            }
        }

        // Have we got a menu for this site?
        if (oldestMenuIndex != undefined)
        {
            // Remove the cached menu
            amplify.store('m_' + oldestMenuIndex.siteId, null);

            // Remove the menu from the index
            menuIndex.splice(oldestMenuIndex, 1);

            // Save the index
            amplify.store('menuIndex', menuIndex);

            // We've made some space
            return true;
        }
        else
        {
            // Nothing left to delete?
            return false;
        }
    }
};

var helper = 
{
    
    newOrder: function ()
    {
        // Empty out the checkout
        checkoutHelper.clearCheckout();

        // Empty out the cart
        app.cart.clearCart();

        guiHelper.showHome();
    },
    formatUTCDate: function (date)
    {
        return date.getUTCFullYear() + '-'
            + helper.pad(date.getUTCMonth() + 1) + '-'
            + helper.pad(date.getUTCDate()) + 'T'
            + helper.pad(date.getUTCHours()) + ':'
            + helper.pad(date.getUTCMinutes()) + ':'
            + helper.pad(date.getUTCSeconds()) + 'Z';
    },
    formatUTCSlot: function (slot)
    {
        var dividerIndex = slot.indexOf(':');
        var hours = slot.substring(0, dividerIndex);
        var minutes = slot.substring(dividerIndex + 1);

        var today = new Date();
        today.setHours(hours);
        today.setMinutes(minutes);

        var dateString = helper.formatUTCDate(today);
        return dateString;

        //var now = new Date();
        //var fixedSlot = slot;

        //if (slot.indexOf(':0', slot.length - 2) != -1)
        //{
        //    fixedSlot += '0';
        //}

        //return now.getUTCFullYear() + '-'
        //    + helper.pad(now.getUTCMonth() + 1) + '-'
        //    + helper.pad(now.getUTCDate()) + 'T'
        //    + fixedSlot + 'Z';
    },
    pad: function (source)
    {
        if ((typeof (source) == 'number' && source < 10) ||
            (typeof (source) == 'string' && source.length == 1))
        {
            return '0' + source;
        }
        else
        {
            return source;
        }
    },
    formatPrice: function (price)
    {
        if (price == undefined)
        {
            return helper.currencySymbol + " -"
        }
        else
        {
            var x = Number(price);
            var y = x / 100;

            var price = helper.currencySymbol + y.toFixed(2);

            if (helper.useCommaDecimalPoint)
            {
                price = price.replace('.', ',');
            }

            return price;
        }
    },
    currencySymbol: '&pound;',
    useCommaDecimalPoint: false,
    findById: function(id, list)
    {
        for (var index = 0; index < list.length; index++)
        {
            var item = list[index];

            if (item.Id == id)
            {
                return item;
            }
        }

        return undefined;
    },
    findCategory: function(category, categories)
    {
        for (var index = 0; index < categories().length; index++)
        {
            var existingCategory = categories()[index];

            if (existingCategory.Name == category.Name)
            {
                return true;
            }
        }

        return false;
    },
    findByMenuId: function (id, list)
    {
        for (var index = 0; index < list.length; index++)
        {
            var item = list[index];

            if ((item.Id == undefined ? item.MenuId : item.Id) == id)
            {
                return item;
            }
        }

        return undefined;
    }
}

var dealHelper =
{
    returnToCart: false,
    mode: ko.observable('addDeal'),
    subscriptions: [],
    hasError: ko.observable(false),
    addToCart: function ()
    {
        // The deal to show on the deal popup
        dealHelper.mode('addDeal');
        dealHelper.hasError(false);
        dealHelper.selectedDeal.name(this.deal.DealName);
        dealHelper.selectedDeal.description(this.deal.Description);
        dealHelper.selectedDeal.deal = this.deal;

        // Build the deal lines
        dealHelper.selectedDeal.bindableDealLines.removeAll();

        for (var index = 0; index < this.dealLineWrappers.length; index++)
        {
            var dealLineWrapper = this.dealLineWrappers[index];

            var bindableDealLine =
            {
                dealLineWrapper: dealLineWrapper,
                itemWrappers: undefined,
                selectedItemWrapper: ko.observable(undefined),
                previouslySelectedItemWrapper: undefined,
                selectedMenuItem: undefined, // Each item could include multiple menu items for different cat1/cat2s - this is the menu item the customer wants
                hasError: ko.observable(false),
                toppings: undefined,
                instructions: '',
                person: '',
                id: 'dl_' + index
            };

            // Build the allowable items in the deal line
            var itemWrappers = [];
            for (var itemIndex = 0; itemIndex < dealLineWrapper.items.length; itemIndex++)
            {
                var item = dealLineWrapper.items[itemIndex];
                var itemWrapper =
                {
                    bindableDealLine: bindableDealLine, // The deal line that the item is in - we're data binding to the item but we need a way to get back to the deal line the item is in
                    item: item,
                    name: item.name
                };

                itemWrappers.push(itemWrapper);
            }

            // Set the allowable items in the deal line
            bindableDealLine.itemWrappers = itemWrappers;

            // Auto select the first item if there is only one
            if (bindableDealLine.itemWrappers.length == 1)
            {
                bindableDealLine.selectedItemWrapper(bindableDealLine.itemWrappers[0]);
                bindableDealLine.selectedMenuItem = bindableDealLine.selectedItemWrapper().item.menuItems()[0];

                // We'll also need the selected items toppings
                bindableDealLine.toppings = app.menu.getItemToppings(bindableDealLine.selectedMenuItem);
            }

            // Add the deal line to the deal
            dealHelper.selectedDeal.bindableDealLines.push(bindableDealLine);
        }

        // Show the deal popup
        dealPopupHelper.returnToCart = false;  // If the user cancels show the menu not the cart (mobile only)
        dealPopupHelper.showDealPopup('addDeal', true);

        // Let knockout sort out the GUI before subscribing to events as the act of building the GUI triggers these events which we're not interested in
        setTimeout
        (
            function ()
            {
                dealHelper.subscribeToDealLineChanges();
            },
            0
        );
    },
    subscribeToDealLineChanges: function()
    {
        for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
        {
            // We want to know when the selected menu item changes
            dealHelper.subscriptions.push(dealHelper.selectedDeal.bindableDealLines()[index].selectedItemWrapper.subscribe(dealHelper.selectedDealLineChanged));
        }
    },
    unSubscribeToDealLineChanges: function ()
    {
        for (var index = 0; index < dealHelper.subscriptions.length; index++)
        {
            var subscription = dealHelper.subscriptions[index];
            subscription.dispose();
        }
    },
    selectedDeal: 
    {
        name: ko.observable(''),
        description: ko.observable(''),
        bindableDealLines: ko.observableArray(),
        deal: undefined
    },
    selectedBindableDealLine: undefined, // This is used to temporarily hold the deal line that is currently being customized in the toppings popup so when the popup is closed we know which deal line to copy the customization to
    dealPopupCancel: function ()
    {
        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    getDealLineTemplateName: function (bindableDealLine)
    {
        return bindableDealLine == undefined ? 'popupDealLine-template' : bindableDealLine.dealLineWrapper.templateName;
    },
    selectedDealLineChanged: function (itemWrapper)
    {
        if (itemWrapper.bindableDealLine != undefined)
        {
            // Different item - different toppings
            itemWrapper.bindableDealLine.toppings = undefined;

            // Different item - different menu item
            itemWrapper.bindableDealLine.menuItem = undefined; //itemWrapper.item.menuItems[1];
            itemWrapper.bindableDealLine.selectedMenuItem = undefined; //itemWrapper.item.menuItems[1];

            // Show the toppings popup
            dealHelper.showToppingsPopupForDealLine(itemWrapper.bindableDealLine, 'addDealItem', true);

            $('#' + itemWrapper.bindableDealLine.dealLineWrapper.id).blur();
        }
    },
    customizeDealLine: function (bindableDealLine)
    {
        dealHelper.showToppingsPopupForDealLine(bindableDealLine, 'addDealItem', false);
    },
    showToppingsPopupForDealLine: function (bindableDealLine, mode, onlyShowIfToppings)
    {
        // The newly selected item
        var selectedItemWrapper = bindableDealLine.selectedItemWrapper();

        if (selectedItemWrapper != undefined && selectedItemWrapper.name != text_pleaseSelectAnItem)
        {
            // We don't need to know about deal line changes any more
            dealHelper.unSubscribeToDealLineChanges();

            // Get the price of the deal line (luckily, we keep a reference to it)
            bindableDealLine.price = app.menu.calculateDealLinePrice(selectedItemWrapper.bindableDealLine.dealLineWrapper.dealLine);

            // Store for later
            dealHelper.selectedBindableDealLine = bindableDealLine;

            // Each allowable item in a deal line could consist of multiple menu items e.g. different sizes.  Default to the first one
            var item = bindableDealLine.selectedItemWrapper().item;

            if (bindableDealLine.selectedMenuItem == undefined)
            {
                // Default to the first menu item
                bindableDealLine.selectedMenuItem = item.menuItems()[0];
            }

            // Get the categories
            var category1 = helper.findById(bindableDealLine.selectedMenuItem.Cat1 == undefined ? bindableDealLine.selectedMenuItem.Category1 : bindableDealLine.selectedMenuItem.Cat1, app.menu.rawMenu.Category1);
            var category2 = helper.findById(bindableDealLine.selectedMenuItem.Cat2 == undefined ? bindableDealLine.selectedMenuItem.Category2 : bindableDealLine.selectedMenuItem.Cat2, app.menu.rawMenu.Category2);

            // The item to show on the toppings popup
            viewModel.selectedItem.name(item.name);
            viewModel.selectedItem.menuItems(item.menuItems());
            viewModel.selectedItem.description(item.description);
            viewModel.selectedItem.quantity(1);
            viewModel.selectedItem.menuItem(bindableDealLine.selectedMenuItem);
            viewModel.selectedItem.freeToppings(bindableDealLine.selectedMenuItem.FreeTops);
// Free deal tops????
            viewModel.selectedItem.freeToppingsRemaining(bindableDealLine.selectedMenuItem.FreeTops);
            viewModel.selectedItem.instructions(bindableDealLine.instructions);
            viewModel.selectedItem.person(bindableDealLine.person);
            viewModel.selectedItem.price(helper.formatPrice(0));

            // Do these last becuase the UI is data bound to them
            viewModel.selectedItem.category1s.removeAll();
            for (var index = 0; index < item.category1s().length; index++)
            {
                viewModel.selectedItem.category1s.push(item.category1s()[index]);
            }
            viewModel.selectedItem.category2s.removeAll();
            for (var index = 0; index < item.category2s().length; index++)
            {
                viewModel.selectedItem.category2s.push(item.category2s()[index]);
            }

            viewModel.selectedItem.selectedCategory1(category1);
            viewModel.selectedItem.selectedCategory2(category2);

            // Get the toppings
            if (dealHelper.selectedBindableDealLine.toppings == undefined)
            {
                viewModel.selectedItem.toppings(app.menu.getItemToppings(viewModel.selectedItem.menuItem()));
            }
            else
            {
                viewModel.selectedItem.toppings(dealHelper.cloneToppings(dealHelper.selectedBindableDealLine.toppings));
            }

            // Does the customer need to pick any toppings?
            if (onlyShowIfToppings &&
                viewModel.selectedItem.toppings().length == 0 &&
                viewModel.selectedItem.category1s().length < 2 &&
                viewModel.selectedItem.category2s().length < 2)
            {
                popupHelper.commitToDeal();
            }
            else
            {
                // Hide the deal popup
                dealPopupHelper.hideDealPopup();

                // Show the toppings popup
                popupHelper.showPopup(mode);
            }
        }
    },
    cloneToppings: function(sourceToppings)
    {
        var cloneToppings = [];
        for (var index = 0; index < sourceToppings.length; index++)
        {
            var topping = sourceToppings[index];

            var dealLineTopping =
            {
                type: topping.type,
                selectedSingle: ko.observable(topping.selectedSingle()),
                selectedDouble: ko.observable(topping.selectedDouble()),
                topping: topping.topping,
                price: topping.price,
                doublePrice: topping.doublePrice,
                freeQuantity: topping.freeQuantity,
                quantity: topping.quantity,
                cartPrice: ko.observable(''),
                cartName: ko.observable(''),
                cartQuantity: ko.observable('')
            };

            cloneToppings.push(dealLineTopping);
        }

        return cloneToppings;
    },
    acceptChanges: function ()
    {
        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    commitToCart: function ()
    {
        // Has the user entered all the required information?
        if (!dealHelper.checkForErrors())
        {
            // Cheapest free
            if (dealHelper.selectedDeal.deal.ForceCheapestFree && dealHelper.selectedDeal.dealLines().length > 1)
            {
                var dealLine1 = dealHelper.selectedDeal.dealLines()[0];
                var dealLine2 = dealHelper.selectedDeal.dealLines()[1];

                // Check the first two deal lines
                var dealLinePrice1 = app.menu.calculateDealLinePrice(dealLine1, true);
                var dealLinePrice2 = app.menu.calculateDealLinePrice(dealLine2, true);

                if (dealLinePrice2 > dealLinePrice1)
                {
                    // We need to switch the menu items around so the more expensive one is in deal line 1

                    // Remove the first two items in the array
                    dealHelper.selectedDeal.dealLines.splice(0, 1);
                    dealHelper.selectedDeal.dealLines.splice(0, 1);

                    // Insert the removed item 
                    dealHelper.selectedDeal.dealLines.push(dealLine2);
                    dealHelper.selectedDeal.dealLines.push(dealLine1);
                }
            }

            // Clone the selected deal
            var deal =
            {
                name: dealHelper.selectedDeal.name(),
                price: 0,
                displayPrice: ko.observable('-'), 
                dealLines: ko.observableArray(),
                deal:
                {
                    name: dealHelper.selectedDeal.name(),
                    description: dealHelper.selectedDeal.description(),
                    deal: dealHelper.selectedDeal.deal
                },
                minimumOrderValue: helper.formatPrice(dealHelper.selectedDeal.deal.MinimumOrderValue),
                minimumOrderValueNotMet: ko.observable(true),
                isEnabled: ko.observable()
            };

            // Add deal lines
            for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
            {
                var bindableDealLine = dealHelper.selectedDeal.bindableDealLines()[index];

                // Copy the toppings to the cart object.  We have to actually clone the topping objects - if we just copy the object references it'll get screwed up later
                var cartToppings = [];
                for (var toppingIndex = 0; toppingIndex < bindableDealLine.toppings.length; toppingIndex++)
                {
                    var topping = bindableDealLine.toppings[toppingIndex];

                    var cartTopping =
                    {
                        type: topping.type,
                        selectedSingle: ko.observable(topping.selectedSingle()),
                        selectedDouble: ko.observable(topping.selectedDouble()),
                        topping: topping.topping,
                        price: topping.price,
                        doublePrice: topping.doublePrice,
                        freeQuantity: topping.freeQuantity,
                        quantity: topping.quantity,
                        cartPrice: ko.observable(''),
                        cartName: ko.observable(''),
                        cartQuantity: ko.observable('')
                    };

                    cartToppings.push(cartTopping);
                }

                // Get the categories
                var category1 = helper.findById(bindableDealLine.selectedMenuItem.Cat1 == undefined ? bindableDealLine.selectedMenuItem.Category1 : bindableDealLine.selectedMenuItem.Cat1, app.menu.rawMenu.Category1);
                var category2 = helper.findById(bindableDealLine.selectedMenuItem.Cat2 == undefined ? bindableDealLine.selectedMenuItem.Category2 : bindableDealLine.selectedMenuItem.Cat2, app.menu.rawMenu.Category2);

                deal.dealLines.push
                (
                    {
                        name: app.cart.getCartItemName(bindableDealLine.selectedItemWrapper().item, category1, category2),
                        price: 0,
                        cartPrice: 0,
                        displayPrice: ko.observable('-'),
                        dealLine: bindableDealLine.dealLineWrapper.dealLine,
                        selectedMenuItem: bindableDealLine.selectedMenuItem,
                        displayToppings: ko.observableArray(app.menu.getCartDisplayToppings(cartToppings)),
                        toppings: bindableDealLine.toppings,
                        categoryPremiumName: ko.observable(undefined),
                        categoryPremium: ko.observable(undefined),
                        itemPremiumName: ko.observable(undefined),
                        itemPremium: ko.observable(undefined),
                        instructions: bindableDealLine.instructions,
                        person: bindableDealLine.person
                    }
                );
            }

            // Add the selected deal to the cart
            app.cart.cart().deals.push(deal);

            // Recalculate the cart price
            app.cart.refreshCart();

            // Hide the deal popup
            dealPopupHelper.hideDealPopup();

            // If we are in mobile mode show the cart
            if (app.isMobileMode())
            {
                guiHelper.showCart();
            }
        }
    },
    removeFromCart: function ()
    {
        // Remove the item from the cart
        app.cart.cart().deals.remove(app.cart.cart().selectedCartItem());

        // Recalculate the total price
        app.cart.refreshCart();

        // Hide the deal popup
        dealPopupHelper.hideDealPopup();
    },
    checkForErrors: function ()
    {
        var errors = false;

        // Check each deal line and make sure the customer has selected an item
        for (var index = 0; index < dealHelper.selectedDeal.bindableDealLines().length; index++)
        {
            var bindableDealLine = dealHelper.selectedDeal.bindableDealLines()[index];

            bindableDealLine.hasError(false); // Reset to default

            // Does the user need to select anything for this deal line?
            if (bindableDealLine.itemWrappers.length > 0)
            {
                // Has the user actually selected anything?
                if (bindableDealLine.selectedMenuItem == undefined)
                {
                    bindableDealLine.hasError(true);
                    errors = true;
                }
            }
        }

        // Show or hide the error message
        dealHelper.hasError(errors);

        return errors;
    }
}

var popupHelper = 
{
    returnToCart: false,
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    mode: ko.observable('addItem'),
    cancel: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    cancelDeal: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();

        // Show the deal popup
        dealPopupHelper.showDealPopup('addDeal', false);

        if (popupHelper.mode() == 'addDealItem')
        {
            // Were there multiple items to pick from?
            if (dealHelper.selectedBindableDealLine.itemWrappers.length > 1)
            {
                // Was an item already selected?
                if (dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper == undefined)
                {
                    // Display the "please select an item" item
                    dealHelper.selectedBindableDealLine.selectedItemWrapper(dealHelper.selectedBindableDealLine.itemWrappers[0]);
                    dealHelper.selectedBindableDealLine.selectedMenuItem = undefined;
                }
                else
                {
                    // Restore the previously selected item
                    dealHelper.selectedBindableDealLine.selectedItemWrapper(dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper);
                }
            }

            // Re-scbscribe to deal line changes
            dealHelper.subscribeToDealLineChanges();
        }
    },
    commitToDeal: function ()
    {
        // Hide the toppings popup
        popupHelper.hidePopup();

        // Show the deal popup
        dealPopupHelper.showDealPopup('addDeal', false);

        // Keep hold of the currently selected item so next time they change it but cancel we can set it back
        dealHelper.selectedBindableDealLine.previouslySelectedItemWrapper = dealHelper.selectedBindableDealLine.selectedItemWrapper();

        // Record which menu item was selected
        dealHelper.selectedBindableDealLine.selectedMenuItem = viewModel.selectedItem.menuItem();

        // Check to see if any errors have now been resolved
        dealHelper.checkForErrors();

        dealHelper.selectedBindableDealLine.toppings = dealHelper.cloneToppings(viewModel.selectedItem.toppings());
        dealHelper.selectedBindableDealLine.instructions = viewModel.selectedItem.instructions();
        dealHelper.selectedBindableDealLine.person = viewModel.selectedItem.person();

        // Resubsribe to deal line changes
        dealHelper.subscribeToDealLineChanges();
    },
    acceptChanges: function ()
    {
        // Update the quantity
        app.cart.cart().selectedCartItem().quantity(viewModel.selectedItem.quantity());

        // Update the cat1
        app.cart.cart().selectedCartItem().selectedCategory1 = viewModel.selectedItem.selectedCategory1();

        // Update the cat2
        app.cart.cart().selectedCartItem().selectedCategory2 = viewModel.selectedItem.selectedCategory2();

        // Update the menu item (will change if a different cat1 / cat2 are selected)
        app.cart.cart().selectedCartItem().menuItem = viewModel.selectedItem.menuItem();

        // Update the chefs instructions
        app.cart.cart().selectedCartItem().instructions = viewModel.selectedItem.instructions();

        // Update the person
        app.cart.cart().selectedCartItem().person = viewModel.selectedItem.person();

        // Update the toppings
        var cartToppings = [];
        for (var index = 0; index < viewModel.selectedItem.toppings().length; index++)
        {
            var topping = viewModel.selectedItem.toppings()[index];

            var cartTopping =
            {
                type: topping.type,
                selectedSingle: ko.observable(topping.selectedSingle()),
                selectedDouble: ko.observable(topping.selectedDouble()),
                topping: topping.topping,
                price: topping.price,
                doublePrice: topping.doublePrice,
                freeQuantity: topping.freeQuantity,
                quantity: topping.quantity,
                cartPrice: ko.observable(''),
                cartName: ko.observable(''),
                cartQuantity: ko.observable('')
            };

            cartToppings.push(cartTopping);
        }
        app.cart.cart().selectedCartItem().toppings.removeAll();
        app.cart.cart().selectedCartItem().toppings(cartToppings);

        // Update the display name
        app.cart.cart().selectedCartItem().displayName(app.menu.getCartItemDisplayName(viewModel.selectedItem, viewModel.selectedItem.selectedCategory1(), viewModel.selectedItem.selectedCategory2()));

        // Recalculate the item price
        var price = app.menu.calculateItemPrice(app.cart.cart().selectedCartItem().menuItem, app.cart.cart().selectedCartItem().quantity(), viewModel.selectedItem.toppings());
        app.cart.cart().selectedCartItem().price = price;
        app.cart.cart().selectedCartItem().displayPrice(helper.formatPrice(price));

        app.cart.cart().selectedCartItem().displayToppings(app.menu.getCartDisplayToppings(cartToppings));

        // Recalculate the total price of all items in the cart
        app.cart.refreshCart();

        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    removeFromCart: function ()
    {
        // Remove the item from the cart
        app.cart.cart().cartItems.remove(app.cart.cart().selectedCartItem());

        // Recalculate the total price
        app.cart.refreshCart();

        // Hide the toppings popup
        popupHelper.hidePopup();
    },
    quantityChanged: function ()
    {
        var price = app.menu.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
        viewModel.selectedItem.price(helper.formatPrice(price));

        return true;
    },
    singleChanged: function ()
    {
        popupHelper.popupToppingChanged('single', this);

        return true;
    },
    doubleChanged: function ()
    {
        popupHelper.popupToppingChanged('double', this);

        return true;
    },
    showPopup: function (mode)
    {
        $(window).scrollTop(0);

        // Show the toppings popup
        popupHelper.mode(mode);
        viewModel.pickToppings(true);

        popupHelper.isBackgroundVisible(true);
        popupHelper.isPopupVisible(true);

        if (app.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
        }

        // Give knockout time to do its thing (Javascript doesn't do not proper multi-threading - need to let the browser have the thread back)
        setTimeout
        (
            function ()
            {
                // Recalculate the item price
                var price = 0;
                if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
                {
                    price = app.menu.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings(), true);
                }
                else
                {
                    price = app.menu.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
                }

                viewModel.selectedItem.price(helper.formatPrice(price));
            },
            0
        );
    },
    hidePopup: function ()
    {
        popupHelper.isBackgroundVisible(false);
        popupHelper.isPopupVisible(false);

        // We might be on top of the deal popup
        if (!dealPopupHelper.isPopupVisible())
        {
            guiHelper.isInnerMenuVisible(true);

            if (app.isMobileMode())
            {
                // In mobile mode the cart is a view and not a popup
                if (popupHelper.returnToCart)
                {
                    // Show the view
                    guiHelper.isViewVisible(true);
                    guiHelper.isInnerMenuVisible(false);
                }
                else
                {
                    // Show the menu
                    guiHelper.isViewVisible(false);
                    guiHelper.isInnerMenuVisible(true);
                }
            }
        }

        // Let knockout do its thing
        setTimeout
        (
            function ()
            {
                guiHelper.resize();
            },
            0
        );
    },
    popupToppingChanged: function (singleOrDouble, topping)
    {
        // Both double and single cannot be ticked:

        // If single was ticked and double is already ticked then untick double
        if (singleOrDouble == 'single' && topping.selectedSingle())
        {
            topping.selectedDouble(false);
        }
        // If double was ticked and single is already ticked then untick single
        else if (singleOrDouble == 'double' && topping.selectedSingle())
        {
            topping.selectedSingle(false);
        }

        if (topping.type == 'removable')
        {
            // Is the topping removed?
            if (!topping.selectedDouble() && !topping.selectedSingle())
            {
                topping.quantity = -1;
            }
                // Is the topping being doubled?
            else if (topping.selectedDouble())
            {
                // This topping is already on the item so doubling up just adds a single topping
                topping.quantity = 1;
            }
        }
        else
        {
            if (topping.selectedSingle())
            {
                topping.quantity = 1;
            }
            else if (topping.selectedDouble())
            {
                topping.quantity = 2;
            }
        }

        // Recalculate the item price
        var price = 0;
        if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
        {
            price = app.menu.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings(), true);
        }
        else
        {
            price = app.menu.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings(), true, true);
        }

        viewModel.selectedItem.price(helper.formatPrice(price));

        // Refresh free toppings
        popupHelper.refeshFreeToppings();
    },
    refeshFreeToppings: function()
    {
        var remainingToppings = 0;

        if (viewModel.selectedItem.freeToppings() > 0)
        {
            // Does the item have any toppings?
            if (viewModel.selectedItem.toppings() != undefined)
            {
                var usedToppings = 0;

                for (var index = 0; index < viewModel.selectedItem.toppings().length; index++)
                {
                    var topping = viewModel.selectedItem.toppings()[index];

                    if (topping.type == 'removable')
                    {
                        // The customer has upgraded an included topping toa double
                        if (topping.selectedDouble())
                        {
                            usedToppings++;
                        }
                    }
                    else
                    {
                        if (topping.selectedDouble())
                        {
                            // Customer has added 2 of the topping
                            usedToppings+=2;
                        }
                        else if (topping.selectedSingle())
                        {
                            // Customer has a topping
                            usedToppings++;
                        }
                    }
                }

                remainingToppings = viewModel.selectedItem.freeToppings() - usedToppings;

                if (remainingToppings < 0) remainingToppings = 0;
            }
        }

        // Update UI
        viewModel.selectedItem.freeToppingsRemaining(remainingToppings);
    },
    popupCategory1Changed: function ()
    {
        if (viewModel.pickToppings())
        {
            viewModel.category1Changed(viewModel.selectedItem);

            // Get the menu item that the user has selected
            var menuItem = app.menu.getSelectedMenuItem(viewModel.selectedItem);

            if (menuItem == undefined) return;

            // Change the selected menu item
            viewModel.selectedItem.menuItem(menuItem);

            // Recalculate the item price
            var price = 0;
            if (popupHelper.mode() == 'addDealItem' || popupHelper.mode() == 'editDealItem')
            {
                price = app.menu.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings());
            }
            else
            {
                price = app.menu.calculateItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.quantity(), viewModel.selectedItem.toppings());
            }

            viewModel.selectedItem.price(helper.formatPrice(price));
        }
    },
    popupSelectedItemChanged: function ()
    {
        if (viewModel.pickToppings())
        {
            // Get the menu item that the user has selected
            var menuItem = app.menu.getSelectedMenuItem(viewModel.selectedItem);

            // Change the selected menu item
            viewModel.selectedItem.menuItem(menuItem);

            if (popupHelper.mode() == 'addItem' || popupHelper.mode() == 'editItem')
            {
                guiHelper.selectedItemChanged(viewModel.selectedItem);
            }
            else
            {
                // Recalculate the item price
                var price = app.menu.calculateDealItemPrice(viewModel.selectedItem.menuItem(), viewModel.selectedItem.toppings());

                viewModel.selectedItem.price(helper.formatPrice(price));
            }
        }
    }
}

var dealPopupHelper = 
{
    isBackgroundVisible: ko.observable(false),
    isPopupVisible: ko.observable(false),
    showDealPopup: function (mode, scrollTop)
    {
        if (scrollTop)
        {
            $(window).scrollTop(0);
        }

        if (app.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            guiHelper.isViewVisible(false);
            guiHelper.isInnerMenuVisible(false);
        }

        dealHelper.mode(mode);

        // Make the popup visible
        dealPopupHelper.isBackgroundVisible(true);
        dealPopupHelper.isPopupVisible(true);
    },
    hideDealPopup: function ()
    {
        // Make the popup visible
        dealPopupHelper.isBackgroundVisible(false);
        dealPopupHelper.isPopupVisible(false);

        if (app.isMobileMode())
        {
            // In mobile mode the cart is a view and not a popup
            if (dealPopupHelper.returnToCart)
            {
                // Show the view
                guiHelper.isViewVisible(true);
                guiHelper.isInnerMenuVisible(false);
            }
            else
            {
                // Show the menu
                guiHelper.isViewVisible(false);
                guiHelper.isInnerMenuVisible(true);
            }
        }

        // Let knockout do its thing
        setTimeout
        (
            function ()
            {
                guiHelper.resize();
            },
            0
        );
    },
}

var tandcHelper = 
{
    isPopupVisible: ko.observable(false),
    hasAgreed: function ()
    {
        if (tandcHelper.agree() == undefined)
        {
            tandcHelper.agree(false);
        }

        return tandcHelper.agree();
    },
    agree: ko.observable(undefined),
    showPopup: function ()
    {
        if (app.isMobileMode())
        {
            guiHelper.isMobileMenuVisible(false);
            checkoutMenuHelper.showMenu('pay', false);
            app.viewEngine.showView('tandcView');
        }
        else
        {
            $(window).scrollTop(0);

            // Make the popup visible
            popupHelper.isBackgroundVisible(true);
            tandcHelper.isPopupVisible(true);
        }

        // Switch the cart to checkout mode
        guiHelper.canChangeOrderType(false);
        guiHelper.cartActions(guiHelper.cartActionsCheckout);

        guiHelper.canChangeOrderType(false);
    },
    hidePopup: function (callback)
    {
        if (app.isMobileMode())
        {
            checkoutHelper.showPaymentPicker();
        }
        else
        {
            popupHelper.isBackgroundVisible(false);
            tandcHelper.isPopupVisible(false);
        }
    }
}

var mapHelper = 
{
    map: undefined,
    storeMarker: undefined,
    markersLayer: undefined,
    urls:
    [
        "http://a.tile.openstreetmap.org/${z}/${x}/${y}.png",
        "http://b.tile.openstreetmap.org/${z}/${x}/${y}.png",
        "http://c.tile.openstreetmap.org/${z}/${x}/${y}.png"
    ],
    initialiseMap: function()
    {
        try
        {
            mapHelper.map = new OpenLayers.Map
            (
                {
                    div: "map",
                    layers: 
                    [
                        new OpenLayers.Layer.XYZ
                        (
                            "OSM (with buffer)", 
                            mapHelper.urls, 
                            {
                                transitionEffect: "resize", 
                                buffer: 2, sphericalMercator: true,
                                attribution: ""
                            }
                        ),
                        new OpenLayers.Layer.XYZ
                        (
                            "OSM (without buffer)", 
                            mapHelper.urls, 
                            {
                                transitionEffect: "resize", 
                                buffer: 0, 
                                sphericalMercator: true,
                                attribution: ""
                            }
                        )
                    ],
                    controls: 
                    [
                        new OpenLayers.Control.Navigation
                        (
                            {
                                dragPanOptions: { enableKinetic: true }
                            }
                        ),
                        new OpenLayers.Control.PanZoom(),
                        new OpenLayers.Control.Attribution()
                    ],
                    center: [0, 0],
                    zoom: 3
                }            
            );

            var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
            var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection

            // allow testing of specific renderers via "?renderer=Canvas", etc
            var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
            renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;

            var wgs84 = new OpenLayers.Projection("EPSG:4326");
            mapHelper.markersLayer = new OpenLayers.Layer.Vector
            (
                "Markers", 
                {
                    /*renderers: renderer*/
                    styleMap: new OpenLayers.StyleMap
                    (
                        {
                            externalGraphic: template_mapMarker,
                            graphicOpacity: 1.0,
                            graphicWidth: 32,
                            graphicHeight: 37,
                            graphicYOffset: -37
                        }
                    ),
                    projection: wgs84
                }
            );
            window.mapped="yes";

            mapHelper.map.addLayer(mapHelper.markersLayer);

            var longitude = 0;

            if (app.site().siteDetails().address["long"] == undefined)
            {
                longitude = app.site().siteDetails().address.longitude;
            }
            else
            {
                longitude = app.site().siteDetails().address["long"];
            }

            var latitude = app.site().siteDetails().address.lat == undefined ? app.site().siteDetails().address.latitude : app.site().siteDetails().address.lat;

            mapHelper.setStoreMarker(longitude, latitude);

            var position = new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection);

            mapHelper.map.setCenter(position, 17);
        }
        catch(exception) {}
    },
    setStoreMarker: function(longitude, latitude)
    {
        // Is there already a store feature on the map?
        if (typeof(mapHelper.storeMarker) == 'object')
        {
            // Remove the store feature
            mapHelper.markersLayer.removeAllFeatures();
            mapHelper.markersLayer.destroyFeatures();
            mapHelper.storeMarker = undefined;
        }

        var fromProjection = new OpenLayers.Projection("EPSG:4326"); // Transform from WGS 1984
        var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection

        var lonLat = new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection);

        // Create a new feature
        mapHelper.storeMarker = 
        {
            "type" : "Feature",
            "geometry" : 
            {
                "type": "Point", 
                "coordinates": [ lonLat.lon, lonLat.lat ]
            }
        };

        // Add the feature to the map
        var features = 
        {
            "type": "FeatureCollection",
            "features": [ mapHelper.storeMarker ]
        };

        var reader = new OpenLayers.Format.GeoJSON();

        var locator = reader.read(features);

        mapHelper.markersLayer.addFeatures(locator);

        // Center the map on the feature
        var latLonBounds = new OpenLayers.Bounds();

        latLonBounds.extend(new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection));
    }
}

var checkoutMenuHelper = 
{
    showMenu: function (backTo, isPlaceOrderEnabled)
    {
        if (app.isMobileMode())
        {
            checkoutMenuHelper.isVisible(true);
            checkoutMenuHelper.backTo(backTo);
            checkoutMenuHelper.isPlaceOrderEnabled(isPlaceOrderEnabled);
        }
    },
    hideMenu: function()
    {
        if (app.isMobileMode())
        {
            checkoutMenuHelper.isVisible(false);
        }
    },
    isVisible: ko.observable(false),
    backTo: ko.observable('menu'),
    isPlaceOrderEnabled: ko.observable(true)
}

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
        if (app.isMobileMode())
        {
            accountHelper.loggedInCallback = loggedInCallback;

            app.viewEngine.showView('loginRegisterView');

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

            if (app.isMobileMode())
            {
                // In mobile mode the cart is a view and not a popup
                guiHelper.isViewVisible(false);
                guiHelper.isInnerMenuVisible(false);
            }
        }
    },
    hidePopup: function (callback)
    {
        if (!app.isMobileMode())
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

        acsApi.getCustomer
        (
            email,
            password,
            function (getCustomerResponse)
            {
                if (getCustomerResponse != undefined && getCustomerResponse.errorCode != undefined)
                {
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
                else
                {
                    accountHelper.isLoggedIn(true);
                    accountHelper.customerDetails = getCustomerResponse;
                    accountHelper.emailAddress = email;
                    accountHelper.password = password;
                    accountHelper.hidePopup(accountHelper.loggedInCallback);
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

                        apiHelper.getCustomer
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
                                            apiHelper.putCustomer
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
        acsApi.putCustomer
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
        if (app.isMobileMode())
        {
            accountHelper.hidePopup(guiHelper.showCart);
        }
        else
        {
            accountHelper.hidePopup();
        }
    }
}