using System.Collections.Generic;
using System.Runtime.InteropServices;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface ITrackerDao : IGenericDao<Tracker, int>
    {
        Tracker FindByName(string name, Store store);
        Tracker FindByName(string name);
        Tracker FindByPhoneNumber(string phoneNumber);
        IList<Tracker> FindByStore(Store store);
        IList<Tracker> FindAll(bool orderByStore);
        void RemoveTrackerFromDriver(ref Tracker tracker);
        void RemoveTrackerFromDriver(ref Driver driver);
        void RemoveTrackerFromDriver(Driver driver);
    }
}
