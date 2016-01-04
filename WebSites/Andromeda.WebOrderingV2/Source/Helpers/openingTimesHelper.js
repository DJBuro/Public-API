var openingTimesHelper =
{
    openingTimes:
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
    },
    todayPropertyName: '',
    getToday: function ()
    {
        // Day of the week for getting the available times from a menu item or deal
        var today = new Date();
        var dayOfWeek = today.getDay();

        switch (dayOfWeek)
        {
            case 0:
                openingTimesHelper.todayPropertyName = 'Sun';
                break;
            case 1:
                openingTimesHelper.todayPropertyName = 'Mon';
                break;
            case 2:
                openingTimesHelper.todayPropertyName = 'Tue';
                break;
            case 3:
                openingTimesHelper.todayPropertyName = 'Wed';
                break;
            case 4:
                openingTimesHelper.todayPropertyName = 'Thr';
                break;
            case 5:
                openingTimesHelper.todayPropertyName = 'Fri';
                break;
            case 6:
                openingTimesHelper.todayPropertyName = 'Sat';
                break;
        }
    },
    getOpeningHours: function (openingTimes, day)
    {
        // Get the opening times
        for (var index = 0; index < viewModel.siteDetails().openingHours.length; index++)
        {
            var timeSpan = viewModel.siteDetails().openingHours[index];

            if (timeSpan.day == day)
            {
                openingTimes.openingTimes.push(timeSpan);
            }
        }

        // Sort the opening times
        openingTimes.openingTimes.sort(function (a, b) { return a.startTime > b.startTime ? 1 : -1 });

        // Generate a nicely formatted list of opening times to display on the page
        var displayText = '';
        var structuredDataTimes = '';

        for (var index = 0; index < openingTimes.openingTimes.length; index++)
        {
            var timeSpan = openingTimes.openingTimes[index];

            if (timeSpan.openAllDay)
            {
                displayText = textStrings.openAllDay;
                break;
            }
            else
            {
                if (displayText.length > 0) displayText += '<br />';

                displayText += textStrings.fromTo.replace('{from}', timeSpan.startTime).replace('{to}', timeSpan.endTime);

                if (structuredDataTimes.length > 0) structuredDataTimes += ',';
                structuredDataTimes += timeSpan.startTime + '-' + timeSpan.endTime;
            }
        }

        if (displayText.length == 0)
        {
            openingTimes.displayText(textStrings.closed);
        }
        else
        {
            openingTimes.displayText('<time itemprop="openingHours" datetime="' + day.substring(0, 2) + (structuredDataTimes.length > 0 ? ' ' : '') + structuredDataTimes + '">' + displayText);
        }
    },
    getTodaysOpeningTimes: function ()
    {
        var today = new Date();
        var dayOfWeek = today.getDay();

        // Get ordering times
        var openingTimes;
        switch (dayOfWeek)
        {
            case 0:
                openingTimes = openingTimesHelper.openingTimes.sunday.openingTimes;
                break;
            case 1:
                openingTimes = openingTimesHelper.openingTimes.monday.openingTimes;
                break;
            case 2:
                openingTimes = openingTimesHelper.openingTimes.tuesday.openingTimes;
                break;
            case 3:
                openingTimes = openingTimesHelper.openingTimes.wednesday.openingTimes;
                break;
            case 4:
                openingTimes = openingTimesHelper.openingTimes.thursday.openingTimes;
                break;
            case 5:
                openingTimes = openingTimesHelper.openingTimes.friday.openingTimes;
                break;
            case 6:
                openingTimes = openingTimesHelper.openingTimes.saturday.openingTimes;
                break;
        }

        return openingTimes;
    }
}