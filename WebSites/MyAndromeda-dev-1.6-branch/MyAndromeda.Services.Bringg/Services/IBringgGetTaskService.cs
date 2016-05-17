using MyAndromeda.Core;
using MyAndromeda.Services.Bringg.Models;
using System;
using System.Threading.Tasks;
namespace MyAndromeda.Services.Bringg.Services
{
    public interface IBringgGetTaskService : IDependency
    {
        Task<BringgTaskModel> Get(BringgAuth auth, int taskId);
    }
}
