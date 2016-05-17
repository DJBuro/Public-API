using System.Threading.Tasks;
using MyAndromeda.Core;

namespace MyAndromeda.Services.Ibs
{
    public interface ICustomerService : ITransientDependency
    {
        Task<Models.CustomerResultModel> Get(
            int andromeadaSiteId,
            Models.TokenResult tokenResult,
            Models.CustomerRequestModel requestModel);
        
        Task<Models.CustomerResultModel> Add(
            int andromeadaSiteId,
            Models.TokenResult tokenResult,
            Models.AddCustomerRequestModel addCustomerModel);
    }
}