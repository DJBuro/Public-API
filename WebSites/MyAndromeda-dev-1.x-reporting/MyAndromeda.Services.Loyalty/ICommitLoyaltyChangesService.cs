using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Services.Loyalty
{
    public interface ICommitLoyaltyChangesService : IDependency
    {
        /// <summary>
        /// Declines the loyalty points async.
        /// </summary>
        /// <param name="orderHeader">The order header.</param>
        /// <returns></returns>
        Task DeclineLoyaltyPointsAsync(OrderHeader orderHeader);

        /// <summary>
        /// Commits the loyalty points async.
        /// </summary>
        /// <param name="orderHeader">The order header.</param>
        /// <returns></returns>
        Task CommitLoyaltyPointsAsync(OrderHeader orderHeader);
    }
}