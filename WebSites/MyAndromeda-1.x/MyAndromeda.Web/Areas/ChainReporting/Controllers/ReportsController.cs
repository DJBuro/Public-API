using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.ChainReporting.ViewModels;
using MyAndromedaDataAccess.Domain.Reporting.Query;
using Kendo.Mvc.UI;

namespace MyAndromeda.Web.Areas.ChainReporting.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IDailyReportingSalesAggregateServices dailyReportingService;
        private readonly ICurrentChain loadedChain;
        private readonly ICurrentUser currentUser;

        public ReportsController(IDailyReportingSalesAggregateServices dailyReportingService, ICurrentChain loadedChain, ICurrentUser currentUser)
        {
            this.currentUser = currentUser;
            this.loadedChain = loadedChain;
            this.dailyReportingService = dailyReportingService;
        }

        public ActionResult Index()
        {
            return View(loadedChain.Chain);
        }

        public ActionResult Map()
        {
            var stores = loadedChain.SitesBelongingToChain;

            return View(stores);
        }

        [HttpPost]
        public ActionResult RangeData([DataSourceRequest]DataSourceRequest request, FilterQuery query)
        {
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
                var now = DateTime.Today;
                query = new FilterQuery()
                {
                    FilterTo = now,
                    FilterFrom = now.AddDays(-8)
                };
            }

            var queryTheseIds = sites.Select(e => (long)e.AndromediaSiteId).ToArray();
            var data = this.dailyReportingService
                .SalesByDay(queryTheseIds, e => e.TheDate > query.FilterFrom && e.TheDate < query.FilterTo)
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