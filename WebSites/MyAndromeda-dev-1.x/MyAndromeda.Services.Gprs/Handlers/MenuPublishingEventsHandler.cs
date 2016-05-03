using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Services.Export;
using MyAndromeda.Logging;
using MyAndromeda.Core.Sync;

namespace MyAndromeda.Services.Gprs.Handlers
{
    public class MenuPublishingEventsHandler : IMenuPublishEvents
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IGprsService gprsService;
        private readonly IAcsMenuXmlJsonSyncDataService acsMenuChewingService;

        public MenuPublishingEventsHandler(
            IMyAndromedaLogger logger,
            IAcsMenuXmlJsonSyncDataService acsMenuChewingService,
            IGprsService gprsService) 
        {
            this.gprsService = gprsService;
            this.logger = logger;
            this.acsMenuChewingService = acsMenuChewingService;
        }

        public async Task Publishing(MenuPublishContext context)
        {
            this.logger.Debug(format: "GPRS Event: do i have a job for - {0}?", args: new object[] { context.AndromedaSiteId });

            bool isGprs = this.gprsService.IsStoreGprsDevicebyAndromedaSiteId(context.AndromedaSiteId);

            if (!isGprs)
            {
                this.logger.Debug(message: "Nope");
                return;
            }

            this.logger.Debug(format: "GPRS Event starting: {0}", args: new object[] { context.AndromedaSiteId });

            try
            {
                await acsMenuChewingService.UpdateAcsMenuAsync(context.AndromedaSiteId);
            }
            catch (Exception e) 
            {
                this.logger.Error(message: "An error occurred merging the menu changes for ACS.");
                this.logger.Error(e);
            }

            this.logger.Debug(message: "GPRS Event: I've done the job for - {0}");
        }

        public Task Published(MenuPublishContext context)
        {
            return Task.FromResult(result: false);   
        }
    }
}