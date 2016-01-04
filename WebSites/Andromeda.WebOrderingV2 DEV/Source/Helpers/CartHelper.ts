/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="./loyaltyHelper.ts" />
/// <reference path="../Models/Cart.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="Settings.ts" />
/// <reference path="../../scripts/typings/google.analytics/ga.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.models.d.ts" />
/// <reference path="../viewmodels/checkout/checkoutviewmodel.ts" />
/// <reference path="../viewmodels/upsellviewmodel.ts" />

module AndroWeb.Helpers
{
    export class CartHelper
    {
        public static isCheckoutMode: KnockoutObservable<boolean> = ko.observable(false);
        public static ignoreOrderTypeChangeEvents: boolean = true;

        // This is the main cart.  Since it's used all over the place it has to live somewhere is here it is...
        public static cart: KnockoutObservable<any> = ko.observable(new AndroWeb.Models.Cart());

        constructor()
        {
        }

        public static isCartEmpty() : boolean
        {
            return AndroWeb.Helpers.CartHelper.cart().cartItems().length === 0 && AndroWeb.Helpers.CartHelper.cart().deals().length === 0;
        }

        public static refreshCart(cart, applyCardCharge) : void
        {
            // Reset total items in cart
            cart.totalItemCount(0);

            // Update the deals availability (depending on whether it's a collection or delivery order)
            menuHelper.refreshDealsAvailabilty(cart);

            // Before we can do anuthing with deals we must distribute menu items between deal lines, most expensive first
            AndroWeb.Helpers.CartHelper.sortDealLines(cart);

            // Get the order value not including deals with a minimum order value
            var orderTotalForMinimumOrderValue = AndroWeb.Helpers.CartHelper.getOrderValueExcludingDealsWithAMinimumPrice(cart);

            var cartDetails =
                {
                    totalPrice: 0,
                    areDisabledItems: false,
                    checkoutEnabled: false,
                    totalPriceItemsOnly: 0
                };

            // Calculate deal prices
            AndroWeb.Helpers.CartHelper.refreshCartDeals(cart, cartDetails);

            // Calculate item prices
            AndroWeb.Helpers.CartHelper.refreshCartLineItems(cart, cartDetails)
        
            // Disabled cart items
            cart.areDisabledItems(cartDetails.areDisabledItems);

            // Calc sub total
            cart.displaySubTotalPrice(helper.formatPrice(cartDetails.totalPrice));
            cart.subTotalPrice(cartDetails.totalPrice);

            // Voucher codes
            AndroWeb.Helpers.CartHelper.refreshCartVouchers(cart, cartDetails);

            // Calculate the full order discount
            var fullOrderDiscountAmount = AndroWeb.Helpers.CartHelper.calculateFullOrderDiscount(cartDetails.totalPriceItemsOnly);

            // Discount
            var discountName = menuHelper.fullOrderDiscountDeal == undefined ? undefined : menuHelper.fullOrderDiscountDeal.DealName;
            cart.discountName(discountName);
            cart.discountAmount(fullOrderDiscountAmount);
            cart.displayDiscountAmount(helper.formatPrice(fullOrderDiscountAmount * -1));

            // Apply the full order discount
            cartDetails.totalPrice -= fullOrderDiscountAmount;

            // Loyalty
            cart.preLoyaltySubTotalPrice(cartDetails.totalPrice);

            // loyalty discount has been done here ... 
            loyaltyHelper.refreshLoyalty(cartDetails);

            // Delivery charge
            AndroWeb.Helpers.CartHelper.refreshDeliveryCharges(cart, cartDetails);

            // Card charge
            AndroWeb.Helpers.CartHelper.refreshCardCharges(cart, cartDetails, applyCardCharge);

            // Grand total - all calculations shouldhave been done by now
            cart.displayTotalPrice(helper.formatPrice(cartDetails.totalPrice));
            cart.totalPrice(cartDetails.totalPrice);

            // True when there are items (enabled or disabled) in the cart
            cart.hasItems(cart.cartItems().length != 0 || cart.deals().length != 0);

            // Is the delivery charge met?  Don't check if already disabled
            if (cartDetails.checkoutEnabled && viewModel.orderType() == 'delivery')
            {
                cartDetails.checkoutEnabled = cart.preLoyaltySubTotalPrice() >= settings.minimumDeliveryOrder;
            }

            // True when there is at least one enabled item in the cart
            cart.checkoutEnabled(cartDetails.checkoutEnabled);

            // Display a nice message that a voucher will be auto applied
            AndroWeb.Helpers.CartHelper.checkAutoApplyVoucher(cart);
        }

        public static refreshCartDeals(cart, cartDetails) : void
        {
            var totalItemCount = cart.totalItemCount();

            // Update the deal prices in the cart
            for (var cartDealIndex = 0; cartDealIndex < cart.deals().length; cartDealIndex++)
            {
                var cartDeal = cart.deals()[cartDealIndex];
                cartDeal.finalPrice = 0;

                // Has the minimum order value been met?
                var minimumOrderValueOk = cartDetails.orderTotalForMinimumOrderValue == undefined || cartDetails.orderTotalForMinimumOrderValue >= cartDeal.dealWrapper.deal.MinimumOrderValue;
                cartDeal.minimumOrderValueNotMet(!minimumOrderValueOk);

                // Is the deal enabled in the cart?
                cartDeal.isEnabled(menuHelper.isDealItemEnabledForCart(cartDeal));

                if (cartDeal.isEnabled())
                {
                    cartDetails.checkoutEnabled = true;
                }
                else
                {
                    cartDetails.areDisabledItems = true;
                }

                // Default the display price
                cartDeal.displayPrice(helper.formatPrice(cartDeal.price));

                var dealPrice: number = 0;

                // Does the deal have any deal lines?
                if (cartDeal.cartDealLines() != undefined)
                {
                    // Update the prices of the deal lines in the deal
                    for (var cartDealLineIndex = 0; cartDealLineIndex < cartDeal.cartDealLines().length; cartDealLineIndex++)
                    {
                        totalItemCount++;

                        var cartDealLine = cartDeal.cartDealLines()[cartDealLineIndex];
                        if (cartDealLine.cartItem() === undefined) continue;

                        // Add category deal premiums
                        var category1 = cartDealLine.cartItem().selectedCategory1();
                        var category2 = cartDealLine.cartItem().selectedCategory2();
                        var categoryPremium = 0;

                        // Deal premiums do not apply to percentage deal lines
                        if (cartDealLine.dealLineWrapper.dealLine.Type.toLowerCase() != "percentage")
                        {
                            categoryPremium = ((category1 == undefined ? 0 : category1.DealPremium) + (category2 == undefined ? 0 : category2.DealPremium));
                        }

                        // Prefix the item name with the categories
                        var categoryText = category1 === undefined ? '' : category1.Name;
                        categoryText += categoryText.length > 0 ? ' ' : '';
                        categoryText += category2 === undefined ? '' : category2.Name;
                        categoryText += categoryText.length > 0 ? ' ' : '';

                        cartDealLine.displayName(categoryText + cartDealLine.cartItem().name);

                        // Set the calculated category premium
                        if (categoryPremium == undefined || categoryPremium == 0)
                        {
                            cartDealLine.categoryPremium(undefined);
                            cartDealLine.categoryPremiumName(undefined);
                        }
                        else
                        {
                            cartDealLine.categoryPremium(helper.formatPrice(categoryPremium));
                            cartDealLine.categoryPremiumName(textStrings.premiumChargeFor.replace('{item}', category2.Name));
                        }

                        // Item deal premium
                        var itemPremium = 0;

                        // Deal premiums do not apply to percentage deal lines
                        if (cartDealLine.dealLineWrapper.dealLine.Type.toLowerCase() != "percentage")
                        {
                            // Add item deal premium
                            itemPremium = cartDealLine.cartItem().menuItem.DealPricePremiumOverride == 0
                                ? cartDealLine.cartItem().menuItem.DealPricePremium
                                : cartDealLine.cartItem().menuItem.DealPricePremiumOverride;
                        }

                        // Set the calculated item premium
                        if (itemPremium == undefined || itemPremium == 0)
                        {
                            cartDealLine.itemPremium(undefined);
                            cartDealLine.itemPremiumName(undefined);
                        }
                        else
                        {
                            cartDealLine.itemPremium(helper.formatPrice(itemPremium));
                            cartDealLine.itemPremiumName(textStrings.premiumChargeFor.replace('{item}', cartDealLine.cartItem().menuItem.Name));
                        }

                        // Add the deal line to the deal price
                        var dealLinePrice: number = menuHelper.calculateDealLinePrice(cartDealLine, false);
                        dealPrice += dealLinePrice;

                        if (cartDeal.isEnabled())
                        {
                            cartDeal.finalPrice += dealLinePrice + categoryPremium + itemPremium;
                            cartDetails.totalPrice += dealLinePrice + categoryPremium + itemPremium;
                        }

                        // Does the deal line have any toppings?
                        if (cartDealLine.cartItem().toppings() != undefined)
                        {
                            cartDealLine.cartItem().displayToppings(menuHelper.getCartDisplayToppings(cartDealLine.cartItem().toppings()));

                            var toppingsPrice = 0;

                            // Update the prices of the toppings in the item
                            for (var toppingIndex = 0; toppingIndex < cartDealLine.cartItem().displayToppings().length; toppingIndex++)
                            {
                                var topping = cartDealLine.cartItem().displayToppings()[toppingIndex];
                                var toppingPrice = topping.price;
                                var finalPrice = toppingPrice * cartDealLine.cartItem().quantity();
                                topping.cartPrice(helper.formatPrice(finalPrice));
                            
                                toppingsPrice += toppingPrice;
                            }

                            if (cartDeal.isEnabled())
                            {
                                cartDealLine.price += toppingsPrice * cartDealLine.cartItem().quantity();

                                cartDetails.totalPrice += (toppingsPrice * cartDealLine.cartItem().quantity());
                                cartDetails.totalPriceItemsOnly += (toppingsPrice * cartDealLine.cartItem().quantity());

                                cartDeal.finalPrice += (toppingsPrice * cartDealLine.cartItem().quantity());
                            }
                        }
                    }

                    cartDeal.price = dealPrice;
                    cartDeal.displayPrice(helper.formatPrice(dealPrice));
                }
            }

            // Update the total count of items in the basket
            cart.totalItemCount(totalItemCount);
        }

        private static sortDealLines(cart): void
        {
            // Make sure the deal line prices are upto date before attempting the sort
            for (var cartDealIndex = 0; cartDealIndex < cart.deals().length; cartDealIndex++)
            {
                var cartDeal = cart.deals()[cartDealIndex];

                for (var cartDealLineIndex: number = 0; cartDealLineIndex < cartDeal.cartDealLines().length; cartDealLineIndex++)
                {
                    var cartDealLine = cartDeal.cartDealLines()[cartDealLineIndex];
                    cartDealLine.cartItem().recalculatePrice();
                }
            }

            // Sort the deal lines inside each deal
            for (var cartDealIndex = 0; cartDealIndex < cart.deals().length; cartDealIndex++)
            {
                var cartDeal = cart.deals()[cartDealIndex];

                // Does the deal have any deal lines?
                if (cartDeal.cartDealLines != undefined && cartDeal.cartDealLines() != undefined && cartDeal.cartDealLines().length > 1)
                {
                    // Starting with the second deal line, check each deal line before it to see if we need to swap them around so the cheapest is last
                    for (var cartDealLineIndex: number = 0; cartDealLineIndex < cartDeal.cartDealLines().length; cartDealLineIndex++)
                    {
                        var cartDealLine = cartDeal.cartDealLines()[cartDealLineIndex];

                        for (var checkDealLineIndex: number = cartDealLineIndex - 1; checkDealLineIndex >= 0; checkDealLineIndex--)
                        {
                            var checkDealLine = cartDeal.cartDealLines()[checkDealLineIndex];

                            // Check each of the allowable menu items in the other deal line
                            // If the selected menu item is also an allowable item in the other deal line then we need to compare prices and possibly swap them
                            for (var menuIdIndex = 0; menuIdIndex < checkDealLine.dealLineWrapper.dealLine.AllowableItemsIds.length; menuIdIndex++)
                            {
                                var checkAllowedItem = checkDealLine.dealLineWrapper.dealLine.AllowableItemsIds[menuIdIndex];

                                // Is this allowable menu item the same menu item as the customer selected in the target deal line?
                                if (cartDealLine.cartItem().menuItem.Id == checkAllowedItem)
                                {
                                    // This deal line's list of allowable menu items does contain the target menu item

                                    // Check if we need to swap them
                                    var cartDealLineMenuItemPrice = menuHelper.getItemPrice(cartDealLine.cartItem().menuItem);
                                    var checkDealLineMenuItemPrice = menuHelper.getItemPrice(checkDealLine.cartItem().menuItem);

                                    if (cartDealLineMenuItemPrice > checkDealLineMenuItemPrice)
                                    {
                                        // We need to switch the menu items around so the more expensive one is first

                                        // Copy the deal line contents into temporary variables
                                        var tempAllowableMenuItemWrappers = checkDealLine.allowableMenuItemWrappers;
                                        var tempCartItem = checkDealLine.cartItem();
                                        var tempCategoryPremium = checkDealLine.categoryPremium();
                                        var tempCategoryPremiumName = checkDealLine.categoryPremiumName();
                                    //    var tempDealLineWrapper = checkDealLine.dealLineWrapper;
                                        var tempDisplayName = checkDealLine.displayName();
                                        var tempDisplayPrice = checkDealLine.displayPrice()
                                        var tempHasError = checkDealLine.hasError()
                                        var tempInstructions = checkDealLine.instructions;
                                        var tempItemPremium = checkDealLine.itemPremium()
                                        var tempItemPremiumName = checkDealLine.itemPremiumName()
                                        var tempName = checkDealLine.name()
                                        var tempPerson = checkDealLine.person;
                                        var tempPreviousCartItem = checkDealLine.previousCartItem;
                                        var tempPreviouslySelectedAllowableMenuItemWrapper = checkDealLine.previouslySelectedAllowableMenuItemWrapper;
                                        var tempPrice = checkDealLine.price;
                                        var tempSelectedAllowableMenuItemWrapper = checkDealLine.selectedAllowableMenuItemWrapper();
                                        var tempSelectedCategoriesText = checkDealLine.selectedCategoriesText();

                                        // Switch over the first deal line
                                        checkDealLine.allowableMenuItemWrappers = cartDealLine.allowableMenuItemWrappers;
                                        checkDealLine.cartItem(cartDealLine.cartItem());
                                        checkDealLine.categoryPremium(cartDealLine.categoryPremium());
                                        checkDealLine.categoryPremiumName(cartDealLine.categoryPremiumName());
                                     //   checkDealLine.dealLineWrapper = cartDealLine.dealLineWrapper;
                                        checkDealLine.displayName(cartDealLine.displayName());
                                        checkDealLine.displayPrice(cartDealLine.displayPrice());
                                        checkDealLine.hasError(cartDealLine.hasError());
                                        checkDealLine.instructions = cartDealLine.instructions;
                                        checkDealLine.itemPremium(cartDealLine.itemPremium());
                                        checkDealLine.itemPremiumName(cartDealLine.itemPremiumName());
                                        checkDealLine.name(cartDealLine.name());
                                        checkDealLine.person = cartDealLine.person;
                                        checkDealLine.previousCartItem = cartDealLine.previousCartItem;
                                        checkDealLine.previouslySelectedAllowableMenuItemWrapper = cartDealLine.previouslySelectedAllowableMenuItemWrapper;
                                        checkDealLine.price = cartDealLine.price;
                                        checkDealLine.selectedAllowableMenuItemWrapper(cartDealLine.selectedAllowableMenuItemWrapper());
                                        checkDealLine.selectedCategoriesText(cartDealLine.selectedCategoriesText());

                                        // Switch over the second deal line
                                        cartDealLine.allowableMenuItemWrappers = tempAllowableMenuItemWrappers;
                                        cartDealLine.cartItem(tempCartItem);
                                        cartDealLine.categoryPremium(tempCategoryPremium);
                                        cartDealLine.categoryPremiumName(tempCategoryPremiumName);
                                    //    cartDealLine.dealLineWrapper = tempDealLineWrapper;
                                        cartDealLine.displayName(tempDisplayName);
                                        cartDealLine.displayPrice(tempDisplayPrice);
                                        cartDealLine.hasError(tempHasError);
                                        cartDealLine.instructions = tempInstructions;
                                        cartDealLine.itemPremium(tempItemPremium);
                                        cartDealLine.itemPremiumName(tempItemPremiumName);
                                        cartDealLine.name(tempName);
                                        cartDealLine.person = tempPerson;
                                        cartDealLine.previousCartItem = tempPreviousCartItem;
                                        cartDealLine.previouslySelectedAllowableMenuItemWrapper = tempPreviouslySelectedAllowableMenuItemWrapper;
                                        cartDealLine.price = tempPrice;
                                        cartDealLine.selectedAllowableMenuItemWrapper(tempSelectedAllowableMenuItemWrapper);
                                        cartDealLine.selectedCategoriesText(tempSelectedCategoriesText);

                                        // Since we've moved the selected menu item to a different deal line we need to continue checking from 
                                        // the new deal line
                                        cartDealLine = checkDealLine;

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static refreshCartLineItems(cart, cartDetails) : void
        {
            var totalItemCount = cart.totalItemCount();

            // Update the item prices in the cart
            for (var index = 0; index < cart.cartItems().length; index++)
            {
                var item = cart.cartItems()[index];

                totalItemCount += Number(item.quantity());

                // Work out if the cart item is valid.  If not set "isEnabled" false and ideally put the reason in the "notes" property
                AndroWeb.Helpers.CartHelper.setCartItemAvailability(item);

                // Enable checkout if any cart item in enabled
                if (item.isEnabled())
                {
                    cartDetails.checkoutEnabled = true;
                }

                // Update the item name
                item.displayName(menuHelper.getCartItemDisplayName(item));

                // Set the price depending on the current order type
                item.price = menuHelper.calculateItemPrice(item.menuItem, item.quantity(), undefined);

                // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
                if (item.isEnabled())
                {
                    cartDetails.totalPrice += item.price;
                    cartDetails.totalPriceItemsOnly += item.price;
                }
                else
                {
                    cartDetails.areDisabledItems = true;
                }

                // Set the display price depending on the current order type
                item.displayPrice(helper.formatPrice(item.price));

                item.displayToppings(menuHelper.getCartDisplayToppings(item.toppings()));

                // Does the item have any toppings?
                if (item.displayToppings() != undefined)
                {
                    var toppingsPrice = 0;

                    // Update the prices of the toppings in the item
                    for (var toppingIndex = 0; toppingIndex < item.displayToppings().length; toppingIndex++)
                    {
                        var topping = item.displayToppings()[toppingIndex];
                        var toppingPrice = topping.price;
                        var finalPrice = toppingPrice * item.quantity();
                        topping.cartPrice(helper.formatPrice(finalPrice));

                        toppingsPrice += toppingPrice;
                    }

                    if (item.isEnabled())
                    {
                        cartDetails.totalPrice += (toppingsPrice * item.quantity());
                        cartDetails.totalPriceItemsOnly += (toppingsPrice * item.quantity());
                    }
                }
            }

            // Update the total count of items in the basket
            cart.totalItemCount(totalItemCount);
        }

        public static refreshCartVouchers(cart, cartDetails) : void
        {
            if (cart.vouchers() != undefined && cart.vouchers().length > 0)
            {
                // Check each voucher code
                for (var voucherCodeIndex = 0; voucherCodeIndex < cart.vouchers().length; voucherCodeIndex++)
                {
                    var voucherCode = cart.vouchers()[voucherCodeIndex];

                    // Apply the voucher code discount
                    cartDetails.totalPrice = AndroWeb.Helpers.CartHelper.calculateVoucherDiscount(voucherCode, cartDetails.totalPrice);
                }
            }
        }

        public static refreshDeliveryCharges(cart, cartDetails) : void
        {
            // Is there a delivery charge?
            if (settings.deliveryCharge === undefined || viewModel.orderType() !== 'delivery')
            {
                AndroWeb.Helpers.CartHelper.removeDeliveryCharge(cart, cartDetails);
                return;
            }

            // Is there a free delivery threshold?
            if (settings.optionalFreeDeliveryThreshold !== undefined)
            {
                // Is the order eligable for free delivery?
                if (cartDetails.totalPrice < settings.optionalFreeDeliveryThreshold)
                {
                    // No - the order total is too low
                    AndroWeb.Helpers.CartHelper.applyDeliveryCharge(cart, cartDetails);
                }
                else
                {
                    // Yes - the order total is above the free delivery threshold
                    AndroWeb.Helpers.CartHelper.removeDeliveryCharge(cart, cartDetails);
                }
            }
            else
            {
                // There's no free delivery threshold - always charge for delivery
                AndroWeb.Helpers.CartHelper.applyDeliveryCharge(cart, cartDetails);
            }
        }

        public static applyDeliveryCharge(cart, cartDetails) : void
        {
            // The delivery charge is a decimal in MyAndromeda
            var deliveryChargePence = settings.deliveryCharge;

            // Add delivery charge
            cart.deliveryCharge = deliveryChargePence;
            cart.displayDeliveryCharge(helper.formatPrice(deliveryChargePence));

            // Add the delivery charge to the total price
            cartDetails.totalPrice = cartDetails.totalPrice + deliveryChargePence;
        }

        public static removeDeliveryCharge (cart, cartDetails) : void
        {
            // Add delivery charge
            cart.deliveryCharge = 0;
            cart.displayDeliveryCharge('');
        }

        public static refreshCardCharges(cart, cartDetails, applyCardCharge) : void
        {
            // Do we need to apply the card charge now?
            if (applyCardCharge === true &&
                settings.cardCharge !== undefined &&
                typeof settings.cardCharge === 'number' &&
                settings.cardCharge > 0)
            {
                // Make a note of the card charge we've applied
                cart.cardCharge = settings.cardCharge;

                // Add the card charge to the final price
                cartDetails.totalPrice = cartDetails.totalPrice + settings.cardCharge;
            }
            else
            {
                cart.cardCharge = 0;
            }

            // Work out whether a card payment notification prompt needs to be displayed
            if (settings.cardCharge !== undefined && typeof settings.cardCharge === 'number' && settings.cardCharge > 0)
            {
                cart.displayCardCharge(textStrings.checkCardCharge.replace('{AMOUNT}', helper.formatPrice(settings.cardCharge)));
                cart.displayCardChargePayment(textStrings.checkCardChargePayment.replace('{AMOUNT}', helper.formatPrice(settings.cardCharge)));
            }
            else
            {
                cart.displayCardCharge('');
                cart.displayCardChargePayment('');
            }
        }

        public static getOrderValueExcludingDealsWithAMinimumPrice(cart) : number
        {
            var totalPrice = 0;

            // Calculate the deal prices
            for (var index = 0; index < cart.deals().length; index++)
            {
                var cartDeal = cart.deals()[index];

                if (cartDeal.dealWrapper.deal.MinimumOrderValue == 0)
                {
                    // Does the deal have any deal lines?
                    if (cartDeal.cartDealLines != undefined)
                    {
                        // Update the prices of the deal lines in the deal
                        for (var dealLineIndex = 0; dealLineIndex < cartDeal.cartDealLines().length; dealLineIndex++)
                        {
                            var dealLine = cartDeal.cartDealLines()[dealLineIndex];
                            var dealLinePrice = menuHelper.calculateDealLinePrice(dealLine, false);

                            totalPrice += dealLinePrice;
                        }
                    }
                }
            }

            var totalPriceItemsOnly = 0;

            // Calculate the item prices
            for (var index = 0; index < cart.cartItems().length; index++)
            {
                var item = cart.cartItems()[index];

                // Set the price depending on the current order type
                var itemPrice = menuHelper.calculateItemPrice(item.menuItem, item.quantity(), undefined);

                // Some items in the cart may not be valid (e.g. user adds a delivery only item and then switches to collection)
                if (menuHelper.isItemAvailable(item.itemWrapper))
                {
                    totalPrice += itemPrice;
                    totalPriceItemsOnly += itemPrice;
                }

                // Does the item have any toppings?
                if (item.displayToppings != undefined)
                {
                    // Update the prices of the toppings in the item
                    for (var toppingIndex = 0; toppingIndex < item.displayToppings().length; toppingIndex++)
                    {
                        var topping = item.displayToppings()[toppingIndex];
                        var toppingPrice = menuHelper.calculateToppingPrice(topping);

                        if (menuHelper.isItemAvailable(item.itemWrapper))
                        {
                            totalPrice += toppingPrice;
                            totalPriceItemsOnly += toppingPrice;
                        }
                    }
                }
            }

            // Apply any full order discount deal
            totalPrice -= AndroWeb.Helpers.CartHelper.calculateFullOrderDiscount(totalPriceItemsOnly);

            if (totalPrice < 0) totalPrice = 0;

            return totalPrice;
        }

        public static calculateFullOrderDiscount(itemsPrice) : number
        {
            var fullOrderDiscount = 0;

            if (menuHelper.fullOrderDiscountDeal != undefined)
            {
                switch (menuHelper.fullOrderDiscountDeal.FullOrderDiscountType.toLowerCase())
                {
                    case 'percentage':

                        var percentage = 100;

                        if (viewModel.orderType().toLowerCase() == 'collection')
                        {
                            percentage = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount / 100;
                        }
                        else
                        {
                            percentage = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount / 100;
                        }

                        // Deduct the percentage from the item price
                        fullOrderDiscount = itemsPrice - (itemsPrice * percentage);

                        break;

                    case 'discount':

                        if (viewModel.orderType().toLowerCase() == 'collection')
                        {
                            fullOrderDiscount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountCollectionAmount;
                        }
                        else
                        {
                            fullOrderDiscount = menuHelper.fullOrderDiscountDeal.FullOrderDiscountDeliveryAmount;
                        }

                        // Deduct the discount from the item price
                        fullOrderDiscount = itemsPrice - fullOrderDiscount;

                        break;
                }
            }

            return Math.round(fullOrderDiscount);
        }

        public static calculateVoucherDiscount(voucherCode: any, totalPrice: number) : number
        {
            var afterDiscount = totalPrice;

            if (voucherCode.IsValid)
            {
                switch (voucherCode.EffectType.toLowerCase())
                {
                    case 'percentage':
                        afterDiscount = Math.round(totalPrice * (voucherCode.EffectValue / 100));
                        voucherCode.discount(totalPrice - afterDiscount);
                        break;

                    case 'fixed':
                        afterDiscount = totalPrice - (voucherCode.EffectValue * 100);
                        if (afterDiscount < 0) afterDiscount = 0;
                        voucherCode.discount(voucherCode.EffectValue);
                        break;
                }
            }

            return afterDiscount;
        }

        public static clearCart() : void
        {
            ga
            (
                "send",
                "event",
                {
                    eventCategory: "Sales",
                    eventAction: "ClearCart",
                    eventLabel: "",
                    eventValue: 1,
                    metric1: 1
                }
            );

            // Clear the cart
            AndroWeb.Helpers.CartHelper.cart().cartItems.removeAll();
            AndroWeb.Helpers.CartHelper.cart().deals.removeAll();

            // clear vouchers if any
            AndroWeb.Helpers.CartHelper.cart().vouchers([]);
            checkoutHelper.checkoutDetails.voucherCodes([]);
            checkoutHelper.voucherError('');

            // Update the total price (probably not needed)
            AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart(), undefined);

            // Clear payment details
            AndroWeb.Helpers.CartHelper.cart().dataCashPaymentDetails(undefined);

            viewModel.timer = new Date();
        }

        public static checkout(): void
        {
            // Is the customer logged in?
            if (settings.customerAccountsEnabled && !accountHelper.isLoggedIn())
            {
                // Show the login popup
                viewModel.pageManager.showPage('Login', true, new AndroWeb.ViewModels.LoginViewModel(true), false);
            }
            // Has the customer filled in all the required my profile details?
            else if
            (
                settings.customerAccountsEnabled &&
                accountHelper.isLoggedIn() &&
                accountHelper.customerDetails.contacts.length != 2)
            {
                viewModel.pageManager.showPage('MyProfile', true, undefined, true);
            }
            else
            {
                CartHelper.showCheckoutView();
            }
        }

        public static showCheckoutView() : boolean
        {
            // Refresh the delivery/collection slots
            checkoutHelper.refreshTimes();

            // Is the store open?
            if (checkoutHelper.times == undefined || checkoutHelper.times().length == 0)
            {
                alert(textStrings.sStoreClosed);
            
                viewModel.pageManager.showPage('Menu', true, undefined, true);

                return false;
            }

            guiHelper.cartActions(guiHelper.cartActionsCheckout);

            guiHelper.canChangeOrderType(false);

            guiHelper.isMobileMenuVisible(false);

            checkoutHelper.checkoutViewModel = new AndroWeb.ViewModels.CheckoutViewModel();

            viewModel.pageManager.showPage('Checkout', true, undefined, true);

            return true;
        }

        public static getDealTemplate (cartItem) : string
        {
            if (AndroWeb.Helpers.CartHelper.isCheckoutMode())
            {
                return 'disabledCartDeal-template';
            }
            else
            {
                return 'cartDeal-template';
            }
        }

        public static getItemTemplate(cartItem): string
        {
            if (AndroWeb.Helpers.CartHelper.isCheckoutMode())
            {
                return 'disabledCartItem-template';
            }
            else
            {
                return 'cartItem-template';
            }
        }

        public static editCartItem(cartItem): void
        {
            if (cartItem === undefined) cartItem = this;

            // Show the toppings popup
            toppingsPopupHelper.returnToCart = true; // In mobile mode the cart is a seperate view so we need to show the view
            toppingsPopupHelper.showPopup(cartItem, false, true);
        }

        public static editCartDeal() : void
        {
            // Show the deal popup
            dealPopupHelper.returnToCart = false;  // If the user cancels show the menu not the cart (mobile only)
            dealPopupHelper.showDealPopup('editDeal', true, this);
        }

        public static getCartItemName(menuItem, selectedCategory1, selectedCategory2): string
        {
            // Build the item name for the cart (product name + category names)
            var itemCategoriesText = "";

            if (selectedCategory1 != undefined)
            {
                itemCategoriesText += " (" + selectedCategory1.Name;
            }

            if (selectedCategory2 != undefined)
            {
                if (itemCategoriesText.length > 0) itemCategoriesText += ", ";

                itemCategoriesText += selectedCategory2.Name;
            }

            if (itemCategoriesText.length > 0) itemCategoriesText += ")";

            return (typeof (menuItem.name) == 'function' ? menuItem.name() : menuItem.name) + itemCategoriesText;
        }

        public static orderTypeChanged(): boolean
        {
            // Update the item prices
            menuHelper.updatePrices();

            // Update the cart prices and availability
            AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart(), undefined);

            return true;
        }

        public static removeCartItem(cartItem) : void
        {
            // Remove the item from the cart
            AndroWeb.Helpers.CartHelper.cart().cartItems.remove(cartItem);

            // Recalculate the total price
            AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart(), undefined);

            // Check that the vouchers are valid
            checkoutHelper.recheckVouchers();

            // Recalculate the total price
            AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart(), undefined);

            // Do we need to update delivery/collection time slots?
            if (AndroWeb.Helpers.CartHelper.isCheckoutMode())
            {
                checkoutHelper.refreshTimes();
            }
        }

        public static removeDealItem(dealItem) : void
        {
            // Remove the item from the cart
            AndroWeb.Helpers.CartHelper.cart().deals.remove(dealItem);

            // Recalculate the total price
            AndroWeb.Helpers.CartHelper.refreshCart(AndroWeb.Helpers.CartHelper.cart(), undefined);
        }

        public static checkAutoApplyVoucher(cart): void
        {
            if (queryStringHelper.voucherCode !== undefined && queryStringHelper.voucherCode.length > 0)
            {
                cart.autoApplyVoucherText(textStrings.cartAutoApplyVoucher.replace('{VOUCHERCODE}', queryStringHelper.voucherCode));
            }
            else
            {
                cart.autoApplyVoucherText('');
            }
        }

        public static removeAutoAppliedVoucher() : void
        {
            queryStringHelper.voucherCode = '';
        }

        public static setCartItemAvailability(cartItem) : void
        {
            // Is it a known menu item?
            if (cartItem.itemWrapper.menuItem.Id == -1)
            {
                cartItem.isEnabled(false);
                cartItem.notes(textStrings.roDiscontinued);
                return;
            }

            // If the menu item is temporarily disabled then it cannot be enabled
            if (cartItem.itemWrapper.isTemporarilyDisabled)
            {
                cartItem.isEnabled(false);
                cartItem.notes(textStrings.miTemporarilyUnavailable);
                return;
            }
        
            // Is the item available for collection or delivery?
            if (viewModel.orderType().toLowerCase() == 'delivery' &&
                ((cartItem.itemWrapper.menuItem.DelPrice == undefined && cartItem.itemWrapper.menuItem.DeliveryPrice == undefined) ||
                cartItem.itemWrapper.menuItem.DelPrice == -1 || cartItem.itemWrapper.menuItem.DeliveryPrice == -1))
            {
                // Item is not available for the current order type
                cartItem.isEnabled(false);
                cartItem.notes(textStrings.miNotAvailableForDelivery);
                return;
            }
            else if (viewModel.orderType().toLowerCase() == 'collection' &&
                ((cartItem.itemWrapper.menuItem.ColPrice == undefined && cartItem.itemWrapper.menuItem.CollectionPrice == undefined) ||
                cartItem.itemWrapper.menuItem.ColPrice == -1 || cartItem.itemWrapper.menuItem.CollectionPrice == -1))
            {
                // Item is not available for the current order type
                cartItem.isEnabled(false);
                cartItem.notes(textStrings.miNotAvailableForCollection);
                return;
            }
            else if (viewModel.orderType().toLowerCase() == 'dinein' &&
                ((cartItem.itemWrapper.menuItem.DineInPrice == undefined) || cartItem.itemWrapper.menuItem.DineInPrice == -1))
            {
                // Item is not available for the current order type
                cartItem.isEnabled(false);
                cartItem.notes(textStrings.miNotAvailableForDineIn);
                return;
            }
            else
            {
                cartItem.isEnabled(true);
                cartItem.notes('');
                return;
            }
        }
    }
}