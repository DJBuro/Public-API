using System;
using System.Linq;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using MyAndromeda.Services.Gprs;
using MyAndromeda.Services.Orders.Events;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders
{
    public class OrderUpdateService : IOrderUpdateService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly ICurrentSite currentSite;
        private readonly IOrderChangedEvent[] events;
        private readonly IOrderStatusDataService orderStatusDataService;
        private readonly ICustomerOrdersDataService customerOrdersDataService;
        private readonly IGprsService gprsService;

        public OrderUpdateService(IOrderChangedEvent[] events,
            IOrderStatusDataService orderStatusDataService,
            ICustomerOrdersDataService customerOrdersDataService,
            ICurrentSite currentSite,
            IMyAndromedaLogger logger,
            IGprsService gprsService)
        {
            this.gprsService = gprsService;
            this.logger = logger;
            this.currentSite = currentSite;
            this.customerOrdersDataService = customerOrdersDataService;
            this.orderStatusDataService = orderStatusDataService;
            this.events = events;
        }

        //public void UpodateOrderStatusHistory(OrderHeader order, OrderStatu status)
        //{
        //    var task = this.orderStatusDataService.AddHistoryAsync(order, status);
        //    task.Wait();
        //}

        public void UpdateOrderWantedTime(string externalSiteId, int ramesesOrderId, DateTime wantedUtcTime)
        {
            var order = this.GetOrder(externalSiteId, ramesesOrderId);

            if (order == null) 
            {
                throw new NullReferenceException("order is null");
            }

            order.OrderWantedTime = wantedUtcTime;
            this.customerOrdersDataService.Update(order);
        }

        public async Task UpdateOrderStatus(int andromeadSiteId, string externalSiteId, int ramesesOrderId, bool success, string msg)
        {
            msg = string.IsNullOrWhiteSpace(msg) ? string.Empty : msg.Trim();

            var allStatus = this.orderStatusDataService.List();
            var order = this.GetOrder(externalSiteId, ramesesOrderId);

            if (order == null)
            {
                throw new NullReferenceException("order is null");
            }

            var specificStatus = allStatus.Where(e => e.Description == msg).FirstOrDefault();
            var maxStaus = allStatus.Max(e => e.Id);

            var oldStatus = order.OrderStatu;

            if (success) 
            {
                //can only ever be this: 
                order.OrderStatu = allStatus.Single(e => e.Description == DataWarehouseDefinitions.OrderStatus.OrderHasBeenCompleted);
            }
            else
            {
                //1000 will be for the escalation message.
                //thsese will append above that
                maxStaus = maxStaus < 1000 ? 1000 : maxStaus;
                //this will always be here
                Func<OrderStatu> genericCancelled = () => 
                    allStatus.Single(e => e.Description == DataWarehouseDefinitions.OrderStatus.OrderHasBeenCancelled);
                //its either found one of previous reasons ... or ... lets add it.
                Func<OrderStatu> specialised = () => 
                    specificStatus ?? new OrderStatu() { Id = maxStaus + 1, Description = msg };

                order.OrderStatu = string.IsNullOrWhiteSpace(msg) ? genericCancelled() : specialised();
            }

            this.customerOrdersDataService.Update(order);

            foreach (var ev in this.events)
            {
                try
                {
                    logger.Debug("Update status event running: " + ev.Name);

                    await ev.OrderStatusChangedAsync(andromeadSiteId, order, oldStatus);
                    //task.Wait();
                }
                catch (Exception e)
                {
                    logger.Error("Update status event handler had a boo boo: " + ev.Name);
                    logger.Error(e);
                    //throw;
                }
                finally 
                {
                    logger.Debug("Update status event handler completed: " + ev.Name);
                }
                
            }
        }

        private OrderHeader GetOrder(string externalSiteId, int orderId)
        {
            if (!currentSite.Available)
            {
                throw new NullReferenceException("CurrentSite in null");
            }

            var query = this.customerOrdersDataService.List()
                            .Where(e => e.RamesesOrderNum == orderId)
                            .Where(e => e.ExternalSiteID == externalSiteId);

            var result = query.SingleOrDefault();

            return result;
        }
    }
}