using System;
using MyAndromeda.Core;
using MyAndromeda.Framework;
using MyAndromeda.Web.Areas.Reporting.Context;
using MyAndromedaDataAccess;
using MyAndromedaDataAccess.Domain.Reporting;
using MyAndromedaDataAccess.Domain.Reporting.Query;

namespace MyAndromeda.Web.Areas.Reporting.Services
{
    //public interface ICustomerOverviewService : IDependency 
    //{
    //    CustomersOverview GetData(FilterQuery filter);
    //}

    //public class CustomerOverviewService : ICustomerOverviewService 
    //{
    //    private readonly IReportingContext reportingContext;

    //    private readonly IDataAccessFactory dataAccessFactory;

    //    public CustomerOverviewService(IReportingContext reportingContext, IDataAccessFactory dataAccessFactory) 
    //    {
    //        this.dataAccessFactory = dataAccessFactory;
    //        this.reportingContext = reportingContext;
    //    }

    //    public CustomersOverview GetData(FilterQuery filter)
    //    {
    //        CustomersOverview overview = this.dataAccessFactory.CustomerDataAccess.GetOverview(reportingContext.SiteId, filter);

    //        return overview;
    //    }
    //}
}