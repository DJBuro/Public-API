using MyAndromeda.Core;
using MyAndromeda.Data.DailyReporting.Services;

namespace MyAndromeda.Stores.Employees
{
    public interface IEmplyoeeService : IDependency
    {
        /// <summary>
        /// Pulls or updates the employees.
        /// </summary>
        /// <param name="androAdminId">The andro admin id.</param>
        /// <returns></returns>
        //IEnumerable<MyAndromedaDataAccess.Domain.Employee> PullOrUpdateEmployees(int androAdminId);
    }

    public class EmployeeService : IEmplyoeeService
    {
        private readonly IStoreEmployeeOverviewService storeEmployeeOverviewService;

        public EmployeeService(IStoreEmployeeOverviewService storeEmployeeOverviewService)
        {
            this.storeEmployeeOverviewService = storeEmployeeOverviewService;
        }
    }
}