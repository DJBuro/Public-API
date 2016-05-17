using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.Web.Controllers
{
    public class GlossaryController : Controller
    {
        //
        // GET: /Glossary/

        public PartialViewResult Index()
        {
            return PartialView();
        }

    }
}
