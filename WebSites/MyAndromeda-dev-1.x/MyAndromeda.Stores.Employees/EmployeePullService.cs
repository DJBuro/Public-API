using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DailyReporting.Services;
using Domain = MyAndromeda.Data.Domain;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Stores.Employees
{
    public class EmployeePullService : IStoreEmployeeService
    {
        private readonly IStoreEmployeeOverviewService dailyReportingStoreEmployeeOverviewService;
        private readonly IStoreEmployeeDataService myAndromedaStoreEmployeeDataService;
        
        public EmployeePullService(IStoreEmployeeOverviewService storeEmployeeOverviewService, IStoreEmployeeDataService myAndromedaStoreEmployeeDataService)
        {
            this.myAndromedaStoreEmployeeDataService = myAndromedaStoreEmployeeDataService;
            this.dailyReportingStoreEmployeeOverviewService = storeEmployeeOverviewService;
        }

        public IEnumerable<Domain.Employee> GetOrUpdateEmployees(int andromedaSiteId)
        {
            //store backup data
            var readOnlyEmployeeList = this.dailyReportingStoreEmployeeOverviewService
                                           .List(e => e.Nstoreid == andromedaSiteId && e.StrNiNum != "jj128655d" && e.StrEmployeeName != "Developer" && e.StrEmployeeName != "Internet")
                                           .ToArray();

            var emplyeeFacadeList = readOnlyEmployeeList.Select(e => new StoreEmployee()
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

                                .Select(e => new Domain.Employee() { EmployeeId = e.EmployeeId, AndromedaSiteId = andromedaSiteId, Firstname = e.Person.FirstName, Surname = e.Person.LastName, FullName = e.Person.FullName, Role = e.JobTitle, Phone = e.PhoneNumber, PayrollNumber = e.PayrollNumber, NationalInsuranceNumber = e.Person.NationallnsuranceNumber, DrivingLicenceNumber = e.Person.DrivingLicenceNumber });

            return employees;
        }
    }
}