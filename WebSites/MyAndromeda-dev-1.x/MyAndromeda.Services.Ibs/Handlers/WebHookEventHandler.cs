using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Logging;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models.Settings;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.Services.Ibs.Checks;

namespace MyAndromeda.Services.Ibs.Handlers
{
    public class WebHookEventHandler : IWebHooksEvent
    {
        //private readonly IOrderCheckForIbsService orderCheckForIbsService;

        private readonly ICheckOrderIsActiveForIbsService checkOrderForIbsService;

        private readonly IMyAndromedaLogger logger;
        private readonly IIbsService ibsService;
        private readonly IOrderHeaderDataService orderHeaderDataService;
        private readonly IIbsStoreDevice ibsStoreDevice;

        private readonly Task emptyTask = Task.FromResult(result: true);

        public WebHookEventHandler(
            //IOrderCheckForIbsService orderCheckForIbsService,
            ICheckOrderIsActiveForIbsService checkOrderForIbsService,
            IIbsService ibsService, 
            IOrderHeaderDataService orderHeaderDataService, 
            IMyAndromedaLogger logger ,
            IIbsStoreDevice ibsStoreDevice
            )
        {

            this.checkOrderForIbsService = checkOrderForIbsService;
            this.ibsStoreDevice = ibsStoreDevice;
            this.logger = logger;
            this.orderHeaderDataService = orderHeaderDataService;
            this.ibsService = ibsService;
        }

        public string Name
        {
            get
            {
                return "IBS - Create Orders";
            }
        }

        public WebHookType InterestedIn
        {
            get
            {
                return WebHookType.OrderStatus;
            }
        }

        public Task NoWebHooksAsync<TModel>(int andromedaSiteId, TModel model)
            where TModel : IHook
        {
            return emptyTask;
        }

        public Task BeforeDistributionAsync<TModel>(int andromedaStoreId, TModel model)
            where TModel : IHook
        {
            return emptyTask;
        }

        public async Task AfterDistributionAsync<TModel>(int andromedaStoreId, TModel model)
            where TModel : IHook
        {
            if (!(model is OutgoingWebHookOrderStatusChange))
            {
                return;
            }

            var change = model as OutgoingWebHookOrderStatusChange;

            //check if the order in oven... but oven is skipped sometime :-/ 
            //2,3,4,5
            if (change.Status < 2 || change.Status > 5)
            {
                return;
            }

            if (!await ibsStoreDevice.IsIbsSetup(model.AndromedaSiteId))
            {
                return;
            }

            Data.DataWarehouse.Models.OrderHeader orderHeader = await
                orderHeaderDataService.OrderHeaders
                //.AsNoTracking()
                .Include(e => e.Customer)
                .Include(e => e.Customer.Contacts)
                .Include(e => e.Customer.Address)
                .Include(e => e.CustomerAddress)
                .Include(e => e.OrderLines)
                    .Where(e => e.ExternalOrderRef == change.ExternalOrderId)
                    .Where(e => e.ExternalSiteID == model.ExternalSiteId)
                    .SingleOrDefaultAsync();

            if (orderHeader.IbsOrders.Any())
            {
                return;
            }

            try
            {
                if (this.checkOrderForIbsService.IsOrderProcessing(orderHeader.ID))
                {
                    return;
                }

                this.checkOrderForIbsService.AddOrderToBeProcessed(orderHeader.ID, model.AndromedaSiteId);

                Models.CustomerResultModel customer = await ibsService.GetCustomerAsync(model.AndromedaSiteId, orderHeader);

                if (customer == null) { customer = await ibsService.AddCustomerAsync(model.AndromedaSiteId, orderHeader); }

                Models.AddOrderRequest createOrderRequest = await ibsService.CreateOrderData(model.AndromedaSiteId, orderHeader, customer);

                Models.AddOrderResult createOrderResult = await this.ibsService.AddOrderAsync(model.AndromedaSiteId, orderHeader, customer, createOrderRequest);

            }
            catch (Exception e)
            {
                //i don't trust the finally. :) 
                this.checkOrderForIbsService.RemoveOrderFromProcessing(orderHeader.ID);
                throw;
            }
            finally
            {
                this.checkOrderForIbsService.RemoveOrderFromProcessing(orderHeader.ID);
            }

            return;
        }

        public Task SendingRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            return emptyTask;
        }

        public Task SentRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            return emptyTask;
        }

        public Task FailedRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            return emptyTask;
        }
    }
}
