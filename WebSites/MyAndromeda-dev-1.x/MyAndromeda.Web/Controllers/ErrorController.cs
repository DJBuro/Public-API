using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;

namespace MyAndromeda.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IMyAndromedaLogger logger;
        private readonly INotifier notifier;

        public ErrorController(IMyAndromedaLogger logger, INotifier notifier)
        {
            this.notifier = notifier;
            this.logger = logger;
        }

        public ActionResult Index(string message)
        {
            var error = new Models.ErrorModel();
            error.Message = message;

            return View(error);
        }

        public ActionResult Test()
        {
            logger.Debug(message: "Debug Line");
            notifier.Notify(message: "Debug line added");

            logger.Error(message: "Error Line");
            notifier.Notify(message: "Error line added");

            logger.Fatal(message: "Fatal Line");
            notifier.Notify(message: "Fatal line added");

            logger.Info(message: "Info Line");
            notifier.Notify(message: "Info line added");

            System.Diagnostics.Trace.WriteLine(message: "debug lines written");

            return View();
        }

        public ActionResult Test2()
        {
            logger.Error(message: "Going to kick off an error");

            throw new Exception(message: "I kicked myself");

            //return View();
        }

    }
}
