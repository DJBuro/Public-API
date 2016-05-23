using MyAndromeda.Core;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.Services.WebHooks.Models.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MyAndromeda.Services.WebHooks
{
    public interface ISendWebHooksService : IDependency
    {
        Task CallEndpoints<TModel>(TModel model, Func<WebhookSettings, List<WebHookEnrolement>> fetchEnrollments) where TModel : IHook;
    }
}
