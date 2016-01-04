using System.Web.Mvc;
using System.Web.Security;

namespace OrderTrackingAdmin.Mvc.Filters
{
    public class RequiresAuthorisation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //redirect if the user is not authenticated
            var isAuthorized = Utilities.Cookie.GetAuthoriationCookie(filterContext.HttpContext.Request);

            if (!isAuthorized)
            {
#if DEBUG
                //var loginUrl = FormsAuthentication.LoginUrl; 
                //filterContext.HttpContext.Response.Redirect(loginUrl, true);

#else
                var loginUrl = FormsAuthentication.LoginUrl;
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
#endif
            }

            return;
        }
    }
    /* TODO: test over feasibility
    public class HandleError : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           

            return;
        }
    }
     */ 
}

