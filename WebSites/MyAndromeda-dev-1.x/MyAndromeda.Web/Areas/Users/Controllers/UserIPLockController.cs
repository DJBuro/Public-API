using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using MyAndromeda.Authorization.Management;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Web.Areas.Users.ViewModels;
using Kendo.Mvc.UI;

namespace MyAndromeda.Web.Areas.Users.Controllers
{
    public class UserIPLockController : Controller
    {
        private readonly INotifier notifier;
        private readonly ICurrentRequest currentRequest;
        private readonly IIpRangeBlockingService ipRangeService;

        public UserIPLockController(INotifier notifier, IIpRangeBlockingService ipRangeService, ICurrentRequest currentRequest) 
        {
            this.currentRequest = currentRequest;
            this.ipRangeService = ipRangeService;
            this.notifier = notifier;
        }


        public ActionResult List([DataSourceRequest] DataSourceRequest request, int userId) 
        {
            var addresses = ipRangeService.ListValidAddresses(userId).Select(e=> new UserIpViewModel(){  IpAddress = e }).ToArray();

            return Json(addresses.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Create([DataSourceRequest] DataSourceRequest request, int userId, UserIpViewModel model) 
        {
            var addAddress = model.IpAddress; // currentRequest.Request.UserHostAddress;
            var currentAdddresses = ipRangeService.ListValidAddresses(userId);

            if(currentAdddresses.Any(e=> e.Equals(addAddress)))
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            var newIpRangeList = currentAdddresses.Union(new []{ addAddress }).OrderBy(e=> e);

            ipRangeService.AddIPRestrictions(userId, newIpRangeList);

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, int userId, UserIpViewModel model) 
        {
            if (model != null) 
            {
                this.ipRangeService.RemoveIpRestriction(userId, model.IpAddress);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}