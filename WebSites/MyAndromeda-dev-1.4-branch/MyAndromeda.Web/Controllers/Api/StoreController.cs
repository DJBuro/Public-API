using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Web.Controllers.Api
{
    public class StoreController : ApiController
    {
        private readonly IStoreDataService storeDataService;

        public StoreController(IStoreDataService storeDataService)
        {
            this.storeDataService = storeDataService;
        }

        [HttpGet]
        public IEnumerable<object> Get() 
        {
            return this.storeDataService.List()
                .Select(e => new { 
                e.ExternalId,
                e.AndromedaSiteId,
                e.ClientSiteName
            })
            .OrderBy(e=> e.ClientSiteName);
        }
    }
}
