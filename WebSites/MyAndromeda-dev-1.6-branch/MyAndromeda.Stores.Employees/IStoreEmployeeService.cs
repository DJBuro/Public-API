using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Stores.Employees
{
    /// <summary>
    /// Service to pull update from Daily Summary to MyAndromeda
    /// </summary>
    public interface IStoreEmployeeService : IDependency
    {
        /// <summary>
        /// Pulls the or update employees.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        IEnumerable<EmployeeDomainModel> GetOrUpdateEmployees(int andromedaSiteId);
    }
}