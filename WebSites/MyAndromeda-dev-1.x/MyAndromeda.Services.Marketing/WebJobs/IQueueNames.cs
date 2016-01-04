using MyAndromeda.Core;
using System;
using System.Configuration;

namespace MyAndromeda.Services.Marketing.WebJobs
{
    public interface IQueueNames : ISingletonDependency
    {
        string MarketingEventQueue { get; }
        string MarketingEventStoreQueue { get; }
        string MarketingEventAuditHistoryQueue { get; }

        string GetName(string name);
    }
}