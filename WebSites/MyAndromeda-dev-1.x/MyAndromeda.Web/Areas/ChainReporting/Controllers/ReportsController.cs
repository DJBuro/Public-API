using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting.Query;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.ChainReporting.ViewModels;
using Kendo.Mvc.UI;
using System.Collections.Generic;

namespace MyAndromeda.Web.Areas.ChainReporting.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IAuthorizer authorizer;
        private readonly IDailyReportingSalesAggregateServices dailyReportingService;
        private readonly ICurrentChain loadedChain;

        public ReportsController(IDailyReportingSalesAggregateServices dailyReportingService, ICurrentChain loadedChain, IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
            this.loadedChain = loadedChain;
            this.dailyReportingService = dailyReportingService;
        }

        public ActionResult Index()
        {
            if (!this.authorizer.Authorize(MyAndromeda.Web.Areas.Reporting.UserReportingPermissions.ViewChainReports)) 
            {
                return new HttpUnauthorizedResult();
            }

            return View(loadedChain.Chain);
        }

        public ActionResult Map()
        {
            if (!this.authorizer.Authorize(MyAndromeda.Web.Areas.Reporting.UserReportingPermissions.ViewChainReports))
            {
                return new HttpUnauthorizedResult();
            }

            var stores = loadedChain.SitesBelongingToChain;

            return View(stores);
        }

        [HttpPost]
        public async Task<ActionResult> RangeData([DataSourceRequest]DataSourceRequest request, FilterQuery query)
        {
            if (!this.authorizer.Authorize(MyAndromeda.Web.Areas.Reporting.UserReportingPermissions.ViewChainReports))
            {
                return new HttpUnauthorizedResult();
            }

            var sites = loadedChain.SitesBelongingToChain.Select(e => new
            {
                e.AndromediaSiteId,
                e.ClientSiteName,
                e.ExternalSiteId,
                e.Longitude,
                e.Latitude
            }).ToArray();

            if (query == null)
            {
                DateTime now = DateTime.Today;
                query = new FilterQuery()
                {
                    FilterTo = now,
                    FilterFrom = now.AddDays(-8)
                };
            }

            long[] queryTheseIds = sites.Select(e => (long)e.AndromediaSiteId).ToArray();
            Dictionary<long, DailyMetricGroup[]> data = 
                (
                    await this.dailyReportingService
                    .SalesByDayAsync(queryTheseIds, e => e.TheDate > query.FilterFrom && e.TheDate < query.FilterTo)
                )
                .GroupBy(e => e.AndromediaSiteId)
                .ToDictionary(e => e.Key.GetValueOrDefault(), e => e.ToArray());

            var groups = sites.Select(e => new StoreReportSet
            {
                AndromediaSiteId = e.AndromediaSiteId,
                ClientSiteName = e.ClientSiteName,
                ExternalSiteId = e.ExternalSiteId,
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                Data = data.ContainsKey(e.AndromediaSiteId) ? data[e.AndromediaSiteId] : new DailyMetricGroup[] { }
                //Historic = historic.ContainsKey(e.AndromediaSiteId) ? data[e.AndromediaSiteId] : new DailyMetricGroup[] {}, 
            }).ToArray();

            return new JsonResult()
            {
                Data = groups.ToDataSourceResult(request),
                MaxJsonLength = Int32.MaxValue
            };
        }

    }

}