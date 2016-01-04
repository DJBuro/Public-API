using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.OrderMonitoring.Models;
using MyAndromeda.Web.Controllers.Api.GprsGateway;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAndromeda.Web.Controllers.Api
{
    public class TestWebApiController : Controller
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IOrderMonitoringWindowsDataService orderMonitoringWindowsService;

        public TestWebApiController(IMyAndromedaLogger logger, IOrderMonitoringWindowsDataService orderMonitoringWindowsService)
        {
            this.orderMonitoringWindowsService = orderMonitoringWindowsService;
            this.logger = logger;
        }

        //
        // GET: /TestWebApi/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MonitoringTest() 
        {
            var ordersWaiting = orderMonitoringWindowsService.GetOrderIds(40, 10, 0);
            var model = new OrderList() { OrderIds = ordersWaiting };
            
            return View(model);
        }

        public ActionResult SuccessFailureEmailTest() 
        {
            return View();
        }

        public ActionResult ChangeInOrderDateTime() 
        {
            return View();
        }

        public ActionResult PublishMenu() 
        {
            return View();
        }

        
	}
}