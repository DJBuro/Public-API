using MyAndromeda.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Services.Marketing.Models;

namespace MyAndromeda.Services.Marketing
{
    public interface IMarketingHistoryTrailService : ITransientDependency
    {
        Task SentEmailsAsync(MarketingRecipientListMessage message);
    }
}
