
using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface ILogDao : IGenericDao<Log, int>
    {
        IList<Log> Last20Logs(string externalStoreId);
        IList<Log> TodaysLogs(string externalStoreId);
        IList<Log> WeeksLogs(string externalStoreId);
        IList<Log> AllAccountLogs(string externalStoreId);
        IList<Log> GlobalLogs();
    }
}
