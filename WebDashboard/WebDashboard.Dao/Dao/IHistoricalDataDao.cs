using System.Collections.Generic;
using System;

using WebDashboard.Dao.Domain;

namespace WebDashboard.Dao
{
    public interface IHistoricalDataDao : IGenericDao<HistoricalData, int>
    {
        IList<HistoricalData> FindByHeadOffice(HeadOffice headOffice);
        IList<string> GetHistoricalDatesByHeadOffice(HeadOffice headOffice);
        HistoricalData FindBySiteIdAndTradingDay(int siteId, DateTime tradingDay);
        IDictionary<int, HistoricalData> FindByHeadOfficeIdAndTradingDay(int headOfficeId, DateTime tradingDay);
    }
}
