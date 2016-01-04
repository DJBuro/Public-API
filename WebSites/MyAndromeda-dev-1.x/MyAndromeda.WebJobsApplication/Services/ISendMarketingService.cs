using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Services.Marketing.Models;

namespace MyAndromeda.WebJobs.EventMarketing.Services
{
    public interface ISendMarketingService : ITransientDependency 
    {
        Task<MarketingRecipientListMessage> SendAsyc(MarketingStoreEventQueueMessage message,
            bool addMatt = true,
            bool sendTheEmail = true);
    }
}
