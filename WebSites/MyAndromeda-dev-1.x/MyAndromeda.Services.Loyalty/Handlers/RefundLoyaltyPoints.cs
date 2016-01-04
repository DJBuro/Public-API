using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Loyalty.Handlers
{
    public class RefundLoyaltyPoints : IOrderChangedEvent
    {
        private readonly ICommitLoyaltyChangesService commitLoyaltyChangesService;
        private readonly IMyAndromedaLogger logger;

        public RefundLoyaltyPoints(ICommitLoyaltyChangesService commitLoyaltyChangesService, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.commitLoyaltyChangesService = commitLoyaltyChangesService;
        }

        public string Name
        {
            get
            {
                return "Refund loyalty points handler";
            }
        }

        public async Task<WorkLevel> OrderStatusChangedAsync(int andromedaSiteId, OrderHeader orderHeader, OrderStatu oldStatus)
        {
            try
            {
                if (orderHeader == null) 
                {
                    throw new ArgumentNullException("orderHeader");
                }

                if (orderHeader.OrderLoyalties == null) 
                {
                    throw new ArgumentNullException("orderHeader.OrderLoyaltyies");
                }

                if (orderHeader.Status == 6 || orderHeader.Status > 1000) 
                {
                    logger.Debug("Loyalty - reject call");

                    await commitLoyaltyChangesService.DeclineLoyaltyPointsAsync(orderHeader);

                    return WorkLevel.CompletedWork;
                    
                }
                return WorkLevel.NoWork;
            }
            catch (Exception ex)
            {
                var orderLoyaltyFailed = orderHeader.OrderLoyalties.Select(e => e.Id.ToString());

                logger.Error("Failed updating RefundLoyaltyPoints");

                foreach (var a in orderLoyaltyFailed) { logger.Error("Failed reverting points: " + a); }

                logger.Error(ex);

                throw;
            }
        }
    }
}
