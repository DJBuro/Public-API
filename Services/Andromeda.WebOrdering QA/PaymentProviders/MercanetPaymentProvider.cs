using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Andromeda.WebOrdering.Model;
using Andromeda.WebOrdering.Services;
using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Andromeda.WebOrdering.PaymentProviders
{
    public class MercanetPaymentProvider : IPaymentProvider
    {
        public static bool PutMercanetPayment(string key, DomainConfiguration domainConfiguration, string siteId, string webOrderJson, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";
            var html = "";

            try
            {
                // Gather all the order details together into an object we can pass around
                OrderDetails orderDetails = null;
                success = OrderServices.GetOrderDetails(key, domainConfiguration, siteId, webOrderJson, out orderDetails);

                if (success)
                {
                    if (!orderDetails.PaymentProviderName.Equals("MercanetTest", StringComparison.CurrentCultureIgnoreCase) &&
                        !orderDetails.PaymentProviderName.Equals("MercanetLive", StringComparison.CurrentCultureIgnoreCase))
                    {
                        success = false;
                        Global.Log.Debug("MerchanetCallback MerchantResponse invalid payment provider: " + orderDetails.PaymentProviderName);
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

                    // Replace the order json with our modified version
                    webOrderJson = orderDetails.OrderElement.ToString(Newtonsoft.Json.Formatting.None);
                }

                if (success)
                {
                    // Run the exe that initialises payment
                    html = MercanetPaymentProvider.Initialise(price, orderDetails.MerchantId, orderReference, domainConfiguration);
                    Global.Log.Debug("DEBUG Initialise completed");
                    Global.Log.Debug("DEBUG Initialise completed HTML=" + (html == null ? "NULL" : html));
                    if (html.Length > 0)
                    {
                        // Mercanet exe ran successfully

                        // Drop the order to a file
                        string ordersFolder = ConfigurationManager.AppSettings["MercanetPendingOrdersFolder"];
                        PaymentProviderHelper.SaveOrderToFile(webOrderJson, orderReference, domainConfiguration, ordersFolder);

                        PaymentResponse paymentResponse = new Model.PaymentResponse()
                        {
                            Html = html,
                            OrderReference = orderReference
                        };

                        json = JsonConvert.SerializeObject(paymentResponse);

                        // All done
                        httpStatus = HttpStatusCode.OK;
                        success = true;
                    }
                }
            }
            catch (Exception exception)
            {
                json = "";
                success = false;
                Global.Log.Error("Unhandled exception", exception);
            }

            return success;
        }

        private static string Initialise(decimal amount, string merchantId, string orderReference, DomainConfiguration domainConfiguration)
        {
            // Get the location of the mercanet exe
            string mercanetRequestExe = ConfigurationManager.AppSettings["MercanetRequestExe"];
            string workingDirectory = Path.GetDirectoryName(mercanetRequestExe);

            if (mercanetRequestExe == null || mercanetRequestExe.Length == 0)
            {
                Global.Log.Error("Setting MercanetRequestExe is missing");
                return null;
            }

            string mercanetPathFile = ConfigurationManager.AppSettings["MercanetPathFile"];
            if (mercanetPathFile == null || mercanetPathFile.Length == 0)
            {
                Global.Log.Error("Setting MercanetPathFile is missing");
                return null;
            }

            // Build the arguments to pass to the Mercanet exe
            string arguments =
                "amount=" + amount.ToString() +
                " merchant_id=" + merchantId +
                " pathfile=" + mercanetPathFile +
                " order_id=" + orderReference;

            Global.Log.Debug("DEBUG Executing Mercanet request with arguments: " + arguments);

            // Run the Mercanet exe
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = mercanetRequestExe;
            startInfo.Arguments = arguments;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = workingDirectory;

            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                output.AppendLine(e.Data);
                            }
                        };
                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                error.AppendLine(e.Data);
                            }
                        };

                        process.Start();

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        if (process.WaitForExit(5000) &&
                            outputWaitHandle.WaitOne(5000) &&
                            errorWaitHandle.WaitOne(5000))
                        {
                            // Process completed. Check process.ExitCode here.
                            if (process.ExitCode == 0)
                            {
                                string html = output.ToString().Trim();
                                html = html.Substring(4); // Remove the first four characters: !0!
                                html = html.Substring(0, html.Length - 1); // Remove the last character: !

                                return html;
                            }
                            else
                            {
                                Global.Log.Error("Executing Mercanet request exe failed with return code " + process.ExitCode.ToString());
                                return null;
                            }
                        }
                        else
                        {
                            // Timed out.
                            Global.Log.Error("Executing Mercanet request exe timed out");
                            return "";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                // Log error.
                Global.Log.Error("Executing Mercanet request exe threw an exception", exception);
            }

            return "";
        }

        public static bool MerchanetCallback(string mercanetResponseString)
        {
            bool success = true;
            MercanetResponse mercanetResponse = null;

            try
            {
                string paymentProvider = "";
                string merchantId = "";
                HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;

                // Use the Mercanet console app to decrypt the response from Mercanet
                mercanetResponse = MercanetPaymentProvider.DecryptMerchantResponse(mercanetResponseString, merchantId);

                if (mercanetResponse != null)
                {
                    // Load the order from disk
                    string pendingOrdersFolder = ConfigurationManager.AppSettings["MercanetPendingOrdersFolder"];
                    JObject orderDetailsJson = PaymentProviderHelper.LoadOrderFromFile(pendingOrdersFolder, mercanetResponse.OrderId);

                    // Get the site id
                    string siteId = (string)orderDetailsJson["toSiteId"];
                    string hostHeader = (string)orderDetailsJson["hostHeader"];

                    Global.Log.Debug("MerchanetCallback siteId:" + siteId + " hostHeader:" + hostHeader);

                    int errorCode = 0;

                    DomainConfiguration domainConfiguration = Helper.GetDomainConfigurationForHost(hostHeader);

                    if (domainConfiguration == null)
                    {
                        success = false;
                        errorCode = -1; // GetDomainConfigurationForHost failed
                    }

                    // Get the Payment provider details
                    OrderDetails orderDetails = new OrderDetails();
                    if (success)
                    {
                        success = Helper.GetPaymentProviderDetails("", domainConfiguration, siteId, orderDetails);
                    }

                    if (success)
                    {
                        if (!orderDetails.PaymentProviderName.Equals("MercanetTest", StringComparison.CurrentCultureIgnoreCase) &&
                            !orderDetails.PaymentProviderName.Equals("MercanetLive", StringComparison.CurrentCultureIgnoreCase))
                        {
                            success = false;
                            Global.Log.Debug("MerchanetCallback MerchantResponse invalid payment provider: " + paymentProvider);
                        }
                    }

                    if (success)
                    {
                        if (mercanetResponse.Code != "0" || mercanetResponse.ResponseCode != "00")
                        {
                            success = false;
                            errorCode = -1;
                            Global.Log.Debug("MerchanetCallback MerchantResponse payment failed with code " + mercanetResponse.Code + " responseCode " + mercanetResponse.ResponseCode);
                        }
                    }

                    // Was payment successfull?
                    if (success)
                    {
                        // Payment has been taken

                        Global.Log.Debug("MerchanetCallback MerchantResponse payment successful");

                        // Send the order to the store
                        string json = "";

                        if (OrderServices.PutOrder("", domainConfiguration, siteId, "{ order: " + orderDetailsJson.ToString() + ", paymentType: '" + orderDetails.PaymentProviderName + "' }", out httpStatus, out json))
                        {
                            // The order has been placed - move the file
                            Global.Log.Debug("MerchanetCallback MerchantResponse send order to store succeeded: httpStatus:" + httpStatus + " json:" + json);

                            PaymentProviderHelper.PendingOrderCompleted
                            (
                                pendingOrdersFolder,
                                ConfigurationManager.AppSettings["MercanetCompletedOrdersFolder"],
                                mercanetResponse.OrderId
                            );
                        }
                        else
                        {
                            if (json != null && json.Length > 0)
                            {
                                // Parse the result json
                                JObject result = JObject.Parse(json);

                                // Get the status
                                string errorCodeText = (string)result["errorCode"];

                                // Forward the status returned by ACS to the caller
                                Int32.TryParse(errorCodeText, out errorCode);
                            }
                            else
                            {
                                errorCode = -1;
                            }

                            Global.Log.Debug("MerchanetCallback MerchantResponse send order to store failed");
                        }

                        // Was there an problem?
                        if (errorCode != 0)
                        {
                            // Send an elert email
                            Global.Log.Error("MerchanetCallback Order.PutOrder failed with errorCode " + errorCode);
                            MercanetPaymentProvider.SendAlertEmail("MercanetPaymentAlertEmail", domainConfiguration, mercanetResponse, orderDetailsJson.ToString(), mercanetResponse.OrderId);
                        }
                    }

                    if (success)
                    {
                        // Try and tell the client that the order is completed (successfully or otherwise)
                        string hubResult = Global.HubProxy.Invoke<string>("OrderCompleted", mercanetResponse.OrderId, errorCode).Result;

                        if (hubResult != "OK")
                        {
                            // The call failed on the hub side
                            Global.Log.Error("MerchanetCallback Hub call to OrderCompleted failed with '" + hubResult + "' for '" + mercanetResponseString + "'");
                            success = false;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                success = false;
                Global.Log.Error("MerchanetCallback Unhandled exception", exception);

                if (mercanetResponse != null)
                {
                    try
                    {
                        // Try and tell the client that the order is completed (successfully or otherwise)
                        string hubResult = Global.HubProxy.Invoke<string>("OrderCompleted", mercanetResponse.OrderId, -1).Result;
                    }
                    catch { }
                }
            }

            return success;
        }

        internal static void SendAlertEmail(string templateName, DomainConfiguration domainConfiguration, MercanetResponse mercanetResponse, string orderJson, string orderId)
        {
            Global.Log.Info("SendAlertEmail SendAlertEmail");

            Email emailTemplate = null;
            if (domainConfiguration.TemplateEmails.TryGetValue(templateName, out emailTemplate))
            {
                string body = emailTemplate.Body;
                body = body.Replace("{ORDERID}", mercanetResponse.OrderId);
                body = body.Replace("{AMOUNT}", (double.Parse(mercanetResponse.Amount) / 100).ToString("0.00"));
                body = body.Replace("{PAYMENTDATE}", mercanetResponse.PaymentDate);
                body = body.Replace("{PAYMENTTIME}", mercanetResponse.PaymentTime);

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

        private static MercanetResponse DecryptMerchantResponse(string merchanetResponse, string merchantId)
        {
            // Get the location of the mercanet exe
            string mercanetResponseExe = ConfigurationManager.AppSettings["MercanetResponseExe"];
            string workingDirectory = Path.GetDirectoryName(mercanetResponseExe);

            if (mercanetResponseExe == null || mercanetResponseExe.Length == 0)
            {
                Global.Log.Error("DecryptMerchantResponse Setting MercanetResponseExe is missing");
                return null;
            }

            string mercanetPathFile = ConfigurationManager.AppSettings["MercanetPathFile"];
            if (mercanetPathFile == null || mercanetPathFile.Length == 0)
            {
                Global.Log.Error("DecryptMerchantResponse Setting MercanetPathFile is missing");
                return null;
            }

            // Run the Mercanet exe
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = mercanetResponseExe;
            startInfo.Arguments = merchanetResponse.Replace("DATA=", "message=") + " pathfile=" + mercanetPathFile;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = workingDirectory;

            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                output.AppendLine(e.Data);
                            }
                        };
                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                error.AppendLine(e.Data);
                            }
                        };

                        process.Start();

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        if (process.WaitForExit(5000) &&
                            outputWaitHandle.WaitOne(5000) &&
                            errorWaitHandle.WaitOne(5000))
                        {
  //                          Global.Log.Debug("DecryptMerchantResponse exitcode=" + process.ExitCode + " response=" + output);

                            // Process completed. Check process.ExitCode here.
                            if (process.ExitCode == 0)
                            {
                                string list = output.ToString().Trim();

 //                               Global.Log.Debug("DecryptMerchantResponse decrypted: " + list);

                                string[] responseItems = list.Split('!');

                                /*
                                !code
                                !error
                                !merchant_id
                                !merchant_country
                                !amount
                                !transaction_id
                                !payment_means
                                !transmission_date
                                !payment_time
                                !payment_date
                                !response_code
                                !payment_certificate
                                !authorisation_id
                                !currency_code
                                !card_number
                                !cvv_flag
                                !cvv_response_code
                                !bank_response_code
                                !complementary_code
                                !complementary_info
                                !return_context
                                !caddie
                                !receipt_complement
                                !merchant_language
                                !language
                                !customer_id
                                !order_id
                                !customer_email
                                !customer_ip_address
                                !capture_day
                                !capture_mode
                                !data!
                                */

                                DateTime now = DateTime.Now;
                                MercanetResponse mercanetResponse = new MercanetResponse()
                                {
                                    Code = responseItems[1],
                                    MerchantId = responseItems[3],
                                    OrderId = responseItems[27],
                                    ResponseCode = responseItems[11],
                                    Amount = responseItems[5],
                                    PaymentDate = now.Year + "/" + now.Month.ToString("00") + "/" + now.Day.ToString("00"),
                                    PaymentTime = now.Hour.ToString("00") + ":" + now.Minute.ToString("00") + ":" + now.Second.ToString("00")
                                };

                                string debug = "\r\n";
                                for (int index = 0; index < responseItems.Length; index++)
                                {
                                    debug += index + "=" + responseItems[index] + "\r\n";
                                }

 //                               Global.Log.Debug("DecryptMerchantResponse extracted" + debug);
                                Global.Log.Debug("DecryptMerchantResponse extracted " + list);
                                Global.Log.Debug("DecryptMerchantResponse extracted code=" + mercanetResponse.Code + " responseCode=" + mercanetResponse.Code + " merchantId=" + mercanetResponse.MerchantId + " orderId=" + mercanetResponse.OrderId);

                                return mercanetResponse;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            // Timed out.
                            return null;
                        }
                    }
                }
            }
            catch
            {
                // Log error.
            }

            return null;
        }

        #region IPaymentProvider

        public string BuildPaymentRollbackFailEmailBody(OrderDetails orderDetails, out Email templateEmail)
        {
            templateEmail = null;
            return "";
        }
        public bool ProcessPayment(Model.OrderDetails orderDetails)
        {
            orderDetails.Payment = new Payment()
            {
                PaymentType = "Card",
                Value = ((decimal)orderDetails.OrderElement["pricing"]["priceAfterDiscount"]).ToString(),
                PaytypeName = null,
                AuthCode = null,
                LastFourDigits = null,
                CVVStatus = "",
                PayProcessor = "Mercanent",
                PSPSpecificDetails = null
            };

            return true;
        }
        public bool RollbackPayment(Model.OrderDetails orderDetails, string reference)
        {
            return false;
        }

        #endregion IPaymentProvider
    }
}