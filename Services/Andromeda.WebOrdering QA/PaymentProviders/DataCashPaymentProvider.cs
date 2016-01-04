using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Andromeda.WebOrdering.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Andromeda.WebOrdering.Services;
using Newtonsoft.Json.Serialization;

namespace Andromeda.WebOrdering.PaymentProviders
{
    public class DataCashPaymentProvider : IPaymentProvider
    {
        public const string TestWebserviceUrl = "https://testserver.datacash.com/Transaction";
        public const string LiveWebserviceUrl = "https://mars.transaction.datacash.com/Transaction";
        public const string SetupWebServiceXml =
            "<Request> " +
                "<Authentication> " +
                    "<password><<<password>>></password> " +
                    "<client><<<client>>></client>" +
                "</Authentication> " +
                "<Transaction> " +
                    "<TxnDetails> " +
                        "<merchantreference><<<merchantreference>>></merchantreference> " +
                        "<amount currency=\"GBP\"><<<amount>>></amount> " +
                        "<capturemethod>ecomm</capturemethod> " +
                        "<<<threedsecure>>>" +
                    "</TxnDetails> " +
                    "<CardTxn> " +
                        "<method>auth</method> " +
                        "<Card>" +
                            "<Cv2Avs> " +
                                "<policy>2</policy> " +
                            "</Cv2Avs> " +
                        "</Card> " +
                    "</CardTxn>" +
                    "<HpsTxn>" +
                        "<method>setup_full</method>" +
                        "<page_set_id><<<pageSetId>>></page_set_id> " +
                        "<return_url><<<url>>></return_url> " +
                    "</HpsTxn>" +
                "</Transaction>" +
            "</Request>";

        public const string ThreeDSecure =
             "<ThreeDSecure> " +
                "<verify>yes</verify> " +
                "<merchant_url><<<merchanturl>>></merchant_url> " +
                "<purchase_desc>Food</purchase_desc> " +
                "<purchase_datetime><<<transactiondatetime>>></purchase_datetime> " +
                "<Browser> " +
                    "<device_category>0</device_category> " +
                    "<accept_headers><<<browserheaders>>></accept_headers> " +
                    "<user_agent><<<browseruseragent>>></user_agent> " +
                "</Browser> " +
            "</ThreeDSecure> ";

        public const string QueryWebServiceXml =
            "<Request>" +
                "<Authentication>" +
                    "<password><<<password>>></password> " +
                    "<client><<<client>>></client>" +
                "</Authentication>" +
                "<Transaction> " +
                    "<HistoricTxn>" +
                        "<method>query</method>" +
                        "<reference><<<reference>>></reference> " +
                    "</HistoricTxn> " +
                "</Transaction> " +
            "</Request>";

        public const string CancelWebServiceXml =
            "<Request> " +
                "<Authentication> " +
                    "<password><<<password>>></password> " +
                    "<client><<<client>>></client>" +
                "</Authentication> " +
                "<Transaction> " +
                    "<HistoricTxn> " +
                        "<reference><<<reference>>></reference> " +
                        "<method>cancel</method> " +
                    "</HistoricTxn> " +
                "</Transaction>" +
            "</Request>";

        private static string GetDataCashUrl(string paymentProvider)
        {
            if (paymentProvider.Equals("DataCashTest", StringComparison.CurrentCultureIgnoreCase))
            {
                return DataCashPaymentProvider.TestWebserviceUrl;
            }
            else
            {
                return DataCashPaymentProvider.LiveWebserviceUrl;
            }
        }

        /// <summary>
        /// Initialises a brand new DataCash payment and returns the customer facing Datacash payment page URL.
        /// Detects if a payment has already been completed and sends the order to ACS instead of creating a new payment.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="domainConfiguration"></param>
        /// <param name="siteId"></param>
        /// <param name="orderJson"></param>
        /// <param name="httpStatus"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool PutDataCashPayment
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
                // Gather all the order details together into an object we can pass around
                success = OrderServices.GetOrderDetails(key, domainConfiguration, siteId, orderJson, out orderDetails);

                if (success)
                {
                    if (!orderDetails.PaymentProviderName.Equals("DataCashTest", StringComparison.CurrentCultureIgnoreCase) &&
                        !orderDetails.PaymentProviderName.Equals("DataCashLive", StringComparison.CurrentCultureIgnoreCase))
                    {
                        json = "{ \"errorCode\": \"" + Errors.DataCashPaymentFailed + "\" }";
                        success = false;
                    }
                }

                // Extract previous payment details
                string reference = "";
                double? amount = 0;
                string browserUserAgent = "";
                string returnUrl = "";

                if (success)
                {
                    reference = (string)orderDetails.PaymentDataElement["reference"];
                    string amountString = (string)orderDetails.PaymentDataElement["amount"];
                    browserUserAgent = (string)orderDetails.PaymentDataElement["browserUserAgent"];
                    returnUrl = (string)orderDetails.PaymentDataElement["returnUrl"];
                    double amountDouble = 0;
                    if (double.TryParse(amountString, out amountDouble))
                    {
                        amount = amountDouble;
                    }
                }

                if (success)
                {
                    // Has there already been a payment for this order?
                    if (reference != null && reference.Length > 0)
                    {
                        // Check to see if the previous payment was successful or not
                        bool dontInitialisePayment = false;
                        success = DataCashPaymentProvider.ReprocessTransaction(
                            domainConfiguration, 
                            orderDetails, 
                            amount, 
                            reference,
                            ref json, 
                            ref httpStatus, 
                            out dontInitialisePayment);

                        if (dontInitialisePayment) return success;
                    }
                }

                if (success)
                {
                    orderDetails.BrowserDetails = new BrowserDetails()
                    {
                        UserAgent = HttpUtility.HtmlEncode(HttpUtility.UrlDecode(browserUserAgent))
                    };

                    // Call the DataCash web service that initialises payment
                    success = DataCashPaymentProvider.Initialise(domainConfiguration, orderDetails, returnUrl);
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
            }
            catch (Exception exception)
            {
                orderDetails.ReturnJson = "{ \"errorCode\": \"" + Errors.UnhandledException + "\" }";
                orderDetails.ReturnHttpStatus = HttpStatusCode.InternalServerError;
                success = false;
                Global.Log.Error("Unhandled exception", exception);
            }
            finally
            {
                httpStatus = orderDetails.ReturnHttpStatus;
                json = orderDetails.ReturnJson;
            }

            return success;
        }

        /// <summary>
        /// Checks if the previous payment was successful and if so sends the order to ACS
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <param name="amount"></param>
        /// <param name="json"></param>
        /// <param name="httpStatus"></param>
        /// <param name="dontInitialisePayment"></param>
        /// <returns></returns>
        private static bool ReprocessTransaction(
            DomainConfiguration domainConfiguration,
            OrderDetails orderDetails, 
            double? amount, 
            string reference,
            ref string json,
            ref HttpStatusCode httpStatus,
            out bool dontInitialisePayment)
        {
            bool success = true;
            dontInitialisePayment = false; // Always initialise another Datacash payment unless we complete the order

            if (orderDetails.Amount > (amount.HasValue ? amount: 0))
            {
                // The amount that has already been paid does not match the order amount.  Most likely the customer added or removed items from the order

                // Cancel the original payment and start over.  The customer will need to re-enter their card details
                success = DataCashPaymentProvider.RollbackPaymentInternal(orderDetails, reference);

                // Doesn't matter if the rollback was successful - customer needs to pay again
                success = true;
            }
            else
            {
                // Check if the previous payment succeeded
                success = DataCashPaymentProvider.ProcessPaymentInternal(orderDetails);

                // Was the previous payment successful?
                if (success)
                {
                    // Yes - payment has already been completed
                    // Add the payment to the order
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                    JObject paymentObject = JObject.FromObject(orderDetails.Payment);
                    orderDetails.Payments.Add(paymentObject);

                    // We need to send the order to ACS rather than setup a new payment
                    List<ACSError> errors = new List<ACSError>();
                    success = OrderServices.SendOrder(domainConfiguration, orderDetails, errors);

                    if (!success)
                    {
                        // There was a problem sending the order to ACS
                        // We may need to send an alert email
                        // If there was a card payment we'll probably need to roll it back
                        OrderServices.ProcessErrors(errors, orderDetails);
                    }

                    if (orderDetails != null)
                    {
                        json = orderDetails.ReturnJson;
                        httpStatus = orderDetails.ReturnHttpStatus;
                    }

                    // Fin... no need to initialise a new Datacash payment as the order has been completed
                    dontInitialisePayment = true;
                }
                else
                {
                    // The previous payment failed so we need to setup a new payment
                    success = true; // We want to continue so reset the result...
                }
            }

            return success;
        }

        private static string GetDataCashDate(DateTime dateTime)
        {
            // 20120731 21:59:42 YYYYMMDD HH:MM:SS

            return
                dateTime.Year.ToString("0000") +
                dateTime.Month.ToString("00") +
                dateTime.Day.ToString("00") +
                " " +
                dateTime.Hour.ToString("00") +
                ":" +
                dateTime.Minute.ToString("00") +
                ":" +
                dateTime.Second.ToString("00");
        }

        /// <summary>
        /// Call Datacash to start a brand new payment
        /// </summary>
        /// <param name="domainConfiguration"></param>
        /// <param name="orderDetails"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private static bool Initialise(DomainConfiguration domainConfiguration, OrderDetails orderDetails, string returnUrl)
        {
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;

            // Setup the xml
            string requestXml = DataCashPaymentProvider.SetupWebServiceXml.Replace("<<<merchantreference>>>", orderDetails.OrderId);
            requestXml = requestXml.Replace("<<<client>>>", orderDetails.MerchantId);
            requestXml = requestXml.Replace("<<<password>>>", orderDetails.Password);
            requestXml = requestXml.Replace("<<<amount>>>", orderDetails.Amount.ToString());
            requestXml = requestXml.Replace("<<<url>>>", returnUrl);
            requestXml = requestXml.Replace("<<<pageSetId>>>", domainConfiguration.DataCashPageSetId);

            string threeDSecureWhiteList = ConfigurationManager.AppSettings["3DSecureWhitelist"];
            threeDSecureWhiteList = String.IsNullOrEmpty(threeDSecureWhiteList) ? "" : threeDSecureWhiteList;

            if 
            (
                domainConfiguration.Is3DSecureEnabled && 
                orderDetails.BrowserDetails != null
            )
            {
                DateTime dateTime = DateTime.UtcNow;
                requestXml = requestXml.Replace("<<<threedsecure>>>", DataCashPaymentProvider.ThreeDSecure);
                requestXml = requestXml.Replace("<<<merchanturl>>>", domainConfiguration.HostHeader);
                requestXml = requestXml.Replace("<<<transactiondatetime>>>", DataCashPaymentProvider.GetDataCashDate(DateTime.UtcNow));
                requestXml = requestXml.Replace("<<<browserheaders>>>", orderDetails.BrowserDetails.Accept);
                requestXml = requestXml.Replace("<<<browseruseragent>>>", HttpUtility.UrlDecode(orderDetails.BrowserDetails.UserAgent));
            }
            else
            {
                requestXml = requestXml.Replace("<<<threedsecure>>>", "");
            }

            // Call DataCash setup web service
            string setupResponse = "";
            bool success = HttpHelper.RestCall
            (
                "PUT", 
                DataCashPaymentProvider.GetDataCashUrl(orderDetails.PaymentProviderName), 
                "Application/XML", 
                "Application/XML", 
                null, 
                requestXml, 
                true, 
                out httpStatus, 
                out setupResponse
            );

            orderDetails.ReturnHttpStatus = httpStatus;

            if (!success) return false;

            // Extract the website url from the response

            // Parse the response xml
            XElement responseXml = XElement.Parse(setupResponse);

            // Get the status
            int status = 0;
            var statusElement = responseXml.Elements("status").FirstOrDefault();
            if (statusElement == null || !int.TryParse(statusElement.Value, out status))
            {
                // "Can't get status from response xml: " + responseXml);
                Global.Log.Error("DataCash error: Can't find status element");
                return false;
            }

            if (status != 1)
            {
                Global.Log.Error("DataCash error: status=" + status);
// TODO GET THE CUSTOMER FACING ERROR MESSAGE FROM DATACASH
                //throw new Exception("Web service call failed: " + responseXml);
                return false;
            }

            // Get the website url
            var websiteUrlElement = responseXml.Elements("HpsTxn").FirstOrDefault().Elements("hps_url").FirstOrDefault();
            if (websiteUrlElement == null)
            {
                Global.Log.Error("DataCash error: Can't find HpsTxn.hps_url");
                return false;
            }
            string datacashWebsiteUrl = websiteUrlElement.Value;

            // Get the session id
            var sessionIdElement = responseXml.Elements("HpsTxn").FirstOrDefault().Elements("session_id").FirstOrDefault();
            if (sessionIdElement == null)
            {
                Global.Log.Error("DataCash error: Can't find HpsTxn.session_id element");
                return false;
            }
            string datacashSessionId = sessionIdElement.Value;

            // Get the reference
            var referenceElement = responseXml.Elements("datacash_reference").FirstOrDefault();
            if (referenceElement == null)
            {
                Global.Log.Error("DataCash error: Can't find datacash_reference element");
                return false;
            }

            orderDetails.ReturnJson = 
                "{\"sessionId\":\"" + datacashSessionId + 
                "\",\"reference\":\"" + referenceElement.Value + 
                "\",\"url\":\"" + datacashWebsiteUrl + 
                "\",\"amount\":\"" + orderDetails.Amount + "\"  }";

            return true;
        }

        public bool ProcessPayment(OrderDetails orderDetails)
        {
            return DataCashPaymentProvider.ProcessPaymentInternal(orderDetails);
        }

        /// <summary>
        /// PHASE 1 - CHECK IF PAYMENT WAS SUCCESSFUL
        /// Calls a Datacash web service that checks the status of the payment.
        /// The Datacash web service does not return detailed information about the transaction.  We'll need to call a second webservice for that.
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        private static bool ProcessPaymentInternal(OrderDetails orderDetails)
        {
            string reference = (string)orderDetails.PaymentDataElement["reference"];

            if (!orderDetails.PaymentProviderName.Equals("DataCashTest", StringComparison.CurrentCultureIgnoreCase) &&
                !orderDetails.PaymentProviderName.Equals("DataCashLive", StringComparison.CurrentCultureIgnoreCase))
            {
                Global.Log.Error("Unknown DataCash type " + orderDetails.PaymentProviderName);
                return false;
            }

            // Call query web service
            string requestXml = DataCashPaymentProvider.QueryWebServiceXml.Replace("<<<reference>>>", reference);
            requestXml = requestXml.Replace("<<<client>>>", orderDetails.MerchantId);
            requestXml = requestXml.Replace("<<<password>>>", orderDetails.Password);

            // Call DataCash query web service
            orderDetails.ReturnHttpStatus = HttpStatusCode.InternalServerError;
            string queryResponse = "";
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
            bool success = HttpHelper.RestCall(
                "POST",
                DataCashPaymentProvider.GetDataCashUrl(orderDetails.PaymentProviderName),
                "Application/XML", "Application/XML",
                null,
                requestXml,
                true,
                out httpStatus,
                out queryResponse);

            orderDetails.ReturnHttpStatus = httpStatus;

            if (!success)
            {
                Global.Log.Error(
                    "DataCash setup 1 failed httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                return false;
            }

            // Parse the response xml
            XElement responseXml = XElement.Parse(queryResponse);

            // Get the status
            int status = 0;
            var statusElement = responseXml.Elements("status").FirstOrDefault();
            if (statusElement == null || !int.TryParse(statusElement.Value, out status))
            {
                Global.Log.Error(
                    "DataCash setup 1 missing status element httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                return false;
            }

            if (status != 1)
            {
                Global.Log.Error(
                    "DataCash setup 1 returned status " + status.ToString() + " httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                return false;
            }

            // Get the payment reference
            var paymentReferenceElement = responseXml
                .Elements("HpsTxn").FirstOrDefault()
                .Elements("datacash_reference").FirstOrDefault();

            if (paymentReferenceElement == null)
            {
                //TODO WHAT DO WE DO HERE????  REVERSE THE PAYMENT???
                Global.Log.Error("DataCash setup 1 missing payment reference " + status.ToString() +
                    " httpStatus=" + httpStatus.ToString() +
                    (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                return false;
            }

            // Call the query web service a second time to get the payment details
            return DataCashPaymentProvider.GetPaymentDetails(orderDetails, paymentReferenceElement.Value);
        }

        /// <summary>
        /// PHASE 2 - CHECK IF PAYMENT WAS SUCCESSFUL
        /// Gets detailed information about the payment to be included in the order json we're going to send to ACS.
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <param name="paymentReference"></param>
        /// <returns></returns>
        private static bool GetPaymentDetails(OrderDetails orderDetails, string paymentReference)
        {
            // Do a second query to get the payment details
            string requestXml = DataCashPaymentProvider.QueryWebServiceXml.Replace("<<<reference>>>", paymentReference);
            requestXml = requestXml.Replace("<<<client>>>", orderDetails.MerchantId);
            requestXml = requestXml.Replace("<<<password>>>", orderDetails.Password);

            // Call DataCash query web service
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
            string queryResponse = "";
            bool success = HttpHelper.RestCall(
                "POST",
                DataCashPaymentProvider.GetDataCashUrl(orderDetails.PaymentProviderName), 
                "Application/XML", 
                "Application/XML", 
                null, 
                requestXml, 
                true, 
                out httpStatus, 
                out queryResponse);

            orderDetails.ReturnHttpStatus = httpStatus;

            if (!success)
            {
                Global.Log.Error("DataCash setup 2 failed" + 
                    (" httpStatus=" + httpStatus.ToString()) + 
                    (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                return false;
            }

            // Parse the response xml
            XElement responseXml = XElement.Parse(queryResponse);

            // Get the status
            int status = 0;
            var statusElement = responseXml.Elements("status").FirstOrDefault();
            if (statusElement == null || !int.TryParse(statusElement.Value, out status))
            {
                Global.Log.Error("DataCash setup 2 missing status element" +
                    (" httpStatus=" + httpStatus.ToString()) +
                    (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                return false;
            }

            if (status != 1)
            {
                Global.Log.Error("DataCash setup 2 returned status " + status.ToString() +
                    (" httpStatus=" + httpStatus.ToString()) +
                    (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                return false;
            }

            // Get the last four digits of the card number.  Datacash return the whole thing with the middle bit starred out
            string lastFourDigits = responseXml.Elements("QueryTxnResult").FirstOrDefault().Elements("Card").FirstOrDefault().Elements("pan").FirstOrDefault().Value;
            if (lastFourDigits.Length > 4) lastFourDigits = lastFourDigits.Substring(lastFourDigits.Length - 4, 4);

            JToken paymentChargeElement = orderDetails.PaymentDataElement["paymentCharge"];
            string paymentCharge = "0";
            if (paymentChargeElement != null)
            {
                paymentCharge = (string)paymentChargeElement;
            }

            // Return the payment details
            orderDetails.Payment = new Payment()
            {
                PaymentType = "Card",
                Value = ((int)(orderDetails.Amount * 100)).ToString(),
                PaytypeName = responseXml.Elements("QueryTxnResult").FirstOrDefault().Elements("Card").FirstOrDefault().Elements("scheme").FirstOrDefault().Value,
                AuthCode = responseXml.Elements("QueryTxnResult").FirstOrDefault().Elements("authcode").FirstOrDefault().Value,
                LastFourDigits = lastFourDigits,
                CVVStatus = "",
                PayProcessor = "DataCash",
                PSPSpecificDetails = new PSPSpecificDetails()
                {
                    DatacashPSP = new DataCashPSP()
                    {
                        MerchantReference = responseXml.Elements("QueryTxnResult").FirstOrDefault().Elements("merchant_reference").FirstOrDefault().Value,
                        UniqueTransactionReference = paymentReference
                    }
                },
                PaymentCharge = paymentCharge
            };

            return true;
        }

        #region IPaymentProvider

        public string BuildPaymentRollbackFailEmailBody(OrderDetails orderDetails, out Email templateEmail)
        {
            string body = "";

            if (orderDetails.DomainConfiguration.TemplateEmails.TryGetValue("DatacashPaymentAlertEmail", out templateEmail))
            {
                // Build the email body
                body = templateEmail.Body;
                body = body.Replace("{REFERENCE}", orderDetails.Payment.PSPSpecificDetails.DatacashPSP.UniqueTransactionReference);
            }

            return body;
        }

        public bool RollbackPayment(OrderDetails orderDetails, string reference)
        {
            return DataCashPaymentProvider.RollbackPaymentInternal(orderDetails, reference);
        }

        public static bool RollbackPaymentInternal(OrderDetails orderDetails, string reference)
        {
            // Build the cancel xml to send to DataCash
            string requestXml = DataCashPaymentProvider.CancelWebServiceXml
                .Replace("<<<password>>>", orderDetails.Password)
                .Replace("<<<client>>>", orderDetails.MerchantId)
                .Replace("<<<reference>>>", reference);

            // Call DataCash query web service
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
            string queryResponse = "";
            bool success = HttpHelper.RestCall(
                "POST", 
                DataCashPaymentProvider.GetDataCashUrl(orderDetails.PaymentProviderName), 
                "Application/XML", 
                "Application/XML", 
                null,
                requestXml, 
                true, 
                out httpStatus, 
                out queryResponse);

            orderDetails.ReturnHttpStatus = httpStatus;

            if (!success)
            {
                Global.Log.Error(
                    "DataCash cancel failed httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                success = false;
            }

            if (success)
            {
                // Parse the response xml
                XElement responseXml = XElement.Parse(queryResponse);

                // Get the status
                int status = 0;
                var statusElement = responseXml.Elements("status").FirstOrDefault();
                if (statusElement == null || !int.TryParse(statusElement.Value, out status))
                {
                    Global.Log.Error(
                        "DataCash cancel missing status element httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                    success = false;
                }

                if (status != 1)
                {
                    Global.Log.Error(
                        "DataCash cancel returned status " + status.ToString() + " httpStatus=" + httpStatus.ToString() + (queryResponse == null ? "" : " queryResponse=" + queryResponse));
                    success = false;
                }
            }

            if (success)
            {
                // We have successfully rolled back the payment.  There was still an error with the order though...
                orderDetails.ReturnJson = "{ \"errorCode\":\"" + Errors.PaymentRolledBack + "\", \"errorMessage\" : \"\" }";
                orderDetails.ReturnHttpStatus = HttpStatusCode.InternalServerError;
            }
            else
            {
                // There was a problem rolling back the payment
                orderDetails.ReturnJson = "{ \"errorCode\":\"" + Errors.PaymentRolledBackFailed + "\", \"errorMessage\" : \"\" }";
                orderDetails.ReturnHttpStatus = httpStatus;
            }

            return success;
        }

        #endregion
    }
}