using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using MyAndromeda.Logging;
using MyAndromeda.Services.Marketing;
using MyAndromeda.Services.Marketing.Models;
using Newtonsoft.Json;

namespace MyAndromeda.WebJobs.EventMarketing
{
    public class MarketingSaveEmailHistoryTask 
    {
        private readonly IMarketingHistoryTrailService mailHistoryTrailService;
        private readonly IMyAndromedaLogger logger; 

        public MarketingSaveEmailHistoryTask(IMarketingHistoryTrailService mailHistoryTrailService, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.mailHistoryTrailService = mailHistoryTrailService;
        }

        public async Task ProccessMaketingSent(
            [QueueTrigger("%marketing-event-audit-history-queue%")]
            string message)
        {
            var o = JsonConvert.DeserializeObject<MarketingRecipientListMessage>(message);

            try
            {
                await mailHistoryTrailService.SentEmailsAsync(o);  
            }
            catch (System.Exception ex)
            {
                this.logger.Error("Problems saving the email history");
                this.logger.Error(ex);
                throw;
            }
        }
    }
}