using System;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services;

namespace MyAndromeda.Services.Orders.Handlers
{
    public class AddHistoryToOrderStatusChangeHandler : Events.IOrderChangedEvent
    {

        private readonly IOrderStatusDataService orderStatusDataService;

        public AddHistoryToOrderStatusChangeHandler(IOrderStatusDataService orderStatusDataService) 
        {
            this.orderStatusDataService = orderStatusDataService;
        }

        public void OrderStatusChanged(OrderHeader orderHeader, OrderStatu oldOrderStatus)
        {
            if (orderHeader.OrderStatu.Id != oldOrderStatus.Id)
            {
                this.orderStatusDataService.AddHistory(orderHeader, oldOrderStatus);
            }
        }

    }
}
