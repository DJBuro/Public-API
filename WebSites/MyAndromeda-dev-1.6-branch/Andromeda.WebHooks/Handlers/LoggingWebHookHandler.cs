using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models.Settings;
using Newtonsoft.Json;
using MyAndromeda.Services.WebHooks.Models;

namespace MyAndromeda.Services.WebHooks.Handlers
{
    public class LoggingWebHookHandler : IWebHooksEvent
    {
        private readonly IMyAndromedaLogger logger;

        public LoggingWebHookHandler(IMyAndromedaLogger logger)
        {
            this.logger = logger;
        }

        public string Name
        {
            get
            {
                return "Logger";
            }
        }

        public WebHookType InterestedIn
        {
            get
            {
                return WebHookType.All;
            }
        }

        public async Task NoWebHooksAsync<TModel>(int andromedaSiteId, TModel model)
            where TModel : IHook
        {
            this.logger.Debug("No web hooks to call");
        }

        public async Task BeforeDistributionAsync<TModel>(int andromedaStoreId, TModel model)
            where TModel : IHook
        {
            this.logger.Debug("About to send some web hooks: " +andromedaStoreId);
        }

        public async Task AfterDistributionAsync<TModel>(int andromedaStoreId, TModel model)
            where TModel : IHook
        {
            this.logger.Debug("Sent some web hooks: " + andromedaStoreId);
        }

        public async Task SendingRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            this.logger.Debug("{0} Sending: {1} - {2}", andromedaStoreId, enrollment.Name, enrollment.CallBackUrl);
            var content = JsonConvert.SerializeObject(model);
            this.logger.Debug(andromedaStoreId + " - " + content);
        }

        public async Task SentRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            this.logger.Debug("{0} Sent: {1} - {2}", andromedaStoreId, enrollment.Name, enrollment.CallBackUrl);
        }

        public async Task FailedRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            this.logger.Debug("Failed sending:" + andromedaStoreId + " " + enrollment.CallBackUrl);
        }
    }
}
