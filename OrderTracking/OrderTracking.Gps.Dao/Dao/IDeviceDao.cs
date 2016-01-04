using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Dao
{
    public interface IDeviceDao : IGenericDao<Device, int>
    {
        Device FindByName(string name);
    }
}
