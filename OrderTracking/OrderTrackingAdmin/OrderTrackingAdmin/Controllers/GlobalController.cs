using System.Web.Mvc;
using OrderTracking.Dao;
using OrderTracking.Dao.Domain;
using OrderTrackingAdmin.Models;
using OrderTrackingAdmin.Mvc;
using OrderTrackingAdmin.Mvc.Filters;

namespace OrderTrackingAdmin.Controllers
{
    public class GlobalController : SiteController
    {
        public IApnDao ApnDao;
        public ITrackerTypeDao TrackerTypeDao;
        public ILogDao LogDao;
        public ISmsCredentialsDao SmsCredentialDao;

        [RequiresAuthorisation]
        public ActionResult Index()
        {
            var trackers = new androGps.Trackers();
            var license = trackers.GetLicense();
          
            var data = new OrderTrackingAdminViewData.GlobalViewData
                           {
                               Apns = ApnDao.FindAll(),
                               TrackerTypes = TrackerTypeDao.FindAll()
                           };

            data.ViewString = license.RegisteredDevices + " registered trackers, " + (license.Licensedusers - license.RegisteredDevices) + " licenses available"; 

            return (View(GlobalControllerViews.Index, data));
        }

        [RequiresAuthorisation]
        public ActionResult Apn(int id)
        {
            var data = new OrderTrackingAdminViewData.GlobalViewData {Apn = ApnDao.FindById(id)};

            return (View(GlobalControllerViews.Apn, data));
        }


        [RequiresAuthorisation]
        public ActionResult SmsProvider()
        {
            var data = new OrderTrackingAdminViewData.GlobalViewData
                           {
                               SmsCredential = SmsCredentialDao.FindById(1)
                           };

            return (View(GlobalControllerViews.Sms, data));
        }


        [RequiresAuthorisation]
        public ActionResult AddApn()
        {
            var data = new OrderTrackingAdminViewData.GlobalViewData();

            return (View(GlobalControllerViews.AddApn, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult SaveApn(Apn apn)
        {
            var data = new OrderTrackingAdminViewData.GlobalViewData();

            if (UpdateModel(apn))
            {
                ApnDao.Update(apn);
            }

            return RedirectToAction(GlobalControllerViews.Index,data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult DeleteApn(Apn apn)
        {
            var data = new OrderTrackingAdminViewData.GlobalViewData();

            if (UpdateModel(apn))
            {
                ApnDao.Delete(apn);
            }

            return RedirectToAction(GlobalControllerViews.Index,data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult SaveSms(SmsCredential smsCredential)
        {
            var data = new OrderTrackingAdminViewData.GlobalViewData();

            if (UpdateModel(smsCredential))
            {
                SmsCredentialDao.Update(smsCredential);
            }

            return RedirectToAction(GlobalControllerViews.Index,data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult CreateApn(Apn apn)
        {
            var data = new OrderTrackingAdminViewData.GlobalViewData();

            if (UpdateModel(apn))
            {
                ApnDao.Create(apn);
            }

            return RedirectToAction(GlobalControllerViews.Index, data);
        }


        [RequiresAuthorisation]
        public ActionResult WeeksGlobalLogs()
        {
            var data = new OrderTrackingAdminViewData.GlobalViewData { Logs = LogDao.GlobalLogs() };

            return (View(GlobalControllerViews.Logs, data));
        }

    }
}
