using System.Threading.Tasks;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;
using MyAndromeda.Services.Bringg.Outgoing;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.Services.WebHooks.Models.Settings;
using Newtonsoft.Json;

namespace MyAndromeda.Services.Bringg.Handlers
{
    public class BringgIncomingWebHooksEventHandler : IWebHooksEvent
    {
        private readonly IMyAndromedaLogger logger;

        public BringgIncomingWebHooksEventHandler(IMyAndromedaLogger logger) 
        {
            this.logger = logger;
        }

        public string Name
        {
            get
            {
                return "Bring Web Hooks Event Handler";
            }
        }

        public WebHookType InterestedIn
        {
            get
            {
                return WebHookType.BringgUpdate | WebHookType.BringgEtaUpdate;
            }
        }

        public async Task BeforeDistributionAsync<TModel>(int andromedaStoreId, TModel model)
            where TModel : IHook
        {
            var o = JsonConvert.SerializeObject(model);

            if (model is BringOutgoingEtaWebHook)
            {
                this.logger.Debug("Bring 'Eta' WebHook is ready to send");
                this.logger.Debug(o);
            }
            else if (model is BringOutgoingWebHook) 
            {
                this.logger.Debug("Bring WebHook is ready to send");
                this.logger.Debug(o);
            }
        }

        public async Task NoWebHooksAsync<TModel>(int andromedaSiteId, TModel model)
            where TModel : IHook
        {
        }

        public async Task AfterDistributionAsync<TModel>(int andromedaStoreId, TModel model)
            where TModel : IHook
        {
            //not interested
        }

        public async Task SendingRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            //not interested
        }

        public async Task SentRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            //not interested
        }

        public async Task FailedRequestAsync<TModel>(int andromedaStoreId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook
        {
            //not interested
        }
    }
}