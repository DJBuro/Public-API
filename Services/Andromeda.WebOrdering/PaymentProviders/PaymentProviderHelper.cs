using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.PaymentProviders
{
    public class PaymentProviderHelper
    {
        public static void SaveOrderToFile(string order, string orderReference, DomainConfiguration domainConfiguration, string pendingOrdersFolder)
        {
            Logger.Log.Debug("DEBUG saving order to file order=" + order + " ref=" + orderReference + " appId=" + domainConfiguration.ApplicationId);

            DateTime now = DateTime.Now;

            string filename = now.ToString("yyyyMMdd_") + orderReference;

            string fullFilename = Path.Combine(pendingOrdersFolder, filename);

            // Write out the file
            using (StreamWriter streamWriter = new StreamWriter(fullFilename))
            {
                streamWriter.Write(order);
            }
        }

        public static JObject LoadOrderFromFile(string pendingOrdersFolder, string orderNumber)
        {
            // Load the order from disk
            DateTime now = DateTime.Now;          

            string pendingFilename = now.ToString("yyyyMMdd_") + orderNumber;

            string fullPendingFilename = Path.Combine(pendingOrdersFolder, pendingFilename);

            // Read in the order file
            string orderJson = "";
            using (StreamReader streamReader = new StreamReader(fullPendingFilename))
            {
                orderJson = streamReader.ReadToEnd();
            }

            Logger.Log.Debug("PayPalCallback Loaded pending order JSON=" + orderJson);

            // Parse the order json
            return JObject.Parse(orderJson);
        }

        public static void PendingOrderCompleted(string pendingOrdersFolder, string completedOrdersFolder, string orderId)
        {
            DateTime now = DateTime.Now;

            string filename = now.ToString("yyyyMMdd_") + orderId;

            string fullPendingFilename = Path.Combine(pendingOrdersFolder, filename);
            string fullCompletedFilename = Path.Combine(completedOrdersFolder, filename);

            Logger.Log.Debug("Move pending order file from \"" + fullPendingFilename + "\" to \"" + fullCompletedFilename + "\"");

            // Move the file from the pending to the completed folder
            File.Move(fullPendingFilename, fullCompletedFilename);
        }
    }
}