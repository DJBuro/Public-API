using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MyAndromeda.Logging;
using System.Threading.Tasks;

namespace MyAndromeda.Web.Handlers
{
    public class LogHandler : DelegatingHandler
    {
        public LogHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            System.Threading.CancellationToken cancellationToken)
        {
            var logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            var httpContext = DependencyResolver.Current.GetService<HttpContextWrapper>();

            var body =  
                await request.Content.ReadAsStringAsync();

            logger.Debug("Request URI: {0}; UserHostAddress: {1}", 
                request.RequestUri.AbsoluteUri,
                httpContext.Request.UserHostAddress);
            
            //logger.Debug("Endpoint hit: {0}", httpContext.Request.RawUrl);
            logger.Debug("Message body:");


            if (string.IsNullOrWhiteSpace(body))
            {
                logger.Debug("NO CONTENT");
            }
            else 
            {
                if (body.IndexOf("filename", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    logger.Debug("Updating a lovely image of some kind...");
                }
                else
                {
                    
                    logger.Debug(body);
                    
                }
            }
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}