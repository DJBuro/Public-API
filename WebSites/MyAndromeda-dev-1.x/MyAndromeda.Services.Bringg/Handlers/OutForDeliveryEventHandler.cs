using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models.Settings;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Services.Orders;

namespace MyAndromeda.Services.Bringg.Handlers
{
    public class OutForDeliveryEventHandler : IWebHooksEvent 
    {
        private readonly Task emptyTask = Task.FromResult(result: false);

        private readonly IBringgService bringgService;
        private readonly IMyAndromedaLogger logger;
        private readonly IStoreDataService storeDataService;
        private readonly IOrderHeaderDataService orderHeaderDataService;

        private readonly INotifier notifier;

        public OutForDeliveryEventHandler(IBringgService bringgService,
            IMyAndromedaLogger logger,
            IStoreDataService storeDataService,
            INotifier notifier,
            IOrderHeaderDataService orderHeaderDataService) 
        {
            this.orderHeaderDataService = orderHeaderDataService;
            this.notifier = notifier;
            this.storeDataService = storeDataService;
            this.logger = logger;
            this.bringgService = bringgService;
        }

        public string Name
        {
            get
            {
                return "Bringg update driver handler";
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

        public Task BeforeDistributionAsync<TModel>(int andromedaSiteId, TModel model) where TModel : IHook
        {
            return emptyTask;
        }

        public async Task AfterDistributionAsync<TModel>(int andromedaSiteId, TModel model) where TModel : IHook
        {
            bool valid = model is OutgoingWebHookOrderStatusChange;

            if (!valid) { return; }

            var modelType = model as OutgoingWebHookOrderStatusChange;
            UsefulOrderStatus status = MyAndromeda.Data.DataWarehouse.OrderStatusExtensions.GetState(modelType.Status);

            if (status != UsefulOrderStatus.OrderIsOutForDelivery)
            {
                return;
            }
            

            try
            {
                Data.DataWarehouse.Models.OrderHeader orderHeader = await this.orderHeaderDataService.OrderHeaders.SingleOrDefaultAsync(e => e.ID == modelType.InternalOrderId);

                if (!orderHeader.BringgTaskId.HasValue) 
                {
                    this.logger.Debug(message: "Skipping adding driver - no task id");
                    return;
                }

                if (!orderHeader.OrderType.Equals(value: "Delivery", comparisonType: StringComparison.InvariantCultureIgnoreCase)) 
                {
                    this.logger.Debug(message: "Skipping order because it is not delivery");
                    return;
                }

                this.logger.Debug("Try to assign driver to bring task id:" + orderHeader.BringgTaskId.Value);
                this.logger.Debug("Bags" + orderHeader.Bags.GetValueOrDefault());
                
                Data.Model.AndroAdmin.Store store = await this.storeDataService.Table.SingleOrDefaultAsync(e => e.AndromedaSiteId == andromedaSiteId);

                await this.bringgService.AddOrderAsync(andromedaSiteId, orderHeader.ID, addNotes: true);
                UpdateDriverResult result = await this.bringgService.UpdateDriverAsync(store.Id, modelType.InternalOrderId, modelType.ExternalOrderId);

                switch (result) 
                {
                    case UpdateDriverResult.CantFindOrderInWarehouse:
                        throw new NullReferenceException(message: "Order"); 
                    case UpdateDriverResult.NoBringgTaskId:
                        throw new ArgumentNullException(paramName: "BringgTaskId"); 
                    case UpdateDriverResult.NoDriverName:
                        throw new ArgumentNullException(paramName: "DriverName");
                    case UpdateDriverResult.NoDriverPhoneNumber:
                        throw new ArgumentNullException(paramName: "DriverPhoneNumber");
                    case UpdateDriverResult.UnknownError :
                        throw new Exception(message: "Unknown error from bringg");
                }

            }
            catch (Exception ex)
            {
                this.notifier.Error(message: "Failed to update driver");
                this.logger.Error(message: "failed to update driver.");
                this.logger.Error(ex);
                throw;
            }

            
        }

        public Task SendingRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return emptyTask;
        }

        public Task SentRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return emptyTask;
        }

        public Task FailedRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model) where TModel : IHook
        {
            return emptyTask;
        }
    }
}
