using System.Threading.Tasks;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting.Query;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Areas.Reporting.Context;
using MyAndromeda.Web.Areas.ChainReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    [MyAndromedaAuthorize]
    public class DailyReportingController : Controller
    {
        private readonly IDateServices dateServices;
        private readonly ICurrentSite currentSite;
        private readonly IAuthorizer authorizer;
        private readonly ITranslator translator;
        private readonly INotifier notifier; 
        private readonly IReportingContext reportingContext;
        private readonly IDailyReportingSalesAggregateServices dailyReportingService;

        public DailyReportingController(IReportingContext reportingContext,
            IDailyReportingSalesAggregateServices dailyReportingService,
            ICurrentSite currentSite,
            IAuthorizer authorizer,
            ITranslator translator,
            INotifier notifier,
            IDateServices dateServices) 
        {
            this.dateServices = dateServices;
            this.notifier = notifier;
            this.translator = translator;
            this.currentSite = currentSite;
            this.authorizer = authorizer;
            this.reportingContext = reportingContext;
            this.dailyReportingService = dailyReportingService;
        }

        public ActionResult Index() 
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicRamesesReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));

                return new HttpUnauthorizedResult();
            }

            return View(reportingContext);
        }

        public ActionResult ServiceTime()
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicRamesesReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));

                return new HttpUnauthorizedResult();
            }

            return View(reportingContext);
        }

        [ChildActionOnly]
        public PartialViewResult Range(FilterQuery filter)
        {
            return PartialView(filter);
        }

        

        public async Task<ActionResult> Hourly(DateTime? day) 
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicRamesesReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));

                return new HttpUnauthorizedResult();
            }


            DateTime queryFor = day.HasValue ? day.Value : DateTime.Today.AddDays(-1);

            var ids = new long[] { this.currentSite.AndromediaSiteId };
            DailyMetricGroup[] data = (await dailyReportingService.SalesByHourAsync(ids,
                query =>
                    query.Thedate.Year == queryFor.Year &&
                    query.Thedate.Month == queryFor.Month &&
                    query.Thedate.Day == queryFor.Day
                )).ToArray();

            //data above may be missing some hours ... which the grids make... so time to fill in the missing hours 
            DateTime max = data.Max(e=> e.Date).GetValueOrDefault();
            DateTime min = data.Min(e=> e.Date).GetValueOrDefault();
            TimeSpan timespan = max - min;
            //var hours = timespan.Hours + 1;

            //if (hours != data.Length) { 
            //    throw new Exception(string.Format("no {0} hours {1} records", hours, data.Length));
            //}

            var set = new StoreReportSet() { 
                Data = data.Length == 0 ? data : FixByFill(data, min, max).ToArray(), //fill in some missing hours
                AndromediaSiteId = this.currentSite.AndromediaSiteId,
                ClientSiteName = this.currentSite.Site.ClientSiteName,
                ExternalSiteId = this.currentSite.Site.ExternalSiteId
            };

            return Json(set);
        }

        private IEnumerable<DailyMetricGroup> FixByFill(DailyMetricGroup[] data, DateTime min, DateTime max)
        {
            int start = min.Hour;
            for (var i = start; i <= max.Hour; i++)
            {
                DailyMetricGroup hourItem = data.FirstOrDefault(e=> e.Date.Value.Hour == i);
                if (hourItem != null)
                {
                    yield return hourItem;
                }
                else 
                {
                    yield return new DailyMetricGroup()
                    {
                        Date = new DateTime(min.Year, min.Month, min.Day, i, 0, 0),
                        AndromediaSiteId = data[0].AndromediaSiteId,
                        Collection = new OrderMetricGroup() { Cancelled = 0, OrderCount = 0, Sales = 0 },
                        Combined = new OrderMetricGroup() { Cancelled = 0, Sales = 0, OrderCount = 0 },
                        Delivery = new OrderMetricGroup() { Cancelled = 0, Sales = 0, OrderCount = 0 },
                        InStore = new OrderMetricGroup() { Cancelled = 0, Sales = 0, OrderCount = 0 },
                        Performance = new PerformanceMetricGroup() { AvgDoorTime= 0, AvgMakeTime = 0, AvgOutTheDoor= 0, AvgRackTime = 0, NumOrdersLT30Mins = 0, NumOrdersLT45Mins = 0, Over15lessThan20 = 0 }
                    };
                }
            }

            yield break;
        }
    }
}