using AndroCloudWebApiExternal.Models.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Threading.Tasks;

namespace AndroCloudWebApiExternal.Services
{
    public static class OrderService 
    {
        /// <summary>
        /// Posts the order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="externalSiteId">The external site id.</param>
        /// <param name="orderId">The order id.</param>
        /// <param name="successAction">The success action. Param1: The service address</param>
        /// <param name="errorAction">The error action.</param>
        /// <returns></returns>
        public static async Task<bool> PostOrder(
            Models.Andromeda.AndromedaOrder order,
            string applicationId,
            string externalSiteId,
            string orderId,
            Action<string> successAction,
            Action<Models.Services.ErrorResponse> errorAction) 
        {
            var serviceEndpoitns = await SignpostService.GetServicesAsync(applicationId);

            foreach (var service in serviceEndpoitns) 
            {
                var result = await PostOrderCallAsync(order, service, applicationId, externalSiteId, orderId, successAction, errorAction);

                if (result) { return true; }
            }

            return false;
        }

        private static async Task<bool> PostOrderCallAsync(Models.Andromeda.AndromedaOrder order,
            string signPostDirection,
            string applicationId,
            string externalSiteId,
            string orderId,
            Action<string> successAction,
            Action<Models.Services.ErrorResponse> errorAction) 
        {
            using (HttpClient client = new HttpClient())
            {
                var postOrder = string.Format(Models.Configuration.OrderEndpoint, externalSiteId, orderId, applicationId);

                var url = string.Format("{0}/{1}", signPostDirection, postOrder);

                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                HttpResponseMessage response = await client.PutAsJsonAsync(url, order);

                string resultMessage = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) 
                {
                    var errorResponse = JsonConvert.DeserializeObject<Models.Services.ErrorResponse>(resultMessage);
                    
                    errorAction(errorResponse);
                }

                successAction(url);

                return response.IsSuccessStatusCode;
            }
        }
    }

    public static class SignpostService
    {
        public static async Task<IEnumerable<string>> GetServicesAsync(string applicationId) 
        {
            var model = await GetServiceLocationRequestContentAsync(applicationId);

            if (string.IsNullOrWhiteSpace(model)) 
            {
                throw new ArgumentNullException("model", new Exception("Calling the signpost(s) failed to give any useful answers"));
            }

            var entries = JsonConvert.DeserializeObject<SignPostEntries>(model);
            var v2Entries = entries.Where(e=> e.Version == 2).Select(e=> e.Url).ToArray();

            return v2Entries;
        }

        private static async Task<string> GetServiceLocationRequestContentAsync(string applicationId) 
        {
            IEnumerable<string> locations = Models.Configuration.SignPostUrl.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var location in locations.Select(e=> e.Trim()))
            { 
                using (HttpClient client = new HttpClient()) 
                {
                    var signpost = string.Format(location, applicationId);

                    client.BaseAddress = new Uri(signpost);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    HttpResponseMessage response = await client.GetAsync(signpost);

                    if (response.IsSuccessStatusCode) 
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    //else give up and move on
                }
            }

            return string.Empty;
        }
    }
}