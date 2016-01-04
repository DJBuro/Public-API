/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function site()
{
    var self = this;

    self.menu = new menu();
    self.cart = new cart();
    self.isTakingOrders = ko.observable(false);
    self.siteDetails = ko.observable(); // The selected site (from the site details web service call)
    self.displayAddress = ko.observable(); // The stores address for display purposes
    self.displayAddressMultiLine = ko.observable(); // The stores address for display purposes
    self.selectedSite = ko.observable(); // The selected site (from the site list web service call)
    self.addressType = ko.observable(); // Address format to use - based on country
    self.openingTimes =
    {
        monday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        tuesday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        wednesday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        thursday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        friday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        saturday:
        {
            openingTimes: [],
            displayText: ko.observable()
        },
        sunday:
        {
            openingTimes: [],
            displayText: ko.observable()
        }
    };
    self.todayPropertyName = '';
    self.getToday = function ()
    {
        // Day of the week for getting the available times from a menu item or deal
        var today = new Date();
        var dayOfWeek = today.getDay();

        switch (dayOfWeek)
        {
            case 0:
                self.todayPropertyName = 'Sun';
                break;
            case 1:
                self.todayPropertyName = 'Mon';
                break;
            case 2:
                self.todayPropertyName = 'Tue';
                break;
            case 3:
                self.todayPropertyName = 'Wed';
                break;
            case 4:
                self.todayPropertyName = 'Thu';
                break;
            case 5:
                self.todayPropertyName = 'Fri';
                break;
            case 6:
                self.todayPropertyName = 'Sat';
                break;
        }
    };
    self.getOpeningHours = function (openingTimes, day)
    {
        // Get the opening times
        for (var index = 0; index < self.siteDetails().openingHours.length; index++)
        {
            var timeSpan = self.siteDetails().openingHours[index];

            if (timeSpan.day == day)
            {
                openingTimes.openingTimes.push(timeSpan);
            }
        }

        // Sort the opening times
        openingTimes.openingTimes.sort(function (a, b) { return a.startTime > b.startTime ? 1 : -1 });

        // Generate a nicely formatted list of opening times to display on the page
        var displayText = '';

        for (var index = 0; index < openingTimes.openingTimes.length; index++)
        {
            var timeSpan = openingTimes.openingTimes[index];

            if (timeSpan.openAllDay)
            {
                displayText = text_openAllDay;
                break;
            }
            else
            {
                displayText += text_fromTo.replace('{from}', timeSpan.startTime).replace('{to}', timeSpan.endTime) + '<br/>';
            }
        }

        if (displayText.length == 0)
        {
            openingTimes.displayText(text_closed);
        }
        else
        {
            openingTimes.displayText(displayText);
        }
    };
    self.getTodaysOpeningTimes = function ()
    {
        var today = new Date();
        var dayOfWeek = today.getDay();

        // Get ordering times
        var openingTimes;
        switch (dayOfWeek)
        {
            case 0:
                openingTimes = self.openingTimes.sunday.openingTimes; //alert('sunday');
                break;
            case 1:
                openingTimes = self.openingTimes.monday.openingTimes; //alert('monday');
                break;
            case 2:
                openingTimes = self.openingTimes.tuesday.openingTimes; //alert('tuesday');
                break;
            case 3:
                openingTimes = self.openingTimes.wednesday.openingTimes; //alert('wednesday');
                break;
            case 4:
                openingTimes = self.openingTimes.thursday.openingTimes; //alert('thursday');
                break;
            case 5:
                openingTimes = self.openingTimes.friday.openingTimes; //alert('friday');
                break;
            case 6:
                openingTimes = self.openingTimes.saturday.openingTimes; //alert('saturday');
                break;
        }

        return openingTimes;
    };
    self.deliveryZones = ko.observable(undefined);
    self.isInDeliveryZone = function (targetDeliveryZone)
    {
        var isInDeliveryZone = false;

        if (self.deliveryZones() != undefined)
        {
            // Remove all white space from the postcode
            var cleanedUpZipCode = targetDeliveryZone.replace(/\s+/g, '');
            if (cleanedUpZipCode.length > 4) cleanedUpZipCode = cleanedUpZipCode.substring(0, 4);

            if (cleanedUpZipCode.length == 4)
            {
                for (var index = 0; index < self.deliveryZones().length; index++)
                {
                    var possibleDeliveryZone = self.deliveryZones()[index];

                    if (possibleDeliveryZone.toUpperCase() == cleanedUpZipCode.toUpperCase())
                    {
                        isInDeliveryZone = true;
                    }
                }
            }
        }

        return isInDeliveryZone
    };
    self.etd = function ()
    {
        var etd = template_defaultETD;
        if (app.site().estDelivTime != undefined && app.site().estDelivTime > 0)
        {
            etd = app.site().estDelivTime;
        }

        return etd;
    };
};