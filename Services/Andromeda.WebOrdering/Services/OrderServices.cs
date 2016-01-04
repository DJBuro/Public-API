using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Andromeda.WebOrdering.Model;
using Andromeda.WebOrdering.PaymentProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Andromeda.WebOrdering.Services
{
    public class OrderServices
    {
        /// <summary>
        /// Sends an order to ACS
        /// </summary>
        /// <param name="key"></param>
        /// <param name="domainConfiguration"></param>
        /// <param name="siteId"></param>
        /// <param name="orderJson"></param>
        /// <param name="httpStatus"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool PutOrder(string key, DomainConfiguration domainConfiguration, string siteId, string orderJson, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";

            try
            {
                OrderDetails orderDetails = null;

                // Gather all the order details together into an object we can pass around
                success = OrderServices.GetOrderDetails(key, domainConfiguration, siteId, orderJson, out orderDetails);

                if (success)
                {
                    // Process the payment
                    success = OrderServices.ProcessPayments(orderDetails);
                }

                if (success)
                {
                    // Send the order to ACS
                    List<ACSError> errors = new List<ACSError>();
                    success = OrderServices.SendOrder(domainConfiguration, orderDetails, errors);

                    if (!success)
                    {
                        // There was a problem sending the order to the store
                        // We may need to send an alert email
                        // If there was a card payment we'll probably need to roll it back
                        OrderServices.ProcessErrors(errors, orderDetails);
                    }
                }

                if (orderDetails != null)
                {
                    json = orderDetails.ReturnJson;
                    
                    httpStatus = orderDetails.ReturnHttpStatus;
                }
            }
            catch(Exception exception)
            {
                Logger.Log.Error("PutOrder unhandled exception", exception);

                json = "{ \"errorCode\":\"" + Errors.UnhandledException + "\"}";

                success = false;
            }

            return success;
        }

        private static void GetPaymentProvider(OrderDetails orderDetails)
        {
            Logger.Log.Debug("GetPaymentProvider");

            // Create an appropriate payment provider
            switch (orderDetails.PaymentType.ToUpper())
            {
                case "MERCURY":
                    orderDetails.PaymentProvider = new MercuryPaymentProvider();
                    break;
                case "DATACASHTEST":
                case "DATACASHLIVE":
                    orderDetails.PaymentProvider = new DataCashPaymentProvider();
                    break;
                case "MERCANETTEST":
                case "MERCANETLIVE":
                    orderDetails.PaymentProvider = new MercanetPaymentProvider();
                    break;
                case "PAYPALTEST":
                case "PAYPALLIVE":
                    orderDetails.PaymentProvider = new PayPalPaymentProvider();
                    break;
                default:
                    orderDetails.PaymentProvider = new PayLaterPaymentProvider();
                    break;
            }
        }

        private static bool ProcessPayments(OrderDetails orderDetails)
        {
            Logger.Log.Debug("PutOrder paymentType=" + orderDetails.PaymentType);

            bool success = false;

            // Check to see if the payment succeeded
            success = orderDetails.PaymentProvider.ProcessPayment(orderDetails);

            if (success)
            {
                // Add the payment to the order
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                JObject paymentObject = JObject.FromObject(orderDetails.Payment);
                orderDetails.Payments.Add(paymentObject);
            }

            return success;
        }

        internal static bool SendOrder(DomainConfiguration domainConfiguration, OrderDetails orderDetails, List<ACSError> errors)
        {
            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
            bool success = HostServices.GetServerUrl(orderDetails.DomainConfiguration, out httpStatus, out serverUrls);
            if (!success) orderDetails.ReturnHttpStatus = httpStatus;

            bool isProvisional = false;
            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    // Build the web service url for this server
                    string url = serverUrl + "/sites/" + orderDetails.SiteId + "/orders/" + orderDetails.OrderId + "?applicationid=" + orderDetails.DomainConfiguration.ApplicationId;

                    // Send the order to the ACS server
                    string json = "";
                    success = HttpHelper.RestCall(
                        "PUT", 
                        url, 
                        "Application/JSON", 
                        "Application/JSON", 
                        null, 
                        orderDetails.OrderElement.ToString(), 
                        true, 
                        out httpStatus, 
                        out json);

                    Logger.Log.Info("OrderServices.SendOrder returned json: " + json);

                    Model.ACSError error = null;
                    if (json != null && json.Length > 0)
                    {
                        // Was there an error?
                        success = Helper.CheckOrderResult(ref json, out isProvisional, out error);
                    }
                    else
                    {
                        error = new ACSError() { ErrorCode = "-1000" };
                    }

                    if (success)
                    {
                        // Order placed successfully!  No need to try any more servers
                        orderDetails.ReturnHttpStatus = HttpStatusCode.OK;
                        orderDetails.ReturnJson = json;
                        break;
                    }
                    else 
                    {
                        // Keep track of the error and try the next ACS server
                        errors.Add(error);
                    }
                }
            }

            if (success && 
                orderDetails.ReturnHttpStatus == HttpStatusCode.OK && 
                orderDetails.CustomerEmailAddress != null && 
                orderDetails.CustomerEmailAddress.Length > 0 &&
                isProvisional == false)
            {
                // Send the confirmation email (only if the order has definately been received & accepted by the store)
                success = OrderServices.SendConfirmationEmail(domainConfiguration, orderDetails);
            }

            return success;
        }

        private static bool SendConfirmationEmail(DomainConfiguration domainConfiguration, OrderDetails orderDetails)
        {
            bool success = true;

            // Queue order confirmation email
            if (domainConfiguration.TemplateEmails == null || domainConfiguration.TemplateEmails.Count == 0)
            {
                Logger.Log.Info("OrderServices.SendConfirmationEmail Unable to send email - no email templates!");
            }
            else
            {
                Email confirmationEmailTemplate = null;
                string emailTemplateName = orderDetails.OrderType.ToUpper() == "DELIVERY" ? "DeliveryOrderReceiptEmail" : "CollectionOrderReceiptEmail";
                if (domainConfiguration.TemplateEmails.TryGetValue(emailTemplateName, out confirmationEmailTemplate))
                {
                    string body = confirmationEmailTemplate.Body;

                    string orderLineTemplate = OrderServices.ExtractTemplateBlock("{orderLine}", "{/orderLine}", ref body);
                    string addOptionTemplate = OrderServices.ExtractTemplateBlock("{addOption}", "{/addOption}", ref body);
                    string removeOptionTemplate = OrderServices.ExtractTemplateBlock("{removeOption}", "{/removeOption}", ref body);

                    // Build a list of order lines
                    StringBuilder orderLines = new StringBuilder();
                    if (orderLineTemplate.Length > 0)
                    {
                        foreach (OrderLine orderLine in orderDetails.OrderLines)
                        {
                            string orderLineText = orderLineTemplate
                                .Replace("{itemQuantity}", orderLine.Quantity)
                                .Replace("{itemQuantityTimes}", orderLine.Quantity)
                                .Replace("{itemName}", orderLine.Name)
                                .Replace("{itemPrice}", orderLine.Price.ToString("0.00"))
                                .Replace("{person}", String.IsNullOrEmpty(orderLine.Person) ? "" : orderLine.Person)
                                .Replace("{instructions}", String.IsNullOrEmpty(orderLine.Instructions) ?"" : orderLine.Instructions);

                            foreach (OrderLineTopping orderLineTopping in orderLine.AddToppings)
                            {
                                orderLineText += addOptionTemplate
                                    .Replace("{itemQuantity}", orderLineTopping.Quantity)
                                    .Replace("{itemQuantityTimes}", orderLineTopping.Quantity)
                                    .Replace("{itemName}", orderLineTopping.Name)
                                    .Replace("{itemPrice}", orderLineTopping.Price.ToString("0.00"));
                            }

                            foreach (OrderLineTopping orderLineTopping in orderLine.RemoveToppings)
                            {
                                orderLineText += removeOptionTemplate
                                    .Replace("{itemQuantity}", orderLineTopping.Quantity)
                                    .Replace("{itemQuantityTimes}", orderLineTopping.Quantity)
                                    .Replace("{itemName}", orderLineTopping.Name)
                                    .Replace("{itemPrice}", orderLineTopping.Price.ToString("0.00"));
                            }

                            orderLines.Append(orderLineText);
                        }
                    }

                    body = body
                        .Replace("{domain}", domainConfiguration.HostHeader)
                        .Replace("{orderLines}", orderLines.ToString())
                        .Replace("{orderTotal}", orderDetails.Amount.ToString("0.00"))
                        .Replace("{chefNotes}", orderDetails.ChefNotes.Length == 0 ? "-" : orderDetails.ChefNotes)
                        .Replace("{customerTitle}", orderDetails.CustomerTitle)
                        .Replace("{customerFirstName}", orderDetails.CustomerFirstName)
                        .Replace("{customerSurname}", orderDetails.CustomerSurname);

                    if (orderDetails.OrderType.ToUpper() == "DELIVERY")
                    {
                        string roadNumberRoadName = orderDetails.OrderAddress.RoadNumber != null && orderDetails.OrderAddress.RoadNumber.Length > 0 ? orderDetails.OrderAddress.RoadNumber : "";
                        roadNumberRoadName += orderDetails.OrderAddress.RoadName != null && orderDetails.OrderAddress.RoadName.Length > 0
                            ? (roadNumberRoadName.Length > 0 ? " " : "") + orderDetails.OrderAddress.RoadName : "";

                        body = body
                            .Replace("{deliveryTime}", orderDetails.OrderWantedTime)
                            .Replace("{org1}", orderDetails.OrderAddress.Org1 != null && orderDetails.OrderAddress.Org1.Length > 0 ? orderDetails.OrderAddress.Org1 + "<br />\r\n" : "")
                            .Replace("{org2}", orderDetails.OrderAddress.Org2 != null && orderDetails.OrderAddress.Org2.Length > 0 ? orderDetails.OrderAddress.Org2 + "<br />\r\n" : "")
                            .Replace("{prem1}", orderDetails.OrderAddress.Prem1 != null && orderDetails.OrderAddress.Prem1.Length > 0 ? orderDetails.OrderAddress.Prem1 + "<br />\r\n" : "")
                            .Replace("{prem2}", orderDetails.OrderAddress.Prem2 != null && orderDetails.OrderAddress.Prem2.Length > 0 ? orderDetails.OrderAddress.Prem2 + "<br />\r\n" : "")
                            .Replace("{roadNumber}", orderDetails.OrderAddress.RoadNumber != null && orderDetails.OrderAddress.RoadNumber.Length > 0 ? orderDetails.OrderAddress.RoadNumber + "<br />\r\n" : "")
                            .Replace("{roadName}", orderDetails.OrderAddress.RoadName != null && orderDetails.OrderAddress.RoadName.Length > 0 ? orderDetails.OrderAddress.RoadName + "<br />\r\n" : "")
                            .Replace("{roadNumberRoadName}", roadNumberRoadName != null && roadNumberRoadName.Length > 0 ? roadNumberRoadName + "<br />\r\n" : "")
                            .Replace("{town}", orderDetails.OrderAddress.Town != null && orderDetails.OrderAddress.Town.Length > 0 ? orderDetails.OrderAddress.Town + "<br />\r\n" : "")
                            .Replace("{userLocality1}", orderDetails.OrderAddress.UserLocality != null && orderDetails.OrderAddress.UserLocality.Length > 0 ? orderDetails.OrderAddress.UserLocality + "<br />\r\n" : "")
                            .Replace("{zipCode}", orderDetails.OrderAddress.ZipCode != null && orderDetails.OrderAddress.ZipCode.Length > 0 ? orderDetails.OrderAddress.ZipCode + "<br />\r\n" : "")
                            .Replace("{directions}", orderDetails.OrderAddress.Directions != null && orderDetails.OrderAddress.Directions.Length > 0 ? "<br />" + orderDetails.OrderAddress.Directions + "<br />\r\n" : "")
                            .Replace("{oneOffDirections}", orderDetails.OneOffDirections != null && orderDetails.OneOffDirections.Length > 0 ? "<br />" + orderDetails.OneOffDirections + "<br />\r\n" : "");
                    }
                    else
                    {
                        body = body
                            .Replace("{collectionTime}", orderDetails.OrderWantedTime);
                    }

                    // Payment
                    string payment = "";
                    if (orderDetails.PaymentType.ToUpper() == "PAYLATER")
                    {
                        if (orderDetails.OrderType.ToUpper() == "DELIVERY")
                        {
                            payment = "you have chosen to pay on delivery";
                        }
                        else
                        {
                            payment = "you have chosen to pay on collection";
                        }
                    }
                    else
                    {
                        payment = "this order has been paid for online";
                    }
                    body = body
                            .Replace("{payment}", payment);

                    // Queue the email to be sent in the background
                    BackgroundServices.QueueEmail
                    (
                        new Email()
                        {
                            To = orderDetails.CustomerEmailAddress,
                            From = confirmationEmailTemplate.From,
                            Body = body,
                            Subject = confirmationEmailTemplate.Subject,
                            HostHeader = domainConfiguration.HostHeader,
                            Server = confirmationEmailTemplate.Server,
                            ServerType = confirmationEmailTemplate.ServerType,
                            Username = confirmationEmailTemplate.Username,
                            Password = confirmationEmailTemplate.Password,
                            AttachmentFilename = null,
                            Attachment = null
                        }
                    );

                    Logger.Log.Info("CustomerServices.RequestResetPassword Email queued");
                }
                else
                {
                    Logger.Log.Error("CustomerServices.SendConfirmationEmail Unable to send email - missing " + emailTemplateName + " email template");
                }
            }

            return success;
        }

        private static string ExtractTemplateBlock(string startTag, string endTag, ref string body)
        {
            string templateBlock = "";

            int templateBlockStartIndex = body.IndexOf(startTag);
            int templateBlockEndIndex = body.IndexOf(endTag);

            if (templateBlockStartIndex > -1 && templateBlockEndIndex > -1)
            {
                // Extract the template block
                templateBlock = body.Substring(templateBlockStartIndex + startTag.Length, templateBlockEndIndex - (templateBlockStartIndex + startTag.Length));

                // Remove the template block from the body
                body = body.Substring(0, templateBlockStartIndex) + body.Substring(templateBlockEndIndex + endTag.Length);
            }

            return templateBlock;
        }

        internal static void ProcessErrors(List<ACSError> errors, OrderDetails orderDetails)
        {
            bool rollbackPayment = true;
            bool manualRollbackRequired = false;
            bool allErrorsSame = true;
            string errorCode = "";
            Logger.Log.Debug("ProcessErrors error0=" + (errors.Count > 0 && errors[0] == null ? "null" : "ok"));
            if (errors.Count > 0)
            {
                errorCode = errors[0].ErrorCode;
            }
            
            // Work out if all ACS servers returned the same error.  
            // At the same time if any of the ACS servers returned a timeout don't rollback payment as the order *might* have gotten through
            foreach (ACSError acsError in errors)
            {
                // Was it a timeout error?
                if (acsError.ErrorCode == "2150")
                {
                    // We got a timeout.  The order may or may not have gone through so don't rollback card payment
                    rollbackPayment = false;
                }

                // Is this a different error (are any of the errors different)
                if (acsError.ErrorCode != errorCode)
                {
                    allErrorsSame = false;
                }
            }

            // Are the errors returned by the ACS servers the same?
            if (allErrorsSame)
            {
                orderDetails.ReturnJson = "{ \"errorCode\":\"" + errorCode + "\"}";
            }

            // Failed to send the order to the store
            if (rollbackPayment)
            {
                // We can be reasonably sure the order didn't get through to the store
                // If the customer paid by card then do a rollback
                if (orderDetails.PaymentType != null && 
                    orderDetails.PaymentType.Length > 0 &&
                    orderDetails.PaymentType.ToUpper() != "PAYLATER")
                {
                    // Attempt to do the payment rollback
                    bool success = OrderServices.RollbackPayment(orderDetails);

                    // Did the rollback work?
                    if (success)
                    {
                        // Tell AndroWeb to forget about the previous payment - it's been cancelled
                        orderDetails.ReturnJson = "{ \"errorCode\":\"" + errorCode + "\" }";
                    }
                    else
                    {
                        // Rollback failed so we'll need to send an additional alert as a manual payment rollback is now required
                        manualRollbackRequired = true;
                    }
                }
            }

            // Do we need to send an alert email?
            Email templateEmail = null;
            if (orderDetails.DomainConfiguration != null &&
                orderDetails.DomainConfiguration.TemplateEmails != null &&
                orderDetails.DomainConfiguration.TemplateEmails.Count > 0)
            {
                string body = "";
                errorCode = allErrorsSame ? errorCode : "";

                if (manualRollbackRequired)
                {
                    // Send an alert email warning that a manual payment rollback is needed
                    // Let the payment provider build the email body so it can add provider specific information
                    body = orderDetails.PaymentProvider.BuildPaymentRollbackFailEmailBody(orderDetails, out templateEmail);

                    if (body.Length > 0)
                    {
                        string subject = OrderServices.InjectEmailValues(templateEmail.Subject, errorCode, orderDetails);
                        body = OrderServices.InjectEmailValues(body, errorCode, orderDetails);

                        // Send the alert 
                        Alerts.SendAlert(
                            templateEmail,
                            orderDetails.DomainConfiguration.HostHeader,
                            subject,
                            body,
                            "order_" + orderDetails.OrderId + ".json",
                            orderDetails.OrderElement.ToString());
                    }
                }
                
                if (orderDetails.DomainConfiguration.TemplateEmails.TryGetValue("OrderAlertEmail", out templateEmail))
                {
                    Logger.Log.Debug("ProcessErrors sending order alert email");

                    // The order failed to go through to the store - send an alert email
                    // Build the email body
                    body = templateEmail.Body;

                    if (body.Length > 0)
                    {
                        string subject = OrderServices.InjectEmailValues(templateEmail.Subject, errorCode, orderDetails);
                        body = OrderServices.InjectEmailValues(body, errorCode, orderDetails);

                        // Send the alert 
                        Alerts.SendAlert(
                            templateEmail,
                            orderDetails.DomainConfiguration.HostHeader,
                            subject,
                            body,
                            "order_" + orderDetails.OrderId + ".json",
                            orderDetails.OrderElement.ToString());
                    }
                }
            }
        }

        internal static string InjectEmailValues(string text, string errorCode, OrderDetails orderDetails)
        {
            text = text.Replace("{ERRORCODE}", errorCode);
            text = text.Replace("{WEBSITE}", orderDetails.DomainConfiguration.HostHeader);
            text = text.Replace("{SITEID}", orderDetails.SiteId);
            text = text.Replace("{ORDERID}", orderDetails.OrderId);
            text = text.Replace("{AMOUNT}", orderDetails.Amount.ToString("0.00"));

            JValue orderPlaced = (JValue)orderDetails.OrderElement["orderPlacedTime"];
            if (orderPlaced != null)
            {
                DateTime orderPlacedDateTime;

                if (DateTime.TryParse(orderPlaced.ToString(), out orderPlacedDateTime))
                {
                    text = text.Replace("{DATE}", orderPlacedDateTime.Year.ToString("00") + "/" + orderPlacedDateTime.Month.ToString("00") + "/" + orderPlacedDateTime.Day.ToString("00"));
                    text = text.Replace("{TIME}", orderPlacedDateTime.Hour.ToString("00") + ":" + orderPlacedDateTime.Minute.ToString("00") + ":" + orderPlacedDateTime.Second.ToString("00"));
                }
            }

            return text;
        }

        private static bool RollbackPayment(OrderDetails orderDetails)
        {
            bool success = true;
            Logger.Log.Debug("PutOrder rollback payment paymentType=" + orderDetails.PaymentType);
            orderDetails.ReturnHttpStatus = HttpStatusCode.BadRequest;

            // Attempt to rollback the payment
            success = orderDetails.PaymentProvider.RollbackPayment(orderDetails, orderDetails.Payment.PSPSpecificDetails.DatacashPSP.UniqueTransactionReference);

            return success;
        }

        internal static bool GetOrderDetails(
            string key, 
            DomainConfiguration domainConfiguration, 
            string siteId, 
            string orderJson,
            out OrderDetails orderDetails)
        {
            bool success = false;
            orderDetails = null;

            // Extract data from the order
            JObject orderRootElement = JObject.Parse(orderJson);
            string paymentType = (string)orderRootElement["paymentType"];

            // Get the order
            JObject orderElement = (JObject)orderRootElement["order"];

            // Get the order reference
            string orderId = (string)orderElement["partnerReference"];

            // Is there a partner reference?
            if (orderId == null || orderId.Length == 0)
            {
                // No partner reference.  Generate one.
                orderId = Helper.GenerateOrderNumber();
                orderElement["partnerReference"] = orderId;
            }

            // Get the order details
            string orderType = (string)orderElement["type"];
            string orderWantedTime = (string)orderElement["orderWantedTime"];
            string chefNotes = (string)orderElement["chefNotes"];
            string oneOffDirections = (string)orderElement["oneOffDirections"];

            // Get the customers name
            JObject customerDataElement = (JObject)orderElement["customer"];
            string title = (string)customerDataElement["title"];
            string firstName = (string)customerDataElement["firstName"];
            string surname = customerDataElement["surname"] == null ? "" : (string)customerDataElement["surname"];

            // Get the customers email address
            string emailAddress = "";
            string phoneNumber = "";
            JArray contactElements = (JArray)orderElement["customer"]["contacts"];
            foreach (JObject contactElement in contactElements)
            {
                if (((string)contactElement["type"]).ToUpper() == "EMAIL")
                {
                    emailAddress = (string)contactElement["value"];
                }
                else if (((string)contactElement["type"]).ToUpper() == "MOBILE")
                {
                    phoneNumber = (string)contactElement["value"];
                }
            }

            // Get the customers address
            string org1 = "";
            string org2 = "";
            string prem1 = "";
            string prem2 = "";
            string roadNumber = "";
            string roadName = "";
            string town = "";
            string userLocality = "";
            string zipCode = "";
            string directions = "";

            JObject addressDataElement = (JObject)orderElement["customer"]["address"];
            if (addressDataElement != null)
            {
                org1 = (string)addressDataElement["org1"];
                org2 = (string)addressDataElement["org2"];
                prem1 = (string)addressDataElement["prem1"];
                prem2 = (string)addressDataElement["prem2"];
                roadNumber = (string)addressDataElement["roadNum"];
                roadName = (string)addressDataElement["roadName"];
                town = (string)addressDataElement["town"];
                userLocality = (string)addressDataElement["userLocality"];
                zipCode = (string)addressDataElement["zipCode"];
                if (zipCode == null)
                {
                    zipCode = (string)addressDataElement["postcode"];
                }
                directions = (string)addressDataElement["directions"];
            }

            // Get the order lines
            List<OrderLine> orderLines = new List<OrderLine>();
            JArray orderLineElements = (JArray)orderElement["orderLines"];
            foreach (JObject orderLineElement in orderLineElements)
            {
                OrderLine orderLine = new OrderLine()
                {
                    Quantity = (string)orderLineElement["quantity"],
                    Name = (string)orderLineElement["name"],
                    Price = OrderServices.PenceTextToDouble((string)orderLineElement["price"]),
                    Person = (string)orderLineElement["person"],
                    Instructions = (string)orderLineElement["instructions"]
                };

                // Add toppings
                orderLine.AddToppings = new List<OrderLineTopping>();
                JArray addToppingElements = (JArray)orderLineElement["addToppings"];

                foreach (JObject addToppingElement in addToppingElements)
                {
                    orderLine.AddToppings.Add
                    (
                        new OrderLineTopping()
                        {
                            Quantity = (string)addToppingElement["quantity"],
                            Name = (string)addToppingElement["name"],
                            Price = OrderServices.PenceTextToDouble((string)addToppingElement["price"])
                        }
                    );
                }

                // Remove toppings
                orderLine.RemoveToppings = new List<OrderLineTopping>();
                JArray removeToppingElements = (JArray)orderLineElement["removeToppings"];

                foreach (JObject removeToppingElement in removeToppingElements)
                {
                    orderLine.RemoveToppings.Add
                    (
                        new OrderLineTopping()
                        {
                            Quantity = (string)removeToppingElement["quantity"],
                            Name = (string)removeToppingElement["name"],
                            Price = OrderServices.PenceTextToDouble((string)removeToppingElement["price"])
                        }
                    );
                }

                orderLines.Add(orderLine);
            }

            // The final price (NOT including delivery charge!)
            double finalPrice = OrderServices.PenceTextToDouble((string)orderElement["pricing"]["finalPrice"]);

            // Get the payments
            JArray payments = (JArray)orderElement["orderPayments"];
            JObject paymentDataElement = (JObject)orderRootElement["paymentData"];

            // Add the card charge
            double paymentCharge = 0;
            if (paymentDataElement != null)
            {
                JToken paymentChargeElement = paymentDataElement["paymentCharge"];
                if (paymentChargeElement != null)
                {
                    string paymentChargeText = (string)paymentChargeElement;
                    if (paymentChargeText.Length > 0)
                    {
                        paymentCharge = OrderServices.PenceTextToDouble(paymentChargeText);
                        finalPrice += paymentCharge;
                    }
                }
            }

            // Add the delivery charge
            JToken deliveryChargeElement = orderElement["pricing"]["deliveryCharge"];
            double deliveryCharge = 0;
            if (deliveryChargeElement != null)
            {
                string deliveryChargeText = (string)deliveryChargeElement;
                if (deliveryChargeText.Length > 0)
                {
                    deliveryCharge = OrderServices.PenceTextToDouble(deliveryChargeText);
                    finalPrice += deliveryCharge;
                }
            }

            // Pull all of the order data together so we can pass it around more efficiently
            orderDetails = new OrderDetails()
            {
                DomainConfiguration = domainConfiguration,
                SiteId = siteId,
                OrderId = orderId,
                PaymentType = paymentType,
                Key = key,
                OrderElement = orderElement,
                OrderRootElement = orderRootElement,
                PaymentDataElement = paymentDataElement,
                Payments = payments,
                Payment = null,
                ReturnHttpStatus = HttpStatusCode.InternalServerError,
                ReturnJson = "",
                PaymentProviderName = "",
                IsOnline = false,
                Password = "",
                MerchantId = "",
                PaymentProvider = null,
                Amount = finalPrice,
                CustomerEmailAddress = emailAddress,
                OrderType = orderType,
                OrderWantedTime = orderWantedTime,
                OrderAddress = new OrderAddress()
                {
                    Org1 = org1,
                    Org2 = org2,
                    Prem1 = prem1,
                    Prem2 = prem2,
                    RoadNumber = roadNumber,
                    RoadName = roadName,
                    Town = town,
                    UserLocality = userLocality,
                    ZipCode = zipCode,
                    Directions = directions
                },
                OrderLines = orderLines,
                OneOffDirections = oneOffDirections,
                ChefNotes = chefNotes,
                CustomerTitle = title,
                CustomerFirstName = firstName,
                CustomerSurname = surname,
                CustomerPhoneNumber = phoneNumber,
                DeliveryCharge = deliveryCharge,
                PaymentCharge = paymentCharge
            };

            // Get the Payment provider details
            success = Helper.GetPaymentProviderDetails(orderDetails.Key, orderDetails.DomainConfiguration, orderDetails.SiteId, orderDetails);

            // Set the payment provider that can process payments for this order
            OrderServices.GetPaymentProvider(orderDetails);

            return success;
        }

        private static double PenceTextToDouble(string penceText)
        {
            return double.Parse(penceText) / 100;
        }

        public static bool CheckOrderVouchers(string key, DomainConfiguration domainConfiguration, string siteId, string orderJson, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";

            try
            {
                // Send the order to ACS to check the vouchers
                List<ACSError> errors = new List<ACSError>();
                success = OrderServices.SendCheckOrderVouchers(domainConfiguration, siteId, orderJson, errors, out httpStatus, out json);
            }
            catch(Exception exception)
            {
                Logger.Log.Error("CheckOrderVouchers unhandled exception", exception);

                json = "{ \"errorCode\":\"" + Errors.UnhandledException + "\"}";

                success = false;
            }

            return success;
        }

        private static bool SendCheckOrderVouchers(
            DomainConfiguration domainConfiguration, 
            string siteId, 
            string orderJson, 
            List<ACSError> errors, 
            out HttpStatusCode httpStatus, 
            out string resultJson)
        {
            resultJson = "";
            httpStatus = HttpStatusCode.InternalServerError;

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            bool success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    // Build the web service url for this server
                    string url = serverUrl + "/sites/" + siteId + "/ordervouchers?applicationid=" + domainConfiguration.ApplicationId;

                    // Send the order to the ACS server
                    string json = "";
                    success = HttpHelper.RestCall(
                        "PUT",
                        url,
                        "Application/JSON",
                        "Application/JSON",
                        null,
                        orderJson,
                        true,
                        out httpStatus,
                        out json);

                    Logger.Log.Info("OrderServices.SendCheckOrderVouchers returned json: " + json);

                    Model.ACSError error = null;
                    if (json != null && json.Length > 0)
                    {
                        // Was there an error?
                        success = Helper.CheckForError(ref json, out error);
                    }
                    else
                    {
                        error = new ACSError() { ErrorCode = "-1000" };
                    }

                    if (success)
                    {
                        // Order placed successfully!  No need to try any more servers
                        httpStatus = HttpStatusCode.OK;
                        resultJson = json;
                        break;
                    }
                    else
                    {
                        // Keep track of the error and try the next ACS server
                        errors.Add(error);
                    }
                }
            }

            return success;
        }
    }
}