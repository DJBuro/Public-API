namespace MyAndromeda.Data.DailyReporting.Services
{
    public class PerformanceMetricGroup 
    {
        public decimal? AvgMakeTime { get; set; }

        public decimal? AvgRackTime { get; set; }

        public decimal? AvgDoorTime { get; set; }

        public decimal? AvgOutTheDoor { get; set; }

        public long? NumOrdersLT30Mins { get; set; }

        public long? NumOrdersLT45Mins { get; set; }

        public long? Over15lessThan20 { get; set; }
    }
}