using System;
using System.Collections.Generic;

namespace MyAndromeda.Services.Marketing
{
    public static class MarketingEventTypes
    {
        public const string NoOrders = "No Orders";
        public const string OneWeekNoOrders = "One Week, no orders";
        public const string OneMonthNoOrders = "One Month, no orders";
        public const string ThreeMonthNoOrders = "Three Months, no orders";

        public const string TestEventCampaign = "Test Event Campaign";

        public static IEnumerable<MarketingEventDefinition> GetScheduledMarketingDefinitions() 
        {
            yield return NoOrdersDefinition;    
            yield return OneWeekNoOrdersDefinition;
            yield return OneMonthNoOrdersDefinition;
            yield return ThreeMonthsNoOrdersDefinition;
        }

        public static MarketingEventDefinition NoOrdersDefinition = new MarketingEventDefinition()
        {
            Name = NoOrders,
            SendAtTime = new TimeSpan(11, 00, 00)
        };
        public static MarketingEventDefinition OneWeekNoOrdersDefinition = new MarketingEventDefinition()
        {
            Name = OneWeekNoOrders,
            SendAtTime = new TimeSpan(11, 00, 00)
        };
        public static MarketingEventDefinition OneMonthNoOrdersDefinition = new MarketingEventDefinition()
        {
            Name = OneMonthNoOrders,
            SendAtTime = new TimeSpan(11, 00, 00)
        };
        public static MarketingEventDefinition ThreeMonthsNoOrdersDefinition = new MarketingEventDefinition()
        {
            Name = ThreeMonthNoOrders,
            SendAtTime = new TimeSpan(11, 00, 00)
        };
    }
}