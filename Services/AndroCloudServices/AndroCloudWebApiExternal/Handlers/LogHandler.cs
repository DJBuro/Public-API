using System.Net;
using AndroCloudWebApiExternal.Contstraints;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using System.Web;

namespace AndroCloudWebApiExternal.Handlers
{
    public class LogHandler : DelegatingHandler
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LogHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            await LogRequestHeadersAndBody(request);

            response = await base.SendAsync(request, cancellationToken);

            await LogResponseBody(response);

            return response;
        }
 
        private async static Task LogRequestHeadersAndBody(HttpRequestMessage request)
        {
            //currently impossible to know if the content is correct. So lets proof it. 
            var context = new HttpContextWrapper(HttpContext.Current);
            log.DebugFormat("Request URI: {0}", request.RequestUri.AbsoluteUri);
            log.DebugFormat("UserHostAddress: {0}", context.Request.UserHostAddress);
            log.DebugFormat("Endpoint hit: {0}", context.Request.RawUrl);

            string headersToLog = String.Join("\r\n", request.Headers.Select(h => h.Key + ": " + String.Join(",", h.Value)));
            string body = await request.Content.ReadAsStringAsync();

            log.Debug(headersToLog);
            log.Debug("==Request====================================================");
            log.Debug(body);
            log.Debug("==End of Request=============================================");
        }

        private async static Task LogResponseBody(HttpResponseMessage response)
        {
            //dont care much about it working correctly. 
            if (response.IsSuccessStatusCode) { return; }

            string body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body)) 
            {
                body = "No content";
            }

            log.Error("Something went wrong");
            log.Error("==Response===================================================");
            log.Error(body);
            log.Error("==End of Response============================================");
        }
    }
}