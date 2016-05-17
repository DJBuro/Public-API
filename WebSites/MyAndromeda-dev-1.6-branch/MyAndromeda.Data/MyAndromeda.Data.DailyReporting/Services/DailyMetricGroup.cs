using System;

namespace MyAndromeda.Data.DailyReporting.Services
{
    public class DailyMetricGroup
    {
        public DailyMetricGroup() 
        {
        }

        public long? AndromediaSiteId { get; set; }

        public DateTime? Date { get; set; }

        public OrderMetricGroup InStore { get; set; }

        public OrderMetricGroup Collection { get; set; }

        public OrderMetricGroup Delivery { get; set; }
        
        /// <summary>
        /// Aggregates of the three above groups
        /// </summary>
        public OrderMetricGroup Combined { get; set; }

        public PerformanceMetricGroup Performance { get; set; }
    }
}