/// <reference path="../../Content/Scripts.js" />

/* >>> removeUnavailableDeliverySlots tests <<< */
module("removeUnavailableDeliverySlots tests", { setup: setup });

var removeUnavailableDeliverySlots_lunchTimeSlots = function ()
{
    var times = [];

    times.push
    (
        {
            mode: "TIMED",
            startHour: 11,
            startMinute: 00,
            endHour: 11,
            endMinute: 15,
            text: "11:00 a.m. - 11:15 a.m.",
            time: "11:00"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 11,
            startMinute: 15,
            endHour: 11,
            endMinute: 30,
            text: "11:15 a.m. - 11:30 a.m.",
            time: "11:15"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 11,
            startMinute: 30,
            endHour: 11,
            endMinute: 45,
            text: "11:30 a.m. - 11:45 a.m.",
            time: "11:30"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 11,
            startMinute: 45,
            endHour: 12,
            endMinute: 00,
            text: "11:45 a.m. - 12:00 a.m.",
            time: "11:45"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 12,
            startMinute: 00,
            endHour: 12,
            endMinute: 15,
            text: "12:00 a.m. - 12:15 a.m.",
            time: "12:00"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 12,
            startMinute: 15,
            endHour: 12,
            endMinute: 30,
            text: "12:15 a.m. - 12:30 a.m.",
            time: "12:15"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 12,
            startMinute: 30,
            endHour: 12,
            endMinute: 45,
            text: "12:30 a.m. - 12:45 a.m.",
            time: "12:30"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 12,
            startMinute: 45,
            endHour: 13,
            endMinute: 0,
            text: "12:45 a.m. - 13:00 a.m.",
            time: "12:45"
        }
    );

    return times;
};

var removeUnavailableDeliverySlots_midnightTimeSlots = function ()
{
    var times = [];

    times.push
    (
        {
            mode: "TIMED",
            startHour: 23,
            startMinute: 30,
            endHour: 23,
            endMinute: 45,
            text: "23:30 a.m. - 23:45 a.m.",
            time: "23:30"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 23,
            startMinute: 45,
            endHour: 0,
            endMinute: 0,
            text: "23:45 p.m. - 00:00 a.m.",
            time: "23:45"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 0,
            startMinute: 0,
            endHour: 0,
            endMinute: 15,
            text: "00:00 a.m. - 00:15 a.m.",
            time: "00:00"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 0,
            startMinute: 15,
            endHour: 0,
            endMinute: 30,
            text: "00:15 a.m. - 00:30 a.m.",
            time: "00:15"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 0,
            startMinute: 30,
            endHour: 0,
            endMinute: 45,
            text: "00:30 a.m. - 00:45 a.m.",
            time: "00:30"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 0,
            startMinute: 45,
            endHour: 1,
            endMinute: 0,
            text: "00:45 a.m. - 1:00 a.m.",
            time: "00:45"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 1,
            startMinute: 0,
            endHour: 1,
            endMinute: 15,
            text: "1:00 a.m. - 1:15 a.m.",
            time: "1:00"
        }
    );

    times.push
    (
        {
            mode: "TIMED",
            startHour: 1,
            startMinute: 15,
            endHour: 1,
            endMinute: 30,
            text: "1:15 a.m. - 1:30 a.m.",
            time: "1:15"
        }
    );

    return times;
};

test
(
    "no available times",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_lunchTimeSlots();
        var cartItems =
        [
            {}
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 8, "Expected 8 times but got " + times.length);
    }
);

test
(
    "availableAllDay true",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_lunchTimeSlots();
        var cartItems =
        [
            {
                availableTimes:
                {
                    availableAllDay: true
                },
                removedTimeSlot: ko.observable(false)
            }
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 8, "Expected 8 times but got " + times.length);
    }
);

test
(
    "timeslot is within all availability times",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_lunchTimeSlots();
        var cartItems =
        [
            {
                availableTimes:
                {
                    availableAllDay: false,
                    displayText: '10:00 PM to 14:00 AM',
                    times:
                    [
                        {
                            from: '10:00',
                            to: '14:00'
                        }
                    ]
                },
                removedTimeSlot: ko.observable(false)
            }
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 8, "Expected 8 times but got " + times.length);
    }
);

test
(
    "item is available for all timeslots",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_lunchTimeSlots();
        var cartItems =
        [
            {
                availableTimes:
                {
                    availableAllDay: false,
                    displayText: '10:30 PM to 14:00 AM',
                    times:
                    [
                        {
                            from: '10:30',
                            to: '14:00'
                        }
                    ]
                },
                removedTimeSlot: ko.observable(false)
            }
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 8, "Expected 8 times but got " + times.length);
    }
);

test
(
    "item is available for first half of timeslots",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_lunchTimeSlots();
        var cartItems =
        [
            {
                availableTimes:
                {
                    availableAllDay: false,
                    displayText: '11:50 PM to 14:00 AM',
                    times:
                    [
                        {
                            from: '11:50',
                            to: '14:00'
                        }
                    ]
                },
                removedTimeSlot: ko.observable(false)
            }
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 4, "Expected 4 times but got " + times.length);

        equal(times[0].startHour, 12, "Expected first timeslot hour to be 12 but got " + times[0].startHour);
        equal(times[0].startMinute, 0, "Expected first timeslot minute to be 0 but got " + times[0].startMinute);

        equal(times[1].startHour, 12, "Expected first timeslot hour to be 12 but got " + times[1].startHour);
        equal(times[1].startMinute, 15, "Expected first timeslot minute to be 15 but got " + times[1].startMinute);

        equal(times[2].startHour, 12, "Expected first timeslot hour to be 12 but got " + times[2].startHour);
        equal(times[2].startMinute, 30, "Expected first timeslot minute to be 30 but got " + times[2].startMinute);

        equal(times[3].startHour, 12, "Expected first timeslot hour to be 12 but got " + times[3].startHour);
        equal(times[3].startMinute, 45, "Expected first timeslot minute to be 45 but got " + times[3].startMinute);
    }
);

test
(
    "all timeslots inside available times spanning midnight",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_midnightTimeSlots();
        var cartItems =
        [
            {
                availableTimes:
                {
                    availableAllDay: false,
                    displayText: '10:00 PM to 4:00 AM',
                    times:
                    [
                        {
                            from: '20:00',
                            to: '4:00'
                        }
                    ]
                },
                removedTimeSlot: ko.observable(false)
            }
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 8, "Expected 8 times but got " + times.length);
    }
);

test
(
    "half of the timeslots inside available times spanning midnight",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_midnightTimeSlots();
        var cartItems =
        [
            {
                availableTimes:
                {
                    availableAllDay: false,
                    displayText: '11:35 PM to 1:05 AM',
                    times:
                    [
                        {
                            from: '23:35',
                            to: '1:05'
                        }
                    ]
                },
                removedTimeSlot: ko.observable(false)
            }
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 6, "Expected 6 times but got " + times.length);

        equal(times[0].startHour, 23, "Expected first timeslot hour to be 23 but got " + times[0].startHour);
        equal(times[0].startMinute, 45, "Expected first timeslot minute to be 45 but got " + times[0].startMinute);

        equal(times[1].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[1].startHour);
        equal(times[1].startMinute, 0, "Expected first timeslot minute to be 0 but got " + times[1].startMinute);

        equal(times[2].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[2].startHour);
        equal(times[2].startMinute, 15, "Expected first timeslot minute to be 15 but got " + times[2].startMinute);

        equal(times[3].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[3].startHour);
        equal(times[3].startMinute, 30, "Expected first timeslot minute to be 30 but got " + times[3].startMinute);

        equal(times[4].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[4].startHour);
        equal(times[4].startMinute, 45, "Expected first timeslot minute to be 45 but got " + times[4].startMinute);

        equal(times[5].startHour, 1, "Expected first timeslot hour to be 1 but got " + times[5].startHour);
        equal(times[5].startMinute, 0, "Expected first timeslot minute to be 0 but got " + times[5].startMinute);
    }
);

test
(
    "mixed - half of the timeslots inside available times spanning midnight",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_midnightTimeSlots();
        var cartItems =
        [
            {
                availableTimes:
                {
                    availableAllDay: false,
                    displayText: '11:35 PM to 1:05 AM',
                    times:
                    [
                        {
                            from: '23:35',
                            to: '1:05'
                        }
                    ]
                },
                removedTimeSlot: ko.observable(false)
            },
            {
                availableTimes:
                {
                    availableAllDay: true,
                    displayText: '',
                    times:
                    [
                    ]
                },
                removedTimeSlot: ko.observable(false)
            }
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 6, "Expected 6 times but got " + times.length);

        equal(times[0].startHour, 23, "Expected first timeslot hour to be 23 but got " + times[0].startHour);
        equal(times[0].startMinute, 45, "Expected first timeslot minute to be 45 but got " + times[0].startMinute);

        equal(times[1].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[1].startHour);
        equal(times[1].startMinute, 0, "Expected first timeslot minute to be 0 but got " + times[1].startMinute);

        equal(times[2].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[2].startHour);
        equal(times[2].startMinute, 15, "Expected first timeslot minute to be 15 but got " + times[2].startMinute);

        equal(times[3].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[3].startHour);
        equal(times[3].startMinute, 30, "Expected first timeslot minute to be 30 but got " + times[3].startMinute);

        equal(times[4].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[4].startHour);
        equal(times[4].startMinute, 45, "Expected first timeslot minute to be 45 but got " + times[4].startMinute);

        equal(times[5].startHour, 1, "Expected first timeslot hour to be 1 but got " + times[5].startHour);
        equal(times[5].startMinute, 0, "Expected first timeslot minute to be 0 but got " + times[5].startMinute);
    }
);

test
(
    "mixed 2 - half of the timeslots inside available times spanning midnight",
    function ()
    {
        // Setup
        var times = removeUnavailableDeliverySlots_midnightTimeSlots();
        var cartItems =
        [
            {
                availableTimes:
                {
                    availableAllDay: true,
                    displayText: '',
                    times:
                    [
                    ]
                },
                removedTimeSlot: ko.observable(false)
            },
            {
                availableTimes:
                {
                    availableAllDay: false,
                    displayText: '11:35 PM to 1:05 AM',
                    times:
                    [
                        {
                            from: '23:35',
                            to: '1:05'
                        }
                    ]
                },
                removedTimeSlot: ko.observable(false)
            },
            {
                availableTimes:
                {
                    availableAllDay: true,
                    displayText: '',
                    times:
                    [
                    ]
                },
                removedTimeSlot: ko.observable(false)
            }
        ];

        // The test
        var result = checkoutHelper.removeUnavailableDeliverySlots
        (
            times,
            cartItems
        );

        // Check the result
        equal(times.length, 6, "Expected 6 times but got " + times.length);

        equal(times[0].startHour, 23, "Expected first timeslot hour to be 23 but got " + times[0].startHour);
        equal(times[0].startMinute, 45, "Expected first timeslot minute to be 45 but got " + times[0].startMinute);

        equal(times[1].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[1].startHour);
        equal(times[1].startMinute, 0, "Expected first timeslot minute to be 0 but got " + times[1].startMinute);

        equal(times[2].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[2].startHour);
        equal(times[2].startMinute, 15, "Expected first timeslot minute to be 15 but got " + times[2].startMinute);

        equal(times[3].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[3].startHour);
        equal(times[3].startMinute, 30, "Expected first timeslot minute to be 30 but got " + times[3].startMinute);

        equal(times[4].startHour, 0, "Expected first timeslot hour to be 0 but got " + times[4].startHour);
        equal(times[4].startMinute, 45, "Expected first timeslot minute to be 45 but got " + times[4].startMinute);

        equal(times[5].startHour, 1, "Expected first timeslot hour to be 1 but got " + times[5].startHour);
        equal(times[5].startMinute, 0, "Expected first timeslot minute to be 0 but got " + times[5].startMinute);
    }
);
