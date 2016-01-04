using System;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Logging;

namespace MyAndromeda.Services.Orders.Handlers
{
    public class EmailWhenCustomerOrderChangedHandler : Events.IOrderChangedEvent 
    {
        private readonly ITranslator translator;

        public const string Completed = "Order has been completed";
        public const string Failed = "Order has been cancelled";

        private readonly IMyAndromedaLogger logger;

        private readonly IOrderEmailingService orderService;
        private readonly ICurrentSite currentSite;

        public EmailWhenCustomerOrderChangedHandler(
            ITranslator translator, 
            IMyAndromedaLogger logger,
            ICurrentSite currentSite, 
            IOrderEmailingService orderService) 
        {
            this.translator = translator;
            this.logger = logger;
            this.orderService = orderService;
            this.currentSite = currentSite;
        }

        public string Name
        {
            get
            {
                return "Email customer on status change";
            }
        }

        public async Task OrderStatusChanged(OrderHeader orderHeader, OrderStatu oldStatus)
        {
            if (!this.currentSite.Available) { this.logger.Debug("Order status changed, but this handler cannot deal with the request."); }

            if (orderHeader.Status == 6 || orderHeader.Status > 1000) 
            {
                await this.OrderFailed(orderHeader); 
            }
            if (orderHeader.Status == 5) 
            { 
                await this.OrderSucceded(orderHeader); 
            }
        }

        private async Task OrderSucceded(OrderHeader orderHeader) 
        {
            var email = this.orderService.CreateEmail(
                currentSite.ExternalSiteId,
                orderHeader.RamesesOrderNum,
                message: translator.T(Completed),
                deliveryTime: orderHeader.OrderWantedTime.GetValueOrDefault(DateTime.UtcNow).ToString("g"),
                success: true);

            this.logger.Debug("a completed email is sending");

            await email.SendAsync();

            this.logger.Debug("a completed email is sent!");
        }

        private async Task OrderFailed(OrderHeader orderHeader)
        {
            var email = this.orderService.CreateEmail(
                currentSite.ExternalSiteId,
                orderHeader.RamesesOrderNum,
                message : translator.T(Failed), 
                deliveryTime : orderHeader.OrderWantedTime.GetValueOrDefault(DateTime.UtcNow).ToString("g"), 
                success: false);

            this.logger.Debug("a canceled email is sending");

            await email.SendAsync();

            this.logger.Debug("a canceled email is sent!");
        }
    }
}