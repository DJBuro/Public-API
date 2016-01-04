
var testMenu =
    {
        Category1 : 
            [ 
                { Id: 1, DealPremium: 0 },
                { Id: 2, DealPremium: 0 },
                { Id: 3, DealPremium: 20 }
            ],
        Category2 :
            [ 
                { Id: 10, DealPremium: 0 },
                { Id: 11, DealPremium: 0 },
                { Id: 12, DealPremium: 0 }
            ]
    };

function compareToppings(toppings1, toppings2)
{
    if (toppings1.length != toppings2.length) return false;

    for (var index = 0; index < toppings1.length; index++)
    {
        if (toppings1[index].id != toppings2[index].id) return false;
    }

    return true;
}

/* >>> getOrderValueExcludingDealsWithAMinimumPrice tests <<< */
module("sortDealLinesCheapestLast tests", { setup: setup });

test
(
    "two deal lines - different menu ids - no switch",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        { 
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } }, 
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops:0, DealPricePremiumOverride:0, DealPricePremium: 0 },
                            toppings: [ ]
                        },
                        { 
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [14, 15, 16, 17], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } }, 
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: [ ]
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "two deal lines - different menu ids - no switch",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        { 
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } }, 
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        },
                        { 
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [14, 15, 16, 17], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } }, 
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "two deal lines - buy one get one free (percentage) - no switch",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "two deal lines - buy one get one free (percentage) - no switch (item deal price premium)",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        // Ordinarily the menu items would be switched BUT the first menu item has a deal price premium making it more expensive than the second item
        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 20 },
                            toppings:[]
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 120, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "two deal lines - buy one get one free (percentage) - no switch (toppings)",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        // Ordinarily the menu items would be switched BUT the first menu item has an additional topping making it more expensive than the second item
        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings:
                                [
                                    { id: 1, freeQuantity: 0, type: 'optional', selectedSingle: ko.observable(true), freeQuantity: 0, topping: { DelPrice: 110, ColPrice: 200 } }
                                ]
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(compareToppings(dealHelper.selectedDeal.cartDealLines()[0].toppings, [{ id: 1 }]), true, 'Deal line 1:  Wrong toppings');

        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(compareToppings(dealHelper.selectedDeal.cartDealLines()[1].toppings, []), true, 'Deal line 2:  Wrong toppings');
    }

);

test
(
    "two deal lines - buy one get one free (percentage) - switch (item deal price premium)",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        // Ordinarily the menu items would not be switched BUT the second menu item has an item deal price premium making it more expensive than the first item
        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 120, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 110, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 20 },
                            toppings: []
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 11, 'Deal line 1:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 12, 'Deal line 2:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "two deal lines - buy one get one free (percentage) - switch (item deal price premium override)",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        // Ordinarily the menu items would not be switched BUT the second menu item has an item deal price premium override making it more expensive than the first item
        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 120, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 110, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 20, DealPricePremium: 0 },
                            toppings: []
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 11, 'Deal line 1:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 12, 'Deal line 2:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "two deal lines - buy one get one free (percentage) - switch (category deal price premium)",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        // Ordinarily the menu items would not be switched BUT the second menu item has a deal price premium making it more expensive than the first item
        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 120, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 110, ColPrice: 190, Cat1: 3, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 11, 'Deal line 1:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 12, 'Deal line 2:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "two deal lines - buy one get one free (percentage) - switch (toppings)",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        // Ordinarily the menu items would not be switched BUT the second menu item has an additional topping making it more expensive than the second item
        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 180, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: 
                                [
                                    { id: 1, freeQuantity: 0, type: 'optional', selectedSingle: ko.observable(true), freeQuantity: 0, topping: { DelPrice: 20, ColPrice: 20 } }
                                ]
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 11, 'Deal line 1:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(compareToppings(dealHelper.selectedDeal.cartDealLines()[0].toppings, [{ id: 1 }]), true, 'Deal line 1:  Wrong toppings');

        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 12, 'Deal line 2:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(compareToppings(dealHelper.selectedDeal.cartDealLines()[1].toppings, []), true, 'Deal line 2:  Wrong toppings');
    }

);

test
(
    "two deal lines - buy one get one free (percentage) - switch (free toppings have no effect)",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        // The menu items would be switched becuase of the additional topping on the second item BUT the second line also has one free topping
        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 120, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 110, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 1, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings:
                                [
                                    { id: 1, freeQuantity: 1, type: 'optional', selectedSingle: ko.observable(true), freeQuantity: 1, topping: { DelPrice: 20, ColPrice: 20 } }
                                ]
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(compareToppings(dealHelper.selectedDeal.cartDealLines()[0].toppings, []), true, 'Deal line 1:  Wrong toppings');

        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(compareToppings(dealHelper.selectedDeal.cartDealLines()[1].toppings, [{ id: 1 }]), true, 'Deal line 2:  Wrong toppings');
    }
);

test
(
    "two deal lines - buy one get one free (percentage) - switch (1 free topping - 2 toppings)",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        // The menu items sould be switched becuase of the additional topping on the second item (one of the two toppings is free)
        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 120, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings: []
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 110, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 1, DealPricePremiumOverride: 0, DealPricePremium: 0 },
                            toppings:
                                [
                                    { id: 1, freeQuantity: 1, type: 'optional', selectedSingle: ko.observable(true), freeQuantity: 1, topping: { DelPrice: 20, ColPrice: 20 } },
                                    { id: 2, freeQuantity: 1, type: 'optional', selectedSingle: ko.observable(true), freeQuantity: 1, topping: { DelPrice: 20, ColPrice: 20 } }
                                ]
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 11, 'Deal line 1:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(compareToppings(dealHelper.selectedDeal.cartDealLines()[0].toppings, [{ id: 1 }, { id: 2 }]), true, 'Deal line 1:  Wrong toppings');

        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 12, 'Deal line 2:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(compareToppings(dealHelper.selectedDeal.cartDealLines()[1].toppings, []), true, 'Deal line 2:  Wrong toppings');
    }
);

test
(
    "two deal lines - buy one get one free (percentage) - switch",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "three deal lines - buy two get one free (percentage) - no switch",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, DelAmount: 100 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 10, DelPrice: 90, ColPrice: 180, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id, 10, 'Deal line 3:  Expected menu id 10 but got ' + dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id);
    }
);

test
(
    "three deal lines - buy two get one free (percentage) - switch second and third lines",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 10, DelPrice: 90, ColPrice: 180, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, DelAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id, 10, 'Deal line 3:  Expected menu id 10 but got ' + dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id);
    }
);

test
(
    "three deal lines - buy two get one free (percentage) - switch first and second lines",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, DelAmount: 100 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 10, DelPrice: 90, ColPrice: 180, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id, 10, 'Deal line 3:  Expected menu id 10 but got ' + dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id);
    }
);

test
(
    "three deal lines - buy two get one free (percentage) - switch first and last lines",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 10, DelPrice: 90, ColPrice: 180, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 100, DelAmount: 100 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id, 10, 'Deal line 3:  Expected menu id 10 but got ' + dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id);
    }
);

test
(
    "two deal lines - buy one get one free (discount/fixed) - switch",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Discount', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 11, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [10, 11, 12, 13], Type: 'Fixed', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 12, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 12, 'Deal line 1:  Expected menu id 12 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 11, 'Deal line 2:  Expected menu id 11 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
    }
);

test
(
    "four deal lines - buy one small and one large and get two free (percentage) - switch",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [97, 98, 99, 100], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 99, DelPrice: 90, ColPrice: 180, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [97, 98, 99, 100], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 100, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [76, 77, 78, 79], Type: 'Percentage', DelAmount: 100, DelAmount: 100 } },
                            selectedMenuItem: { Id: 77, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [76, 77, 78, 79], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 78, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 100, 'Deal line 1:  Expected menu id 100 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 99, 'Deal line 2:  Expected menu id 99 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id, 78, 'Deal line 3:  Expected menu id 78 but got ' + dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[3].selectedMenuItem.Id, 77, 'Deal line 4:  Expected menu id 77 but got ' + dealHelper.selectedDeal.cartDealLines()[3].selectedMenuItem.Id);
    }
);

test
(
    "four deal lines - buy one get one free + side & desert (percentage) - switch",
    function ()
    {
        // Setup
        viewModel.orderType('delivery');
        viewModel.menu = testMenu;

        dealHelper.selectedDeal =
            {
                cartDealLines: ko.observableArray
                (
                    [
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [97, 98, 99, 100], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 99, DelPrice: 90, ColPrice: 180, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [97, 98, 99, 100], Type: 'Percentage', DelAmount: 0, ColAmount: 0 } },
                            selectedMenuItem: { Id: 100, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [76, 77, 78, 79], Type: 'Percentage', DelAmount: 100, DelAmount: 100 } },
                            selectedMenuItem: { Id: 77, DelPrice: 100, ColPrice: 190, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        },
                        {
                            dealLineWrapper: { dealLine: { AllowableItemsIds: [80, 81, 82, 83], Type: 'Percentage', DelAmount: 100, ColAmount: 100 } },
                            selectedMenuItem: { Id: 81, DelPrice: 110, ColPrice: 200, Cat1: 1, Cat2: undefined, FreeTops: 0, DealPricePremiumOverride: 0, DealPricePremium: 0 }
                        }
                    ]
                )
            };

        // The test
        dealHelper.sortDealLinesCheapestLast();

        // Check the result
        equal(dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id, 100, 'Deal line 1:  Expected menu id 100 but got ' + dealHelper.selectedDeal.cartDealLines()[0].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id, 99, 'Deal line 2:  Expected menu id 99 but got ' + dealHelper.selectedDeal.cartDealLines()[1].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id, 77, 'Deal line 3:  Expected menu id 77 but got ' + dealHelper.selectedDeal.cartDealLines()[2].selectedMenuItem.Id);
        equal(dealHelper.selectedDeal.cartDealLines()[3].selectedMenuItem.Id, 81, 'Deal line 4:  Expected menu id 81 but got ' + dealHelper.selectedDeal.cartDealLines()[3].selectedMenuItem.Id);
    }
);
