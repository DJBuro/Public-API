using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Dao
{
    public interface ITrackDataDao : IGenericDao<Trackdata, int>
    {
        string ReturnTest();
    }
}
