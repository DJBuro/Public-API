using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting.Query;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Areas.Reporting.Context;
using MyAndromeda.Web.Areas.ChainReporting.ViewModels;
using MyAndromeda.Framework;
using System.Threading.Tasks;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly ICurrentSite currentSite;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer; 
        private readonly ITranslator translator;

        //private readonly IReportingContext reportingContext;
        private readonly IDailyReportingSalesAggregateServices dailyReportingService;

        public PerformanceController(IDailyReportingSalesAggregateServices dailyReportingService,
            ICurrentSite currentSite,
            INotifier notifier,
            ITranslator translator,
            IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
            this.currentSite = currentSite;
            this.notifier = notifier;
            this.translator = translator;
            this.dailyReportingService = dailyReportingService;
        }

        [HttpPost]
        public async Task<ActionResult> Data(FilterQuery filter)
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicRamesesReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));

                return new HttpUnauthorizedResult();
            }

            var ids = new long[] { this.currentSite.AndromediaSiteId };
            var data = (await dailyReportingService
                .SalesByDayAsync(ids, e => 
                    e.TheDate > filter.FilterFrom && 
                    e.TheDate < filter.FilterTo))
                .ToArray();

            var result = new StoreReportSet() { 
                Data = data,
                AndromediaSiteId = this.currentSite.AndromediaSiteId,
                ClientSiteName = this.currentSite.Site.ExternalName,
                ExternalSiteId = this.currentSite.Site.ExternalSiteId
            };

            return Json(result);
        }
    }
}