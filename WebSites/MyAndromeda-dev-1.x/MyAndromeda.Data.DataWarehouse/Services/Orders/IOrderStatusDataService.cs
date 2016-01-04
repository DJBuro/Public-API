using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services
{
    public interface IOrderStatusDataService : IDataProvider<OrderStatu>, IDependency
    {

        /// <summary>
        /// Adds the history.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="oldStatus">The old status.</param>
        Task AddHistoryAsync(OrderHeader order, OrderStatu oldStatus);
    }
}
