using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Data.DataAccess.Sites;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Stores.Employees
{
    public class EmployeePullService : IStoreEmployeeService
    {
        private readonly IStoreEmployeeOverviewService dailyReportingStoreEmployeeOverviewService;
        private readonly IStoreEmployeeDataService myAndromedaStoreEmployeeDataService;
        
        public EmployeePullService(
            IStoreEmployeeOverviewService storeEmployeeOverviewService, 
            IStoreEmployeeDataService myAndromedaStoreEmployeeDataService)
        {
            this.myAndromedaStoreEmployeeDataService = myAndromedaStoreEmployeeDataService;
            this.dailyReportingStoreEmployeeOverviewService = storeEmployeeOverviewService;
        }

        public IEnumerable<EmployeeDomainModel> GetOrUpdateEmployees(int andromedaSiteId)
        {
            //store backup data
            Data.DailyReporting.Model.CodeFirst.Employee[] readOnlyEmployeeList = this.dailyReportingStoreEmployeeOverviewService
                                           .List(e => e.Nstoreid == andromedaSiteId && e.StrNiNum != "jj128655d" && e.StrEmployeeName != "Developer" && e.StrEmployeeName != "Internet")
                                           .ToArray();

            IEnumerable<StoreEmployee> emplyeeFacadeList = readOnlyEmployeeList.Select(e => new StoreEmployee()
            {
                AndromedaSiteId = andromedaSiteId,
                EmployeeId = e.NEmployeecode.GetValueOrDefault().ToString(),
                PayrollNumber = e.NPayrollNo.GetValueOrDefault().ToString(),
                JobTitle = string.Empty,
                LastUpdated = e.NLastChange.GetValueOrDefault(),
                PhoneNumber = string.Empty,
                Person = new Person()
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    FullName = e.StrEmployeeName,
                    NationallnsuranceNumber = e.StrNiNum,
                    DrivingLicenceNumber = e.StrLicenceNumber
                }
            });

            var employees = this.myAndromedaStoreEmployeeDataService.AddOrUpdate(andromedaSiteId, emplyeeFacadeList)
                .Select(e => new EmployeeDomainModel() { EmployeeId = e.EmployeeId, AndromedaSiteId = andromedaSiteId, Firstname = e.Person.FirstName, Surname = e.Person.LastName, FullName = e.Person.FullName, Role = e.JobTitle, Phone = e.PhoneNumber, PayrollNumber = e.PayrollNumber, NationalInsuranceNumber = e.Person.NationallnsuranceNumber, DrivingLicenceNumber = e.Person.DrivingLicenceNumber });

            return employees;
        }
    }
}