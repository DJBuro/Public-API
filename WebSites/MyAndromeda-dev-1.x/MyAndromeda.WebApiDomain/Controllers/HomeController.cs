using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Framework.Logging;

namespace MyAndromeda.WebApiServices.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMyAndromedaLogger logger;

        public HomeController(IMyAndromedaLogger logger)
        { 
            this.logger = logger;
        }

        public ActionResult Index()
        {
            this.logger.Debug("Home");
            return this.View();
        }

        public ActionResult Error() 
        {
            this.logger.Error("Im a tumor");
            this.logger.Fatal("Im a fatal tumor");

            return this.View("Home");
        }
    }
}