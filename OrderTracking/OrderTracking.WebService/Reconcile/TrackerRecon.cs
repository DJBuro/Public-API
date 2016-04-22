using System.Collections.Generic;
using System.Linq;
using OrderTracking.Dao.Domain;

namespace OrderTracking.WebService.Reconcile
{
    //todo: move to OrderTrackingAdmin.MVC
    public static class TrackerRecon
    {
        public static void ReconTrackers(ref Account account)
        {
            var list = new List<string>();

            foreach (Tracker tracker in account.Store.Trackers)
            {
                list.Add(tracker.Name);
            }

            var gpsGateWs = new AndroGps.Trackers();
            var allGpsGateTrackers = gpsGateWs.GetTrackersByNames(list.ToArray());

            foreach (Tracker tracker in account.Store.Trackers)
            {
                var gpsGateTrackers = allGpsGateTrackers.Where(gt => gt.Name == tracker.Name);

                if (tracker.Coordinates == null)
                {
                    tracker.Coordinates = new Coordinates();
                    tracker.BatteryLevel = 0;
                    tracker.Status = new TrackerStatus(tracker.Status.Name);
                }

                if (!gpsGateTrackers.Any()) continue;

                if (gpsGateTrackers.ElementAt(0).Latitude != null)
                {
                    tracker.Coordinates.Latitude = (float)gpsGateTrackers.ElementAt(0).Latitude;                    
                    tracker.Coordinates.Longitude = (float)gpsGateTrackers.ElementAt(0).Longitude;
                }

                tracker.BatteryLevel = gpsGateTrackers.ElementAt(0).BatteryLevel.Value;
                tracker.LastUpdate = gpsGateTrackers.ElementAt(0).LastUpdate;
                tracker.Speed = gpsGateTrackers.ElementAt(0).Speed;
                
                tracker.Status = new TrackerStatus();
                if (gpsGateTrackers.ElementAt(0).HasFix)
                {
                    tracker.Status.Id = 1;
                    tracker.Status.Name = "Alive";
                }
                else
                {
                    tracker.Status.Id = 2;
                    tracker.Status.Name = "Offline";
                }
            }
        }

        public static void ReconTracker(ref Tracker tracker)
        {
            var trackers = new AndroGps.Trackers();
            var trackerByName = trackers.GetTrackerByName(tracker.Name);

            if (tracker.Coordinates == null)
            {
                tracker.Coordinates = new Coordinates();
                tracker.BatteryLevel = 0;
                tracker.Status = new TrackerStatus(tracker.Status.Name);
            }

            if (trackerByName == null) return;

            tracker.Coordinates.Latitude = (float)trackerByName.Latitude.Value;
            tracker.Coordinates.Longitude = (float)trackerByName.Longitude.Value;

            if (trackerByName.HasFix)
            {
                tracker.Status.Id = 1;
                tracker.Status.Name = "Alive";
            }
            else
            {
                tracker.Status.Id = 2;
                tracker.Status.Name = "Offline";
            }         
        }

        public static void ReconTrackers(ref List<Tracker> trackers)
        {
            //http://debug/Trackers.asmx
            //androGps
            var gpsGateWs = new AndroGps.Trackers();

            var nameList = TrackerNameList(trackers);

            var gpsGateTrackers = gpsGateWs.GetTrackersByNames(nameList.ToArray());

            foreach (var tracker in trackers)
            {
                var enumerable = gpsGateTrackers.Where(gt => gt.Name == tracker.Name);

                if (tracker.Coordinates == null)
                {
                    tracker.Coordinates = new Coordinates();
                    tracker.BatteryLevel = 0;
                    tracker.Status = new TrackerStatus(tracker.Status.Name);
                }

                if (!enumerable.Any()) continue;

                if (enumerable.ElementAt(0).Latitude != null)
                {
                    tracker.Coordinates.Latitude = (float)enumerable.ElementAt(0).Latitude;
                    tracker.Coordinates.Longitude = (float)enumerable.ElementAt(0).Longitude;
                }
                    
                tracker.BatteryLevel = enumerable.ElementAt(0).BatteryLevel.Value;

                tracker.Status = new TrackerStatus();

                if (enumerable.ElementAt(0).HasFix)
                {
                    tracker.Status.Id = 1;
                    tracker.Status.Name = "Alive";
                }
                else
                {
                    tracker.Status.Id = 2;
                    tracker.Status.Name = "Offline";
                }
            }
        }


        private static List<string> TrackerNameList(IEnumerable<Tracker> trackers)
        {
            var list = new List<string>();

            foreach (var tracker in trackers)
            {
                list.Add(tracker.Name);
            }

            return list;
        }
    }
}
