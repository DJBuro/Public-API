using System;

namespace MyAndromeda.Data.DailyReporting.Services
{
    public class OrderMetricGroup 
    {
        public Int64 Sales { get; set; }

        public decimal AvgSale
        {
            get
            {
                if (this.Sales == 0 || this.OrderCount == 0)
                {
                    return 0;
                }
                return (decimal)Sales / (decimal)OrderCount;
            }
        }

        public long OrderCount { get; set; }

        public long? Cancelled { get; set; }
    }
}