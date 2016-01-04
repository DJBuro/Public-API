/// <reference path="../../Content/Scripts.js" />

/* >>> getAvailableTimes tests <<< */
module("getAvailableTimes tests", { setup: setup });

test
(
    "available all day",
    function ()
    {
        // Setup
        var availableTimes =
        [
            {
                NotAvailableToday: true
            }
        ];

        // The test
        var result = menuHelper.getAvailableTimes(availableTimes);

        // Check the result
        equal(result.notAvailableToday, true, "Expected notAvailableToday to be true but got false");
        equal(result.availableAllDay, false, "Expected notAvailableToday to be false but got true");
        equal(result.displayText, "", "Expected displayText to be blank but got " + result.displayText);
        equal(result.displayText, "", "Expected displayText to be blank but got " + result.displayText);
    }
);

test
(
    "available at lunchtime",
    function ()
    {
        // Setup
        var availableTimes =
        [
            {
                NotAvailableToday: false,
                From: '11:00 AM',
                To: '2:00 PM'
            }
        ];

        // The test
        var result = menuHelper.getAvailableTimes(availableTimes);

        // Check the result
        equal(result.notAvailableToday, false, "Expected notAvailableToday to be false but got true");
        equal(result.availableAllDay, false, "Expected notAvailableToday to be true but got false");

        var expectedDisplayText = "11:00 a.m. to 2:00 p.m.";
        equal(result.displayText, expectedDisplayText, "Expected displayText to be \"" + expectedDisplayText + "\" but got " + result.displayText);
    }
);

test
(
    "available over midnight",
    function ()
    {
        // Setup
        var availableTimes =
        [
            {
                NotAvailableToday: false,
                From: '7:25 PM',
                To: '2:13 AM'
            }
        ];

        // The test
        var result = menuHelper.getAvailableTimes(availableTimes);

        // Check the result
        equal(result.notAvailableToday, false, "Expected notAvailableToday to be false but got true");
        equal(result.availableAllDay, false, "Expected notAvailableToday to be true but got false");

        var expectedDisplayText = "7:25 p.m. to 2:13 a.m.";
        equal(result.displayText, expectedDisplayText, "Expected displayText to be \"" + expectedDisplayText + "\" but got " + result.displayText);
    }
);




//removeUnavailableDeliverySlots: function(timeSlots, cartItems)



