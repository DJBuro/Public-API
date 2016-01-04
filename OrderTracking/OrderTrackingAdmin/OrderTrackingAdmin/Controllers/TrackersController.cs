using System.Web.Mvc;
using OrderTracking.Dao;
using OrderTracking.Dao.Domain;
using OrderTrackingAdmin.Models;
using OrderTrackingAdmin.Mvc;
using OrderTrackingAdmin.Mvc.Extensions;
using OrderTrackingAdmin.Mvc.Filters;
using OrderTrackingAdmin.Mvc.Utilities;

namespace OrderTrackingAdmin.Controllers
{
    public class TrackersController : SiteController
    {
        public ITrackerDao TrackerDao;
        public IApnDao ApnDao;
        public ITrackerTypeDao TrackerTypeDao;
        public ITrackerCommandDao TrackerCommandDao;
        public ISmsCredentialsDao SmsCredentialDao;

        [RequiresAuthorisation]
        public ActionResult Index()
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData();

           

            return (View(TrackersControllerViews.Index, data));
        }

        [RequiresAuthorisation]
        public ActionResult Add()
        {

            var trackers = new androGps.Trackers();

            var data = new OrderTrackingAdminViewData.TrackerViewData
                           {
                               ApnListItems = ApnDao.FindAll().ToSelectList("Id", "Provider"),
                               TrackerTypeListItems = TrackerTypeDao.FindAll().ToSelectList("Id", "Name"),
                               StoreListItems = StoreDao.FindAllGpsEnabled().ToSelectList("Id", "Name"),
                           };

            var license = trackers.GetLicense();

            data.LicenseCount = license.Licensedusers - license.RegisteredDevices;

            return (View(TrackersControllerViews.Add, data));
        }

        [RequiresAuthorisation]
        public ActionResult All()
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData {Trackers = TrackerDao.FindAll(true)};

            return (View(TrackersControllerViews.All, data));
        }

        [RequiresAuthorisation]
        public ActionResult Tracker(string id)
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData {Tracker = TrackerDao.FindByName(id)};

            var trackers = new androGps.Trackers();
            var tracker = trackers.GetTrackerByName(id);

            if(tracker !=null)
            {
                data.Tracker.Coordinates = new Coordinates();

                if(tracker.Longitude != null)
                {
                    data.Tracker.Coordinates.Longitude = (float) tracker.Longitude;
                    data.Tracker.Coordinates.Latitude = (float) tracker.Latitude;
                }

                data.Tracker.BatteryLevel = tracker.BatteryLevel.Value;

                if (tracker.HasFix)
                {
                    data.Tracker.Status.Id = 1;
                    data.Tracker.Status.Name = "Alive";
                }
                else
                {
                    data.Tracker.Status.Id = 2;
                    data.Tracker.Status.Name = "Offline";
                }
            }
            else
            {
                data.Tracker.Coordinates = new Coordinates();
                data.Tracker.Status = new TrackerStatus();
                data.Tracker.Status.Id = 3;
                data.Tracker.Status.Name = "Unknown";
                data.Tracker.BatteryLevel = 0;
            }

            data.ApnListItems = ApnDao.FindAll().ToSelectList("Id", "Provider", data.Tracker.Apn.Id.ToString());
            data.TrackerTypeListItems = TrackerTypeDao.FindAll().ToSelectList("Id", "Name", data.Tracker.Type.Id.ToString());
            data.StoreListItems = StoreDao.FindAllGpsEnabled().ToSelectList("Id", "Name", data.Tracker.Store.Id.ToString());

            return (View(TrackersControllerViews.Tracker, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult AddTracker(Tracker tracker)
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData();

            

            if (UpdateModel(tracker))
            {
                tracker.SerialNumber = tracker.Name;
                tracker.IMEI = tracker.Name;
                tracker.Status = new TrackerStatus {Id = 3};

                data.Tracker = TrackerDao.Create(tracker);

               var trackers = new androGps.Trackers();
               var active = trackers.AddTracker(tracker.Name, tracker.PhoneNumber, data.Tracker.Type.GpsGateId);

                data.Tracker.Active = active;

                TrackerDao.Save(data.Tracker);
            }

            data.TrackerCommands = TrackerCommandDao.FindAllSetup(data.Tracker);

            return (View(TrackersControllerViews.Setup, data));
        }        
        
        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult UpdateTracker(Tracker tracker)
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData();

            var t = TrackerDao.FindByName(tracker.Name);
            if(t.PhoneNumber != tracker.PhoneNumber)
            {
                var oldT = TrackerDao.FindByPhoneNumber(tracker.PhoneNumber);

                if(oldT != null)
                {
                    return Tracker(tracker.Name);
                }

                var trackers = new androGps.Trackers();
                var success = trackers.UpdateTrackerPhoneNumber(tracker.Name, tracker.PhoneNumber);
            }

            if (UpdateModel(tracker))
            {
                tracker.SerialNumber = tracker.Name;
                tracker.IMEI = tracker.Name;
                tracker.BatteryLevel = 100;
                tracker.Status = new TrackerStatus {Id = 3};
                TrackerDao.Update(tracker);
            }

            data.Tracker = tracker;
            
           return (View(TrackersControllerViews.Index, data));
        }

        [RequiresAuthorisation]
        public ActionResult Setup(string id)
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData {Tracker = TrackerDao.FindByName(id)};

            data.TrackerCommands = TrackerCommandDao.FindAllSetup(data.Tracker);

            return (View(TrackersControllerViews.Setup, data));
        }

        [RequiresAuthorisation]
        public ActionResult Map(string id)
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData {Tracker = TrackerDao.FindByName(id)};

            var trackers = new androGps.Trackers();
            var tracker = trackers.GetTrackerByName(id);

            data.Tracker.Coordinates = new Coordinates();
           
            if(tracker.Latitude != null)
            {
                data.Tracker.Coordinates.Longitude = (float) tracker.Longitude.Value;
                data.Tracker.Coordinates.Latitude = (float) tracker.Latitude.Value;
            }

            return (View(TrackersControllerViews.Map, data));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult RunSetupCommands(string id, string phone)
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData {Tracker = TrackerDao.FindByName(id)};

            var commands = TrackerCommandDao.FindAllSetup(data.Tracker);

            var smsCredential = SmsCredentialDao.FindById(1);

            //note: this is coded purely for GT30
            foreach (var command in commands)
            {
                var reply = SmsCommands.RunAdminSmsSetup(smsCredential.Username, smsCredential.Password, data.Tracker.PhoneNumber, phone, SmsCommands.BuildCommand(data.Tracker, command));

                Logging.LogNotice("RunSetupCommands", data.Tracker.Store.ExternalStoreId, "notice", "none", "Tracker: <strong>" + data.Tracker.Name + "</strong> Number: <strong>" + data.Tracker.PhoneNumber + "</strong> Command: <strong>" + command.Name + "</strong> Reply: <strong>" + reply + "</strong>");
            }

            return RedirectToAction(TrackersControllerViews.Tracker+"/" + data.Tracker.Name, data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult RunSingleSetupCommands(string id, string phone, int cmdId)
        {
            var data = new OrderTrackingAdminViewData.TrackerViewData { Tracker = TrackerDao.FindByName(id) };

            var command = TrackerCommandDao.FindById(cmdId);

            var smsCredential = SmsCredentialDao.FindById(1);

            //note: this is coded purely for GT30
            var reply = SmsCommands.RunAdminSmsSetup(smsCredential.Username, smsCredential.Password, data.Tracker.PhoneNumber, phone, SmsCommands.BuildCommand(data.Tracker, command));

            Logging.LogNotice("RunSingleSetupCommands", data.Tracker.Store.ExternalStoreId, "notice", "none", "Tracker: <strong>" + data.Tracker.Name + "</strong> Number: <strong>" + data.Tracker.PhoneNumber + "</strong> Command: <strong>" + command.Name + "</strong> Reply: <strong>" + reply + "</strong>");

            return RedirectToAction(TrackersControllerViews.Tracker + "/" + data.Tracker.Name, data);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult FindByStore(string id)
        {
            var store = StoreDao.FindByExternalId(id);

            if (store == null)
                return RedirectToAction("index"); 


            var data = new OrderTrackingAdminViewData.TrackerViewData { Trackers = TrackerDao.FindByStore(store) };

            return (View(TrackersControllerViews.All, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult FindByPhone(string id)
        {
            var tracker = TrackerDao.FindByPhoneNumber(id);

            if (tracker == null)
                return RedirectToAction("index"); 

            return this.Tracker(tracker.Name);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresAuthorisation]
        public ActionResult FindByName(string id)
        {
            var tracker = TrackerDao.FindByName(id);

            if (tracker == null)
               return RedirectToAction("index"); 


            return this.Tracker(tracker.Name);
        }

    }
}
