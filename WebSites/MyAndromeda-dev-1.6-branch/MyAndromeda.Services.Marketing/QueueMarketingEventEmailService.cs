using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using MyAndromeda.Services.Marketing.Models;
using MyAndromeda.Services.Marketing.WebJobs;
using Newtonsoft.Json;

namespace MyAndromeda.Services.Marketing
{
    public class QueueMarketingEventEmailService : IQueueMarketingEventEmailService 
    {
        private readonly IQueueManager queueManager;

        public QueueMarketingEventEmailService(IQueueManager queueManager)
        {
            this.queueManager = queueManager;
        }

        public async Task StartMarketingForAnyStore(string marketingType)
        {
            var queue = await this.queueManager.GetMarketingEventQueue();

            var cloudMessage = new CloudQueueMessage(marketingType);
            await queue.AddMessageAsync(cloudMessage);
        }

        public async Task<string> AddToMarketingQueueAsync(int andromedaSiteId, string name)
        {
            var queue = await this.queueManager.GetMarketingEventStoreQueueAsync();

            var messageObject = new MarketingStoreEventQueueMessage()
            {
                AndromedaSiteId = andromedaSiteId,
                MarketingCampaignType = name
            };

            var message = JsonConvert.SerializeObject(messageObject);
            var cloudMessage = new CloudQueueMessage(message);

            //var messagesInQueue = await queue.GetMessagesAsync(queue.ApproximateMessageCount.GetValueOrDefault());

            //var obs = messagesInQueue.Select(e=> JsonConvert.DeserializeObject<MarketingStoreEventQueueMessage>(e.AsString));
            
            //if (obs.Any(e => e.MarketingCampaignType == name && e.AndromedaSiteId == andromedaSiteId)) 
            //{
            //    return null;
            //}

            await queue.AddMessageAsync(cloudMessage);

            return queue.Name;
        }
    }
}