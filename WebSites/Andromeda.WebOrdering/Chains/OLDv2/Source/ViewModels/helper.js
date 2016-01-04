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
        if (settings.returnToParentWebsiteAfterOrder)
        {
            viewModel.returnToHostWebsite();
        }
        else
        {
            // Empty out the checkout
            checkoutHelper.clearCheckout();

            // Empty out the cart
            cartHelper.clearCart();

            guiHelper.showHome();
        }
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
        var dividerIndex = slot.indexOf(':');
        var hours = slot.substring(0, dividerIndex);
        var minutes = slot.substring(dividerIndex + 1);

        var today = new Date();
        today.setHours(hours);
        today.setMinutes(minutes);

        var dateString = helper.formatUTCDate(today);
        return dateString;

        //var now = new Date();
        //var fixedSlot = slot;

        //if (slot.indexOf(':0', slot.length - 2) != -1)
        //{
        //    fixedSlot += '0';
        //}

        //return now.getUTCFullYear() + '-'
        //    + helper.pad(now.getUTCMonth() + 1) + '-'
        //    + helper.pad(now.getUTCDate()) + 'T'
        //    + fixedSlot + 'Z';
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
        for (var index = 0; index < categories().length; index++)
        {
            var existingCategory = categories()[index];

            if (existingCategory.Name == category.Name)
            {
                return true;
            }
        }

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
    }
}