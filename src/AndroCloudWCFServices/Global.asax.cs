namespace AndroCloudWCFServices
{
    using System;
    using System.ServiceModel.Activation;
    using System.Web;
    using System.Web.Routing;
    using AndroCloudWCFServices.Tools;
    using Microsoft.ApplicationInsights.Extensibility;

    public class Global : HttpApplication
    {
        internal static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start(object sender, EventArgs e)
        {
            // Setup routes
            RegisterRoutes();

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new CloudRoleNameInitializer("ACS-Public-API"));


            // Inject the data access factory
            AndroCloudDataAccess.DataAccessHelper.DataAccessFactory = new AndroCloudDataAccessEntityFramework.EntityFrameworkDataAccessFactory();
            DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory = new DataWarehouseDataAccessEntityFramework.EntityFrameworkDataAccessFactory();
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

        private static void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("weborderapi", new WebServiceHostFactory(), typeof(RESTServices)));
            RouteTable.Routes.Add(new ServiceRoute("weborderapiv2", new WebServiceHostFactory(), typeof(RestServicesV2)));
        }
    }
}