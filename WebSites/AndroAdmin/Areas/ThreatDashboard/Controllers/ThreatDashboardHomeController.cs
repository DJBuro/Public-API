using AndroAdmin.ThreatBoard.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;

namespace AndroAdmin.Areas.ThreatDashboard.Controllers
{
    [Authorize]
    public class ThreatDashboardHomeController : Controller
    {
        public ThreatDashboardHomeController() { }
        
        
        public ActionResult Index() 
        {
            return View();
        }

        public JsonResult List([DataSourceRequest]DataSourceRequest request) 
        {
            var stores = CreateModels(100);

            return Json(stores.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<StoreViewModel> CreateModels(int count) 
        {
            var r= new Random(5000);
            while (count > 0) 
            {
                yield return new StoreViewModel()
                {
                    Name = "Store",
                    //Online = r.Next(0, 1) == 1,
                    Connections = new Collection<StoreConnectionViewModel>()
                };

                count--;
            }
        }

    }

}
