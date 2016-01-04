using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.SendGridService.MarketingApi.Models.Schedule;

namespace MyAndromeda.SendGridService.MarketingApi
{
    public interface IScheduleService : IDependency
    {
        /// <summary>
        /// Gets async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<ScheduleGetResponseModel> GetAsync(ScheduleGetRequestModel model);

        /// <summary>
        /// Adds async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> AddAsync(ScheduleAddRequestModel model);

        /// <summary>
        /// Deletes async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(ScheduleDeleteRequestModel model);
    }
}