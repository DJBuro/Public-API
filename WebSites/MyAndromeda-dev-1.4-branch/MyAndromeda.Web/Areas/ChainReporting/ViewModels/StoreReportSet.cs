using System;
using System.Linq;
using MyAndromeda.Data.DailyReporting.Services;

namespace MyAndromeda.Web.Areas.ChainReporting.ViewModels
{
    public class StoreReportSet
    {
        /// <summary>
        /// Gets or sets the andromedia site id.
        /// </summary>
        /// <value>The andromedia site id.</value>
        public int AndromediaSiteId { get; set; }

        public string ClientSiteName { get; set; }

        public string ExternalSiteId { get; set; }

        public DailyMetricGroup[] Data { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public string Longitude { get; set; }

        public double AvgSpend
        {
            get
            {
                //var data = Data.Where(e=> e.Combined.AvgSale > 0);
                var total = Data.Sum(e => e.Combined.Sales);
                var count = Data.Sum(e => e.Combined.OrderCount);

                return !Data.Any() ? 0 : total / count;
                //return !data.Any() ? 0 : data.Average(e => e.Combined.AvgSale);
            }
        }

        public Int64 TotalSales
        {
            get
            {
                return Data.Sum(e => e.Combined.Sales);
            }
        }

        public double AvgSales 
        {
            get 
            {
                var data = Data.Where(e=> e.Combined.Sales > 0);
                return !data.Any() ? 0 : data.Average(e => e.Combined.Sales);
            }
        }

        public Int64 TotalCancelled
        {
            get 
            {
                return Data.Sum(e=> e.Combined.Cancelled.GetValueOrDefault());
            }
        }

        public Int64 TotalOrders
        {
            get { return Data.Sum(e => e.Combined.OrderCount); 
            }
        }
        
        public double Less30Mins 
        {
            get { return Data.Sum(e=> e.Performance.NumOrdersLT30Mins.GetValueOrDefault()); 
            } 
        }

        public double AvgOrders 
        {
            get
            { 
                return !Data.Any() ? 0 : Data.Average(e=> e.Combined.OrderCount); 
            } 
        }
        public double AvgCancelled 
        {
            get 
            {
                return !Data.Any() ? 0: Data.Average(e=> e.Combined.Cancelled.GetValueOrDefault()); 
            } 
        }
        public double SumDelivered 
        {
            get 
            {
                return Data.Sum(e=> e.Delivery.OrderCount); 
            } 
        }
        public double SumCollected 
        { 
            get 
            { 
                return Data.Sum(e=> e.Collection.OrderCount); 
            } 
        }
        public double SumInStore 
        {
            get 
            {
                return Data.Sum(e => e.InStore.OrderCount); 
            } 
        }

        public decimal AvgOutTheDoor 
        { 
            get
            {
                return !Data.Any() ? 0 : Data.Average(e => e.Performance.AvgOutTheDoor.GetValueOrDefault()); 
            }
        }
        public decimal AvgToTheDoor
        {
            get
            {
                return !Data.Any() ? 0 : Data.Average(e => e.Performance.AvgDoorTime.GetValueOrDefault());
            }
        }
        public decimal AvgRackTime
        {
            get
            {
                return !Data.Any() ? 0 : Data.Average(e => e.Performance.AvgRackTime.GetValueOrDefault());
            }
        }
        public decimal AvgMakeTime
        {
            get
            {
                return !Data.Any() ? 0 : Data.Average(e => e.Performance.AvgMakeTime.GetValueOrDefault());
            }
        }

        public int Total
        {
            get
            {
                return Data.Length;
            }
        }
    }

}