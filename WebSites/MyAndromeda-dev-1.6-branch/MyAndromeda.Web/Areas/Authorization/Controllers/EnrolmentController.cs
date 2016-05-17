using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Authorization.Services;
using MyAndromeda.Core.Services;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.Authorization.Controllers
{
    public class EnrolmentController : Controller
    {
        private readonly IEnrolmentService enrolementService;
        private readonly IAuthorizer authorizer;

        public EnrolmentController(IEnrolmentService enrolementService, IAuthorizer authorizer)
        { 
            this.authorizer = authorizer;
            this.enrolementService = enrolementService;
        }

        //public ActionResult Index() 
        //{
        //    var levels = enrolementService.ListEnrolmentLevels();

        //    re
        //}

        public JsonResult Read() 
        {
            var data = enrolementService.ListEnrolmentLevels();

            return Json(data.Select(e => new
            {
                e.Id,
                e.Name
            }), JsonRequestBehavior.AllowGet);
        }
    }
}