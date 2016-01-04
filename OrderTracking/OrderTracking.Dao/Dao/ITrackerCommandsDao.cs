using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface ITrackerCommandDao : IGenericDao<TrackerCommand, int>
    {
        IList<TrackerCommand> FindAllSetup(Tracker tracker);
    }
}
