using MyAndromeda.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using MyAndromeda.Data.DailyReporting.Model.CodeFirst;

namespace MyAndromeda.Data.DailyReporting.Services
{
    public interface IStoreEmployeeOverviewService : IDependency 
    {
        IEnumerable<Employee> List(Expression<Func<Employee, bool>> query);
    }

    public class StoreEmployeeOverviewService : IStoreEmployeeOverviewService 
    {
        public IEnumerable<Employee> List(Expression<Func<Employee, bool>> query)
        {
            var results = Enumerable.Empty<Employee>();

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
