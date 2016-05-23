using System.Configuration;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using MyAndromeda.Services.Marketing.WebJobs;

namespace MyAndromeda.Services.Marketing.WebJobs
{
    public class QueueManager : IQueueManager
    {
        private readonly IQueueNames queueNames; 
        
        public QueueManager(IQueueNames queueNames) 
        {
            this.queueNames = queueNames;
        }

        private CloudStorageAccount GetStorageSettings()
        {
            var settings = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
            return settings;
        }
        
        

        public async Task<CloudQueue> GetMarketingEventQueue()
        {
            CloudStorageAccount storageAccount = this.GetStorageSettings();
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(this.queueNames.MarketingEventQueue);
            await queue.CreateIfNotExistsAsync();

            return queue;
        }

        public async Task<CloudQueue> GetMarketingEventStoreQueueAsync()
        {
            CloudStorageAccount storageAccount = this.GetStorageSettings();
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(this.queueNames.MarketingEventStoreQueue);
            await queue.CreateIfNotExistsAsync();

            return queue;
        }


        public async Task<CloudQueue> GetMarketingEventAuditHistoryQueueAsync()
        {
            CloudStorageAccount storageAccount = this.GetStorageSettings();
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(this.queueNames.MarketingEventAuditHistoryQueue);
            await queue.CreateIfNotExistsAsync();

            return queue;
        }
    }
}