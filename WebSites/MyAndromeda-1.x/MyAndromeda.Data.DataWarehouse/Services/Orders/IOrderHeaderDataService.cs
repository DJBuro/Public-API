using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public interface IOrderHeaderDataService : IDependency
    {
        OrderHeader GetByOrderId(Guid acsOrderId);

    }
}
