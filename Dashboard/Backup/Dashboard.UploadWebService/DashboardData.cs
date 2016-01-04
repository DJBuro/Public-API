using System;
using WebDashboard.Dao;
using Dashboard.UploadWebService.Dao;
using WebDashboard.Dao.Domain;

namespace Dashboard.UploadWebService
{
    public class DashboardData : IDashboardData
    {
        ISiteDao SiteDao { get; set; }
        IRegionDao RegionDao { get; set; }
        IHistoricalDataDao HistoricalDataDao { get; set; }

        #region IDashboardData Members

        public void UploadData(
            int siteId, 
            int column1, 
            int column2, 
            int column3, 
            int column4, 
            int column5, 
            int column6, 
            int column7, 
            int column8, 
            int column9, 
            int column10, 
            int column11, 
            int column12, 
            int column13, 
            int column14, 
            int column15, 
            int column16, 
            int column17, 
            int column18, 
            int column19, 
            int column20)
        {
            try
            {
                // Get the site details
                Site site = SiteDao.FindByRamesesId(siteId);

                if (site == null)
                {
                    //                Logging.Logging.LogError("UploadData", siteId,"invalid siteId");   
                    return;
                }

                // Update the site data for todays trading date
                site.SiteId = siteId;
                site.LastUpdated = DateTime.Now;
                site.Column1 = column1;
                site.Column2 = column2;
                site.Column3 = column3;
                site.Column4 = column4;
                site.Column5 = column5;
                site.Column6 = column6;
                site.Column7 = column7;
                site.Column8 = column8;
                site.Column9 = column9;
                site.Column10 = column10;
                site.Column11 = column11;
                site.Column12 = column12;
                site.Column13 = column13;
                site.Column14 = column14;
                site.Column15 = column15;
                site.Column16 = column16;
                site.Column17 = column17;
                site.Column18 = column18;
                site.Column19 = column19;
                site.Column20 = column20;

                SiteDao.Update(site);

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

                HistoricalData historicalData = HistoricalDataDao.FindBySiteIdAndTradingDay(siteId, storeTradingDay);
                bool create = false;

                // Is there already a historic data row for this store and trading day?
                if (historicalData == null)
                {
                    // There isn't a row for this store and trading day so we'll need to create a new one
                    historicalData = new HistoricalData();
                    historicalData.SiteId = site.SiteId;
                    historicalData.HeadOffice = site.HeadOffice;
                    historicalData.Name = site.Name;
                    historicalData.TradingDay = storeTradingDay;

                    create = true;
                }

                // Store the data
                historicalData.LastUpdated = currentStoreUTCDateTime;
                historicalData.Column1 = site.Column1;
                historicalData.Column2 = site.Column2;
                historicalData.Column3 = site.Column3;
                historicalData.Column4 = site.Column4;
                historicalData.Column5 = site.Column5;
                historicalData.Column6 = site.Column6;
                historicalData.Column7 = site.Column7;
                historicalData.Column8 = site.Column8;
                historicalData.Column9 = site.Column9;
                historicalData.Column10 = site.Column10;
                historicalData.Column11 = site.Column11;
                historicalData.Column12 = site.Column12;
                historicalData.Column13 = site.Column13;
                historicalData.Column14 = site.Column14;
                historicalData.Column15 = site.Column15;
                historicalData.Column16 = site.Column16;
                historicalData.Column17 = site.Column17;
                historicalData.Column18 = site.Column18;
                historicalData.Column19 = site.Column19;
                historicalData.Column20 = site.Column20;

                if (create)
                {
                    // Create a new row in the history table for the store and trading day
                    HistoricalDataDao.Create(historicalData);
                }
                else
                {
                    // Update the existing row in the history table
                    HistoricalDataDao.Update(historicalData);
                }

                //         Logging.Logging.LogInfo("UploadData", data);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        #endregion
    }
}
