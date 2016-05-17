using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Logging;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models.Settings;
using MyAndromeda.Services.WebHooks.Models;

namespace MyAndromeda.Services.Bringg.Handlers
{
    public class CancelBringgTaskEventHandler : IWebHooksEvent
    {
        private readonly Task emptyTask = Task.FromResult(false);

        private readonly IBringgService bringgSerivce;
        private readonly IMyAndromedaLogger logger;
        private readonly IOrderHeaderDataService orderHeaderDataService;


        public CancelBringgTaskEventHandler(
            IBringgService bringgSerivce, 
            IMyAndromedaLogger logger, 
            IOrderHeaderDataService orderHeaderDataService)
        {
            this.orderHeaderDataService = orderHeaderDataService;
            this.logger = logger;
            this.bringgSerivce = bringgSerivce;
        }

        public string Name
        {
            get
            {
                return "Cancel Bringg Task";
            }
        }

        public WebHookType InterestedIn
        {
            get
            {
                return WebHookType.OrderStatus;
            }
        }

        public Task NoWebHooksAsync<TModel>(int andromedaSiteId, TModel model) where TModel : IHook
        {
            return this.emptyTask;
        }

        public async Task BeforeDistributionAsync<TModel>(int andromedaSiteId, TModel model) where TModel : IHook
        {
            bool correctType = (model is OutgoingWebHookOrderStatusChange);

            if (!correctType)
            {
                return;
            }

            var update = model as OutgoingWebHookOrderStatusChange;

            //canceled 
            bool cancelled = update.Status == 6;
            bool rejected = update.Status > 1000;

            if (!cancelled && !rejected) { return; }

            if (string.IsNullOrWhiteSpace(update.ExternalOrderId))
            {
                this.logger.Error("ExternalOrderId is missing from order status update");
            }

            if (!update.InternalOrderId.HasValue)
            {
                this.logger.Error("InternalOrderId is missing from order status update");
            }

            try
            {
                var orderHeader = await this.orderHeaderDataService.OrderHeaders.SingleOrDefaultAsync(e => e.ID == update.InternalOrderId);

                if (!orderHeader.BringgTaskId.HasValue) { return; }

                var isAvailable = await this.bringgSerivce.IsBringgConfigured(andromedaSiteId);
                if (!isAvailable) { return; }

                this.logger.Debug("Try to cancel bringg task for orderid: " + update.InternalOrderId);
                //create order - completed for GPRS 
                //create order - created for Rameses
                
                var result = await this.bringgSerivce.CancelOrder(andromedaSiteId, update.InternalOrderId, update.ExternalOrderId);

                if (result) 
                {
                    this.logger.Debug("The bringg task should be cancelled");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Canceling the bring task has thrown an error");

                throw;
            }
            
        }

        public Task AfterDistributionAsync<TModel>(int andromedaSiteId, TModel model) where TModel : IHook
        {
            return this.emptyTask;
        }

        public Task SendingRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return this.emptyTask;
        }

        public Task SentRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return this.emptyTask;
        }

        public Task FailedRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return this.emptyTask;
        }
    }
}
