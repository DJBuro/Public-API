using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Dashboard.Dao.Domain;

namespace Dashboard.Web.Mvc.Filters
{
    public class RequiresAuthorisation : ActionFilterAttribute
    {
        //bug: this needs changing.../ implementing/fixing!
        //friggin server...
        public string User { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //redirect if the user is not authenticated
            bool isAuthorized = Dashboard.Web.Mvc.Utilities.Cookie.GetAuthoriationCookie(filterContext.HttpContext.Request);

            if (!isAuthorized)
            {
                //todo: sort out authorization, currently any logged on user can view any page
                //use: GetAuthoriationCookieSiteId and match again site/company
                filterContext.HttpContext.Response.Redirect("/Dashboard/Account/Logon", true);//todo: add an error to login page.
            }

          return;       
        }
    }
}
