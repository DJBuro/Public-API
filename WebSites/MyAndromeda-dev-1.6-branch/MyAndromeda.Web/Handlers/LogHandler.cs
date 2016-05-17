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
            IMyAndromedaLogger logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>();
            HttpContextWrapper httpContext = DependencyResolver.Current.GetService<HttpContextWrapper>();

            string body =  
                await request.Content.ReadAsStringAsync();

            logger.Debug(format: "Request URI: {0}; UserHostAddress: {1}", args: new object[] { request.RequestUri.AbsoluteUri, httpContext.Request.UserHostAddress });
            logger.Debug(message: "Message body:");


            if (string.IsNullOrWhiteSpace(body))
            {
                logger.Debug(message: "NO CONTENT");
            }
            else 
            {
                if (body.IndexOf(value: "filename", comparisonType: StringComparison.InvariantCultureIgnoreCase)>= 0)
                {
                    logger.Debug(message: "Updating a lovely image of some kind...");
                }
                else
                {
                    logger.Debug(body);
                    
                }
            }

            HttpResponseMessage response = null;
            try
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw ;
            }
            
            return response;
        }
    }
}