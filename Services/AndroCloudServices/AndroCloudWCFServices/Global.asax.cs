using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.ServiceModel.Activation;
using AndroCloudDataAccessEntityFramework;
using AndroCloudDataAccess;
using DataWarehouseDataAccess;
using DataWarehouseDataAccessEntityFramework;

namespace AndroCloudWCFServices
{
    public class Global : System.Web.HttpApplication
    {
        internal static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start(object sender, EventArgs e)
        {
            // Setup routes
            this.RegisterRoutes();

            // Inject the data access factory
            AndroCloudDataAccess.DataAccessHelper.DataAccessFactory = new AndroCloudDataAccessEntityFramework.EntityFrameworkDataAccessFactory();
            DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory = new DataWarehouseDataAccessEntityFramework.EntityFrameworkDataAccessFactory();
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("weborderapi", new WebServiceHostFactory(), typeof(RESTServices)));
            RouteTable.Routes.Add(new ServiceRoute("weborderapiv2", new WebServiceHostFactory(), typeof(RESTServicesV2_Host)));
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}