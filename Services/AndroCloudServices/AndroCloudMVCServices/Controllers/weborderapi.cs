using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AndroCloudMVCServices.Services;
using AndroCloudWCFServices;
using AndroCloudMVCServices.ActionResults;

namespace AndroCloudMVCServices.Controllers
{
    public class weborderapiController : Controller
    {
        /// <summary>
        /// Hosts GET
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="partnerId"></param>
        /// <returns>A list of hosts</returns>
        //[WebGet(UriTemplate = "host?partnerId={partnerId}&applicationId={applicationId}")]
        public ActionResult host(string applicationId)
        {
            string responseText = Host.GetAllHosts(Helper.GetDataTypes(this.HttpContext), null, applicationId, this.HttpContext);
            
            return new ServiceResult(responseText);
        }

        /// <summary>
        /// Menu GET
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="partnerId"></param>
        /// <returns>A menu (XML or JSON)</returns>
        //[WebGet(UriTemplate = "menu/{siteId}?partnerId={partnerId}&applicationId={applicationId}")]
        public ActionResult menu(string siteId, string applicationId)
        {
            string responseText = Menu.GetMenu(Helper.GetDataTypes(this.HttpContext), siteId, null, applicationId, this.HttpContext);

            return new ServiceResult(responseText);
        }

        /// <summary>
        /// Site GET
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="orderId"></param>
        /// <param name="partnerId"></param>
        /// <param name="groupIdFilter"></param>
        /// <param name="maxDistanceFilter"></param>
        /// <param name="longitudeFilter"></param>
        /// <param name="latitudeFilter"></param>
        /// <returns>A list of sites</returns>
        //[WebGet(UriTemplate = "site?partnerId={partnerId}&groupId={groupIdFilter}&maxDistance={maxDistanceFilter}&longitude={longitudeFilter}&latitude={latitudeFilter}&applicationId={applicationId}")]
        public ActionResult site(string groupIdFilter, string maxDistanceFilter, string longitudeFilter, string latitudeFilter, string applicationId)
        {
            string responseText = AndroCloudMVCServices.Services.Site.GetSite(Helper.GetDataTypes(this.HttpContext), null, maxDistanceFilter, longitudeFilter, latitudeFilter, applicationId, this.HttpContext);

            return new ServiceResult(responseText);
        }

        /// <summary>
        /// SiteDetails GET
        /// </summary>
        /// <param name="securityToken"></param>
        /// <param name="siteId"></param>
        /// <param name="orderId"></param>
        /// <returns>A list of site details</returns>
        //[WebGet(UriTemplate = "sitedetails/{siteId}?partnerId={partnerId}&applicationId={applicationId}")]
        public ActionResult sitedetails(string siteId, string applicationId)
        {
            string responseText = AndroCloudMVCServices.Services.SiteDetails.GetSiteDetails(Helper.GetDataTypes(this.HttpContext), null, siteId, applicationId, this.HttpContext);

            return new ServiceResult(responseText);
        }

        /// <summary>
        /// Order PUT
        /// </summary>
        /// <param name="input"></param>
        /// <param name="siteId"></param>
        /// <param name="orderId"></param>
        /// <param name="partnerId"></param>
        /// <param name="version"></param>
        /// <returns>A response indicating whether or not the order was successfully received by the store</returns>
        //[WebInvoke(Method = "PUT", UriTemplate = "order/{siteId}/{orderId}?partnerId={partnerId}&applicationId={applicationId}")]
        public ActionResult order(Stream input, string siteId, string orderId, string applicationId)
        {
            string responseText = AndroCloudMVCServices.Services.Order.PutOrder(Helper.GetDataTypes(this.HttpContext), input, siteId, orderId, null, applicationId, this.HttpContext);

            return new ServiceResult(responseText);
        }

        /// <summary>
        /// Order GET
        /// </summary>
        /// <param name="input"></param>
        /// <param name="siteId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        //[WebGet(UriTemplate = "order/{siteId}/{orderId}?partnerId={partnerId}&applicationId={applicationId}")]
        public ActionResult order(string siteId, string orderId, string partnerId, string applicationId)
        {
            string responseText = AndroCloudMVCServices.Services.Order.GetOrder(Helper.GetDataTypes(this.HttpContext), siteId, orderId, partnerId, applicationId, this.HttpContext);

            return new ServiceResult(responseText);
        }

        /// <summary>
        /// PriceCheck PUT
        /// </summary>
        /// <param name="input"></param>
        /// <param name="siteGuid"></param>
        /// <param name="partnerId"></param>
        /// <param name="version"></param>
        /// <returns>A response indicating whether or not the price check was successfull</returns>
        //[WebInvoke(Method = "PUT", UriTemplate = "priceCheck/{siteGuid}?partnerId={partnerId}&applicationId={applicationId}")]
        public ActionResult priceCheck(Stream input, string siteGuid, string partnerId, string applicationId)
        {
            string responseText = PriceCheck.PutPriceCheck(Helper.GetDataTypes(this.HttpContext), input, siteGuid, partnerId, applicationId, this.HttpContext);

            return new ServiceResult(responseText);
        }
    }
}
