using System.Web.Mvc;
using AndroAdmin.Mvc;

namespace AndroAdmin.Controllers
{
    public class AuthenticationController : SiteController
    {

        public ActionResult Index()
        {
            return View();

            /*
             * userId/emailAddress 
             * Send a request to each domain to create a cookie? (AuthCookie?)
             
             * internal:
             * logon data/time (update +20min)
             * check caller program, is valid/enabled project
             * check user is active
             * 
             
             * RETURN:
             * true/false
             * False/Reason (can be done in AndroAdmin on Return URL)
             * 
             */

        }

    }
}
