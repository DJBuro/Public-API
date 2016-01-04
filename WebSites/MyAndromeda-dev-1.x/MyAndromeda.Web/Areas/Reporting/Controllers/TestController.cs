using MyAndromedaDataAccess.Domain.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Reporting/Another/

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult DashboardChart() 
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult RadialGauge() 
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult SparkLine() 
        {
            return PartialView();
        }

        private static Random r;
        internal static SummaryByDay<decimal> CreateData(int year, int month, int day)
        {
            var a = r ?? (r = new Random());

            return new SummaryByDay<decimal>()
            {
                Day = new DateTime(year, month, day),
                Average = r.Next(0, 1000),
                Max = r.Next(0, 1000),
                Min = r.Next(0, 1000),
                Total = r.Next(5000, 10000)
            };
        }

        internal static IEnumerable<SummaryByDay<decimal>> CreateWeekOfData(int year, int month, int day)
        {
            var a = r ?? (r = new Random());

            var dateTime = new DateTime(year, month, day);

            for (var i = 0; i < 7; i++) 
            {
                yield return
                    new SummaryByDay<decimal>()
                    {
                        Day = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day),
                        Average = r.Next(0, 1000),
                        Max = r.Next(0, 1000),
                        Min = r.Next(0, 1000),
                        Total = r.Next(5000, 10000)
                    };

                dateTime.AddDays(-1);
            }

        }
    }
}
