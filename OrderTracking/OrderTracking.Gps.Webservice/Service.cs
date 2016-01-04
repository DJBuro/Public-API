using System;
using System.Collections.Generic;
using OrderTracking.Gps.Dao;
using OrderTracking.Gps.Dao.Domain;
using OrderTracking.Gps.Webservice.Dao;

namespace OrderTracking.Gps.Webservice
{
    public class Service : IOrderTrackingGps
    {
        public IDeviceDao DeviceDao;
        public IUserDao UserDao;
        public IGateUserDao GateUserDao;
        public IUserGroupDao UserGroupDao;
        public ILicenseDao LicenseDao;

        #region IOrderTrackingGps Members

        public Tracker GetTrackerByName(string trackerName)
        {
            var device = DeviceDao.FindByName(trackerName);

            if (device == null || device.owner_id == null || device.owner_id.Id == null)
                return null;
                
            var gate_user = GateUserDao.FindById(device.owner_id.Id.Value);

            if (gate_user == null)
                return null;

            var tracker = new Tracker { 
                BatteryLevel = 100,
                Longitude = gate_user.Longitude,
                Latitude = gate_user.Latitude, 
                HasFix = false, 
                Name = trackerName,
                LastUpdate = gate_user.Timestamp,
                Speed = gate_user.Groundspeed
            };

            if (gate_user.Timestamp == null)
            {
                return tracker;
            }

            if ((DateTime.UtcNow.Date == gate_user.Timestamp.Value.Date) &&
                (DateTime.UtcNow.Hour == gate_user.Timestamp.Value.Hour))
            {
                int lag = (DateTime.Now - gate_user.Timestamp.Value).Seconds;
                tracker.HasFix = lag < 25;
            }

            if (!tracker.Longitude.HasValue || tracker.Longitude.Value == 0)
            {
                tracker.HasFix = false;
            }

            return tracker;
        }

        public List<Tracker> GetTrackersByNames(List<string> trackerNames)
        {
            var trackers = new List<Tracker>();

            foreach (var trackerName in trackerNames)
            {
                var tracker = GetTrackerByName(trackerName);
                
                if(tracker != null)
                    trackers.Add(tracker);
            }

            return trackers;
        }

        public bool AddTracker(string trackerName, string phoneNumber, int trackerType)
        {
            var user = new User
                           {
                               Username = trackerName,
                               Name = trackerName,
                               Password = "",
                               Active = 1,
                               Created = DateTime.Now,
                               Botype = "GpsGate.Online.GateUser"
                           };

            UserDao.Create(user);

            var gateUser = new GateUser
                               {
                                   Id = user.Id.Value,
                                   Altitude = 0,
                                   Latitude = 0,
                                   Longitude = 0,
                                   Heading = 0,
                                   Groundspeed = 0,
                                   Lasttransport = "tcp",
                                   Timestamp = DateTime.Now,
                                   Servertimestamp = DateTime.Now,
                                   Deviceactivity = DateTime.Now
                               };

            GateUserDao.Create(gateUser);

            var userGroup = new UserGroup {Id = user.Id.Value, Groupid = 1, Grouprightid = 7, Adminrightid = 3};

            UserGroupDao.Create(userGroup);

            var device = new Device();
            device.Devicename = trackerName;
            device.IMEI = trackerName;
            device.Phonenumber = phoneNumber;
            device.Botype = "GpsGate.Online.Directory.Device";
            device.Created = DateTime.Now;
            device.Devdefid = 27; // trackerType;
            device.Protocolid = "Meiligao";
            device.Msgfielddictionary = new Msgfielddictionary {Id = 103};
            device.owner_id = user;
            
            DeviceDao.Create(device);

            return true;
        }

        public bool UpdateTrackerPhoneNumber(string trackerName, string phoneNumber)
        {
            var tracker = DeviceDao.FindByName(trackerName);

            tracker.Phonenumber = phoneNumber;

            DeviceDao.Update(tracker);

            return true;
        }

        public License GetLicense()
        {
            var licenses = LicenseDao.FindAll();

            var y = DeviceDao.FindAll().Count;

            var i = 0;

            foreach (var license in licenses)
            {
                i = i + license.Licensedusers;
            }

            return new License{Licensedusers = i,RegisteredDevices = y};
        }

        #endregion
    }
}
