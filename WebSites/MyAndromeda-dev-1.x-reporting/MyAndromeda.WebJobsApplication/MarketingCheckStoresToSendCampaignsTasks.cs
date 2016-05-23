using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService;
using MyAndromeda.Services.Marketing.Models;
using Newtonsoft.Json;

namespace MyAndromeda.WebJobs.EventMarketing
{
    public class MarketingCheckStoresToSendCampaignsTasks
    {
        private readonly IMarketingTemplateDataService marketingTemplateDataService;
        private readonly IMyAndromedaLogger logger;

        public MarketingCheckStoresToSendCampaignsTasks(IMyAndromedaLogger logger, IMarketingTemplateDataService marketingTemplateDataService) 
        {
            this.marketingTemplateDataService = marketingTemplateDataService;
            this.logger = logger;
        }

        public async Task FindStoresToMarket(
            [QueueTrigger("%marketing-event-queue%")] string startType,
            [Queue("%marketing-event-store-queue%")] ICollector<string> outputQueueMessage)
        {
            string message = "Check all stores for marketing events: " + startType;

            this.logger.Debug(message);

            var allEnabled = await this.marketingTemplateDataService.MarketingEventCampaigns
                                       .Where(e => e.TemplateName == startType)
                                       .Where(e => e.EnableEmail)
                                       .Select(e => new { e.AndromedaSiteId, e.TemplateName })
                                       .ToListAsync();

            var result = string.Format("{0} - {1} stores added to the queue", startType, allEnabled.Count);

            this.logger.Debug(result);

            foreach (var item in allEnabled)
            {
                var outputMessageObject = new MarketingStoreEventQueueMessage()
                {
                    AndromedaSiteId = item.AndromedaSiteId,
                    MarketingCampaignType = item.TemplateName
                };

                var outputMessage = JsonConvert.SerializeObject(outputMessageObject);

                outputQueueMessage.Add(outputMessage);
            }
        }
    }
}