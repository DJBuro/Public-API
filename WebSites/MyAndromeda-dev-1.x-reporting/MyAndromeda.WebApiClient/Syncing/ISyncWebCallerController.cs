using System.Threading.Tasks;
using MyAndromeda.Core;

namespace MyAndromeda.WebApiClient.Syncing
{
    public interface ISyncWebCallerController : ITransientDependency 
    {
        /// <summary>
        /// Requests the menu sync async.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        Task<bool> RequestMenuSyncAsync(int andromedaSiteId);
    }
}