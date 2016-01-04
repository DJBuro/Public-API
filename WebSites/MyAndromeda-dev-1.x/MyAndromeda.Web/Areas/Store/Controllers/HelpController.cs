using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Store.Controllers
{
    public class HelpController : Controller
    {
        [ChildActionOnly]
        public PartialViewResult Address() 
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult Employee() 
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult OpeningHours() 
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult StoreDetail() 
        {
            return PartialView();
        }
    }
}
