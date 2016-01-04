using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Acs.Controllers
{
    public class WebHooksController : Controller
    {
        // GET: Acs/WebHooks
        public ActionResult Index(int acsApplicationId)
        {
            var model = new Models.AcsApplicationViewModel() { Id = acsApplicationId }; 
            
            return View(model);
        }

        /* Read and update are in WebHookWebApiController */
    }
}