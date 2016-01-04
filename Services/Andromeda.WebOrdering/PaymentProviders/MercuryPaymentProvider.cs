using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Andromeda.WebOrdering.Model;
using Andromeda.WebOrdering.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Andromeda.WebOrdering.PaymentProviders
{
    public class MercuryPaymentProvider : IPaymentProvider
    {
        public static bool PutMercuryPayment(string key, DomainConfiguration domainConfiguration, string siteId, string paymentJson, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";

            try
            {
                // Gather all the order details together into an object we can pass around
                OrderDetails orderDetails = null;
                success = OrderServices.GetOrderDetails(key, domainConfiguration, siteId, paymentJson, out orderDetails);

                if (success)
                {
                    if (!orderDetails.PaymentProviderName.Equals("MercuryTest", StringComparison.CurrentCultureIgnoreCase) &&
                        !orderDetails.PaymentProviderName.Equals("MercuryLive", StringComparison.CurrentCultureIgnoreCase))
                    {
                        json = "{ \"errorCode\": \"" + Errors.MercuryPaymentFailed + "\" }";
                        success = false;
                    }
                }

                double amount = 0;
                Model.PaymentDetails payment = null;
                if (success)
                {
                    // Get the payment details that need to be sent to mercury
                    payment = JsonConvert.DeserializeObject<Model.PaymentDetails>(paymentJson);

                    // Get the payment amount
                    success = double.TryParse(payment.Amount, out amount);
                }
                
                string paymentId = "";
                if (success)
                {
                    // Amount is in pence
                    amount = amount / 100;

                    // Call the Mercuray web service that initialises payment
                    paymentId = MercuryPaymentProvider.Initialise(amount, orderDetails.MerchantId, orderDetails.Password, payment.ReturnUrl);

                    // Was there a problem?
                    if (paymentId == null || paymentId.Length == 0)
                    {
                        json = "{ \"errorCode\": \"" + Errors.InitialiseMercuryPaymentFailed + "\" }";
                        success = false;
                    }
                }

                if (success)
                {
                    // The payment id returned by the web service is needed to display the Mercury payment web page
                    json = "{ \"paymentId\": \"" + paymentId + "\" }";
                }
            }
            catch (Exception exception)
            {
                json = "{ \"errorCode\": \"" + Errors.UnhandledException + "\" }";
                success = false;
                Logger.Log.Error("Unhandled exception", exception);
            }

            return success;
        }

        private static string Initialise(double amount, string merchantId, string password, string returnUrl)
        {
            // Set the URL variables.
            
            string orderCompleteUrl = returnUrl; // "http://androtechnology.co.uk"; // "https://localhost:4307/ECommerceSite_1.0/ShoppingCart_SampleCode.aspx";
  //          string logoURL = ConfigurationManager.AppSettings["LogoURL"];

            // Create the InitializePayment request
            MercuryHC.InitPaymentRequest hcRequest = new MercuryHC.InitPaymentRequest();

            //Populated the request fields.
            hcRequest.MerchantID = merchantId;
            hcRequest.Password = password;
            hcRequest.TranType = "PreAuth";
            hcRequest.TotalAmount = amount;
            hcRequest.Frequency = "OneTime";
            Random rand = new Random();
            hcRequest.Invoice = Helper.GenerateOrderNumber();
            hcRequest.Memo = "Andromeda Web Ordering";
            hcRequest.TaxAmount = 0;
            hcRequest.ReturnUrl = returnUrl;
            hcRequest.ProcessCompleteUrl = orderCompleteUrl;


            //hcRequest.CardHolderName = "John Jones";
            //hcRequest.AVSAddress = "4 Corporate Square";
            //hcRequest.AVSZip = "30329";
            //hcRequest.BackgroundColor = "Gray";
            //hcRequest.FontColor = "Black";
            //hcRequest.FontSize = "Medium";
            //hcRequest.FontFamily = "FontFamily1";
            //hcRequest.PageTitle = "Demo Ecommerce Merchant";
            //hcRequest.LogoUrl = "";
            //hcRequest.DisplayStyle = "Custom";
            //hcRequest.SecurityLogo = "on";
            //hcRequest.OrderTotal = "on";
            //hcRequest.SubmitButtonText = "Submit Order";
            //hcRequest.CancelButtonText = "Cancel Order";

            // Call the InitializePayment Web Service Method.
            MercuryHC.HCServiceSoapClient hcWS = new MercuryHC.HCServiceSoapClient();
            MercuryHC.InitPaymentResponse response = hcWS.InitializePayment(hcRequest);

            string paymentId = "";

            // Check the responseCode
            if (response.ResponseCode == 0)
            {
                // get the payment ID
                paymentId = response.PaymentID;
            }

            return paymentId;
        }

        #region IPaymentProvider

        public string BuildPaymentRollbackFailEmailBody(OrderDetails orderDetails, out Email templateEmail)
        {
            templateEmail = null;
            return "";
        }
        public bool ProcessPayment(OrderDetails orderDetails)
        {
            bool success = true;

            if (!orderDetails.PaymentProviderName.Equals("Mercury", StringComparison.CurrentCultureIgnoreCase))
            {
                success = false;
            }

            string reference = (string)orderDetails.PaymentDataElement["reference"];
            string amount = (string)orderDetails.PaymentDataElement["amount"];
            string pid = (string)orderDetails.OrderRootElement["paymentData"];

            string errorMessage = "";
            MercuryHC.PaymentInfoResponse response = null;
            if (success)
            {
                // Create the request 
                MercuryHC.PaymentInfoRequest verifyPaymentRequest = new MercuryHC.PaymentInfoRequest();

                verifyPaymentRequest.MerchantID = orderDetails.MerchantId;
                verifyPaymentRequest.Password = orderDetails.Password;
                verifyPaymentRequest.PaymentID = pid;

                // Call the VerifyPayment web method
                MercuryHC.HCServiceSoapClient hcService = new MercuryHC.HCServiceSoapClient();
                response = hcService.VerifyPayment(verifyPaymentRequest);

                // Was the payment approved?
                if (response.ResponseCode != 0 || (response.ResponseCode == 0 && response.Status != "Approved"))
                {
                    success = false;
                    errorMessage = response.DisplayMessage;
                }
            }

            if (success)
            {
                // Return the payment details
                orderDetails.Payment = new Payment()
                {
                    PaymentType = "Card",
                    Value = (response.Amount * 100).ToString(),
                    PaytypeName = response.CardType,
                    AuthCode = response.AuthCode,
                    LastFourDigits = response.MaskedAccount,
                    CVVStatus = response.CvvResult,
                    PayProcessor = "Mercury",
                    PSPSpecificDetails = new PSPSpecificDetails()
                    {
                        MercuryPSP = new MercuryPSP()
                        {
                            Token = response.Token,
                            Refno = response.RefNo,
                            ProcessData = response.ProcessData,
                            AcqRefData = response.AcqRefData,
                            Invoice = response.Invoice,
                        }
                    }
                };
            
                // Complete the payment
                success = MercuryPaymentProvider.CompleteMercuryPayment(pid);
            }

            if (!success)
            {
                // There was a problem
                orderDetails.ReturnJson = 
                    "{ \"errorCode\":\"" + Errors.MercuryPaymentFailed + "\"" + 
                    (errorMessage.Length > 0 ? ", \"errorMessage\" : \"" + errorMessage + "\"" : "") + " }";

                orderDetails.ReturnHttpStatus = HttpStatusCode.InternalServerError;
            }

            return success;
        }
        public bool RollbackPayment(OrderDetails orderDetails, string reference)
        {
            return false;
        }

        #endregion

        private static bool CompleteMercuryPayment(string pid)
        {
            MercuryHC.PaymentInfoRequest hcAckRequest = new MercuryHC.PaymentInfoRequest();

            hcAckRequest.MerchantID = "494691720";
            hcAckRequest.Password = "KRD%8rw#+p9C13,T";
            hcAckRequest.PaymentID = pid;

            MercuryHC.HCServiceSoapClient hcWS = new MercuryHC.HCServiceSoapClient(); 
            int ackResponse = hcWS.AcknowledgePayment(hcAckRequest);

            if (ackResponse != 0)
            {
                return false;
            }

            return true;
        }
    }
}