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
    public class CommitLoyaltyPoints : IOrderChangedEvent 
    {
        private readonly ICommitLoyaltyChangesService commitLoyaltyChangesService;
        private readonly IMyAndromedaLogger logger;

        public CommitLoyaltyPoints(ICommitLoyaltyChangesService commitLoyaltyChangesService, IMyAndromedaLogger logger) 
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

                if (!orderHeader.OrderLoyalties.Any())
                {
                    logger.Debug("No loyalty added ... skipping over.");
                    return WorkLevel.NoWork;
                }

                if (orderHeader.Status == 5) 
                {
                    logger.Debug("Loyalty - commit call");
                    await commitLoyaltyChangesService.CommitLoyaltyPointsAsync(orderHeader);

                    return WorkLevel.CompletedWork;
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

            return WorkLevel.NoWork;
        }
    }
}
