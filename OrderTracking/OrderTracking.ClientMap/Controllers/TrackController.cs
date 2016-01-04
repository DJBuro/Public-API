using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using OrderTracking.ClientMap.Models;

namespace OrderTracking.ClientMap.Controllers
{
    public class TrackController : Controller
    {
        public ActionResult Index(string chain)
        {
            if(chain.Length == 0)
                return  View("Error");
            
            var data = new TrackingViewData.TrackViewData {SiteName = chain};
 
            return View("Index", data);
        }

        public ActionResult Order(string chain, string credentials)
        {
            if (credentials.Length == 0)
                return RedirectToAction("Error");

            var data = new TrackingViewData.TrackViewData {SiteName = chain, Credentials = credentials};

            return View("Order", data);
        }

        public JsonResult GetOrder(string chain, string credentials)
        {
            var j = new JsonResult();

            var data = new TrackingViewData.TrackViewData();           
            var tracking = new ClientMapService.ExternalMapTracking();

            data.ClientMapData = tracking.GetClientOrderByCredentials(credentials);

            j.Data = data.ClientMapData;

            return j;
        }

        public ActionResult Error()
        {
            return View("Error");
        }

    }
}
