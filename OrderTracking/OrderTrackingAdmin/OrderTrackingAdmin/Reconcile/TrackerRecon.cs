
using System.Collections.Generic;
using System.Linq;
using OrderTracking.Dao.Domain;

namespace OrderTrackingAdmin.Reconcile
{
    //todo: move to OrderTrackingAdmin.MVC
    public static class TrackerRecon
    {
        public static void Recon(ref IList<Tracker> trackers)
        {
            //http://gps.andromedagps.com/Trackers.asmx
            //androGps
            var gpsGateWs = new androGps.Trackers();

            var list = new List<string>();

            foreach (var tracker in trackers)
            {
                list.Add(tracker.Name);
            }
            
            var gpsGateTrackers = gpsGateWs.GetTrackersByNames(list.ToArray());

            foreach (Tracker tracker in trackers)
            {
                var gpsGateTracker = from gt in gpsGateTrackers where gt.Name == tracker.Name select gt;

                if (tracker.Coordinates == null)
                {
                    tracker.Coordinates = new Coordinates();
                    tracker.BatteryLevel = 0;
                    tracker.Status = new TrackerStatus(tracker.Status.Name);
                }
                
                if(gpsGateTrackers.Any() && gpsGateTracker.Any())
                {
                    if (gpsGateTracker.ElementAt(0).Latitude != null)
                   {
                       tracker.Coordinates.Latitude = (float) gpsGateTracker.ElementAt(0).Latitude;
                       tracker.Coordinates.Latitude = (float)gpsGateTracker.ElementAt(0).Latitude;
                   }

                    tracker.BatteryLevel = gpsGateTracker.ElementAt(0).BatteryLevel.Value;

                    tracker.Status = new TrackerStatus();

                    if (gpsGateTracker.ElementAt(0).HasFix)
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
        }
    }
}
