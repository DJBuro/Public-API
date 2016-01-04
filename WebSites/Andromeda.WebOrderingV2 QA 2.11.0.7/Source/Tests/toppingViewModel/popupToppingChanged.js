/// <reference path="../../Content/Scripts.js" />

/* >>> popupToppingChanged tests <<< */
module("popupToppingChanged tests", { setup: setup });

var __generateTestMenuItem =
    function ()
    {
        return {
            FreeTops: 0,
            ItemToppingRules:
            {
                Max: 0,
                MaxDuplicates: 0
            },
            DefTopIds: [45, 46],
            OptTopIds: [30, 31]
        };
    };

var __generateTestCartItem =
    function (testMenuItem)
    {
        return new CartItem
        (
            // ViewModel
            {
                menu:
                    {
                        Category1: undefined,
                        Toppings:
                            [
                                { Id: 45, DelPrice: 210, ColPrice: 200, Name: 'Test topping 1' },
                                { Id: 46, DelPrice: 210, ColPrice: 200, Name: 'Test topping 2' },
                                { Id: 30, DelPrice: 210, ColPrice: 200, Name: 'Test topping 3' },
                                { Id: 31, DelPrice: 210, ColPrice: 200, Name: 'Test topping 4' }
                            ]
                    }
            },
            // Menu item wrapper
            {
                availableTimes: [],
                menuItems: ko.observableArray([testMenuItem]),
                menuItem: testMenuItem,
                quantity: 1,
                name: 'Test menu item'
            },
            // Menu item
            testMenuItem,
            false  // additionsonly (for deal items)
        );
    };

/* ------------------------------- */
/* >>>> Add optional topiings <<<< */
/* ------------------------------- */

test
(
    "simple",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);
    }
);

test
(
    "max 1 topping, add singles",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 1;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a single optional topping
        cartItem.toppings()[3].selectedSingle(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[3]);
        equal(toppingResult2, false, "Expected false but got " + toppingResult2);
    }
);

test
(
    "max 2 topping, add double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 2;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a double optional topping
        cartItem.toppings()[2].selectedDouble(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);
    }
);

test
(
    "max 1 topping, add double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 1;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a double optional topping
        cartItem.toppings()[2].selectedDouble(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult1, false, "Expected false but got " + toppingResult1);
    }
);

test
(
    "max 3 topping, add double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 3;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a double optional topping
        cartItem.toppings()[2].selectedDouble(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);
    }
);

test
(
    "max 2 topping, add double then single",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 2;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a double optional topping
        cartItem.toppings()[2].selectedDouble(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a single optional topping
        cartItem.toppings()[3].selectedSingle(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[3]);
        equal(toppingResult2, false, "Expected false but got " + toppingResult2);
    }
);

test
(
    "max 2 topping, add single then double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 2;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a double optional topping
        cartItem.toppings()[3].selectedDouble(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[3]);
        equal(toppingResult2, false, "Expected false but got " + toppingResult2);
    }
);

test
(
    "max 3 topping, add single then double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 3;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a double optional topping
        cartItem.toppings()[3].selectedDouble(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[3]);
        equal(toppingResult2, true, "Expected true but got " + toppingResult2);
    }
);

test
(
    "max 4 topping, add double then double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 4;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a double optional topping
        cartItem.toppings()[2].selectedDouble(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a double optional topping
        cartItem.toppings()[3].selectedDouble(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[3]);
        equal(toppingResult2, true, "Expected true but got " + toppingResult2);
    }
);

test
(
    "max 3 toppings, add double then double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 3;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a double optional topping
        cartItem.toppings()[2].selectedDouble(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a double optional topping
        cartItem.toppings()[3].selectedDouble(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[3]);
        equal(toppingResult2, false, "Expected false but got " + toppingResult2);
    }
);

/* ------------------------------------------ */
/* >>>> Add and remove optional toppings <<<< */
/* ------------------------------------------ */

test
(
    "max 2 toppings, add double then remove double then add double then add single ",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 2;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a double optional topping
        cartItem.toppings()[2].selectedDouble(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Remove a double optional topping
        cartItem.toppings()[2].selectedDouble(false);
        var toppingResult2 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult2, true, "Expected true but got " + toppingResult2);

        // Add a double optional topping
        cartItem.toppings()[2].selectedDouble(true);
        var toppingResult3 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult3, true, "Expected true but got " + toppingResult3);

        // Add a single optional topping
        cartItem.toppings()[3].selectedSingle(true);
        var toppingResult4 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[3]);
        equal(toppingResult4, false, "Expected false but got " + toppingResult4);
    }
);

/* ---------------------------------- */
/* >>>> Remove included toppings <<<< */
/* ---------------------------------- */

test
(
    "max 1 topping, remove topping then add single",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 1;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Remove an included topping
        cartItem.toppings()[0].selectedSingle(false);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[0]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a single optional topping
        cartItem.toppings()[3].selectedSingle(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[3]);
        equal(toppingResult2, true, "Expected true but got " + toppingResult2);
    }
);

test
(
    "max 1 topping, remove topping then add double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 1;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Remove an included topping
        cartItem.toppings()[0].selectedSingle(false);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[0]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a double optional topping
        cartItem.toppings()[3].selectedDouble(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[3]);
        equal(toppingResult2, false, "Expected false but got " + toppingResult2);
    }
);

test
(
    "max 1 topping, remove topping then add single then add single",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 1;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Remove an included topping
        cartItem.toppings()[0].selectedSingle(false);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[0]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult2, true, "Expected true but got " + toppingResult2);

        // Add a single optional topping
        cartItem.toppings()[3].selectedSingle(true);
        var toppingResult3 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[3]);
        equal(toppingResult3, true, "Expected true but got " + toppingResult3);
    }
);

test
(
    "max 1 topping, remove topping then add single then add double",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 1;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Remove an included topping
        cartItem.toppings()[0].selectedSingle(false);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[0]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult2, true, "Expected true but got " + toppingResult2);

        // Add a double optional topping
        cartItem.toppings()[3].selectedDouble(true);
        var toppingResult3 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[3]);
        equal(toppingResult3, false, "Expected false but got " + toppingResult3);
    }
);

test
(
    "max 1 topping, add single then remove topping then add single",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 1;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a single optional topping
        cartItem.toppings()[3].selectedSingle(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[3]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Add a single optional topping (fails)
        cartItem.toppings()[3].selectedSingle(true);
        var toppingResult2 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[3]);
        equal(toppingResult2, false, "Expected false but got " + toppingResult2);

        // Remove an included topping
        cartItem.toppings()[0].selectedSingle(false);
        var toppingResult3 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[0]);
        equal(toppingResult3, true, "Expected true but got " + toppingResult3);

        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult4 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[2]);
        equal(toppingResult4, true, "Expected ture but got " + toppingResult4);
    }
);


test
(
    "max 2 toppings, add double then remove topping then add single then add included then remove single then add single",
    function ()
    {
        // Setup test data
        var menuItem = __generateTestMenuItem();
        menuItem.ItemToppingRules.Max = 2;
        var cartItem = __generateTestCartItem(menuItem);

        // We want to modify toppings for this cartitem
        var toppingViewModel = new ToppingsViewModel(cartItem);

        // Add a double optional topping
        cartItem.toppings()[3].selectedDouble(true);
        var toppingResult1 = toppingViewModel.popupToppingChanged('double', cartItem.toppings()[3]);
        equal(toppingResult1, true, "Expected true but got " + toppingResult1);

        // Remove an included topping
        cartItem.toppings()[0].selectedSingle(false);
        var toppingResult2 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[0]);
        equal(toppingResult2, true, "Expected true but got " + toppingResult2);

        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult3 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult3, true, "Expected true but got " + toppingResult3);

        // Add a single included topping (fail)
        cartItem.toppings()[0].selectedSingle(true);
        var toppingResult4 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[0]);
        equal(toppingResult4, false, "Expected false but got " + toppingResult4);

        // Remove a single optional topping
        cartItem.toppings()[2].selectedSingle(false);
        var toppingResult5 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult5, true, "Expected true but got " + toppingResult5);
       
        // Add a single optional topping
        cartItem.toppings()[2].selectedSingle(true);
        var toppingResult6 = toppingViewModel.popupToppingChanged('single', cartItem.toppings()[2]);
        equal(toppingResult6, true, "Expected true but got " + toppingResult6);
    }
);

