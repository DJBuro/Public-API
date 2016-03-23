using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Logging;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.Services.WebHooks.Models.Settings;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Bringg.Handlers
{
    public class UpdateBringgTaskEventHandler : IWebHooksEvent
    {
        private readonly Task emptyTask = Task.FromResult(false);

        private readonly IBringgService bringgSerivce;
        private readonly IMyAndromedaLogger logger;
        private readonly IOrderHeaderDataService orderHeaderDataService;

        public UpdateBringgTaskEventHandler(IBringgService bringgSerivce, IMyAndromedaLogger logger, IOrderHeaderDataService orderHeaderDataService)
        {
            this.orderHeaderDataService = orderHeaderDataService;
            this.logger = logger;
            this.bringgSerivce = bringgSerivce;
        }

        public string Name
        {
            get
            {
                return "Update Bringg Task";
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

            if (string.IsNullOrWhiteSpace(update.ExternalOrderId))
            {
                this.logger.Error(message: "ExternalOrderId is missing from order status update");
            }

            if (!update.InternalOrderId.HasValue)
            {
                this.logger.Error(message: "InternalOrderId is missing from order status update");
            }

            //already jobs for create / cancel 
            if (update.Status == 1 || update.Status == 4 || update.Status == 6 || update.Status == 5 || update.Status >= 1000) { return; }

            try
            {
                bool isAvailable = await this.bringgSerivce.IsBringgConfigured(andromedaSiteId);

                if (!isAvailable) { return; }

                Data.DataWarehouse.Models.OrderHeader orderHeader = await this.orderHeaderDataService.OrderHeaders.SingleOrDefaultAsync(e => e.ID == update.InternalOrderId);

                if (!orderHeader.BringgTaskId.HasValue) 
                {
                    this.logger.Error("BringgTask id expected for order id: " + orderHeader.ID);
                    return;
                }

                await this.bringgSerivce.AddOrderAsync(andromedaSiteId, orderHeader.ID, addNotes: false);
            }
            catch (Exception ex)
            {
                this.logger.Error(message: "Updating the bring task has thrown an error");
                this.logger.Error(ex);
                //throw;
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
