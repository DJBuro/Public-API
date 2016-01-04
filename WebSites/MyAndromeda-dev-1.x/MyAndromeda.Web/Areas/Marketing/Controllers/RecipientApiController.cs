using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Helpers;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Services.Marketing;

namespace MyAndromeda.Web.Areas.Marketing.Controllers
{
    public class RecipientApiController : ApiController
    {
        
        private readonly IRecipientListForEventMarketingService recipientListForEventMarketingService;

        public RecipientApiController(IRecipientListForEventMarketingService recipientListForEventMarketingService) 
        {
            this.recipientListForEventMarketingService = recipientListForEventMarketingService;
        }

        [HttpPost]
        [Route("marketing/{andromedaSiteId}/marketing/previewRecipients")]
        public async Task<IEnumerable<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>> ListPeople([FromUri]int andromedaSiteId, [FromBody]MarketingEventCampaign model) 
        {
            var people = await this.recipientListForEventMarketingService.GetRecipients(model);

            return people;

        }
    }
}