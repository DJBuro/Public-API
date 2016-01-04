using System;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromedaDataAccess.Domain.Reporting;
using System.Collections.Generic;
using MyAndromeda.Core;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public interface ISalesDataService : IDependency
    {
        IEnumerable<SummaryByDay<decimal>> SalesByDay(Expression<Func<OrderHeader, bool>> query);
    }
}