using MyAndromeda.Data.DataAccess.Chains;
using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using MyAndromeda.Web.Controllers.Api.Data.Models;
using MyAndromeda.Data.DataAccess.Sites;

namespace MyAndromeda.Web.Controllers.Api.Data
{
    public class DataWarehouseDataController: ApiController
    {
        private readonly MyAndromeda.Data.DataWarehouse.Models.DataWarehouseDbContext dataWarehouseDbContext;
        
        private readonly IChainDataService chainDataService;
        private readonly IStoreDataService storeDataService;
        private readonly MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDbContext;

        public DataWarehouseDataController(
            MyAndromeda.Data.DataWarehouse.Models.DataWarehouseDbContext dbContext, 
            MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDbContext,
            IChainDataService chainDataService, 
            IStoreDataService storeDataService
            ) 
        { 
            this.androAdminDbContext = androAdminDbContext;
            this.storeDataService = storeDataService;
            this.chainDataService = chainDataService;
            this.dataWarehouseDbContext = dbContext;
        }

        [Route("chain-data-warehouse/{chainId}")]
        [HttpPost]
        public async Task<DataWarehouseChain> ChainData(
            [FromUri] int chainId,
            [FromBody] DailyReportingQuery queryModel) 
        {
            var chain = this.chainDataService.Get(chainId);

            //var chain = this.chainDataService.Get(chainId);
            //var storeData = await this.storeDataService.Table
            //    .Where(e => e.ChainId == chainId)
            //    .Select(e => new MyAndromeda.Web.Controllers.Api.Data.DailyReportingDataController.StoreParams { AndromedaSiteId = e.AndromedaSiteId, ExternalSiteName = e.ExternalSiteName })
            //    .ToArrayAsync();

            var applications = await androAdminDbContext.ACSApplications
                .Where(e => e.ACSApplicationSites.Any(site => site.Store.ChainId == chainId))
                .Select(e => new { e.ExternalDisplayName, e.Name, e.ExternalApplicationId, e.Id })
                .ToArrayAsync();

            DataWareHouseStore[] stores = await androAdminDbContext.Stores
                .Where(e => e.ChainId == chainId)
                .Select(e => new DataWareHouseStore()
                { 
                     AndromedaSiteId = e.AndromedaSiteId,
                     ExternalSiteName = e.ExternalSiteName,
                     Name = e.Name,
                     ExternalSiteId = e.ExternalId
                })
                .ToArrayAsync();

            int[] applicationIds = applications.Select(e => e.Id).ToArray();
            IEnumerable<string> storeExternalSiteIds = stores.Select(e => e.ExternalSiteId);

            DataWarehouseOrder[] orders = await this.dataWarehouseDbContext.OrderHeaders
                .Where(e => applicationIds.Contains(e.ApplicationID))
                .Where(e => storeExternalSiteIds.Contains(e.ExternalSiteID))
                .Where(e => e.OrderPlacedTime >= queryModel.From)
                .Where(e => e.OrderPlacedTime <= queryModel.To)
                .Select(e => new DataWarehouseOrder
                { 
                    OrderType = e.OrderType,
                    ApplicationId = e.ApplicationID,
                    ExternalSiteId = e.ExternalSiteID,
                    FinalPrice = e.FinalPrice + e.DeliveryCharge,
                    WantedTime = e.OrderWantedTime,   
                    Status = e.Status
                }).ToArrayAsync();

            var result = new DataWarehouseChain() 
            {
                Name = chain.Name
            };
            
            result.Stores = stores.Select(e => new Models.DataWareHouseStore()
            {
                Name = e.Name,
                AndromedaSiteId = e.AndromedaSiteId,
                ExternalSiteId = e.ExternalSiteId,
                ExternalSiteName = e.ExternalSiteName,
                Orders = orders.Where(k => k.ExternalSiteId == e.ExternalSiteId).ToList()
            }).ToList();

            return result;
        }

        [Route("chain-data-warehouse/{chainId}")]
        [HttpPost]
        private void StoreData(DailyReportingQuery queryModel) 
        {
            
        }
    }
}