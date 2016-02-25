using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Newtonsoft.Json;

namespace MyAndromeda.Web.Controllers.Api
{
    [RoutePrefix("store/{chainId}")]
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

        //[HttpPost]
        //public async Task<DataSourceResult> ListOcassionTimes() 
        //{
        //    string content = await this.Request.Content.ReadAsStringAsync();

        //    DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(content);

        //    return 
        //}
    }
}
