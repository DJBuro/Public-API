using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System.Data.Entity;
using MyAndromeda.Web.Controllers.Api.Store.Models;

namespace MyAndromeda.Web.Controllers.Api.Store
{
    [RoutePrefix("api/chain/{chainId}/store")]
    public class StoreController : ApiController
    {
        private readonly IStoreDataService storeDataService;
        private readonly MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDbContext;
        private readonly DbSet<MyAndromeda.Data.Model.AndroAdmin.StoreOccasionTime> storeOccasionTimes;

        public StoreController(IStoreDataService storeDataService, MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDbContext)
        {
            this.androAdminDbContext = androAdminDbContext;
            this.storeOccasionTimes = androAdminDbContext.Set<MyAndromeda.Data.Model.AndroAdmin.StoreOccasionTime>();
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

        [HttpPost]
        [Route("{andromedaSiteId}/Occasions")]
        public async Task<DataSourceResult> ListOcassionTimes(int andromedaSiteId) 
        {
            string content = await this.Request.Content.ReadAsStringAsync();

            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(content);

            var occasions = await this.storeOccasionTimes
                .Where(e => e.Store.AndromedaSiteId == andromedaSiteId)
                .ToArrayAsync();

            return new DataSourceResult()
            {
                Total = occasions.Length,
                Data = occasions.Select(e => e.ToViewModel())
            };

            //return  
        }
    }
}
