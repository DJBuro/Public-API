using System.Web.Mvc;
using GpsGate.Client;
using OrderTracking.Gps.Dao;
/*
 http://gps.andromedagps.com
 
 */
namespace OrderTracking.Gps.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ITrackDataDao TrackDataDao;
        public IDeviceDao DeviceDao;
        
        //TODO: set 10 set cache on this
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            var blah = DeviceDao.FindByName("999");

         
            var client = new GpsGateClient("94.236.121.24", 30175);

            //client.Connect("vtAdmin", "ImM5RtSc");
            client.Connect("123", "hello");

            var ppp = client.GetLastTrackPoint("vtadmin");

            //var blah = client.ReceiveUsernameList;

            var xy = new double();
            xy = 10000;

            client.RequestBuddyListTrackPoints(xy);

            client.Disconnect();
           /*    */


            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 10, VaryByParam = "id")]
        public JsonResult GetTracker(string id)
        {
            var blah = DeviceDao.FindByName(id);

            var jsonResult = Json(blah, JsonRequestBehavior.AllowGet);

            return jsonResult;
        }


    }
}
