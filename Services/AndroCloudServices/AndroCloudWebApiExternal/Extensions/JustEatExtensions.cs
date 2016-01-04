using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Extensions
{
    public static class JustEatExtensions
    {
        public const string MarketingType = "OrderOnly";

        /// <summary>
        /// Converts the specified order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public static Models.Andromeda.AndromedaOrder Convert(this Models.JustEat.JustEatOrder order) 
        {
            var model = new Models.Andromeda.AndromedaOrder();

            //andromedaOrder.
            model.partnerReference = order.Id;
            model.type = 
                order.Order.ServiceType.Equals("Delivery", StringComparison.InvariantCultureIgnoreCase) ||
                order.Order.ServiceType.Equals("2", StringComparison.InvariantCultureIgnoreCase) 
                ? "Delivery" 
                : "Collection";
            model.orderTimeType = order.Order.PromptAsap && 
                DateTime.Compare(order.Order.DueDate, order.Order.InitialDueDate) == 0 ? "ASAP" : "TIMED";
            model.orderWantedTime = order.Order.DueDate;
            model.orderPlacedTime = order.Order.PlacedDate;
            model.timeToTake = 0;
            model.estimatedDeliveryTime = 0;
            model.oneOffDirections = order.Order.NoteToRestaurant ?? string.Empty;
            
            //build customer
            model.customer = order.CustomerInfo.Convert();
            //build pricing 
            model.pricing = order.ConvertPricing();
            //order lines 
            model.orderLines = order.BasketInfo.Convert();
            //payments and payment lines
            model.orderPayments = order.PaymentInfo.Convert();

            return model;
        }

        /// <summary>
        /// Converts the specified payment info.
        /// </summary>
        /// <param name="paymentInfo">The payment info.</param>
        /// <returns></returns>
        private static List<Models.Andromeda.OrderPayment> Convert(this Models.JustEat.PaymentInfo paymentInfo) 
        {
            var orderPayments = new List<Models.Andromeda.OrderPayment>();

            foreach (var item in paymentInfo.PaymentLines) 
            {
                var orderPayment = item.Convert();
                orderPayments.Add(orderPayment);
            }

            return orderPayments;
        }

        /// <summary>
        /// Converts the specified payment line.
        /// </summary>
        /// <param name="paymentLine">The payment line.</param>
        /// <returns></returns>
        private static Models.Andromeda.OrderPayment Convert(this Models.JustEat.PaymentLine paymentLine)
        {
            if (paymentLine.Type == "Cash" || paymentLine.Type == "1") 
            {
                return new Models.Andromeda.OrderPayment()
                {
                    PaymentType = "PayLater",
                    Value = System.Convert.ToInt32(paymentLine.Value * 100),
                };    
            }
            return new Models.Andromeda.OrderPayment()
            {
                PaymentType = "Card",
                PaytypeName = "JUSTEAT",
                Value = System.Convert.ToInt32(paymentLine.Value * 100)
            };
        }

        /// <summary>
        /// Converts the specified basket.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns></returns>
        private static List<Models.Andromeda.OrderLine> Convert(this Models.JustEat.BasketInfo basket) 
        {
            var items = new List<Models.Andromeda.OrderLine>();

            foreach (var groupedBasketItem in basket.GroupedBasketItems)
            {
                foreach (var item in groupedBasketItem.Convert())
                {
                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// Converts the specified grouped basket item.
        /// </summary>
        /// <param name="groupedBasketItem">The grouped basket item.</param>
        /// <returns></returns>
        private static IEnumerable<Models.Andromeda.OrderLine> Convert(this Models.JustEat.GroupedBasketItem groupedBasketItem) 
        {
            var individualPrice = (groupedBasketItem.CombinedPrice / groupedBasketItem.Quantity); 

            var model = new Models.Andromeda.OrderLine() 
            {
                productId = -1,
                addToppings = new List<Models.Andromeda.AddTopping>(),
                removeToppings = new List<Models.Andromeda.AddTopping>(),
                
                quantity = groupedBasketItem.Quantity,
                price = System.Convert.ToInt32(groupedBasketItem.BasketItem.UnitPrice * 100 * groupedBasketItem.Quantity),
                name = string.Format("{0} {1}", groupedBasketItem.BasketItem.Name, groupedBasketItem.BasketItem.Synonym).Trim(),
                //probably
                lineType = groupedBasketItem.BasketItem.MealParts.Any() ? 0 : 3
            };

            foreach (var item in groupedBasketItem.BasketItem.RequiredAccessories) 
            {
                var topping = item.Convert();
                model.addToppings.Add(topping);
            }
            foreach (var item in groupedBasketItem.BasketItem.OptionalAccessories)
            {
                var topping = item.Convert();
                model.addToppings.Add(topping);
            }

            //return main line item
            yield return model;

            //deal lines maybe 
            var dealIndex = 1;
            foreach (var part in groupedBasketItem.BasketItem.MealParts) 
            {
                var lineItem = part.Convert(dealIndex);
                yield return lineItem;

                dealIndex++;
            }

            yield break;
        }

        /// <summary>
        /// Converts the specified meal part.
        /// </summary>
        /// <param name="mealPart">The meal part.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private static Models.Andromeda.OrderLine Convert(this Models.JustEat.MealPart mealPart, int index) 
        {
            var model = new Models.Andromeda.OrderLine()
            {
                productId = -1,
                name = string.Format("{0} {1}", mealPart.Name, mealPart.Synonym),
                addToppings = new List<Models.Andromeda.AddTopping>(),
                removeToppings = new List<Models.Andromeda.AddTopping>(),
                quantity = 1,
                price = 0,
                inDealFlag = true,
                lineType = 0,
                orderLineIndex = index,
                instructions = ""
            };

            foreach (var item in mealPart.RequiredAccessories) 
            {
                var topping = item.Convert();
                model.addToppings.Add(topping);
            }

            foreach (var item in mealPart.OptionalAccessories) 
            {
                var topping = item.Convert();
                model.addToppings.Add(topping);
            }

            return model;
        }

        /// <summary>
        /// Converts the specified accessory.
        /// </summary>
        /// <param name="accessory">The accessory.</param>
        /// <returns></returns>
        private static Models.Andromeda.AddTopping Convert(this Models.JustEat.Accessory accessory)
        {
            if (accessory.Quantity == 0) { accessory.Quantity = 1; }

            var model = new Models.Andromeda.AddTopping()
            {
                productId = -1,
                name = accessory.Name,
                //price is included in the combined price for deals 
                price = System.Convert.ToInt32(accessory.UnitPrice * accessory.Quantity * 100),
                itemPrice = System.Convert.ToInt32(accessory.UnitPrice * 100),
                quantity = accessory.Quantity
            };

            //time to fail certification and into the 'workaround' that I don't want to implement. 
            model.name = "++" + model.name;

            return model;
        }

        /// <summary>
        /// Converts the pricing.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        private static Models.Andromeda.Pricing ConvertPricing(this Models.JustEat.JustEatOrder order) 
        {
            int basketTotalPrice = System.Convert.ToInt32(order.BasketInfo.Total * 100);
            int deliveryCharge = System.Convert.ToInt32(order.BasketInfo.DeliveryCharge * 100);
            
            var cardCharges = order.PaymentInfo.PaymentLines.Sum(e => e.CardFee) * 100;
            var finalPrice = basketTotalPrice - deliveryCharge;// +System.Convert.ToInt32(cardCharges);

            var model = new Models.Andromeda.Pricing
            {
                DeliveryCharge = deliveryCharge,
                PriceBeforeDiscount = System.Convert.ToInt32(order.BasketInfo.SubTotal * 100),
                PaymentCharge = cardCharges,
                //PriceAfterDiscount = System.Convert.ToInt32(order.BasketInfo.Total * 100),
                //only for percentage:
                //DiscountTypeAmount = System.Convert.ToInt32(order.BasketInfo.Discount * 100),
                PricesIncludeTax = true,
                FinalPrice = finalPrice//basketTotalPrice - deliveryCharge,
                //Discounts = new List<Models.Andromeda.Discount>(),
            };

            if (order.BasketInfo.Discount > 0) 
            {
                model.DiscountType = "Fixed";
                model.DiscountAmount = System.Convert.ToInt32(order.BasketInfo.Discount * 100);
            }

            //skip the discounts section.
            //if (order.BasketInfo.Discounts != null && order.BasketInfo.Discounts.Any())
            //{
            //    foreach (var discount in order.BasketInfo.Discounts)
            //    {
            //        model.Discounts.Add(discount.Convert());
            //    }
            //}

            return model;
        }

        /// <summary>
        /// Converts the specified discount.
        /// </summary>
        /// <param name="discount">The discount.</param>
        /// <returns></returns>
        public static Models.Andromeda.Discount Convert(this Models.JustEat.DiscountItem discount) 
        {
            var model = new Models.Andromeda.Discount()
            {
                discountTypeAmount = System.Convert.ToInt32(discount.Discount * 100),
                discountType = discount.DiscountType.Equals("Percent", StringComparison.InvariantCultureIgnoreCase) ? "Percentage" : "Fixed",
            };

            return model;
        }

        /// <summary>
        /// Converts the specified customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        public static Models.Andromeda.Customer Convert(this Models.JustEat.CustomerInfo customer) 
        {
            var model = new Models.Andromeda.Customer() 
            {
                firstName = customer.Name,
                surname = string.Empty
            };

            model.address = new Models.Andromeda.Address() 
            {
                ZipCode = customer.Postcode,
                town = customer.City,
                roadName = customer.Address,
                //there is no country in the just eat feed. 
                country = "GB"
            };

            model.contacts = new List<Models.Andromeda.Contact>()
            {
                new Models.Andromeda.Contact()
                {
                    marketingLevel = MarketingType,
                    type = "Email",
                    value = customer.Email
                },
                new Models.Andromeda.Contact()
                {
                    marketingLevel = MarketingType,
                    type = "Mobile",
                    value = customer.PhoneNumber
                }
            };

            return model;
        }
    }
}