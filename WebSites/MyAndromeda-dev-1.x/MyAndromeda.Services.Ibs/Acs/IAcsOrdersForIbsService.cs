using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs.Acs
{
    public interface IAcsOrdersForIbsService : ITransientDependency
    {
        Task SaveSentOrderToIbsAsync(OrderHeader orderHeader, Models.AddOrderResult result);
    }
}
