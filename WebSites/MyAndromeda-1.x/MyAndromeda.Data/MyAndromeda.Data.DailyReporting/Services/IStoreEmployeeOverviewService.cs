using MyAndromeda.Core;
using System;
using System.Linq;
using MyAndromeda.Data.DailyReporting.Model.CodeFirst;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyAndromeda.Data.DailyReporting.Services
{
    public interface IStoreEmployeeOverviewService : IDependency 
    {
        IEnumerable<Employees> List(Expression<Func<Employees, bool>> query);
    }

    public class StoreEmployeeOverviewService : IStoreEmployeeOverviewService 
    {
        public IEnumerable<Employees> List(Expression<Func<Employees, bool>> query)
        {
            var results = Enumerable.Empty<Employees>();

            using (var dbContext = new DailyReportingCodeFirstDbContext()) 
            {
                var table = dbContext.Employees;
                var tableQuery = query == null ? table :
                    table.Where(query);

                results = tableQuery.ToArray();
            }

            return results;
        }
    }
}
