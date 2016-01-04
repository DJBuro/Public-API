using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Framework.Logging;
using System.Threading;

namespace MyAndromeda.WebApiServices.Handlers
{
    public class LogHandler : DelegatingHandler
    {
        public LogHandler() 
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            var httpContext = DependencyResolver.Current.GetService<HttpContextWrapper>();

            var body = await request.Content.ReadAsStringAsync();

            logger.Debug("UserHostAddress: {0}", httpContext.Request.UserHostAddress);
            logger.Debug("Endpoint hit: {0}", httpContext.Request.RawUrl);
            logger.Debug("Message body:");
            logger.Debug(body);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}