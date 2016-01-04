using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Services.Orders.Context;
using MyAndromeda.Services.Orders.Emails;

namespace MyAndromeda.Services.Orders.Services
{
    public class OrderSuppertEmailService : IOrderSuppertEmailService 
    {
        private readonly IOrderSupportService orderSupportService;
        
        private readonly EmailConfiguration configuration;

        public OrderSuppertEmailService(IOrderSupportService orderSupportService, EmailConfiguration configuration)
        {
            this.orderSupportService = orderSupportService;
            this.configuration = configuration;
        }

        public OrderWatchingEmail CreateEmailBasedOnOrderIds(IEnumerable<Guid> ids) 
        {
            var data = this.orderSupportService.List(ids);
            var dateServicesFactory = new DateServicesFactory();
            var email = new Emails.OrderWatchingEmail()
            {
                DateServicesFactory = dateServicesFactory,
                EmailTo = this.configuration.OrderMonitoringEailToValue,
                StoreOrderCollection = data.ToList()
            };

            return email;
        }
    }
}