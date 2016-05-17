//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace MyAndromeda.Web.Filters
//{
//    public class ProfilingActionFilter : ActionFilterAttribute
//    {
//        IDisposable prof;

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            var mp = MiniProfiler.Current;
//            if (mp != null)
//            {
//                prof = MiniProfiler.Current.Step("C: " + filterContext.Controller.ToString() + "." + filterContext.ActionDescriptor.ActionName);
//            }
//            base.OnActionExecuting(filterContext);
//        }

//        public override void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            base.OnActionExecuted(filterContext);
//            if (prof != null)
//            {
//                prof.Dispose();
//            }
//        }
//    }
//}