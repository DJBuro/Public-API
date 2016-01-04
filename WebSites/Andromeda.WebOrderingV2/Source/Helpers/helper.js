var helper =
{
    markOfflineStores: function ()
    {
        for (var index = 0; index < viewModel.sites().length; index++)
        {
            var site = viewModel.sites()[index];

            var displayName = site.name;

            if (!site.isOpen)
            {
                displayName += textStrings.appendOffline;
            }

            site.displayText = ko.observable(displayName);
        }
    },
    newOrder: function ()
    {
        helper.clearOrder();

        if (settings.returnToParentWebsiteAfterOrder)
        {
            viewModel.returnToHostWebsite();
        }
        else
        {
            viewModel.pageManager.showPage('Menu', true);
        }
    },
    clearOrder: function ()
    {
        // Empty out the checkout
        checkoutHelper.clearCheckout();

        // Empty out the cart
        cartHelper.clearCart();

        // Clear anything that the user can change on the menu pages
        menuHelper.clearSelectables();

        // make sure there is no left over voucher code
        checkoutHelper.checkVoucherCode = '';

        //Bug 8134:GPRS\Andro Web: Voucher code with percentage discount is still displayed in the 'My Order' section after it is being redeemed in previous order
        checkoutHelper.removeVouchers();
    },
    formatUTCDate: function (date)
    {
        return date.getUTCFullYear() + '-'
            + helper.pad(date.getUTCMonth() + 1) + '-'
            + helper.pad(date.getUTCDate()) + 'T'
            + helper.pad(date.getUTCHours()) + ':'
            + helper.pad(date.getUTCMinutes()) + ':'
            + helper.pad(date.getUTCSeconds()) + 'Z';
    },
    formatUTCSlot: function (slot)
    {
        // Extract the hours and minutes
        var dividerIndex = slot.indexOf(':');
        var hours = slot.substring(0, dividerIndex);
        var minutes = slot.substring(dividerIndex + 1);

        // By default wanted date is today
        var today = new Date();
        today.setHours(hours);
        today.setMinutes(minutes);

        // Is the order for tomorrow?  
        // A Rameses trading day is 6am to 6am so wanted times before 6am are actually for the next day
        if (hours < 6)
        {
            today.setDate(today.getDate() + 1);
        }

        // Format to JSON date
        var dateString = helper.formatUTCDate(today);
        return dateString;
    },
    pad: function (source)
    {
        if ((typeof (source) == 'number' && source < 10) ||
            (typeof (source) == 'string' && source.length == 1))
        {
            return '0' + source;
        }
        else
        {
            return source;
        }
    },
    formatPrice: function (price)
    {
        if (price == undefined)
        {
            if (helper.curencyAfter)
            {
                return "- " + helper.currencySymbol;
            }
            else
            {
                return helper.currencySymbol + " -";
            }
        }
        else
        {
            var x = Number(price);
            var y = x / 100;

            var price = undefined;

            if (helper.curencyAfter)
            {
                price = y.toFixed(2) + helper.currencySymbol;
            }
            else
            {
                price = helper.currencySymbol + y.toFixed(2);
            }

            if (helper.useCommaDecimalPoint)
            {
                price = price.replace('.', ',');
            }

            return price;
        }
    },
    currencySymbol: '&pound;',
    useCommaDecimalPoint: false,
    curencyAfter: false,
    use24hourClock: false,
    findById: function (id, list)
    {
        for (var index = 0; index < list.length; index++)
        {
            var item = list[index];

            if (item.Id == id)
            {
                return item;
            }
        }

        return undefined;
    },
    findCategory: function (category, categories)
    {
        var categoriesArray = typeof (categories) === 'function' ? categories() : categories;

        for (var index = 0; index < categoriesArray.length; index++)
        {
            var existingCategory = categoriesArray[index];

            if (existingCategory.Name == category.Name)
            {
                return true;
            }
        }

        //for (var index = 0; index < categories().length; index++)
        //{
        //    var existingCategory = categories()[index];

        //    if (existingCategory.Name == category.Name)
        //    {
        //        return true;
        //    }
        //}

        return false;
    },
    findByMenuId: function (id, list)
    {
        for (var index = 0; index < list.length; index++)
        {
            var item = list[index];

            if ((item.Id == undefined ? item.MenuId : item.Id) == id)
            {
                return item;
            }
        }

        return undefined;
    },
    ensureBlank: function (string)
    {
        return string == undefined ? '' : (string == null ? '' : string);
    },
    numbersOnly: function (data, event)
    {
        // Did the user press a number or backspace or delete?
        if ((event.which >= 48 && event.which <= 57) ||
            (event.keyCode >= 48 && event.keyCode <= 57) ||
            event.which == 8 || event.keyCode == 8 ||
            event.which == 127 || event.keyCode == 127 ||
            event.which == 9 || event.keyCode == 9)
        {
            return true;
        }

        return false;
    },
    change24HoursTo12Hours: function(hours)
    {
        if (hours > 12)
        {
            hours -= 12;
        }
        else if (hours === 0)
        {
            hours = 12;
        }

        return hours;
    }
}