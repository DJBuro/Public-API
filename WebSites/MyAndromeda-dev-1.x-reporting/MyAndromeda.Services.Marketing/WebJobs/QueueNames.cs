using System;
using System.Configuration;
using MyAndromeda.Services.Marketing.WebJobs;

namespace MyAndromeda.Services.Marketing.WebJobs
{
    public class QueueNames : IQueueNames
    {
        private string marketingEventQueueName;

        /// <summary>
        /// Start a marketing event - ie no orders for a week - for everyone 
        /// </summary>
        public string MarketingEventQueue
        {
            get
            {
                return marketingEventQueueName ?? 
                    (marketingEventQueueName = ConfigurationManager.AppSettings["MarketingEventQueue"]);
            }
        }

        private string storeMarketingQueueName;

        /// <summary>
        /// Start a marketing event for a store 
        /// </summary>
        public string MarketingEventStoreQueue
        {
            get
            {
                return storeMarketingQueueName ??
                    (storeMarketingQueueName = ConfigurationManager.AppSettings["MarketingEventStoreQueue"]);
            }
        }

        private string marketingEventAuditHistory;

        public string MarketingEventAuditHistoryQueue 
        {
            get 
            {
                return marketingEventAuditHistory ?? 
                    (marketingEventAuditHistory = ConfigurationManager.AppSettings["MarketingEventAuditHistoryQueue"]);
            }   
        }

        public string GetName(string name)
        {
            switch (name) 
            {
                case "marketing-event-queue":
                    return this.MarketingEventQueue;

                case "marketing-event-store-queue":
                    return this.MarketingEventStoreQueue;

                case "marketing-event-audit-history-queue":
                    return this.MarketingEventAuditHistoryQueue;
                
            }

            return string.Empty;
        }
    }
}