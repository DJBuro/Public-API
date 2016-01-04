using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Logging;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models.Settings;
using MyAndromeda.Services.WebHooks.Models;

namespace MyAndromeda.Services.Ibs.Handlers
{
    public class WebHookEventHandler : IWebHooksEvent
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IIbsService ibsService;
        private readonly IOrderHeaderDataService orderHeaderDataService;
        private readonly IIbsStoreDevice ibsStoreDevice;

        private readonly Task emptyTask = Task.FromResult(false);

        public WebHookEventHandler(
            IIbsService ibsService, 
            IOrderHeaderDataService orderHeaderDataService, 
            IMyAndromedaLogger logger ,
            IIbsStoreDevice ibsStoreDevice
            )
        {

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
            if (!(model is OrderStatusChange))
            {
                return;
            }

            var change = model as OrderStatusChange;

            //check if the order in oven. 
            if (change.Status != 2)
            {
                return;
            }

            if (!await ibsStoreDevice.IsIbsSetup(model.AndromedaSiteId))
            {
                return;
            }

            var orderHeader = await
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

            var customer = await ibsService.GetCustomerAsync(model.AndromedaSiteId, orderHeader);

            if (customer == null) { customer = await ibsService.AddCustomerAsync(model.AndromedaSiteId, orderHeader); }

            var createOrderRequest = await ibsService.CreateOrderData(model.AndromedaSiteId, orderHeader, customer);

            var createOrderResult = await this.ibsService.AddOrderAsync(model.AndromedaSiteId, orderHeader, customer, createOrderRequest);

            
            //model.
            //var createOrderRequest = ibsService.CreateOrderData(order, customer);
            //var order = await this.GetOrderHeader(orderId);
            //var customer = await ibsService.GetCustomerAsync(andromedaSiteId, order);
            //if (customer == null) { customer = await ibsService.AddCustomerAsync(andromedaSiteId, order); }

            //this.ibsService.AddOrderAsync()

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
