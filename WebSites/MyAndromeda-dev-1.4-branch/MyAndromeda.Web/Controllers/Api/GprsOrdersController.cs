using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MyAndromeda.Web.Controllers.Api
{
    public class GprsOrdersController : ApiController
    {
        private readonly IStoreDataService storeDataService;
        private readonly ICustomerOrdersDataService orderHeaderDataService;

        public GprsOrdersController(IStoreDataService storeDataService, ICustomerOrdersDataService orderHeaderDataService)
        { 
            this.orderHeaderDataService = orderHeaderDataService;
            this.storeDataService = storeDataService;
        }

        [HttpGet]
        public IEnumerable<object> Get(int andromedaSiteId)
        {
            var store = this.storeDataService.List()
                .Where(e => e.AndromedaSiteId == andromedaSiteId)
                .Single();

            if (!store.StoreDevices.Any()) { return new List<object>();  }

            return this.orderHeaderDataService.List()
                //.Where(e => e.DestinationDevice == "iBT8000")
                .Where(e => e.ExternalSiteID == store.ExternalId)
                .Select(e => new { e.ID, e.Status, e.RamesesOrderNum })
                .OrderBy(e => e.RamesesOrderNum);
        }

    }
}
