using System.Collections;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Spring.Context.Support;
using WebDashboard.Mvc.Helpers;

namespace WebDashboard.Mvc.Spring
{
    public class SpringControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext context, string controllerName)
        {
            var ctxt = WebApplicationContext.GetRootContext();

            //todo: why is this coming along?
            if (controllerName == "favicon.ico")
                return null;

            if (controllerName == "Content")
                return null;

            return (Controller)ctxt.GetObject(controllerName + "Controller");
        }

        //todo: dispose or release?
        public void DisposeController(IController controller)
        {
            var ctxt = WebApplicationContext.GetRootContext();

            var closeables = ctxt.GetObjectsOfType(typeof(ICloseable));

            foreach (var closeable in closeables)
            {
                ((ICloseable)closeable.Value).Close();
            }

        }

        #region IControllerFactory Members

        public void ReleaseController(IController controller)
        {
            var ctxt = WebApplicationContext.GetRootContext();

            var closeables = ctxt.GetObjectsOfType(typeof(ICloseable));

            foreach (var closeable in closeables)
            {
                ((ICloseable)closeable.Value).Close();
            }

        }

        #endregion

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }
    }
}