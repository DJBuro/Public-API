using MyAndromeda.Web.Controllers.WebHooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MyAndromeda.Web.Controllers.WebHooks
{
    public class NotificationApiController : ApiController
    {

        public NotificationApiController() 
        {
        
        }

        

       
        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-online-status")]
        public async Task<HttpResponseMessage> OnlineState(StoreOnlineStatus model)
        {
            //to-do: find anyone to notify

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-order-status")]
        public async Task<HttpResponseMessage> OrderStatusChange(OrderStatusChange model)
        {
            //to-do: find any

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-estimated-delivery-time")]
        public async Task<HttpResponseMessage> UpdateDeliveryTime(UpdateDeliveryTime model)
        {
            //to-do: find anyone to notify

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-menu")]
        public async Task<HttpResponseMessage> MenuChange(MenuChange model)
        {
            //to-do: find anyone to notify

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        

    }
}