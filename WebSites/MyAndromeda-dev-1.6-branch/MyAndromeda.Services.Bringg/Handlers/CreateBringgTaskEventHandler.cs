using System;
using System.Data.Entity;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Logging;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.Services.WebHooks.Models.Settings;
using MyAndromeda.Services.Bringg.Services.Checks;

namespace MyAndromeda.Services.Bringg.Handlers
{
    public class CreateBringgTaskEventHandler : IWebHooksEvent 
    {
        private readonly Task emptyTask = Task.FromResult(result: false);

        private readonly IBringgService bringgSerivce;
        private readonly IMyAndromedaLogger logger;
        private readonly IOrderHeaderDataService orderHeaderDataService;
        private readonly ICheckNewOrderIsActiveForBringgService checkNewOrderIsActiveForBringgService;

        public CreateBringgTaskEventHandler(
            ICheckNewOrderIsActiveForBringgService checkNewOrderIsActiveForBringgService,
            IBringgService bringgSerivce, 
            IMyAndromedaLogger logger, 
            IOrderHeaderDataService orderHeaderDataService)
        {
            this.checkNewOrderIsActiveForBringgService = checkNewOrderIsActiveForBringgService;
            this.orderHeaderDataService = orderHeaderDataService;
            this.logger = logger;
            this.bringgSerivce = bringgSerivce;
        }

        public string Name
        {
            get
            {
                return "Bringg create order handler";
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
            return emptyTask;
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
                this.logger.Error(message: "ExternalOrderId is missing from orderstatus update");
            }
            if (!update.InternalOrderId.HasValue) 
            {
                this.logger.Error(message: "InternalOrderId is missing from orderstatus update");
                return;
            }

            if (this.checkNewOrderIsActiveForBringgService.IsOrderProcessing(update.InternalOrderId.GetValueOrDefault()))
            {
                this.logger.Debug("{0} - store: {1} - The order already processing - {2}", update.InternalOrderId, update.AndromedaSiteId, update.ExternalOrderId);

                return;
            }

            //passed the checks ... lets do some work. 

            Data.DataWarehouse.Models.OrderHeader orderHeader = await this.orderHeaderDataService.OrderHeaders.SingleOrDefaultAsync(e => e.ID == update.InternalOrderId);

            if (orderHeader.BringgTaskId.HasValue) 
            {
                this.logger.Debug("{0} - The order already has a bringgId - {1} (skipping 'create' task)", 
                    orderHeader.ExternalOrderRef, 
                    orderHeader.BringgTaskId);

                return;
            }

            //this.orderHeaderDataService.GetByOrderId(update.InternalOrderId.GetValueOrDefault());
            
            bool validStore = await this.bringgSerivce.IsBringgConfigured(andromedaSiteId);
            
            if (!validStore)
            {
                this.logger.Debug("Bringg is not configured: " + andromedaSiteId);
                return;
            }

            if (orderHeader.BringgTaskId.HasValue)
            {
                this.logger.Debug("Bringg task already has a BringgTaskId: orderid - " + orderHeader.ID);
                return;
            }

            try
            {
                this.logger.Debug("Sending the order to BRINGG: " + orderHeader.ExternalOrderRef);
                UsefulOrderStatus currentState = orderHeader.GetState();

                //create order - completed for GPRS 
                //create order - created for Rameses
                bool createOrderForBring = await this.bringgSerivce.ShallWeSendOrder(andromedaSiteId, currentState);

                if (!createOrderForBring)
                {
                    return;
                }

                bool delivery = orderHeader.OrderType.ToLower().Equals(value: "DELIVERY", comparisonType: StringComparison.InvariantCultureIgnoreCase);

                if (!delivery) { return; }

                this.logger.Debug("Try to use Bringg: " + andromedaSiteId);

                await this.bringgSerivce.AddOrderAsync(andromedaSiteId, orderHeader.ID, addNotes: false);

            }
            catch (Exception e)
            {
                this.checkNewOrderIsActiveForBringgService.RemoveOrderFromProcessing(orderHeader.ID);
                this.logger.Error(e);
                throw;
            }
            finally
            {
                this.checkNewOrderIsActiveForBringgService.RemoveOrderFromProcessing(orderHeader.ID);
            }
        }

        public Task AfterDistributionAsync<TModel>(int andromedaSiteId, TModel model) where TModel : IHook
        {
            return emptyTask;
        }

        public Task SendingRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return emptyTask;
        }

        public Task SentRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return emptyTask;
        }

        public Task FailedRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return emptyTask;
        }
    }
}