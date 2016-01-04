using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(MyAndromeda.Web.AppStart.SignalrStartup))]
namespace MyAndromeda.Web.AppStart
{
    public class SignalrStartup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.MapSignalR(");
            app.Map("/signalr", map =>
            {
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
                }
            });
        }
    }
}