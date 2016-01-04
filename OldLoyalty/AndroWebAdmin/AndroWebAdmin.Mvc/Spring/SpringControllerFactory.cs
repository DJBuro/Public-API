using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Spring.Context.Support;

namespace AndroWebAdmin.Mvc.Spring
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

            //todo: recog
            //if (controllerName.EndsWith(".asmx"))
            //   return (System.Web.Services.WebService)ctxt.GetObject(controllerName);

            return (System.Web.Mvc.Controller)ctxt.GetObject(controllerName + "Controller");
        }

        //todo: dispose or release?
        public void DisposeController(IController controller)
        {
            var ctxt = WebApplicationContext.GetRootContext();

            var closeables = ctxt.GetObjectsOfType(typeof(ICloseable));

            foreach (DictionaryEntry closeable in closeables)
            {
                ((ICloseable)closeable.Value).Close();
            }

        }

        #region IControllerFactory Members

        public void ReleaseController(IController controller)
        {
            var ctxt = WebApplicationContext.GetRootContext();

            var closeables = ctxt.GetObjectsOfType(typeof(ICloseable));

            foreach (DictionaryEntry closeable in closeables)
            {
                ((ICloseable)closeable.Value).Close();
            }

        }

        #endregion
    }
}
