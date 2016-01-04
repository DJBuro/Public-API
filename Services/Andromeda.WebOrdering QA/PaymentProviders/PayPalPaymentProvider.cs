using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Andromeda.WebOrdering.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal;
using PayPal.Api.Payments;
using System.Configuration;
using System.IO;
using System.Text;
using PayPal.Exception;
using Andromeda.WebOrdering.Services;
using System.Globalization;

namespace Andromeda.WebOrdering.PaymentProviders
{
    public class PayPalPaymentProvider : IPaymentProvider
    {
        public const string TestWebserviceUrl = "https://testserver.datacash.com/Transaction";
        public const string LiveWebserviceUrl = "https://mars.transaction.datacash.com/Transaction";

        private static string GetPayPalUrl(string paymentProvider)
        {
            if (paymentProvider.Equals("PayPalTest", StringComparison.CurrentCultureIgnoreCase))
            {
                return PayPalPaymentProvider.TestWebserviceUrl;
            }
            else
            {
                return PayPalPaymentProvider.LiveWebserviceUrl;
            }
        }

        public static bool PutPayPalPayment
        (
            string key, 
            DomainConfiguration domainConfiguration, 
            string siteId, 
            string orderJson, 
            out HttpStatusCode httpStatus, 
            out string json
        )
        {
            bool success = false;
            json = "";
            httpStatus = HttpStatusCode.InternalServerError;

            OrderDetails orderDetails = new OrderDetails();
            try
            {
                success = OrderServices.GetOrderDetails(key, domainConfiguration, siteId, orderJson, out orderDetails);

                if (success)
                {
                    if (!orderDetails.PaymentProviderName.Equals("PayPalTest", StringComparison.CurrentCultureIgnoreCase) &&
                        !orderDetails.PaymentProviderName.Equals("PayPalLive", StringComparison.CurrentCultureIgnoreCase))
                    {
                        json = "{ \"errorCode\": \"" + Errors.PaypalPaymentFailed + "\" }";
                        success = false;
                    }
                }

                decimal price = 0;
                string orderReference = "";

                if (success)
                {
                    // Get the price
                    price = (decimal)orderDetails.OrderElement["pricing"]["priceAfterDiscount"];

                    // Create an order reference
                    orderReference = Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "");

                    // Replace the order reference in the order with our new order reference
                    JProperty partnerReferenceProperty = orderDetails.OrderElement.Property("partnerReference");
                    partnerReferenceProperty.Value = orderReference;

                    // Store the application id with the order
                    JProperty applicationIdProperty = new JProperty("hostHeader", domainConfiguration.HostHeader);
                    orderDetails.OrderElement.Add(applicationIdProperty);

                    // Store the application id with the order
                    JProperty toSiteIdProperty = new JProperty("toSiteId", siteId);
                    orderDetails.OrderElement.Add(toSiteIdProperty);
                }

                string payToken = "";
                if (success)
                {
                    // Call the PayPal web service that initialises payment
                    success = PayPalPaymentProvider.Initialise(domainConfiguration, orderDetails, orderReference, out payToken);
                }

                if (success)
                {
                    // Store the token with the order
                    JProperty tokenProperty = new JProperty("payToken", payToken);
                    orderDetails.OrderElement.Add(tokenProperty);

                    // Replace the order json with our modified version
                    orderJson = orderDetails.OrderElement.ToString(Newtonsoft.Json.Formatting.None);
                }

                if (success)
                {
                    // Was there a problem?
                    if (orderDetails.ReturnJson == null || orderDetails.ReturnJson.Length == 0)
                    {
                        orderDetails.ReturnJson = "{ \"errorCode\": \"" + Errors.InitialiseDatacashPaymentFailed + "\" }";
                        success = false;
                    }
                }

                if (success)
                {
                    // Drop the order to a file
                    string pendingOrdersFolder = ConfigurationManager.AppSettings["PayPalPendingOrdersFolder"];
                    PaymentProviderHelper.SaveOrderToFile(orderJson, orderReference, domainConfiguration, pendingOrdersFolder);

                    orderDetails.ReturnHttpStatus = HttpStatusCode.OK;
                }
            }
            catch (PayPalException exception)
            {
                if (exception.InnerException != null && exception.InnerException is ConnectionException)
                {
                    Global.Log.Error("Paypal exception: " + ((ConnectionException)exception.InnerException).Response, exception);
                }
                else
                {
                    Global.Log.Error("Unhandled exception", exception);
                }

                orderDetails.ReturnJson = "{ \"errorCode\": \"" + Errors.UnhandledException + "\" }";
                orderDetails.ReturnHttpStatus = HttpStatusCode.InternalServerError;
                success = false;
            }
            catch (Exception exception)
            {
                orderDetails.ReturnJson = "{ \"errorCode\": \"" + Errors.UnhandledException + "\" }";
                orderDetails.ReturnHttpStatus = HttpStatusCode.InternalServerError;
                success = false;
                Global.Log.Error("Unhandled exception: ", exception);
            }
            finally
            {
                httpStatus = orderDetails.ReturnHttpStatus;
                json = orderDetails.ReturnJson;
            }

            return success;
        }

        private static bool Initialise(DomainConfiguration domainConfiguration, OrderDetails orderDetails, string orderReference, out string payToken)
        {
            bool success = true;
            payToken = "";

            Dictionary<string, string> sdkConfig = new Dictionary<string, string>();

            // Are we using the test environment?
            if (orderDetails.PaymentProviderName == "PayPalTest")
            {
                sdkConfig.Add("mode", "sandbox");
            }

            string accessToken = new OAuthTokenCredential(orderDetails.MerchantId, orderDetails.Password, sdkConfig).GetAccessToken();

            APIContext apiContext = new APIContext(accessToken);
            apiContext.Config = sdkConfig;

            //Payee payee = new Payee()
            //{
            //    email = HttpUtility.UrlEncode(orderDetails.CustomerEmailAddress),
            //    phone = orderDetails.CustomerPhoneNumber
            //};
            Payer payer = new Payer()
            {
                payment_method = "paypal"//,
                //payer_info = new PayerInfo()
                //{
                //    email = orderDetails.CustomerEmailAddress,
                //    first_name = orderDetails.CustomerFirstName,
                //    last_name = orderDetails.CustomerSurname,
                //    phone = orderDetails.CustomerPhoneNumber
                //}
            };

            string line1 = "";
            line1 = PayPalPaymentProvider.AddAddressChunck(line1, orderDetails.OrderAddress.Org1);
            line1 = PayPalPaymentProvider.AddAddressChunck(line1, orderDetails.OrderAddress.Org2);

            string line2 = "";
            line2 = PayPalPaymentProvider.AddAddressChunck(line2, orderDetails.OrderAddress.Prem1);
            line2 = PayPalPaymentProvider.AddAddressChunck(line2, orderDetails.OrderAddress.Prem2);
            line2 = PayPalPaymentProvider.AddAddressChunck(line2, orderDetails.OrderAddress.RoadNumber);
            line2 = PayPalPaymentProvider.AddAddressChunck(line2, orderDetails.OrderAddress.RoadName);

            ItemList itemList = new PayPal.Api.Payments.ItemList();

            // If it's a delivery provide the shipping address
            if (orderDetails.OrderType.Equals("delivery", StringComparison.OrdinalIgnoreCase))
            {
                itemList.shipping_address = new ShippingAddress()
                {
                    city = orderDetails.OrderAddress.Town,
                    country_code = "GB",
                    line1 = line1.Length != 0 ? line1 : line2,
                    line2 = line2,
                    phone = orderDetails.CustomerPhoneNumber,
                    postal_code = orderDetails.OrderAddress.ZipCode,
                    recipient_name = "",
                    state = ""
                };
            }
            itemList.items = new List<Item>();
            foreach (OrderLine orderLine in orderDetails.OrderLines)
            {
                double price = orderLine.Price / Int32.Parse(orderLine.Quantity);

                string toppings = "";
                foreach (OrderLineTopping orderLineTopping in orderLine.AddToppings)
                {
                    toppings += (toppings.Length == 0 ? " - " : ",") + " add " + (orderLineTopping.Quantity == "2" ? " 2x " : "") + orderLineTopping.Name;
                    price += orderLineTopping.Price;
                }

                foreach (OrderLineTopping orderLineTopping in orderLine.RemoveToppings)
                {
                    toppings += (toppings.Length == 0 ? " - " : ",") + " remove " + (orderLineTopping.Quantity == "2" ? " 2x " : "") + orderLineTopping.Name;
                }

                Item newItem = new Item()
                {
                    currency = "GBP",
                    name = orderLine.Name += toppings,
                    price = price.ToString("0.00", CultureInfo.CurrentCulture),
                    quantity = orderLine.Quantity,
                    sku = "-" 
                };

                // The PayPal API only allows 127 characters for item names
                newItem.name = newItem.name.Length > 127 ? (newItem.name.Substring(0, 124) + "...") : newItem.name;

                itemList.items.Add(newItem);
            }

            string paypalLocalhostOverrideSetting = ConfigurationManager.AppSettings["paypalLocalhostOverride"];
            bool paypalLocalhostOverride = 
                (paypalLocalhostOverrideSetting != null && paypalLocalhostOverrideSetting.Equals("true", StringComparison.OrdinalIgnoreCase)) ? true : false;
               
            PayPal.Api.Payments.Payment payment = new PayPal.Api.Payments.Payment
            {
                transactions = new List<Transaction> 
                { 
                    new Transaction
                    {
                        amount = new Amount()
                        {
                            currency = "GBP",
                            total = orderDetails.Amount.ToString("0.00", CultureInfo.CurrentCulture),
                            details = new Details
                            {
                                subtotal = orderDetails.Amount.ToString("0.00", CultureInfo.CurrentCulture),
                                shipping = "0.00",
                                tax = "0.00",
                                fee = "0.00"
                            }
                        },
                        description = "Purchase From Application",
                        item_list = itemList//,
                        //payee = payee
                    } 
                },
                intent = "sale",
                payer = payer,
                redirect_urls = new RedirectUrls
                {
                    cancel_url = paypalLocalhostOverride ? "http://localhost/websites/webordering" : "http://" + domainConfiguration.HostHeader,
                    return_url = paypalLocalhostOverride ? "http://localhost/websites/webordering/WaitForOrderS.aspx" : "http://" + domainConfiguration.HostHeader + "/WaitForOrderS.aspx"
                }
            };

            PayPal.Api.Payments.Payment createdPayment = payment.Create(apiContext);

            if (createdPayment.state == "created")
            {
                Global.Log.Debug("DEBUG Create payment successful");

                orderDetails.ReturnJson =
                    "{" +
                        "\"url\":\"" + PayPalPaymentProvider.GetApprovalURL(createdPayment) + "\"," +
                        "\"amount\":\"" + orderDetails.Amount + "\"," +
                        "\"orderReference\":\"" + orderReference + "\"" +
                    "}";

                payToken = createdPayment.id;
            }
            else
            {
                Global.Log.Debug("ERROR Create payment failed");
            }

            return success;
        }

        private static string AddAddressChunck(string addressLine, string addressChunk)
        {
            if (addressLine.Length > 0)
            {
                addressLine += ", ";
            }

            addressLine += addressChunk;

            return addressLine;
        }

        private static string GetApprovalURL(PayPal.Api.Payments.Payment payment)
        {
            string redirectUrl = null;
            List<Links> links = payment.links;
            foreach (Links link in links)
            {
                if (link.rel.ToLower().Equals("approval_url"))
                {
                    redirectUrl = WebUtility.UrlDecode(link.href);
                    break;
                }
            }
            return redirectUrl;
        }

        public static bool PayPalCallback(DomainConfiguration domainConfiguration, string orderId, string requestJson, out string json)
        {
            bool success = false;
            json = "";

            // Extract data from the request
            JObject requestRootElement = JObject.Parse(requestJson);
            string token = (string)requestRootElement["token"];
            string payerId = (string)requestRootElement["payerId"];

            success = PayPalPaymentProvider.PaymentAccepted(domainConfiguration, orderId, token, payerId, out json);

            return success;
        }

        private static bool PaymentAccepted(DomainConfiguration domainConfiguration, string orderId, string token, string payerId, out string json)
        {
            json = "";
            bool success = false;

            try
            {
                // Load the order details from disk
                JObject orderDetailsJson = PaymentProviderHelper.LoadOrderFromFile(ConfigurationManager.AppSettings["PayPalPendingOrdersFolder"], orderId);

                PayPalOrderDetails payPalOrderDetails = new PayPalOrderDetails()
                {
                    DomainConfiguration = domainConfiguration,
                    OrderDetailsJson = PaymentProviderHelper.LoadOrderFromFile(ConfigurationManager.AppSettings["PayPalPendingOrdersFolder"], orderId),
                    SiteId = (string)orderDetailsJson["toSiteId"],
                    HostHeader = (string)orderDetailsJson["hostHeader"],
                    PayToken = (string)orderDetailsJson["payToken"],
                    PayerId = payerId,
                    OrderId = orderId
                };

                Global.Log.Debug("PayPalPaymentProvider ExecutePayment siteId:" + payPalOrderDetails.SiteId + " hostHeader:" + payPalOrderDetails.HostHeader);

                // Complete the PayPal payment
                OrderDetails orderDetails = new OrderDetails();
                success = PayPalPaymentProvider.Execute(payPalOrderDetails);

                // Was payment successfull?
                if (success)
                {
                    // Send the order to the store
                    success = PayPalPaymentProvider.PaymentTaken(payPalOrderDetails, out json);
                }
            }
            catch (PayPalException exception)
            {
                if (exception.InnerException != null && exception.InnerException is ConnectionException)
                {
                    Global.Log.Error("Paypal exception: " + ((ConnectionException)exception.InnerException).Response, exception);
                }
                else
                {
                    Global.Log.Error("Unhandled exception", exception);
                }

                json = "{ \"errorCode\":\"-1\" }";
            }
            catch (Exception exception)
            {
                json = "{ \"errorCode\":\"-1\" }";
                Global.Log.Error("Unhandled exception: ", exception);
            }

            return success;
        }

        private static bool Execute(PayPalOrderDetails payPalOrderDetails)
        {
            // Get the Payment provider details
            payPalOrderDetails.OrderDetails = new OrderDetails();
            bool success = Helper.GetPaymentProviderDetails("", payPalOrderDetails.DomainConfiguration, payPalOrderDetails.SiteId, payPalOrderDetails.OrderDetails);

            if (success)
            {
                Dictionary<string, string> sdkConfig = new Dictionary<string, string>();

                // Are we using the test environment?
                if (payPalOrderDetails.OrderDetails.PaymentProviderName == "PayPalTest")
                {
                    sdkConfig.Add("mode", "sandbox");
                }

                PaymentExecution paymentExecution = new PaymentExecution
                {
                    payer_id = payPalOrderDetails.PayerId
                };

                string accessToken = new OAuthTokenCredential(payPalOrderDetails.OrderDetails.MerchantId, payPalOrderDetails.OrderDetails.Password, sdkConfig).GetAccessToken();

                APIContext apiContext = new APIContext(accessToken);
                apiContext.Config = sdkConfig;

                // We must use the token returned in the initialisation web service call
                PayPal.Api.Payments.Payment payment = new PayPal.Api.Payments.Payment() { id = payPalOrderDetails.PayToken };
                PayPal.Api.Payments.Payment executedPayment = payment.Execute(apiContext, paymentExecution);

                // Was the payment processed successfully?
                if (!executedPayment.state.Equals("approved", StringComparison.OrdinalIgnoreCase))
                {
                    // No - somthing went wrong
                    Global.Log.Error("PayPalPaymentProvider ExecutePayment PayPal execute returned a state of '" + executedPayment.state + "' for order id: " + payPalOrderDetails.OrderId);
                    success = false;
                }
            }

            return success;
        }

        private static bool PaymentTaken(PayPalOrderDetails payPalOrderDetails, out string json)
        {
            bool success = true;

            // Payment has been taken
            Global.Log.Debug("PayPalPaymentProvider ExecutePayment successful");

            // Send the order to the store
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;

            success = OrderServices.PutOrder("", payPalOrderDetails.DomainConfiguration, payPalOrderDetails.SiteId, "{ order: " + payPalOrderDetails.OrderDetailsJson.ToString() + ", paymentType: '" + payPalOrderDetails.OrderDetails.PaymentProviderName + "' }", out httpStatus, out json);

            if (success)
            {
                // The order has been placed - move the file
                Global.Log.Debug("PayPalPaymentProvider ExecutePayment send order to store succeeded: httpStatus:" + httpStatus + " json:" + json);

                PaymentProviderHelper.PendingOrderCompleted
                (
                    ConfigurationManager.AppSettings["PayPalPendingOrdersFolder"],
                    ConfigurationManager.AppSettings["PayPalCompletedOrdersFolder"],
                    payPalOrderDetails.OrderId
                );
            }
            else
            {
                if (json == null || json.Length == 0)
                {
                    json = "{ \"errorCode\":\"-1\" }";
                }

                Global.Log.Debug("PayPalPaymentProvider ExecutePayment send order to store failed");
            }

            // Was there an problem?
            if (success == false)
            {
                // Send an elert email
                Global.Log.Error("PayPalPaymentProvider ExecutePayment Order.PutOrder failed with: " + json == null ? "no json" : json);
                PayPalPaymentProvider.SendAlertEmail("PayPalPaymentAlertEmail", payPalOrderDetails.DomainConfiguration, payPalOrderDetails.OrderDetailsJson.ToString(), payPalOrderDetails.OrderId);
            }

            return success;
        }

        #region IPaymentProvider

        internal static void SendAlertEmail
        (
            string templateName, 
            DomainConfiguration domainConfiguration, 
            string orderJson, 
            string orderId
        )
        {
            Global.Log.Info("SendAlertEmail SendAlertEmail");

            Email emailTemplate = null;
            if (domainConfiguration.TemplateEmails.TryGetValue(templateName, out emailTemplate))
            {
                string body = emailTemplate.Body;
                body = body.Replace("{ORDERID}", orderId);
                //body = body.Replace("{AMOUNT}", (double.Parse(mercanetResponse.Amount) / 100).ToString("0.00"));
                //body = body.Replace("{PAYMENTDATE}", mercanetResponse.PaymentDate);
                //body = body.Replace("{PAYMENTTIME}", mercanetResponse.PaymentTime);

                Alerts.SendAlert(
                    emailTemplate,
                    domainConfiguration.HostHeader,
                    emailTemplate.Subject,
                    body,
                    "order_" + orderId + ".json",
                    orderJson);
            }

            Global.Log.Info("SendAlertEmail SendAlertEmail Email queued");
        }

        public string BuildPaymentRollbackFailEmailBody(OrderDetails orderDetails, out Email templateEmail)
        {
            string body = "";

            if (orderDetails.DomainConfiguration.TemplateEmails.TryGetValue("PayPalPaymentAlertEmail", out templateEmail))
            {
                // Build the email body
                body = templateEmail.Body;
                body = body.Replace("{REFERENCE}", orderDetails.Payment.PSPSpecificDetails.DatacashPSP.UniqueTransactionReference);
            }

            return body;
        }

        public bool ProcessPayment(Model.OrderDetails orderDetails)
        {
            orderDetails.Payment = new Andromeda.WebOrdering.Model.Payment()
            {
                PaymentType = "Card",
                Value = ((decimal)orderDetails.OrderElement["pricing"]["priceAfterDiscount"]).ToString(),
                PaytypeName = null,
                AuthCode = null,
                LastFourDigits = null,
                CVVStatus = "",
                PayProcessor = "PayPal",
                PSPSpecificDetails = null
            };

            return true;
        }

        public bool RollbackPayment(OrderDetails orderDetails, string reference)
        {
            // We need the reference number for the payment that needs to be cancelled
            //string reference = orderDetails.Payment.PSPSpecificDetails.DatacashPSP.UniqueTransactionReference;

            //// Build the cancel xml to send to DataCash
            //string requestXml = PayPalPaymentProvider.CacncelWebServiceXml
            //    .Replace("<<<password>>>", orderDetails.Password)
            //    .Replace("<<<client>>>", orderDetails.MerchantId)
            //    .Replace("<<<reference>>>", reference);

            //// Call DataCash query web service
            //HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
            //string queryResponse = "";
            //bool success = HttpHelper.RestCall(
            //    "POST", 
            //    PayPalPaymentProvider.GetDataCashUrl(orderDetails.PaymentProviderName), 
            //    "Application/XML", 
            //    "Application/XML", 
            //    null,
            //    requestXml, 
            //    true, 
            //    out httpStatus, 
            //    out queryResponse);

            //orderDetails.ReturnHttpStatus = httpStatus;

            //if (!success)
            //{
            //    Global.Log.Error(
            //        "DataCash cancel failed httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
            //    success = false;
            //}

            //if (success)
            //{
            //    // Parse the response xml
            //    XElement responseXml = XElement.Parse(queryResponse);

            //    // Get the status
            //    int status = 0;
            //    var statusElement = responseXml.Elements("status").FirstOrDefault();
            //    if (statusElement == null || !int.TryParse(statusElement.Value, out status))
            //    {
            //        Global.Log.Error(
            //            "DataCash cancel missing status element httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
            //        success = false;
            //    }

            //    if (status != 1)
            //    {
            //        Global.Log.Error(
            //            "DataCash cancel returned status " + status.ToString() + " httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
            //        success = false;
            //    }
            //}

            //if (success)
            //{
            //    // We have successfully rolled back the payment.  There was still an error with the order though...
            //    orderDetails.ReturnJson = "{ \"errorCode\":\"" + Errors.PaymentRolledBack + "\", \"errorMessage\" : \"\" }";
            //    orderDetails.ReturnHttpStatus = HttpStatusCode.InternalServerError;
            //    success = false;
            //}
            //else
            //{
            //    // There was a problem rolling back the payment
            //    orderDetails.ReturnJson = "{ \"errorCode\":\"" + Errors.PaymentRolledBackFailed + "\", \"errorMessage\" : \"\" }";
            //    orderDetails.ReturnHttpStatus = httpStatus;
            //}

            //return success;

            return false;
        }

        #endregion
    }
}