using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using MyAndromeda.Core;

namespace MyAndromeda.Services.Marketing.WebJobs
{
    public interface IQueueManager : ITransientDependency
    {
        /// <summary>
        /// Gets the marketing event queue.
        /// </summary>
        /// <returns></returns>
        Task<CloudQueue> GetMarketingEventQueue();

        /// <summary>
        /// Gets the marketing event queue to start marketing for a store.
        /// Queue will be different per environment 
        /// </summary>
        /// <returns></returns>
        Task<CloudQueue> GetMarketingEventStoreQueueAsync();

        /// <summary>
        /// Gets the marketing event audit history queue async.
        /// </summary>
        /// <returns></returns>
        Task<CloudQueue> GetMarketingEventAuditHistoryQueueAsync();
    }
}
