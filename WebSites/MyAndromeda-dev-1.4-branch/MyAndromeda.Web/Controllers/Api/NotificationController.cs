using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Web.Models;

namespace MyAndromeda.Web.Controllers.Api
{
    public class NotificationController : ApiController
    {
        private readonly INotifierEvent notifierEvent;

        public NotificationController(INotifierEvent notifierEvent) 
        {
            this.notifierEvent = notifierEvent;
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/Notify")]
        public async Task<HttpResponseMessage> Notify([FromBody]NotificationContext notification) 
        {
            this.notifierEvent.OnNotify(notification);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/Success")]
        public async Task<HttpResponseMessage> Success([FromBody]NotificationContext notification)
        {
            this.notifierEvent.OnSuccess(notification);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/Error")]
        public async Task<HttpResponseMessage> Error([FromBody]NotificationContext notification)
        {
            this.notifierEvent.OnNotify(notification);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/Announce")]
        public async Task<HttpResponseMessage> Announce([FromBody]NotificationContext notification) 
        {
            this.notifierEvent.OnNotify(notification);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
