using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MyAndromeda.Web.Controllers.WebHooks
{
    public class AlertController : ApiController
    {
        public AlertController() { }

        //[HttpPost,HttpPut]
        //[Route["/WebHooks/OrderStatusChange"]]
        //public async Task<HttpResponseMessage> OrderStatusChange(int adromedaSiteId, string externalStoreId)
        //{
        //    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        //}

        //[HttpPost, HttpPut]
        //public async Task<HttpResponseMessage> StoreOnlineStatusChange(int andromedaSiteId)
        //{
        //    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        //}

        //[HttpPost, HttpPut]
        //public async Task<HttpResponseMessage> OrderEstimatedDeliveryTimeChange(int andromedaSiteId)
        //{
        //    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        //}

        //[HttpPost, HttpPut]
        //public async Task<HttpResponseMessage> MenuVersionChang(int andromedaSiteId, string version)
        //{
        //    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        //}
    }
}