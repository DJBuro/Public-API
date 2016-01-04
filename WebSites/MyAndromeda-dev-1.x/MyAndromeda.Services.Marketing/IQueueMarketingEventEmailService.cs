using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Services.Marketing.Models;
using MyAndromeda.Services.Marketing.WebJobs;
using Newtonsoft.Json;

namespace MyAndromeda.Services.Marketing
{
    public interface IQueueMarketingEventEmailService : ITransientDependency
    {
        Task StartMarketingForAnyStore(string marketingType);
        Task<string> AddToMarketingQueueAsync(int andromedaSiteId, string name);
    }
}
