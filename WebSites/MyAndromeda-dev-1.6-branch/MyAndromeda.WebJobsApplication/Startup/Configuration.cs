using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using MyAndromeda.Logging;
using MyAndromeda.Services.Marketing.WebJobs;

namespace MyAndromeda.WebJobs.EventMarketing.Startup
{
    internal static class Configuration 
    {
        internal static bool VerifyStorageConfiguration(IJobActivator jobActivator)
        {
            string webJobsDashboard =
                ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
            string webJobsStorage =
                ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

            bool configOK = true;

            if (string.IsNullOrWhiteSpace(webJobsDashboard) || string.IsNullOrWhiteSpace(webJobsStorage))
            {
                configOK = false;
                Console.WriteLine("Please add the Azure Storage account credentials in App.config");
            }

            if (configOK) 
            {
                CheckQueues(jobActivator);
            }

            return configOK;
        }

        internal static bool CheckQueues(IJobActivator jobActivator)
        {
            var queueManager = jobActivator.CreateInstance<IQueueManager>();
            var logger = jobActivator.CreateInstance<IMyAndromedaLogger>();
            try
            {
                var startQueue = queueManager.GetMarketingEventQueue().Result;

                var storeMarketingStartQueue = queueManager.GetMarketingEventStoreQueueAsync().Result;

                var marketingAuitQueue = queueManager.GetMarketingEventAuditHistoryQueueAsync().Result;
                
                logger.Debug("Hi");
                logger.Debug(startQueue.Name + ": " + startQueue.ApproximateMessageCount.GetValueOrDefault());
                logger.Debug(storeMarketingStartQueue.Name + ": " + storeMarketingStartQueue.ApproximateMessageCount.GetValueOrDefault());
                logger.Debug(marketingAuitQueue.Name + ": " + marketingAuitQueue.ApproximateMessageCount.GetValueOrDefault());
            }
            catch (Exception)
            {
                Console.WriteLine("Getting / Creating the queues failed.");
                throw;
            }

            return true;            
        }
    }
}
