using MyAndromeda.Core;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs
{
    public interface ILocationService : IDependency
    {
        /// <summary>
        /// Loads the locations.
        /// </summary>
        /// <param name="tokenResult">The token result.</param>
        /// <returns></returns>
        Task<Models.Locations> LoadLocationsAsync(int andromedaSiteId, Models.TokenResult tokenResult);
    }
}