using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace AndroCloudWebApiExternal.Attributres
{
    public class JustEatAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string AuthorizationHeader = "Authorization";

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //base authorization 
            //base.OnAuthorization(actionContext);

            if (!actionContext.Request.Headers.Any(e => e.Key == AuthorizationHeader)) 
            {
                log.DebugFormat("Missing authorization header: {0}", AuthorizationHeader);

                HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                return;
            }

            if (actionContext.Request.Headers.GetValues(AuthorizationHeader) != null)
            {
                string authenticationTokenPersistant = Models.JustEatConfiguration.AuthorizationTokenValue;

                // get value from header
                string authenticationToken = Convert.ToString(actionContext.Request.Headers.GetValues(AuthorizationHeader).FirstOrDefault());
                authenticationToken = authenticationToken.Trim();
                
                //compare the key in the config file to the key in the header.
                if (!authenticationTokenPersistant.Equals(authenticationToken, StringComparison.InvariantCultureIgnoreCase))
                {
                    log.DebugFormat("Not authorized: {0}", authenticationToken);

                    HttpContext.Current.Response.AddHeader(AuthorizationHeader, authenticationToken);
                    HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                    return;
                }

                HttpContext.Current.Response.AddHeader(AuthorizationHeader, authenticationToken);
                HttpContext.Current.Response.AddHeader("AuthenticationStatus", "Authorized");
                return;
            }

            
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            actionContext.Response.ReasonPhrase = "Please provide valid inputs";
        }
    }
}