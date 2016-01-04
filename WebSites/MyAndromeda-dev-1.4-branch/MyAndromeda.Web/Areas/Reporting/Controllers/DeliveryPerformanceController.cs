using MyAndromeda.Data.DailyReporting.Services;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Areas.Reporting.ViewModels;
using MyAndromedaDataAccess.Domain.Reporting.Query;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class DeliveryPerformanceController : Controller
    {
        private readonly ICurrentSite currentSite;
        private readonly IDailyReportingSalesAggregateServices service;
        private readonly IAuthorizer authorizer;
        private readonly INotifier notifier;
        private readonly ITranslator translator; 

        public DeliveryPerformanceController(ICurrentSite currentSite, IDailyReportingSalesAggregateServices service, IAuthorizer authorizer, ITranslator translator, INotifier notifier) 
        {
            this.notifier = notifier;
            this.translator = translator;
            this.authorizer = authorizer;
            this.service = service;
            this.currentSite = currentSite;
        }

        public ActionResult Index()
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicAcsReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));

                return new HttpUnauthorizedResult();
            }

            return View();
        }

        public ActionResult Read(FilterQuery query) 
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicAcsReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));

                return new HttpUnauthorizedResult();
            }


            var ids = new long[] { this.currentSite.Site.AndromediaSiteId };
            var showDate = query.FilterFrom.GetValueOrDefault(DateTime.Today.AddDays(-1));

            var data = service.QueryByHour(ids, 
                    (filter) => filter.Thedate.Year == showDate.Year && filter.Thedate.Month == showDate.Month && filter.Thedate.Day == showDate.Day, 
                    (result) => new DeliveryPerformanceViewModel() { 
                        Time = result.Thedate.AddHours(result.Thehour),
                        TotalDeliveryOrders = result.DeliveryCount.GetValueOrDefault(),
                        Under15 = result.LessThen15.GetValueOrDefault(),
                        Between15And20 = result.Over15LessThan20.GetValueOrDefault(),
                        Between20And25 = result.Over20LessThan25.GetValueOrDefault(),
                        Between25And30 = result.Over25LessThan30.GetValueOrDefault(),
                        Between30And35 = result.Over30LessThan35.GetValueOrDefault(),
                        Between35And45 = result.Over35LessThan45.GetValueOrDefault(),
                        Between45And60 = result.Over45LessThan60.GetValueOrDefault(),
                        Over60 = result.Over60.GetValueOrDefault()
                    }).ToArray();

            return Json(new {
                Data = data,
                Total = data.Length
            });
        }

        public ActionResult ReadDaily(FilterQuery query) 
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicAcsReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));

                return new HttpUnauthorizedResult();
            }

            var ids = new long[] { this.currentSite.Site.AndromediaSiteId };

            var showDateTo = query.FilterTo.GetValueOrDefault(DateTime.Today.AddDays(-1));
            var showDateFrom = query.FilterFrom.GetValueOrDefault(showDateTo.AddDays(-30));

            var data = service.QueryByHour(ids,
                    (filter) => filter.Thedate >= showDateFrom && filter.Thedate <= showDateTo,
                    (result) => new DeliveryPerformanceViewModel()
                    {
                        Time = result.Thedate.AddHours(result.Thehour),
                        TotalDeliveryOrders = result.DeliveryCount.GetValueOrDefault(),
                        Under15 = result.LessThen15.GetValueOrDefault(),
                        Between15And20 = result.Over15LessThan20.GetValueOrDefault(),
                        Between20And25 = result.Over20LessThan25.GetValueOrDefault(),
                        Between25And30 = result.Over25LessThan30.GetValueOrDefault(),
                        Between30And35 = result.Over30LessThan35.GetValueOrDefault(),
                        Between35And45 = result.Over35LessThan45.GetValueOrDefault(),
                        Between45And60 = result.Over45LessThan60.GetValueOrDefault(),
                        Over60 = result.Over60.GetValueOrDefault()
                    }).ToArray()
                    .GroupBy(e=> new { e.Time.Year, e.Time.Month, e.Time.Day })
                    .Select(e=> new DeliveryPerformanceViewModel(){
                        Time = new DateTime(e.Key.Year, e.Key.Month, e.Key.Day),
                        TotalDeliveryOrders = e.Sum(hour => hour.TotalDeliveryOrders),
                        Under15 = e.Sum(hour => hour.Under15),
                        Between15And20 = e.Sum(hour => hour.Between15And20),
                        Between20And25 = e.Sum(hour => hour.Between20And25),
                        Between25And30 = e.Sum(hour => hour.Between25And30),
                        Between30And35 = e.Sum(hour => hour.Between30And35),
                        Between35And45 = e.Sum(hour => hour.Between35And45),
                        Between45And60 = e.Sum(hour => hour.Between45And60),
                        Over60 = e.Sum(hour => hour.Over60)
                    }).ToArray();

            //var data = service.QueryByDay(ids, e => e.TheDate >= showDateFrom && e.TheDate <= showDateTo, (result) => new DeliveryPerformanceViewModel()
            //{
            //    Time = result.TheDate.GetValueOrDefault(),
            //    TotalDeliveryOrders = result.DelTotalOrders.GetValueOrDefault(),
            //    Under15 = 0, //result.LessThen15.GetValueOrDefault(),
            //    Between15And20 = 0, //= result.Over15lessThan20.GetValueOrDefault(),
            //    Between25And30 = result.NumOrdersLt30Mins.GetValueOrDefault(),
            //    Between35And45 = result.NumOrdersLt45Mins.GetValueOrDefault(),
            //    Between45And60 = 0,
            //    Over60 = 0
                
            //    //Between20And25 = result.Over20lessThan25.GetValueOrDefault(),
            //    //Between25And30 = result.Over25lessThan30.GetValueOrDefault(),
            //    //Between30And35 = result.Over30lessThan35.GetValueOrDefault(),
            //    //Between35And45 = result.Over35lessThan45.GetValueOrDefault(),
            //    //Between45And60 = result.Over45lessThan60.GetValueOrDefault()
            //}).OrderByDescending(e=> e.Time).ToArray();

            return Json(new { 
                Data = data,
                Total = data.Length
            });
        }
    }
}