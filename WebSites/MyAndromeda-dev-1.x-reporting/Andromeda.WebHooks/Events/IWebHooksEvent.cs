using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.Services.WebHooks.Models.Settings;

namespace MyAndromeda.Services.WebHooks.Events
{
    public interface IWebHooksEvent : IDependency
    {
        string Name { get; }

        WebHookType InterestedIn { get; }

        Task NoWebHooksAsync<TModel>(int andromedaSiteId, TModel model)
            where TModel : IHook;

        Task BeforeDistributionAsync<TModel>(int andromedaSiteId, TModel model)
            where TModel : IHook;

        Task AfterDistributionAsync<TModel>(int andromedaSiteId, TModel model)
            where TModel : IHook;

        Task SendingRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook;

        Task SentRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook;

        Task FailedRequestAsync<TModel>(int andromedaSiteId, WebHookEnrolement enrollment, TModel model)
            where TModel : IHook;
    }
}

