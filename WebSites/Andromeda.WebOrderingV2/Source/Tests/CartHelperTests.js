
/* >>> getOrderValueExcludingDealsWithAMinimumPrice tests <<< */
module("getOrderValueExcludingDealsWithAMinimumPrice tests", { setup: setup });

test
(
    "empty basket",
    function ()
    {
        // The test
        var amount = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice();
        equal(amount, 0, "Expected 0 but got " + amount);
    }
);

test
(
    "one deal",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var dealLine =
        {
            Type: 'Fixed',
            DeliveryAmount: 2000,
            CollectionAmount: 4000,
            selectedMenuItem:
            {
                DeliveryPrice: 1000,
                CollectionPrice: 3000
            },
            dealLine: undefined
        };
        dealLine.dealLine = dealLine;

        cartHelper.cart().deals().push
        (
            {
                dealLines: ko.observableArray
                (
                    [
                        dealLine
                    ]
                ),
                deal:
                {
                    deal:
                    {
                        MinimumOrderValue: 0
                    }
                }
            }
        );

        // The test
        var amount = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice(500);
        equal(amount, 2000, "Expected 2000 but got " + amount);
    }
);

test
(
    "one available menu item x1 no toppings",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        cartHelper.cart().cartItems().push
        (
            {
                quantity: ko.observable(1),
                menuItem:
                {
                    DeliveryPrice: 1000,
                    CollectionPrice: 3000
                }
            }
        );

        // The test
        var amount = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice(500);
        equal(amount, 1000, "Expected 1000 but got " + amount);
    }
);

test
(
    "one available menu item x3 no toppings",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        cartHelper.cart().cartItems().push
        (
            {
                quantity: ko.observable(3),
                menuItem:
                {
                    DeliveryPrice: 1000,
                    CollectionPrice: 3000
                }
            }
        );

        // The test
        var amount = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice(500);
        equal(amount, 3000, "Expected 3000 but got " + amount);
    }
);

test
(
    "one available menu item x1, 1 optional topping, no free toppings",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        cartHelper.cart().cartItems().push
        (
            {
                quantity: ko.observable(1),
                menuItem:
                {
                    DeliveryPrice: 1000,
                    CollectionPrice: 3000
                },
                displayToppings: ko.observableArray
                (
                    [
                        {
                            type: 'optional',
                            selectedSingle: ko.observable(true),
                            selectedDouble: ko.observable(false),
                            freeQuantity: 0,
                            topping:
                            {
                                DelPrice: 10,
                                ColPrice: 30
                            }
                        }
                    ]
                )
            }
        );

        // The test
        var amount = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice(500);
        equal(amount, 1010, "Expected 1010 but got " + amount);
    }
);

test
(
    "one available menu item x1, 1 optional topping, no free toppings & one deal",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        cartHelper.cart().cartItems().push
        (
            {
                quantity: ko.observable(1),
                menuItem:
                {
                    DeliveryPrice: 6000,
                    CollectionPrice: 7000
                },
                displayToppings: ko.observableArray
                (
                    [
                        {
                            type: 'optional',
                            selectedSingle: ko.observable(true),
                            selectedDouble: ko.observable(false),
                            freeQuantity: 0,
                            topping:
                            {
                                DelPrice: 10,
                                ColPrice: 30
                            }
                        }
                    ]
                )
            }
        );

        var dealLine =
        {
            Type: 'Fixed',
            DeliveryAmount: 2000,
            CollectionAmount: 4000,
            selectedMenuItem:
            {
                DeliveryPrice: 1000,
                CollectionPrice: 3000
            },
            dealLine: undefined
        };
        dealLine.dealLine = dealLine;

        cartHelper.cart().deals().push
        (
            {
                dealLines: ko.observableArray
                (
                    [
                        dealLine
                    ]
                ),
                deal:
                {
                    deal:
                    {
                        MinimumOrderValue: 0
                    }
                }
            }
        );

        // The test
        var amount = cartHelper.getOrderValueExcludingDealsWithAMinimumPrice(500);
        equal(amount, 8010, "Expected 8010 but got " + amount);
    }
);

/* >>> calculateFullOrderDiscount tests <<< */
module("calculateFullOrderDiscount tests", { setup: setup });

test
(
    "empty basket",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        menuHelper.fullOrderDiscountDeal =
        {
            FullOrderDiscountType: 'Percentage',
            FullOrderDiscountDeliveryAmount: 10,
            FullOrderDiscountCollectionAmount: 20
        };

        // The test
        var amount = cartHelper.calculateFullOrderDiscount(0);
        equal(amount, 0, "Expected 0 but got " + amount);
    }
);

test
(
    "delivery, percentage",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        menuHelper.fullOrderDiscountDeal =
        {
            FullOrderDiscountType: 'Percentage',
            FullOrderDiscountDeliveryAmount: 10,
            FullOrderDiscountCollectionAmount: 20
        };

        // The test
        var amount = cartHelper.calculateFullOrderDiscount(100);
        equal(amount, 90, "Expected 90 but got " + amount);
    }
);

test
(
    "collection, percentage",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        menuHelper.fullOrderDiscountDeal =
        {
            FullOrderDiscountType: 'Percentage',
            FullOrderDiscountDeliveryAmount: 10,
            FullOrderDiscountCollectionAmount: 20
        };

        // The test
        var amount = cartHelper.calculateFullOrderDiscount(100);
        equal(amount, 80, "Expected 80 but got " + amount);
    }
);

test
(
    "delivery, discount",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        menuHelper.fullOrderDiscountDeal =
        {
            FullOrderDiscountType: 'Discount',
            FullOrderDiscountDeliveryAmount: 15,
            FullOrderDiscountCollectionAmount: 25
        };

        // The test
        var amount = cartHelper.calculateFullOrderDiscount(100);
        equal(amount, 85, "Expected 85 but got " + amount);
    }
);

test
(
    "collection, discount",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        menuHelper.fullOrderDiscountDeal =
        {
            FullOrderDiscountType: 'Discount',
            FullOrderDiscountDeliveryAmount: 15,
            FullOrderDiscountCollectionAmount: 25
        };

        // The test
        var amount = cartHelper.calculateFullOrderDiscount(100);
        equal(amount, 75, "Expected 75 but got " + amount);
    }
);