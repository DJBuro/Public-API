using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Core;

namespace MyAndromeda.WebApiClient.Notifications
{
    public interface INotifyWebCallerController : ITransientDependency
    {
        /// <summary>
        /// Notifies the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<bool> Notify(int andromedaSiteId, NotificationContext context);

        /// <summary>
        /// Successes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<bool> Success(int andromedaSiteId, NotificationContext context);

        /// <summary>
        /// Errors the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<bool> Error(int andromedaSiteId, NotificationContext context);
    }

    public class NotifyWebCallerController : INotifyWebCallerController 
    {
        private readonly IWebApiClientContext context;

        public NotifyWebCallerController(IWebApiClientContext context)
        { 
            this.context = context;
        }

        public async Task<bool> Notify(int andromedaSiteId, NotificationContext model)
        {
            var result = false;
            using (var client = this.CreateClient()) 
            {
                var endpoint = "api/{AndromedaSiteId}/Notify".Replace("{AndromedaSiteId}", andromedaSiteId.ToString());
                var response = await client.PostAsJsonAsync(endpoint, model);

                result = response.IsSuccessStatusCode;
            }

            return result;
        }

        public async Task<bool> Success(int andromedaSiteId, NotificationContext model)
        {
            var result = false;
            using (var client = this.CreateClient())
            {
                var endpoint = "api/{AndromedaSiteId}/Success".Replace("{AndromedaSiteId}", andromedaSiteId.ToString());
                var response = await client.PostAsJsonAsync(endpoint, model);

                result = response.IsSuccessStatusCode;
            }

            return result;
        }

        public async Task<bool> Error(int andromedaSiteId, NotificationContext model)
        {
            var result = false;
            using (var client = this.CreateClient())
            {
                var endpoint = "api/{AndromedaSiteId}/Error".Replace("{AndromedaSiteId}", andromedaSiteId.ToString());
                var response = await client.PostAsJsonAsync(endpoint, model);

                result = response.IsSuccessStatusCode;
            }

            return result;
        }

        private HttpClient CreateClient() 
        {
            var address = new Uri(context.BaseAddress);
            return ClientHelper.GetANewJsonClient(address);
        }



    }
}
