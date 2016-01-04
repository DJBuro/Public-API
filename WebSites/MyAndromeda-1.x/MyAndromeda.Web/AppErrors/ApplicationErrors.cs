using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;

namespace MyAndromeda.Web.AppErrors
{
    //[assembly: WebActivator.PreApplicationStartMethod(typeof(MyAndromeda.Web.AppErrors.ApplicationErrors), "Start")]
    //public static class ApplicationErrors
    //{
    //    public static void Start()
    //    {
            
    //    }
    //}

    public class WatchHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            IMyAndromedaLogger logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            INotifier notifier = DependencyResolver.Current.GetService<INotifier>();

            var exception = filterContext.Exception;
            var handled = filterContext.ExceptionHandled;

            notifier.Exception(exception);
            logger.Error(filterContext.Exception.Message);
            if (!handled)
            {
                logger.Error("Error not handled");
                logger.Error(filterContext.Exception.Message);
            }
        }
    }
}