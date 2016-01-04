using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.Events;

namespace MyAndromeda.Services.Loyalty.Handlers
{
    public class OrderChangedEventHandler : IOrderChangedEvent 
    {
        private readonly ICommitLoyaltyChangesService commitLoyaltyChangesService;
        private readonly IMyAndromedaLogger logger;

        public OrderChangedEventHandler(ICommitLoyaltyChangesService commitLoyaltyChangesService, IMyAndromedaLogger logger) 
        {
            this.logger = logger;
            this.commitLoyaltyChangesService = commitLoyaltyChangesService;
        }

        public string Name
        {
            get
            {
                return "Update loyalty points handler";
            }
        }

        public async Task OrderStatusChanged(OrderHeader orderHeader, OrderStatu oldStatus)
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

                if (!orderHeader.OrderLoyalties.Any())
                {
                    logger.Debug("No loyalty added ... skipping over.");
                    return;
                }

                switch (orderHeader.Status)
                {
                    case 5:
                        {
                            logger.Debug("Loyalty - commit call");
                            await commitLoyaltyChangesService.CommitLoyaltyPoints(orderHeader);
                            break;
                        }
                    case 6:
                        {
                            logger.Debug("Loyalty - reject call");
                            await commitLoyaltyChangesService.DeclineLoyaltyPoints(orderHeader);
                            break;
                        }

                    default: { break; }
                }

	        }
	        catch (Exception ex)
	        {
                var orderLoyaltyFailed = orderHeader.OrderLoyalties.Select(e => e.Id.ToString());

                logger.Error("Failed in updating Loyalty.OrderStatusChanged");

                foreach (var a in orderLoyaltyFailed) { logger.Error("Failed committing: " + a); }

		        logger.Error(ex);

		        throw;
	        }
        }
    }
}
