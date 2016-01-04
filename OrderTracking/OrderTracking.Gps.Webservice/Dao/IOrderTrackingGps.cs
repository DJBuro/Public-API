using System.Collections.Generic;
using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Webservice.Dao
{
    public interface IOrderTrackingGps
    {
        Tracker GetTrackerByName(string trackerName);
        List<Tracker> GetTrackersByNames(List<string> trackerNames);
        bool AddTracker(string trackerName, string phoneNumber, int trackerType);
        bool UpdateTrackerPhoneNumber(string trackerName, string phoneNumber);
        License GetLicense();
    }
}
