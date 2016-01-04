using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using MyAndromeda.Logging;
using MyAndromeda.Services.Marketing;

namespace MyAndromeda.WebJobs.EventMarketing
{
    public class MarketingCheckingEventTasks
    {
        private readonly IMyAndromedaLogger logger;

        public MarketingCheckingEventTasks(IMyAndromedaLogger logger) 
        {
            this.logger = logger;
        }

        [NoAutomaticTriggerAttribute]
        public async Task AlwaysRunning(
            //output back into this queue
            [Queue("%marketing-event-queue%")]
            ICollector<string> outputQueueMessage) 
        {
            this.logger.Debug("Starting the endless check for marketing emails.");
            
            while (true)
            {
                try
                {
                    var utcNow = DateTime.UtcNow;

                    MarketingEventTypes.GetScheduledMarketingDefinitions().ToList().ForEach((marketingType) =>
                    {
                        bool correctHour = utcNow.Hour == marketingType.SendAtTime.Hours;
                        bool correctMin = utcNow.Minute == marketingType.SendAtTime.Minutes;

                        if (correctHour && correctMin)
                        {
                            this.logger.Debug("Adding the marking type to start: " + marketingType.Name);

                            outputQueueMessage.Add(marketingType.Name);
                        }
                    });
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex.Message);
                }

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}