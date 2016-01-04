using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromedaDataAccess.Domain;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

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
        IEnumerable<MyAndromedaDataAccess.Domain.Employee> GetOrUpdateEmployees(int andromedaSiteId);
    }

    public class EmployeePullService : IStoreEmployeeService
    {
        private readonly IStoreEmployeeOverviewService dailyReportingStoreEmployeeOverviewService;
        private readonly IStoreEmployeeDataService myAndromedaStoreEmployeeDataService;
        
        public EmployeePullService(IStoreEmployeeOverviewService storeEmployeeOverviewService, IStoreEmployeeDataService myAndromedaStoreEmployeeDataService)
        {
            this.myAndromedaStoreEmployeeDataService = myAndromedaStoreEmployeeDataService;
            this.dailyReportingStoreEmployeeOverviewService = storeEmployeeOverviewService;
        }

        public IEnumerable<Employee> GetOrUpdateEmployees(int andromedaSiteId)
        {
            //store backup data
            var readOnlyEmployeeList = this.dailyReportingStoreEmployeeOverviewService
                                           .List(e => e.Nstoreid == andromedaSiteId && e.StrNiNum != "jj128655d" && e.StrEmployeeName != "Developer" && e.StrEmployeeName != "Internet")
                                           .ToArray();

            var emplyeeFacadeList = readOnlyEmployeeList.Select(e => new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.StoreEmployee()
            {
                AndromedaSiteId = andromedaSiteId,
                EmployeeId = e.NEmployeecode.GetValueOrDefault().ToString(),
                PayrollNumber = e.NPayrollNo.GetValueOrDefault().ToString(),
                JobTitle = string.Empty,
                LastUpdated = e.NLastChange.GetValueOrDefault(),
                PhoneNumber = string.Empty,
                Person = new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.Person()
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    FullName = e.StrEmployeeName,
                    NationallnsuranceNumber = e.StrNiNum,
                    DrivingLicenceNumber = e.StrLicenceNumber
                }
            });

            var employees = this.myAndromedaStoreEmployeeDataService.AddOrUpdate(andromedaSiteId, emplyeeFacadeList)

                                .Select(e => new Employee(){ EmployeeId = e.EmployeeId, AndromedaSiteId = andromedaSiteId, Firstname = e.Person.FirstName, Surname = e.Person.LastName, FullName = e.Person.FullName, Role = e.JobTitle, Phone = e.PhoneNumber, PayrollNumber = e.PayrollNumber, NationalInsuranceNumber = e.Person.NationallnsuranceNumber, DrivingLicenceNumber = e.Person.DrivingLicenceNumber });

            return employees;
        }
    }
}