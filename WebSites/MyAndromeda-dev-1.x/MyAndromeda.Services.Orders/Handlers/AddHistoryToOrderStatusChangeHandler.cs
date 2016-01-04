using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services;
using MyAndromeda.Services.Orders.Events;

namespace MyAndromeda.Services.Orders.Handlers
{
    public class AddHistoryToOrderStatusChangeHandler : Events.IOrderChangedEvent
    {

        private readonly IOrderStatusDataService orderStatusDataService;

        public AddHistoryToOrderStatusChangeHandler(IOrderStatusDataService orderStatusDataService) 
        {
            this.orderStatusDataService = orderStatusDataService;
        }

        public string Name
        {
            get
            {
                return "Update Order Status history";
            }
        }

        public async Task<WorkLevel> OrderStatusChangedAsync(int andromedaSiteId, OrderHeader orderHeader, OrderStatu oldOrderStatus)
        {
            if (orderHeader.OrderStatu.Id != oldOrderStatus.Id)
            {
                await this.orderStatusDataService.AddHistoryAsync(orderHeader, oldOrderStatus);

                return WorkLevel.CompletedWork;
            }

            return WorkLevel.NoWork;
        }

    }
}
