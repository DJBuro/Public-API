
/* >>> refreshTimes tests <<< */
module("refreshTimes tests", { setup: setup });

test
(
    "test1",
    function ()
    {
        // Setup
        textStrings = englishTextStrings;

        viewModel.selectedSite = ko.observable
        (
            {
                estDelivTime: 30
            }
        );

        viewModel.siteDetails = ko.observable
        (
            {
                openingHours:
                [
                    {
                        day: "Thursday",
                        openAllDay: false,
                        startTime: '17:00',
                        endTime: '02:00'
                    }
                ]
            }
        );

        openingTimesHelper.getToday();
        openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.monday, 'Monday');
        openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.tuesday, 'Tuesday');
        openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.wednesday, 'Wednesday');
        openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.thursday, 'Thursday');
        openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.friday, 'Friday');
        openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.saturday, 'Saturday');
        openingTimesHelper.getOpeningHours(openingTimesHelper.openingTimes.sunday, 'Sunday');

        checkoutHelper.refreshTimes();

        // The test
        //        var amount = AndroWeb.Helpers.CartHelper.getOrderValueExcludingDealsWithAMinimumPrice(500);
//        equal(amount, 8010, "Expected 8010 but got " + amount);
    }
);
