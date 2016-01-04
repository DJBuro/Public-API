using System;
using OrderTracking.Gps.Dao;
using System.Web.Mvc;
using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Controllers
{
    public class TrackersController : Controller
    {
        public IDeviceDao DeviceDao;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 10, VaryByParam = "id")]
        public JsonResult Tracker(string id)
        {
            var device = DeviceDao.FindByName(id);

            var tracker = new Tracker {BatteryLevel = 100,Longitude = device.Longitude, Latitude = device.Latitude, HasFix = false, Name = id};

            if ((DateTime.Now.Date == device.Timestamp.Value.Date) &&
                (DateTime.Now.Hour == device.Timestamp.Value.Hour) &&
                (DateTime.Now.Minute == device.Timestamp.Value.Minute)
                )
            {
                tracker.HasFix = (DateTime.Now - device.Timestamp.Value).Seconds < 20;
            }

        var jsonResult = Json(tracker, JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

    }
}
