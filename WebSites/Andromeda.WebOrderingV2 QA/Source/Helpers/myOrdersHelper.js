﻿var myOrdersHelper =
{
    status: ko.observable('get'),   // idle, get, got, error
    orders: ko.observableArray(),
    repeatOrder: ko.observable(undefined),
    getOrders: function ()
    {
        myOrdersHelper.status('get');

        // Remove orders from the UI first
        myOrdersHelper.orders.removeAll();

        // Let knockout do its thing
        setTimeout
        (
            function ()
            {
                // Get orders
                acsapi.getCustomerOrders
                (
                    accountHelper.emailAddress,
                    accountHelper.password,
                    function (data, errorMessage)
                    {
                        if (errorMessage != undefined && errorMessage.length > 0)
                        {
                            return;
                        }

                        if (data != undefined)
                        {
                            for (var index = 0; index < data.length; index++)
                            {
                                var order = data[index];

                                var status = '';
                                switch (order.OrderStatus)
                                {
                                    case 0: status = textStrings['moStatus0']; break;
                                    case 1: status = textStrings['moStatus1']; break;
                                    case 2: status = textStrings['moStatus2']; break;
                                    case 3: status = textStrings['moStatus3']; break;
                                    case 4: status = textStrings['moStatus4']; break;
                                    case 5: status = textStrings['moStatus5']; break;
                                    case 6: status = textStrings['moStatus6']; break;
                                }

                                // Parse the date so we can extract the date and time
                                //var placedDateTime = new Date(order.ForDateTime);

                                // Split out the date and time parts
                                var dateAndTimeParts = order.ForDateTime.split('T');

                                // Split out each part of the date and time
                                var dateParts = dateAndTimeParts[0].split('-');
                                var timeParts = dateAndTimeParts[1].split(':');

                                // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
                                //'6/29/2011 4:52:48 PM UTC'
                                var utcDate = 
                                    dateParts[0] + '/' + dateParts[1] + '/' + dateParts[2] + ' ' + timeParts[0] + ':' + timeParts[1] + ':' + timeParts[2] + ' UTC';

                                var placedDateTime = new Date(utcDate);

                                myOrdersHelper.orders.push
                                (
                                    {
                                        id: order.Id,
                                        placedDate: placedDateTime.getDate().toString() + '.' + (placedDateTime.getMonth() + 1).toString() + '.' + placedDateTime.getFullYear(),
                                        placedTime: helper.change24HoursTo12Hours(placedDateTime.getHours()).toString() + ":" + helper.pad(placedDateTime.getMinutes()).toString() + (placedDateTime.getHours() >= 12 ? 'pm' : 'am'),
                                        status: status,
                                        dataStatus: ko.observable(''),
                                        orderLines: ko.observableArray(),
                                        deals: ko.observableArray(),
                                        isSelected: ko.observable(false),
                                        total: ko.observable(0),
                                        deliveryCharge: ko.observable(0),
                                        paymentCharge: ko.observable(0),
                                        originalDate: placedDateTime,
                                        discounts: ko.observableArray()
                                    }
                                );
                            }
                            
                            myOrdersHelper.status('got');

                            // Expand the first item
                            if (myOrdersHelper.orders().length > 0)
                            {
                                myOrdersHelper.getOrder(myOrdersHelper.orders()[0]);
                            }
                        }
                    },
                    function (errorMessage)
                    {
                        //alert('error');
                    }
                )
            },
            0
        );
    },
    unselectAllOrders: function()
    {
        for (var orderIndex = 0; orderIndex < myOrdersHelper.orders().length; orderIndex++)
        {
            var clearOrder = myOrdersHelper.orders()[orderIndex];
            clearOrder.isSelected(false);
            clearOrder.orderLines.removeAll();
            clearOrder.deals.removeAll();
            clearOrder.discounts.removeAll();
        }
    },
    getOrder: function (order)
    {
        // Unselect the existing order
        myOrdersHelper.unselectAllOrders();

        // We're getting the order details
        order.isSelected(true);
        order.dataStatus('getDetails');

        // Get order
        acsapi.getCustomerOrder
        (
            order.id,
            accountHelper.emailAddress,
            accountHelper.password,
            function (data, errorMessage)
            {
                if (errorMessage != undefined && errorMessage.length > 0)
                {
                    return;
                }

                // Just in case the user was clicking other orders while we were making the web service call
                myOrdersHelper.unselectAllOrders();

                if (data == undefined)
                {
                    myOrdersHelper.status('error');
                }
                else
                {
                    for (var orderLineIndex = 0; orderLineIndex < data.OrderLines.length; orderLineIndex++)
                    {
                        var orderLine = data.OrderLines[orderLineIndex];

                        var displayOrderLine =
                        {
                            id: orderLine.MenuId,
                            name: '',
                            quantity: orderLine.Quantity,
                            price: orderLine.UnitPrice,
                            chefNotes: orderLine.ChefNotes,
                            person: orderLine.Person,
                            modifiers: []
                        };

                        // Prefix name with cat1 and cat2 if needed
                        if (orderLine.Cat1 !== undefined && orderLine.Cat1.length > 0)
                        {
                            displayOrderLine.name += orderLine.Cat1 + ' ';
                        }

                        if (orderLine.Cat2 !== undefined && orderLine.Cat2.length > 0)
                        {
                            displayOrderLine.name += orderLine.Cat2 + ' ';
                        }

                        displayOrderLine.name += orderLine.ProductName;

                        // Toppings
                        for (var orderLineModifierIndex = 0; orderLineModifierIndex < orderLine.Modifiers.length; orderLineModifierIndex++)
                        {
                            var orderLineModifier = orderLine.Modifiers[orderLineModifierIndex];


                            var description = orderLineModifier.Description;

                            if (orderLineModifier.Removed)
                            {
                                description = '&nbsp;&nbsp;&nbsp;remove ' + description;
                            }
                            else
                            {
                                if (orderLineModifier.Quantity == 1)
                                {
                                    description = '&nbsp;&nbsp;&nbsp;add single ' + description;
                                }
                                else
                                {
                                    description = '&nbsp;&nbsp;&nbsp;add double ' + description;
                                }
                            }

                            displayOrderLine.modifiers.push
                            (
                                {
                                    description: description,
                                    price: orderLineModifier.Price,
                                    quantity: orderLineModifier.Quantity,
                                    removed: orderLineModifier.Removed
                                }
                            );
                        }

                        order.orderLines.push(displayOrderLine);
                    }

                    // Deals
                    for (var dealIndex = 0; dealIndex < data.Deals.length; dealIndex++)
                    {
                        var deal = data.Deals[dealIndex];

                        var displayDeal =
                        {
                            id: deal.MenuId,
                            name: deal.ProductName,
                            price: deal.UnitPrice,
                            dealLines: []
                        };

                        // Deal lines
                        for (var dealLineIndex = 0; dealLineIndex < deal.DealLines.length; dealLineIndex++)
                        {
                            var dealLine = deal.DealLines[dealLineIndex];

                            var displayDealLine =
                            {
                                id: dealLine.MenuId,
                                name: '&nbsp;&nbsp;&nbsp;' + dealLine.ProductName,
                                quantity: dealLine.Quantity,
                                price: dealLine.UnitPrice * dealLine.Quantity,
                                chefNotes: dealLine.ChefNotes,
                                person: dealLine.Person,
                                modifiers: []
                            };

                            // Deal line modifiers
                            for (var dealLineModifierIndex = 0; dealLineModifierIndex < dealLine.Modifiers.length; dealLineModifierIndex++)
                            {
                                var dealLineModifier = dealLine.Modifiers[dealLineModifierIndex];

                                var description = dealLineModifier.Description;

                                if (dealLineModifier.Removed)
                                {
                                    description = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;remove ' + description;
                                }
                                else
                                {
                                    if (dealLineModifier.Quantity == 1)
                                    {
                                        description = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;add single ' + description;
                                    }
                                    else
                                    {
                                        description = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;add double ' + description;
                                    }
                                }

                                displayDealLine.modifiers.push
                                (
                                    {
                                        description: description,
                                        price: dealLineModifier.Price,
                                        quantity: dealLineModifier.Quantity,
                                        removed: dealLineModifier.Removed
                                    }
                                );
                            }

                            displayDeal.dealLines.push(displayDealLine);
                        }

                        order.deals.push(displayDeal);
                    }

                    // Discounts
                    for (var discountIndex = 0; discountIndex < data.Discounts.length; discountIndex++)
                    {
                        var discount = data.Discounts[discountIndex];

                        // Figure out what to display
                        var name = '';
                        var price = '';

                        if (discount.Type == 'VOUCHER')
                        {
                            name = textStrings.moVoucher.replace('{code}', discount.Reason);
                            price = discount.Amount * -1; // For some reason these are not in pence
                        }
                        else if (discount.Type == 'LOYALTY')
                        {
                            name = textStrings.moLoyalty;
                            price = (discount.Amount / 100) * -1; // In pence as expected
                        }
                        else
                        {
                            // Not supported so just display whatever we're given
                            name = discount.Type;
                            price = (discount.Amount / 100) * -1; // Assume pence
                        }

                        order.discounts.push
                        (
                            {
                                name: name,
                                price: price
                            }
                        );
                    }

                    order.isSelected(true);
                    order.dataStatus('got');
                    order.deliveryCharge(data.DeliveryCharge * 100);
                    order.paymentCharge(data.PaymentCharge);

                    var total = data.OrderTotal;
                    if (data.DeliveryCharge !== undefined)
                    {
                        total += data.DeliveryCharge;
                    }

                    order.total(total * 100);

                }
            },
            function (errorMessage)
            {
                myOrdersHelper.status('error');
            }
        );
    },
    refresh: function ()
    {
        myOrdersHelper.getOrders();
    }
}