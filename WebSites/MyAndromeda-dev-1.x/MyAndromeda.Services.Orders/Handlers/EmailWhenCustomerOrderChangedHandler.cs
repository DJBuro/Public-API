using System;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService;
using MyAndromeda.Services.Orders.Events;

namespace MyAndromeda.Services.Orders.Handlers
{
    public class EmailWhenCustomerOrderChangedHandler : Events.IOrderChangedEvent 
    {
        private readonly ITranslator translator;

        public const string Completed = "Order has been completed";
        public const string Failed = "Order has been cancelled";

        private readonly IMyAndromedaLogger logger;

        private readonly IOrderEmailingService orderService;
        private readonly IMyAndromedaTransactionalEmailService myAndromedaEmailService;

        private readonly ICurrentSite currentSite;

        public EmailWhenCustomerOrderChangedHandler(
            ITranslator translator, 
            IMyAndromedaLogger logger,
            ICurrentSite currentSite, 
            IOrderEmailingService orderService,
            IMyAndromedaTransactionalEmailService myAndromedaEmailService) 
        {
            this.translator = translator;
            this.logger = logger;
            this.orderService = orderService;
            this.myAndromedaEmailService = myAndromedaEmailService;
            this.currentSite = currentSite;
        }

        public string Name
        {
            get
            {
                return "Email customer on status change";
            }
        }

        public async Task<WorkLevel> OrderStatusChangedAsync(int andromedaSiteId, OrderHeader orderHeader, OrderStatu oldStatus)
        {
            if (!this.currentSite.Available) { this.logger.Debug("Order status changed, but this handler cannot deal with the request."); }

            if (orderHeader.Status == 6 || orderHeader.Status > 1000) 
            {
                await this.OrderFailed(orderHeader, andromedaSiteId);
                return WorkLevel.CompletedWork;
            }
            if (orderHeader.Status == 5) 
            { 
                await this.OrderSucceded(orderHeader, andromedaSiteId);
                return WorkLevel.CompletedWork;
            }

            return WorkLevel.NoWork;
        }

        private async Task OrderSucceded(OrderHeader orderHeader, int andromedaSiteId) 
        {
            var email = this.orderService.CreateEmail(
                currentSite.ExternalSiteId,
                orderHeader.RamesesOrderNum,
                message: translator.T(Completed),
                deliveryTime: orderHeader.OrderWantedTime.GetValueOrDefault(DateTime.UtcNow).ToString("g"),
                success: true);

            this.logger.Debug("a completed email is sending");

            //await email.SendAsync();
            await myAndromedaEmailService.SendAsync(email, andromedaSiteId, orderHeader.ID, orderHeader.CustomerID);

            this.logger.Debug("a completed email is sent!");
        }

        private async Task OrderFailed(OrderHeader orderHeader, int andromedaSiteId)
        {
            var email = this.orderService.CreateEmail(
                currentSite.ExternalSiteId,
                orderHeader.RamesesOrderNum,
                message : translator.T(Failed), 
                deliveryTime : orderHeader.OrderWantedTime.GetValueOrDefault(DateTime.UtcNow).ToString("g"), 
                success: false);

            this.logger.Debug("a canceled email is sending");

            //await email.SendAsync();
            await myAndromedaEmailService.SendAsync(email, andromedaSiteId, orderHeader.ID, orderHeader.CustomerID);

            this.logger.Debug("a canceled email is sent!");
        }
    }
}