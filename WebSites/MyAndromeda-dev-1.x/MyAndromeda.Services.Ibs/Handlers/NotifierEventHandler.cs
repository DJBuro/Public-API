using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Services.Ibs.Models;

namespace MyAndromeda.Services.Ibs.Handlers
{
    public class NotifierEventHandler : Events.IIbsOrderEvents
    {
        private readonly INotifier notifier;

        public NotifierEventHandler(INotifier notifier)
        {
            this.notifier = notifier;
        }

        public async Task OrderCreatingAsync(OrderHeader orderHeader, Models.CustomerResultModel customerResult)
        {
            string message = string.Format("Creating order for: {0}; from OrderHeader.Id: {1}", customerResult.FirstName, orderHeader.ID);
            this.notifier.Success(message);
        }

        public async Task OrderCreatingFailedAsync(OrderHeader orderHeader, CustomerResultModel customerResult, Exception ex)
        {
            this.notifier.Error(ex.Message);
        }

        public async Task OrderRequestCreatedAsync(OrderHeader orderHeader, Models.CustomerResultModel customerResult, Models.AddOrderRequest orderRequest)
        {
            string message = string.Format("Created order for: {0}; from OrderHeader.Id: {1}", customerResult.FirstName, orderHeader.ID);
            this.notifier.Success(message);
        }

        public async Task OrderSendingAsync(Models.LocationResult locationResult, Models.CustomerResultModel customerResult, Models.AddOrderRequest addOrderRequest)
        {
            string message = string.Format("Sending order for: {0}; Sending order to: {1}", customerResult.FirstName, locationResult.SiteReference);
            this.notifier.Success(message);
        }

        public async Task OrderSentAsync(Models.LocationResult locationResult, Models.CustomerResultModel customerResult, Models.AddOrderResult orderResult)
        {
            string message = string.Format("Sending order for: {0}; Sending order to: {1}", customerResult.FirstName, locationResult.SiteReference);
            this.notifier.Success(message);
        }

        public async Task OrderFailedAsync(Models.LocationResult locationResult, Models.CustomerResultModel customerResult, Models.AddOrderFailure orderFailure)
        {
            string message = string.Format("Sending order failed for: {0}; Sending order to: {1}", customerResult.FirstName, locationResult.SiteReference);
            this.notifier.Error(message);
        }
    }
}
