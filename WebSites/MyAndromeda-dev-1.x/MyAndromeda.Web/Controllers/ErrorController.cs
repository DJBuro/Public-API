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
            var error = new MyAndromeda.Web.Models.ErrorModel();
            error.Message = message;

            return View(error);
        }

        public ActionResult Test()
        {
            logger.Debug("Debug Line");
            notifier.Notify("Debug line added");

            logger.Error("Error Line");
            notifier.Notify("Error line added");

            logger.Fatal("Fatal Line");
            notifier.Notify("Fatal line added");

            logger.Info("Info Line");
            notifier.Notify("Info line added");

            System.Diagnostics.Trace.WriteLine("debug lines written");

            return View();
        }

        public ActionResult Test2()
        {
            logger.Error(message: "Going to kick off an error");
            throw new Exception(message: "I kicked myself");

            return View();
        }

    }
}
