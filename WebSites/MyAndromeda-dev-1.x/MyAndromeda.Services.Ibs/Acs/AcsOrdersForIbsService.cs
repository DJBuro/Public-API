using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Logging;

namespace MyAndromeda.Services.Ibs.Acs
{
    public class AcsOrdersForIbsService : IAcsOrdersForIbsService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IOrderHeaderDataService orderHeaderDataService;
        private readonly AndroAdminDbContext androAdminDbContext;
        private readonly DataWarehouseDbContext dataWarehouseDbContext;

        public AcsOrdersForIbsService(IOrderHeaderDataService orderHeaderDataService,
            AndroAdminDbContext androAdminDbContext,
            DataWarehouseDbContext dataWarehouseDbContext,
            IMyAndromedaLogger logger) 
        {
            this.logger = logger;
            this.dataWarehouseDbContext = dataWarehouseDbContext;
            this.androAdminDbContext = androAdminDbContext;
            this.orderHeaderDataService = orderHeaderDataService;
        }

        //public IQueryable<OrderHeader> AllOrders()
        //{
        //    // TODO: Implement this method
        //    throw new NotImplementedException();
        //}

        public IQueryable<StoreDevice> IbsStores() 
        {
            IQueryable<StoreDevice> query = this.androAdminDbContext.StoreDevices
                            .Include(e => e.Store)
                            .Where(e => e.Device.Name == "IBS");

            return query;
        }

        public IQueryable<OrderHeader> AllOrders(string[] externalSiteIds)
        {
            IQueryable<OrderHeader> query = this.orderHeaderDataService
                .OrderHeaders
                .Where(e => externalSiteIds.Contains(e.ExternalSiteID));

            return query;
        }

        public IQueryable<OrderHeader> ReadyOrders(string[] externalSiteIds)
        {
            IQueryable<OrderHeader> query = this.AllOrders(externalSiteIds);

            IQueryable<OrderHeader> readyOrdersQuery = query
                                        .Where(e => e.Status == (int)UsefulOrderStatus.OrderIsInOven);
         
            return readyOrdersQuery;            
        }

        public async Task SaveSentOrderToIbsAsync(OrderHeader orderHeader, Models.AddOrderResult result)
        {
            this.dataWarehouseDbContext.IbsOrders.Add(new IbsOrder()
            {
                OrderHeaderId = orderHeader.ID,
                IbsOrderId = result.Id,
                CustomerId = result.CustomerId,
                CreatedAtUtc = DateTime.UtcNow
            });

            try
            {
                await this.dataWarehouseDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.logger.Error(message: "Could not save in datawarehouse");
                this.logger.Error(ex);
                throw;
            }
            //await this.orderHeaderDataService.SaveChangesAsync();
        }
    }
}