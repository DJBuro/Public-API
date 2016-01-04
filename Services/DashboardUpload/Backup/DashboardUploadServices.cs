using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.IO;
using System.Net;
using System.Text;
using DashboardDataAccess;
using DashboardDataAccess.Domain;
using DashboardDataAccess.DataAccess;

namespace DashboardUpload
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class DashboardUploadServices
    {
        [WebGet(UriTemplate = "Test")]
        public Stream Test()
        {
            // Stream the result back
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            byte[] returnBytes = Encoding.UTF8.GetBytes("<html><body>Ping</body></html>");
            return new MemoryStream(returnBytes);
        }

        [WebGet(UriTemplate = "TestDashboardUpload")]
        public Stream TestDashboardUpload()
        {
            // Stream the result back
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            Stream s = new MemoryStream(ASCIIEncoding.Default.GetBytes("1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1"));
            

            return this.DashboardData(s, "14");
        }

        [WebInvoke(UriTemplate = "DashboardData/{siteIdParameter}", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        public Stream DashboardData
        (
            Stream input,
            string siteIdParameter
        )
        {
            string responseText = "";
            int siteId = 0;

            try
            {
                // Was a site id provided?
                if (siteIdParameter == null)
                {
                    responseText = "<Response errorMessage=\"Siteid missing\" />";
                }
                // Is the siteid a number?
                else if (!int.TryParse(siteIdParameter, out siteId))
                {
                    responseText = "<Response errorMessage=\"Siteid is not a valid number: " + siteIdParameter + " />";
                }

                // Was any csv data provided
                if (input == null)
                {
                    responseText = "<Response errorMessage=\"No csv data\" />";
                }

                // Split the csv text up
                string[] items = null;
                string requestText = "";
                if (responseText.Length == 0)
                {
                    // Receive the csv text from the caller
                    StreamReader streamReader = new StreamReader(input);

                    if (streamReader == null)
                    {
                        responseText = "<Response errorMessage=\"No csv data\" />";
                    }

                    if (responseText.Length == 0)
                    {
                        requestText = streamReader.ReadToEnd();
                        streamReader.Dispose();

                        // Split the csv text
                        items = requestText.Split(new char[] { ',' });

                        // Are there enough items?
                        if (items == null || items.Length < 23)
                        {
                            responseText = "<Response errorMessage=\"Not enough csv columns: " + requestText + "\" />";;
                        }
                    }
                }

                // Extract the data
                int column1 = 0;
                int column2 = 0;
                int column3 = 0;
                int column4 = 0;
                int column5 = 0;
                int column6 = 0;
                int column7 = 0;
                int column8 = 0;
                int column9 = 0;
                int column10 = 0;
                int column11 = 0;
                int column12 = 0;
                int column13 = 0;
                int column14 = 0;
                int column15 = 0;
                int column16 = 0;
                int column17 = 0;
                int column18 = 0;
                int column19 = 0;
                int column20 = 0;
                Site site = null;

                if (responseText.Length == 0)
                {
                    column1 = this.GetInteger(items[4], "column1", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column2 = this.GetInteger(items[5], "column2", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column3 = this.GetInteger(items[6], "column3", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column4 = this.GetInteger(items[7], "column4", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column5 = this.GetInteger(items[8], "column5", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column6 = this.GetInteger(items[9], "column6", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column7 = this.GetInteger(items[10], "column7", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column8 = this.GetInteger(items[11], "column8", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column9 = this.GetInteger(items[12], "column9", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column10 = this.GetInteger(items[13], "column10", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column11 = this.GetInteger(items[14], "column11", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column12 = this.GetInteger(items[15], "column12", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column13 = this.GetInteger(items[16], "column13", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column14 = this.GetInteger(items[17], "column14", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column15 = this.GetInteger(items[18], "column15", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column16 = this.GetInteger(items[19], "column16", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column17 = this.GetInteger(items[20], "column17", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column18 = this.GetInteger(items[21], "column18", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column19 = this.GetInteger(items[22], "column19", out responseText);
                }

                if (responseText.Length == 0)
                {
                    column20 = column3 + column7;

                    // Get the site details
                    site = SiteDAO.FindBySiteId(siteId);

                    if (site == null)
                    {
                        responseText = "<Response errorMessage=\"Unknown site: " + siteId.ToString() + "\" />";
                    }
                }

                if (responseText.Length == 0)
                {
                    // Get the timezone that the store is in
                    TimeZoneInfo storeTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(site.Region.TimeZone);

                    // Get the current time at the store (may not be the same as the server time) so we determine what
                    // the trading day is
                    DateTime currentStoreUTCDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, storeTimeZoneInfo);
                    DateTime storeTradingDay = new DateTime(currentStoreUTCDateTime.Year, currentStoreUTCDateTime.Month, currentStoreUTCDateTime.Day, 0, 0, 0, 0);

                    // Is the store still in yesterdays trading day?
                    if (currentStoreUTCDateTime.Hour < 5) // ||
                    //                    (currentStoreUTCDateTime.Hour == 6 && currentStoreUTCDateTime.Minute < 20))
                    {
                        // We are still in yesterdays trading day
                        storeTradingDay = storeTradingDay.AddDays(-1);
                    }

                    HistoricalData historicalData = HistoricalDataDAO.FindBySiteIdAndTradingDay(siteId, storeTradingDay);
                    bool isHistoricalDataNew = false;

                    // Is there already a historic data row for this store and trading day?
                    if (historicalData == null)
                    {
                        // There isn't a row for this store and trading day so we'll need to create a new one
                        historicalData = new HistoricalData();
                        historicalData.SiteId = site.SiteId;
                        historicalData.HeadOfficeId = site.HeadOfficeID;
                        historicalData.Name = site.Name;
                        historicalData.TradingDay = storeTradingDay;

                        isHistoricalDataNew = true;
                    }

                    // Store the data

                    // Site
                    site.SiteId = siteId;
                    site.LastUpdated = DateTime.Now;
                    site.Column_1 = column1;
                    site.Column_2 = column2;
                    site.Column_3 = column3;
                    site.Column_4 = column4;
                    site.Column_5 = column5;
                    site.Column_6 = column6;
                    site.Column_7 = column7;
                    site.Column_8 = column8;
                    site.Column_9 = column9;
                    site.Column_10 = column10;
                    site.Column_11 = column11;
                    site.Column_12 = column12;
                    site.Column_13 = column13;
                    site.Column_14 = column14;
                    site.Column_15 = column15;
                    site.Column_16 = column16;
                    site.Column_17 = column17;
                    site.Column_18 = column18;
                    site.Column_19 = column19;
                    site.Column_20 = column20;

                    // Historical data
                    historicalData.LastUpdated = currentStoreUTCDateTime;
                    historicalData.Column_1 = site.Column_1;
                    historicalData.Column_2 = site.Column_2;
                    historicalData.Column_3 = site.Column_3;
                    historicalData.Column_4 = site.Column_4;
                    historicalData.Column_5 = site.Column_5;
                    historicalData.Column_6 = site.Column_6;
                    historicalData.Column_7 = site.Column_7;
                    historicalData.Column_8 = site.Column_8;
                    historicalData.Column_9 = site.Column_9;
                    historicalData.Column_10 = site.Column_10;
                    historicalData.Column_11 = site.Column_11;
                    historicalData.Column_12 = site.Column_12;
                    historicalData.Column_13 = site.Column_13;
                    historicalData.Column_14 = site.Column_14;
                    historicalData.Column_15 = site.Column_15;
                    historicalData.Column_16 = site.Column_16;
                    historicalData.Column_17 = site.Column_17;
                    historicalData.Column_18 = site.Column_18;
                    historicalData.Column_19 = site.Column_19;
                    historicalData.Column_20 = site.Column_20;

                    // Update the database
                    DataAccess.UpdateDashboardData(site, historicalData, isHistoricalDataNew);
                }
            }
            catch (Exception exception)
            {
                responseText = "<Response errorMessage=\"" + exception.Message + "\" />";
            }

            // Was there an error?
            if (responseText.Length == 0)
            {
                responseText = "OK";
            }
            else
            {
                // Log the error
                Log log = new Log();
                log.Created = DateTime.Now;
                log.Message = responseText;
                log.Method = ".DashboardData";
                log.Severity = "ERROR";
                log.Source = "DashboardUploadServices";
                log.StoreId = siteId.ToString();
                LogDAO.Create(log);
            }

            // Stream the result back
            WebOperationContext.Current.OutgoingResponse.ContentType = "plain/text";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            byte[] returnBytes = Encoding.UTF8.GetBytes(responseText);
            return new MemoryStream(returnBytes);

            //WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            //WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            //byte[] returnBytes = Encoding.UTF8.GetBytes("<html><body>" + responseText  + "</body></html>");
            //return new MemoryStream(returnBytes);
        }

        private int GetInteger(string inputData, string itemName, out string responseText)
        {
            responseText = "";
            int outputData = 0;

            // Try and convert the string data to an integer
            if (!int.TryParse(inputData, out outputData))
            {
                responseText = "<Response errorMessage=\"Data is not numeric. Item:" + itemName + " data:" + inputData + "\" />";
            }

            // ABS the integer
            if (responseText.Length == 0)
            {
                outputData = Math.Abs(outputData);
            }

            return outputData;
        }
    }
}