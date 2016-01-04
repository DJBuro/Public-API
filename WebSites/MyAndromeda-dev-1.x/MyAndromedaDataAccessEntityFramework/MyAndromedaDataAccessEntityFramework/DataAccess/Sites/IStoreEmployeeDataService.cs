using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Sites
{
    public interface IStoreEmployeeDataService : IDependency
    {
        /// <summary>
        /// Lists the store employees.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<StoreEmployee> ListStoreEmployees(Expression<Func<StoreEmployee, bool>> query);

        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="employees">The employees.</param>
        /// <returns></returns>
        IEnumerable<StoreEmployee> AddOrUpdate(int andromedaSiteId, IEnumerable<StoreEmployee> employees);
    }
}