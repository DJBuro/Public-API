/// <reference path="../../Content/Scripts.js" />

/* >>> calculateFullOrderDiscount tests <<< */
module("calculateFullOrderDiscount tests", { setup: setup });

test
(
    "delivery percentage",
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
        var amount = cartHelper.calculateFullOrderDiscount(500);
        equal(amount, 450, "Expected 450 but got " + amount);
    }
);

test
(
    "collection percentage",
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
        var amount = cartHelper.calculateFullOrderDiscount(500);
        equal(amount, 400, "Expected 400 but got " + amount);
    }
);

test
(
    "delivery discount",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        menuHelper.fullOrderDiscountDeal =
        {
            FullOrderDiscountType: 'Discount',
            FullOrderDiscountDeliveryAmount: 10,
            FullOrderDiscountCollectionAmount: 20
        };

        // The test
        var amount = cartHelper.calculateFullOrderDiscount(500);
        equal(amount, 490, "Expected 490 but got " + amount);
    }
);

test
(
    "collection discount",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        menuHelper.fullOrderDiscountDeal =
        {
            FullOrderDiscountType: 'Discount',
            FullOrderDiscountDeliveryAmount: 10,
            FullOrderDiscountCollectionAmount: 20
        };

        // The test
        var amount = cartHelper.calculateFullOrderDiscount(500);
        equal(amount, 480, "Expected 480 but got " + amount);
    }
);

/* >>> getItemPrice <<< */
module("getItemPrice tests", { setup: setup });

test
(
    "collection old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var menuItem =
        {
            DelPrice: 10,
            ColPrice: 30
        };

        // The test
        var amount = menuHelper.getItemPrice(menuItem);
        equal(amount, 30, "Expected 30 but got " + amount);
    }
);

test
(
    "delivery old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var menuItem =
        {
            DelPrice: 10,
            ColPrice: 30
        };

        // The test
        var amount = menuHelper.getItemPrice(menuItem);
        equal(amount, 10, "Expected 10 but got " + amount);
    }
);

test
(
    "collection new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var menuItem =
        {
            DeliveryPrice: 20,
            CollectionPrice: 40
        };

        // The test
        var amount = menuHelper.getItemPrice(menuItem);
        equal(amount, 40, "Expected 40 but got " + amount);
    }
);

test
(
    "delivery new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var menuItem =
        {
            DeliveryPrice: 20,
            CollectionPrice: 40
        };

        // The test
        var amount = menuHelper.getItemPrice(menuItem);
        equal(amount, 20, "Expected 20 but got " + amount);
    }
);

/* >>> getDealLinePrice <<< */
module("getDealLinePrice tests", { setup: setup });

test
(
    "collection old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var dealLine =
        {
            dealLine: 
            {
                DelAmount: 10,
                ColAmount: 30
            }
        };

        // The test
        var amount = menuHelper.getDealLinePrice(dealLine);
        equal(amount, 30, "Expected 30 but got " + amount);
    }
);

test
(
    "delivery old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var dealLine =
        {
            dealLine: 
            {
                DelAmount: 10,
                ColAmount: 30
            }
        };

        // The test
        var amount = menuHelper.getDealLinePrice(dealLine);
        equal(amount, 10, "Expected 10 but got " + amount);
    }
);

test
(
    "collection new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var dealLine =
        {
            dealLine: 
            {
                DeliveryAmount: 20,
                CollectionAmount: 40
            }
        };

        // The test
        var amount = menuHelper.getDealLinePrice(dealLine);
        equal(amount, 40, "Expected 40 but got " + amount);
    }
);

test
(
    "delivery new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var dealLine =
        {
            dealLine: 
            {
                DeliveryAmount: 20,
                CollectionAmount: 40
            }
        };

        // The test
        var amount = menuHelper.getDealLinePrice(dealLine);
        equal(amount, 20, "Expected 20 but got " + amount);
    }
);

/* >>> getToppingPrice <<< */
module("getToppingPrice tests", { setup: setup });

test
(
    "collection old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var topping =
        {
            DelPrice: 10,
            ColPrice: 30
        };

        // The test
        var amount = menuHelper.getToppingPrice(topping);
        equal(amount, 30, "Expected 30 but got " + amount);
    }
);

test
(
    "delivery old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var topping =
        {
            DelPrice: 10,
            ColPrice: 30
        };

        // The test
        var amount = menuHelper.getToppingPrice(topping);
        equal(amount, 10, "Expected 10 but got " + amount);
    }
);

test
(
    "collection new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var topping =
        {
            DeliveryPrice: 20,
            CollectionPrice: 40
        };

        // The test
        var amount = menuHelper.getToppingPrice(topping);
        equal(amount, 40, "Expected 40 but got " + amount);
    }
);

test
(
    "delivery new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var topping =
        {
            DeliveryPrice: 20,
            CollectionPrice: 40
        };

        // The test
        var amount = menuHelper.getToppingPrice(topping);
        equal(amount, 20, "Expected 20 but got " + amount);
    }
);

/* >>> calculateDealLinePrice tests <<< */
module("calculateDealLinePrice tests", { setup: setup });

test
(
    "Fixed, not excludeDealCalculation",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var bindableDealLine =
        {
            selectedMenuItem: 
            {
                DeliveryPrice: 5000,
                CollectionPrice: 6000
            },
            dealLine:
            {
                Type: 'Fixed',
                DeliveryAmount: 2000,
                CollectionAmount: 4000
            }
        };

        // The test
        var amount = menuHelper.calculateDealLinePrice(bindableDealLine, false);
        equal(amount, 2000, "Expected 2000 but got " + amount);
    }
);

test
(
    "Discount, not excludeDealCalculation",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var bindableDealLine =
        {
            selectedMenuItem:
            {
                DeliveryPrice: 5000,
                CollectionPrice: 6000
            },
            dealLine:
            {
                Type: 'Discount',
                DeliveryAmount: 2000,
                CollectionAmount: 4000
            }
        };

        // The test
        var amount = menuHelper.calculateDealLinePrice(bindableDealLine, false);
        equal(amount, 3000, "Expected 3000 but got " + amount);
    }
);

test
(
    "Percentage, not excludeDealCalculation",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var bindableDealLine =
        {
            selectedMenuItem:
            {
                DeliveryPrice: 5000,
                CollectionPrice: 6000
            },
            dealLine:
            {
                Type: 'Percentage',
                DeliveryAmount: 20,
                CollectionAmount: 40
            }
        };

        // The test
        var amount = menuHelper.calculateDealLinePrice(bindableDealLine, false);
        equal(amount, 1000, "Expected 1000 but got " + amount);
    }
);

test
(
    "Fixed, excludeDealCalculation",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var bindableDealLine =
        {
            selectedMenuItem:
            {
                DeliveryPrice: 5000,
                CollectionPrice: 6000
            },
            dealLine:
            {
                Type: 'Fixed',
                DeliveryAmount: 2000,
                CollectionAmount: 4000
            }
        };

        // The test
        var amount = menuHelper.calculateDealLinePrice(bindableDealLine, true);
        equal(amount, 5000, "Expected 5000 but got " + amount);
    }
);

test
(
    "Discount, excludeDealCalculation",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var bindableDealLine =
        {
            selectedMenuItem:
            {
                DeliveryPrice: 5000,
                CollectionPrice: 6000
            },
            dealLine:
            {
                Type: 'Discount',
                DeliveryAmount: 2000,
                CollectionAmount: 4000
            }
        };

        // The test
        var amount = menuHelper.calculateDealLinePrice(bindableDealLine, true);
        equal(amount, 5000, "Expected 5000 but got " + amount);
    }
);

test
(
    "Percentage, excludeDealCalculation",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var bindableDealLine =
        {
            selectedMenuItem:
            {
                DeliveryPrice: 5000,
                CollectionPrice: 6000
            },
            dealLine:
            {
                Type: 'Percentage',
                DeliveryAmount: 20,
                CollectionAmount: 40
            }
        };

        // The test
        var amount = menuHelper.calculateDealLinePrice(bindableDealLine, true);
        equal(amount, 5000, "Expected 5000 but got " + amount);
    }
);

/* >>> isItemAvailable tests <<< */
module("isItemAvailable tests", { setup: setup });

test
(
    "Not available for collection old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var menuItem =
        {
            DelPrice: 5000
        };

        // The test
        var isAvailable = menuHelper.isItemAvailable(menuItem);
        equal(isAvailable, false, "Expected false but got " + isAvailable);
    }
);

test
(
    "Not available for collection new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var menuItem =
        {
            DeliveryPrice: 6000
        };

        // The test
        var isAvailable = menuHelper.isItemAvailable(menuItem);
        equal(isAvailable, false, "Expected false but got " + isAvailable);
    }
);

test
(
    "Not available for delivery old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var menuItem =
        {
            ColPrice: 5000
        };

        // The test
        var isAvailable = menuHelper.isItemAvailable(menuItem);
        equal(isAvailable, false, "Expected false but got " + isAvailable);
    }
);

test
(
    "Not available for delivery new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var menuItem =
        {
            CollectionPrice: 6000
        };

        // The test
        var isAvailable = menuHelper.isItemAvailable(menuItem);
        equal(isAvailable, false, "Expected false but got " + isAvailable);
    }
);

test
(
    "Available for collection old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var menuItem =
        {
            ColPrice: 5000
        };

        // The test
        var isAvailable = menuHelper.isItemAvailable(menuItem);
        equal(isAvailable, true, "Expected true but got " + isAvailable);
    }
);

test
(
    "Available for collection new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var menuItem =
        {
            CollectionPrice: 6000
        };

        // The test
        var isAvailable = menuHelper.isItemAvailable(menuItem);
        equal(isAvailable, true, "Expected true but got " + isAvailable);
    }
);

test
(
    "Available for delivery old menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var menuItem =
        {
            DelPrice: 5000
        };

        // The test
        var isAvailable = menuHelper.isItemAvailable(menuItem);
        equal(isAvailable, true, "Expected true but got " + isAvailable);
    }
);

test
(
    "Available for delivery new menu",
    function ()
    {
        // Setup
        viewModel.orderType('Delivery');
        var menuItem =
        {
            DeliveryPrice: 6000
        };

        // The test
        var isAvailable = menuHelper.isItemAvailable(menuItem);
        equal(isAvailable, true, "Expected true but got " + isAvailable);
    }
);

/* >>> calculateToppingPrice tests <<< */
module("calculateToppingPrice tests", { setup: setup });

test
(
    "Double up default topping, no free toppings",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var toppingWrapper =
        {
            type: 'removable',
            selectedSingle: ko.observable(false),
            selectedDouble: ko.observable(true),
            freeQuantity: 0,
            topping:
            {
                DelPrice: 10,
                ColPrice: 30
            }
        };
        var prices = [];

        // The test
        var price = menuHelper.calculateToppingPrice(toppingWrapper, prices);
        equal(price, 30, "Expected 30 but got " + price);
    }
);

test
(
    "Double up default topping, one free topping",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var toppingWrapper =
        {
            type: 'removable',
            selectedSingle: ko.observable(false),
            selectedDouble: ko.observable(true),
            freeQuantity: 1,
            topping:
            {
                DelPrice: 10,
                ColPrice: 30
            }
        };
        var prices = [];

        // The test
        var price = menuHelper.calculateToppingPrice(toppingWrapper, prices);
        equal(price, 0, "Expected 0 but got " + price);
    }
);

test
(
    "Add single topping, no free toppings",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var toppingWrapper =
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
        };
        var prices = [];

        // The test
        var price = menuHelper.calculateToppingPrice(toppingWrapper, prices);
        equal(price, 30, "Expected 30 but got " + price);
    }
);

test
(
    "Add double topping, no free toppings",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var toppingWrapper =
        {
            type: 'optional',
            selectedSingle: ko.observable(false),
            selectedDouble: ko.observable(true),
            freeQuantity: 0,
            topping:
            {
                DelPrice: 10,
                ColPrice: 30
            }
        };
        var prices = [];

        // The test
        var price = menuHelper.calculateToppingPrice(toppingWrapper, prices);
        equal(price, 60, "Expected 60 but got " + price);
    }
);

test
(
    "Add single topping, 1 free topping",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var toppingWrapper =
        {
            type: 'optional',
            selectedSingle: ko.observable(true),
            selectedDouble: ko.observable(false),
            freeQuantity: 1,
            topping:
            {
                DelPrice: 10,
                ColPrice: 30
            }
        };
        var prices = [];

        // The test
        var price = menuHelper.calculateToppingPrice(toppingWrapper, prices);
        equal(price, 0, "Expected 0 but got " + price);
    }
);

test
(
    "Add double topping, 1 free topping",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var toppingWrapper =
        {
            type: 'optional',
            selectedSingle: ko.observable(false),
            selectedDouble: ko.observable(true),
            freeQuantity: 1,
            topping:
            {
                DelPrice: 10,
                ColPrice: 30
            }
        };
        var prices = [];

        // The test
        var price = menuHelper.calculateToppingPrice(toppingWrapper, prices);
        equal(price, 30, "Expected 30 but got " + price);
    }
);

test
(
    "Add double topping, 2 free toppings",
    function ()
    {
        // Setup
        viewModel.orderType('Collection');
        var toppingWrapper =
        {
            type: 'optional',
            selectedSingle: ko.observable(false),
            selectedDouble: ko.observable(true),
            freeQuantity: 2,
            topping:
            {
                DelPrice: 10,
                ColPrice: 30
            }
        };
        var prices = [];

        // The test
        var price = menuHelper.calculateToppingPrice(toppingWrapper, prices);
        equal(price, 0, "Expected 0 but got " + price);
    }
);