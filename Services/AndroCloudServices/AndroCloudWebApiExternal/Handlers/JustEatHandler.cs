using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AndroCloudWebApiExternal.Contstraints;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;

namespace AndroCloudWebApiExternal.Handlers
{
    public class JustEatHandler : DelegatingHandler 
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            var body = await request.Content.ReadAsStringAsync();

            ValidateJustEatJson(body, request, response);

            if (response != null)
            {
                var task = new TaskCompletionSource<HttpResponseMessage>();
                task.SetResult(response);

                return await task.Task;
            }

            response = await base.SendAsync(request, cancellationToken);

            return response;
        }

        public static void ValidateJustEatJson(string body, HttpRequestMessage request, HttpResponseMessage response)
        {
            //todo ... log something, somewhere useful.
            try
            {
                var obj = JToken.Parse(body);
            }
            catch (JsonReaderException jex)
            {
                log.Debug("Could not parse the just eat order");
                log.Error(jex);
                //Exception in parsing json
                Console.WriteLine(jex.Message);

                HttpError myCustomError = new HttpError("The content could not be parsed")
                {
                    { "Timestamp", DateTime.UtcNow.ToStringAsUtc() },
                    { "Message", "The content could not be parsed" },
                    { "Details", jex.Message }
                };
                response = request.CreateErrorResponse(HttpStatusCode.InternalServerError, myCustomError);
            }
            catch (Exception ex) //some other exception
            {
                log.Debug("Could not parse the just eat order");
                log.Error(ex);

                HttpError myCustomError = new HttpError("The content could not be parsed")
                {
                    { "Timestamp", DateTime.UtcNow.ToStringAsUtc() },
                    { "Message", "The content could not be parsed" },
                    { "Details", ex.Message }
                };
                response = request.CreateErrorResponse(HttpStatusCode.InternalServerError, myCustomError);
            }
        }
    }
}