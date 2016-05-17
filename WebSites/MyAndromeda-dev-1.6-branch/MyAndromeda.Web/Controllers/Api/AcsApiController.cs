using MyAndromeda.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MyAndromeda.Web.Controllers.Api
{
    public class AcsApiController : ApiController
    {
        private readonly IAcsApplicationDataService dataService;
        public AcsApiController(IAcsApplicationDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet]
        [Route("api/acs/list/{andromedaSiteId}")]
        //[HttpGet]
        public IEnumerable<object> Get(int andromedaSiteId)
        {
            var query = this.dataService.Query().Where(e => e.ACSApplicationSites.Any(o => o.Store.AndromedaSiteId == andromedaSiteId));
            var result = query.Select(e => new { 
                e.Id,
                e.Name,
                e.ExternalDisplayName,
                PartnerName = e.Partner.Name,
                e.ExternalApplicationId
            })
            .OrderBy(e=> e.Name)
            .ToArray();

            return result;
        }
    }
}