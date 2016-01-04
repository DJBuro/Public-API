using System.Web.Mvc;
using System.Web.Security;


namespace WebDashboard.Mvc.Filters
{
    public class RequiresAuthorisation : ActionFilterAttribute
    {
        //bug: this needs changing.../ implementing/fixing!
        //friggin server...
        //public string User { get; set; }

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

}

