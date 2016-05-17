using MyAndromeda.Core;
using System;
using System.Linq;
using MyAndromeda.Data.DailyReporting.Model.CodeFirst;
using System.Data.Entity;

namespace MyAndromeda.Data.DailyReporting.Services
{
    public interface IDailySummaryDataService : IDependency
    {
        IDbSet<DailySummary> Table { get; }
    }

    public class DailySummaryDataService : IDailySummaryDataService 
    {
        private readonly Model.CodeFirst.DailyReportingCodeFirstDbContext dbContext;

        public DailySummaryDataService(Model.CodeFirst.DailyReportingCodeFirstDbContext dbContext) 
        {
            this.dbContext = dbContext;
            this.Table = dbContext.DailySummaries;
        }

        public IDbSet<DailySummary> Table
        {
            get;
            private set;
        }
    }
}
