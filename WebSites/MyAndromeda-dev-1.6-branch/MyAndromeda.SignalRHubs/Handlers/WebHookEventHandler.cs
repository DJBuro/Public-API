using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Framework.Contexts;
using Microsoft.AspNet.SignalR;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models.Settings;
using MyAndromeda.Services.WebHooks.Models;

namespace MyAndromeda.SignalRHubs.Handlers
{
    public class WebHookEventHandler : IWebHooksEvent
    {
        private readonly IApplicationSettings applicationSettings;
        private readonly IHubContext hubContext;
        private Task empty = Task.FromResult(false);

        public WebHookEventHandler(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;

            this.hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.StoreHub>();
        }

        public string Name
        {
            get
            {
                return "WebHookEventHandler - Logger";
            }
        }

        public WebHookType InterestedIn
        {
            get
            {
                return WebHookType.All;
            }
        }


        public Task NoWebHooksAsync<TModel>(int andromedaSiteId, TModel model)
            where TModel : IHook
        {
            return empty;
            //not interested
        }

        public Task BeforeDistributionAsync<TModel>(int andromedaStoreId, TModel model)
            where TModel : IHook
        {
            return empty;
            //not interested
        }

        public Task AfterDistributionAsync<TModel>(int andromedaStoreId, TModel model)
            where TModel : IHook
        {
            return empty;
            //not interested
        }

        public Task SendingRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            this.GoingWell(string.Format("Sending request ({0}) - {1}: {2} ", andromedaStoreId, enrollment.Name, enrollment.CallBackUrl));
            return empty;
        }

        public Task SentRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            this.GoingWell(string.Format("Sent request ({0}) - {1}: {2} ", andromedaStoreId, enrollment.Name, enrollment.CallBackUrl));
            return empty;
        }

        public Task FailedRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            this.Error(string.Format("Failed request ({0}) - {1}: {2} ", andromedaStoreId, enrollment.Name, enrollment.CallBackUrl));
            return empty;
        }

        private void GoingWell(string message)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .OnDebug(message);
        }

        private void Error(string message)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .OnError(message);
        }


    }
}
