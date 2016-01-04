using System;
using System.Linq;
using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;
using MyAndromeda.Logging;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(MyAndromeda.Web.AppStart.SignalrStartup))]
namespace MyAndromeda.Web.AppStart
{
    public class SignalrStartup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.MapSignalR(");
            app.Map("/signalr", map => {
                var hubConfiguration = new HubConfiguration()
                {

                };
                hubConfiguration.EnableDetailedErrors = true;
                hubConfiguration.EnableJavaScriptProxies = true;
                hubConfiguration.EnableJSONP = true;

                try
                {
                    map.RunSignalR(hubConfiguration); 
                }
                catch (Exception e) 
                {
                    var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>(); ;
                    logger.Error(e);
                }
            });
        }
    }
}